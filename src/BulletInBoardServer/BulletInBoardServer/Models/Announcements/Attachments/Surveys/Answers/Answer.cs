using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;
using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;

public class Answer : AnswerBase
{
    public ReadOnlyVoters Voters => _voters.AsReadOnly();

    public override int VotersCount => Voters.Count;



    private readonly Voters.Voters _voters;



    public Answer(Guid id, string content, Voters.Voters voters) : base(id, content)
    {
        _voters = voters;
    }



    public void Vote(User voter)
    {
        if (voter is null)
            throw new ArgumentNullException(nameof(voter));
        
        _voters.Add(voter);
    }
}