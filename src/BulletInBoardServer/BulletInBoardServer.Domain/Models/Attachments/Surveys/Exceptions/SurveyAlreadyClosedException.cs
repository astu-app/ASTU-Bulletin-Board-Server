namespace BulletInBoardServer.Domain.Models.Attachments.Surveys.Exceptions;

public class SurveyAlreadyClosedException : InvalidOperationException
{
    public SurveyAlreadyClosedException()
        : base("Нельзя закрыть уже закрытый опрос")
    {
    }
}