using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;

public class QuestionList : Collection<Question>
{
    public QuestionList(IList<Question> questions) : base(questions)
    {
    }

    public QuestionList()
    {
    }
}