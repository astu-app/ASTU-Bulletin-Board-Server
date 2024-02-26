using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Domain.Models.Announcements.Exceptions;
using BulletInBoardServer.Services.Services.Announcements;
using BulletInBoardServer.Services.Services.Announcements.Exceptions;
using BulletInBoardServer.Services.Services.Announcements.Models;
using BulletInBoardServer.Services.Services.Announcements.ServiceCore;
using BulletInBoardServer.Services.Test.Services.Announcements.DelayedOperations;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Test.Infrastructure;
using Test.Infrastructure.DbInvolvingTests;
using static BulletInBoardServer.Domain.TestDbFiller.TestDataIds;

namespace BulletInBoardServer.Services.Test.Services.Announcements;

[TestSubject(typeof(AnnouncementService))]
public class AnnouncementServiceTest : DbInvolvingTestBase
{
    private readonly DelayedAnnouncementOperationsDispatcherMock _dispatcher;
    private readonly AnnouncementService _announcementService;

    public AnnouncementServiceTest()
    {
        var scopeFactory = ServiceScopeFactoryConfigurator.Configure(DbContext);
        
        _dispatcher = new DelayedAnnouncementOperationsDispatcherMock();
        _announcementService = new AnnouncementService(
            new GeneralOperationsService(scopeFactory, _dispatcher),
            new PublishedAnnouncementService(scopeFactory, _dispatcher),
            new HiddenAnnouncementService(scopeFactory, _dispatcher),
            new DelayedPublicationAnnouncementService(scopeFactory),
            new DelayedHidingAnnouncementService(scopeFactory));
    }

    /* ********************************** Общие операции *********************************** */

    // Create
    [Fact]
    public void Create_NullContent_Throws()
    {
        var createAnnouncement = new CreateAnnouncement(
            content: null!,
            userIds: [MainUsergroupAdminId],
            categoryIds: [],
            attachmentIds: [],
            delayedPublishingAt: null,
            delayedHidingAt: null
        );

        var create = () => _announcementService.Create(MainUsergroupAdminId, createAnnouncement);

        create.Should().ThrowExactly<AnnouncementContentNullOrEmptyException>();
    }

    [Fact]
    public void Create_WhitespaceContent_Throws()
    {
        var createAnnouncement = new CreateAnnouncement(
            content: "   ",
            userIds: [MainUsergroupAdminId],
            categoryIds: [],
            attachmentIds: [],
            delayedPublishingAt: null,
            delayedHidingAt: null
        );

        var create = () => _announcementService.Create(MainUsergroupAdminId, createAnnouncement);

        create.Should().ThrowExactly<AnnouncementContentNullOrEmptyException>();
    }

    [Fact]
    public void Create_NonExistingAuthorId_Throws()
    {
        var authorId = Guid.NewGuid();
        var createAnnouncement = new CreateAnnouncement(
            content: "qwerty",
            userIds: [MainUsergroupAdminId],
            categoryIds: [],
            attachmentIds: [],
            delayedPublishingAt: null,
            delayedHidingAt: null
        );

        var create = () => _announcementService.Create(authorId, createAnnouncement);

        create.Should().ThrowExactly<DbUpdateException>();
    }

    [Fact]
    public void Create_ExpectsDelayedPublishing_DoesNotPublishInstant()
    {
        var createAnnouncement = new CreateAnnouncement(
            content: "qwerty",
            userIds: [MainUsergroupAdminId],
            categoryIds: [],
            attachmentIds: [],
            delayedPublishingAt: DateTime.Now.AddDays(1),
            delayedHidingAt: null
        );

        var created = _announcementService.Create(MainUsergroupAdminId, createAnnouncement);

        created.PublishedAt.Should().BeNull();
    }

    [Fact]
    public void Create_DelayedHidingMomentNotInFuture_Throws()
    {
        var createAnnouncement = new CreateAnnouncement(
            content: "qwerty",
            userIds: [MainUsergroupAdminId],
            categoryIds: [],
            attachmentIds: [],
            delayedPublishingAt: DateTime.Now.Subtract(TimeSpan.FromMinutes(1)),
            delayedHidingAt: null
        );

        var create = () => _announcementService.Create(MainUsergroupAdminId, createAnnouncement);

        create.Should().ThrowExactly<DelayedPublishingMomentComesInPastException>();
    }

