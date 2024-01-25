namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;

public class AnonymousSurveyVoters(Guid surveyId, VoterList voters) : SurveyVotersBase(surveyId, voters);