namespace BulletInBoardServer.Models.Announcements.Attachments;

public class File(Guid id, string name, string hash, int linksCount)
    : IAttachment
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;

    public string Hash  { get; } = hash;
    public int LinksCount { get; } = linksCount;

    public void AddLink() =>
        ++linksCount;

    public void RemoveLink() =>
        --linksCount;
}