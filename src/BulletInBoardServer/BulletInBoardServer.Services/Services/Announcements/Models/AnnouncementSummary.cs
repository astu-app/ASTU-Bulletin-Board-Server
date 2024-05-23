using BulletInBoardServer.Domain.Models.Attachments;
using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Services.Services.Announcements.Models;

/// <summary>
/// Краткая информация об объявлении
/// </summary>
/// <param name="id">Идентификатор объявления</param>
/// <param name="author">Автор объявления</param>
/// <param name="content">Текст объявления</param>
/// <param name="viewsCount">Количество просмотров объявления</param>
/// <param name="publishedAt">Момент публикации объявления</param>
/// <param name="hiddenAt">Момент сокрытия объявления</param>
/// <param name="delayedPublishingAt">Момент отложенной объявления</param>
/// <param name="delayedHidingAt">Момент отложенного сокрытия объявления</param>
/// <param name="attachments">Вложения объявления</param>
public class AnnouncementSummary(
    Guid id,
    User author,
    string content,
    int viewsCount,
    int audienceSize,
    DateTime? publishedAt,
    DateTime? hiddenAt,
    DateTime? delayedPublishingAt,
    DateTime? delayedHidingAt,
    ICollection<AttachmentBase>? attachments)
{
    /// <summary>
    /// Идентификатор объявления
    /// </summary>
    public Guid Id { get; } = id;

    /// <summary>
    /// Автор объявления
    /// </summary>
    public User Author { get; } = author;

    /// <summary>
    /// Текст объявления
    /// </summary>
    public string Content { get; } = content;

    /// <summary>
    /// Количество просмотров объявления
    /// </summary>
    public int ViewsCount { get; } = viewsCount;

    /// <summary>
    /// Размер аудитории объявления
    /// </summary>
    public int AudienceSize { get; } = audienceSize;

    /// <summary>
    /// Момент публикации объявления
    /// </summary>
    /// <remarks>
    /// null, если объявление еще не опубликовано
    /// </remarks>
    public DateTime? PublishedAt { get; } = publishedAt;

    /// <summary>
    /// Момент сокрытия объявления
    /// </summary>
    /// <remarks>
    /// null, если объявление еще не скрыто
    /// </remarks>
    public DateTime? HiddenAt { get; } = hiddenAt;
    
    /// <summary>
    /// Момент отложенной объявления
    /// </summary>
    public DateTime? DelayedPublishingAt{ get;} = delayedPublishingAt;
    
    /// <summary>
    /// Момент отложенного сокрытия объявления
    /// </summary>
    public DateTime? DelayedHidingAt{ get;} = delayedHidingAt;

    /// <summary>
    /// Список вложений объявления
    /// </summary>
    /// <remarks>
    /// null, если к объявлению не прикреалены вложения
    /// </remarks>
    public ICollection<AttachmentBase>? Attachments { get; } = attachments;
}