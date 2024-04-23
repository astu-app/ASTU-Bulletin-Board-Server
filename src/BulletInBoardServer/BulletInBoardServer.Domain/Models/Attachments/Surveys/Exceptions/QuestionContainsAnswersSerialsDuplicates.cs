namespace BulletInBoardServer.Domain.Models.Attachments.Surveys.Exceptions;

/// <summary>
/// Вопрос содержит варианты ответов с повторяющимися порядковыми номерами
/// </summary>
public class QuestionContainsAnswersSerialsDuplicates(string message, Exception? innerException = null) 
    : InvalidOperationException(message, innerException);