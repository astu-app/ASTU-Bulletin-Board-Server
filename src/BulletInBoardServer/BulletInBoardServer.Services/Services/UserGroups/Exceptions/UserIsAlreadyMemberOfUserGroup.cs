namespace BulletInBoardServer.Services.Services.UserGroups.Exceptions;

public class UserIsAlreadyMemberOfUserGroup(Exception? innerException = null)
    : InvalidOperationException("Пользователь уже состоит в группе пользователей", innerException);