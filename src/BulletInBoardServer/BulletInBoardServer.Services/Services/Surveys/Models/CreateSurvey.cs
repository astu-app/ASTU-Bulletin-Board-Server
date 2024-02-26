namespace BulletInBoardServer.Services.Services.Surveys.Models;

public record CreateSurvey(
    bool IsAnonymous,
    DateTime AutoClosingAt,
    IEnumerable<CreateQuestion> Questions);