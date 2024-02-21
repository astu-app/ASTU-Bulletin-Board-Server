namespace BulletInBoardServer.Services.Services.Surveys.DelayedOperations;

public interface IAutomaticSurveyOperationsDispatcher
{
    void ConfigureAutoClosing(Guid surveyId, DateTime closeAt);
    void DisableAutoClosing(Guid surveyId);
}