using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;

public static class VoterListExtensions
{
    public static VoterList ToVoters(this IEnumerable<User> users) =>
        new (users.ToList());
}