namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;

public class PublicSurveyVoters
{
    public Guid SurveyId { get; set; }
    public ICollection<PublicQuestionVoters> QuestionListVoters { get; set; }
}