using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Domain.Models.Attachments.Surveys.Voters;

public static class VoterListExtensions
{
    public static VoterList ToVoters(this IEnumerable<User> users) =>
        new(users.ToList());
}