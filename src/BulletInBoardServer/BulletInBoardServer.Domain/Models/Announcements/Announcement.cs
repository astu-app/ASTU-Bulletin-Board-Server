using BulletInBoardServer.Domain.Models.AnnouncementCategories;
using BulletInBoardServer.Domain.Models.Attachments;
using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Domain.Models.Announcements;

public class Announcement
{
    /// <summary>
    /// Идентификатор объявления
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Текстовое содержимое объявления
    /// </summary>
    public string Content
    {
        get => _content;
        set => SetContent(value);
    }

    /// <summary>
    /// Идентификатор автора объявления
    /// </summary>
    public Guid AuthorId { get; init; }

    /// <summary>
    /// Автор объявления
    /// </summary>
    public User Author { get; init; } = null!;

    /// <summary>
    /// Категории объявления
    /// </summary>
    public AnnouncementCategoryList Categories { get; private init; } = null!;

    /// <summary>
    /// Аудитория объявления
    /// </summary>
    public AnnouncementAudience Audience { get; private init; } = null!;

    /// <summary>
    /// Количество пользователей, просмотревших объявление
    /// </summary>
    /// <remarks>
    /// Свойство устанавливается из базы данных и в БД не сохраняется
    /// </remarks>
    public int ViewsCount { get; set; }

    /// <summary>
    /// Размер аудитории объявления
    /// </summary>
    public int AudienceSize { get; init; }

    /// <summary>
    /// Момент публикации уже опубликованного объявления. Null, если объявления не опубликовано
    /// </summary>
    public DateTime? PublishedAt
    {
        get => _publishedAt;
        set => SetPublishedMoment(value);
    }

    /// <summary>
    /// Опубликовано ли объявление в настоящий момент
    /// </summary>
    public bool IsPublished { get; private set; }

    /// <summary>
    /// Момент автоматической публикации объявления. Null, если автоматическая публикация не задана
    /// </summary>
    public DateTime? DelayedPublishingAt
    {
        get => _delayedPublishingAt;
        set => SetDelayedPublishingMoment(value);
    }

    /// <summary>
    /// Ожидает ли объявление отложенной автоматической публикации
    /// </summary>
    public bool ExpectsDelayedPublishing { get; private set; } // private set для EF

    /// <summary>
    /// Момент сокрытия уже скрытого объявления. Null, если объявления не скрыто
    /// </summary>
    public DateTime? HiddenAt
    {
        get => _hiddenAt;
        set => SetHiddenAtMoment(value);
    }

    /// <summary>
    /// Скрыто ли объявление в настоящий момент
    /// </summary>
    public bool IsHidden { get; private set; }

    /// <summary>
    /// Момент отложенного сокрытия объявления. Null, если отложенное сокрытие не задано
    /// </summary>
    public DateTime? DelayedHidingAt
    {
        get => _delayedHidingAt;
        set => SetDelayedHidingMoment(value);
    }

    /// <summary>
    /// Ожидает ли объявление отложенного автоматического сокрытия
    /// </summary>
    public bool ExpectsDelayedHiding { get; private set; } // private set для EF

    /// <summary>
    /// Вложения объявления
    /// </summary>
    public AttachmentList Attachments { get; private set; } = []; // private set для EF



    private string _content = null!;
    private DateTime? _delayedPublishingAt;
    private DateTime? _delayedHidingAt;
    private DateTime? _publishedAt;
    private DateTime? _hiddenAt;

    private bool ContainsSurvey => Attachments.Any(a => a is Survey);



    public Announcement(Guid id, string content, User author, AnnouncementCategoryList categories,
        AnnouncementAudience audience, DateTime? publishedAt, DateTime? hiddenAt,
        DateTime? delayedPublishingAt, DateTime? delayedHidingAt,
        AttachmentList attachments)
    {
        CategoriesValidOrThrow(categories);
        AudienceValidOrThrow(audience);

        Id = id;
        Content = content;
        AuthorId = author.Id;
        Author = author;
        Categories = categories;
        Audience = audience;
        AudienceSize = audience.Count;
        PublishedAt = publishedAt;
        HiddenAt = hiddenAt;
        DelayedPublishingAt = delayedPublishingAt;
        DelayedHidingAt = delayedHidingAt;

        if (attachments.Count > 0)
            Attach(attachments);
    }

