namespace BulletInBoardServer.Domain.Models.Announcements.Exceptions;

public class AutoPublishAnAlreadyPublishedAnnouncementException : InvalidOperationException
{
    public AutoPublishAnAlreadyPublishedAnnouncementException()
        : base("Нельзя задать момент автоматической публикации уже опубликованному объявлению")
    {
    }
}