    [Fact]
    public void Create_DelayedPublishingMomentNotInFuture_Throws()
    {
        var createAnnouncement = new CreateAnnouncement(
            content: "qwerty",
            userIds: [MainUsergroupAdminId],
            categoryIds: [],
            attachmentIds: [],
            delayedPublishingAt: null,
            delayedHidingAt: DateTime.Now.Subtract(TimeSpan.FromMinutes(1))
        );

        var create = () => _announcementService.Create(MainUsergroupAdminId, createAnnouncement);

        create.Should().ThrowExactly<DelayedHidingMomentComesInPastException>();
    }

    [Fact]
    public void Create_DelayedPublishingAfterDelayedHiding_Throws()
    {
        var createAnnouncement = new CreateAnnouncement(
            content: "qwerty",
            userIds: [MainUsergroupAdminId],
            categoryIds: [],
            attachmentIds: [],
            delayedPublishingAt: DateTime.Now.AddHours(5),
            delayedHidingAt: DateTime.Now.AddHours(1)
        );

        var create = () => _announcementService.Create(MainUsergroupAdminId, createAnnouncement);

        create.Should().ThrowExactly<DelayedPublishingAfterDelayedHidingException>();
    }

    [Fact]
    public void Create_NonExistingUserIds_Throws()
    {
        var createAnnouncement = new CreateAnnouncement(
            content: "qwerty",
            userIds: [MainUsergroupAdminId, Guid.NewGuid()],
            categoryIds: [],
            attachmentIds: [],
            delayedPublishingAt: null,
            delayedHidingAt: null
        );

        var create = () => _announcementService.Create(MainUsergroupAdminId, createAnnouncement);

        create.Should().ThrowExactly<DbUpdateException>();
    }

    [Fact]
    public void Create_NonExistingCategoryIds_Throws()
    {
        var createAnnouncement = new CreateAnnouncement(
            content: "qwerty",
            userIds: [MainUsergroupAdminId],
            categoryIds: [Guid.NewGuid()],
            attachmentIds: [],
            delayedPublishingAt: null,
            delayedHidingAt: null
        );

        var create = () => _announcementService.Create(MainUsergroupAdminId, createAnnouncement);

        create.Should().ThrowExactly<DbUpdateException>();
    }

    [Fact]
    public void Create_NonExistingAttachmentIds_Throws()
    {
        var createAnnouncement = new CreateAnnouncement(
            content: "qwerty",
            userIds: [MainUsergroupAdminId],
            categoryIds: [],
            attachmentIds: [PublicSurvey_1_Id, Guid.NewGuid()],
            delayedPublishingAt: null,
            delayedHidingAt: null
        );

        var create = () => _announcementService.Create(MainUsergroupAdminId, createAnnouncement);

        create.Should().ThrowExactly<DbUpdateException>();
    }

    [Fact]
    public void Create_SetDelayedPublishingMoment_CallsConfigurationOfDelayedPublishing()
    {
        var createAnnouncement = new CreateAnnouncement(
            content: "qwerty",
            userIds: [MainUsergroupAdminId],
            categoryIds: [],
            attachmentIds: [],
            delayedPublishingAt: DateTime.Now.AddDays(1),
            delayedHidingAt: null
        );

        _announcementService.Create(MainUsergroupAdminId, createAnnouncement);

        _dispatcher.ConfigureDelayedPublishingCalled.Should().Be(1);
    }

    [Fact]
    public void Create_SetDelayedHidingMoment_CallsConfigurationOfDelayedHiding()
    {
        var createAnnouncement = new CreateAnnouncement(
            content: "qwerty",
            userIds: [MainUsergroupAdminId],
            categoryIds: [],
            attachmentIds: [],
            delayedPublishingAt: null,
            delayedHidingAt: DateTime.Now.AddDays(1)
        );

        _announcementService.Create(MainUsergroupAdminId, createAnnouncement);

        _dispatcher.ConfigureDelayedHidingCalled.Should().Be(1);
    }

