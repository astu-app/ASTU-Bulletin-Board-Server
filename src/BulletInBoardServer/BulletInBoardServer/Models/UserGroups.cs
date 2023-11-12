using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models;

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