namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;

public static class QuestionExtensions
{
    public static QuestionList ToQuestions(this IEnumerable<Question> questions) =>
        new (questions.ToList());
}