namespace BulletInBoardServer.Services.Services.Attachments.Exceptions;

public class AttachmentDoesNotExistException(Exception? internalException = null)
    : InvalidOperationException("Вложения не найдены", internalException);