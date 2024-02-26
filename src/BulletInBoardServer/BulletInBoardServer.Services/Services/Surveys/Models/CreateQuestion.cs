namespace BulletInBoardServer.Services.Services.Surveys.Models;

public record CreateQuestion(string Content, bool IsMultipleChoiceAllowed, IEnumerable<CreateAnswer> Answers);