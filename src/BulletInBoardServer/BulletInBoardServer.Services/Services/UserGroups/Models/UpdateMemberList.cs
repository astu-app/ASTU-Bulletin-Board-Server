using BulletInBoardServer.Domain.Models.UserGroups;

namespace BulletInBoardServer.Services.Services.UserGroups.Models;

public class UpdateMemberList(ICollection<Guid>? idsToRemove, ICollection<SingleMemberRights>? newMembers)
{
    public ICollection<Guid>? IdsToRemove { get; } = idsToRemove;
    public ICollection<SingleMemberRights>? NewMembers { get; } = newMembers;
}