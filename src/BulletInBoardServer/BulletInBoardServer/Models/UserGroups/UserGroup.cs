using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.UserGroups;

/// <summary>
/// Группа пользователей доски объявлений
/// </summary>
public class UserGroup
{
    /// <summary>
    /// Группа пользователей доски объявлений
    /// </summary>
    /// <param name="id">Идентификатор группы пользователей</param>
    /// <param name="name">Название группы пользователей</param>
    /// <param name="adminId">Идентификатор администратора группы пользователей</param>
    /// <param name="memberRights">Права участника группы пользователей</param>
    /// <param name="childrenGroups">Дочерние группы пользователей</param>
    public UserGroup(Guid id, string name, Guid? adminId, GroupMemberRights memberRights,
        UserGroupList childrenGroups)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название группы пользователей не может быть пустым");
        
        Id = id;
        Name = name;
        AdminId = adminId;
        MemberRights = memberRights;
        ChildrenGroups = childrenGroups;
    }

    public UserGroup(Guid id, string name, Guid? adminId)
    { // debug
        Id = id;
        Name = name;
        AdminId = adminId;
        // MemberRights = memberRights;
        // ChildrenGroups = childrenGroups;
    }

    /// <summary>
    /// Идентификатор группы пользователей
    /// </summary>
    public Guid Id { get; }
    
    /// <summary>
    /// Название группы пользователей
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Идентификатор администратора группы пользователей
    /// </summary>
    public Guid? AdminId { get; }
    
    /// <summary>
    /// Администратор группы пользователей
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public User? Admin { get; set; }

    /// <summary>
    /// Права участника группы пользователей
    /// </summary>
    public GroupMemberRights MemberRights { get; }

    /// <summary>
    /// Дочерние группы пользователей
    /// </summary>
    public UserGroupList ChildrenGroups { get; }
}