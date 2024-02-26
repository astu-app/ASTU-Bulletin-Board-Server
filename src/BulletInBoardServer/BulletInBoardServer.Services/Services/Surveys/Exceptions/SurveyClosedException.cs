namespace BulletInBoardServer.Services.Services.Surveys.Exceptions;

public class SurveyClosedException : InvalidOperationException
{
    public SurveyClosedException()
        : base("Нельзя проголосовать в закрытом опросе")
    {
    }
}