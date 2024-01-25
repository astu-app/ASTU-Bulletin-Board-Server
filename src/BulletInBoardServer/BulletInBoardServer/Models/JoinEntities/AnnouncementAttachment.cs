using BulletInBoardServer.Models.Announcements;
using BulletInBoardServer.Models.Attachments;

namespace BulletInBoardServer.Models.JoinEntities;

/// <summary>
/// Сущность для настройки связи многие-ко-многим <see cref="Announcement"/> & <see cref="Attachment"/>
/// </summary>
public class AnnouncementAttachment
{
    /// <summary>
    /// Идентификатор объявления
    /// </summary>
    public Guid AnnouncementId { get; init; }
    
    /// <summary>
    /// Объявление
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public Announcement Announcement { get; set; } = null!;
    
    /// <summary>
    /// Идентификатор вложения
    /// </summary>
    public Guid AttachmentId { get; init; }
    
    
    
    /// <summary>
    /// Вложение
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public AttachmentBase Attachment { get; set; } = null!;
    
    /// <summary>
    /// Сущность для настройки связи многие-ко-многим <see cref="Announcement"/> & <see cref="Attachment"/>
    /// </summary>
    /// <param name="announcementId">Идентификатор объявления</param>
    /// <param name="attachmentId">Идентификатор вложения</param>
    public AnnouncementAttachment(Guid announcementId, Guid attachmentId)
    {
        AnnouncementId = announcementId;
        AttachmentId = attachmentId;
    }

    public AnnouncementAttachment()
    {
    }
}