namespace BulletInBoardServer.Services.Services.Audience.Exceptions;

public class PieceOfAudienceDoesNotExistException(Exception? internalException = null)
    : InvalidOperationException("Часть аудитории не найдена", internalException);