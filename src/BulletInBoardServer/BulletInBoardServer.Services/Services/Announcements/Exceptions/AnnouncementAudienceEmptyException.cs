namespace BulletInBoardServer.Services.Services.Announcements.Exceptions;

public class AnnouncementAudienceEmptyException : ArgumentException
{
    public AnnouncementAudienceEmptyException()
        : base("Аудитория объявления не может быть пустой")
    {
    }
}