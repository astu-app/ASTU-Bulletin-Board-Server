using BulletInBoardServer.Domain;
using BulletInBoardServer.Services.Services.Announcements.DelayedOperations;

namespace BulletInBoardServer.Services.Services.Announcements.ServiceCore;

public class DispatcherDependentAnnouncementServiceBase(
    ApplicationDbContext dbContext,
    IDelayedAnnouncementOperationsDispatcher dispatcher)
    : CoreAnnouncementServiceBase(dbContext)
{
    protected readonly IDelayedAnnouncementOperationsDispatcher Dispatcher = dispatcher;
}