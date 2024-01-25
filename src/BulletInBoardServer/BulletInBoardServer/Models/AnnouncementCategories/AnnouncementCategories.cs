using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.AnnouncementCategories;

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