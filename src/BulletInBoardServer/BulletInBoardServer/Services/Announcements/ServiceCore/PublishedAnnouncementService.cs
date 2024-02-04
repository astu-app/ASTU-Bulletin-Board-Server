using BulletInBoardServer.DataAccess;
using BulletInBoardServer.Services.Announcements.DelayedOperations;
using BulletInBoardServer.Services.Announcements.Infrastructure;

namespace BulletInBoardServer.Services.Announcements.ServiceCore;

public class PublishedAnnouncementService(
    ApplicationDbContext dbContext,
    IDelayedAnnouncementOperationsDispatcher dispatcher) 
    : CoreAnnouncementServiceBase(dbContext, dispatcher)
{
    /// <summary>
    /// Метод возвращает список опубликованных для пользователя объявлений
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <returns>Список с краткой информацией об объявлениях</returns>
    public IEnumerable<AnnouncementSummary> GetListForUser(Guid requesterId) =>
        DbContext.Announcements
            .Join(DbContext.AnnouncementAudience,
                a => a.Id, au => au.AnnouncementId,
                (a, au) => new { Announcement = a, Audience = au })
            .Where(res => res.Announcement.IsPublished && res.Audience.UserId == requesterId)
            .GroupBy(res => res.Announcement.Id)
            // так как группируем по Id объявления и все объявления группы будут содержать одно и то же объявление,
            // из группы выбираем объявление первого элемента
            .Select(group => group.First().Announcement.GetSummary(group.Count())); 

    /// <summary>
    /// Метод скрывает объявление указанным пользователем
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <param name="announcementId">Id объявления, которое требуется скрыть</param>
    /// <param name="hiddenAt">Момент сокрытия объявления</param>
    public void HideManually(Guid requesterId, Guid announcementId, DateTime hiddenAt)
    {
        var announcement = GetAnnouncementSummary(announcementId);
        if (announcement.AuthorId != requesterId)
            throw new InvalidOperationException("Объявление может скрыть только его автор");
        
        if (announcement.ExpectsDelayedHiding)
            Dispatcher.DisableDelayedHiding(announcementId);

        announcement.Hide(hiddenAt);
        DbContext.SaveChanges();
    }
}