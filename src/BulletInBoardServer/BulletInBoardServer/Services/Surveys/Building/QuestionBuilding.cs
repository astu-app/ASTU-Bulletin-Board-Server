namespace BulletInBoardServer.Services.Surveys.Building;

public class QuestionBuilding(string content, bool isMultipleChoiceAllowed, AnswerListBuilding answers)
{
    public string Content { get; } = content;
    public bool IsMultipleChoiceAllowed { get; } = isMultipleChoiceAllowed;
    public AnswerListBuilding Answers { get; } = answers;
}