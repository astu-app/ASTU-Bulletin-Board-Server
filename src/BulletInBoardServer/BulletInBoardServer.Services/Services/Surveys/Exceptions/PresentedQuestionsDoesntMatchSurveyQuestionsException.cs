namespace BulletInBoardServer.Services.Services.Surveys.Exceptions;

public class PresentedQuestionsDoesntMatchSurveyQuestionsException : InvalidOperationException
{
    public PresentedQuestionsDoesntMatchSurveyQuestionsException()
        : base("Список предоставленных вопросов не соответствует списку вопросов опроса")
    {
    }
}