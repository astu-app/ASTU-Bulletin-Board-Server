using BulletInBoardServer.Models.Announcements.Attachments;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys;
using BulletInBoardServer.Models.Announcements.Categories;
using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Announcements;

public class Announcement
{
    /// <summary>
    /// Идентификатор объявления
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Текстовое содержимое объявления
    /// </summary>
    public string Content
    {
        get => _content;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(
                    "Контент не может быть null или состоять только из пробельных символов");
            _content = value;
        }
    }

    /// <summary>
    /// Автор объявления
    /// </summary>
    public User Author
    {
        get => _author;
        set => _author = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Категории объявления
    /// </summary>
    public AnnouncementCategories Categories { get;  }

    /// <summary>
    /// Аудитория объявления
    /// </summary>
    public AnnouncementAudience Audience { get; }

    /// <summary>
    /// Момент публикации уже опубликованного объявления. Null, если объявления не опубликовано
    /// </summary>
    public DateTime? PublishedAt
    {
        get => _publishedAt;
        set => SetPublishedMoment(value);
    }

    public bool IsPublished => PublishedAt is not null;

    /// <summary>
    /// Момент автоматической публикации объявления. Null, если автоматическая публикация не задана
    /// </summary>
    public DateTime? AutoPublishingAt
    {
        get => _autoPublishingAt;
        set => SetAutoPublishingMoment(value);
    }

    public bool ExpectsAutoPublishing => AutoPublishingAt is not null;

    /// <summary>
    /// Момент сокрытия уже скрытого объявления. Null, если объявления не скрыто
    /// </summary>
    public DateTime? HiddenAt
    {
        get => _hiddenAt;
        set => SetHiddenAtMoment(value);
    }

    public bool IsHidden => HiddenAt is not null;

    /// <summary>
    /// Момент автоматического сокрытия объявления. Null, если автоматическок сокрытие не задано
    /// </summary>
    public DateTime? AutoHidingAt
    {
        get => _autoHidingAt;
        set => SetAutoHidingMoment(value);
    }

    public bool ExpectsAutoHiding => AutoHidingAt is not null;

    /// <summary>
    /// Прикрепление к объявлению
    /// </summary>
    public ReadOnlyAttachments Attachments => _attachments.AsReadOnly();



    private string _content = null!;
    private User _author = null!;
    private DateTime? _autoPublishingAt;
    private DateTime? _autoHidingAt;
    private DateTime? _publishedAt;
    private DateTime? _hiddenAt;
    private readonly Attachments.Attachments _attachments;

    private bool _containsSurvey;



    public Announcement(Guid id, string content, User author, AnnouncementCategories categories,
        AnnouncementAudience audience, DateTime? publishedAt, DateTime? hiddenAt, DateTime? autoPublishingAt,
        DateTime? autoHidingAt, Attachments.Attachments attachments)
    {
        CategoriesValidOrThrow(categories);
        AudienceValidOrThrow(audience);  
        
        Id = id;
        Content = content;
        Author = author;
        Categories = categories;
        Audience = audience;
        PublishedAt = publishedAt;
        HiddenAt = hiddenAt;
        AutoPublishingAt = autoPublishingAt;
        AutoHidingAt = autoHidingAt;
        
        Attach(attachments);
    }



    public void Attach(IAttachment attachment) => 
        AddAttachmentOrThrow(attachment);

    public void Attach(Attachments.Attachments attachments)
    {
        if (attachments is null)
            throw new ArgumentNullException(nameof(attachments));
        
        foreach (var attachment in attachments) 
            AddAttachmentOrThrow(attachment);
    }

    public void Unattach(IAttachment attachment)
    {
        _attachments.Remove(attachment);
        _containsSurvey = attachment is Survey;
    }



    private static void CategoriesValidOrThrow(AnnouncementCategories categories)
    {
        if (categories is null)
            throw new ArgumentNullException(nameof(categories));
    }

    private static void AudienceValidOrThrow(AnnouncementAudience audience)
    {
        if (audience is null)
            throw new ArgumentNullException(nameof(audience));
        if (!audience.Any())
            throw new ArgumentException("Аудитория объявления не может быть пустой");      
    }

    private void SetPublishedMoment(DateTime? publishedAt)
    {
        if (PublishedAt is not null)
            throw new InvalidOperationException("Время публикации объявления не может быть изменено");

        if (DateTime.UtcNow < publishedAt)
            throw new InvalidOperationException(
                "Момент публикации уже опубликованного объявления не может наступить в будущем");

        if (IsHidden && HiddenAt <= publishedAt)
            throw new InvalidOperationException(
                "Момент публикации уже опубликованного объявления не может наступить позже момента его сокрытия");

        _publishedAt = publishedAt;
    }

    private void SetHiddenAtMoment(DateTime? hiddenAt)
    {
        if (DateTime.UtcNow < hiddenAt)
            throw new InvalidOperationException(
                "Момент сокрытия уже скрытого объявления не может наступить в будущем");

        if (IsPublished && hiddenAt <= PublishedAt ||
            ExpectsAutoPublishing && hiddenAt < AutoPublishingAt)
            throw new InvalidOperationException("Объявление не могло быть скрыто до момента публикации");

        _hiddenAt = hiddenAt;
    }

    private void SetAutoPublishingMoment(DateTime? publishAt)
    {
        if (publishAt is null)
        {
            _autoPublishingAt = null;
            return;
        }

        if (IsPublished)
            throw new InvalidOperationException(
                "Нельзя задать момент автоматической публикации уже опубликованному объявлению");

        if (publishAt < DateTime.UtcNow)
            throw new InvalidOperationException(
                "Автоматическая публикация объявления не молжет пройзойти в прошлом");

        _autoPublishingAt = publishAt;
    }

    private void SetAutoHidingMoment(DateTime? hideAt)
    {
        if (hideAt is null)
        {
            _autoHidingAt = null;
            return;
        }

        if (IsHidden)
            throw new InvalidOperationException(
                "Нельзя задать срок автоматического сокрытия уже скрытому объявлению");

        if (hideAt < DateTime.UtcNow)
            throw new InvalidOperationException(
                "Автоматическое сокрытие объявления не молжет пройзойти в прошлом");

        _autoHidingAt = hideAt;
    }
    
    private void AddAttachmentOrThrow(IAttachment attachment)
    {
        if (attachment is null)
            throw new ArgumentNullException(nameof(attachment));

        var isSurvey = attachment is Survey; 
        if (isSurvey && _containsSurvey)
            throw new InvalidOperationException("Объявление уже содержит опрос");
        
        _attachments.Add(attachment);
        _containsSurvey = isSurvey;
    }
}