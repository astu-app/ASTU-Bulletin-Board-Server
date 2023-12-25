using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.UserGroups;

// todo продумать права пользователей
/// <summary>
/// Права конкретного участника группы пользователей
/// </summary>
public class SingleMemberRights(Guid userId, Guid userGroupId)
{
    /// <summary>
    /// Идентификатор участника группы пользователей, к которому относятся права
    /// </summary>
    public Guid UserId { get; } = userId;

    /// <summary>
    /// Участник группы пользователей, к которому относятся права
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public User User { get; set; } = null!;

    /// <summary>
    /// Идентификатор группы пользователей, в рамках которой существуют права
    /// </summary>
    public Guid UserGroupId { get; } = userGroupId;

    /// <summary>
    /// Группа пользователей, в рамках которой существуют права
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public UserGroup UserGroup { get; set; } = null!;
}