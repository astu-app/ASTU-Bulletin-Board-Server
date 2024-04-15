using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Exceptions;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Questions;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Questions.Validation;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Voters;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace BulletInBoardServer.Domain.Models.Attachments.Surveys;

/// <summary>
/// Опрос
/// </summary>
public class Survey : AttachmentBase
{
    /// <summary>
    /// Открыт ли опрос
    /// </summary>
    public bool IsOpen { get; private set; }

    /// <summary>
    /// Анонимен ли опрос
    /// </summary>
    public bool IsAnonymous { get; init; }

    /// <summary>
    /// Момент автоматического закрытия опроса. Null, если автоматическое закрытие не задано
    /// </summary>
    public DateTime? AutoClosingAt { get; private set; }

    /// <summary>
    /// Задано ли автоматическое закрытие опроса. true, если задано, иначе - false
    /// </summary>
    public bool ExpectsAutoClosing { get; private set; } // private set для EF
    
    /// <summary>
    /// Момент фактического закрытия опроса. Null, если голосование еще не завершено
    /// </summary>
    public DateTime? VoteFinishedAt { get; private set; }

    /// <summary>
    /// Список вопросов опроса
    /// </summary>
    public QuestionList Questions { get; init; } = null!;

    /// <summary>
    /// Количество проголосовавших в опросе
    /// </summary>
    public int VotersCount { get; private set; }

    /// <summary>
    /// Пользователи, проголосовавшие в опросе
    /// </summary>
    public VoterList Voters { get; init; } = [];



    /// <summary>
    /// Опрос
    /// </summary>
    public Survey(Guid id, bool isOpen, bool isAnonymous, DateTime? autoClosingAt, DateTime? voteFinishedAt)
        : base(id, [], AttachmentTypes.Survey)
    {
        IsOpen = isOpen;
        IsAnonymous = isAnonymous;
        AutoClosingAt = autoClosingAt;
        VoteFinishedAt = voteFinishedAt;
    }

    /// <summary>
    /// Опрос
    /// </summary>
    /// <param name="id">Идентификатор опроса</param>
    /// <param name="announcements">Объявления, к которым опрос прикреплен</param>
    /// <param name="isOpen">Открыт ли опрос</param>
    /// <param name="isAnonymous">Анонимен ли опрос</param>
    /// <param name="autoClosingAt">Время автоматического закрытия опроса</param>
    /// <param name="voteFinishedAt">Момент фактического закрытия опроса</param>
    /// <param name="questions">Вопросы опроса</param>
    /// <exception cref="ArgumentNullException">Список вопросов null</exception>
    /// <exception cref="ArgumentException">Список вопросов пустой</exception>
    public Survey(
        Guid id,
        AnnouncementList announcements,
        bool isOpen, bool isAnonymous,
        DateTime? autoClosingAt,
        DateTime? voteFinishedAt,
        QuestionList questions)
        : base(id, announcements, AttachmentTypes.Survey)
    {
        QuestionValidator.AllQuestionsValidOrThrow(questions);

        IsOpen = isOpen;
        IsAnonymous = isAnonymous;
        AutoClosingAt = autoClosingAt;
        Questions = questions;
        VoteFinishedAt = voteFinishedAt;
    }



    /// <summary>
    /// Закрыть опрос
    /// </summary>
    /// <exception cref="SurveyAlreadyClosedException">Генерируется в случае, если опрос уже закрыт</exception>
    public void Close()
    {
        if (!IsOpen)
            throw new SurveyAlreadyClosedException();

        IsOpen = false;
        AutoClosingAt = null;
    }

    /// <summary>
    /// Увеличение количества проголосовавших в опросе пользователей
    /// </summary>
    public void IncreaseVotersCount() =>
        VotersCount++;
}