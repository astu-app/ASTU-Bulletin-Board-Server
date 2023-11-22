using System.Collections.ObjectModel;
using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;

public class Voters : Collection<User>
{
    public Voters(IList<User> users) : base(users)
    {
    }

    public Voters()
    {
    }
}