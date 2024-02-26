namespace BulletInBoardServer.Services.Services.Announcements.Exceptions;

public class AnnouncementContentEmptyException() : ArgumentException(
    "Текстовое содержимое объявление не может быть пустым или состоять только из пробельных символов");