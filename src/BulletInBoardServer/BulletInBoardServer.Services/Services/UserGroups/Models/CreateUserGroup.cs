namespace BulletInBoardServer.Services.Services.UserGroups.Models;

public class CreateUserGroup
{
    public required string Name { get; set; }

    public Guid? AdminId { get; set; } = null;
    public ICollection<Guid> MemberIds { get; set; } = [];

    public ICollection<Guid> ParentGroupIds { get; set; } = [];
    public ICollection<Guid> ChildGroupIds { get; set; } = [];
}