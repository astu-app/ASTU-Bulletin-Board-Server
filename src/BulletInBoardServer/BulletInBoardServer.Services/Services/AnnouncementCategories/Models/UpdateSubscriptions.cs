using BulletInBoardServer.Services.Services.Common.Models;

namespace BulletInBoardServer.Services.Services.AnnouncementCategories.Models;

/// <summary>
/// Объект с необходимой информацией об обновлении списка подписок пользователя на категории объявлений
/// </summary>
/// <param name="Update">Объект с необходимой информацией об обновлении списка идентификаторов категорий объявлений</param>
public record UpdateSubscriptions(UpdateIdentifierList Update);