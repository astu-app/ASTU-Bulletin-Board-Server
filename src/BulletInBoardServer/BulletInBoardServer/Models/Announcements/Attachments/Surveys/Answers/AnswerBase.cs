namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;

public abstract class AnswerBase(Guid id, string content)
{
    /// <summary>
    /// Идентификатор ответа 
    /// </summary>
    public Guid Id { get; } = id;

    /// <summary>
    /// Текстовое содержимое ответа
    /// </summary>
    public string Content { get; } = content;

    /// <summary>
    /// Количествоп выбравших ответ пользователей
    /// </summary>
    public abstract int VotersCount { get; }
}