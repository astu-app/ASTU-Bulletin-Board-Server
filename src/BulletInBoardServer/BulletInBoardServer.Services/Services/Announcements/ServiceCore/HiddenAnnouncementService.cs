﻿using BulletInBoardServer.Services.Services.Announcements.DelayedOperations;
using BulletInBoardServer.Services.Services.Announcements.Models;
using BulletInBoardServer.Services.Services.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace BulletInBoardServer.Services.Services.Announcements.ServiceCore;

public class HiddenAnnouncementService(
    IServiceScopeFactory scopeFactory,
    IDelayedAnnouncementOperationsDispatcher dispatcher)
    : DispatcherDependentAnnouncementServiceBase(scopeFactory, dispatcher)
{
    /// <summary>
    /// Метод возвращает список скрытых объявлений для указанного пользователя
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <returns>Список с краткой информацией об объявлениях</returns>
    public IEnumerable<AnnouncementSummary> GetListForUser(Guid requesterId)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        return dbContext.Announcements
            .Where(a => a.AuthorId == requesterId && a.IsHidden)
            .Select(a => new
            {
                Announcement = a,
                ViewsCount = dbContext.AnnouncementAudience.Count(au => au.AnnouncementId == a.Id && au.Viewed)
            })
            .Select(res => res.Announcement.GetSummary(res.ViewsCount));
    }

    /// <summary>
    /// Метод восстанавливает скрытое объявление
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <param name="announcementId">Id восстанавливаемого объявления</param>
    /// <param name="restoredAt">Время восстановления объявления</param>
    /// <exception cref="OperationNotAllowedException">Пользователь не имеет права  выполнения операции</exception>
    public void Restore(Guid requesterId, Guid announcementId, DateTime restoredAt)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        var announcement = GetAnnouncementSummary(announcementId, dbContext);
        if (announcement.AuthorId != requesterId)
            throw new OperationNotAllowedException("Восстановить скрытое объявление может только его автор");

        if (announcement.ExpectsDelayedPublishing)
            Dispatcher.DisableDelayedPublishing(announcementId);

        announcement.Restore(DateTime.Now, restoredAt);
        dbContext.SaveChanges();
    }
}