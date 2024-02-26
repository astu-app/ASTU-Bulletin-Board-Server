using BulletInBoardServer.Domain.Models.Attachments.Surveys.Voters;

namespace BulletInBoardServer.Services.Services.Surveys.VotersGetting.Models;

public class AnonymousSurveyVoters(Guid surveyId, VoterList voters) : SurveyVotersBase(surveyId, voters);