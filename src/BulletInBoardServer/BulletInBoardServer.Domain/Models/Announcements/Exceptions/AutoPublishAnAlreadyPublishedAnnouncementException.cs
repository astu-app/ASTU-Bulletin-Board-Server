namespace BulletInBoardServer.Domain.Models.Announcements.Exceptions;

public class AutoPublishAnAlreadyPublishedAnnouncementException()
    : InvalidOperationException("Нельзя задать момент автоматической публикации уже опубликованному объявлению");