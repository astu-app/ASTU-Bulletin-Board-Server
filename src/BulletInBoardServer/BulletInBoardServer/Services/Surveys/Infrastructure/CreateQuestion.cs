namespace BulletInBoardServer.Services.Surveys.Infrastructure;

public record CreateQuestion(string Content, bool IsMultipleChoiceAllowed, IEnumerable<CreateAnswer> Answers);