    // Edit
    [Fact]
    public void Edit_AddAudience_AudienceAddedCorrectly()
    {
        Guid[] newUserIds = [UsualUser_2_Id];
        var beforeChanging = LoadAnnouncement(FullyFilledAnnouncement_1_Id);
        DbContext.ChangeTracker.Clear();

        var newAudience = beforeChanging.Audience
            .Select(u => u.Id)
            .Concat(newUserIds)
            .ToList();
        var edit = new EditAnnouncement
        {
            Id = FullyFilledAnnouncement_1_Id,
            AudienceIds = newAudience,
        };
        _announcementService.Edit(MainUsergroupAdminId, edit);

        var afterChanging = LoadAnnouncement(FullyFilledAnnouncement_1_Id);
        afterChanging.Audience
            .Should()
            .HaveCount(newAudience.Count)
            .And
            .Contain(u => newAudience.Contains(u.Id));
    }

    [Fact]
    public void Edit_RemoveAudience_AudienceRemovedCorrectly()
    {
        Guid[] removingUserIds = [UsualUser_1_Id];
        var beforeChanging = LoadAnnouncement(FullyFilledAnnouncement_1_Id);
        DbContext.ChangeTracker.Clear();

        var newAudience = beforeChanging.Audience
            .Where(u => !removingUserIds.Contains(u.Id))
            .Select(u => u.Id)
            .ToList();
        var edit = new EditAnnouncement
        {
            Id = FullyFilledAnnouncement_1_Id,
            AudienceIds = newAudience,
        };
        _announcementService.Edit(MainUsergroupAdminId, edit);

        var afterChanging = LoadAnnouncement(FullyFilledAnnouncement_1_Id);
        afterChanging.Audience
            .Should()
            .HaveCount(newAudience.Count)
            .And
            .NotContain(u => removingUserIds.Contains(u.Id));
    }

    [Fact]
    public void Edit_AddCategories_CategoriesAddedCorrectly()
    {
        Guid[] newCategoryIds = [AnnouncementCategory_3_Id];
        var beforeChanging = LoadAnnouncement(FullyFilledAnnouncement_1_Id);
        DbContext.ChangeTracker.Clear();

        var changedCategories = beforeChanging.Categories
            .Select(ac => ac.Id)
            .Concat(newCategoryIds)
            .ToList();
        var edit = new EditAnnouncement
        {
            Id = FullyFilledAnnouncement_1_Id,
            CategoryIds = changedCategories,
        };
        _announcementService.Edit(MainUsergroupAdminId, edit);

        var afterChanging = LoadAnnouncement(FullyFilledAnnouncement_1_Id);
        afterChanging.Categories
            .Should()
            .HaveCount(changedCategories.Count)
            .And
            .Contain(ac => newCategoryIds.Contains(ac.Id));
    }

    [Fact]
    public void Edit_RemoveCategories_CategoriesRemovedCorrectly()
    {
        Guid[] removingCategoryIds = [UsualUser_1_Id];
        var beforeChanging = LoadAnnouncement(FullyFilledAnnouncement_1_Id);
        DbContext.ChangeTracker.Clear();

        var changedCategories = beforeChanging.Categories
            .Where(u => !removingCategoryIds.Contains(u.Id))
            .Select(u => u.Id)
            .ToList();
        var edit = new EditAnnouncement
        {
            Id = FullyFilledAnnouncement_1_Id,
            CategoryIds = changedCategories,
        };
        _announcementService.Edit(MainUsergroupAdminId, edit);

        var afterChanging = LoadAnnouncement(FullyFilledAnnouncement_1_Id);
        afterChanging.Categories
            .Should()
            .HaveCount(changedCategories.Count)
            .And
            .NotContain(ac => removingCategoryIds.Contains(ac.Id));
    }

    [Fact]
    public void Edit_AddAttachments_AttachmentsAddedCorrectly()
    {
        Guid[] newAttachmentIds = [File_2_Id];
        var beforeChanging = LoadAnnouncement(FullyFilledAnnouncement_1_Id);
        DbContext.ChangeTracker.Clear();

        var changedAttachments = beforeChanging.Attachments
            .Select(a => a.Id)
            .Concat(newAttachmentIds)
            .ToList();
        var edit = new EditAnnouncement
        {
            Id = FullyFilledAnnouncement_1_Id,
            AttachmentIds = changedAttachments,
        };
        _announcementService.Edit(MainUsergroupAdminId, edit);

        var afterChanging = LoadAnnouncement(FullyFilledAnnouncement_1_Id);
        afterChanging.Attachments
            .Should()
            .HaveCount(changedAttachments.Count)
            .And
            .Contain(a => newAttachmentIds.Contains(a.Id));
    }

