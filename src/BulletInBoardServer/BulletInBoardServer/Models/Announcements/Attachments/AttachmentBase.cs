namespace BulletInBoardServer.Models.Announcements.Attachments;

/// <summary>
/// Базовый класс для вложения объявлений
/// </summary>
/// <param name="id">Идентификатор вложения</param>
/// <param name="type">Тип вложения</param>
public abstract class AttachmentBase(Guid id, string type)
{
    /// <summary>
    /// Идентификатор вложения
    /// </summary>
    public Guid Id { get; } = id;

    /// <summary>
    /// Тип вложения
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public string Type { get; } = type;
}