    public Announcement(Guid id, string content, Guid authorId,
        DateTime? publishedAt, DateTime? hiddenAt,
        DateTime? delayedPublishingAt, DateTime? delayedHidingAt)
    {
        Id = id;
        Content = content;
        AuthorId = authorId;
        PublishedAt = publishedAt;
        HiddenAt = hiddenAt;
        DelayedPublishingAt = delayedPublishingAt;
        DelayedHidingAt = delayedHidingAt;
    }



    public void Publish(DateTime publishedAt)
    {
        if (PublishedAt is not null)
            throw new InvalidOperationException("Нельзя опубликовать уже опубликованное объявление");

        HiddenAt = null;
        PublishedAt = publishedAt;

        DelayedPublishingAt = null;
    }

    public void Hide(DateTime hiddenAt)
    {
        if (!IsPublished || IsHidden)
            throw new InvalidOperationException("Нельзя скрыть уже скрытое объявление");

        HiddenAt = hiddenAt;
        PublishedAt = null;

        DelayedHidingAt = null;
        DelayedPublishingAt = null;
    }

    public void Restore(DateTime restoredAt) =>
        Publish(restoredAt);



    private void Attach(AttachmentList attachments)
    {
        ArgumentNullException.ThrowIfNull(attachments);

        foreach (var attachment in attachments)
            AddAttachmentOrThrow(attachment);
    }

    private void SetContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Контент не может быть null или состоять только из пробельных символов");

        _content = content;
    }

    private static void CategoriesValidOrThrow(AnnouncementCategoryList categories) =>
        ArgumentNullException.ThrowIfNull(categories);

    private static void AudienceValidOrThrow(AnnouncementAudience audience)
    {
        ArgumentNullException.ThrowIfNull(audience);
        if (!audience.Any())
            throw new ArgumentException("Аудитория объявления не может быть пустой");
    }

    private void SetPublishedMoment(DateTime? publishedAt)
    {
        if (PublishedAt is not null && publishedAt is not null)
            throw new InvalidOperationException(
                "Время публикации уже опубликованного объявления не может быть изменено");

        if (DateTime.Now < publishedAt)
            throw new InvalidOperationException(
                "Момент публикации уже опубликованного объявления не может наступить в будущем");

        if (IsHidden && HiddenAt <= publishedAt)
            throw new InvalidOperationException(
                "Момент публикации уже опубликованного объявления не может наступить позже момента его сокрытия");

        _publishedAt = publishedAt;
        if (publishedAt is not null)
            IsPublished = true;
    }

    private void SetHiddenAtMoment(DateTime? hiddenAt)
    {
        if (HiddenAt is not null && hiddenAt is not null)
            throw new InvalidOperationException("Время сокрытия уже скрытого объявления не может быть изменено");

        if (DateTime.Now < hiddenAt)
            throw new InvalidOperationException(
                "Момент сокрытия уже скрытого объявления не может наступить в будущем");

        if (IsPublished && hiddenAt <= PublishedAt ||
            ExpectsDelayedPublishing && hiddenAt < DelayedPublishingAt)
            throw new InvalidOperationException("Объявление не могло быть скрыто до момента публикации");

        _hiddenAt = hiddenAt;
        if (hiddenAt is not null)
            IsHidden = true;
    }

    private void SetDelayedPublishingMoment(DateTime? publishAt)
    {
        if (publishAt is null)
        {
            _delayedPublishingAt = publishAt;
            return;
        }

        if (IsPublished)
            throw new InvalidOperationException(
                "Нельзя задать момент автоматической публикации уже опубликованному объявлению");

        if (publishAt < DateTime.Now)
            throw new InvalidOperationException(
                "Автоматическая публикация объявления не может произойти в прошлом");

        _delayedPublishingAt = publishAt;
    }

    private void SetDelayedHidingMoment(DateTime? hideAt)
    {
        if (hideAt is null)
        {
            _delayedHidingAt = null;
            return;
        }

        if (IsHidden)
            throw new InvalidOperationException(
                "Нельзя задать срок автоматического сокрытия уже скрытому объявлению");

        _delayedHidingAt = hideAt;
    }

    private void AddAttachmentOrThrow(AttachmentBase attachment)
    {
        ArgumentNullException.ThrowIfNull(attachment);

        var isSurvey = attachment is Survey;
        if (isSurvey && ContainsSurvey)
            throw new InvalidOperationException("Объявление уже содержит опрос");

        Attachments.Add(attachment);
    }
}