namespace BulletInBoardServer.Services.Services.Surveys.Exceptions;

public class SurveyAlreadyVotedException()
    : InvalidOperationException("Пользователь не может проголосовать дважды в одном опросе");