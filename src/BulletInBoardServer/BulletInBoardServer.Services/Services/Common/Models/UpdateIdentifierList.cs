namespace BulletInBoardServer.Services.Services.Common.Models;

/// <summary>
/// Объект для обновления списка идентификаторов, прикрепленных к какой-либо сущности
/// </summary>
public class UpdateIdentifierList
{
    /// <summary>
    /// Идентификаторы, которые необходимо добавить
    /// </summary>
    public ICollection<Guid>? ToAdd { get; init; } = [];

    /// <summary>
    /// Идентификаторы, которые необходимо удалить
    /// </summary>
    public ICollection<Guid>? ToRemove { get; init; } = [];
}