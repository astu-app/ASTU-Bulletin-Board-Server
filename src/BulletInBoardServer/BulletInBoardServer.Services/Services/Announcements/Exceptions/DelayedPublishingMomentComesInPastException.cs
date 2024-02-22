namespace BulletInBoardServer.Services.Services.Announcements.Exceptions;

public class DelayedPublishingMomentComesInPastException : InvalidOperationException
{
    public DelayedPublishingMomentComesInPastException()
        : base("Момент отложенной публикации не может наступить в прошлом")
    {
    }
}