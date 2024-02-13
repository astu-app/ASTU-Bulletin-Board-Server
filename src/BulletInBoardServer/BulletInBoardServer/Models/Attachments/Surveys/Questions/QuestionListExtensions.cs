namespace BulletInBoardServer.Models.Attachments.Surveys.Questions;

public static class QuestionListExtensions
{
    public static QuestionList AsQuestionList(this IEnumerable<Question> questions) =>
        new(questions.ToList());
}