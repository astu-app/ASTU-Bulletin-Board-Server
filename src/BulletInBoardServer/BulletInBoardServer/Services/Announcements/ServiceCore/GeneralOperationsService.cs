using BulletInBoardServer.DataAccess;
using BulletInBoardServer.Models.Announcements;
using BulletInBoardServer.Models.Attachments;
using BulletInBoardServer.Models.Attachments.Surveys;
using BulletInBoardServer.Models.JoinEntities;
using BulletInBoardServer.Services.Announcements.DelayedOperations;
using BulletInBoardServer.Services.Announcements.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AnnouncementAudience = BulletInBoardServer.Models.JoinEntities.AnnouncementAudience;

namespace BulletInBoardServer.Services.Announcements.ServiceCore;

public class GeneralOperationsService(
    ApplicationDbContext dbContext,
    IDelayedAnnouncementOperationsDispatcher dispatcher)
    : CoreAnnouncementServiceBase(dbContext, dispatcher)
{
    public Announcement Create(Guid requesterId, CreateAnnouncement create)
    {
        if (requesterId != create.AuthorId)
            throw new InvalidOperationException("Создать объявление может только его автор");

        DelayedMomentsCorrectOrThrow(create);

        var announcement = InitAnnouncement(create);
        DbContext.Announcements.Add(announcement);

        AddRelatedEntitiesToDb(create, announcement);
        DbContext.SaveChanges();

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

    public Announcement GetDetails(Guid requesterId, Guid announcementId)
    {
        var announcement = GetAnnouncementSummary(announcementId);
        if (announcement.AuthorId != requesterId)
            throw new InvalidOperationException("Получить детали объявления может только его автор");

        // В данном случае можно использовать явную загрузку содержимого коллекций методом Load, но в таком случае
        // для каждой коллекции создается отдельный запрос к базе данных, что создает излишнюю нагрузку на СУБД
        // и на сеть. Для уменьшения нагрузки можно загрузить содержимое коллекций отдельным запросом. В таком 
        // случае будет выполнена повторная загрузка уже загруженных данных, но учитывая небольшой объем этих
        // данных, временные затраты на повторную его загрузку минимальны. 
        announcement = DbContext.Announcements
            .Where(a => a.Id == announcementId)
            .Include(a => a.Author)
            .Include(a => a.Audience)
            .Include(a => a.Categories)
            .Include(a => a.Attachments)
            .Single();
        // Так как к объявлению могут быть прикреплены разные типы вложений и присутствует необходимость загрузить
        // связанные с этими вложениями сущности, для каждого из типов вложений используется явная загрузка.
        DbContext.Entry(announcement).Collection(a => a.Attachments)
            .Query()
            .Where(a => a.Type == AttachmentTypes.Survey)
            .Cast<Survey>()
            .Include(s => s.Voters)
            .Include(s => s.Questions)
            .ThenInclude(q => q.Answers)
            .ThenInclude(q => q.Participation)
            .Load();

        return announcement;
    }

    public void Edit(Guid requesterId, EditAnnouncement edit)
    {
        var announcement = GetAnnouncementSummary(edit.Id);
        if (requesterId != announcement.AuthorId)
            throw new InvalidOperationException("Редактировать объявление может только его автор");

        if (edit.Content is not null)
            announcement.Content = edit.Content;
        if (edit.AudienceIds is not null)
            ApplyAudienceChanging(announcement.Id, edit.AudienceIds);
        if (edit.CategoryIds is not null)
            ApplyCategoriesChanging(announcement.Id, edit.CategoryIds);
        if (edit.AttachmentIds is not null)
            ApplyAttachmentsChanging(announcement.Id, edit.AttachmentIds);
        if (edit.DelayedPublishingAtChanged)
            announcement.DelayedPublishingAt = edit.DelayedPublishingAt;
        if (edit.DelayedHidingAtChanged)
            announcement.DelayedHidingAt = edit.DelayedHidingAt;

        DbContext.SaveChanges();

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

    public void Delete(Guid requesterId, Guid announcementId)
    {
        var announcement = GetAnnouncementSummary(announcementId);
        if (requesterId != announcement.AuthorId)
            throw new InvalidOperationException("Удалить объявление может только его автор");

        if (announcement.ExpectsDelayedPublishing)
            Dispatcher.DisableDelayedPublishing(announcementId);
        if (announcement.ExpectsDelayedHiding)
            Dispatcher.DisableDelayedHiding(announcementId);

        DbContext.Announcements
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
    public void PublishManually(Guid requesterId, Guid announcementId, DateTime publishedAt)
    {
        var announcement = GetAnnouncementSummary(announcementId);
        if (announcement.AuthorId != requesterId)
            throw new InvalidOperationException("Опубликовать объявление может только его автор");

        if (announcement.ExpectsDelayedPublishing)
            Dispatcher.DisableDelayedPublishing(announcementId);

        announcement.Publish(publishedAt);
        DbContext.SaveChanges();

        // todo уведомление о публикации
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
            MomentWillComeBeforeOrThrow(now, publishAt.Value,
                "Момент отложенной публикации не может наступить в прошлом");
            return;
        }

        if (publishAt is null && hideAt is not null)
        {
            MomentWillComeBeforeOrThrow(now, hideAt.Value,
                "Момент отложенного сокрытия не может наступить в прошлом");
            return;
        }

        MomentWillComeBeforeOrThrow(now, publishAt!.Value,
            "Момент отложенной публикации не может наступить в прошлом");
        MomentWillComeBeforeOrThrow(now, hideAt!.Value,
            "Момент отложенного сокрытия не может наступить в прошлом");
        MomentWillComeBeforeOrThrow(publishAt.Value, hideAt.Value,
            "Момент отложенной публикации не может наступить после момента отложенного сокрытия");
    }

    /// <summary>
    /// Метод проверяет, что первый момент наступит до второго, и генерирует исключение в противном случае
    /// </summary>
    /// <param name="first">Первый момент</param>
    /// <param name="second">Второй момент</param>
    /// <param name="errMessage">Сообщение об ошибке</param>
    /// <exception cref="InvalidOperationException">Генерируется в случае, если первый момент наступит не раньше второго</exception>
    private static void MomentWillComeBeforeOrThrow(DateTime first, DateTime second, string errMessage)
    {
        if (first >= second)
            throw new InvalidOperationException(errMessage);
    }

    private static Announcement InitAnnouncement(CreateAnnouncement createAnnouncement) =>
        new(
            id: Guid.NewGuid(),
            content: createAnnouncement.Content,
            authorId: createAnnouncement.AuthorId,
            publishedAt: null,
            hiddenAt: null,
            delayedPublishingAt: createAnnouncement.DelayedPublishingAt,
            delayedHidingAt: createAnnouncement.DelayedHidingAt
        );

    private void AddRelatedEntitiesToDb(CreateAnnouncement create, Announcement announcement)
    {
        var audience = InitUserAudience(announcement.Id, create.UserIds);
        DbContext.AnnouncementAudience.AddRange(audience);

        var attachmentJoins = InitAttachmentJoins(announcement.Id, create.AttachmentIds);
        DbContext.AnnouncementAttachmentJoins.AddRange(attachmentJoins);

        var categoryJoins = InitCategoryJoins(announcement.Id, create.CategoryIds);
        DbContext.AnnouncementCategoryJoins.AddRange(categoryJoins);
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

    private void ApplyAudienceChanging(Guid announcementId, IEnumerable<Guid> changedIds)
    {
        var changedIdList = changedIds.ToList();

        // удаляем связки с пользователями, id которых нет в новом списке 
        DbContext.AnnouncementAudience
            .Where(aa => aa.AnnouncementId == announcementId && !changedIdList.Contains(aa.UserId))
            .ExecuteDelete();

        // добавляем связки с пользователями, id которых присутствуют в новом списке, но отсутствуют в бд
        var newIds = changedIdList.Except(
            DbContext.AnnouncementAudience
                .Where(aa => aa.AnnouncementId == announcementId)
                .Select(aa => aa.UserId));
        foreach (var newId in newIds)
            DbContext.AnnouncementAudience.Add(new AnnouncementAudience(announcementId, newId));
    }

    private void ApplyCategoriesChanging(Guid announcementId, IEnumerable<Guid> changedIds)
    {
        var changedIdList = changedIds.ToList();

        // удаляем связки с пользователями, id которых нет в новом списке
        DbContext.AnnouncementCategoryJoins
            .Where(aa => 
                aa.AnnouncementId == announcementId && !changedIdList.Contains(aa.AnnouncementCategoryId))
            .ExecuteDelete();

        // добавляем связки с пользователями, id которых присутствуют в новом списке, но отсутствуют в бд
        var newIds = changedIdList.Except(
            DbContext.AnnouncementCategoryJoins
                .Where(aa => aa.AnnouncementId == announcementId)
                .Select(aa => aa.AnnouncementCategoryId));
        foreach (var newId in newIds)
            DbContext.AnnouncementCategoryJoins.Add(new AnnouncementAnnouncementCategory(announcementId, newId));
    }

    private void ApplyAttachmentsChanging(Guid announcementId, IEnumerable<Guid> changedIds)
    {
        var changedIdList = changedIds.ToList();

        // удаляем связки с пользователями, id которых нет в новом списке
        DbContext.AnnouncementAttachmentJoins
            .Where(aa => aa.AnnouncementId == announcementId && !changedIdList.Contains(aa.AttachmentId))
            .ExecuteDelete();

        // добавляем связки с пользователями, id которых присутствуют в новом списке, но отсутствуют в бд
        var newIds = changedIdList.Except(
            DbContext.AnnouncementAttachmentJoins
                .Where(aa => aa.AnnouncementId == announcementId)
                .Select(aa => aa.AttachmentId));
        foreach (var newId in newIds)
            DbContext.AnnouncementAttachmentJoins.Add(new AnnouncementAttachment(announcementId, newId));
    }
}