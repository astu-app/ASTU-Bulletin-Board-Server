namespace BulletInBoardServer.Services.Services.UserGroups.Exceptions;

public class UserGroupCreatesCycleException(Exception? internalException = null)
    : InvalidOperationException("Группа пользователей создает цикл в графе групп пользователей", internalException);