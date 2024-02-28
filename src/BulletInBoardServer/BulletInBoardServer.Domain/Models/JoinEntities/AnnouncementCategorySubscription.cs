using BulletInBoardServer.Domain.Models.AnnouncementCategories;
using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Domain.Models.JoinEntities;

/// <summary>
/// Сущность для настройки связи многие-ко-многим <see cref="AnnouncementCategory"/> & <see cref="Users.User"/>
/// </summary>
/// <param name="announcementCategoryId">Идентификатор категории объявлений</param>
/// <param name="subscriberId">Идентификатор подписчика</param>
public class AnnouncementCategorySubscription(Guid announcementCategoryId, Guid subscriberId)
{
    /// <summary>
    /// Идентификатор категории объявлений 
    /// </summary>
    public Guid AnnouncementCategoryId { get; } = announcementCategoryId;

    /// <summary>
    /// Категория объявлений
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework или конструктора.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public AnnouncementCategory AnnouncementCategory { get; init; } = null!;

    /// <summary>
    /// Идентификатор подписчика
    /// </summary>
    public Guid SubscriberId { get; } = subscriberId;

    /// <summary>
    /// Подписчик
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework или конструктора.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public User Subscriber { get; init; } = null!;
}