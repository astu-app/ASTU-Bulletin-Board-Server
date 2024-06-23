using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.UserGroups;
using BulletInBoardServer.Services.Services.UserGroups.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BulletInBoardServer.Services.Services.MemberRights;

public class MemberRightsLoader(ApplicationDbContext dbContext)
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="right"></param>
    /// <param name="requesterId"></param>
    /// <param name="userGroupId"></param>
    /// <param name="availableToAdmin"></param>
    /// <returns></returns>
    /// <exception cref="UserGroupDoesNotExistException">Группа пользователей не существует</exception>
    public bool CanDo(Func<SingleMemberRights, bool> right, Guid requesterId, Guid userGroupId,
        bool availableToAdmin = true)
    {
        /*
         * 1. Загрузить группу пользователей
         * 2. проверить, является ли текущий пользователь админом
         * 3. если является, проверить, доступна ли операция для админа
         * 4. если доступна, вернуть true
         * 5. если не доступна, загрузить права пользователя в этой группе
         * 6. проверить право
         * 7. если право присутствует, вернуть true
         * 8. если право отсутствует, вернуть false
         */

        var userGroup = LoadUserGroupSummary(userGroupId);
        if (userGroup is null)
            throw new UserGroupDoesNotExistException();
        
        if (userGroup.AdminId == requesterId && availableToAdmin)
            return true;
        
        var memberRights = LoadMemberRights(requesterId, userGroupId);
        return memberRights is not null && right(memberRights);
    }



    private UserGroup? LoadUserGroupSummary(Guid id) =>
        dbContext.UserGroups.SingleOrDefault(ug => ug.Id == id);
    
    private SingleMemberRights? LoadMemberRights(Guid memberId, Guid userGroupId) => 
        dbContext.MemberRights
            .Where(mr => mr.UserId == memberId && mr.UserGroupId == userGroupId)
            .Include(mr => mr.UserGroup)
            .SingleOrDefault();
}