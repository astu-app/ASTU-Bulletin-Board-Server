using System;
using BulletInBoardServer.Controllers.UsersController.Models;
using BulletInBoardServer.Domain.Models.Users;
using BulletInBoardServer.Services.Services.Announcements;
using BulletInBoardServer.Services.Services.Users;
using BulletInBoardServer.Services.Services.Users.Exceptions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BulletInBoardServer.Controllers.UsersController.Controllers;

/// <summary>
/// Контроллер пользователей
/// </summary>
public class UsersApiControllerImpl : UsersApiController
{
    private readonly UserService _userService;

    private readonly ILogger _logger = Log.ForContext<AnnouncementService>();



    /// <summary>
    /// Контроллер пользователей
    /// </summary>
    public UsersApiControllerImpl(UserService userService)
    {
        _userService = userService;
    }



    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="userSummaryDto"></param>
    /// <response code="201">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult RegisterUser(UserSummaryDto userSummaryDto)
    {
        /*
         * 201 +
         * 409 +
         * 500 +
         */

        var user = userSummaryDto.Adapt<User>();
        try
        {
            _userService.RegisterUser(user);

            _logger.Information("Пользователь {UserId} был зарегистрирован в системе", user.Id);

            return Created();
        }
        catch (UserAlreadyExistsException)
        {
            _logger.Warning(
                "Пользователь {UserId} не был зарегистрирован в системе - пользователь уже существует",
                user.Id);
            return Conflict();
        }
        catch (Exception err)
        {
            _logger.Error(err, "Непредвиденная ошибка при регистрации пользователя {UserId}", user.Id);
            return Problem();
        }
    }
}