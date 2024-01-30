using BulletInBoardServer.Models.Announcements;
using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Attachments;

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
    /// Количество объявлений, ссылающихся на файл
    /// </summary>
    public int LinksCount { get; private set; }

    /// <summary>
    /// Увеличение количества ссылок на файл
    /// </summary>
    public void AddLink() =>
        ++LinksCount;

    /// <summary>
    /// Уменьшение количества ссылок на файл
    /// </summary>
    public void RemoveLink() =>
        --LinksCount;



    /// <summary>
    /// Файл, который можно прикрепить к объявлению
    /// </summary>
    /// <param name="id">Идентификатор файла</param>
    /// <param name="uploaderId">Идентификатор загрузившего файл пользователя</param>
    /// <param name="name">Название файла</param>
    /// <param name="hash">Хэш файла в кодировке base64</param>
    /// <param name="linksCount">Количество объявлений, ссылающихся на файл</param>
    public File(Guid id, Guid uploaderId, string name, string hash, int linksCount)
        : this(id, [], uploaderId, name, hash, linksCount)
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
    /// <param name="linksCount">Количество объявлений, ссылающихся на файл</param>
    public File(Guid id, AnnouncementList announcements, Guid uploaderId, string name, string hash, int linksCount)
        : base(id, announcements, "File")
    {
        UploaderId = uploaderId;
        Name = name;
        Hash = hash;
        LinksCount = linksCount;
    }
}