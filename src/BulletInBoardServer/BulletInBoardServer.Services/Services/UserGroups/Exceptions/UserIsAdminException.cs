namespace BulletInBoardServer.Services.Services.UserGroups.Exceptions;

public class UserIsAdminException(Exception? innerException = null)
    : InvalidOperationException("Пользователь является администратором группы пользователей", innerException);