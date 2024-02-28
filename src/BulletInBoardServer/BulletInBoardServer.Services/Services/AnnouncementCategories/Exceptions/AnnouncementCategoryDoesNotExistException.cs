namespace BulletInBoardServer.Services.Services.AnnouncementCategories.Exceptions;

public class AnnouncementCategoryDoesNotExistException(Exception? internalException = null)
    : InvalidOperationException("Категория объявлений не найдена", internalException);