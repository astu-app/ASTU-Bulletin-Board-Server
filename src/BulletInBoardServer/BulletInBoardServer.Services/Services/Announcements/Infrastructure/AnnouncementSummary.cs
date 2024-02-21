using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Services.Services.Announcements.Infrastructure;

/// <summary>
/// Краткая информация об объявлении
/// </summary>
/// <param name="id">Идентификатор объявления</param>
/// <param name="author">Автор объявления</param>
/// <param name="content">Текст объявления</param>
/// <param name="viewsCount">Количество просмотров объявления</param>
/// <param name="publishedAt">Момент публикации объявления</param>
/// <param name="hiddenAt">Момент сокрытия объявления</param>
public class AnnouncementSummary(
    Guid id,
    User author,
    string content,
    int viewsCount,
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
}