using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Domain.Models.Announcements;

public static class AnnouncementAudienceExtensions
{
    public static AnnouncementAudience ToAudience(this UserList users) =>
        new(users);
    
    public static AnnouncementAudience ToAudience(this IEnumerable<User> users) =>
        new(users.ToUserList().ToAudience());
}