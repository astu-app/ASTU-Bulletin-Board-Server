using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.UserGroups;
using BulletInBoardServer.Domain.Models.UserGroups.Exceptions;
using BulletInBoardServer.Domain.Models.Users;
using BulletInBoardServer.Services.Services.UserGroups.Exceptions;
using BulletInBoardServer.Services.Services.UserGroups.Models;
using BulletInBoardServer.Services.Services.UserGroups.ServiceCore;
using BulletInBoardServer.Services.Services.Users.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BulletInBoardServer.Services.Services.UserGroups;

/// <summary>
/// Сервис для управления группами пользователей
/// </summary>
/// <param name="dbContext"></param>
public class UserGroupService(ApplicationDbContext dbContext)
{
    /// <summary>
    /// Получение данных для создания группы пользователей
    /// </summary>
    /// <param name="requesterId">Идентификатор пользователя, запросившего операцию</param>
    /// <returns>Данные для создания группы пользователей</returns>
    public CreateUserGroupContent GetContentForUserGroupCreation(Guid requesterId)
    {
        var users = dbContext.Users.ToUserList();
        var userGroups = GetUsergroupHierarchy(requesterId)
            .FlattenUserGroupHierarchy()
            .ToUserGroupList();
        
        return new CreateUserGroupContent(users, userGroups);
    }
    
    /// <summary>
    /// Создать группу пользователей
    /// </summary>
    /// <param name="create">Объект с необходимой для создания группы информацией</param>
    /// <returns>Идентификатор созданной группы пользователей</returns>
    /// <exception cref="UserGroupNameEmptyException">Название группы пользователей пустое</exception>">
    /// <exception cref="AdminCannotBeOrdinaryMemberException">Администратор группы пользователей не может быть ее рядовым участником</exception>">
    /// <exception cref="UserGroupDoesNotExistException">Группа пользователей, указанная как дочерняя или родительская, не существует</exception>
    /// <exception cref="UserDoesNotExistException">Пользователь, указанный как участник или администратор группы, не существует</exception>
    /// <exception cref="UserGroupCreatesCycleException">Создаваемая группа пользователей образует цикл в графе групп пользователей</exception>
    public Guid Create(CreateUserGroup create)
    {
        var creator = new UserGroupCreator(create, dbContext);
        var usergroupId = creator.Create();

        return usergroupId;
    }

    /// <summary>
    /// Получить список всех присутствующих в системе групп пользователей
    /// </summary>
    /// <returns>Список всех групп пользователей</returns>
    public UserGroupList GetAll() =>
        dbContext.UserGroups.ToUserGroupList();

    /// <summary>
    /// Получить список групп пользователей, управляемых пользователем
    /// </summary>
    /// <param name="userId">Id администратора</param>
    /// <returns>Список управляемых администратором групп пользователей</returns>
    /// <remarks>
    /// Пользователь управляет группу, если он является ее администратором или имеет право управления группой
    /// </remarks>
    public UserGroupList GetOwnedList(Guid userId) =>
        dbContext.UserGroups
            .Where(ug => ug.AdminId == userId)
            .Union(dbContext.MemberRights
                .Where(mr => mr.UserId == userId && mr.CanRuleUserGroupHierarchy)
                .Select(mr => mr.UserGroup))
            .Distinct()
            .Include(ug => ug.Admin)
            .ToUserGroupList();

    /// <summary>
    /// Получить иерархии групп пользователей, управляемых указанным пользователем
    /// </summary>
    /// <param name="managerId">Пользователь, управляющий иерархией групп</param>
    /// <returns>Набор корневых групп пользователей, управляемых пользователем</returns>
    public UserGroupList GetUsergroupHierarchy(Guid managerId)
    {
        // todo сделать удаление из ownedUsergroups групп, которые находятся в подчинении других групп, находящихся в этом же списке 
        var ownedUsergroups = dbContext.UserGroups
            .Where(ug => ug.AdminId == managerId)
            .Union(
                dbContext.MemberRights
                    .Where(mr => mr.UserId == managerId && mr.CanRuleUserGroupHierarchy)
                    .Include(mr => mr.UserGroup)
                    .Select(mr => mr.UserGroup)
            )
            .Include(ug => ug.Admin)
            .ToUserGroupList();

        foreach (var usergroup in ownedUsergroups) 
            LoadUserGroupRecursively(usergroup);

        return ownedUsergroups;
    }

