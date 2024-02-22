namespace BulletInBoardServer.Services.Services.Announcements.Exceptions;

public class DelayedHidingMomentComesInPastException : InvalidOperationException
{
    public DelayedHidingMomentComesInPastException()
        : base("Момент отложенного сокрытия не может наступить в прошлом")
    {
    }
}