using System.Collections.ObjectModel;
using BulletInBoardServer.Models.Users;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;

public class ReadOnlyVoters(IList<User> list) : ReadOnlyCollection<User>(list);