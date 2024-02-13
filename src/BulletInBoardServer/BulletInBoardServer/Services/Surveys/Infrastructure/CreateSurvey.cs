namespace BulletInBoardServer.Services.Surveys.Infrastructure;

public record CreateSurvey(bool IsOpen, bool IsAnonymous, DateTime AutoClosingAt, IEnumerable<CreateQuestion> Questions);