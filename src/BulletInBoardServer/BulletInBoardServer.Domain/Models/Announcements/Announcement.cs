using BulletInBoardServer.Domain.Models.Announcements.Exceptions;
using BulletInBoardServer.Domain.Models.Attachments;
using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using BulletInBoardServer.Domain.Models.Users;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

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
    public string Content { get; private set; } 

    /// <summary>
    /// Идентификатор автора объявления
    /// </summary>
    public Guid AuthorId { get; init; }

    /// <summary>
    /// Автор объявления
    /// </summary>
    public User Author { get; init; } = null!;

    /// <summary>
    /// Аудитория объявления
    /// </summary>
    public AnnouncementAudience Audience { get; set; } = [];
    
    /// <summary>
    /// Корневой узел аудитории объявления, представленной в виде дерева (узел - группа пользователей, лист -
    /// пользователь)
    /// </summary>
    // public __AudienceNode? AudienceThreeNode { get; set; } // todo remove
    

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
    public int AudienceSize { get; set; }

    /// <summary>
    /// Момент публикации уже опубликованного объявления. Null, если объявления не опубликовано
    /// </summary>
    public DateTime? PublishedAt { get; private set; } 

    /// <summary>
    /// Опубликовано ли объявление в настоящий момент
    /// </summary>
    public bool IsPublished { get; private set; }
    
    /// <summary>
    /// Момент автоматической публикации объявления. Null, если автоматическая публикация не задана
    /// </summary>
    public DateTime? DelayedPublishingAt { get; private set; } 

    /// <summary>
    /// Ожидает ли объявление отложенной автоматической публикации
    /// </summary>
    public bool ExpectsDelayedPublishing { get; private set; } 

    /// <summary>
    /// Момент сокрытия уже скрытого объявления. Null, если объявления не скрыто
    /// </summary>
    public DateTime? HiddenAt { get; private set; } 

    /// <summary>
    /// Скрыто ли объявление в настоящий момент
    /// </summary>
    public bool IsHidden { get; private set; }

    /// <summary>
    /// Момент отложенного сокрытия объявления. Null, если отложенное сокрытие не задано
    /// </summary>
    public DateTime? DelayedHidingAt { get; private set; } 

    /// <summary>
    /// Ожидает ли объявление отложенного автоматического сокрытия
    /// </summary>
    public bool ExpectsDelayedHiding { get; private set; } 

    /// <summary>
    /// Вложения объявления
    /// </summary>
    public AttachmentList Attachments { get; private set; } = []; 


    
    private bool ContainsSurvey => Attachments.Any(a => a is Survey);



    public Announcement(Guid id, string content, User author,
        AnnouncementAudience audience, DateTime? publishedAt, DateTime? hiddenAt,
        DateTime? delayedPublishingAt, DateTime? delayedHidingAt,
        AttachmentList attachments)
    {
        AudienceValidOrThrow(audience);

        Id = id;
        Content = content;
        AuthorId = author.Id;
        Author = author;
        Audience = audience;
        AudienceSize = audience.Count;
        PublishedAt = publishedAt;
        HiddenAt = hiddenAt;
        DelayedPublishingAt = delayedPublishingAt;
        DelayedHidingAt = delayedHidingAt;

        if (attachments.Count > 0)
            Attach(attachments);
    }

    public Announcement(Guid id, string content, Guid authorId, int audienceSize,
        DateTime? publishedAt, DateTime? hiddenAt,
        DateTime? delayedPublishingAt, DateTime? delayedHidingAt)
    {
        Id = id;
        Content = content;
        AuthorId = authorId;
        AudienceSize = audienceSize;
        PublishedAt = publishedAt;
        HiddenAt = hiddenAt;
        DelayedPublishingAt = delayedPublishingAt;
        DelayedHidingAt = delayedHidingAt;
    }



    /// <summary>
    /// Публикация объявления
    /// </summary>
    /// <param name="now">Текущий момент времени</param>
    /// <param name="publishingMoment">Момент публикации объявления</param>
    /// <exception cref="InvalidOperationException">Генерируется, если объявление уже опубликовано</exception>
    public void Publish(DateTime now, DateTime publishingMoment)
    {
        if (PublishedAt is not null)
            throw new InvalidOperationException("Нельзя опубликовать уже опубликованное объявление");

        SetHiddenMoment(now, null);
        SetPublishedMoment(now, publishingMoment);

        DelayedPublishingAt = null;
    }

    /// <summary>
    /// Сокрытие объявления
    /// </summary>
    /// <param name="now">Текущий момент времени</param>
    /// <param name="hidingMoment">Момент сокрытия объявления</param>
    /// <exception cref="AnnouncementNotYetPublishedException">Генерируется, если объявление еще не опубликовано</exception>
    /// <exception cref="AnnouncementAlreadyHiddenException">Генерируется, если объявление уже скрыто</exception>
    public void Hide(DateTime now, DateTime hidingMoment)
    {
        if (!IsPublished)
            throw new AnnouncementNotYetPublishedException();
        if (IsHidden)
            throw new AnnouncementAlreadyHiddenException();

        SetHiddenMoment(now, hidingMoment);
        SetPublishedMoment(now, null);

        DelayedHidingAt = null;
        DelayedPublishingAt = null;
    }

    /// <summary>
    /// Восстановить объявление
    /// </summary>
    /// <param name="now">Текущий момент времени</param>
    /// <param name="restoringMoment">Момент восстановления</param>
    /// <exception cref="AnnouncementNotHiddenException">Генерируется, если объявление не скрыто</exception>
    public void Restore(DateTime now, DateTime restoringMoment)
    {
        if (!IsHidden)
            throw new AnnouncementNotHiddenException();
        
        HiddenAt = null;
        SetPublishedMoment(now, restoringMoment);

        DelayedPublishingAt = null;
    }
    
    /// <summary>
    /// Установка текстового содержимого объявления
    /// </summary>
    /// <param name="content">Текстовое содержимое объявления</param>
    /// <exception cref="AnnouncementContentNullOrEmptyException">Текстовое содержимое null или состоит только из пробельных символов</exception>
    public void SetContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new AnnouncementContentNullOrEmptyException();

        Content = content;
    }
    
    /// <summary>
    /// Установка момента публикации объявления
    /// </summary>
    /// <param name="now">Текущий момент времени</param>
    /// <param name="publishedAt">Момент публикации объявления</param>
    /// <exception cref="InvalidOperationException">
    /// Генерируется в следующих случаях:
    /// <list type="bullet">
    ///   <item>Время публикации уже опубликованного объявления не может быть изменено</item>
    ///   <item>Момент публикации уже опубликованного объявления не может наступить в будущем</item>
    ///   <item>Момент публикации уже опубликованного объявления не может наступить позже момента его сокрытия</item>
    /// </list>
    /// </exception>
    public void SetPublishedMoment(DateTime now, DateTime? publishedAt)
    {
        if (PublishedAt is not null && publishedAt is not null)
            throw new InvalidOperationException(
                "Время публикации уже опубликованного объявления не может быть изменено");

        if (now < publishedAt)
            throw new InvalidOperationException("Момент публикации объявления не может наступить в будущем");

        if (IsHidden && HiddenAt <= publishedAt)
            throw new InvalidOperationException(
                "Момент публикации уже опубликованного объявления не может наступить позже момента его сокрытия");

        PublishedAt = publishedAt;
        if (publishedAt is not null)
            IsPublished = true;
    }
    
    /// <summary>
    /// Установка момента сокрытия объявления
    /// </summary>
    /// <param name="now">Текущий момент времени</param>
    /// <param name="hiddenAt">Момент сокрытия объявления</param>
    /// <exception cref="InvalidOperationException">
    /// Генерируется в следующих случаях:
    /// <list type="bullet">
    ///   <item>Время сокрытия уже скрытого объявления не может быть изменено</item>
    ///   <item>Момент сокрытия уже скрытого объявления не может наступить в будущем</item>
    ///   <item>Объявление не могло быть скрыто до момента публикации</item>
    /// </list>
    /// </exception>
    public void SetHiddenMoment(DateTime now, DateTime? hiddenAt)
    {
        if (HiddenAt is not null && hiddenAt is not null)
            throw new InvalidOperationException("Время сокрытия уже скрытого объявления не может быть изменено");

        if (now < hiddenAt)
            throw new InvalidOperationException("Момент сокрытия объявления не может наступить в будущем");

        if (IsPublished && hiddenAt <= PublishedAt ||
            ExpectsDelayedPublishing && hiddenAt < DelayedPublishingAt)
            throw new InvalidOperationException("Объявление не могло быть скрыто до момента публикации");

        HiddenAt = hiddenAt;
        if (hiddenAt is not null)
            IsHidden = true;
    }
    
    /// <summary>
    /// Установка момента отложенной публикации
    /// </summary>
    /// <param name="now">Текущий момент времени</param>
    /// <param name="publishingMoment">Момент, в который объявление должно быть опубликовано</param>
    /// <exception cref="DelayedPublishingMomentComesInPastException">Момент отложенной публикации не может наступить в прошлом</exception>
    /// <exception cref="DelayedPublishingAfterDelayedHidingException">Момент отложенного сокрытия не может раньше момента отложенной публикации</exception>
    /// <exception cref="AutoPublishAnAlreadyPublishedAnnouncementException">Нельзя установить момент отложенной публикации уже опубликованному объявлению</exception>
    public void SetDelayedPublishingMoment(DateTime now, DateTime? publishingMoment)
    {
        if (publishingMoment is null)
        {
            DelayedPublishingAt = publishingMoment;
            return;
        }

        if (IsPublished)
            throw new AutoPublishAnAlreadyPublishedAnnouncementException();
        
        if (DelayedHidingAt < publishingMoment)
            throw new DelayedPublishingAfterDelayedHidingException();

        if (publishingMoment < now)
            throw new DelayedPublishingMomentComesInPastException();

        DelayedPublishingAt = publishingMoment;
    }
    
    /// <summary>
    /// Установка момента отложенного сокрытия
    /// </summary>
    /// <param name="now">Текущий момент времени</param>
    /// <param name="hidingMoment">Момент, в который объявление должно быть скрыто</param>
    /// <exception cref="DelayedHidingMomentComesInPastException">Момент отложенного сокрытия не может наступить в прошлом</exception>
    /// <exception cref="DelayedPublishingAfterDelayedHidingException">Момент отложенного сокрытия не может раньше момента отложенной публикации</exception>
    /// <exception cref="AutoHidingAnAlreadyHiddenAnnouncementException">Нельзя установить момент отложенного сокрытия уже скрытому объявлению</exception>
    public void SetDelayedHidingMoment(DateTime now, DateTime? hidingMoment)
    {
        if (hidingMoment is null) 
        {
            DelayedHidingAt = null;
            return;
        }

        if (hidingMoment < now)
            throw new DelayedHidingMomentComesInPastException();

        if (hidingMoment < DelayedPublishingAt)
            throw new DelayedPublishingAfterDelayedHidingException();

        if (IsHidden)
            throw new AutoHidingAnAlreadyHiddenAnnouncementException();

        DelayedHidingAt = hidingMoment;
    }



    private void Attach(AttachmentList attachments)
    {
        ArgumentNullException.ThrowIfNull(attachments);

        foreach (var attachment in attachments)
            AddAttachmentOrThrow(attachment);
    }

    private static void AudienceValidOrThrow(AnnouncementAudience audience)
    {
        ArgumentNullException.ThrowIfNull(audience);
        if (!audience.Any())
            throw new ArgumentException("Аудитория объявления не может быть пустой");
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