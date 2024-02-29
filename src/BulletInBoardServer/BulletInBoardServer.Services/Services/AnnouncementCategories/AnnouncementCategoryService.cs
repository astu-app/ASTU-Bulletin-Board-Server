using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.AnnouncementCategories;
using BulletInBoardServer.Domain.Models.AnnouncementCategories.Exceptions;
using BulletInBoardServer.Services.Services.AnnouncementCategories.Exceptions;
using BulletInBoardServer.Services.Services.AnnouncementCategories.Models;
using Microsoft.EntityFrameworkCore;

namespace BulletInBoardServer.Services.Services.AnnouncementCategories;

public class AnnouncementCategoryService(ApplicationDbContext dbContext)
{
    /// <summary>
    /// Метод создает категорию объявлений
    /// </summary>
    /// <param name="create">Объект, содержащий все необходимые данные для создания категории объявлений</param>
    /// <exception cref="ColorIsInvalidHexException">Цвет категории представлен в формате, отличном от HEX</exception>
    /// <exception cref="AnnouncementCategoryNameIsNullOrWhitespace">Название категории объявлений null, пустое или состоит только из пробельных символов</exception>
    public AnnouncementCategory Create(CreateCategory create)
    {
        var newCategory = new AnnouncementCategory(Guid.NewGuid(), create.Name, create.Color);
        dbContext.AnnouncementCategories.Add(newCategory);
        dbContext.SaveChanges();

        return newCategory;
    }
    
    /// <summary>
    /// Метод возвращает список всех существующих в базе данных категорий объявлений
    /// </summary>
    /// <returns></returns>
    public ICollection<CategorySummary> GetList() => 
        dbContext.AnnouncementCategories
            .Select(ac => new CategorySummary(ac.Id, ac.Name, ac.ColorHex))
            .ToList();

    /// <summary>
    /// Метод редактирует указанную категорию объявлений
    /// </summary>
    /// <exception cref="AnnouncementCategoryDoesNotExistException">Категория объявлений с указанным Id отсутствует в базе</exception>
    /// <exception cref="AnnouncementCategoryNameIsNullOrWhitespace">Название категории объявлений null, пустое или состоит только из пробельных символов</exception>
    /// <exception cref="ColorIsInvalidHexException">Цвет категории объявлений представлен в формате, отличном от HEX</exception>
    public void Edit(EditCategory edit)
    {
        var category = LoadCategorySummary(edit.Id);
        
        if (edit.Name is not null)
            category.SetName(edit.Name);
        if (edit.Color is not null)
            category.SetColor(edit.Color);

        dbContext.SaveChanges();
    }
    
    /// <summary>
    /// Удаление категории объявлений
    /// </summary>
    /// <param name="categoryId">Id удаляемой категории объявлений</param>
    /// <exception cref="AnnouncementCategoryDoesNotExistException">Категория объявлений объявлений с указанным Id отсутствует в БД</exception>
    public void Delete(Guid categoryId)
    {
        var deletedCount = dbContext.AnnouncementCategories
            .Where(ac => ac.Id == categoryId)
            .ExecuteDelete();

        if (deletedCount < 1)
            throw new AnnouncementCategoryDoesNotExistException();
    }

    /// <summary>
    /// Получение списка категории объявлений находящихся среди подписок у пользователя
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию</param>
    public ICollection<CategorySummary> GetSubscriptions(Guid requesterId) =>
        dbContext.AnnouncementCategories
            .Join(dbContext.AnnouncementCategorySubscriptions,
                ac => ac.Id,
                acs => acs.AnnouncementCategoryId,
                (ac, acs) => new { Category = ac, Subscriber = acs })
            .Where(join => join.Subscriber.SubscriberId == requesterId)
            .Select(join => new CategorySummary(join.Category.Id, join.Category.Name, join.Category.ColorHex))
            .ToList();

    /// <summary>
    /// Метод обновляет список категорий объявлений, на который подписан пользователь
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию</param>
    /// <param name="update">Объект, содержащий данные об обновляемых категориях объявлений</param>
    /// <exception cref="AnnouncementCategoryDoesNotExistException">Среди Id категорий присутствуют Id, отсутствующие в БД</exception>
    // public void UpdateSubscriptions(Guid requesterId, ICollection<Guid> changedIds)
    public void UpdateSubscriptions(Guid requesterId, UpdateSubscriptions update)
    {
        var toRemove = update.Update.ToRemove;
        if (toRemove is not null)
        {
            dbContext.AnnouncementCategorySubscriptions
                .Where(aac => aac.SubscriberId == requesterId && toRemove.Contains(aac.SubscriberId))
                .ExecuteDelete();
        }

        var toAdd = update.Update.ToAdd;
        if (toAdd is not null)
        {
            NewIdsValidOrThrow(toAdd);

            /*
             * Пришлось использовать чистый SQL скрипт, так как по причине, которую я так и не смог выяснить,
             * добавление объекта AnnouncementCategorySubscription вызывает ошибку:
             *     The value of 'AnnouncementCategorySubscription.SubscriberId' is unknown when attempting to save changes.
             *     This is because the property is also part of a foreign key for which the principal entity in the
             *     relationship is not known.
             * В других местах, в которых, по моим предположениям, должна возникать такая же ошибка, все работает,
             * как задумано.
             */
            foreach (var newAnnouncementId in toAdd)
                dbContext.Database.ExecuteSql(
                    $"""
                     insert into announcement_categories_subscribers (announcement_category_id, subscriber_id)
                     values ({newAnnouncementId}, {requesterId});
                     """
                );
        }
        
        dbContext.SaveChanges();
    }



    private AnnouncementCategory LoadCategorySummary(Guid categoryId)
    {
        try
        {
            var category = dbContext.AnnouncementCategories.SingleOrDefault(c => c.Id == categoryId);
            if (category is null)
                throw new AnnouncementCategoryDoesNotExistException();
            
            return category;
        }
        catch (InvalidOperationException err) when (err is not AnnouncementCategoryDoesNotExistException)
        {
            throw new InvalidOperationException("Не удалось загрузить категорию объявлений из БД", err);
        }
    }

    private void NewIdsValidOrThrow(ICollection<Guid> newIds)
    {
        var allIdsContainedByDb = dbContext.AnnouncementCategories
            .Select(ac => ac.Id)
            .Intersect(newIds)
            .Count() == newIds.Count;

        if (!allIdsContainedByDb)
            throw new AnnouncementCategoryDoesNotExistException();
    }
}