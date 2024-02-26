using BulletInBoardServer.Domain.Models.Attachments.Surveys.Voters;

namespace BulletInBoardServer.Services.Services.Surveys.VotersGetting.Models;

public class PublicSurveyVoters(Guid surveyId, VoterList voters, ICollection<QuestionVoters> everyQuestionVoters)
    : SurveyVotersBase(surveyId, voters)
{
    public ICollection<QuestionVoters> EveryQuestionVoters { get; init; } = everyQuestionVoters;
}