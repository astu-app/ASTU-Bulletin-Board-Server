namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;

/// <summary>
/// Пользователи, проголосовавшие в анонимном опросе
/// </summary>
/// <param name="surveyId">Идентификатор опроса</param>
/// <param name="questionListVoters">Пользователи, проголосовавшие в вопросах опроса</param>
public class AnonymousSurveyVoters(Guid surveyId, ICollection<PrivateQuestionVoters> questionListVoters)
{
    /// <summary>
    /// Идентификатор опроса
    /// </summary>
    public Guid SurveyId { get; } = surveyId;

    /// <summary>
    /// Пользователи, проголосовавшие в вопросах опроса
    /// </summary>
    public ICollection<PrivateQuestionVoters> QuestionListVoters { get; } = questionListVoters;
}