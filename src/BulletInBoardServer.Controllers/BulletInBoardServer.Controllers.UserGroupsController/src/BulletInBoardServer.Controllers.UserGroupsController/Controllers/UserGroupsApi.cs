/*
 * API Шлюз. Группы пользователей
 *
 * API шлюза для управления группами пользователей
 *
 * The version of the OpenAPI document: 0.0.3
 *
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.ComponentModel.DataAnnotations;
using BulletInBoardServer.Controllers.UserGroupsController.Attributes;
using BulletInBoardServer.Controllers.UserGroupsController.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulletInBoardServer.Controllers.UserGroupsController.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public abstract class UserGroupsApiController : ControllerBase
    { 
        /// <summary>
        /// Добавить пользователей в группу пользователей
        /// </summary>
        /// <param name="addMembersToUsergroupDto"></param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("/api/usergroups/add-members")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 403, type: typeof(AddMembersToUsergroupForbidden))]
        [ProducesResponseType(statusCode: 404, type: typeof(AddMembersToUsergroupNotFound))]
        [ProducesResponseType(statusCode: 409, type: typeof(AddMembersToUsergroupConflict))]
        public abstract IActionResult AddMembersToUsergroup([FromBody]AddMembersToUsergroupDto addMembersToUsergroupDto);

        /// <summary>
        /// Создать группу пользователей
        /// </summary>
        /// <param name="createUserGroupDto"></param>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("/api/usergroups/create")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 201, type: typeof(CreateUsergroupCreated))]
        [ProducesResponseType(statusCode: 400, type: typeof(CreateUsergroupBadRequest))]
        [ProducesResponseType(statusCode: 403, type: typeof(CreateUsergroupForbidden))]
        [ProducesResponseType(statusCode: 404, type: typeof(CreateUsergroupNotFound))]
        [ProducesResponseType(statusCode: 409, type: typeof(CreateUsergroupConflict))]
        public abstract IActionResult CreateUsergroup([FromBody]CreateUserGroupDto createUserGroupDto);

        /// <summary>
        /// Удалить пользователей из группы пользователей
        /// </summary>
        /// <param name="deleteUsersFromUsergroupDto"></param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("/api/usergroups/delete-members")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 403, type: typeof(DeleteUsersFromUsergroupForbidden))]
        [ProducesResponseType(statusCode: 409, type: typeof(DeleteUsersFromUsergroupConflict))]
        public abstract IActionResult DeleteMembersFromUsergroup([FromBody]DeleteUsersFromUsergroupDto deleteUsersFromUsergroupDto);

        /// <summary>
        /// Удалить группу пользователей
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete]
        [Route("/api/usergroups/delete")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 400, type: typeof(DeleteUsergroupBadRequest))]
        [ProducesResponseType(statusCode: 404, type: typeof(DeleteUsergroupNotFound))]
        public abstract IActionResult DeleteUsergroup([FromBody]Guid body);

        /// <summary>
        /// Получение списка всех групп пользователей
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/api/usergroups/get-all")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetAllUsergroupsOk))]
        [ProducesResponseType(statusCode: 403, type: typeof(GetAllUsergroupsForbidden))]
        public abstract IActionResult GetAllUsergroups();

        /// <summary>
        /// Получение иерархии управляемых групп пользователей для пользователя
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/api/usergroups/get-owned-hierarchy")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetOwnedHierarchyOk))]
        [ProducesResponseType(statusCode: 403, type: typeof(GetOwnedHierarchyForbidden))]
        public abstract IActionResult GetOwnedHierarchy();

        /// <summary>
        /// Получение списка групп пользователей, администратором которой является пользователь, запрашивающий операцию
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/api/usergroups/get-owned-list")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetOwnedUsergroupsOk))]
        [ProducesResponseType(statusCode: 403, type: typeof(GetOwnedUsergroupsForbidden))]
        public abstract IActionResult GetOwnedUsergroups();

        /// <summary>
        /// Получение подробной информации о группе пользователей
        /// </summary>
        /// <param name="id">Идентификатор группы пользователей</param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/api/usergroups/get-details/{id}")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetUsergroupDetailsOk))]
        [ProducesResponseType(statusCode: 403, type: typeof(GetUsergroupDetailsForbidden))]
        [ProducesResponseType(statusCode: 404, type: typeof(GetUsergroupDetailsNotFound))]
        public abstract IActionResult GetUsergroupDetails([FromRoute (Name = "id")][Required]Guid id);

        /// <summary>
        /// Редактирование группы пользователей
        /// </summary>
        /// <param name="updateUserGroupDto"></param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut]
        [Route("/api/usergroups/update")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 400, type: typeof(UpdateUsergroupBadRequest))]
        [ProducesResponseType(statusCode: 403, type: typeof(UpdateUsergroupForbidden))]
        [ProducesResponseType(statusCode: 404, type: typeof(UpdateUsergroupNotFound))]
        [ProducesResponseType(statusCode: 409, type: typeof(UpdateUsergroupConflict))]
        public abstract IActionResult UpdateUsergroup([FromBody]UpdateUserGroupDto updateUserGroupDto);
    }
}
