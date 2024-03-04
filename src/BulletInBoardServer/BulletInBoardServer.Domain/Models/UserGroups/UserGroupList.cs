using System.Collections.ObjectModel;

namespace BulletInBoardServer.Domain.Models.UserGroups;

public class UserGroupList : Collection<UserGroup>
{
    public UserGroupList()
    {
    }

    public UserGroupList(IEnumerable<UserGroup> list) : base(list.ToList())
    {
    }
}