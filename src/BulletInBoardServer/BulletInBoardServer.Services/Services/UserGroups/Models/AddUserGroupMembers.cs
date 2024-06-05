using BulletInBoardServer.Domain.Models.UserGroups;

namespace BulletInBoardServer.Services.Services.UserGroups.Models;

// ReSharper disable once InconsistentNaming
public class AddUserGroupMembers(Guid userGroupId, IEnumerable<SingleMemberRights> members)
{
    public Guid UserGroupId { get; } = userGroupId;
    public IEnumerable<SingleMemberRights> Members { get; } = members;
}