    /// <summary>
    /// Получить детальную информацию о группе пользователей
    /// </summary>
    /// <param name="usergroupId">Id группы пользователей</param>
    /// <returns>Детальная информация о группе пользователей</returns>
    /// <exception cref="UserGroupDoesNotExistException">Группа пользователей с указанным Id не существует</exception>
    public UserGroupDetails GetDetails(Guid usergroupId)
    {
        var usergroup = dbContext.UserGroups
            .Where(ug => ug.Id == usergroupId)
            .Include(ug => ug.Admin)
            .Include(ug => ug.MemberRights)
            .ThenInclude(m => m.User)
            .Include(ug => ug.ChildrenGroups)
            .ThenInclude(ug => ug.Admin)
            .FirstOrDefault();
        if (usergroup is null)
            throw new UserGroupDoesNotExistException();

        var parents = dbContext.UserGroups
            .Include(ug => ug.Admin)
            .Join(dbContext.ChildUseGroups,
                ug => ug.Id,
                cug => cug.UserGroupId,
                (ug, cug) => new { UserGroup = ug, ChildUserGroup = cug })
            .Where(join => join.ChildUserGroup.ChildUserGroupId == usergroupId)
            .Select(join => join.UserGroup)
            .Distinct()
            .OrderBy(ug => ug.Name)
            .ToUserGroupList();

        var details = new UserGroupDetails
        (
            id: usergroup.Id,
            name: usergroup.Name,
            admin: usergroup.Admin,
            memberRights: usergroup.MemberRights,
            childrenGroups: usergroup.ChildrenGroups,
            parentGroups: parents
        );
        return details;
    }

    /// <summary>
    /// Получить данные для редактирования группы пользователей
    /// </summary>
    /// <param name="userGroupId">Идентификатор группы пользователей, для которой запрашиваются данные</param>
    /// <param name="requesterId">Пользователь, запросивший операцию</param>
    public UpdateUserGroupContent GetContentForUserGroupUpdating(Guid userGroupId, Guid requesterId)
    {
        var details = GetDetails(userGroupId);

        var potentialMembers = dbContext.Users

            .Except(dbContext.MemberRights
                // Ищем таких пользователей, которые являются участниками группы пользователей
                .Where(mr => mr.UserGroupId == userGroupId)
                .Select(r => r.User))
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.SecondName)
            .ThenBy(u => u.Patronymic)
            .ToUserList();

        // var potentialRelatives = GetOwnedList(requesterId); // remove

        return new UpdateUserGroupContent(details, potentialMembers/*, potentialRelatives*/); // remove
    }

    /// <summary>
    /// Редактировать группу пользователей
    /// </summary>
    /// <param name="edit">Объект с необходимой для редактирования информацией</param>
    /// <exception cref="UserGroupDoesNotExistException">Группа пользователей не существует</exception>
    /// <exception cref="UserGroupNameEmptyException">Название группы пользователей пустое</exception>">
    public void Edit(EditUserGroup edit)
    {
        var redactor = new UserGroupRedactor(dbContext);
        redactor.Edit(edit);
    }

    /// <summary>
    /// Удалить группу пользователей
    /// </summary>
    /// <param name="id">Id удаляемой группы пользователей</param>
    /// <exception cref="UserGroupDoesNotExistException">Группа пользователей не существует</exception>
    public void Delete(Guid id)
    {
        var rowsDeleted = dbContext.UserGroups
            .Where(ug => ug.Id == id)
            .ExecuteDelete();

        if (rowsDeleted == 0)
            throw new UserGroupDoesNotExistException();
    }

    /// <summary>
    /// Добавить участников в группу пользователей
    /// </summary>
    /// <param name="add">Объект с необходимой для добавления информацией</param>
    /// <exception cref="UserIsAdminException">В группу пользователей нельзя добавить участника, являющегося администратором этой группы</exception>
    /// <exception cref="UserIsAlreadyMemberOfUserGroup">Участник уже состоит в группе</exception>
    /// <exception cref="UserDoesNotExistException">Пользователь не существует в БД</exception>
    /// <exception cref="UserGroupDoesNotExistException">Группа пользователей не существует в БД</exception>
    public void AddMembers(AddUserGroupMembers add)
    {
        var redactor = new UserGroupRedactor(dbContext);
        redactor.AddMembers(add);
    }

    /// <summary>
    /// Удалить участников из группы пользователей
    /// </summary>
    /// <param name="delete">Объект с необходимой для удаления информацией</param>
    /// <exception cref="UserIsAdminException">Из группы пользователей нельзя удалить участника, являющегося администратором этой группы</exception>
    public void DeleteMembers(DeleteUserGroupMembers delete)
    {
        var redactor = new UserGroupRedactor(dbContext);
        redactor.DeleteMembers(delete);
    }



    private void LoadUserGroupRecursively(UserGroup usergroup)
    {
        dbContext.Entry(usergroup)
            .Collection(ug => ug.ChildrenGroups)
            .Query()
            .Include(ug => ug.Admin)
            .Load();
        dbContext.Entry(usergroup)
            .Collection(ug => ug.MemberRights)
            .Query()
            .Include(ug => ug.User)
            .Load();
        
        foreach (var child in usergroup.ChildrenGroups) 
            LoadUserGroupRecursively(child);
    }
}