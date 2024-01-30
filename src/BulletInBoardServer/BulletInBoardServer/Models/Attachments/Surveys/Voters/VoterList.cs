using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Attachments.Surveys.Voters;

public class VoterList : UserList
{
    public VoterList(IList<User> users) : base(users)
    {
    }

    public VoterList()
    {
    }
}