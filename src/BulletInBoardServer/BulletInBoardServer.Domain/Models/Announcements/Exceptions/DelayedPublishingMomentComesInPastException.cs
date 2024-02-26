namespace BulletInBoardServer.Domain.Models.Announcements.Exceptions;

public class DelayedPublishingMomentComesInPastException()
    : InvalidOperationException("Момент отложенной публикации не может наступить в прошлом");