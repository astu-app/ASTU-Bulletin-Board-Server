using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;
using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;

public class Answer(Guid id, string content, Voters.Voters voters) : AnswerBase(id, content)
{
    public ReadOnlyVoters Voters => voters.AsReadOnly();

    public override int VotersCount => Voters.Count;



    public void Vote(User voter)
    {
        if (voter is null)
            throw new ArgumentNullException(nameof(voter));
        
        voters.Add(voter);
    }
}