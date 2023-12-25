using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;
using BulletInBoardServer.Services.Surveys.Validators;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys;

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
    public bool IsAnonymous { get; }
    
    /// <summary>
    /// Возможно ли при голосовании выбрать несколько вариантов ответов
    /// </summary>
    public bool IsMultipleChoiceAllowed { get; }

    /// <summary>
    /// Момент автоматического закрытия опроса. Null, если автоматическое закрытие не задано
    /// </summary>
    public DateTime? AutoClosingAt { get; }
    
    /// <summary>
    /// Задано ли автоматическое закрытие опроса. true, если задано, иначе - false
    /// </summary>
    public bool ExpectsAutoClosing => AutoClosingAt is not null;

    /// <summary>
    /// Список вопросов опроса
    /// </summary>
    public ReadOnlyQuestionList Questions => _questions.AsReadOnly();

    /// <summary>
    /// Количество проголосовавших в опросе
    /// </summary>
    public int VotersCount { get; private set; }



    private readonly QuestionList _questions;



    /// <summary>
    /// Конструктор опроса
    /// </summary>
    /// <exception cref="ArgumentNullException">Список вопросов null</exception>
    /// <exception cref="ArgumentException">Список вопросов пустой</exception>
    public Survey(
        Guid id,
        bool isOpen, bool isAnonymous, bool isMultipleChoiceAllowed,
        DateTime? autoClosingAt,
        QuestionList questions)
        : base(id, "Survey")
    {
        QuestionValidator.AllQuestionsValidOrThrow(questions);

        IsOpen = isOpen;
        IsAnonymous = isAnonymous;
        IsMultipleChoiceAllowed = isMultipleChoiceAllowed;
        AutoClosingAt = autoClosingAt;
        _questions = questions;
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
    
    /// <summary>
    /// Уменьшение количества проголосовавших в опросе пользователей
    /// </summary>
    public void DecreaseVotersCount() =>
        VotersCount--;
}