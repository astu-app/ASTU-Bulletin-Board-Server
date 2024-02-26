using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Domain.Models.Announcements.Exceptions;
using BulletInBoardServer.Domain.Models.Attachments;
using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using BulletInBoardServer.Domain.Models.JoinEntities;
using BulletInBoardServer.Services.Services.AnnouncementCategories.Exceptions;
using BulletInBoardServer.Services.Services.Announcements.DelayedOperations;
using BulletInBoardServer.Services.Services.Announcements.Exceptions;
using BulletInBoardServer.Services.Services.Announcements.Models;
using BulletInBoardServer.Services.Services.Attachments.Exceptions;
using BulletInBoardServer.Services.Services.Audience.Exceptions;
using BulletInBoardServer.Services.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AnnouncementAudience = BulletInBoardServer.Domain.Models.JoinEntities.AnnouncementAudience;

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
        
        ContentValidOrThrow(create);
        AudienceValidOrThrow(create);
        DelayedMomentsCorrectOrThrow(create);

        var announcement = InitAnnouncement(create, authorId: requesterId);
        dbContext.Announcements.Add(announcement);

        AddRelatedEntitiesToDb(create, announcement);
        dbContext.SaveChanges();

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
    public void Edit(Guid requesterId, EditAnnouncement edit)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        var announcement = GetAnnouncementSummary(edit.Id, dbContext);
        if (requesterId != announcement.AuthorId)
            throw new OperationNotAllowedException("Редактировать объявление может только его автор");

        if (edit.Content is not null)
        {
            NewContentValidOrThrow(edit);
            announcement.SetContent(edit.Content);
        }
        if (edit.AudienceIds is not null)
        {
            NewAudienceValidOrThrow(edit);
            ApplyAudienceChanging(announcement.Id, edit.AudienceIds);
        }
        if (edit.CategoryIds is not null)
            ApplyCategoriesChanging(announcement.Id, edit.CategoryIds);
        if (edit.AttachmentIds is not null)
            ApplyAttachmentsChanging(announcement.Id, edit.AttachmentIds);
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



    private static void ContentValidOrThrow(CreateAnnouncement create)
    {
        if (string.IsNullOrWhiteSpace(create.Content))
            throw new AnnouncementContentNullOrEmptyException();
    }
    
    private static void AudienceValidOrThrow(CreateAnnouncement create)
    {
        if (create.UserIds is null || !create.UserIds.Any())
            throw new AnnouncementAudienceNullOrEmptyException();
    }
    
    private static void DelayedMomentsCorrectOrThrow(CreateAnnouncement create)
    {
        var now = DateTime.Now;
        var publishAt = create.DelayedPublishingAt;
        var hideAt = create.DelayedHidingAt;

        if (publishAt is null && hideAt is null)
            return;

        if (publishAt is not null && hideAt is null)
        {
            DelayedPublishingMomentComesInFutureOrThrow(now, publishAt.Value);
            return;
        }

        if (publishAt is null && hideAt is not null)
        {
            DelayedHidingMomentComesInFutureOrThrow(now, hideAt.Value);
            return;
        }

        DelayedPublishingMomentComesInFutureOrThrow(now, publishAt!.Value);
        DelayedHidingMomentComesInFutureOrThrow(now, hideAt!.Value);
        MomentWillComeBeforeOrThrow<DelayedPublishingAfterDelayedHidingException>(publishAt.Value, hideAt.Value);
    }
    
    /// <summary>
    /// Метод проверяет, что момент отложенной публикации наступит позже текущего, или кидает исключение
    /// </summary>
    /// <param name="now">Текущий момент времени</param>
    /// <param name="moment">Проверяемый момент отложенной публикации</param>
    /// <exception cref="DelayedPublishingMomentComesInPastException">Генерируемое исключение</exception>
    private static void DelayedPublishingMomentComesInFutureOrThrow(DateTime now, DateTime moment) => 
        MomentWillComeBeforeOrThrow<DelayedPublishingMomentComesInPastException>(now, moment);

    /// <summary>
    /// Метод проверяет, что момент отложенного сокрытия наступит позже текущего, или кидает исключение
    /// </summary>
    /// <param name="now">Текущий момент времени</param>
    /// <param name="moment">Проверяемый момент отложенного сокрытия</param>
    /// <exception cref="DelayedHidingMomentComesInPastException">Генерируемое исключение</exception>
    private static void DelayedHidingMomentComesInFutureOrThrow(DateTime now, DateTime moment) => 
        MomentWillComeBeforeOrThrow<DelayedHidingMomentComesInPastException>(now, moment);

    /// <summary>
    /// Метод проверяет, что первый момент наступит до второго, и генерирует исключение в противном случае
    /// </summary>
    /// <param name="first">Первый момент</param>
    /// <param name="second">Второй момент</param>
    /// <typeparam name="TException">Тип генерируемого исключения</typeparam>
    private static void MomentWillComeBeforeOrThrow<TException>(DateTime first, DateTime second) 
        where TException : InvalidOperationException
    {
        if (first >= second)
            throw Activator.CreateInstance<TException>();
    }


    private static Announcement InitAnnouncement(CreateAnnouncement createAnnouncement, Guid authorId) =>
        new(
            id: Guid.NewGuid(),
            content: createAnnouncement.Content,
            authorId: authorId,
            publishedAt: null,
            hiddenAt: null,
            delayedPublishingAt: createAnnouncement.DelayedPublishingAt,
            delayedHidingAt: createAnnouncement.DelayedHidingAt
        );

    private void AddRelatedEntitiesToDb(CreateAnnouncement create, Announcement announcement)
    {
        var audience = InitUserAudience(announcement.Id, create.UserIds);
        TryAddAnnouncementAudienceOrThrow(audience);

        var attachmentJoins = InitAttachmentJoins(announcement.Id, create.AttachmentIds);
        TryAddAttachmentsOrThrow(attachmentJoins);

        var categoryJoins = InitCategoryJoins(announcement.Id, create.CategoryIds);
        TryAddAnnouncementCategoriesOrThrow(categoryJoins);
    }

    private void TryAddAttachmentsOrThrow(IEnumerable<AnnouncementAttachment> attachment)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        try
        {
            dbContext.AnnouncementAttachmentJoins.AddRange(attachment);
        }
        catch (InvalidOperationException err)
        {
            throw new AttachmentDoesNotExist(err);
        }
    }
    
    private void TryAddAnnouncementAudienceOrThrow(IEnumerable<AnnouncementAudience> audience)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        try
        {
            dbContext.AnnouncementAudience.AddRange(audience);
        }
        catch (InvalidOperationException err)
        {
            throw new PieceOfAudienceDoesNotExist(err);
        }
    }
    
    private void TryAddAnnouncementCategoriesOrThrow(IEnumerable<AnnouncementAnnouncementCategory> audience)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        try
        {
            dbContext.AnnouncementCategoryJoins.AddRange(audience);
        }
        catch (InvalidOperationException err)
        {
            throw new AnnouncementCategoriesDoNotExist(err);
        }
    }

    private static IEnumerable<AnnouncementAudience> InitUserAudience(Guid announcementId,
        IEnumerable<Guid> userIds)
    {
        var joins = new List<AnnouncementAudience>();
        foreach (var userId in userIds)
            joins.Add(new AnnouncementAudience(announcementId, userId));

        return joins;
    }

    private static IEnumerable<AnnouncementAttachment> InitAttachmentJoins(Guid announcementId,
        IEnumerable<Guid> attachmentIds)
    {
        var joins = new List<AnnouncementAttachment>();
        foreach (var attachmentId in attachmentIds)
            joins.Add(new AnnouncementAttachment(announcementId, attachmentId));

        return joins;
    }

    private static IEnumerable<AnnouncementAnnouncementCategory> InitCategoryJoins(Guid announcementId,
        IEnumerable<Guid> categoryIds)
    {
        var joins = new List<AnnouncementAnnouncementCategory>();
        foreach (var categoryId in categoryIds)
            joins.Add(new AnnouncementAnnouncementCategory(announcementId, categoryId));

        return joins;
    }

    private static void NewContentValidOrThrow(EditAnnouncement edit)
    {
        if (!string.IsNullOrWhiteSpace(edit.Content))
            throw new AnnouncementContentEmptyException();
    }
    
    private static void NewAudienceValidOrThrow(EditAnnouncement edit)
    {
        if (edit.AudienceIds is not null && !edit.AudienceIds.Any())
            throw new AnnouncementAudienceEmptyException();
    }
    
    private void ApplyAudienceChanging(Guid announcementId, IEnumerable<Guid> changedIds)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        var changedIdList = changedIds.ToList();

        // удаляем связки с пользователями, id которых нет в новом списке 
        dbContext.AnnouncementAudience
            .Where(aa => aa.AnnouncementId == announcementId && !changedIdList.Contains(aa.UserId))
            .ExecuteDelete();

        // добавляем связки с пользователями, id которых присутствуют в новом списке, но отсутствуют в бд
        var newIds = changedIdList.Except(
            dbContext.AnnouncementAudience
                .Where(aa => aa.AnnouncementId == announcementId)
                .Select(aa => aa.UserId));
        foreach (var newId in newIds)
            dbContext.AnnouncementAudience.Add(new AnnouncementAudience(announcementId, newId));
    }

    private void ApplyCategoriesChanging(Guid announcementId, IEnumerable<Guid> changedIds)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        var changedIdList = changedIds.ToList();

        // удаляем связки с пользователями, id которых нет в новом списке
        dbContext.AnnouncementCategoryJoins
            .Where(aa =>
                aa.AnnouncementId == announcementId && !changedIdList.Contains(aa.AnnouncementCategoryId))
            .ExecuteDelete();

        // добавляем связки с пользователями, id которых присутствуют в новом списке, но отсутствуют в бд
        var newIds = changedIdList.Except(
            dbContext.AnnouncementCategoryJoins
                .Where(aa => aa.AnnouncementId == announcementId)
                .Select(aa => aa.AnnouncementCategoryId));
        foreach (var newId in newIds)
            dbContext.AnnouncementCategoryJoins.Add(new AnnouncementAnnouncementCategory(announcementId, newId));
    }

    private void ApplyAttachmentsChanging(Guid announcementId, IEnumerable<Guid> changedIds)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        // todo нельзя откреплять опросы
        var changedIdList = changedIds.ToList();

        // удаляем связки с пользователями, id которых нет в новом списке
        dbContext.AnnouncementAttachmentJoins
            .Where(aa => aa.AnnouncementId == announcementId && !changedIdList.Contains(aa.AttachmentId))
            .ExecuteDelete();

        // добавляем связки с пользователями, id которых присутствуют в новом списке, но отсутствуют в бд
        var newIds = changedIdList.Except(
            dbContext.AnnouncementAttachmentJoins
                .Where(aa => aa.AnnouncementId == announcementId)
                .Select(aa => aa.AttachmentId));
        foreach (var newId in newIds)
            dbContext.AnnouncementAttachmentJoins.Add(new AnnouncementAttachment(announcementId, newId));
    }
}