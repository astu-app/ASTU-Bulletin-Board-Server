using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.JoinEntities;
using BulletInBoardServer.Domain.Models.UserGroups;
using BulletInBoardServer.Domain.Models.UserGroups.Exceptions;
using BulletInBoardServer.Services.Services.UserGroups.Exceptions;
using BulletInBoardServer.Services.Services.UserGroups.Models;
using BulletInBoardServer.Services.Services.Users.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

// ReSharper disable InconsistentNaming

namespace BulletInBoardServer.Services.Services.UserGroups.ServiceCore;

/// <summary>
/// Класс-создатель групп пользователей
/// </summary>
/// <param name="create">Объект с необходимой для создания группы информацией</param>
/// <param name="dbContext">Контекст базы данных</param>
public class UserGroupCreator(CreateUserGroup create, ApplicationDbContext dbContext)
{
    private UserGroup _usergroup = null!; // инициализируется при вызове единственного публичного метода Create



    /// <summary>
    /// Метод создает группу пользователей
    /// </summary>
    /// <returns>Идентификатор созданной группы пользователей</returns>
    /// <exception cref="UserGroupNameEmptyException">Название группы пользователей пустое</exception>">
    /// <exception cref="AdminCannotBeOrdinaryMemberException">Администратор группы пользователей не может быть ее рядовым участником</exception>">
    /// <exception cref="UserGroupDoesNotExistException">Группа пользователей, указанная как дочерняя или родительская, не существует</exception>
    /// <exception cref="UserDoesNotExistException">Пользователь, указанный как участник или администратор группы, не существует</exception>
    /// <exception cref="UserGroupCreatesCycleException">Создаваемая группа пользователей образует цикл в графе групп пользователей</exception>
    public Guid Create()
    {
        var transaction = dbContext.Database.BeginTransaction();
        
        try
        {
            _usergroup = new UserGroup(Guid.NewGuid(), create.Name, create.AdminId);
            dbContext.UserGroups.Add(_usergroup);

            MembersNotContainAdminIdOrThrow();
            
            AddMembers();
            AddChildUserGroups();
            AddParentUserGroups();

            dbContext.SaveChanges();
            transaction.Commit();

            return _usergroup.Id; 
        }
        catch (DbUpdateException err)
        {
            transaction.Rollback();

            if (err.InnerException is not PostgresException inner)
                throw;
            
            switch (inner)
            {
                case { SqlState: "23503", ConstraintName: "child_usergroups_usergroup_id_fkey" }:
                    throw new UserGroupDoesNotExistException(err);
                case { SqlState: "23503", ConstraintName: "child_usergroup_child_usergroup_id_fkey" }:
                    throw new UserGroupDoesNotExistException(err);
                case { SqlState: "23503", ConstraintName: "member_rights_user_id_fkey" }:
                    throw new UserDoesNotExistException(err);
                case { SqlState: "23503", ConstraintName: "usergroups_admin_id_fkey" }:
                    throw new UserDoesNotExistException(err);
                case { SqlState: "P0001" }:
                    throw new UserGroupCreatesCycleException(err);
                default:
                    throw;
            }
        }
    }



    private void MembersNotContainAdminIdOrThrow()
    {
        var contain = create.Members
            .Select(m => m.UserId)
            .Any(id => id == create.AdminId);
        if (contain)
            throw new AdminCannotBeOrdinaryMemberException();
    }
    
    private void AddMembers()
    {
        foreach (var member in create.Members) 
            dbContext.MemberRights.Add(new SingleMemberRights(
                userId: member.UserId, 
                userGroupId: _usergroup.Id,
                canViewAnnouncements: member.CanViewAnnouncements,
                canCreateAnnouncements: member.CanCreateAnnouncements,
                canCreateSurveys: member.CanCreateSurveys,
                canViewUserGroupDetails: member.CanViewUserGroupDetails,
                canCreateUserGroups: member.CanCreateUserGroups,
                canEditUserGroups: member.CanEditUserGroups,
                canEditMembers: member.CanEditMembers,
                canEditMemberRights: member.CanEditMemberRights,
                canEditUserGroupAdmin: member.CanEditUserGroupAdmin,
                canDeleteUserGroup: member.CanDeleteUserGroup
            ));
    }

    private void AddChildUserGroups()
    {
        foreach (var childGroupId in create.ChildGroupIds) 
            dbContext.ChildUseGroups.Add(new ChildUseGroup(_usergroup.Id, childGroupId));
    }

    private void AddParentUserGroups()
    {
        foreach (var parentGroupId in create.ParentGroupIds) 
            dbContext.ChildUseGroups.Add(new ChildUseGroup(parentGroupId, _usergroup.Id));
    }
}