using System.Collections.ObjectModel;

namespace BulletInBoardServer.Domain.Models.Attachments.Surveys.Questions;

public class QuestionList : Collection<Question>
{
    public QuestionList(IList<Question> questions) : base(questions)
    {
    }

    public QuestionList()
    {
    }
}