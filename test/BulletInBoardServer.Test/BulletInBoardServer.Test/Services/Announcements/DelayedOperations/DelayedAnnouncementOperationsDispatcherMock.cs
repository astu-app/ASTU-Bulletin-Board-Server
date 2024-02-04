using BulletInBoardServer.Services.Announcements.DelayedOperations;

namespace BulletInBoardServer.Test.Services.Announcements.DelayedOperations;

public class DelayedAnnouncementOperationsDispatcherMock : IDelayedAnnouncementOperationsDispatcher
{
    public int ConfigureDelayedPublishingCalled { get; private set; }
    public int ReconfigureDelayedPublishingCalled { get; private set; }
    public int DisableDelayedPublishingCalled { get; private set; }
    
    public int ConfigureDelayedHidingCalled { get; private set; }
    public int ReconfigureDelayedHidingCalled { get; private set; }
    public int DisableDelayedHidingCalled { get; private set; }
    
    public void ConfigureDelayedPublishing(Guid announcementId, DateTime publishAt) => 
        ConfigureDelayedPublishingCalled++;

    public void ReconfigureDelayedPublishing(Guid announcementId, DateTime publishAt) => 
        ReconfigureDelayedPublishingCalled++;

    public void DisableDelayedPublishing(Guid announcementId) => 
        DisableDelayedPublishingCalled++;

    public void ConfigureDelayedHiding(Guid announcementId, DateTime hideAt) => 
        ConfigureDelayedHidingCalled++;

    public void ReconfigureDelayedHiding(Guid announcementId, DateTime hideAt) => 
        ReconfigureDelayedHidingCalled++;

    public void DisableDelayedHiding(Guid announcementId) => 
        DisableDelayedHidingCalled++;
}