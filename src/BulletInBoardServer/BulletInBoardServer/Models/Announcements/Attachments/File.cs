namespace BulletInBoardServer.Models.Announcements.Attachments;

public class File : IAttachment
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public string Hash  { get; set; }
    public int LinksCount { get; set; }
}