using System.Collections.ObjectModel;

namespace BulletInBoardServer.Domain.Models.AnnouncementCategories;

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