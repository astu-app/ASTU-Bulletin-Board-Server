namespace BulletInBoardServer.Domain.Models.UserGroups;

public static class UserGroupExtensions
{
    public static UserGroupList ToUserGroupList(this IEnumerable<UserGroup> userGroups) =>
        new(userGroups);
}