using Microsoft.AspNetCore.Mvc;

namespace BulletInBoardServer.Controllers.PingController.Controllers;

/// <summary>
/// 
/// </summary>
public class PingApiControllerImpl : PingApiController
{
    /// <inheritdoc />
    public override IActionResult Ping()
    {
        return Ok("pong");
    }
}