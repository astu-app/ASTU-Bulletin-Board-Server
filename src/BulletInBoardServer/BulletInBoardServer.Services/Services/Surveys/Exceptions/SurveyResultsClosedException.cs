namespace BulletInBoardServer.Services.Services.Surveys.Exceptions;

public class SurveyResultsClosedException(string? message = null, Exception? innerException = null) 
    : InvalidOperationException(message, innerException);