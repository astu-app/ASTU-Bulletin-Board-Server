using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;

public class ReadOnlyQuestions : ReadOnlyCollection<Question>
{
    public ReadOnlyQuestions(IList<Question> list) : base(list)
    {
    }
}