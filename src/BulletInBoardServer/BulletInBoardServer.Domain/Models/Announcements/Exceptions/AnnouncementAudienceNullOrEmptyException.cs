namespace BulletInBoardServer.Domain.Models.Announcements.Exceptions;

public class AnnouncementAudienceNullOrEmptyException : ArgumentException
{
    public AnnouncementAudienceNullOrEmptyException()
        : base("Аудитория объявления не может быть null или пустой")
    {
    }
}