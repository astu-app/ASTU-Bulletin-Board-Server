namespace BulletInBoardServer.Services.Services.Audience.Exceptions;

public class PieceOfAudienceDoesNotExist : InvalidOperationException
{
    public PieceOfAudienceDoesNotExist(Exception? internalException)
        : base("Часть аудитории не найдена", internalException)
    {
    }
}