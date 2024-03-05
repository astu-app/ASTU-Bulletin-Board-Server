namespace BulletInBoardServer.Services.Services.Surveys.Exceptions;

public class AnswerDoesNotExistException(Exception? innerException = null)
    : InvalidOperationException("Вариант ответа не существует", innerException);