    [Fact]
    public void Edit_RemoveAttachments_AttachmentsRemovedCorrectly()
    {
        Guid[] removingAttachmentIds = [File_1_Id];
        var beforeChanging = LoadAnnouncement(FullyFilledAnnouncement_1_Id);
        DbContext.ChangeTracker.Clear();

        var changedAttachments = beforeChanging.Attachments
            .Where(u => !removingAttachmentIds.Contains(u.Id))
            .Select(u => u.Id)
            .ToList();
        var edit = new EditAnnouncement
        {
            Id = FullyFilledAnnouncement_1_Id,
            AttachmentIds = changedAttachments,
        };
        _announcementService.Edit(MainUsergroupAdminId, edit);

        var afterChanging = LoadAnnouncement(FullyFilledAnnouncement_1_Id);
        afterChanging.Attachments
            .Should()
            .HaveCount(changedAttachments.Count)
            .And
            .NotContain(ac => removingAttachmentIds.Contains(ac.Id));
    }

    [Fact]
    public void Edit_EnableDelayedPublishing_CallsEnablingOfDelayedPublishing()
    {
        var edit = new EditAnnouncement
        {
            Id = HiddenAnnouncementWithDisabledDelayedMomentsId,
            DelayedPublishingAtChanged = true,
            DelayedPublishingAt = DateTime.Now.AddDays(1),
        };

        _announcementService.Edit(MainUsergroupAdminId, edit);

        _dispatcher.ReconfigureDelayedPublishingCalled.Should().Be(1);
    }

    [Fact]
    public void Edit_EnableDelayedHidingOnPublishedAnnouncement_CallsEnablingOfDelayedHiding()
    {
        var edit = new EditAnnouncement
        {
            Id = AnnouncementWithPublicSurvey_1_Id,
            DelayedHidingAtChanged = true,
            DelayedHidingAt = DateTime.Now.AddDays(1),
        };

        _announcementService.Edit(MainUsergroupAdminId, edit);

        _dispatcher.ReconfigureDelayedHidingCalled.Should().Be(1);
    }

    [Fact]
    public void Edit_EnableDelayedHidingOnHiddenAnnouncement_Throws()
    {
        var edit = new EditAnnouncement
        {
            Id = HiddenAnnouncementWithDisabledDelayedMomentsId,
            DelayedHidingAtChanged = true,
            DelayedHidingAt = DateTime.Now.AddDays(1),
        };

        var editAction = () => _announcementService.Edit(MainUsergroupAdminId, edit);

        editAction.Should().ThrowExactly<AutoHidingAnAlreadyHiddenAnnouncementException>();
    }

    [Fact]
    public void Edit_EditDelayedPublishingMoment_CallsReconfigurationOfDelayedPublishing()
    {
        var edit = new EditAnnouncement
        {
            Id = AnnouncementWithFilledDelayedMomentsId,
            DelayedPublishingAtChanged = true,
            DelayedPublishingAt = DateTime.Now.AddHours(12)
        };

        _announcementService.Edit(MainUsergroupAdminId, edit);

        _dispatcher.ReconfigureDelayedPublishingCalled.Should().Be(1);
    }

    [Fact]
    public void Edit_EditDelayedHidingMoment_CallsReconfigurationOfHidingPublishing()
    {
        var edit = new EditAnnouncement
        {
            Id = AnnouncementWithFilledDelayedMomentsId,
            DelayedHidingAtChanged = true,
            DelayedHidingAt = DateTime.Now.AddDays(3)
        };

        _announcementService.Edit(MainUsergroupAdminId, edit);

        _dispatcher.ReconfigureDelayedHidingCalled.Should().Be(1);
    }

