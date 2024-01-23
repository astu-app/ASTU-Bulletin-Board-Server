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
    private AttachmentList? _attachments;

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

    public AnnouncementBuilder Attach(AttachmentBase attachment)
    {
        AddAttachmentOrThrow(attachment);
        return this;
    }
    
    public AnnouncementBuilder Attach(AttachmentList attachments)
    {
        if (attachments is null)
            throw new ArgumentNullException(nameof(attachments));
        
        foreach (var attachment in attachments) 
            AddAttachmentOrThrow(attachment);

        return this;
    }

    public Announcement Build()
    {
        if (_id is null)
            throw new InvalidOperationException("Id объявления должен быть задан");
        
        if (string.IsNullOrWhiteSpace(_content))
            throw new InvalidOperationException("Контент объявления должен быть задан");
        if (_author is null)
            throw new InvalidOperationException("Автор объявления должен быть задан");
        if (_audience is null)
            throw new InvalidOperationException("Аудитория объявления должна быть задана");
    
        var announcement = new Announcement(
            id: _id ?? Guid.NewGuid(),
            content: _content,
            author: _author,
            categories: _categories ?? [],
            audience: _audience,
            publishedAt: _publishedAt,
            hiddenAt: _hiddenAt,
            autoPublishingAt: _autoPublishingAt,
            autoHidingAt: _autoHidingAt,
            attachments: _attachments ?? []
        );
    
        return announcement;
    }

    private void AddAttachmentOrThrow(AttachmentBase attachment)
    {
        ArgumentNullException.ThrowIfNull(attachment);

        _attachments ??= [];
        _attachments.Add(attachment);
        _containsSurvey = attachment is Survey;
    }
}