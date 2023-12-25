using BulletInBoardServer.Models.Announcements.Categories;
using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Join;

/// <summary>
/// Сущность для настройки связи многие-ко-многим <see cref="AnnouncementCategory"/> & <see cref="User"/>
/// </summary>
/// <param name="announcementCategoryId">Идентификатор категории объявлений</param>
/// <param name="subscriberId">Идентификатор подписчика</param>
public class AnnouncementCategorySubscriber(Guid announcementCategoryId, Guid subscriberId)
{
    /// <summary>
    /// Идентификатор категории объявлений 
    /// </summary>
    public Guid AnnouncementCategoryId { get; } = announcementCategoryId;

    /// <summary>
    /// Категория объявлений
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public AnnouncementCategory AnnouncementCategory { get; set; } = null!;
    
    /// <summary>
    /// Идентификатор подписчика
    /// </summary>
    public Guid SubscriberId { get; } = subscriberId;

    /// <summary>
    /// Подписчик
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public User Subscriber { get; set; } = null!;
}