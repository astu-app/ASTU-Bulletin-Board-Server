namespace BulletInBoardServer.Services.Services.AnnouncementCategories.Exceptions;

public class AnnouncementCategoriesDoNotExist : InvalidOperationException
{
    public AnnouncementCategoriesDoNotExist(Exception? internalException)
        : base("Категория объявлений не найдена", internalException)
    {
        
    }
}