namespace BulletInBoardServer.Domain.Models.Announcements.Exceptions;

public class DelayedHidingMomentComesInPastException()
    : InvalidOperationException("Момент отложенного сокрытия не может наступить в прошлом");