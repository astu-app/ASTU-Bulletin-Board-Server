using System.Linq;
using BulletInBoardServer.Controllers.AnnouncementsController.Models;
using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Domain.Models.Attachments;
using BulletInBoardServer.Services.Services.Announcements.Models;
using Mapster;

namespace BulletInBoardServer.Controllers.AnnouncementsController.Config.Mapping;

// ReSharper disable once UnusedType.Global - will be used on startup while IRegisters scanning
/// <inheritdoc />
public class AnnouncementMapsterConfig : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateAnnouncementDto, CreateAnnouncement>()
            .ConstructUsing(src => new CreateAnnouncement(
                src.Content, 
                src.UserIds, 
                src.CategoryIds,
                src.AttachmentIds, 
                src.DelayedPublishingAt, 
                src.DelayedHidingAt));

        config.NewConfig<Announcement, AnnouncementDetailsDto>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Content, s => s.Content)
            .Map(d => d.AuthorName, s => $"{s.Author.FirstName} {s.Author.SecondName} {s.Author.Patronymic}")
            .Map(d => d.ViewsCount, s => s.ViewsCount)
            .Map(d => d.AudienceSize, s => s.AudienceSize)
            .Map(d => d.Categories, s => s.Categories)
            .Map(d => d.Files, s => s.Attachments.Where(a => a.Type == AttachmentTypes.File))
            .Map(d => d.Surveys, s => s.Attachments.Where(a => a.Type == AttachmentTypes.Survey))
            .Map(d => d.HiddenAt, s => s.HiddenAt)
            .Map(d => d.PublishedAt, s => s.PublishedAt)
            .Map(d => d.DelayedPublishingAt, s => s.DelayedPublishingAt)
            .Map(d => d.DelayedHidingAt, s => s.DelayedHidingAt);

        config.NewConfig<UpdateAnnouncementDto, EditAnnouncement>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Content, s => s.Content)
            .Map(d => d.CategoryIds, s => s.CategoryIds)
            .Map(d => d.AudienceIds, s => s.AudienceIds)
            .Map(d => d.AttachmentIds, s => s.AttachmentIds)
            .Map(d => d.DelayedPublishingAtChanged, s => s.DelayedPublishingAtChanged)
            .Map(d => d.DelayedPublishingAt, s => s.DelayedPublishingAt, s => s.DelayedPublishingAtChanged)
            .Map(d => d.DelayedHidingAtChanged, s => s.DelayedHidingAtChanged)
            .Map(d => d.DelayedHidingAt, s => s.DelayedHidingAt, s => s.DelayedHidingAtChanged);

        config.NewConfig<AnnouncementSummary, AnnouncementSummaryDto>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.AuthorName, s => $"{s.Author.FirstName} {s.Author.SecondName} {s.Author.Patronymic}")
            .Map(d => d.Content, s => s.Content)
            .Map(d => d.Content, s => s.Content)
            .Map(d => d.ViewsCount, s => s.ViewsCount)
            .Map(d => d.PublishedAt, s => s.PublishedAt);
    }
}