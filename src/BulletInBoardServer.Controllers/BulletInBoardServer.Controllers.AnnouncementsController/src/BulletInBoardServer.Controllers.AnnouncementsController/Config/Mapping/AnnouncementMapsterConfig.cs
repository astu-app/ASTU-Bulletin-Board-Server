using System.Linq;
using BulletInBoardServer.Controllers.AnnouncementsController.Models;
using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Domain.Models.Attachments;
using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Answers;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Questions;
using BulletInBoardServer.Domain.Models.Users;
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
            .Map(d => d.Files, s => s.Attachments.OfType<File>())
            .Map(d => d.Surveys, s => s.Attachments.OfType<Survey>())
            .Map(d => d.HiddenAt, s => s.HiddenAt)
            .Map(d => d.PublishedAt, s => s.PublishedAt)
            .Map(d => d.DelayedPublishingAt, s => s.DelayedPublishingAt)
            .Map(d => d.DelayedHidingAt, s => s.DelayedHidingAt)
            // .Map(d => d.Audience, s => s.AudienceThreeNode); // todo remove
            .Map(d => d.Audience, s => s.Audience);

        config.NewConfig<User, AnnouncementAudienceUser>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.FirstName, s => s.FirstName)
            .Map(d => d.SecondName, s => s.SecondName)
            .Map(d => d.Patronymic, s => s.Patronymic);

        // config.NewConfig<IAudienceNode, AnnouncementAudienceDto>() // remove
        //     .ConstructUsing(src => new AnnouncementAudienceDto 
        //     { 
        //         RootNode = src.Adapt<AnnouncementAudienceDtoRootNode>()
        //     });

        // config.NewConfig<__AudienceNode, AnnouncementAudienceDtoRootNode>()
        //     // src is user
        //     .Map(dst => dst.Id, src => src.User.Id, src => src.User != null)
        //     .Map(dst => dst.FirstName, src => src.User.FirstName, src => src.User != null)
        //     .Map(dst => dst.SecondName, src => src.User.SecondName, src => src.User != null)
        //     .Map(dst => dst.Patronymic, src => src.User.Patronymic, src => src.User != null)
        //     // src is usergroup
        //     .Map(dst => dst.Id, src => src.UserGroup.Id, src => src.UserGroup != null)
        //     .Map(dst => dst.Nodes, src => src.UserGroup.ChildrenGroups, src => src.UserGroup != null); // todo мб добавить Adapt на ChildrenGroups

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
            .Map(d => d.AudienceSize, s => s.AudienceSize)
            .Map(d => d.PublishedAt, s => s.PublishedAt)
            .Map(d => d.Files, a => a.Attachments.OfType<File>())
            .Map(d => d.Surveys, a => a.Attachments.OfType<Survey>());

        config.NewConfig<File, FileSummaryDto>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Name, s => s.Name)
            .Map(d => d.SizeInBytes, s => s.SizeInBytes);

        config.NewConfig<Survey, SurveyDetailsDto>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.IsOpen, s => s.IsOpen)
            .Map(d => d.IsAnonymous, s => s.IsAnonymous)
            .Map(d => d.VotersAmount, s => s.VotersCount)
            .Map(d => d.AutoClosingAt, s => s.AutoClosingAt)
            .Map(d => d.VoteFinishedAt, s => s.VoteFinishedAt)
            .Map(d => d.Questions, s => s.Questions);
            // .Map(d => d.Questions, s => s.Questions.Select(q => q.Adapt<QuestionDetailsDto>()).ToList());

        // config.NewConfig<QuestionList, List<QuestionDetailsDto>>()
        //     .MapWith(s => s.Select(q => q.Adapt<QuestionDetailsDto>()).ToList());
        
        config.NewConfig<Question, QuestionDetailsDto>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Content, s => s.Content)
            .Map(d => d.IsMultipleChoiceAllowed, s => s.IsMultipleChoiceAllowed)
            .Map(d => d.Answers, s => s.Answers);

        config.NewConfig<Answer, QuestionAnswerDetailsDto>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Content, s => s.Content)
            .Map(d => d.VotersAmount, s => s.VotersCount);
    }
}