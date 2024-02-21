using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Domain.Models.JoinEntities;

/// <summary>
/// Сущность для настройки связи многие-ко-многим <see cref="Announcement"/> & <see cref="User"/>
/// </summary>
public class AnnouncementAudience
{
    /// <summary>
    /// Идентификатор объявления
    /// </summary>
    public Guid AnnouncementId { get; init; }

    /// <summary>
    /// Объявление
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework или конструктора.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public Announcement Announcement { get; init; } = null!;

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Пользователь
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework или конструктора.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public User User { get; init; } = null!;

    /// <summary>
    /// Просмотрено ли объявление пользователем
    /// </summary>
    public bool Viewed { get; init; }



    /// <summary>
    /// Сущность для настройки связи многие-ко-многим <see cref="Announcement"/> & <see cref="User"/>
    /// </summary>
    /// <param name="announcementId">Идентификатор объявления</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="viewed">Просмотрено ли объявление пользователем</param>
    public AnnouncementAudience(Guid announcementId, Guid userId, bool viewed = false)
    {
        AnnouncementId = announcementId;
        UserId = userId;
        Viewed = viewed;
    }

    public AnnouncementAudience()
    {
    }
}