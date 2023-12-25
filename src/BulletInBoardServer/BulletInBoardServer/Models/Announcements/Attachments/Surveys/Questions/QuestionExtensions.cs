namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;

public static class QuestionExtensions
{
    public static ReadOnlyQuestionList AsReadOnly(this QuestionList questions) =>
        new (questions.ToList());

    public static QuestionList ToQuestions(this IEnumerable<Question> questions) =>
        new (questions.ToList());
}