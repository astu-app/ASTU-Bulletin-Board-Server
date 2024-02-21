namespace BulletInBoardServer.Services.Services.Exceptions;

public class OperationNotAllowedException : InvalidOperationException
{
    public OperationNotAllowedException(string? message) : base(message)
    {
    }

    public OperationNotAllowedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}