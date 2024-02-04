using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Services.Announcements.Infrastructure;

/// <summary>
/// Краткая информация об объявлении
/// </summary>
/// <param name="id">Идентификатор объявления</param>
/// <param name="author">Автор объявления</param>
/// <param name="content">Текст объявления</param>
/// <param name="viewsCount">Количество просмотров объявления</param>
/// <param name="delayedPublishingAt">Момент отложенной публикации объявления</param>
/// <param name="delayedHidingAt">Момент отложенного сокрытия объявления</param>
/// <param name="publishedAt">Момент публикации объявления</param>
/// <param name="hiddenAt">Момент сокрытия объявления</param>
public class AnnouncementSummary(
    Guid id,
    User author,
    string content,
    int viewsCount,
    bool expectsDelayedPublishing,
    DateTime? delayedPublishingAt,
    bool expectsDelayedHiding,
    DateTime? delayedHidingAt,
    DateTime? publishedAt,
    DateTime? hiddenAt)
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
    /// Момент отложенной публикации объявления
    /// </summary>
    /// <remarks>
    /// null, если отложенная публикация не запланирована
    /// </remarks>
    public DateTime? DelayedPublishingAt { get; } = delayedPublishingAt;

    /// <summary>
    /// Ожидает ли объявление отложенной публикации
    /// </summary>
    public bool ExpectsDelayedPublishing { get; } = expectsDelayedPublishing;
    
    /// <summary>
    /// Момент отложенного сокрытия объявления
    /// </summary>
    /// <remarks>
    /// null, если отложенное сокрытие не запланировано
    /// </remarks>
    public DateTime? DelayedHidingAt { get; } = delayedHidingAt;

    /// <summary>
    /// Ожидает ли объявление отложенного сокрытия
    /// </summary>
    public bool ExpectsDelayedHiding { get; } = expectsDelayedHiding;

    /// <summary>
    /// Момент публикации объявления
    /// </summary>
    /// <remarks>
    /// null, если объявление еще не опубликовано
    /// </remarks>
    public DateTime? PublishedAt { get; } = publishedAt;

    /// <summary>
    /// Опубликовано ли объявление
    /// </summary>
    public bool IsPublished => PublishedAt is not null;
    
    /// <summary>
    /// Момент сокрытия объявления
    /// </summary>
    /// <remarks>
    /// null, если объявление еще не скрыто
    /// </remarks>
    public DateTime? HiddenAt { get; } = hiddenAt;
    
    /// <summary>
    /// Скрыто ли объявление
    /// </summary>
    public bool IsHidden => HiddenAt is not null;

}