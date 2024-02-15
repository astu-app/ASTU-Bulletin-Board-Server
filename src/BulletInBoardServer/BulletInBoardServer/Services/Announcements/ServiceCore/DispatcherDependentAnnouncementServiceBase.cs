using BulletInBoardServer.DataAccess;
using BulletInBoardServer.Services.Announcements.DelayedOperations;

namespace BulletInBoardServer.Services.Announcements.ServiceCore;

public class DispatcherDependentAnnouncementServiceBase(
    ApplicationDbContext dbContext,
    IDelayedAnnouncementOperationsDispatcher dispatcher) 
    : CoreAnnouncementServiceBase(dbContext)
{
    protected readonly IDelayedAnnouncementOperationsDispatcher Dispatcher = dispatcher;
}