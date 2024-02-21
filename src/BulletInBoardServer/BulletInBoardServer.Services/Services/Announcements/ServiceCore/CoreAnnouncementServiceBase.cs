using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.Announcements;

namespace BulletInBoardServer.Services.Services.Announcements.ServiceCore;

public class CoreAnnouncementServiceBase(ApplicationDbContext dbContext)
{
    protected readonly ApplicationDbContext DbContext = dbContext;



    protected Announcement GetAnnouncementSummary(Guid announcementId)
    {
        try
        {
            return DbContext.Announcements.Single(a => a.Id == announcementId);
        }
        catch (InvalidOperationException err)
        {
            throw new InvalidOperationException("Не удалось загрузить объявление из БД", err);
        }
    }
}