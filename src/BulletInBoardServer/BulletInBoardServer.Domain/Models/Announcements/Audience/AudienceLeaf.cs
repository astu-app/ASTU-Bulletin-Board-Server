namespace BulletInBoardServer.Domain.Models.Announcements.Audience;

public class AudienceLeaf<T>(T content) : IAudienceNode
{
    public T Content { get; set; } = content;
}