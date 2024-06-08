using Microsoft.AspNetCore.Http;

namespace BulletInBoardServer.Controllers.Core.Extensions;

public static class ApiControllerExtensions
{
    [Obsolete("мусор")]
    public static Guid? ExtractRequesterId(this IHeaderDictionary headers)
    {
        headers.TryGetValue("X-User-Id", out var requesterIdValues);
        var requesterIdStr = requesterIdValues.FirstOrDefault();
        if (string.IsNullOrEmpty(requesterIdStr))
            return null;

        if (Guid.TryParse(requesterIdStr, out var requesterId))
            return requesterId;

        return null;
    }
}