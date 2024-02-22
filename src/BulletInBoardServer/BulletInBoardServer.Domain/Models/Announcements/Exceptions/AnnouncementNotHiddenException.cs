namespace BulletInBoardServer.Domain.Models.Announcements.Exceptions;

public class AnnouncementNotHiddenException : InvalidOperationException
{
    public AnnouncementNotHiddenException()
        : base("Нельзя восстановить объявление, не являющееся скрытым")
    {
    }
}