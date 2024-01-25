using BulletInBoardServer.Models.Announcements;
using BulletInBoardServer.Models.UserGroups;

namespace BulletInBoardServer.Models.JoinEntities;

/// <summary>
/// Сущность для настройки связи многие-ко-многим <see cref="Announcement"/> & <see cref="UserGroup"/>
/// </summary>
public class AnnouncementUserGroup
{
    /// <summary>
    /// Идентификатор объявления
    /// </summary>
    public Guid AnnouncementId { get; set; }

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
    public Guid UserGroupId { get; set; }

    /// <summary>
    /// Группа пользователей
    /// </summary>
    /// <remarks>
    /// Список вариантов ответов должен содержать ровно один элемент в случае,
    /// если в опросе запрещен множественный выбор, и не меньше в противном случае
    /// </remarks>
    public UserGroup UserGroup { get; set; } = null!;
    
    
    
    /// <summary>
    /// Сущность для настройки связи многие-ко-многим <see cref="Announcement"/> & <see cref="UserGroup"/>
    /// </summary>
    /// <param name="announcementId">Идентификатор объявления</param>
    /// <param name="userGroupId">Идентификатор группы пользователей</param>
    public AnnouncementUserGroup(Guid announcementId, Guid userGroupId)
    {
        AnnouncementId = announcementId;
        UserGroupId = userGroupId;
    }

    public AnnouncementUserGroup()
    {
        
    }
}