using BulletInBoardServer.Models.Announcements;

namespace BulletInBoardServer.Services.Announcements.Infrastructure;

public static class AnnouncementSummaryExtensions
{
    // public static AnnouncementSummary GetSummary(this Announcement announcement, ApplicationDbContext dbContext) =>
    public static AnnouncementSummary GetSummary(this Announcement announcement, int viewsCount) =>
        new(
            id: announcement.Id,
            author: announcement.Author,
            content: announcement.Content,
            viewsCount: viewsCount,
            expectsDelayedPublishing: announcement.ExpectsDelayedPublishing,
            delayedPublishingAt: announcement.DelayedPublishingAt,
            expectsDelayedHiding: announcement.ExpectsDelayedHiding,
            delayedHidingAt: announcement.DelayedHidingAt,
            publishedAt: announcement.PublishedAt,
            hiddenAt: announcement.HiddenAt);
}