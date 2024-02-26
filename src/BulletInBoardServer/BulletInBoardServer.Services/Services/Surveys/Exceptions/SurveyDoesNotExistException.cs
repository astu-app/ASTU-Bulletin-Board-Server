namespace BulletInBoardServer.Services.Services.Surveys.Exceptions;

public class SurveyDoesNotExistException : InvalidOperationException
{
    public SurveyDoesNotExistException(Exception? internalException = null)
        : base("Опрос не существует", internalException)
    {
    }
}