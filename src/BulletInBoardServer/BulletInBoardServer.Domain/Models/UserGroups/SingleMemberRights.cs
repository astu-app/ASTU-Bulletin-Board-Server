using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Domain.Models.UserGroups;

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
    /// Поле должно устанавливаться только при помощи Entity Framework или конструктора.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public User User { get; init; } = null!;

    /// <summary>
    /// Идентификатор группы пользователей, в рамках которой существуют права
    /// </summary>
    public Guid UserGroupId { get; set; }

    /// <summary>
    /// Группа пользователей, в рамках которой существуют права
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework или конструктора.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public UserGroup UserGroup { get; init; } = null!;

    /* ************************************** Объявления ************************************** */
    /// <summary>
    /// Может ли пользователь создавать объявления
    /// </summary>
    public bool CanCreateAnnouncements { get; set; }

    /* ************************************** Опросы************************************** */
    /// <summary>
    /// Может ли пользователь создавать опросы
    /// </summary>
    public bool CanCreateSurveys { get; set; }

    /* ************************************** Группы пользователей ************************************** */
    /// <summary>
    /// Может ли пользователь управлять иерархией группы пользователей
    /// </summary>
    public bool CanRuleUserGroupHierarchy { get; set; }

    /// <summary>
    /// Может ли пользователь просматривать подробности своей группы пользователей и групп из подчиненной иерархии.
    /// </summary>
    public bool CanViewUserGroupDetails { get; set; }

    /// <summary>
    /// Может ли пользователь создавать группы пользователей и встраивать их в подчиненную иерархию
    /// </summary>
    public bool CanCreateUserGroups { get; set; }

    /// <summary>
    /// Может ли пользователь редактировать свою группу пользователей и групп из подчиненной иерархии
    /// </summary>
    public bool CanEditUserGroups { get; set; }

    /// <summary>
    /// Может ли пользователь изменять состав участников своей группы пользователей и групп из подчиненной иерархии
    /// </summary>
    public bool CanEditMembers { get; set; }

    /// <summary>
    /// Может ли пользователь редактировать права участников своей группы пользователей и групп из подчиненной иерархии
    /// </summary>
    public bool CanEditMemberRights { get; set; }

    /// <summary>
    /// Может ли пользователь редактировать администратора групп из подчиненной иерархии
    /// </summary>
    public bool CanEditUserGroupAdmin { get; set; }

    /// <summary>
    /// Может ли пользователь удалять группы пользователей подчиненной иерархии
    /// </summary>
    public bool CanDeleteUserGroup { get; set; }



    /// <summary>
    /// Права конкретного участника группы пользователей
    /// </summary>
    public SingleMemberRights(User user, UserGroup userGroup,
        bool canCreateAnnouncements = false, bool canCreateSurveys = false, bool canRuleUserGroupHierarchy = false, 
        bool canViewUserGroupDetails = false, bool canCreateUserGroups = false, bool canEditUserGroups = false, 
        bool canEditMembers = false, bool canEditMemberRights = false, bool canEditUserGroupAdmin = false, 
        bool canDeleteUserGroup = false)
        : this(user.Id, userGroup.Id, canCreateAnnouncements, canCreateSurveys, canRuleUserGroupHierarchy,
            canViewUserGroupDetails, canCreateUserGroups, canEditUserGroups, canEditMembers, canEditMemberRights,
            canEditUserGroupAdmin, canDeleteUserGroup)
    {
        User = user;
        UserGroup = userGroup;
    }

    /// <summary>
    /// Права конкретного участника группы пользователей
    /// </summary>
    public SingleMemberRights(Guid userId, Guid userGroupId, bool canCreateAnnouncements = false, 
        bool canCreateSurveys = false, bool canRuleUserGroupHierarchy = false, bool canViewUserGroupDetails = false, 
        bool canCreateUserGroups = false, bool canEditUserGroups = false, bool canEditMembers = false, 
        bool canEditMemberRights = false, bool canEditUserGroupAdmin = false, bool canDeleteUserGroup = false)
    {
        UserId = userId;
        UserGroupId = userGroupId;
        
        CanCreateAnnouncements = canCreateAnnouncements;
        CanCreateSurveys = canCreateSurveys;
        CanRuleUserGroupHierarchy = canRuleUserGroupHierarchy;
        CanViewUserGroupDetails = canViewUserGroupDetails;
        CanCreateUserGroups = canCreateUserGroups;
        CanEditUserGroups = canEditUserGroups;
        CanEditMembers = canEditMembers;
        CanEditMemberRights = canEditMemberRights;
        CanEditUserGroupAdmin = canEditUserGroupAdmin;
        CanDeleteUserGroup = canDeleteUserGroup;
    }
}