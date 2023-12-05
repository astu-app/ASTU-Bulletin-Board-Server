namespace BulletInBoardServer.Models.Announcements.Categories.Subscribers;

public static class SubscribersExtensions
{
    public static ReadOnlySubscribers AsReadOnly(this Subscribers subscribers) =>
        new (subscribers);
}