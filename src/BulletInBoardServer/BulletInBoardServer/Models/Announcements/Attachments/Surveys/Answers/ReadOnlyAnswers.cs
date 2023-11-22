using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;

public class ReadOnlyAnswers : ReadOnlyCollection<AnswerBase>
{
    public ReadOnlyAnswers(IList<AnswerBase> list) : base(list)
    {
    }
}