    [Fact]
    public void Edit_DisableDelayedPublishing_CallsDisablingOfDelayedPublishing()
    {
        var edit = new EditAnnouncement
        {
            Id = AnnouncementWithFilledDelayedMomentsId,
            DelayedPublishingAtChanged = true,
            DelayedPublishingAt = null,
        };

        _announcementService.Edit(MainUsergroupAdminId, edit);

        _dispatcher.DisableDelayedPublishingCalled.Should().Be(1);
    }

    [Fact]
    public void Edit_DisableDelayedHiding_CallsDisablingOfDelayedHiding()
    {
        var edit = new EditAnnouncement
        {
            Id = AnnouncementWithFilledDelayedMomentsId,
            DelayedHidingAtChanged = true,
            DelayedHidingAt = null,
        };

        _announcementService.Edit(MainUsergroupAdminId, edit);

        _dispatcher.DisableDelayedHidingCalled.Should().Be(1);
    }

    // Delete
    [Fact]
    public void Delete_AnnouncementExpectsDelayedPublishing_CallsDisablingOfDelayedPublishing()
    {
        var removeId = AnnouncementWithFilledDelayedMomentsId;

        _announcementService.Delete(MainUsergroupAdminId, removeId);

        _dispatcher.DisableDelayedPublishingCalled.Should().Be(1);
    }

    [Fact]
    public void Delete_AnnouncementExpectsDelayedHiding_CallsDisablingOfDelayedHiding()
    {
        var removeId = AnnouncementWithFilledDelayedMomentsId;

        _announcementService.Delete(MainUsergroupAdminId, removeId);

        _dispatcher.DisableDelayedHidingCalled.Should().Be(1);
    }

