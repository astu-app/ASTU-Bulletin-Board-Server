namespace BulletInBoardServer.Domain.Models.Attachments.Surveys.Voters;

public class AnonymousSurveyVoters(Guid surveyId, VoterList voters) : SurveyVotersBase(surveyId, voters);