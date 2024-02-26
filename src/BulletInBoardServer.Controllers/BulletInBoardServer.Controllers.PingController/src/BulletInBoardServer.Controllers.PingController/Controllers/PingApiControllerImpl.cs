using Microsoft.AspNetCore.Mvc;

namespace BulletInBoardServer.Controllers.PingController.Controllers;

/// <summary>
/// Базовый класс контроллера проверки работоспособности сервера
/// </summary>
public class PingApiControllerImpl : PingApiController
{
    /// <summary>
    /// Проверка работоспособности сервера
    /// </summary>
    /// <response code="200">Ok</response>
    public override IActionResult Ping() => 
        Ok("pong");
}