using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Domain.Models.Attachments;
using BulletInBoardServer.Domain.Models.JoinEntities;
using BulletInBoardServer.Services.Services.AnnouncementCategories.Exceptions;
using BulletInBoardServer.Services.Services.Announcements.Exceptions;
using BulletInBoardServer.Services.Services.Announcements.Models;
using BulletInBoardServer.Services.Services.Attachments.Exceptions;
using BulletInBoardServer.Services.Services.Audience.Exceptions;
using BulletInBoardServer.Services.Services.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using AnnouncementAudience = BulletInBoardServer.Domain.Models.JoinEntities.AnnouncementAudience;

namespace BulletInBoardServer.Services.Services.Announcements.ServiceCore;

/// <summary>
/// Класс, инкапсулирующий логику редактирования объявления
/// </summary>
public class AnnouncementRedactor
{
    private readonly Announcement _announcement;
    private readonly EditAnnouncement _edit;

    private readonly ApplicationDbContext _dbContext;



    /// <summary>
    /// Класс, инкапсулирующий логику редактирования объявления
    /// </summary>
    /// <param name="announcementSummary">Объявление с незагруженными связанными сущностями</param>
    /// <param name="edit">Объект с необходимой для редактирования информацией</param>
    /// <param name="dbContext">Контекст базы данных</param>
    public AnnouncementRedactor(Announcement announcementSummary, EditAnnouncement edit,
        ApplicationDbContext dbContext)
    {
        _announcement = announcementSummary;
        _edit = edit;
        _dbContext = dbContext;
    }



    /// <summary>
    /// Редактирование объявления
    /// </summary>
    /// <exception cref="OperationNotAllowedException">Пользователь не имеет права выполнить операцию</exception>
    /// <exception cref="AnnouncementContentEmptyException">Нельзя установить текстовое содержимое, которое является null, пустым или состоит только из пробельных символов</exception>
    /// <exception cref="AnnouncementAudienceEmptyException">Нельзя установить пустую аудиторию объявления</exception>
    /// <exception cref="CannotDetachSurveyException">От объявления нельзя открепить опрос</exception>
    public void Edit()
    {
        if (_edit.Content is not null)
        {
            NewContentValidOrThrow();
            _announcement.SetContent(_edit.Content);
        }

        if (_edit.AudienceIds is not null)
        {
            NewAudienceValidOrThrow();
            ApplyAudienceChanging();
        }

        if (_edit.CategoryIds is not null)
        {
            NewCategoriesValidOrThrow();
            ApplyCategoriesChanging();
        }

        if (_edit.AttachmentIds is not null)
        {
            NewAttachmentsValidOrThrow();
            ApplyAttachmentsChanging();
        }
    }



    private void NewContentValidOrThrow()
    {
        if (!string.IsNullOrWhiteSpace(_edit.Content))
            throw new AnnouncementContentEmptyException();
    }

    private void NewAudienceValidOrThrow()
    {
        if (_edit.AudienceIds is null)
            return;

        var toAdd = _edit.AudienceIds.ToAdd;
        var toRemove = _edit.AudienceIds.ToRemove;

        if (toRemove is not null && toAdd is not null)
            if (_announcement.AudienceSize - toRemove.Count + toAdd.Count <= 0)
                throw new AnnouncementAudienceEmptyException();

        if (toAdd is not null && !NewAudiencePresentedInDb(toAdd, _dbContext))
            throw new PieceOfAudienceDoesNotExistException();
    }

    private static bool NewAudiencePresentedInDb(ICollection<Guid> toAdd, ApplicationDbContext dbContext)
    {
        var allAudienceContainedInDb = dbContext.Users
            .Select(u => u.Id)
            .Intersect(toAdd)
            .Count() == toAdd.Count;
        return allAudienceContainedInDb;
    }

    private void ApplyAudienceChanging()
    {
        if (_edit.AudienceIds is null)
            throw new InvalidOperationException($"{nameof(_edit.AudienceIds)} не может быть null на данном этапе");

        var toRemove = _edit.AudienceIds.ToRemove;
        if (toRemove is not null)
        {
            _dbContext.AnnouncementAudience
                .Where(aa => aa.AnnouncementId == _announcement.Id && toRemove.Contains(aa.UserId))
                .ExecuteDelete();
        }

        var toAdd = _edit.AudienceIds.ToAdd;
        if (toAdd is not null)
        {
            var newAudience = toAdd.Select(id => new AnnouncementAudience(_announcement.Id, id));
            _dbContext.AnnouncementAudience.AddRange(newAudience);
        }
    }

