namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;

/// <summary>
/// Пользователи, проголосовавшие в анонимном вопросе
/// </summary>
/// <param name="questionId">Идентификатор вопроса</param>
/// <param name="voters">Проголосовавшие в вопросе пользователи</param>
public class PrivateQuestionVoters(Guid questionId, VoterList voters)
{
    /// <summary>
    /// Идентификатор вопроса
    /// </summary>
    public Guid QuestionId { get; } = questionId;

    /// <summary>
    /// Проголосовавшие в вопросе пользователи
    /// </summary>
    public VoterList Voters { get; } = voters;
}