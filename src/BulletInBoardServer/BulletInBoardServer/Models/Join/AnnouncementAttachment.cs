using BulletInBoardServer.Models.Announcements;
using BulletInBoardServer.Models.Announcements.Attachments;

namespace BulletInBoardServer.Models.Join;

/// <summary>
/// Сущность для настройки связи многие-ко-многим <see cref="Announcement"/> & <see cref="Attachment"/>
/// </summary>
/// <param name="announcementId">Идентификатор объявления</param>
/// <param name="attachmentId">Идентификатор вложения</param>
public class AnnouncementAttachment(Guid announcementId, Guid attachmentId)
{
    /// <summary>
    /// Идентификатор объявления
    /// </summary>
    public Guid AnnouncementId { get; } = announcementId;
    
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
    public Guid AttachmentId { get; } = attachmentId;
    
    /// <summary>
    /// Вложение
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public AttachmentBase Attachment { get; set; } = null!;
}