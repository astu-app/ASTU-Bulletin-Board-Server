using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.UserGroups;

public class UserGroupList : Collection<UserGroup>
{
    public UserGroupList(IList<UserGroup> userGroups) 
        : base(userGroups)
    {
    }

    public UserGroupList()
    {
    }
}