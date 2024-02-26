namespace BulletInBoardServer.Domain.Models.Announcements.Exceptions;

public class AnnouncementNotYetPublishedException()
    : InvalidOperationException("Нельзя скрыть объявление, которое еще не было опубликовано");