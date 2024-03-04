namespace BulletInBoardServer.Services.Services.UserGroups.Exceptions;

public class UserGroupDoesNotExistException(Exception? internalException = null)
    : InvalidOperationException("Группа пользователей не существует", internalException);