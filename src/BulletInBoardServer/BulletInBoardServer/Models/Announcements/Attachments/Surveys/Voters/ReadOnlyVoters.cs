using System.Collections.ObjectModel;
using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;

public class ReadOnlyVoters : ReadOnlyCollection<User>
{
    public ReadOnlyVoters(IList<User> list) : base(list)
    {
    }
}