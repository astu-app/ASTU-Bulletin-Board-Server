namespace BulletInBoardServer.Models.Announcements.Attachments;

public static class AttachmentExtensions
{
    public static ReadOnlyAttachments AsReadOnly(this Attachments attachments) =>
        new (attachments);
}