using BulletInBoardServer.Domain.Models.Attachments.Surveys.Answers;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Questions.Validation;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace BulletInBoardServer.Domain.Models.Attachments.Surveys.Questions;

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
    /// Порядковый номер вопроса в списке
    /// </summary>
    public int Serial { get; init; }

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



    public Question(Guid id, int serial, Guid surveyId, string content, bool isMultipleChoiceAllowed, AnswerList answers)
    {
        QuestionValidator.QuestionContentValidOrThrow(content);
        QuestionValidator.AllAnswersValidOrThrow(answers);

        Id = id;
        Serial = serial;
        SurveyId = surveyId;
        Content = content;
        IsMultipleChoiceAllowed = isMultipleChoiceAllowed;
        Answers = answers;
    }

    private Question() // конструктор нужен для EF'а 
    {
    }
}