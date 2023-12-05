using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.UserGroups;

public class UserGroup(Guid id, string name, User? admin, GroupMemberRights memberRights,
    UserGroups childrenGroups)
{
    public Guid Id { get; } = id;
    public string Name { get; set; } = name;

    public User? Admin { get; set; } = admin;

    public GroupMemberRights MemberRights { get; } = memberRights;

    public UserGroups ChildrenGroups { get; } = childrenGroups;
}