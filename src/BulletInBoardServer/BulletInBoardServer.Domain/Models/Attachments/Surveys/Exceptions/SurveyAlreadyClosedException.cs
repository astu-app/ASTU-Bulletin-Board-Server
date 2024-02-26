namespace BulletInBoardServer.Domain.Models.Attachments.Surveys.Exceptions;

public class SurveyAlreadyClosedException() : InvalidOperationException("Нельзя закрыть уже закрытый опрос");