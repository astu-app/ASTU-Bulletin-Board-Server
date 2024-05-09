namespace BulletInBoardServer.Domain.Models.Attachments.Surveys;

public class SurveyVotes
{
    private readonly IDictionary<Guid, List<Guid>> _votes;

    
    
    public SurveyVotes(IDictionary<Guid, List<Guid>> votes)
    {
        _votes = votes;
    }

    
    // ReSharper disable once UnusedMember.Global - Используется для маппинга библиотекой Mapster
    public SurveyVotes() : this(new Dictionary<Guid, List<Guid>>())
    {
    }

    public IEnumerable<Guid> GetQuestionsIds() =>
        _votes.Keys;

    public IEnumerable<Guid> GetVotes(Guid questionId)
    {
        var answerExist = _votes.TryGetValue(questionId, out var value);
        if (!answerExist)
            throw new InvalidOperationException("Вопрос с указанным Id отсутствует");

        return value!; // переменная будет null только если answerExist = false 
    }
}