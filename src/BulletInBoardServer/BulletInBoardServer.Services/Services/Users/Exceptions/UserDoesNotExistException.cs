namespace BulletInBoardServer.Services.Services.Users.Exceptions;

public class UserDoesNotExistException(Exception? innerException = null)
    : InvalidOperationException("Пользователь не существует", innerException);