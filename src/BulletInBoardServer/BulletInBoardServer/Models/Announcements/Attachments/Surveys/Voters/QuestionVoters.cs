namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;

public record QuestionVoters(Guid QuestionId, ICollection<AnswerVoters> AnswerListVoters);