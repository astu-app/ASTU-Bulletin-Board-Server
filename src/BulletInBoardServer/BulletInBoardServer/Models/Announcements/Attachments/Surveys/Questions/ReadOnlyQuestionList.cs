using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;

public class ReadOnlyQuestionList(IList<Question> list) : ReadOnlyCollection<Question>(list);