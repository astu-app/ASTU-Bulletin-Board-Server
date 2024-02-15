using BulletInBoardServer.DataAccess;
using BulletInBoardServer.Services.Announcements.Infrastructure;

namespace BulletInBoardServer.Services.Announcements.ServiceCore;

/// <summary>
/// Сервис для управления объявлениями, ожидающими отложенного сокрытия
/// </summary>
public class DelayedHidingAnnouncementService(ApplicationDbContext dbContext)
    : CoreAnnouncementServiceBase(dbContext)
{
    /// <summary>
    /// Метод возвращает список объявлений, ожидающих отложенного автоматического сокрытия,
    /// для заданного пользователя
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <returns>Список с краткой информацией об объявлениях</returns>
    public IEnumerable<AnnouncementSummary> GetDelayedHiddenAnnouncementListForUser(Guid requesterId) =>
        DbContext.Announcements
            .Where(a => a.AuthorId == requesterId && a.ExpectsDelayedHiding)
            .Select(a => new
            {
                Announcement = a,
                ViewsCount = DbContext.AnnouncementAudience.Count(au => au.AnnouncementId == a.Id && au.Viewed)
            })
            .Select(res => res.Announcement.GetSummary(res.ViewsCount));

    /// <summary>
    /// Метод скрывает объявление в автоматическом порядке
    /// </summary>
    /// <param name="announcementId">Id объявления, которое требуется скрыть</param>
    /// <param name="hiddenAt">Момент сокрытия объявления</param>
    public void HideAutomatically(Guid announcementId, DateTime hiddenAt)
    {
        // Отключение отложенного сокрытия происходит при вызове диспетчером этого метода
        var announcement = GetAnnouncementSummary(announcementId);
        announcement.Hide(hiddenAt);
        DbContext.SaveChanges();
    }
}