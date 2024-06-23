using System;
using System.Collections.Generic;
using System.Linq;
using BulletInBoardServer.Controllers.Core.Logging;
using BulletInBoardServer.Controllers.Core.Responding;
using BulletInBoardServer.Controllers.UserGroupsController.Models;
using BulletInBoardServer.Domain.Models.UserGroups.Exceptions;
using BulletInBoardServer.Services.Services.MemberRights;
using BulletInBoardServer.Services.Services.UserGroups;
using BulletInBoardServer.Services.Services.UserGroups.Exceptions;
using BulletInBoardServer.Services.Services.UserGroups.Models;
using BulletInBoardServer.Services.Services.Users.Exceptions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BulletInBoardServer.Controllers.UserGroupsController.Controllers;

/// <summary>
/// Контроллер управления группами пользователей
/// </summary>
public class UserGroupsApiControllerImpl : UserGroupsApiController
{
    private readonly UserGroupService _userGroupService;
    private readonly MemberRightsLoader _rightsService;

    private readonly ILogger _logger = Log.ForContext<UserGroupService>();
    private readonly LoggingHelper _loggingHelper;



    /// <summary>
    /// Контроллер управления группами пользователей
    /// </summary>
    public UserGroupsApiControllerImpl(UserGroupService userGroupService, MemberRightsLoader rightsService)
    {
        _userGroupService = userGroupService;
        _rightsService = rightsService;
        _loggingHelper = new LoggingHelper(_logger);
    }



