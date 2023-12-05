using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.Users;

public class ReadOnlyUsers(IList<User> users) : ReadOnlyCollection<User>(users);