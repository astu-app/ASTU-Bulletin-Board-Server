using BulletInBoardServer.Domain.Models.UserGroups;
using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Services.Services.UserGroups.Models;

public class UserGroupDetails
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public User? Admin { get; set; }
    public GroupMemberRights MemberRights { get; set; }
    
    public UserGroupList ChildrenGroups { get; init; }
    public UserGroupList ParentGroups { get; init; }
}