namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;

public class PublicQuestionVoters
{
    public Guid QuestionId { get; set; }
    public ICollection<AnswerVoters> AnswerListVoters { get; set; }
}