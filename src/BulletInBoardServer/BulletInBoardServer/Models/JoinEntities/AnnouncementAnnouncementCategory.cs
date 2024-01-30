using BulletInBoardServer.Models.AnnouncementCategories;
using BulletInBoardServer.Models.Announcements;

namespace BulletInBoardServer.Models.JoinEntities;

/// <summary>
/// Сущность для настройки связи многие-ко-многим <see cref="Announcement"/> & <see cref="AnnouncementCategory"/>
/// </summary>
/// <param name="announcementId">Идентификатор объявления</param>
/// <param name="announcementCategoryId">Идентификатор категории объявлений</param>
public class AnnouncementAnnouncementCategory(Guid announcementId, Guid announcementCategoryId)
{
    /// <summary>
    /// Идентификатор объявления
    /// </summary>
    public Guid AnnouncementId { get; } = announcementId;

    /// <summary>
    /// Объявление
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework или конструктора.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public Announcement Announcement { get; set; } = null!;
    
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
    public AnnouncementCategory AnnouncementCategory { get; set; } = null!;
}