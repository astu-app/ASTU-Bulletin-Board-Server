namespace BulletInBoardServer.Domain.Models.Announcements.Exceptions;

public class AnnouncementAudienceNullOrEmptyException()
    : ArgumentException("Аудитория объявления не может быть null или пустой");