using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Domain.Models.Attachments.Surveys.Voters;

public class VoterList : UserList
{
    public VoterList(IList<User> users) : base(users)
    {
    }

    public VoterList()
    {
    }
}