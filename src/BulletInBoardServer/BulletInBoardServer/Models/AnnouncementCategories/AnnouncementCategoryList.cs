using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.AnnouncementCategories;

public class AnnouncementCategoryList : Collection<AnnouncementCategory>
{
    public AnnouncementCategoryList(IList<AnnouncementCategory> categories)
        : base(categories)
    {
    }

    public AnnouncementCategoryList()
    {
    }
}