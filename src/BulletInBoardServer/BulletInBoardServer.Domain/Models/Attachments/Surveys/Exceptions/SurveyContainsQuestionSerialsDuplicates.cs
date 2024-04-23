namespace BulletInBoardServer.Domain.Models.Attachments.Surveys.Exceptions;

/// <summary>
/// Опрос содержит вопросы с повторяющимися порядковыми номерами
/// </summary>
public class SurveyContainsQuestionSerialsDuplicates(string message, Exception? innerException = null)
    : InvalidOperationException(message, innerException);