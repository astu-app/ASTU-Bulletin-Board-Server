using BulletInBoardServer.Domain.Models.UserGroups;

namespace BulletInBoardServer.Services.Services.UserGroups.Models;

public class CreateUserGroup
{
    public required string Name { get; set; }

    public Guid? AdminId { get; set; }
    public ICollection<SingleMemberRights> Members { get; set; } = [];

    public ICollection<Guid> ParentGroupIds { get; set; } = [];
    public ICollection<Guid> ChildGroupIds { get; set; } = [];
}