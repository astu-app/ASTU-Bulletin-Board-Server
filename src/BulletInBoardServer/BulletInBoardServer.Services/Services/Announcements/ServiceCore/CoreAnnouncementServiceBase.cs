using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Services.Services.Announcements.Exceptions;

namespace BulletInBoardServer.Services.Services.Announcements.ServiceCore;

public class CoreAnnouncementServiceBase(ApplicationDbContext dbContext)
{
    protected readonly ApplicationDbContext DbContext = dbContext;



    protected Announcement GetAnnouncementSummary(Guid announcementId)
    {
        try
        {
            var announcement = DbContext.Announcements.SingleOrDefault(a => a.Id == announcementId);
            if (announcement is null)
                throw new AnnouncementDoesNotExist($"Объявление с Id = {announcementId} отсутствует в БД");

            return announcement;
        }
        catch (InvalidOperationException err)
        {
            throw new InvalidOperationException("Не удалось загрузить объявление из БД", err);
        }
    }
}