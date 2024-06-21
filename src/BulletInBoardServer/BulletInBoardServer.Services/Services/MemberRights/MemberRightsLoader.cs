using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.UserGroups;

namespace BulletInBoardServer.Services.Services.MemberRights;

public class MemberRightsLoader(ApplicationDbContext dbContext)
{
    public SingleMemberRights? Load(Guid memberId, Guid userGroupId) => 
        dbContext.MemberRights.SingleOrDefault(mr => mr.UserId == memberId && mr.UserGroupId == userGroupId);
}