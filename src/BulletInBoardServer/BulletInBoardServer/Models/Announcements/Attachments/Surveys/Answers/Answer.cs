using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;

/// <summary>
/// Вариант ответа вопроса
/// </summary>
/// <param name="id">Идентификатор варианта ответа</param>
/// <param name="content">Текст варианта ответа</param>
/// <param name="votersCount">Количество проголосовавших за вариант ответа</param>
public class Answer(Guid id, Guid questionId, string content, int votersCount = 0) 
{
    /// <summary>
    /// Идентификатор варианта ответа
    /// </summary>
    public Guid Id { get; init; } = id;

    /// <summary>
    /// Идентификатор опроса, к которому относится вариант ответа
    /// </summary>
    public Guid QuestionId { get; init; } = questionId;

    /// <summary>
    /// Опрос, к которому относится вариант ответа
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public Question Question { get; set; } = null!;

    /// <summary>
    /// Текстовое содержимое варианта ответа
    /// </summary>
    public string Content { get; init; } = content;

    /// <summary>
    /// Количество выбравших вариант ответа пользователей
    /// </summary>
    public int VotersCount { get; private set; } = votersCount;



    /// <summary>
    /// Увеличить количество проголосовавших за вариант ответа на 1
    /// </summary>
    public void IncreaseVotersCount() =>
        VotersCount++;
}