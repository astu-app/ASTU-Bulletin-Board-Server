namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;

public class AnonymousAnswer(Guid id, string content) : AnswerBase(id, content)
{
    public override int VotersCount => _votersCount;
    
    private int _votersCount;



    public void Vote() =>
        ++_votersCount;
}