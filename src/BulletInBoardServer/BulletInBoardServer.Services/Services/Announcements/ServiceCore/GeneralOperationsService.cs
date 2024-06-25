using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Domain.Models.Announcements.Exceptions;
using BulletInBoardServer.Domain.Models.Attachments;
using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Answers;
using BulletInBoardServer.Domain.Models.Users;
using BulletInBoardServer.Services.Services.Announcements.DelayedOperations;
using BulletInBoardServer.Services.Services.Announcements.Exceptions;
using BulletInBoardServer.Services.Services.Announcements.Models;
using BulletInBoardServer.Services.Services.Common.Exceptions;
using BulletInBoardServer.Services.Services.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BulletInBoardServer.Services.Services.Announcements.ServiceCore;

public class GeneralOperationsService(
    IServiceScopeFactory scopeFactory,
    IDelayedAnnouncementOperationsDispatcher dispatcher,
    NotificationService notificationService)
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
            PublishManually(announcement, create.UserIds, DateTime.Now, dbContext);

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
    /// <exception cref="AnnouncementDoesNotExistException">Объявление отсутствует в БД</exception>
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
        //
        // Аудитория будет подгружена позже
        // ReSharper disable once EntityFramework.NPlusOne.IncompleteDataQuery - аудитория будет загружена и установлена позже
        announcement = dbContext.Announcements
            .Where(a => a.Id == announcementId)
            .Include(a => a.Author)
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
            // .ThenInclude(a => a.Participation)
            // .ThenInclude(p => p.User)
            
            // .ThenInclude(a => a.Participation.Where(_ => a.Question.Survey.IsOpen || a.Question.Survey.ResultsOpenBeforeClosing))
            .Load();

        foreach (var survey in announcement.Attachments.OfType<Survey>())
        {
            var answersEnumerable = survey.Questions.SelectMany(q => q.Answers);
            // Если открыт и результаты опроса открыты до того, как опрос будет закрыт, догружаем проголосовавших
            // за каждый вариант ответа пользователей. Иначе же зануляем количество проголосовавших за каждый
            // вариант ответа
            if (survey is { IsOpen: true, ResultsOpenBeforeClosing: false })
                SetVotersCountToZero(answersEnumerable.ToList());
            else
                LoadAnswersParticipants(answersEnumerable.ToList());
        }

        // announcement.AudienceThreeNode = dbContext.UserGroups
        //     .Include(ug => ug.ChildrenGroups)
        //     // .Include(ug => ug.MemberRights)
        //     // .ThenInclude(mr => mr.User)
        //     .Join(dbContext.MemberRights,
        //         ug => ug.Id,
        //         mr => mr.UserGroupId,
        //         (ug, mr) => new { UserGrpup = ug, MemberRights = mr })
        //     .Join(dbContext.Users,
        //         join => join.MemberRights.UserId,
        //         u => u.Id,
        //         (join, u) => new { UserGroup = join.UserGrpup, User = u })
        //     .Join(dbContext.AnnouncementAudience,
        //         join => join.User.Id,
        //         aa => aa.UserId,
        //         (join, aa) => new { UserGroup = join.UserGroup, User = join.User, Audience = aa })
        //     .Where(join => join.Audience.AnnouncementId == announcementId && join.UserGroup.AdminId == requesterId)
        //     .Select(join => new __AudienceNode(null, join.UserGroup, null))
        //     .FirstOrDefault();
        //     // .Select(join => join.UserGroup)
        //     // .ToList()
        
        announcement.Audience = dbContext.AnnouncementAudience
            .Where(a => a.AnnouncementId == announcementId)
            .Include(a => a.User)
            .ToList()
            .Select(a => new CheckableUser(a.User, a.Viewed))
            .ToAudience();
        
        // ReSharper disable once EntityFramework.NPlusOne.IncompleteDataUsage - так как аудитория загружается и утсанавливается в предыдущем выражении
        announcement.ViewsCount = dbContext.AnnouncementAudience.Count(aa => aa.Viewed && aa.AnnouncementId == announcementId);

        return announcement;
    }

    /// <summary>
    /// Зафиксировать факт просмотра пользователем объявления 
    /// </summary>
    /// <param name="requesterId">Идентификатор пользователя, просматривающего объявление</param>
    /// <param name="announcementId">Идентификатор просматриваемого объявления</param>
    /// <exception cref="UserAnnouncementBindingDoesNotExistException">Отсутствует привязка пользователя к объявлению</exception>
    /// <exception cref="AnnouncementDoesNotExistException">Объявление отсутствует в бд</exception>
    public void AddView(Guid requesterId, Guid announcementId)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);

        var updatedRows = dbContext.AnnouncementAudience
            .Where(aa => aa.UserId == requesterId && aa.AnnouncementId == announcementId)
            .ExecuteUpdate(setter => setter.SetProperty(aa => aa.Viewed, true));

        if (updatedRows < 1)
            throw new UserAnnouncementBindingDoesNotExistException(
                $"Объявление с Id = {announcementId} не предназначено для пользователя с Id = {requesterId}");
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

        // Если объявление скрыто, то не уведомляем его аудиторию об изменениях 
        if (!announcement.IsPublished) 
            return;

        // Отправляем уведомление о публикации объявления новым пользователям. Ситуации, когда пользователям придет
        // уведомление о редактирования скрытого объявления не произойдет, так как нельзя одновременно скрыть 
        // объявление и отредактировать его
        if (edit.AudienceIds?.ToAdd is not null && announcement.IsPublished)
            Task.Run(() => notificationService.NotifyAll(edit.AudienceIds.ToAdd, "Новое объявление",
                $"Для вас опубликовано объявление от {announcement.FirstlyPublishedAt:d} {announcement.FirstlyPublishedAt:t}: {announcement.Content}"));
    }

    /// <summary>
    /// Удаление объявления
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию </param>
    /// <param name="announcementId">Id объявления</param>
    /// <exception cref="AnnouncementDoesNotExistException">Объявление отсутствует в БД</exception>
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
        
        dbContext.SaveChanges();
    }

    /// <summary>
    /// Метод публикует заданное объявление указанным пользователем
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию</param>
    /// <param name="announcementId">Id заданного объявления</param>
    /// <param name="publishedAt">Время публикации объявления</param>
    /// <exception cref="AnnouncementDoesNotExistException">Объявление отсутствует в БД</exception>
    /// <exception cref="OperationNotAllowedException">Пользователь не имеет права  выполнения операции</exception>
    public void PublishManually(Guid requesterId, Guid announcementId, DateTime publishedAt)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        var announcement = GetAnnouncementSummary(announcementId, dbContext);
        if (announcement.AuthorId != requesterId)
            throw new OperationNotAllowedException("Опубликовать объявление может только его автор");
        
        var audienceIds = LoadAnnouncementAudience(announcementId, dbContext);
        PublishManually(announcement, audienceIds, publishedAt, dbContext);
    }



    private void SetVotersCountToZero(IEnumerable<Answer> answers)
    {
        foreach (var answer in answers)
            answer.VotersCount = 0;
    }

    private void LoadAnswersParticipants(IEnumerable<Answer> answers) => 
        answers.AsParallel().ForAll(LoadAnswerParticipants);

    private void LoadAnswerParticipants(Answer answer)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);

        dbContext.Entry(answer)
            .Collection(a => a.Participation)
            .Load();
        
        foreach (var participation in answer.Participation) 
            dbContext.Entry(participation)
                .Reference(p => p.User)
                .Load();
    }

    private void PublishManually(Announcement announcement, IEnumerable<Guid> audienceIds, DateTime publishedAt, DbContext dbContext)
    {
        if (announcement.ExpectsDelayedPublishing)
            Dispatcher.DisableDelayedPublishing(announcement.Id);

        var hasBeenPublished = announcement.HasBeenPublished;
        announcement.Publish(DateTime.Now, publishedAt);

        Console.WriteLine($"published: {publishedAt}");
        dbContext.SaveChanges();

        // Если объявление уже публиковалось, то никаких уведомлений не рассылаем
        if (hasBeenPublished)
            return;
        
        Task.Run(() => notificationService.NotifyAll(audienceIds, "Новое объявление", announcement.Content));
    }

    private static IEnumerable<Guid> LoadAnnouncementAudience(Guid announcementId, ApplicationDbContext dbContext) =>
        dbContext.AnnouncementAudience
            .Where(a => a.AnnouncementId == announcementId)
            .Select(a => a.UserId);
}