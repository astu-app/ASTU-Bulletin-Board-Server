namespace BulletInBoardServer.Models.UserGroups;

public class UserGroupBuilder
{
    private Guid? _id;
    private string? _name;
    private Guid? _adminId;
    private GroupMemberRights? _memberRights;
    private UserGroupList? _childrenGroups;



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

    public UserGroupBuilder SetAdmin(Guid? adminId)
    {
        _adminId = adminId;
        return this;
    }

    public UserGroupBuilder SetMemberRights(GroupMemberRights memberRights)
    {
        _memberRights = memberRights ?? throw new ArgumentNullException(nameof(memberRights));
        return this;
    }

    public UserGroupBuilder SetChildrenGroups(UserGroupList childrenGroups)
    {
        _childrenGroups = childrenGroups ?? throw new ArgumentNullException(nameof(childrenGroups));
        return this;
    }

    public UserGroup Build()
    {
        if (string.IsNullOrWhiteSpace(_name))
            throw new ArgumentException("Название группы пользователей должно быть задано");

        return new UserGroup(
            _id ?? Guid.NewGuid(),
            _name,
            _adminId,
            _memberRights ?? new GroupMemberRights(),
            _childrenGroups ?? []
        );
    }
}