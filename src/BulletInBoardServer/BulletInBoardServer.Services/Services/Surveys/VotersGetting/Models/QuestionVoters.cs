namespace BulletInBoardServer.Services.Services.Surveys.VotersGetting.Models;

public record QuestionVoters(Guid QuestionId, ICollection<AnswerVoters> AnswerListVoters);