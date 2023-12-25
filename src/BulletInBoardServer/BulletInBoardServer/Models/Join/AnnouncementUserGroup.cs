using BulletInBoardServer.Models.Announcements;
using BulletInBoardServer.Models.UserGroups;

namespace BulletInBoardServer.Models.Join;

/// <summary>
/// Сущность для настройки связи многие-ко-многим <see cref="Announcement"/> & <see cref="UserGroup"/>
/// </summary>
/// <param name="announcementId">Идентификатор объявления</param>
/// <param name="userGroupId">Идентификатор группы пользователей</param>
public class AnnouncementUserGroup(Guid announcementId, Guid userGroupId)
{
    /// <summary>
    /// Идентификатор объявления
    /// </summary>
    public Guid AnnouncementId { get; } = announcementId;

    /// <summary>
    /// Объявление
    /// </summary>
    /// <remarks>
    /// Список вариантов ответов должен содержать ровно один элемент в случае,
    /// если в опросе запрещен множественный выбор, и не меньше в противном случае
    /// </remarks>
    public Announcement Announcement { get; set; } = null!;

    /// <summary>
    /// Идентификатор группы пользователей
    /// </summary>
    public Guid UserGroupId { get; } = userGroupId;

    /// <summary>
    /// Группа пользователей
    /// </summary>
    /// <remarks>
    /// Список вариантов ответов должен содержать ровно один элемент в случае,
    /// если в опросе запрещен множественный выбор, и не меньше в противном случае
    /// </remarks>
    public UserGroup UserGroup { get; set; } = null!;
}