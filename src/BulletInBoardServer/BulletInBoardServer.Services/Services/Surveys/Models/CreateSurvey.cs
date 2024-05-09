namespace BulletInBoardServer.Services.Services.Surveys.Models;

public record CreateSurvey(
    bool IsAnonymous,
    bool ResultsOpenBeforeClosing,
    DateTime AutoClosingAt,
    IEnumerable<CreateQuestion> Questions);