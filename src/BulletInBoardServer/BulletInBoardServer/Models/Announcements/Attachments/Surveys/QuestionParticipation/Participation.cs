using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.QuestionParticipation;

/// <summary>
/// Участие пользователя в вопросе
/// </summary>
public class Participation
{
    /// <summary>
    /// Идентификатор участия
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Идентификатор пользователя, связанного с участием
    /// </summary>
    public Guid UserId { get; }

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
    public Guid SurveyId { get; }

    /// <summary>
    /// Опрос, к которому относится участие
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public Survey Survey { get; set; } = null!;

    /// <summary>
    /// Является ли участие анонимным // todo проверить, что будет, если использовать, не загружая Survey
    /// </summary>
    public bool IsAnonymous => Survey.IsAnonymous;
    
    
    
    /// <summary>
    /// Участие пользователя в вопросе
    /// </summary>
    /// <param name="id">Идентификатор участия</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="surveyId">Идентификатор опроса, к которому относится участие</param>
    public Participation(Guid id, Guid userId, Guid surveyId)
    {
        Id = id;
        UserId = userId;
        SurveyId = surveyId;
    }
}