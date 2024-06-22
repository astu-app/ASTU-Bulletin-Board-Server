namespace BulletInBoardServer.Services.Services.Users.Exceptions;

public class UserAlreadyExistsException(Exception? innerException = null)
    : InvalidOperationException("Пользователь уже существует", innerException);