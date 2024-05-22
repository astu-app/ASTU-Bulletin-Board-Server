using System.Collections.ObjectModel;
using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Domain.Models.Announcements;

public class AnnouncementAudience : UserList
{
    public AnnouncementAudience(Collection<User> users) : base(users)
    {
    }

    public AnnouncementAudience()
    {
    }
}