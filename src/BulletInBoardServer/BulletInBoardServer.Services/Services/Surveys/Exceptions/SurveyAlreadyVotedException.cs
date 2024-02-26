namespace BulletInBoardServer.Services.Services.Surveys.Exceptions;

public class SurveyAlreadyVotedException : InvalidOperationException
{
    public SurveyAlreadyVotedException()
        : base("Пользователь не может проголосовать дважды в одном опросе")
    {
    }
}