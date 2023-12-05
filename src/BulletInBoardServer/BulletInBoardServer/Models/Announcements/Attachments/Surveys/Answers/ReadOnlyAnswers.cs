using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;

public class ReadOnlyAnswers(IList<AnswerBase> list) : ReadOnlyCollection<AnswerBase>(list);