    private void NewCategoriesValidOrThrow()
    {
        var toAdd = _edit.CategoryIds?.ToAdd;
        if (toAdd is null)
            return;

        if (!NewCategoriesPresentedInDb(toAdd, _dbContext))
            throw new AnnouncementCategoryDoesNotExistException();
    }

    private static bool NewCategoriesPresentedInDb(ICollection<Guid> toAdd, ApplicationDbContext dbContext)
    {
        var allIdsNotPresentedInDb = dbContext.AnnouncementCategories
            .Select(a => a.Id)
            .Intersect(toAdd)
            .Count() == toAdd.Count;

        return allIdsNotPresentedInDb;
    }

    private void ApplyCategoriesChanging()
    {
        if (_edit.CategoryIds is null)
            throw new InvalidOperationException($"{nameof(_edit.CategoryIds)} не может быть null на данном этапе");

        var toRemove = _edit.CategoryIds.ToRemove;
        if (toRemove is not null)
        {
            _dbContext.AnnouncementCategoryJoins
                .Where(aa =>
                    aa.AnnouncementId == _announcement.Id &&
                    toRemove.Contains(aa.AnnouncementCategoryId))
                .ExecuteDelete();
        }

        var toAdd = _edit.CategoryIds.ToAdd;
        if (toAdd is not null)
        {
            var newAudience = toAdd.Select(id => new AnnouncementAnnouncementCategory(_announcement.Id, id));
            _dbContext.AnnouncementCategoryJoins.AddRange(newAudience);
        }
    }

    private void NewAttachmentsValidOrThrow()
    {
        if (_edit.AttachmentIds is null)
            return;

        var toRemove = _edit.AttachmentIds.ToRemove;
        if (toRemove is not null && ToRemoveContainsSurvey(toRemove))
            throw new CannotDetachSurveyException();

        var toAdd = _edit.AttachmentIds.ToAdd;
        if (toAdd is not null && !NewAttachmentsPresentedInDb(toAdd))
            throw new AttachmentDoesNotExistException();
    }

    private bool ToRemoveContainsSurvey(IEnumerable<Guid> toRemove)
    {
        var containsSurvey = _dbContext.AnnouncementAttachmentJoins
            .Join(_dbContext.Attachments,
                aa => aa.AttachmentId,
                a => a.Id,
                (aa, a) => new { Join = aa, Attachment = a })
            .Any(join =>
                join.Join.AnnouncementId ==
                _announcement.Id && // рассматриваем вложения только изменяемого объявления.
                toRemove.Contains(join.Attachment.Id) && // отбираем только те вложения, которые будут откреплены
                join.Attachment.Type ==
                AttachmentTypes.Survey); // среди отобранных вложений проверяем наличие опросов

        return containsSurvey;
    }

    private bool NewAttachmentsPresentedInDb(ICollection<Guid> toAdd)
    {
        var allIdsPresentedInDb = _dbContext.Attachments
            .Select(a => a.Id)
            .Intersect(toAdd)
            .Count() == toAdd.Count;

        return allIdsPresentedInDb;
    }

    private void ApplyAttachmentsChanging()
    {
        if (_edit.AttachmentIds is null)
            throw new InvalidOperationException(
                $"{nameof(_edit.AttachmentIds)} не может быть null на данном этапе");

        var toRemove = _edit.AttachmentIds.ToRemove;
        if (toRemove is not null)
        {
            _dbContext.AnnouncementAttachmentJoins
                .Where(aa => aa.AnnouncementId == _announcement.Id && toRemove.Contains(aa.AttachmentId))
                .ExecuteDelete();
        }

        var toAdd = _edit.AttachmentIds.ToAdd;
        if (toAdd is not null)
        {
            var newAudience = toAdd.Select(id => new AnnouncementAttachment(_announcement.Id, id));
            _dbContext.AnnouncementAttachmentJoins.AddRange(newAudience);
        }
    }
}