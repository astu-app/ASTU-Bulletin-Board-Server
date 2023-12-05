using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;

public class ReadOnlyQuestions(IList<Question> list) : ReadOnlyCollection<Question>(list);