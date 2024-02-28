using BulletInBoardServer.Domain.Models.Attachments.Surveys.Answers;
using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Domain.Models.Attachments.Surveys.SurveyParticipation;

/// <summary>
/// Участие пользователя в вопросе
/// </summary>
public class Participation
{
    /// <summary>
    /// Идентификатор участия
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор пользователя, связанного с участием
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Пользователь, связанный с участием
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework или конструктора.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public User User { get; init; } = null!;

    /// <summary>
    /// Идентификатор опроса, к которому относится участие
    /// </summary>
    public Guid SurveyId { get; init; }

    /// <summary>
    /// Опрос, к которому относится участие
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework или конструктора.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public Survey Survey { get; init; } = null!;

    /// <summary>
    /// Является ли участие анонимным 
    /// </summary>
    public bool IsAnonymous => Survey.IsAnonymous;

    /// <summary>
    /// Список вариантов ответов, с которым связано участие
    /// </summary>
    /// <remarks>
    /// Не будет содержать элементов, если варианты ответов связаны с анонимным опросом 
    /// </remarks>
    public AnswerList Answers { get; init; } = [];



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

    /// <summary>
    /// Участие пользователя в вопросе
    /// </summary>
    public Participation()
    {
    }
}