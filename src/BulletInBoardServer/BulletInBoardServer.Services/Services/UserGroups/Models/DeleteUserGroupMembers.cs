namespace BulletInBoardServer.Services.Services.UserGroups.Models;

public class DeleteUserGroupMembers(Guid userGroupId, IEnumerable<Guid> memberIds)
{
    public Guid UserGroupId { get; } = userGroupId;
    public IEnumerable<Guid> MemberIds { get; } = memberIds;
}