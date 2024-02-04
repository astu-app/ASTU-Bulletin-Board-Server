namespace BulletInBoardServer.Services.Announcements.DelayedOperations;

public interface IDelayedAnnouncementOperationsDispatcher
{
    void ConfigureDelayedPublishing(Guid announcementId, DateTime publishAt);
    void ReconfigureDelayedPublishing(Guid announcementId, DateTime publishAt);
    void DisableDelayedPublishing(Guid announcementId);
    
    void ConfigureDelayedHiding(Guid announcementId, DateTime hideAt);
    void ReconfigureDelayedHiding(Guid announcementId, DateTime hideAt);
    void DisableDelayedHiding(Guid announcementId);
}