using BulletInBoardServer.Domain.Models.UserGroups;
using BulletInBoardServer.Domain.Models.Users;

namespace BulletInBoardServer.Services.Services.UserGroups.Models;

public class CreateUserGroupContent
{
    public UserList Users { get; set; }
    public UserGroupList UserGroups { get; set; }
    
    public CreateUserGroupContent(UserList users, UserGroupList userGroups)
    {
        Users = users;
        UserGroups = userGroups;
    }
}