using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.Announcements.Categories;

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