using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys;

public class Survey : IAttachment
{
    /// <summary>
    /// Идентификатор опроса
    /// </summary>
    public Guid Id { get; }

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
    public bool ExpectsAutoClosing => AutoClosingAt is not null;

    /// <summary>
    /// Список вопросов опроса
    /// </summary>
    public ReadOnlyQuestions Questions => _questions.AsReadOnly();

    /// <summary>
    /// Количество проголосовавших в опросе
    /// </summary>
    public int VotersCount => _questions.Sum(question => question.VotersCount);

    // todo размер аудитории опроса


    private readonly Questions.Questions _questions;



    /// <summary>
    /// Конструктор опроса
    /// </summary>
    /// <exception cref="ArgumentNullException">Список вопросов null</exception>
    /// <exception cref="ArgumentException">Список вопросов пустой</exception>
    public Survey(Guid id, bool isOpen, bool isAnonymous, bool isMultipleChoiceAllowed, DateTime? autoClosingAt, 
        Questions.Questions questions)
    {
        if (questions is null)
            throw new ArgumentNullException(nameof(questions));
        if (!questions.Any())
            throw new ArgumentException("Оопрос должен модержать хотя бы один вопрос");
        
        Id = id;
        IsOpen = isOpen;
        IsAnonymous = isAnonymous;
        IsMultipleChoiceAllowed = isMultipleChoiceAllowed;
        AutoClosingAt = autoClosingAt;
        _questions = questions;
    }



    /// <summary>
    /// Метод закрывает опрос
    /// </summary>
    /// <exception cref="InvalidOperationException">Генерируется в случае, если опрос уже закрыт</exception>
    public void Close()
    {
        if (!IsOpen)
            throw new InvalidOperationException("Нельзя заркыть уже закрытый опрос");

        IsOpen = false;
    }
}