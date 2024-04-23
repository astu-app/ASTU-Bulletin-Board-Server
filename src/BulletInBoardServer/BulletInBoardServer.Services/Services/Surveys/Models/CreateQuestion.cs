namespace BulletInBoardServer.Services.Services.Surveys.Models;

public record CreateQuestion(int Serial, string Content, bool IsMultipleChoiceAllowed, IEnumerable<CreateAnswer> Answers);