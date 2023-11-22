using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;

public class Answers : Collection<AnswerBase>
{
    public Answers(IList<AnswerBase> answers) : base(answers)
    {
    }

    public Answers()
    {
    }
}