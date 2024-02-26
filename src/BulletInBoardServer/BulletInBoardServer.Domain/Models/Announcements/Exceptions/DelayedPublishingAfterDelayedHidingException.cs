namespace BulletInBoardServer.Domain.Models.Announcements.Exceptions;

public class DelayedPublishingAfterDelayedHidingException()
    : InvalidOperationException(
        "Момент отложенной публикации не может наступить после момента отложенного сокрытия");