using System.Collections.ObjectModel;
using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Attachments.Surveys.Voters;

public class VoterList : Collection<User>
{
    public VoterList(IList<User> users) : base(users)
    {
    }

    public VoterList()
    {
    }
}