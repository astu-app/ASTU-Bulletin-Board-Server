using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Services.Services.Announcements.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace BulletInBoardServer.Services.Services.Announcements.ServiceCore;

// public class CoreAnnouncementServiceBase(ApplicationDbContext dbContext)
public class CoreAnnouncementServiceBase(IServiceScopeFactory scopeFactory)
{
    /// <summary>
    /// Метод загружает объявление без связанных сущностей
    /// </summary>
    /// <param name="announcementId">Id загружаемого объявления</param>
    /// <param name="dbContext">Контекст базы данных</param>
    /// <returns></returns>
    /// <exception cref="AnnouncementDoesNotExistException">Объявление отсутствует в БД</exception>
    /// <exception cref="InvalidOperationException">Не удалось загрузить объявление из БД</exception>
    protected static Announcement GetAnnouncementSummary(Guid announcementId, ApplicationDbContext dbContext)
    {
        try
        {
            var announcement = dbContext.Announcements.SingleOrDefault(a => a.Id == announcementId);
            if (announcement is null)
                throw new AnnouncementDoesNotExistException($"Объявление с Id = {announcementId} отсутствует в БД");

            return announcement;
        }
        catch (InvalidOperationException err) when (err is not AnnouncementDoesNotExistException)
        {
            throw new InvalidOperationException("Не удалось загрузить объявление из БД", err);
        }
    }

    protected IServiceScope CreateScope() =>
        scopeFactory.CreateScope();
    
    protected static ApplicationDbContext GetDbContextForScope(IServiceScope scope) =>
        scope.ServiceProvider.GetService<ApplicationDbContext>() ??
        throw new ApplicationException(
            $"{nameof(ApplicationDbContext)} не зарегистрирован в качестве сервиса");
}