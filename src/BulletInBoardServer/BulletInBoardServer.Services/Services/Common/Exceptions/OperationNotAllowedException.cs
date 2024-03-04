namespace BulletInBoardServer.Services.Services.Common.Exceptions;

public class OperationNotAllowedException(string? message) : InvalidOperationException(message);