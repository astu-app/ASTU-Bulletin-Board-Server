namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;

public class AnonymousAnswerBase : AnswerBase
{
    public override int VotersCount => _votersCount;
    
    private int _votersCount;



    public AnonymousAnswerBase(Guid id, string content) : base(id, content)
    {
    }



    public void Vote() =>
        ++_votersCount;
}