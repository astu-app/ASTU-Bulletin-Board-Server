using BulletInBoardServer.Domain.Models.UserGroups.Exceptions;
using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Domain.Models.UserGroups;

/// <summary>
/// Группа пользователей доски объявлений
/// </summary>
public class UserGroup
{
    /// <summary>
    /// Идентификатор группы пользователей
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название группы пользователей
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Идентификатор администратора группы пользователей
    /// </summary>
    public Guid? AdminId { get; set; }

    /// <summary>
    /// Администратор группы пользователей
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework или конструктора.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public User? Admin { get; set; }

    /// <summary>
    /// Права участника группы пользователей
    /// </summary>
    public GroupMemberRights MemberRights { get; init; }

    /// <summary>
    /// Дочерние группы пользователей
    /// </summary>
    public UserGroupList ChildrenGroups { get; init; }



    /// <summary>
    /// Группа пользователей доски объявлений
    /// </summary>
    /// <param name="id">Идентификатор группы пользователей</param>
    /// <param name="name">Название группы пользователей</param>
    /// <param name="adminId">Идентификатор администратора группы пользователей</param>
    /// <exception cref="UserGroupNameEmptyException">Название группы пользователей пустое</exception>">
    public UserGroup(Guid id, string name, Guid? adminId)
        : this(id, name, adminId, [], [])
    {
    }

    /// <summary>
    /// Группа пользователей доски объявлений
    /// </summary>
    /// <param name="id">Идентификатор группы пользователей</param>
    /// <param name="name">Название группы пользователей</param>
    /// <param name="adminId">Идентификатор администратора группы пользователей</param>
    /// <param name="memberRights">Права участника группы пользователей</param>
    /// <param name="childrenGroups">Дочерние группы пользователей</param>
    /// <exception cref="UserGroupNameEmptyException">Название группы пользователей пустое</exception>">
    public UserGroup(Guid id, string name, Guid? adminId, GroupMemberRights memberRights,
        UserGroupList childrenGroups)
    {
        NameValidOrThrow(name);

        Id = id;
        Name = name;
        AdminId = adminId;
        MemberRights = memberRights;
        ChildrenGroups = childrenGroups;
    }



    public void SetName(string name)
    {
        NameValidOrThrow(name);
        Name = name;
    }



    private static void NameValidOrThrow(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new UserGroupNameEmptyException();
    }
}