namespace BulletInBoardServer.Domain.Models.Attachments.Surveys.Voters;

public class SurveyVotersBase(Guid surveyId, VoterList voters)
{
    public Guid SurveyId { get; init; } = surveyId;
    public VoterList Voters { get; init; } = voters;
}