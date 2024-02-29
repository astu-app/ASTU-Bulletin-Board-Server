using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Domain.Models.Announcements.Exceptions;
using BulletInBoardServer.Domain.Models.Attachments;
using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using BulletInBoardServer.Services.Services.Announcements.DelayedOperations;
using BulletInBoardServer.Services.Services.Announcements.Exceptions;
using BulletInBoardServer.Services.Services.Announcements.Models;
using BulletInBoardServer.Services.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BulletInBoardServer.Services.Services.Announcements.ServiceCore;

public class GeneralOperationsService(
    IServiceScopeFactory scopeFactory,
    IDelayedAnnouncementOperationsDispatcher dispatcher)
    : DispatcherDependentAnnouncementServiceBase(scopeFactory, dispatcher)
{
    /// <summary>
    /// Создание объявления
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию</param>
    /// <param name="create">Объект с необходимыми для создания данными</param>
    /// <returns>Созданное объявление</returns>
    /// <exception cref="AnnouncementContentNullOrEmptyException">Текстовое содержимое объявления null, пустой или состоит только из пробельных символов</exception>
    /// <exception cref="AnnouncementAudienceNullOrEmptyException">Аудитория объявления null или пуста</exception>
    /// <exception cref="InvalidOperationException">Момент отложенной публикации или сокрытия не были перенесены в создаваемое объявление</exception>
    public Announcement Create(Guid requesterId, CreateAnnouncement create)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);

        var creator = new AnnouncementCreator(authorId: requesterId, create, dbContext);
        var announcement = creator.Create();
        
        if (!announcement.ExpectsDelayedPublishing)
            PublishManually(requesterId, announcement.Id, DateTime.Now);

        if (announcement.ExpectsDelayedPublishing)
            Dispatcher.ConfigureDelayedPublishing(
                announcement.Id,
                announcement.DelayedPublishingAt ??
                throw new InvalidOperationException(
                    $"{nameof(announcement.DelayedPublishingAt)} не может быть null"));

        if (announcement.ExpectsDelayedHiding)
            Dispatcher.ConfigureDelayedHiding(
                announcement.Id,
                announcement.DelayedHidingAt ??
                throw new InvalidOperationException($"{nameof(announcement.DelayedHidingAt)} не может быть null"));

        return announcement;
    }

    /// <summary>
    /// Получение деталей объявления
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию</param>
    /// <param name="announcementId">Id объявления</param>
    /// <returns>Объявление со связанными сущностями</returns>
    /// <exception cref="OperationNotAllowedException">Пользователь не имеет права  выполнения операции</exception>
    public Announcement GetDetails(Guid requesterId, Guid announcementId)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        var announcement = GetAnnouncementSummary(announcementId, dbContext);
        if (announcement.AuthorId != requesterId)
            throw new OperationNotAllowedException("Получить детали объявления может только его автор");

        // В данном случае можно использовать явную загрузку содержимого коллекций методом Load, но в таком случае
        // для каждой коллекции создается отдельный запрос к базе данных, что создает излишнюю нагрузку на СУБД
        // и на сеть. Для уменьшения нагрузки можно загрузить содержимое коллекций отдельным запросом. В таком 
        // случае будет выполнена повторная загрузка уже загруженных данных, но учитывая небольшой объем этих
        // данных, временные затраты на повторную его загрузку минимальны. 
        announcement = dbContext.Announcements
            .Where(a => a.Id == announcementId)
            .Include(a => a.Author)
            .Include(a => a.Audience)
            .Include(a => a.Categories)
            .Include(a => a.Attachments)
            .Single();
        // Так как к объявлению могут быть прикреплены разные типы вложений и присутствует необходимость загрузить
        // связанные с этими вложениями сущности, для каждого из типов вложений используется явная загрузка.
        dbContext.Entry(announcement).Collection(a => a.Attachments)
            .Query()
            .Where(a => a.Type == AttachmentTypes.Survey)
            .Cast<Survey>()
            .Include(s => s.Voters)
            .Include(s => s.Questions)
            .ThenInclude(q => q.Answers)
            .ThenInclude(q => q.Participation)
            .Load();

        announcement.ViewsCount = dbContext.AnnouncementAudience
            .AsQueryable()
            .Count(aa => aa.AnnouncementId == announcementId && aa.Viewed);

        return announcement;
    }

    /// <summary>
    /// Редактирование объявления
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию</param>
    /// <param name="edit">Объект с данными, необходимыми для редактирования объявления</param>
    /// <exception cref="OperationNotAllowedException">Пользователь не имеет права выполнить операцию</exception>
    /// <exception cref="AnnouncementContentEmptyException">Нельзя установить текстовое содержимое, которое является null, пустым или состоит только из пробельных символов</exception>
    /// <exception cref="AnnouncementAudienceEmptyException">Нельзя установить пустую аудиторию объявления</exception>
    /// <exception cref="CannotDetachSurveyException">От объявления нельзя открепить опрос</exception>
    public void Edit(Guid requesterId, EditAnnouncement edit)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        var announcement = GetAnnouncementSummary(edit.Id, dbContext);
        if (requesterId != announcement.AuthorId)
            throw new OperationNotAllowedException("Редактировать объявление может только его автор");

        var redactor = new AnnouncementRedactor(announcement, edit, dbContext);
        redactor.Edit();
        
        if (edit.DelayedPublishingAtChanged)
            announcement.SetDelayedPublishingMoment(DateTime.Now, edit.DelayedPublishingAt);
        if (edit.DelayedHidingAtChanged)
            announcement.SetDelayedHidingMoment(DateTime.Now, edit.DelayedHidingAt);

        dbContext.SaveChanges();

        if (edit is { DelayedPublishingAtChanged: true, DelayedPublishingAt: not null })
            Dispatcher.ReconfigureDelayedPublishing(edit.Id, edit.DelayedPublishingAt.Value);
        else if (edit is { DelayedPublishingAtChanged: true, DelayedPublishingAt: null })
            Dispatcher.DisableDelayedPublishing(edit.Id);

        if (edit is { DelayedHidingAtChanged: true, DelayedHidingAt: not null })
            Dispatcher.ReconfigureDelayedHiding(edit.Id, edit.DelayedHidingAt.Value);
        else if (edit is { DelayedHidingAtChanged: true, DelayedHidingAt: null })
            Dispatcher.DisableDelayedHiding(edit.Id);
        
        // todo отправить уведомление об изменении объявления клиентам
    }

    /// <summary>
    /// Удаление объявления
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию </param>
    /// <param name="announcementId">Id объявления</param>
    /// <exception cref="OperationNotAllowedException">Пользователь не имеет права  выполнения операции</exception>
    public void Delete(Guid requesterId, Guid announcementId)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        var announcement = GetAnnouncementSummary(announcementId, dbContext);
        if (requesterId != announcement.AuthorId)
            throw new OperationNotAllowedException("Удалить объявление может только его автор");

        if (announcement.ExpectsDelayedPublishing)
            Dispatcher.DisableDelayedPublishing(announcementId);
        if (announcement.ExpectsDelayedHiding)
            Dispatcher.DisableDelayedHiding(announcementId);

        dbContext.Announcements
            .Where(a => a.Id == announcementId)
            .ExecuteDelete();

        // todo отправить уведомление об удалении объявления пользователям 
    }

    /// <summary>
    /// Метод публикует заданное объявление указанным пользователем
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию</param>
    /// <param name="announcementId">Id заданного объявления</param>
    /// <param name="publishedAt">Время публикации объявления</param>
    /// <exception cref="OperationNotAllowedException">Пользователь не имеет права  выполнения операции</exception>
    public void PublishManually(Guid requesterId, Guid announcementId, DateTime publishedAt)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        var announcement = GetAnnouncementSummary(announcementId, dbContext);
        if (announcement.AuthorId != requesterId)
            throw new OperationNotAllowedException("Опубликовать объявление может только его автор");

        if (announcement.ExpectsDelayedPublishing)
            Dispatcher.DisableDelayedPublishing(announcementId);

        announcement.Publish(DateTime.Now, publishedAt);
        dbContext.SaveChanges();

        // todo уведомление о публикации
    }
}