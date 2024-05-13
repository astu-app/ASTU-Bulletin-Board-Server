using BulletInBoardServer.Services.Services.Announcements.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BulletInBoardServer.Services.Services.Announcements.ServiceCore;

/// <summary>
/// Сервис работы с объявлениями, ожидающими отложенную публикацию
/// </summary>
public class DelayedPublicationAnnouncementService(IServiceScopeFactory scopeFactory)
    : CoreAnnouncementServiceBase(scopeFactory)
{
    /// <summary>
    /// Метод возвращает список объявлений, ожидающих отложенной автоматической публикации,
    /// для заданного пользователя
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <returns>Список с краткой информацией об объявлениях</returns>
    public IEnumerable<AnnouncementSummary> GetDelayedPublicationAnnouncementListForUser(Guid requesterId)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);

        var announcements = dbContext.Announcements
            .Where(a => a.AuthorId == requesterId && a.ExpectsDelayedPublishing)
            .Include(a => a.Author)
            .Select(a => new
            {
                Announcement = a,
                ViewsCount = dbContext.AnnouncementAudience.Count(au => au.AnnouncementId == a.Id && au.Viewed)
            })
            .ToList();

        foreach (var announcement in announcements)
            LoadAnnouncementSurveys(announcement.Announcement, requesterId, dbContext);

        return announcements
            .Select(res => res.Announcement.GetSummary(res.ViewsCount)); // todo сортировка по возрастанию времени отложенной публикации 
    }

    /// <summary>
    /// Метод публикует заданное объявление в автоматическом порядке
    /// </summary>
    /// <param name="announcementId">Id заданного объявления</param>
    /// <param name="publishedAt">Время публикации объявления</param>
    public void PublishAutomatically(Guid announcementId, DateTime publishedAt)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        // Отключение отложенной публикации происходит при вызове диспетчером этого метода
        var announcement = GetAnnouncementSummary(announcementId, dbContext);
        announcement.Publish(DateTime.Now, publishedAt);
        dbContext.SaveChanges();

        // todo уведомление о публикации
    }
}