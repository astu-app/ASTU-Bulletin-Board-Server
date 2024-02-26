namespace BulletInBoardServer.Services.Services.Surveys.Exceptions;

public class PresentedQuestionsDoesntMatchSurveyQuestionsException()
    : InvalidOperationException("Список предоставленных вопросов не соответствует списку вопросов опроса");