using System.Collections.ObjectModel;
using BulletInBoardServer.Models.Announcements.Attachments;

namespace BulletInBoardServer.Models.Announcements;

public class AnnouncementCategories : Collection<AnnouncementCategory>
{
    public AnnouncementCategories(IList<AnnouncementCategory> categories)
        : base(categories)
    {
    }

    public AnnouncementCategories()
    {
    }
}