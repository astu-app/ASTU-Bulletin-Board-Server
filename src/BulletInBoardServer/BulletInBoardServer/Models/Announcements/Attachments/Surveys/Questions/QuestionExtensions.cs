namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;

public static class QuestionExtensions
{
    public static ReadOnlyQuestions AsReadOnly(this Questions questions) =>
        new (questions);
}