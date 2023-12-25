using BulletInBoardServer.Models.Announcements.Categories.Subscribers;
using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Announcements.Categories;

/// <summary>
/// Категория объявлений
/// </summary>
public class AnnouncementCategory
{
    /// <summary>
    /// Категория объявлений
    /// </summary>
    /// <param name="id">Идентификатор категории объявлений</param>
    /// <param name="name">Название категории объявлений</param>
    /// <param name="colorHex">Цвет категории объявлений в пользовательском интерфейсе</param>
    /// <param name="subscribers">Подписчики категории объявлений</param>
    public AnnouncementCategory(Guid id, string name, string colorHex, SubscriberList subscribers)
    {
        Id = id;
        Name = name;
        ColorHex = colorHex;
        Subscribers = subscribers;
    }
    
    /// <summary>
    /// Категория объявлений
    /// </summary>
    /// <param name="id">Идентификатор категории объявлений</param>
    /// <param name="name">Название категории объявлений</param>
    /// <param name="colorHex">Цвет категории объявлений в пользовательском интерфейсе</param>
    /// <param name="subscribers">Подписчики категории объявлений</param>
    public AnnouncementCategory(Guid id, string name, string colorHex)
    {
        Id = id;
        Name = name;
        ColorHex = colorHex;
        // Subscribers = subscribers; // todo подумать
    }

    /// <summary>
    /// Идентификатор категории объявлений
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Название категории объявлений 
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Цвет категории объявлений в пользовательском интерфейсе 
    /// </summary>
    public string ColorHex { get; }

    /// <summary>
    /// Подписчики категории объявлений
    /// </summary>
    public SubscriberList Subscribers { get; }



    public void Subscribe(User user)
    {
        if (Subscribers.Contains(user))
            throw new InvalidOperationException("Пользователь уже подписан на категорию объявлений");
        
        Subscribers.Add(user);
    }
    
    public void Unsubscribe(User user)
    {
        Subscribers.Remove(user);
    }
}