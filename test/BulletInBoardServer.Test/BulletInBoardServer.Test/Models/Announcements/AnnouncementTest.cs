using BulletInBoardServer.Models.Announcements;
using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Test.Models.Announcements;

public class AnnouncementTest
{
    [Fact]
    public void SetContent_NullString_Throws()
    {
        var announcement = CreateValidAnnouncement();

        var setContent = () => announcement.Content = null!;
        setContent.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void SetContent_EmptyString_Throws()
    {
        var announcement = CreateValidAnnouncement();

        var setContent = () => announcement.Content = string.Empty;
        setContent.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void SetContent_WhiteSpaceString_Throws()
    {
        var announcement = CreateValidAnnouncement();

        var whitespaces = new[] { " ", "\t", "\n", "\r", "\v", "\f" };
        foreach (var whitespace in whitespaces)
        {
            var setContent = () => announcement.Content = whitespace;
            setContent.Should().Throw<ArgumentException>();
        }
    }



    [Fact]
    public void SetPublishedAt_CurrentMoment_Passes()
    {
        var announcement = CreateValidAnnouncement();

        var setPublishedAt = () => announcement.PublishedAt = DateTime.Now;
        setPublishedAt.Should().NotThrow();
    }

    [Fact]
    public void SetPublishedAt_MomentFromFuture_Throws()
    {
        var announcement = CreateValidAnnouncement();

        var setPublishedAt = () => announcement.PublishedAt = DateTime.Now.AddHours(1);
        setPublishedAt.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Момент публикации уже опубликованного объявления не может наступить в будущем");
    }

    [Fact]
    public void SetPublishedAt_MomentFromPast_Passes()
    {
        var announcement = CreateValidAnnouncement();

        var setPublishedAt = () =>
            announcement.PublishedAt = DateTime.Now.Subtract(TimeSpan.FromHours(1));
        setPublishedAt.Should().NotThrow();
    }

    [Fact]
    public void SetPublishedAt_ChangeValueFromNotNull_Throws()
    {
        var announcement = CreateValidAnnouncement();
        announcement.PublishedAt = DateTime.MinValue;

        var changePublishedAt = () => announcement.PublishedAt = DateTime.Today;
        changePublishedAt.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Время публикации уже опубликованного объявления не может быть изменено");
    }

    [Fact]
    public void SetPublishedAt_ChangeValueFromNull_Passes()
    {
        var announcement = CreateValidAnnouncement();

        var twoHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(2));
        var changePublishedAt = () => announcement.PublishedAt = twoHoursAgo;
        changePublishedAt.Should().NotThrow();
    }

    [Fact]
    public void SetPublishedAt_BeforeHidingMoment_Passes()
    {
        var announcement = CreateValidAnnouncement();
        var fiveHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(5));
        announcement.HiddenAt = fiveHoursAgo;

        var sixHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(6));
        var changePublishedAt = () => announcement.PublishedAt = sixHoursAgo;
        changePublishedAt.Should().NotThrow();
    }

    [Fact]
    public void SetPublishedAt_EqualToHidingMoment_Throws()
    {
        var announcement = CreateValidAnnouncement();
        var fiveHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(5));
        announcement.HiddenAt = fiveHoursAgo;

        var changePublishedAt = () => announcement.PublishedAt = fiveHoursAgo;
        changePublishedAt.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Момент публикации уже опубликованного объявления не может наступить позже момента его сокрытия");
    }

    [Fact]
    public void SetPublishedAt_AfterHidingMoment_Throws()
    {
        var announcement = CreateValidAnnouncement();
        var fiveHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(5));
        announcement.HiddenAt = fiveHoursAgo;

        var twoHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(2));
        var changePublishedAt = () => announcement.PublishedAt = twoHoursAgo;
        changePublishedAt.Should().Throw<InvalidOperationException>();
    }



    [Fact]
    public void SetHiddenAt_CurrentMoment_Passes()
    {
        var announcement = CreateValidAnnouncement();

        var setHiddenAt = () => announcement.PublishedAt = DateTime.Now;
        setHiddenAt.Should().NotThrow();
    }

    [Fact]
    public void SetHiddenAt_MomentFromFuture_Throws()
    {
        var announcement = CreateValidAnnouncement();

        var setHiddenAt = () => announcement.HiddenAt = DateTime.Now.AddHours(1);
        setHiddenAt.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void SetHiddenAt_MomentFromPast_Passes()
    {
        var announcement = CreateValidAnnouncement();

        var setHiddenAt = () =>
            announcement.HiddenAt = DateTime.Now.Subtract(TimeSpan.FromHours(1));
        setHiddenAt.Should().NotThrow();
    }

    [Fact]
    public void SetHiddenAt_AfterPublishingMoment_Passes()
    {
        var announcement = CreateValidAnnouncement();
        var fiveHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(5));
        announcement.PublishedAt = fiveHoursAgo;

        var twoHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(2));
        var changePublishedAt = () => announcement.HiddenAt = twoHoursAgo;
        changePublishedAt.Should().NotThrow();
    }

    [Fact]
    public void SetHiddenAt_EqualToPublishingMoment_Throws()
    {
        var announcement = CreateValidAnnouncement();
        var fiveHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(5));
        announcement.PublishedAt = fiveHoursAgo;

        var changePublishedAt = () => announcement.HiddenAt = fiveHoursAgo;
        changePublishedAt.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void SetHiddenAt_BeforePublishingMoment_Throws()
    {
        var announcement = CreateValidAnnouncement();
        var fiveHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(5));
        announcement.PublishedAt = fiveHoursAgo;

        var sixHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(6));
        var changePublishedAt = () => announcement.HiddenAt = sixHoursAgo;
        changePublishedAt.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void SetHiddenAt_NotAfterBeforeAutoPublishingMoment_Throws()
    {
        var announcement = CreateValidAnnouncement();
        var fiveHoursLater = DateTime.Now.AddHours(5);
        announcement.DelayedPublishingAt = fiveHoursLater;

        var threeHoursLater = DateTime.Now.AddHours(3);
        var changePublishedAt = () => announcement.HiddenAt = threeHoursLater;
        changePublishedAt.Should().Throw<InvalidOperationException>();
    }



    [Fact]
    public void SetAutoPublishingAt_Null_Passes()
    {
        var announcement = CreateValidAnnouncement();

        var setAutoPublishingMoment = () => announcement.DelayedPublishingAt = null;
        setAutoPublishingMoment.Should().NotThrow();
    }

    [Fact]
    public void SetAutoPublishingAt_ForAlreadyPublishedAnnouncement_Throws()
    {
        var announcement = CreateValidAnnouncement();
        announcement.PublishedAt = DateTime.Now;

        var threeHoursLater = DateTime.Now.AddHours(3);
        var setAutoPublishingMoment = () => announcement.DelayedPublishingAt = threeHoursLater;
        setAutoPublishingMoment.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Нельзя задать момент автоматической публикации уже опубликованному объявлению");
    }

    [Fact]
    public void SetAutoPublishingAt_BeforeCurrentMoment_Throws()
    {
        var announcement = CreateValidAnnouncement();

        var threeHoursAgo = DateTime.Now.Subtract(TimeSpan.FromHours(3));
        var setAutoPublishingMoment = () => announcement.DelayedPublishingAt = threeHoursAgo;
        setAutoPublishingMoment.Should().Throw<InvalidOperationException>();
    }



    [Fact]
    public void SetAutoHidingAt_Null_Passes()
    {
        var announcement = CreateValidAnnouncement();

        var setAutoHidingMoment = () => announcement.DelayedHidingAt = null;
        setAutoHidingMoment.Should().NotThrow();
    }

    [Fact]
    public void SetAutoHidingAt_ForAlreadyHiddenAnnouncement_Throws()
    {
        var announcement = CreateValidAnnouncement();
        announcement.HiddenAt = DateTime.Now;

        var threeHoursLater = DateTime.Now.AddHours(3);
        var setAutoHidingMoment = () => announcement.DelayedHidingAt = threeHoursLater;
        setAutoHidingMoment.Should().Throw<InvalidOperationException>();
    }



    private static Announcement CreateValidAnnouncement() =>
        new(Guid.Empty, "content",
            new User("name", "second name"),
            [],
            [new User("name", "second name")],
            null, null, null, null, 
            []);
}