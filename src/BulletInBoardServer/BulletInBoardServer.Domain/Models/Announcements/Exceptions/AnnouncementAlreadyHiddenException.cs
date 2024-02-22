namespace BulletInBoardServer.Domain.Models.Announcements.Exceptions;

public class AnnouncementAlreadyHiddenException : InvalidOperationException
{
    public AnnouncementAlreadyHiddenException() 
        : base("Нельзя скрыть уже скрытое объявление")
    {
    }
}