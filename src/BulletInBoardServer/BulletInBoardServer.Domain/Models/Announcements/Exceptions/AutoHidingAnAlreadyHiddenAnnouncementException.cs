namespace BulletInBoardServer.Domain.Models.Announcements.Exceptions;

public class AutoHidingAnAlreadyHiddenAnnouncementException : InvalidOperationException
{
    public AutoHidingAnAlreadyHiddenAnnouncementException()
        : base("Нельзя задать срок автоматического сокрытия уже скрытому объявлению")
    {
    }
}