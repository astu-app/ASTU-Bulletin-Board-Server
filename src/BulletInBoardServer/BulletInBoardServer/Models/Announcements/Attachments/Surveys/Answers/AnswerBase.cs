namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;

public abstract class AnswerBase
{
    /// <summary>
    /// Идентификатор ответа 
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Текстовое содержимое ответа
    /// </summary>
    public string Content { get; set; }
    
    /// <summary>
    /// Количествоп выбравших ответ пользователей
    /// </summary>
    public abstract int VotersCount { get; }



    protected AnswerBase(Guid id, string content)
    {
        Id = id;
        Content = content;
    }
}