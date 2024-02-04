using BulletInBoardServer.DataAccess;
using BulletInBoardServer.Services.Announcements.DelayedOperations;
using BulletInBoardServer.Services.Announcements.Infrastructure;

namespace BulletInBoardServer.Services.Announcements.ServiceCore;

public class DelayedHidingAnnouncementService(
    ApplicationDbContext dbContext,
    IDelayedAnnouncementOperationsDispatcher dispatcher)
    : CoreAnnouncementServiceBase(dbContext, dispatcher)
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
        var announcement = GetAnnouncementSummary(announcementId);

        // Мы не вызываем отключения отложенного сокрытия, так как попадаем в этот метод во время процесса
        // автоматического отложенного сокрытия
        Dispatcher.DisableDelayedPublishing(announcementId);

        announcement.Hide(hiddenAt);
        DbContext.SaveChanges();
    }
}