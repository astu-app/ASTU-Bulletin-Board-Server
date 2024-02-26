namespace BulletInBoardServer.Services.Services.Surveys.Exceptions;

public class SurveyClosedException() : InvalidOperationException("Нельзя проголосовать в закрытом опросе");