    // Publish
    [Fact]
    public void Publish_PublishedAnnouncement_Throws()
    {
        var announcementId = AnnouncementWithPublicSurvey_1_Id;

        var publish = () => _announcementService.Publish(MainUsergroupAdminId, announcementId, DateTime.Now);

        publish.Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void Publish_HiddenAnnouncementExpectDelayedPublishing_CallsDisablingOfDelayedPublishing()
    {
        var announcementId = HiddenAnnouncementWithEnabledDelayedPublishingId;

        _announcementService.Publish(MainUsergroupAdminId, announcementId, DateTime.Now);

        _dispatcher.DisableDelayedPublishingCalled.Should().Be(1);
    }


    /* ***************************** Опубликованные объявления ***************************** */

    // GetPublishedAnnouncements
    [Fact]
    public void GetPublishedAnnouncements_UserHasRelatedPublishedAnnouncements_AllRelatedPublishedAnnouncements()
    {
        var userId = UsualUser_1_Id;
        Guid[] expectedAnnouncementIds =
        [
            AnnouncementWithPublicSurvey_1_Id,
            AnnouncementWithAnonymousSurvey_1_Id,
            AnnouncementWithClosedAnonymousSurvey_1_Id,
            FullyFilledAnnouncement_1_Id,
        ];

        var actualAnnouncementIds = _announcementService.GetPublishedAnnouncements(userId)
            .Select(a => a.Id);

        actualAnnouncementIds.Should().BeEquivalentTo(expectedAnnouncementIds);
    }

    [Fact]
    public void GetPublishedAnnouncements_UserHasNoRelatedAnnouncements_EmptyList()
    {
        var userId = UsualUser_2_Id;

        var actualAnnouncementIds = _announcementService.GetPublishedAnnouncements(userId)
            .Select(a => a.Id);

        actualAnnouncementIds.Should().BeEmpty();
    }

    // Hide
    [Fact]
    public void Hide_NonPublishedAnnouncement_Throws()
    {
        var announcementId = AnnouncementWithFilledDelayedMomentsId;

        var hide = () => _announcementService.Hide(MainUsergroupAdminId, announcementId, DateTime.Now);

        hide.Should().ThrowExactly<AnnouncementNotYetPublishedException>();
    }

    [Fact]
    public void Hide_AnnouncementThatExpectDelayedHiding_CallsDisablingOfDelayedHiding()
    {
        var announcementId = PublishedAnnouncementWithEnabledDelayedHidingId;

        _announcementService.Hide(MainUsergroupAdminId, announcementId, DateTime.Now);

        _dispatcher.DisableDelayedHidingCalled.Should().Be(1);
    }

    [Fact]
    public void Hide_AnnouncementNotExpectingDelayedHiding_DoesNotCallDisablingOfDelayedHiding()
    {
        var announcementId = PublishedAnnouncementWithDisabledDelayedHidingId;

        _announcementService.Hide(MainUsergroupAdminId, announcementId, DateTime.Now);

        _dispatcher.DisableDelayedHidingCalled.Should().Be(0);
    }


    /* ******************************** Скрытые объявления ********************************* */

    // GetHiddenAnnouncements
    [Fact]
    public void GetHiddenAnnouncements_UserHasHiddenAnnouncements_AllRelatedHiddenAnnouncements()
    {
        var userId = MainUsergroupAdminId;
        Guid[] expectedAnnouncementIds =
        [
            FullyFilledAnnouncement_1_Id,
            HiddenAnnouncementWithDisabledDelayedMomentsId,
            HiddenAnnouncementWithEnabledDelayedPublishingId,
        ];

        var actualAnnouncementIds = _announcementService.GetHiddenAnnouncements(userId)
            .Select(a => a.Id);

        actualAnnouncementIds.Should().BeEquivalentTo(expectedAnnouncementIds);
    }

    [Fact]
    public void GetHiddenAnnouncements_UserHasNoHiddenAnnouncements_EmptyList()
    {
        var userId = UsualUser_1_Id;

        var actualAnnouncementIds = _announcementService.GetHiddenAnnouncements(userId)
            .Select(a => a.Id);

        actualAnnouncementIds.Should().BeEmpty();
    }

    // Restore
    [Fact]
    public void Restore_PublishedAnnouncement_Throws()
    {
        var announcementId = PublishedAnnouncementWithDisabledDelayedHidingId;

        var restore = () => _announcementService.Restore(MainUsergroupAdminId, announcementId, DateTime.Now);

        restore.Should().ThrowExactly<AnnouncementNotHiddenException>();
    }

    [Fact]
    public void Restore_AnnouncementExpectingDelayedPublishing_CallsDisablingOfDelayedPublishing()
    {
        var announcementId = HiddenAnnouncementWithEnabledDelayedPublishingId;

        _announcementService.Restore(MainUsergroupAdminId, announcementId, DateTime.Now);

        _dispatcher.DisableDelayedPublishingCalled.Should().Be(1);
    }

    [Fact]
    public void Hide_AnnouncementNotExpectingDelayedPublishing_DoesNotCallDisablingOfDelayedPublishing()
    {
        var announcementId = HiddenAnnouncementWithDisabledDelayedMomentsId;

        _announcementService.Restore(MainUsergroupAdminId, announcementId, DateTime.Now);

        _dispatcher.DisableDelayedPublishingCalled.Should().Be(0);
    }


    /* ******************************* Отложенные объявления ******************************* */

    // GetDelayedPublicationAnnouncements
    [Fact]
    public void GetDelayedPublicationAnnouncements_UserHasDelayedPublicationAnnouncements_AllRelatedAnnouncements()
    {
        var userId = MainUsergroupAdminId;
        Guid[] expectedIds =
        [
            AnnouncementWithFilledDelayedMomentsId,
            HiddenAnnouncementWithEnabledDelayedPublishingId,
        ];

        var actualIds = _announcementService.GetDelayedPublicationAnnouncements(userId)
            .Select(a => a.Id);

        actualIds.Should().BeEquivalentTo(expectedIds);
    }

    // GetDelayedHidingAnnouncementsForUser
    [Fact]
    public void GetDelayedHidingAnnouncementsForUser_UserHasDelayedHidingAnnouncements_AllRelatedAnnouncements()
    {
        var userId = MainUsergroupAdminId;
        Guid[] expectedIds =
        [
            AnnouncementWithFilledDelayedMomentsId,
            PublishedAnnouncementWithEnabledDelayedHidingId,
        ];

        var actualIds = _announcementService.GetDelayedHidingAnnouncementsForUser(userId)
            .Select(a => a.Id);

        actualIds.Should().BeEquivalentTo(expectedIds);
    }



    private Announcement LoadAnnouncement(Guid announcementId) =>
        DbContext.Announcements
            .Where(a => a.Id == announcementId)
            .Include(a => a.Author)
            .Include(a => a.Audience)
            .Include(a => a.Categories)
            .Include(a => a.Attachments)
            .Single();
}