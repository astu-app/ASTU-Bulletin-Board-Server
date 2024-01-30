using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.Users;

public class UserList : Collection<User>
{
    public UserList(IList<User> users) : base(users)
    {
    }

    public UserList()
    {
    }
}