    /// <summary>
    /// Добавить пользователей в группу пользователей
    /// </summary>
    /// <param name="requesterId"></param>
    /// <param name="rootUserGroupId"></param>
    /// <param name="dto"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="409">Conflict</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult AddMembersToUsergroup([FromHeader(Name = "X-User-Id")]Guid requesterId, [FromHeader(Name = "X-Root-UserGroup-Id")]Guid rootUserGroupId, AddMembersToUsergroupDto dto)
    {
        /*
         * 200 +
         * 403
         *   addMembersToUsergroupForbidden
         * 404
         *   UserGroupDoesNotExist +
         *   usersDoNotExist +
         * 409
         *   userAlreadyMember +
         *   userIsAdmin +
         * 500 +
         */

        if (dto.Members.Count > 0 && !_rightsService.CanDo(mt => mt.CanCreateUserGroups, requesterId, rootUserGroupId))
            return StatusCode(403,
                ResponseConstructor.ConstructResponseWithOnlyCode(DeleteMembersFromUsergroupResponses
                    .RemoveUsersFromUsergroupForbidden));

        try
        {
            var addMembersToUsergroup = dto.Adapt<AddUserGroupMembers>();
            _userGroupService.AddMembers(addMembersToUsergroup);

            _logger.Information(
                "Пользователь {UserId} добавил пользователей в группу {GroupId}",
                requesterId,
                addMembersToUsergroup.UserGroupId);

            return Ok();
        }
        catch (UserDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Добавление участников в группу пользователей",
                nameof(AddMembersToUsergroupResponses.UsersDoNotExist), requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(AddMembersToUsergroupResponses.UsersDoNotExist));
        }
        catch (UserGroupDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Добавление участников в группу пользователей",
                nameof(AddMembersToUsergroupResponses.UserGroupDoesNotExist), requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(AddMembersToUsergroupResponses
                    .UserGroupDoesNotExist));
        }
        catch (UserIsAdminException err)
        {
            _loggingHelper.LogWarning(409, "Добавление участников в группу пользователей",
                nameof(AddMembersToUsergroupResponses.UserIsAdmin), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(AddMembersToUsergroupResponses.UserIsAdmin));
        }
        catch (UserIsAlreadyMemberOfUserGroup err)
        {
            _loggingHelper.LogWarning(409, "Добавление участников в группу пользователей",
                nameof(AddMembersToUsergroupResponses.UserAlreadyMember), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(AddMembersToUsergroupResponses
                    .UserAlreadyMember));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Добавление участников в группу пользователей", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Создать группу пользователей
    /// </summary>
    /// <param name="requesterId"></param>
    /// <param name="rootUserGroupId"></param>
    /// <param name="dto"></param>
    /// <response code="201">Created</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="409">Conflict</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult CreateUsergroup([FromHeader(Name = "X-User-Id")]Guid requesterId, [FromHeader(Name = "X-Root-UserGroup-Id")]Guid rootUserGroupId, CreateUserGroupDto dto)
    {
        /*
         * 200
         * 400
         *   nameNullOrWhitespace +
         * 403
         *   usergroupCreationForbidden +
         * 404
         *   usersDoNotExist  +
         *   UserGroupsDoNotExist +
         * 409
         *   adminCannotBeOrdinaryMember +
         *   cyclicDependency +
         * 500 +
         */

        var canDo = _rightsService.CanDo(mt => mt.CanCreateUserGroups, requesterId, rootUserGroupId);
        if (!canDo)
            return StatusCode(403, ResponseConstructor.ConstructResponseWithOnlyCode(CreateUsergroupResponses
                .UsergroupCreationForbidden));
        
        try
        {
            var create = dto.Adapt<CreateUserGroup>();
            var usergroupId = _userGroupService.Create(create);

            _logger.Information("Пользователь {UserId} создал группу {GroupId}", requesterId, usergroupId);

            return Created("/api/usergroups/get-details", usergroupId);
        }
        catch (UserGroupNameEmptyException err)
        {
            _loggingHelper.LogWarning(400, "Создание группы пользователей",
                nameof(CreateUsergroupResponses.NameIsNullOrWhitespace), requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(CreateUsergroupResponses
                    .NameIsNullOrWhitespace));
        }
        catch (UserGroupDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Создание группы пользователей",
                nameof(CreateUsergroupResponses.UserGroupsDoNotExist), requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(CreateUsergroupResponses
                    .UserGroupsDoNotExist));
        }
        catch (UserDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Создание группы пользователей",
                nameof(CreateUsergroupResponses.UsersDoNotExist), requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(CreateUsergroupResponses.UsersDoNotExist));
        }
        catch (AdminCannotBeOrdinaryMemberException err)
        {
            _loggingHelper.LogWarning(409, "Создание группы пользователей",
                nameof(CreateUsergroupResponses.AdminCannotBeOrdinaryMember), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(CreateUsergroupResponses
                    .AdminCannotBeOrdinaryMember));
        }
        catch (UserGroupCreatesCycleException err)
        {
            _loggingHelper.LogWarning(409, "Создание группы пользователей",
                nameof(CreateUsergroupResponses.CyclicDependency), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(CreateUsergroupResponses.CyclicDependency));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Создание группы пользователей", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Удалить пользователей из группы пользователей
    /// </summary>
    /// <param name="requesterId"></param>
    /// <param name="rootUserGroupId"></param>
    /// <param name="dto"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="409">Conflict</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult DeleteMembersFromUsergroup([FromHeader(Name = "X-User-Id")]Guid requesterId, [FromHeader(Name = "X-Root-UserGroup-Id")]Guid rootUserGroupId, DeleteUsersFromUsergroupDto dto)
    {
        /*
         * 200 +
         * 403
         *   removeUsersFromUsergroupForbidden
         * 409
         *   userIsAdmin +
         * 500
         */

        if (dto.MemberIds.Count > 0 && !_rightsService.CanDo(mt => mt.CanCreateUserGroups, requesterId, rootUserGroupId))
            return StatusCode(403,
                ResponseConstructor.ConstructResponseWithOnlyCode(DeleteMembersFromUsergroupResponses
                    .RemoveUsersFromUsergroupForbidden));
        
        try
        {
            var deleteMembersFromUsergroup = dto.Adapt<DeleteUserGroupMembers>();
            _userGroupService.DeleteMembers(deleteMembersFromUsergroup);

            _logger.Information("Пользователь {UserId} удалил пользователей из группы {GroupId}", requesterId,
                deleteMembersFromUsergroup.UserGroupId);

            return Ok();
        }
        catch (UserIsAdminException err)
        {
            _loggingHelper.LogWarning(409, "Удаление участников из группы пользователей",
                nameof(DeleteMembersFromUsergroupResponses.UserIsAdmin), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(DeleteMembersFromUsergroupResponses
                    .UserIsAdmin));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Удаление участников из группы пользователей", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Удалить группу пользователей
    /// </summary>
    /// <param name="requesterId"></param>
    /// <param name="rootUserGroupId"></param>
    /// <param name="id"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult DeleteUsergroup([FromHeader(Name = "X-User-Id")]Guid requesterId, [FromHeader(Name = "X-Root-UserGroup-Id")]Guid rootUserGroupId, Guid id)
    {
        /*
         * 200 +
         * 403
         *   usergroupDeletionForbidden
         * 404
         *   UserGroupDoesNotExist +
         * 500 +
         */
        
        var canDo = _rightsService.CanDo(mt => mt.CanDeleteUserGroup, requesterId, rootUserGroupId);
        if (!canDo)
            return StatusCode(403, ResponseConstructor.ConstructResponseWithOnlyCode(DeleteUsergroupResponses
                .UsergroupDeletionForbidden));
        
        try
        {
            _userGroupService.Delete(id);

            _logger.Information("Пользователь {UserId} удалил группу пользователей {GroupId}", requesterId, id);

            return Ok();
        }
        catch (UserGroupDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Удаление группы пользователей",
                nameof(DeleteUsergroupResponses.UserGroupDoesNotExist), requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(DeleteUsergroupResponses.UserGroupDoesNotExist));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Удаление группы пользователей", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Получение списка всех групп пользователей
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult GetAllUsergroups([FromHeader(Name = "X-User-Id")]Guid requesterId)
    {
        /*
         * 200 +
         * 403
         *   getAllUsergroupsForbidden
         * 500 +
         */
        
        try
        {
            var usergroups = _userGroupService.GetAll();

            _logger.Information("Пользователь {UserId} получил список всех групп пользователей", requesterId);

            var dtos = usergroups.Adapt<IEnumerable<UserGroupSummaryDto>>();
            return Ok(dtos);
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Получение списка всех групп пользователей", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Получение списка групп пользователей, администратором которой является пользователь, запрашивающий операцию
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult GetOwnedUsergroups([FromHeader(Name = "X-User-Id")]Guid requesterId)
    {
        /*
         * 200 +
         * 403
         *   getOwnedUsergroupsForbidden
         * 500 +
         */
        
        try
        {
            var usergroups = _userGroupService.GetOwnedList(requesterId);

            _logger.Information("Пользователь {UserId} получил список групп пользователей, в которых является администратором", requesterId);

            var dtos = usergroups.Adapt<IEnumerable<UserGroupSummaryDto>>();
            return Ok(dtos);
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Получение списка своих групп пользователей", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Получить данные для создания группы пользователей
    /// </summary>
    /// <response code="200">OK</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult GetUsergroupCreateContent([FromHeader(Name = "X-User-Id")]Guid requesterId, [FromHeader(Name = "X-Root-UserGroup-Id")]Guid rootUserGroupId)
    {
        /*
         * 200 +
         * 403
         *   getUsergroupCreateContentForbidden
         * 500 +
         */
        
        var canDo = _rightsService.CanDo(mt => mt.CanCreateUserGroups, requesterId, rootUserGroupId);
        if (!canDo)
            return StatusCode(403, ResponseConstructor.ConstructResponseWithOnlyCode(GetUsergroupCreateContentResponses
                .GetUsergroupCreateContentForbidden));

        try
        {
            var usergroups = _userGroupService.GetContentForUserGroupCreation(requesterId);

            _logger.Information("Пользователь {UserId} получил данные для создания группы пользователей", requesterId);

            var dto = usergroups.Adapt<GetUsergroupCreateContentDto>();
            return Ok(dto);
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Получение данных для создания группы пользователей", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Получение иерархии управляемых групп пользователей для пользователя
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult GetOwnedHierarchy([FromHeader(Name = "X-User-Id")]Guid requesterId)
    {
        /*
         * 200 +
         * 403
         *   getUsergroupHierarchyForbidden - если пользователь не может получить иерархии групп пользователей,
         *                                    в которых состоит, то он получает пустой список
         * 500 +
         */
        
        try
        {
            var usergroups = _userGroupService.GetUsergroupHierarchy(requesterId);

            _logger.Information("Пользователь {UserId} запросил иерархию своих групп пользователей", requesterId);

            var dto = usergroups.Adapt<UsergroupHierarchyDto>();
            return Ok(dto);
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Получение списка своих групп пользователей", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Получение подробной информации о группе пользователей
    /// </summary>
    /// <param name="requesterId"></param>
    /// <param name="rootUserGroupId"></param>
    /// <param name="id"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult GetUsergroupDetails([FromHeader(Name = "X-User-Id")]Guid requesterId, [FromHeader(Name = "X-Root-UserGroup-Id")]Guid rootUserGroupId, Guid id)
    {
        /*
         * 200 +
         * 403
         *   getUsergroupDetailsForbidden
         * 404
         *   UserGroupDoesNotExist +
         * 500 +
         */

        var canDo = _rightsService.CanDo(mt => mt.CanViewUserGroupDetails, requesterId, rootUserGroupId);
        if (!canDo)
            return StatusCode(403, ResponseConstructor.ConstructResponseWithOnlyCode(GetUsergroupDetailsResponses
                .GetUsergroupDetailsForbidden));
        
        try
        {
            var details = _userGroupService.GetDetails(id);

            _logger.Information(
                "Пользователь {UserId} получил подробную информацию о группе пользователей {GroupId}", requesterId,
                id);

            var dto = details.Adapt<UserGroupDetailsDto>();
            return Ok(dto);
        }
        catch (UserGroupDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Получение подробной информации о группе пользователей",
                nameof(GetUsergroupDetailsResponses.UserGroupDoesNotExist), requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(GetUsergroupDetailsResponses
                    .UserGroupDoesNotExist));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Получение подробной информации о группе пользователей",
                requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Получение данных для редактирования группы пользователей
    /// </summary>
    /// <param name="requesterId"></param>
    /// <param name="rootUserGroupId"></param>
    /// <param name="userGroupId">Идентификатор группы пользователей</param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult GetUsergroupUpdateContent([FromHeader(Name = "X-User-Id")]Guid requesterId, [FromHeader(Name = "X-Root-UserGroup-Id")]Guid rootUserGroupId, Guid userGroupId)
    {
        /*
         * 200 +
         * 403
         *   getUsergroupEditContentForbidden +
         * 404
         *   userGroupDoesNotExist +
         * 500 +
         */
        
        var canDo = _rightsService.CanDo(mt => mt.CanEditUserGroups, requesterId, rootUserGroupId);
        if (!canDo)
            return StatusCode(403, ResponseConstructor.ConstructResponseWithOnlyCode(
                ContentForUserGroupEditingResponses.GetUsergroupUpdateContentForbidden));

        try
        {
            var content = _userGroupService.GetContentForUserGroupUpdating(userGroupId, requesterId);

            _logger.Information(
                "Пользователь {UserId} получил данные для редактирования группы пользователей {GroupId}",
                requesterId, userGroupId);

            return Ok(content.Adapt<ContentForUserGroupEditingDto>());
        }
        catch (UserGroupDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Получение данных для редактирования группы пользователей",
                nameof(ContentForUserGroupEditingResponses.UserGroupDoesNotExist), requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(CreateUsergroupResponses
                    .UserGroupsDoNotExist));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Получение данных для редактирования группы пользователей",
                requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Редактирование группы пользователей
    /// </summary>
    /// <param name="requesterId"></param>
    /// <param name="rootUserGroupId"></param>
    /// <param name="dto"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="409">Conflict</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult UpdateUsergroup([FromHeader(Name = "X-User-Id")]Guid requesterId, [FromHeader(Name = "X-Root-UserGroup-Id")]Guid rootUserGroupId, UpdateUserGroupDto dto)
    {
        /*
         * 200 +
         * 400
         *   nameIsEmptyOrWhitespace +
         * 403
         *   updateUsergroupForbidden
         * 404
         *   usersDoNotExist +
         *   UserGroupsDoNotExist +
         * 409
         *   adminCannotBeOrdinaryMember +
         * 500 +
         */

        if (!_rightsService.CanDo(mt => mt.CanEditUserGroups, requesterId, rootUserGroupId))
            return StatusCode(403, ResponseConstructor.ConstructResponseWithOnlyCode(UpdateUsergroupResponses
                .UpdateUsergroupForbidden));
        
        if (dto.AdminChanged &&
            !_rightsService.CanDo(mt => mt.CanEditUserGroupAdmin, requesterId, rootUserGroupId))
            return StatusCode(403, ResponseConstructor.ConstructResponseWithOnlyCode(UpdateUsergroupResponses
                .ChangeAdminForbidden));
        
        var newMemberIds = dto.Members.NewMembers.Select(nm => nm.UserId).ToList();
        var memberToRemoveIds = dto.Members.IdsToRemove;
        // Добавили новых пользователей или удалили существующих (Eсли пользователь присутствует и там, и там, то
        // его права были изменены. Для отслеживания таких ситуаций используется Except)
        if ((newMemberIds.Except(memberToRemoveIds).Any() || memberToRemoveIds.Except(newMemberIds).Any()) &&
            !_rightsService.CanDo(mt => mt.CanEditMembers, requesterId, rootUserGroupId))
            return StatusCode(403, ResponseConstructor.ConstructResponseWithOnlyCode(UpdateUsergroupResponses
                .ChangeUsersForbidden));
        if (newMemberIds.Intersect(memberToRemoveIds).Any() &&
            !_rightsService.CanDo(mt => mt.CanEditMemberRights, requesterId, rootUserGroupId))
            return StatusCode(403, ResponseConstructor.ConstructResponseWithOnlyCode(UpdateUsergroupResponses
                .ChangeUserRightsForbidden));
        
        try
        {
            var edit = dto.Adapt<EditUserGroup>();
            _userGroupService.Edit(edit);

            _logger.Information("Пользователь {UserId} отредактировал группу пользователей {GroupId}",
                requesterId, edit.Id);

            return Ok();
        }
        catch (UserGroupNameEmptyException err)
        {
            _loggingHelper.LogWarning(400, "Редактирование группы пользователей",
                nameof(UpdateUsergroupResponses.NameIsNullOrWhitespace), requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(UpdateUsergroupResponses
                    .NameIsNullOrWhitespace));
        }
        // catch (UserGroupDoesNotExistException err) // remove
        // {
        //     _loggingHelper.LogWarning(404, "Редактирование группы пользователей",
        //         nameof(UpdateUsergroupResponses.UserGroupsDoNotExist), requesterId, err.Message);
        //     return NotFound(
        //         ResponseConstructor.ConstructResponseWithOnlyCode(CreateUsergroupResponses
        //             .UserGroupsDoNotExist));
        // }
        catch (UserDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Редактирование группы пользователей",
                nameof(UpdateUsergroupResponses.UsersDoNotExist), requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(UpdateUsergroupResponses.UsersDoNotExist));
        }
        catch (AdminCannotBeOrdinaryMemberException err)
        {
            _loggingHelper.LogWarning(409, "Редактирование группы пользователей",
                nameof(UpdateUsergroupResponses.AdminCannotBeOrdinaryMember), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(UpdateUsergroupResponses
                    .AdminCannotBeOrdinaryMember));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Редактирование группы пользователей",
                requesterId);
            return Problem();
        }
    }
}