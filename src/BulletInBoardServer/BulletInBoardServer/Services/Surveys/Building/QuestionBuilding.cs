namespace BulletInBoardServer.Services.Surveys.Building;

public class QuestionBuilding(string content, AnswerListBuilding answers)
{
    public string Content { get; } = content;
    public AnswerListBuilding Answers { get; } = answers;
}