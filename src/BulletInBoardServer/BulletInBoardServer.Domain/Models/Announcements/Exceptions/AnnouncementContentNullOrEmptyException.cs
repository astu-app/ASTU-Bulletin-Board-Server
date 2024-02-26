namespace BulletInBoardServer.Domain.Models.Announcements.Exceptions;

public class AnnouncementContentNullOrEmptyException() : ArgumentException(
    "Текстовое содержимое объявления не может быть null, пустым или состоять только из пробельных символов");