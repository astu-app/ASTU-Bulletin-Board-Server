using System.Collections.ObjectModel;

namespace BulletInBoardServer.Domain.Models.Attachments.Surveys.Answers;

public class AnswerList : Collection<Answer>
{
    public AnswerList(IList<Answer> answers) : base(answers)
    {
    }

    public AnswerList()
    {
    }
}