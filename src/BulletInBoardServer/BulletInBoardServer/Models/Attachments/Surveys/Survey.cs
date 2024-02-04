using BulletInBoardServer.Models.Announcements;
using BulletInBoardServer.Models.Attachments.Surveys.Questions;
using BulletInBoardServer.Models.Attachments.Surveys.Voters;
using BulletInBoardServer.Services.Surveys.Validating;

namespace BulletInBoardServer.Models.Attachments.Surveys;

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
    public DateTime? AutoClosingAt { get; init; }
    
    /// <summary>
    /// Задано ли автоматическое закрытие опроса. true, если задано, иначе - false
    /// </summary>
    public bool ExpectsAutoClosing => AutoClosingAt is not null;

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
    public Survey(Guid id, bool isOpen, bool isAnonymous, DateTime? autoClosingAt)
        : base(id, [], AttachmentTypes.Survey)
    {
        IsOpen = isOpen;
        IsAnonymous = isAnonymous;
        AutoClosingAt = autoClosingAt;
    }

    /// <summary>
    /// Опрос
    /// </summary>
    /// <param name="id">Идентификатор опроса</param>
    /// <param name="announcements">Объявления, к которым опрос прикреплен</param>
    /// <param name="isOpen">Открыт ли опрос</param>
    /// <param name="isAnonymous">Анонимен ли опрос</param>
    /// <param name="autoClosingAt">Время автоматического закрытия опроса</param>
    /// <param name="questions">Вопросы опроса</param>
    /// <exception cref="ArgumentNullException">Список вопросов null</exception>
    /// <exception cref="ArgumentException">Список вопросов пустой</exception>
    public Survey(
        Guid id,
        AnnouncementList announcements,
        bool isOpen, bool isAnonymous,
        DateTime? autoClosingAt,
        QuestionList questions)
        : base(id, announcements, AttachmentTypes.Survey)
    {
        QuestionValidator.AllQuestionsValidOrThrow(questions);

        IsOpen = isOpen;
        IsAnonymous = isAnonymous;
        AutoClosingAt = autoClosingAt;
        Questions = questions;
    }



    /// <summary>
    /// Закрыть опрос
    /// </summary>
    /// <exception cref="InvalidOperationException">Генерируется в случае, если опрос уже закрыт</exception>
    public void Close()
    {
        if (!IsOpen)
            throw new InvalidOperationException("Нельзя закрыть уже закрытый опрос");

        IsOpen = false;
    }
    
    /// <summary>
    /// Увеличение количества проголосовавших в опросе пользователей
    /// </summary>
    public void IncreaseVotersCount() =>
        VotersCount++;
}