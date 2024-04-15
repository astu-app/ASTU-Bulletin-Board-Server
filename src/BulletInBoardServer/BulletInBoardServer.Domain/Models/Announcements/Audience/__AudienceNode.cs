using BulletInBoardServer.Domain.Models.UserGroups;
using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Domain.Models.Announcements.Audience;

/// <summary>
/// Узел дерева аудитории объявления
/// </summary>
public class __AudienceNode // todo remove
{
    public __AudienceNode(User? user = null, UserGroup? userGroup = null, IEnumerable<__AudienceNode>? children = null)
    {
        User = user;
        UserGroup = userGroup;
        Children = children;
    }

    // Такая структура класса обусловлена удобством передачи данных по API
    
    /// <summary>
    /// Пользователь
    /// </summary>
    public User? User { get; set; }
    
    /// <summary>
    /// Группа пользователей
    /// </summary>
    public UserGroup? UserGroup { get; set; }
    
    /// <summary>
    /// Дочерние узлы
    /// </summary>
    public IEnumerable<__AudienceNode>? Children { get; set; }
}