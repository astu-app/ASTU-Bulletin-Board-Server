namespace BulletInBoardServer.Models.Announcements.Attachments;

public static class AttachmentListExtensions
{
    public static ReadOnlyAttachmentList AsReadOnly(this AttachmentList attachments) => 
        new (attachments);
}