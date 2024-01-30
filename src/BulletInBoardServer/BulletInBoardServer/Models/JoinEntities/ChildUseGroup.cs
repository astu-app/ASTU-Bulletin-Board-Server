using BulletInBoardServer.Models.UserGroups;

namespace BulletInBoardServer.Models.JoinEntities;

/// <summary>
/// Сущность для настройки связи многие-ко-многим UserGroup 
/// </summary>
/// <param name="userGroupId">Идентификатор рассматриваемой группы пользователей</param>
/// <param name="childUserGroupId">Идентификатор дочерней группы пользователей</param>
public class ChildUseGroup(Guid userGroupId, Guid childUserGroupId)
{
    /// <summary>
    /// Идентификатор рассматриваемой группы пользователей
    /// </summary>
    public Guid UserGroupId { get; } = userGroupId;

    /// <summary>
    /// Рассматриваемая группа пользователей
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework или конструктора.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public UserGroup UserGroup { get; set; } = null!;
    
    /// <summary>
    /// Идентификатор дочерней группы пользователей
    /// </summary>
    public Guid ChildUserGroupId { get; } = childUserGroupId;

    /// <summary>
    /// Дочерняя группа пользователей
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework или конструктора.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public UserGroup ChildUserGroup { get; set; } = null!;
}