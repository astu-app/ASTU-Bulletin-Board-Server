using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.UserGroups;

public class UserGroup
{
    public Guid Id { get; }
    public string Name { get; set; }

    public User? Admin { get; set; }

    public GroupMemberRights MemberRights { get; }

    public UserGroups ChildrenGroups { get; }



    public UserGroup(Guid id, string name, User? admin, GroupMemberRights memberRights, UserGroups childrenGroups)
    {
        Id = id;
        Name = name;
        Admin = admin;
        MemberRights = memberRights;
        ChildrenGroups = childrenGroups;
    }
}