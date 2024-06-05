using System;
using System.Collections.Generic;
using BulletInBoardServer.Controllers.Core.Logging;
using BulletInBoardServer.Controllers.Core.Responding;
using BulletInBoardServer.Controllers.UserGroupsController.Models;
using BulletInBoardServer.Domain.Models.UserGroups.Exceptions;
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
    private readonly UserGroupService _service;

    private readonly ILogger _logger = Log.ForContext<UserGroupService>();
    private readonly LoggingHelper _loggingHelper;



    /// <summary>
    /// Контроллер управления группами пользователей
    /// </summary>
    public UserGroupsApiControllerImpl(UserGroupService service)
    {
        _service = service;
        _loggingHelper = new LoggingHelper(_logger);
    }



    /// <summary>
    /// Добавить пользователей в группу пользователей
    /// </summary>
    /// <param name="dto"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="409">Conflict</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult AddMembersToUsergroup(AddMembersToUsergroupDto dto)
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

        var requesterId = Guid.Empty; // todo id пользователя

        try
        {
            var addMembersToUsergroup = dto.Adapt<AddUserGroupMembers>();
            _service.AddMembers(addMembersToUsergroup);

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
    /// <param name="dto"></param>
    /// <response code="201">Created</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="409">Conflict</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult CreateUsergroup(CreateUserGroupDto dto)
    {
        /*
         * 200
         * 400
         *   nameNullOrWhitespace +
         * 403
         *   usergroupCreationForbidden
         * 404
         *   usersDoNotExist  +
         *   UserGroupsDoNotExist +
         * 409
         *   adminCannotBeOrdinaryMember +
         *   cyclicDependency +
         * 500 +
         */

        var requesterId = Guid.Empty; // todo id пользователя

        try
        {
            var create = dto.Adapt<CreateUserGroup>();
            var usergroupId = _service.Create(create);

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
    /// <param name="dto"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="409">Conflict</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult DeleteMembersFromUsergroup(DeleteUsersFromUsergroupDto dto)
    {
        /*
         * 200 +
         * 403
         *   removeUsersFromUsergroupForbidden
         * 409
         *   userIsAdmin +
         * 500
         */

        var requesterId = Guid.Empty; // todo id пользователя

        try
        {
            var deleteMembersFromUsergroup = dto.Adapt<DeleteUserGroupMembers>();
            _service.DeleteMembers(deleteMembersFromUsergroup);

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
    /// <param name="id"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult DeleteUsergroup(Guid id)
    {
        /*
         * 200 +
         * 403
         *   usergroupDeletionForbidden
         * 404
         *   UserGroupDoesNotExist +
         * 500 +
         */

        var requesterId = Guid.Empty; // todo id пользователя

        try
        {
            _service.Delete(id);

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
    public override IActionResult GetAllUsergroups()
    {
        /*
         * 200 +
         * 403
         *   getAllUsergroupsForbidden
         * 500 +
         */

        var requesterId = Guid.Empty; // todo id пользователя

        try
        {
            var usergroups = _service.GetAll();

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
    public override IActionResult GetOwnedUsergroups()
    {
        /*
         * 200 +
         * 403
         *   getOwnedUsergroupsForbidden
         * 500 +
         */

        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // todo id пользователя

        try
        {
            var usergroups = _service.GetOwnedList(requesterId);

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
    public override IActionResult GetUsergroupCreateContent()
    {
        /*
         * 200 +
         * 403
         *   getUsergroupCreateContentForbidden
         * 500 +
         */

        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // todo id пользователя

        try
        {
            var usergroups = _service.GetContentForUserGroupCreation(requesterId);

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
    public override IActionResult GetOwnedHierarchy()
    {
        /*
         * 200 +
         * 403
         *   getUsergroupHierarchyForbidden
         * 500 +
         */

        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // todo id пользователя

        try
        {
            var usergroups = _service.GetUsergroupHierarchy(requesterId);

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
    /// <param name="id"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult GetUsergroupDetails(Guid id)
    {
        /*
         * 200 +
         * 403
         *   getUsergroupDetailsForbidden
         * 404
         *   UserGroupDoesNotExist +
         * 500 +
         */

        var requesterId = Guid.Empty; // todo id пользователя

        try
        {
            var details = _service.GetDetails(id);

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
    /// <param name="userGroupId">Идентификатор группы пользователей</param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult GetUsergroupUpdateContent(Guid userGroupId)
    {
        /*
         * 200 +
         * 403
         *   getUsergroupEditContentForbidden
         * 404
         *   userGroupDoesNotExist +
         * 500 +
         */
        
        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // todo id пользователя

        try
        {
            var content = _service.GetContentForUserGroupUpdating(userGroupId, requesterId);

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
    /// <param name="dto"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="409">Conflict</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult UpdateUsergroup(UpdateUserGroupDto dto)
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

        var requesterId = Guid.Empty; // todo id пользователя

        try
        {
            var edit = dto.Adapt<EditUserGroup>();
            _service.Edit(edit);

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