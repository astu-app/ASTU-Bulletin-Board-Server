using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.Users;

public class Users : Collection<User>
{
    public Users(IList<User> users) : base(users)
    {
    }

    public Users()
    {
    }
}