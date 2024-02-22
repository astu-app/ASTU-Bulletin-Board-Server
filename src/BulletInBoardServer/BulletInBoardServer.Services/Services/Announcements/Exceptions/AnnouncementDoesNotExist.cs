namespace BulletInBoardServer.Services.Services.Announcements.Exceptions;

public class AnnouncementDoesNotExist(string message) : InvalidOperationException(message);