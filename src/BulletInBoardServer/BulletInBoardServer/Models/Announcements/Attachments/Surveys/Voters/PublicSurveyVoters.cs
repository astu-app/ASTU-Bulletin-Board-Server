namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;

public class PublicSurveyVoters(Guid surveyId, VoterList voters, ICollection<QuestionVoters> everyQuestionVoters)
    : SurveyVotersBase(surveyId, voters)
{
    public ICollection<QuestionVoters> EveryQuestionVoters { get; init; } = everyQuestionVoters;
}