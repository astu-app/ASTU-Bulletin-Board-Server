using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.QuestionParticipation;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;

/// <summary>
/// Вопрос опроса
/// </summary>
public class Question
{
    /// <summary>
    /// Идентификатор вопроса
    /// </summary>
    // public Guid Id { get; }
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор опроса, к которому относится вопрос
    /// </summary>
    // public Guid SurveyId { get; }
    public Guid SurveyId { get; set; }

    /// <summary>
    /// Опрос, к которому относится вопрос
    /// </summary>
    public Survey? Survey { get; set; }

    /// <summary>
    /// Текстовое содержимое вопроса
    /// </summary>
    // public string Content { get; }
    public string Content { get; set; }

    /// <summary>
    /// Варианты ответов вопроса
    /// </summary>
    public AnswerList Answers { get; set; }

    /// <summary>
    /// Участия в опросе
    /// </summary>
    public ParticipationList ParticipationList { get; set; } = [];



    public Question(Guid id, Guid surveyId, string content, AnswerList answers)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Контент вопроса не может быть пустым или Null");

        if (answers is null)
            throw new ArgumentNullException(nameof(answers));
        if (answers.Count < 2)
            throw new ArgumentException("Количество вариантов ответов вопроса не может быть меньше двух");

        Id = id;
        SurveyId = surveyId;
        Answers = answers;
        Content = content;
    }

    public Question(Guid id, Guid surveyId, string content)
    { // debug
        Id = id;
        SurveyId = surveyId;
        Content = content;
    }
}