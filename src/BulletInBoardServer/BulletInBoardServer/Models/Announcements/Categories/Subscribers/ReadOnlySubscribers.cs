using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Announcements.Categories.Subscribers;

public class ReadOnlySubscribers(IList<User> users) : ReadOnlyUsers(users);