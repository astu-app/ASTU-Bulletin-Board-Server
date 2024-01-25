using BulletInBoardServer.Models.Attachments.Surveys.Answers;
using BulletInBoardServer.Models.Attachments.Surveys.SurveyParticipation;

namespace BulletInBoardServer.Models.Attachments.Surveys;

/// <summary>
/// Выбор пользователя в открытом опросе. Содержит связку Пользователь - Вариант ответа
/// </summary>
public class UserSelection
{
    /// <summary>
    /// Идентификатор участия, с которы связан выбор пользователя
    /// </summary>
    public Guid ParticipationId { get; init; }

    /// <summary>
    /// Участие, с которым связан выбор пользователя
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public Participation Participation { get; init; } = null!;

    /// <summary>
    /// Идентификатор ответа, с которым связан выбор
    /// </summary>
    public Guid AnswerId { get; init; }

    /// <summary>
    /// Ответ, с которым связан выбор
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public Answer Answer { get; init; } = null!;



    /// <summary>
    /// Выбор пользователя в открытом опросе. Содержит связку Пользователь - Вариант ответа
    /// </summary>
    /// <param name="participationId">Идентификатор участия пользователя в вопросе</param>
    /// <param name="answerId">Идентификатор пользователя</param>
    public UserSelection(Guid participationId, Guid answerId)
    {
        ParticipationId = participationId;
        AnswerId = answerId;
    }

    /// <summary>
    /// Выбор пользователя в открытом опросе. Содержит связку Пользователь - Вариант ответа
    /// </summary>
    /// <param name="participation">Участие пользователя в вопросе</param>
    /// <param name="answerId">Идентификатор пользователя</param>
    public UserSelection(Participation participation, Guid answerId)
    {
        Participation = participation;
        ParticipationId = participation.Id;
        AnswerId = answerId;
    }
}