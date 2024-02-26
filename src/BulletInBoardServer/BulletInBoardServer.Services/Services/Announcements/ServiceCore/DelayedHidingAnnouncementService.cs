﻿using BulletInBoardServer.Services.Services.Announcements.Models;
using Microsoft.Extensions.DependencyInjection;

namespace BulletInBoardServer.Services.Services.Announcements.ServiceCore;

/// <summary>
/// Сервис для управления объявлениями, ожидающими отложенного сокрытия
/// </summary>
public class DelayedHidingAnnouncementService(IServiceScopeFactory scopeFactory)
    : CoreAnnouncementServiceBase(scopeFactory)
{
    /// <summary>
    /// Метод возвращает список объявлений, ожидающих отложенного автоматического сокрытия,
    /// для заданного пользователя
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <returns>Список с краткой информацией об объявлениях</returns>
    public IEnumerable<AnnouncementSummary> GetDelayedHiddenAnnouncementListForUser(Guid requesterId)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        return dbContext.Announcements
            .Where(a => a.AuthorId == requesterId && a.ExpectsDelayedHiding)
            .Select(a => new
            {
                Announcement = a,
                ViewsCount = dbContext.AnnouncementAudience.Count(au => au.AnnouncementId == a.Id && au.Viewed)
            })
            .Select(res => res.Announcement.GetSummary(res.ViewsCount));
    }

    /// <summary>
    /// Метод скрывает объявление в автоматическом порядке
    /// </summary>
    /// <param name="announcementId">Id объявления, которое требуется скрыть</param>
    /// <param name="hiddenAt">Момент сокрытия объявления</param>
    public void HideAutomatically(Guid announcementId, DateTime hiddenAt)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        // Отключение отложенного сокрытия происходит при вызове диспетчером этого метода
        var announcement = GetAnnouncementSummary(announcementId, dbContext);
        announcement.Hide(DateTime.Now, hiddenAt);
        dbContext.SaveChanges();
    }
}