using BulletInBoardServer.DataAccess;
using BulletInBoardServer.Services.Announcements.DelayedOperations;
using BulletInBoardServer.Services.Announcements.Infrastructure;

namespace BulletInBoardServer.Services.Announcements.ServiceCore;

/// <summary>
/// Сервис работы с объявлениями, ожидающими отложенную публикацию
/// </summary>
public class DelayedPublicationAnnouncementService(
    ApplicationDbContext dbContext,
    IDelayedAnnouncementOperationsDispatcher dispatcher)
    : CoreAnnouncementServiceBase(dbContext, dispatcher)
{
    /// <summary>
    /// Метод возвращает список объявлений, ожидающих отложенной автоматической публикации,
    /// для заданного пользователя
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <returns>Список с краткой информацией об объявлениях</returns>
    public IEnumerable<AnnouncementSummary> GetDelayedPublicationAnnouncementListForUser(Guid requesterId) =>
        DbContext.Announcements
            .Where(a => a.AuthorId == requesterId && a.ExpectsDelayedPublishing)
            .Select(a => new
            {
                Announcement = a,
                ViewsCount = DbContext.AnnouncementAudience.Count(au => au.AnnouncementId == a.Id && au.Viewed)
            })
            .Select(res => res.Announcement.GetSummary(res.ViewsCount));

    /// <summary>
    /// Метод публикует заданное объявление в автоматическом порядке
    /// </summary>
    /// <param name="announcementId">Id заданного объявления</param>
    /// <param name="publishedAt">Время публикации объявления</param>
    public void PublishAutomatically(Guid announcementId, DateTime publishedAt)
    {
        var announcement = GetAnnouncementSummary(announcementId);

        // Мы не вызываем отключения отложенную публикацию, так как попадаем в этот метод во время процесса
        // автоматической отложенной публикации
        Dispatcher.DisableDelayedPublishing(announcementId);

        announcement.Publish(publishedAt);
        DbContext.SaveChanges();

        // todo уведомление о публикации
    }
}