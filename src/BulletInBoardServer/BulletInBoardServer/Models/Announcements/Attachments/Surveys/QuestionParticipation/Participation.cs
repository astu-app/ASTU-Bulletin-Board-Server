using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.QuestionParticipation;

/// <summary>
/// Участие пользователя в вопросе
/// </summary>
/// <param name="userId">Идентификатор пользователя</param>
/// <param name="questionId">Идентификатор Вопроса, к которому относится участие</param>
public class Participation(Guid userId, Guid questionId)
{
    /// <summary>
    /// Идентификатор участия
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор пользователя, связанного с участием
    /// </summary>
    public Guid UserId { get; } = userId;

    /// <summary>
    /// Пользователь, связанный с участием
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public User User { get; set; } = null!;
    
    /// <summary>
    /// Идентификатор опроса, к которому относится участие
    /// </summary>
    public Guid SurveyId { get; } = questionId;

    /// <summary>
    /// Опрос, к которому относится участие
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public Survey Survey { get; set; } = null!;
}