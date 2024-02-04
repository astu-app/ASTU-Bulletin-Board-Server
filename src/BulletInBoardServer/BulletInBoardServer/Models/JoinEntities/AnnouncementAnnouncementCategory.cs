using BulletInBoardServer.Models.AnnouncementCategories;
using BulletInBoardServer.Models.Announcements;

namespace BulletInBoardServer.Models.JoinEntities;

/// <summary>
/// Сущность для настройки связи многие-ко-многим <see cref="Announcement"/> & <see cref="AnnouncementCategory"/>
/// </summary>
public class AnnouncementAnnouncementCategory
{
    /// <summary>
    /// Идентификатор объявления
    /// </summary>
    public Guid AnnouncementId { get; }

    /// <summary>
    /// Объявление
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework или конструктора.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public Announcement Announcement { get; init; } = null!;
    
    /// <summary>
    /// Идентификатор категории объявлений 
    /// </summary>
    public Guid AnnouncementCategoryId { get; }

    /// <summary>
    /// Категория объявлений
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework или конструктора.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public AnnouncementCategory AnnouncementCategory { get; init; } = null!;
    
    
    
    /// <summary>
    /// Сущность для настройки связи многие-ко-многим <see cref="Announcement"/> & <see cref="AnnouncementCategory"/>
    /// </summary>
    /// <param name="announcementId">Идентификатор объявления</param>
    /// <param name="announcementCategoryId">Идентификатор категории объявлений</param>
    public AnnouncementAnnouncementCategory(Guid announcementId, Guid announcementCategoryId)
    {
        AnnouncementId = announcementId;
        AnnouncementCategoryId = announcementCategoryId;
    }

    /// <summary>
    /// Сущность для настройки связи многие-ко-многим <see cref="Announcement"/> & <see cref="AnnouncementCategory"/>
    /// </summary>
    public AnnouncementAnnouncementCategory()
    {
    }
}