using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Domain.Models.Announcements.Exceptions;
using BulletInBoardServer.Domain.Models.JoinEntities;
using BulletInBoardServer.Services.Services.AnnouncementCategories.Exceptions;
using BulletInBoardServer.Services.Services.Announcements.Models;
using BulletInBoardServer.Services.Services.Attachments.Exceptions;
using BulletInBoardServer.Services.Services.Audience.Exceptions;
using AnnouncementAudience = BulletInBoardServer.Domain.Models.JoinEntities.AnnouncementAudience;

namespace BulletInBoardServer.Services.Services.Announcements.ServiceCore;

/// <summary>
/// Класс, инкапсулирующий логику создания объявления
/// </summary>
public class AnnouncementCreator
{
    private Announcement _announcement = null!; // Инициализируется при вызове единственного публичного метода Create

    private readonly Guid _authorId;
    private readonly CreateAnnouncement _create;

    private readonly ApplicationDbContext _dbContext;



    /// <summary>
    /// Класс, инкапсулирующий логику создания объявления
    /// </summary>
    /// <param name="authorId">Id автора объявления</param>
    /// <param name="create">Объект с необходимой для создания информацией</param>
    /// <param name="dbContext">Контекст базы данных</param>
    public AnnouncementCreator(Guid authorId, CreateAnnouncement create, ApplicationDbContext dbContext)
    {
        _authorId = authorId;
        _create = create;
        _dbContext = dbContext;
    }



    /// <summary>
    /// Создание объявления
    /// </summary>
    /// <returns>Созданное объявление</returns>
    /// <exception cref="AnnouncementContentNullOrEmptyException">Текстовое содержимое объявления null, пустой или состоит только из пробельных символов</exception>
    /// <exception cref="AnnouncementAudienceNullOrEmptyException">Аудитория объявления null или пуста</exception>
    /// <exception cref="InvalidOperationException">Момент отложенной публикации или сокрытия не были перенесены в создаваемое объявление</exception>
    public Announcement Create()
    {
        ContentValidOrThrow();
        AudienceValidOrThrow();
        DelayedMomentsCorrectOrThrow();

        _announcement = InitAnnouncement();
        _dbContext.Announcements.Add(_announcement);

        AddRelatedEntitiesToDb();
        _dbContext.SaveChanges();

        return _announcement;
    }

    private void ContentValidOrThrow()
    {
        if (string.IsNullOrWhiteSpace(_create.Content))
            throw new AnnouncementContentNullOrEmptyException();
    }

    private void AudienceValidOrThrow()
    {
        if (_create.UserIds is null || !_create.UserIds.Any())
            throw new AnnouncementAudienceNullOrEmptyException();
    }

    private void DelayedMomentsCorrectOrThrow()
    {
        var now = DateTime.Now;
        var publishAt = _create.DelayedPublishingAt;
        var hideAt = _create.DelayedHidingAt;

        if (publishAt is null && hideAt is null)
            return;

        if (publishAt is not null && hideAt is null)
        {
            DelayedPublishingMomentComesInFutureOrThrow(now, publishAt.Value);
            return;
        }

        if (publishAt is null && hideAt is not null)
        {
            DelayedHidingMomentComesInFutureOrThrow(now, hideAt.Value);
            return;
        }

        DelayedPublishingMomentComesInFutureOrThrow(now, publishAt!.Value);
        DelayedHidingMomentComesInFutureOrThrow(now, hideAt!.Value);
        MomentWillComeBeforeOrThrow<DelayedPublishingAfterDelayedHidingException>(publishAt.Value, hideAt.Value);
    }

    /// <summary>
    /// Метод проверяет, что момент отложенной публикации наступит позже текущего, или кидает исключение
    /// </summary>
    /// <param name="now">Текущий момент времени</param>
    /// <param name="moment">Проверяемый момент отложенной публикации</param>
    /// <exception cref="DelayedPublishingMomentComesInPastException">Генерируемое исключение</exception>
    private static void DelayedPublishingMomentComesInFutureOrThrow(DateTime now, DateTime moment) =>
        MomentWillComeBeforeOrThrow<DelayedPublishingMomentComesInPastException>(now, moment);

