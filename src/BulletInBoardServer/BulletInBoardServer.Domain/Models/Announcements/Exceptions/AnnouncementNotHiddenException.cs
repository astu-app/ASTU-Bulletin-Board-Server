namespace BulletInBoardServer.Domain.Models.Announcements.Exceptions;

public class AnnouncementNotHiddenException()
    : InvalidOperationException("Нельзя восстановить объявление, не являющееся скрытым");