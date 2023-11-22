using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;

public class Questions : Collection<Question>
{
    public Questions(IList<Question> questions) : base(questions)
    {
    }

    public Questions()
    {
    }
}