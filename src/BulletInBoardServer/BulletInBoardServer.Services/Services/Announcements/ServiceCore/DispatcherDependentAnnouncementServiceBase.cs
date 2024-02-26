using BulletInBoardServer.Services.Services.Announcements.DelayedOperations;
using Microsoft.Extensions.DependencyInjection;

namespace BulletInBoardServer.Services.Services.Announcements.ServiceCore;

public class DispatcherDependentAnnouncementServiceBase(
    IServiceScopeFactory scopeFactory,
    IDelayedAnnouncementOperationsDispatcher dispatcher)
    : CoreAnnouncementServiceBase(scopeFactory)
{
    protected readonly IDelayedAnnouncementOperationsDispatcher Dispatcher = dispatcher;
}