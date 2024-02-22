namespace BulletInBoardServer.Domain.Models.Announcements.Exceptions;

public class AnnouncementNotYetPublishedException : InvalidOperationException
{
    public AnnouncementNotYetPublishedException()
        : base("Нельзя скрыть объявление, которое еще не было опубликовано")
    {
        
    }
}