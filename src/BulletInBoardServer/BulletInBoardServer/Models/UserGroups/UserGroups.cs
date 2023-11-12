using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.UserGroups;

public class UserGroups : Collection<UserGroup>
{
    public UserGroups(IList<UserGroup> userGroups) 
        : base(userGroups)
    {
    }

    public UserGroups()
    {
    }
}