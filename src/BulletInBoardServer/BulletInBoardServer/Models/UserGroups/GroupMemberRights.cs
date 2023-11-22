using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.UserGroups;

/// <summary>
/// Права всех участников группы пользователей
/// </summary>
public class GroupMemberRights : Dictionary<User, SingleMemberRights>
{
    
}