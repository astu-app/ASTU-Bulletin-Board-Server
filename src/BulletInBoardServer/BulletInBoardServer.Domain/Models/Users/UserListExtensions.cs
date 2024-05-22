namespace BulletInBoardServer.Domain.Models.Users;

public static class UserListExtensions
{
    public static UserList ToUserList(this IEnumerable<User> users) =>
        new(users.ToList());
}