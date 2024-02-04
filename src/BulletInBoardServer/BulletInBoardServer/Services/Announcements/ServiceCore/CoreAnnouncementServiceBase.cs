using BulletInBoardServer.DataAccess;
using BulletInBoardServer.Models.Announcements;
using BulletInBoardServer.Services.Announcements.DelayedOperations;

namespace BulletInBoardServer.Services.Announcements.ServiceCore;

public class CoreAnnouncementServiceBase(
    ApplicationDbContext dbContext,
    IDelayedAnnouncementOperationsDispatcher dispatcher)
{
    protected readonly ApplicationDbContext DbContext = dbContext;
    protected readonly IDelayedAnnouncementOperationsDispatcher Dispatcher = dispatcher;



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