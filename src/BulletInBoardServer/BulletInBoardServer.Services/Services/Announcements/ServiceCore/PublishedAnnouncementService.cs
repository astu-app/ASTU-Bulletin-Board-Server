using BulletInBoardServer.Services.Services.Announcements.DelayedOperations;
using BulletInBoardServer.Services.Services.Announcements.Models;
using BulletInBoardServer.Services.Services.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace BulletInBoardServer.Services.Services.Announcements.ServiceCore;

public class PublishedAnnouncementService(
    IServiceScopeFactory scopeFactory,
    IDelayedAnnouncementOperationsDispatcher dispatcher)
    : DispatcherDependentAnnouncementServiceBase(scopeFactory, dispatcher)
{
    /// <summary>
    /// Метод возвращает список опубликованных для пользователя объявлений
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <returns>Список с краткой информацией об объявлениях</returns>
    public IEnumerable<AnnouncementSummary> GetListForUser(Guid requesterId)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        return dbContext.Announcements
            .Join(dbContext.AnnouncementAudience,
                a => a.Id, au => au.AnnouncementId,
                (a, au) => new { Announcement = a, Audience = au })
            .Where(res => res.Announcement.IsPublished && res.Audience.UserId == requesterId)
            .GroupBy(res => res.Announcement.Id)
            // так как группируем по Id объявления и все объявления группы будут содержать одно и то же объявление,
            // из группы выбираем объявление первого элемента
            .Select(group => group.First().Announcement.GetSummary(group.Count()));
    }

    /// <summary>
    /// Метод скрывает объявление указанным пользователем
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <param name="announcementId">Id объявления, которое требуется скрыть</param>
    /// <param name="hiddenAt">Момент сокрытия объявления</param>
    /// <exception cref="OperationNotAllowedException">Пользователь не имеет права  выполнения операции</exception>
    public void HideManually(Guid requesterId, Guid announcementId, DateTime hiddenAt)
    {
        using var scope = CreateScope();
        var dbContext = GetDbContextForScope(scope);
        
        var announcement = GetAnnouncementSummary(announcementId, dbContext);
        if (announcement.AuthorId != requesterId)
            throw new OperationNotAllowedException("Объявление может скрыть только его автор");

        if (announcement.ExpectsDelayedHiding)
            Dispatcher.DisableDelayedHiding(announcementId);

        announcement.Hide(DateTime.Now, hiddenAt);
        dbContext.SaveChanges();
    }
}