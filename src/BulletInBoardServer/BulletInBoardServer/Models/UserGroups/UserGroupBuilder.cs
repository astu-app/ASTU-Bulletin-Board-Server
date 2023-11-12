using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.UserGroups;

public class UserGroupBuilder
{
    private Guid? _id;
    private string? _name;
    private User? _admin;
    private GroupMemberRights? _memberRights;
    private UserGroups? _childrenGroups;



    public UserGroupBuilder SetId(Guid id)
    {
        _id = id;
        return this;
    }

    public UserGroupBuilder SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название группы пользователей не может быть пустым");

        _name = name;
        return this;
    }

    public UserGroupBuilder SetAdmin(User admin)
    {
        _admin = admin ?? throw new ArgumentNullException(nameof(admin));
        return this;
    }

    public UserGroupBuilder SetMemberRights(GroupMemberRights memberRights)
    {
        _memberRights = memberRights ?? throw new ArgumentNullException(nameof(memberRights));
        return this;
    }

    public UserGroupBuilder SetChildrenGroups(UserGroups childrenGroups)
    {
        _childrenGroups = childrenGroups ?? throw new ArgumentNullException(nameof(childrenGroups));
        return this;
    }

    public UserGroup Build()
    {
        if (string.IsNullOrWhiteSpace(_name))
            throw new ArgumentException("Название группы пользователей должнно быть задано");

        return new UserGroup(
            _id ?? Guid.NewGuid(),
            _name,
            _admin,
            _memberRights ?? new GroupMemberRights(),
            _childrenGroups ?? new UserGroups()
        );
    }
}