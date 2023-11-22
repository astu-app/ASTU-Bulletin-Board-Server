namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;

public static class AnswersExtensions
{
    public static ReadOnlyAnswers AsReadOnly(this Answers answers) =>
        new (answers);
}