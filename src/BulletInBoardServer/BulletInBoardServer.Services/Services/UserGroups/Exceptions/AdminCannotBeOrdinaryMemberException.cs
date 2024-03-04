namespace BulletInBoardServer.Services.Services.UserGroups.Exceptions;

public class AdminCannotBeOrdinaryMemberException(Exception? innerException = null)
    : InvalidOperationException("Администратор группы пользователей не может быть ее рядовым участником",
        innerException);