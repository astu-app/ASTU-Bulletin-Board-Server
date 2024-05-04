using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Domain.Models.UserGroups;

namespace BulletInBoardServer.Services.Services.Announcements.Models;

/// <summary>
/// Класс с данными для редактирования объявления, отправляемый клиенту
/// </summary>
public class UpdateAnnouncementContent
{
    /// <summary>
    /// Объявление, детали которого отправляются
    /// </summary>
    public Announcement Announcement { get; set; }

    /// <summary>
    /// Потенциальная иерархия объявления в полном объеме. Содержит набор иерархий групп пользователей, подчиненных
    /// автору объявления.
    /// </summary>
    public UserGroupList PotentialAudienceHierarchy { get; set; }



    /// <summary>
    /// Класс с данными для редактирования объявления, отправляемый клиенту
    /// </summary>
    /// <param name="announcement">Объявление, детали которого отправляются</param>
    /// <param name="potentialAudienceHierarchy">Потенциальная иерархия объявления в полном объеме. Содержит набор иерархий групп пользователей, подчиненных автору объявления.</param>
    public UpdateAnnouncementContent(Announcement announcement, UserGroupList potentialAudienceHierarchy)
    {
        Announcement = announcement;
        PotentialAudienceHierarchy = potentialAudienceHierarchy;
    }
}