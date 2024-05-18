using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Domain.Models.Users;
using FluentAssertions;
using JetBrains.Annotations;

namespace BulletInBoardServer.Domain.Test.Models.Announcements;

[TestSubject(typeof(Announcement))]
public class AnnouncementTest
{
    [Fact]
    public void SetContent_NullString_Throws()
    {
        var announcement = CreateValidAnnouncement();

        var setContent = () => announcement.SetContent(null!);
        setContent.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void SetContent_EmptyString_Throws()
    {
        var announcement = CreateValidAnnouncement();

        var setContent = () => announcement.SetContent(string.Empty);
        setContent.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void SetContent_WhiteSpaceString_Throws()
    {
        var announcement = CreateValidAnnouncement();

        var whitespaces = new[] { " ", "\t", "\n", "\r", "\v", "\f" };
        foreach (var whitespace in whitespaces)
        {
            var setContent = () => announcement.SetContent(whitespace);
            setContent.Should().Throw<ArgumentException>();
        }
    }



    [Fact]
    public void SetPublishedMoment_CurrentMoment_Passes()
    {
        var announcement = CreateValidAnnouncement();
        var now = DateTime.Now;
        
        var setPublishedMoment = () => announcement.SetPublishedMoment(now: now, publishedAt: now);
        setPublishedMoment.Should().NotThrow();
    }

    [Fact]
    public void SetPublishedMoment_MomentFromFuture_Throws()
    {
        var announcement = CreateValidAnnouncement();

        var setPublishedMoment = () => announcement.SetPublishedMoment(DateTime.Now, DateTime.Now.AddHours(1));
        setPublishedMoment.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Момент публикации объявления не может наступить в будущем");
    }

    [Fact]
    public void SetPublishedMoment_MomentFromPast_Passes()
    {
        var announcement = CreateValidAnnouncement();

        var setPublishedMoment = () =>
            announcement.SetPublishedMoment(DateTime.Now, DateTime.Now.Subtract(TimeSpan.FromHours(1)));
        setPublishedMoment.Should().NotThrow();
    }

    [Fact]
    public void SetPublishedMoment_ChangeValueFromNotNull_Throws()
    {
        var announcement = CreateValidAnnouncement();
        announcement.SetPublishedMoment(DateTime.Now, DateTime.MinValue);

        var changePublishedAt = () => announcement.SetPublishedMoment(DateTime.Now, DateTime.Today);
        changePublishedAt.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Время публикации уже опубликованного объявления не может быть изменено");
    }

    [Fact]
    public void SetPublishedMoment_ChangeValueFromNull_Passes()
    {
        var announcement = CreateValidAnnouncement();

        var twoHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(2));
        var changePublishedAt = () => announcement.SetPublishedMoment(DateTime.Now, twoHoursAgo);
        changePublishedAt.Should().NotThrow();
    }

    [Fact]
    public void SetPublishedMoment_BeforeHidingMoment_Passes()
    {
        var announcement = CreateValidAnnouncement();
        var fiveHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(5));
        announcement.SetHiddenMoment(DateTime.Now, fiveHoursAgo);

        var sixHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(6));
        var changePublishedAt = () => announcement.SetPublishedMoment(DateTime.Now, sixHoursAgo);
        changePublishedAt.Should().NotThrow();
    }

    [Fact]
    public void SetPublishedMoment_EqualToHidingMoment_Throws()
    {
        var announcement = CreateValidAnnouncement();
        var fiveHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(5));
        announcement.SetHiddenMoment(DateTime.Now, fiveHoursAgo);

        var changePublishedAt = () => announcement.SetPublishedMoment(DateTime.Now, fiveHoursAgo);
        changePublishedAt.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "Момент публикации уже опубликованного объявления не может наступить позже момента его сокрытия");
    }

    [Fact]
    public void SetPublishedMoment_AfterHidingMoment_Throws()
    {
        var announcement = CreateValidAnnouncement();
        var fiveHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(5));
        announcement.SetHiddenMoment(DateTime.Now, fiveHoursAgo);

        var twoHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(2));
        var changePublishedAt = () => announcement.SetPublishedMoment(DateTime.Now, twoHoursAgo);
        changePublishedAt.Should().Throw<InvalidOperationException>();
    }



    [Fact]
    public void SetHiddenAt_CurrentMoment_Passes()
    {
        var announcement = CreateValidAnnouncement();
        var now = DateTime.Now;
        
        var setHiddenAt = () => announcement.SetPublishedMoment(now: now, publishedAt: now);
        setHiddenAt.Should().NotThrow();
    }

    [Fact]
    public void SetHiddenAt_MomentFromFuture_Throws()
    {
        var announcement = CreateValidAnnouncement();

        var setHiddenAt = () => announcement.SetHiddenMoment(DateTime.Now, DateTime.Now.AddHours(1));
        setHiddenAt.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void SetHiddenAt_MomentFromPast_Passes()
    {
        var announcement = CreateValidAnnouncement();

        var setHiddenAt = () =>
            announcement.SetHiddenMoment(DateTime.Now, DateTime.Now.Subtract(TimeSpan.FromHours(1)));
        setHiddenAt.Should().NotThrow();
    }

    [Fact]
    public void SetHiddenAt_AfterPublishingMoment_Passes()
    {
        var announcement = CreateValidAnnouncement();
        var fiveHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(5));
        announcement.SetPublishedMoment(DateTime.Now, fiveHoursAgo);

        var twoHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(2));
        var changePublishedAt = () => announcement.SetHiddenMoment(DateTime.Now, twoHoursAgo);
        changePublishedAt.Should().NotThrow();
    }

    [Fact]
    public void SetHiddenAt_EqualToPublishingMoment_Throws()
    {
        var announcement = CreateValidAnnouncement();
        var fiveHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(5));
        announcement.SetPublishedMoment(DateTime.Now, fiveHoursAgo);

        var changePublishedAt = () => announcement.SetHiddenMoment(DateTime.Now, fiveHoursAgo);
        changePublishedAt.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void SetHiddenAt_BeforePublishingMoment_Throws()
    {
        var announcement = CreateValidAnnouncement();
        var fiveHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(5));
        announcement.SetPublishedMoment(DateTime.Now, fiveHoursAgo);

        var sixHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(6));
        var changePublishedAt = () => announcement.SetHiddenMoment(DateTime.Now, sixHoursAgo);
        changePublishedAt.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void SetHiddenAt_NotAfterBeforeAutoPublishingMoment_Throws()
    {
        var announcement = CreateValidAnnouncement();
        var fiveHoursLater = DateTime.Now.AddHours(5);
        announcement.SetDelayedPublishingMoment(DateTime.Now, fiveHoursLater);

        var threeHoursLater = DateTime.Now.AddHours(3);
        var changePublishedAt = () => announcement.SetHiddenMoment(DateTime.Now, threeHoursLater);
        changePublishedAt.Should().Throw<InvalidOperationException>();
    }



    [Fact]
    public void SetAutoPublishingAt_Null_Passes()
    {
        var announcement = CreateValidAnnouncement();

        var setAutoPublishingMoment = () => announcement.SetDelayedPublishingMoment(DateTime.Now, null);
        setAutoPublishingMoment.Should().NotThrow();
    }

    [Fact]
    public void SetAutoPublishingAt_ForAlreadyPublishedAnnouncement_Throws()
    {
        var announcement = CreateValidAnnouncement();
        var now = DateTime.Now;
        announcement.SetPublishedMoment(now: now, publishedAt: now);

        var threeHoursLater = DateTime.Now.AddHours(3);
        var setAutoPublishingMoment = () => announcement.SetDelayedPublishingMoment(DateTime.Now, threeHoursLater);
        setAutoPublishingMoment.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Нельзя задать момент автоматической публикации уже опубликованному объявлению");
    }

    [Fact]
    public void SetAutoPublishingAt_BeforeCurrentMoment_Throws()
    {
        var announcement = CreateValidAnnouncement();

        var threeHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(3));
        var setAutoPublishingMoment = () => announcement.SetDelayedPublishingMoment(DateTime.Now, threeHoursAgo);
        setAutoPublishingMoment.Should().Throw<InvalidOperationException>();
    }



    [Fact]
    public void SetAutoHidingAt_Null_Passes()
    {
        var announcement = CreateValidAnnouncement();

        var setAutoHidingMoment = () => announcement.SetDelayedHidingMoment(DateTime.Now, null);
        setAutoHidingMoment.Should().NotThrow();
    }

    [Fact]
    public void SetAutoHidingAt_ForAlreadyHiddenAnnouncement_Throws()
    {
        var announcement = CreateValidAnnouncement();
        var now = DateTime.Now;
        announcement.SetHiddenMoment(now: now, hiddenAt: now);

        var threeHoursLater = DateTime.Now.AddHours(3);
        var setAutoHidingMoment = () => announcement.SetDelayedHidingMoment(DateTime.Now, threeHoursLater);
        setAutoHidingMoment.Should().Throw<InvalidOperationException>();
    }



    private static Announcement CreateValidAnnouncement() =>
        new(
            id: Guid.Empty, 
            content: "content",
            author: new User("name", "second name"),
            audience: [new User("name", "second name")],
            publishedAt: null, 
            hiddenAt: null, delayedPublishingAt: null, delayedHidingAt: null,
            attachments: []);
}