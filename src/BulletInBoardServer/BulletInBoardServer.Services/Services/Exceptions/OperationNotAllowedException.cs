namespace BulletInBoardServer.Services.Services.Exceptions;

public class OperationNotAllowedException(string? message) : InvalidOperationException(message);