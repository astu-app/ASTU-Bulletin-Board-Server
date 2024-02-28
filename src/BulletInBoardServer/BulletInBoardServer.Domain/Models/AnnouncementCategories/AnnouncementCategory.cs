using System.Text.RegularExpressions;
using BulletInBoardServer.Domain.Models.AnnouncementCategories.Exceptions;
using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Domain.Models.AnnouncementCategories;

/// <summary>
/// Категория объявлений
/// </summary>
public class AnnouncementCategory
{
    /// <summary>
    /// Категория объявлений
    /// </summary>
    /// <param name="id">Идентификатор категории объявлений</param>
    /// <param name="name">Название категории объявлений</param>
    /// <param name="colorHex">Цвет категории объявлений в пользовательском интерфейсе</param>
    /// <exception cref="AnnouncementCategoryNameIsNullOrWhitespace">Название категории объявлений null, пустое или состоит только из пробельных символов</exception>
    /// <exception cref="ColorIsInvalidHexException">Цвет категории объявлений представлен в формате, отличном от HEX</exception>
    public AnnouncementCategory(Guid id, string name, string colorHex)
        : this(id, name, colorHex, [])
    {
    }

    /// <summary>
    /// Категория объявлений
    /// </summary>
    /// <param name="id">Идентификатор категории объявлений</param>
    /// <param name="name">Название категории объявлений</param>
    /// <param name="colorHex">Цвет категории объявлений в пользовательском интерфейсе</param>
    /// <param name="subscribers">Подписчики категории объявлений</param>
    /// <exception cref="AnnouncementCategoryNameIsNullOrWhitespace">Название категории объявлений null, пустое или состоит только из пробельных символов</exception>
    /// <exception cref="ColorIsInvalidHexException">Цвет категории объявлений представлен в формате, отличном от HEX</exception>
    public AnnouncementCategory(Guid id, string name, string colorHex, SubscriberList subscribers)
    {
        NameValidOrThrow(name);
        ColorIsValidHexOrThrow(colorHex);
        
        Id = id;
        Name = name;
        ColorHex = colorHex;
        Subscribers = subscribers;
    }

    /// <summary>
    /// Идентификатор категории объявлений
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Название категории объявлений 
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Цвет категории объявлений в пользовательском интерфейсе 
    /// </summary>
    public string ColorHex { get; private set; }

    /// <summary>
    /// Подписчики категории объявлений
    /// </summary>
    public SubscriberList Subscribers { get; }



    /// <summary>
    /// Установка названия категории объявлений
    /// </summary>
    /// <param name="name">Название</param>
    /// <exception cref="AnnouncementCategoryNameIsNullOrWhitespace">Название категории объявлений null, пустое или состоит только из пробельных символов</exception>
    public void SetName(string name) =>
        Name = name;

    /// <summary>
    /// Установка цвета категории объявлений
    /// </summary>
    /// <param name="colorHex">Код цвета категории в HEX формате</param>
    /// <exception cref="ColorIsInvalidHexException">Цвет категории объявлений представлен в формате, отличном от HEX</exception>
    public void SetColor(string colorHex)
    {
        ColorIsValidHexOrThrow(colorHex);
        ColorHex = colorHex;
    }
    
    public void Subscribe(User user)
    {
        if (Subscribers.Contains(user))
            throw new InvalidOperationException("Пользователь уже подписан на категорию объявлений");

        Subscribers.Add(user);
    }

    public void Unsubscribe(User user)
    {
        Subscribers.Remove(user);
    }



    private static void NameValidOrThrow(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new AnnouncementCategoryNameIsNullOrWhitespace();
    }
    
    private static void ColorIsValidHexOrThrow(string color)
    {
        var valid = Regex.IsMatch(color, "#[0-9a-fA-F]{6}");
        if (!valid)
            throw new ColorIsInvalidHexException();
    }
}