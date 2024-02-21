using BulletInBoardServer.Domain.Models.Announcements;

namespace BulletInBoardServer.Domain.Models.Attachments;

/// <summary>
/// Базовый класс для вложения объявлений
/// </summary>
/// <param name="id">Идентификатор вложения</param>
/// <param name="type">Тип вложения</param>
public abstract class AttachmentBase(Guid id, AnnouncementList announcements, string type)
{
    /// <summary>
    /// Идентификатор вложения
    /// </summary>
    public Guid Id { get; init; } = id;

    /// <summary>
    /// Объявление, к которому прикреплено вложение
    /// </summary>
    public AnnouncementList Announcements { get; init; } = announcements;

    /// <summary>
    /// Тип вложения
    /// </summary>
    public string Type { get; init; } = type;
}