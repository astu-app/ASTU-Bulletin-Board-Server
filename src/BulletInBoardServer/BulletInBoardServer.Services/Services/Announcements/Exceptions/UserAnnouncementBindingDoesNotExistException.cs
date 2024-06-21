namespace BulletInBoardServer.Services.Services.Announcements.Exceptions;

public class UserAnnouncementBindingDoesNotExistException(string message, Exception? innerException = null) : InvalidOperationException(message, innerException);