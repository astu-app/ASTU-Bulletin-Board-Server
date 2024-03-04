namespace BulletInBoardServer.Services.Services.UserGroups.Models;

public class ChangeUserGroupMembers(Guid userGroupId, IEnumerable<Guid> userIds)
{
    public Guid UserGroupId { get; } = userGroupId;
    public IEnumerable<Guid> UserIds { get; } = userIds;
}