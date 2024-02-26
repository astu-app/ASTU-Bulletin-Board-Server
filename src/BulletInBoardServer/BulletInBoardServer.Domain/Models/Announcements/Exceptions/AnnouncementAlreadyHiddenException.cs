namespace BulletInBoardServer.Domain.Models.Announcements.Exceptions;

public class AnnouncementAlreadyHiddenException()
    : InvalidOperationException("Нельзя скрыть уже скрытое объявление");