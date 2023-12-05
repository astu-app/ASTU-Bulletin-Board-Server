using BulletInBoardServer.Models.Announcements.Attachments;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys;
using BulletInBoardServer.Models.Announcements.Categories;
using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Announcements;

public class AnnouncementBuilder
{
    private Guid? _id;
    private string? _content;
    private User? _author;
    private AnnouncementCategories? _categories;
    private AnnouncementAudience? _audience;
    private DateTime? _publishedAt;
    private DateTime? _hiddenAt;
    private DateTime? _autoPublishingAt;
    private DateTime? _autoHidingAt;
    private Attachments.Attachments _attachments = new();

    private bool _containsSurvey;



    public AnnouncementBuilder SetId(Guid id)
    {
        _id = id;
        return this;
    }

    public AnnouncementBuilder SetContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Контент сообщения не может быть пустым");

        _content = content;
        return this;
    }

    public AnnouncementBuilder SetAuthor(User author)
    {
        _author = author ?? throw new ArgumentNullException(nameof(author));
        return this;
    }

    public AnnouncementBuilder SetCategories(AnnouncementCategories categories)
    {
        _categories = categories ?? throw new ArgumentNullException(nameof(categories));
        return this;
    }

    public AnnouncementBuilder SetAudience(AnnouncementAudience audience)
    {
        _audience = audience ?? throw new ArgumentNullException(nameof(audience));
        return this;
    }

    public AnnouncementBuilder SetPublished(DateTime publishedAt)
    {
        _publishedAt = publishedAt;
        return this;
    }

    public AnnouncementBuilder SetHidden(DateTime hiddenAt)
    {
        _hiddenAt = hiddenAt;
        return this;
    }

    public AnnouncementBuilder AutoPublishAt(DateTime publishAt)
    {
        _autoPublishingAt = publishAt;
        return this;
    }

    public AnnouncementBuilder AutoHideAt(DateTime hideAt)
    {
        _autoHidingAt = hideAt;
        return this;
    }

    public AnnouncementBuilder Attach(IAttachment attachment)
    {
        AddAttachmentOrThrow(attachment);
        return this;
    }
    
    public AnnouncementBuilder Attach(Attachments.Attachments attachments)
    {
        if (attachments is null)
            throw new ArgumentNullException(nameof(attachments));
        
        foreach (var attachment in attachments) 
            AddAttachmentOrThrow(attachment);

        return this;
    }

    public Announcement Build()
    {
        if (string.IsNullOrWhiteSpace(_content))
            throw new InvalidOperationException("Контент сообщения должен быть задан");
        if (_author is null)
            throw new InvalidOperationException("Автор сообщения должен быть задан");
        if (_audience is null)
            throw new InvalidOperationException("Аудитория сообщения должна быть задана");

        var announcement = new Announcement(
            _id ?? Guid.NewGuid(),
            _content,
            _author,
            _categories ?? new AnnouncementCategories(),
            _audience,
            _publishedAt,
            _hiddenAt,
            _autoPublishingAt,
            _autoHidingAt,
            _attachments
        );

        return announcement;
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