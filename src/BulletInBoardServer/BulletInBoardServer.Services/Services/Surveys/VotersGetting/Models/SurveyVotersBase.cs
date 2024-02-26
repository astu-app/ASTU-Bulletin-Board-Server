using BulletInBoardServer.Domain.Models.Attachments.Surveys.Voters;

namespace BulletInBoardServer.Services.Services.Surveys.VotersGetting.Models;

public class SurveyVotersBase(Guid surveyId, VoterList voters)
{
    public Guid SurveyId { get; init; } = surveyId;
    public VoterList Voters { get; init; } = voters;
}