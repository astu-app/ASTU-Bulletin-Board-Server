using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.QuestionParticipation;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys;

/// <summary>
/// Выбор пользователя в открытом опросе. Содержит связку Пользователь - Вариант ответа
/// </summary>
public class UserSelection
{
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
        AnswerId = answerId;
    }

    /// <summary>
    /// Идентификатор участия, с которы связан выбор пользователя
    /// </summary>
    public Guid ParticipationId { get; set; }
    
    /// <summary>
    /// Участие, с которым связан выбор пользователя
    /// </summary>
    public Participation Participation { get; set; } = null!; // todo remark

    /// <summary>
    /// Идентификатор ответа, с которым связан выбор
    /// </summary>
    public Guid AnswerId { get; set; }

    /// <summary>
    /// Ответ, с которым связан выбор
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public Answer Answer { get; set; } = null!;
}