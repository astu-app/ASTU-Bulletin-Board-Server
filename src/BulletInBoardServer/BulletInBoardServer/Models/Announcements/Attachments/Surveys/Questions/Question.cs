using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;

public class Question
{
    /// <summary>
    /// Идентификатор вопроса
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Текстовое содержимое вопроса
    /// </summary>
    public string Content { get; }

    /// <summary>
    /// Варианты ответов вопроса
    /// </summary>
    public ReadOnlyAnswers Answers => _answers.AsReadOnly();

    /// <summary>
    /// Количество проголосовавших в вопросе
    /// </summary>
    public int VotersCount => _answers.Sum(answer => answer.VotersCount);



    private readonly Answers.Answers _answers;



    public Question(Guid id, string content, Answers.Answers answers)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Контент вопроса не может быть пустым или Null");
        
        if (answers is null)
            throw new ArgumentNullException(nameof(answers));
        if (answers.Count < 2)
            throw new ArgumentException("Количество вариантов ответов вопроса не может быть меньше двух");
        
        Id = id;
        Content = content;
        _answers = answers;
    }
}