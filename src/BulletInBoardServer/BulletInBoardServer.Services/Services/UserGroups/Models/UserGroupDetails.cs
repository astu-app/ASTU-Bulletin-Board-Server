using BulletInBoardServer.Domain.Models.UserGroups;
using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Services.Services.UserGroups.Models;

/// <summary>
/// Подробная информация о группе пользователей
/// </summary>
public class UserGroupDetails
{
    /// <summary>
    /// Идентификатор группы пользователей
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название группы пользователей
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Администратор группы пользователей
    /// </summary>
    public User? Admin { get; init; }

    /// <summary>
    /// Участники группы пользователей, включая их права
    /// </summary>
    public GroupMemberRights MemberRights { get; init; }

    /// <summary>
    /// Родительские группы пользователей
    /// </summary>
    public UserGroupList ParentGroups { get; init; }

    /// <summary>
    /// Дочерние группы пользователей
    /// </summary>
    public UserGroupList ChildrenGroups { get; init; }

    
    
    /// <summary>
    /// Подробная информация о группе пользователей
    /// </summary>
    /// <param name="id">Идентификатор группы пользователей</param>
    /// <param name="name">Название группы пользователей</param>
    /// <param name="admin">Администратор группы пользователей</param>
    /// <param name="memberRights">Участники группы пользователей, включая их права</param>
    /// <param name="parentGroups">Родительские группы пользователей</param>
    /// <param name="childrenGroups">Дочерние группы пользователей</param>
    public UserGroupDetails(Guid id, string name, User? admin, GroupMemberRights memberRights, UserGroupList parentGroups, UserGroupList childrenGroups)
    {
        Id = id;
        Name = name;
        Admin = admin;
        MemberRights = memberRights;
        ParentGroups = parentGroups;
        ChildrenGroups = childrenGroups;
    }
}