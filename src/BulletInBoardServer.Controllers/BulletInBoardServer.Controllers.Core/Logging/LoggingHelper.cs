using Serilog;

namespace BulletInBoardServer.Controllers.Core.Logging;

public class LoggingHelper(ILogger logger)
{
    public void LogWarning(int httpStatusCode, string operation, string reasonCode, Guid requesterId,
        string? additionalMessage = null) =>
        logger.Warning(
            "HTTP-код: {HttpStatusCode}; Операция: {Operation}; Код причины: {ReasonCode}; ID пользователя: {RequesterId}; Дополнительная информация: {AdditionalMessage}",
            httpStatusCode, operation, reasonCode, requesterId, additionalMessage);

    public void LogError(Exception err, int httpStatusCode, string operation, Guid requesterId) =>
        logger.Error(
            err, "HTTP-код: {HttpStatusCode}; Операция: {Operation}; ID пользователя: {RequesterId}",
            httpStatusCode, operation, requesterId);
}