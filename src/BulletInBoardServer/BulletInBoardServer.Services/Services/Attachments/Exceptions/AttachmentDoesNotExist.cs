namespace BulletInBoardServer.Services.Services.Attachments.Exceptions;

public class AttachmentDoesNotExist : InvalidOperationException
{
    public AttachmentDoesNotExist(Exception? internalException)
        : base("Вложения не найдены", internalException)
    {
    }
}