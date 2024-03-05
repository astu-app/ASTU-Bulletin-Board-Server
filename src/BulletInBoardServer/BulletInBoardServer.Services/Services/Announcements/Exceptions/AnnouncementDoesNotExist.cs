namespace BulletInBoardServer.Services.Services.Announcements.Exceptions;

public class AnnouncementDoesNotExist(string message, Exception? innerException = null) 
    : InvalidOperationException(message, innerException);