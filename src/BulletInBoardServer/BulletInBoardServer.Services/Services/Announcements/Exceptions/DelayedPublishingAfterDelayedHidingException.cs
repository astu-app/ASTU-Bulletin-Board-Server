namespace BulletInBoardServer.Services.Services.Announcements.Exceptions;

public class DelayedPublishingAfterDelayedHidingException : InvalidOperationException
{
    public DelayedPublishingAfterDelayedHidingException()
        : base("Момент отложенной публикации не может наступить после момента отложенного сокрытия")
    {
    }
}