namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;

public class AnswerVoters
{
    public Guid AnswerId { get; set; }
    public VoterList Voters { get; set; }
}