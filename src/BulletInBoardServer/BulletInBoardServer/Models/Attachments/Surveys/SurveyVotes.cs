namespace BulletInBoardServer.Models.Attachments.Surveys;

public class SurveyVotes(IDictionary<Guid, List<Guid>> votes)
{
    public ICollection<Guid> GetQuestionsIds() =>
        votes.Keys;

    public ICollection<Guid> GetVotes(Guid questionId)
    {
        var answerExist = votes.TryGetValue(questionId, out var value);
        if (!answerExist)
            throw new InvalidOperationException("Вопрос с указанным Id отсутствует");

        return value!; // переменная будет null только если answerExist = false 
    }
}