namespace BulletInBoardServer.Domain.Models.AnnouncementCategories.Exceptions;

public class AnnouncementCategoryNameIsNullOrWhitespace() : ArgumentException(
    "Название категории объявлений не может быть null, пустым или состоять только из пробельных символов");