    /// <summary>
    /// Метод проверяет, что момент отложенного сокрытия наступит позже текущего, или кидает исключение
    /// </summary>
    /// <param name="now">Текущий момент времени</param>
    /// <param name="moment">Проверяемый момент отложенного сокрытия</param>
    /// <exception cref="DelayedHidingMomentComesInPastException">Генерируемое исключение</exception>
    private static void DelayedHidingMomentComesInFutureOrThrow(DateTime now, DateTime moment) =>
        MomentWillComeBeforeOrThrow<DelayedHidingMomentComesInPastException>(now, moment);

    /// <summary>
    /// Метод проверяет, что первый момент наступит до второго, и генерирует исключение в противном случае
    /// </summary>
    /// <param name="first">Первый момент</param>
    /// <param name="second">Второй момент</param>
    /// <typeparam name="TException">Тип генерируемого исключения</typeparam>
    private static void MomentWillComeBeforeOrThrow<TException>(DateTime first, DateTime second)
        where TException : InvalidOperationException
    {
        if (first >= second)
            throw Activator.CreateInstance<TException>();
    }

    private Announcement InitAnnouncement() =>
        new(
            id: Guid.NewGuid(),
            content: _create.Content,
            authorId: _authorId,
            publishedAt: null,
            hiddenAt: null,
            delayedPublishingAt: _create.DelayedPublishingAt,
            delayedHidingAt: _create.DelayedHidingAt
        );

    private void AddRelatedEntitiesToDb()
    {
        var audience = InitUserAudience(_create.UserIds);
        TryAddAnnouncementAudienceOrThrow(audience);

        var attachmentJoins = InitAttachmentJoins(_create.AttachmentIds);
        TryAddAttachmentsOrThrow(attachmentJoins);

        var categoryJoins = InitCategoryJoins(_create.CategoryIds);
        TryAddAnnouncementCategoriesOrThrow(categoryJoins);
    }

    private IEnumerable<AnnouncementAudience> InitUserAudience(IEnumerable<Guid> userIds)
    {
        var joins = new List<AnnouncementAudience>();
        foreach (var userId in userIds)
            joins.Add(new AnnouncementAudience(_announcement.Id, userId));

        return joins;
    }

    private void TryAddAnnouncementAudienceOrThrow(IEnumerable<AnnouncementAudience> audience)
    {
        try
        {
            _dbContext.AnnouncementAudience.AddRange(audience);
        }
        catch (InvalidOperationException err)
        {
            throw new PieceOfAudienceDoesNotExistException(err);
        }
    }

    private IEnumerable<AnnouncementAttachment> InitAttachmentJoins(IEnumerable<Guid> attachmentIds)
    {
        var joins = new List<AnnouncementAttachment>();
        foreach (var attachmentId in attachmentIds)
            joins.Add(new AnnouncementAttachment(_announcement.Id, attachmentId));

        return joins;
    }

    private void TryAddAttachmentsOrThrow(IEnumerable<AnnouncementAttachment> attachment)
    {
        try
        {
            _dbContext.AnnouncementAttachmentJoins.AddRange(attachment);
        }
        catch (InvalidOperationException err)
        {
            throw new AttachmentDoesNotExistException(err);
        }
    }

    private IEnumerable<AnnouncementAnnouncementCategory> InitCategoryJoins(IEnumerable<Guid> categoryIds)
    {
        var joins = new List<AnnouncementAnnouncementCategory>();
        foreach (var categoryId in categoryIds)
            joins.Add(new AnnouncementAnnouncementCategory(_announcement.Id, categoryId));

        return joins;
    }

    private void TryAddAnnouncementCategoriesOrThrow(IEnumerable<AnnouncementAnnouncementCategory> audience)
    {
        try
        {
            _dbContext.AnnouncementCategoryJoins.AddRange(audience);
        }
        catch (InvalidOperationException err)
        {
            throw new AnnouncementCategoryDoesNotExistException(err);
        }
    }
}