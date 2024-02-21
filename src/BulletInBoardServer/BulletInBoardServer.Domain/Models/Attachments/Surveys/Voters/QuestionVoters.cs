namespace BulletInBoardServer.Domain.Models.Attachments.Surveys.Voters;

public record QuestionVoters(Guid QuestionId, ICollection<AnswerVoters> AnswerListVoters);