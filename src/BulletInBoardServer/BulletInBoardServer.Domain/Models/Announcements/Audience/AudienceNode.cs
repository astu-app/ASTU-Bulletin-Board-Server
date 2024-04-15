namespace BulletInBoardServer.Domain.Models.Announcements.Audience;

public class AudienceNode<T>(T content, IEnumerable<T> children) : IAudienceNode
{
    public T Content { get; set; } = content;
    public IEnumerable<T> Children { get; set; } = children;
}