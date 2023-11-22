namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;

public static class VotersExtensions
{
    public static ReadOnlyVoters AsReadOnly(this Voters voters) =>
        new (voters);
}