using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Domain.Models.Attachments;

/// <summary>
/// Файл, который можно прикрепить к объявлению
/// </summary>
public class File : AttachmentBase
{
    /// <summary>
    /// Идентификатор пользователя, впервые загрузившего файл на сервер
    /// </summary>
    public Guid UploaderId { get; }

    /// <summary>
    /// Пользователь, впервые загрузивший файл на сервер
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework или конструктора.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public User Uploader { get; init; } = null!;

    /// <summary>
    /// Название файла
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Хэш файла в кодировке base64
    /// </summary>
    public string Hash { get; }



    /// <summary>
    /// Файл, который можно прикрепить к объявлению
    /// </summary>
    /// <param name="id">Идентификатор файла</param>
    /// <param name="uploaderId">Идентификатор загрузившего файл пользователя</param>
    /// <param name="name">Название файла</param>
    /// <param name="hash">Хэш файла в кодировке base64</param>
    public File(Guid id, Guid uploaderId, string name, string hash)
        : this(id, [], uploaderId, name, hash)
    {
    }



    /// <summary>
    /// Файл, который можно прикрепить к объявлению
    /// </summary>
    /// <param name="id">Идентификатор файла</param>
    /// <param name="announcements">Объявления, к которым прикреплен файл</param>
    /// <param name="uploaderId">Идентификатор загрузившего файл пользователя</param>
    /// <param name="name">Название файла</param>
    /// <param name="hash">Хэш файла в кодировке base64</param>
    public File(Guid id, AnnouncementList announcements, Guid uploaderId, string name, string hash)
        : base(id, announcements, AttachmentTypes.File)
    {
        UploaderId = uploaderId;
        Name = name;
        Hash = hash;
    }
}