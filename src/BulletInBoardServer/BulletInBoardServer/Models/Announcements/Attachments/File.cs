﻿using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Announcements.Attachments;

/// <summary>
/// Файл, который можно прикрепить к объявлению
/// </summary>
/// <param name="id">Идентификатор файла</param>
/// <param name="name">Название файла</param>
/// <param name="hash">Хэш файла в кодировке base64</param>
/// <param name="linksCount">Количество объявлений, ссылающихся на файл</param>
public class File(Guid id, Guid uploaderId, string name, string hash, int linksCount)
    : AttachmentBase(id, "File")
{
    /// <summary>
    /// Идентификатор пользователя, впервые загрузившего файл на сервер
    /// </summary>
    public Guid UploaderId { get; } = uploaderId;

    /// <summary>
    /// Пользователь, впервые загрузивший файл на сервер
    /// </summary>
    /// <remarks>
    /// Поле должно устанавливаться только при помощи Entity Framework.
    /// Перед использование обязательно должно быть установлено
    /// </remarks>
    public User Uploader { get; init; } = null!;
    
    /// <summary>
    /// Название файла
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Хэш файла в кодировке base64
    /// </summary>
    public string Hash  { get; } = hash;
    
    /// <summary>
    /// Количество объявлений, ссылающихся на файл
    /// </summary>
    public int LinksCount { get; private set; } = linksCount;

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
}