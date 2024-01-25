using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;

/// <summary>
/// Вопрос опроса
/// </summary>
public class Question
{
    /// <summary>
    /// Идентификатор вопроса
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор опроса, к которому относится вопрос
    /// </summary>
    public Guid SurveyId { get; init; }

    /// <summary>
    /// Опрос, к которому относится вопрос
    /// </summary>
    public Survey? Survey { get; init; }

    /// <summary>
    /// Текстовое содержимое вопроса
    /// </summary>
    // public string Content { get; }
    public string Content { get; init; }
    
    /// <summary>
    /// Возможно ли при голосовании выбрать несколько вариантов ответов
    /// </summary>
    public bool IsMultipleChoiceAllowed { get; init; }

    /// <summary>
    /// Варианты ответов вопроса
    /// </summary>
    public AnswerList Answers { get; init; }



    public Question(Guid id, Guid surveyId, string content, bool isMultipleChoiceAllowed, AnswerList answers)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Контент вопроса не может быть пустым или Null");

        if (answers is null)
            throw new ArgumentNullException(nameof(answers));
        if (answers.Count < 2)
            throw new ArgumentException("Количество вариантов ответов вопроса не может быть меньше двух");

        Id = id;
        SurveyId = surveyId;
        Content = content;
        IsMultipleChoiceAllowed = isMultipleChoiceAllowed;
        Answers = answers;
    }

    public Question(Guid id, Guid surveyId, string content)
    { // debug
        Id = id;
        SurveyId = surveyId;
        Content = content;
    }
}