namespace BulletInBoardServer.Services.Services.Surveys.Exceptions;

public class SurveyDoesNotExistException(Exception? internalException = null)
    : InvalidOperationException("Опрос не существует", internalException);