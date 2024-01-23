using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.UserGroups;

// todo продумать права пользователей
/// <summary>
/// Права конкретного участника группы пользователей
/// </summary>
public class SingleMemberRights
{
    /// <summary>
    /// Идентификатор участника группы пользователей, к которому относятся права
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Участник группы пользователей, к которому относятся права
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public User User { get; init; } = null!;

    /// <summary>
    /// Идентификатор группы пользователей, в рамках которой существуют права
    /// </summary>
    public Guid UserGroupId { get; init; }

    /// <summary>
    /// Группа пользователей, в рамках которой существуют права
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public UserGroup UserGroup { get; init; } = null!;



    /// <summary>
    /// Права конкретного участника группы пользователей
    /// </summary>
    public SingleMemberRights(User user, UserGroup userGroup) 
        : this (user.Id, userGroup.Id)
    {
        User = user;
        UserGroup = userGroup;
    }

    /// <summary>
    /// Права конкретного участника группы пользователей
    /// </summary>
    public SingleMemberRights(Guid userId, Guid userGroupId)
    {
        UserId = userId;
        UserGroupId = userGroupId;
    }
}