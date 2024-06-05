using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Services.Services.UserGroups.Models;

/// <summary>
/// Класс с данными для редактирования группы пользователей
/// </summary>
public class UpdateUserGroupContent
{
    /// <summary>
    /// Редактируемая группа пользователей/
    /// </summary>
    public UserGroupDetails UserGroup { get; set; }

    /// <summary>
    /// Потенциальные участники или администратор группы поль/зователей. В списке отсутствует текущий администратор и участники
    /// </summary>
    public UserList PotentialMembers { get; set; }

    // /// <summary>
    // /// Потенциальные непосредственные родители или потомки группы пользователей/
    // /// </summary>
    // public UserGroupList PotentialRelatives { get; set; } // remove



    /// <summary>
    /// Класс с данными для редактирования группы пользователей
    /// </summary>
    /// <param name="userGroup">Редактируемая группа пользователей/</param>
    /// <param name="potentialMembers">Потенциальные участники или администратор группы поль/зователей. В списке отсутствует текущий администратор и участники</param>
    // /// <param name="potentialRelatives">Потенциальные непосредственные родители или потомки группы пользователей/</param>
    public UpdateUserGroupContent(UserGroupDetails userGroup, UserList potentialMembers/*, UserGroupList potentialRelatives */) // remove
    {
        UserGroup = userGroup;
        PotentialMembers = potentialMembers;
        // PotentialRelatives = potentialRelatives; // remove
    }
}