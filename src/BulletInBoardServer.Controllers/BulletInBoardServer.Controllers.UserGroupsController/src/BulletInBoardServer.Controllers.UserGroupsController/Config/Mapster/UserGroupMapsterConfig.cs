using System.Collections.Generic;
using BulletInBoardServer.Controllers.UserGroupsController.Models;
using BulletInBoardServer.Domain.Models.UserGroups;
using BulletInBoardServer.Domain.Models.Users;
using BulletInBoardServer.Services.Services.Common.Models;
using BulletInBoardServer.Services.Services.UserGroups.Models;
using Mapster;

namespace BulletInBoardServer.Controllers.UserGroupsController.Config.Mapster;

// ReSharper disable once UnusedType.Global - will be used on startup while IRegisters scanning
/// <inheritdoc />
public class UserGroupMapsterConfig : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddMembersToUsergroupDto, ChangeUserGroupMembers>() 
            .Map(d => d.UserGroupId, s => s.UserGroupId)
            .Map(d => d.UserIds, s => s.UserIds);
        
        config.NewConfig<DeleteUsersFromUsergroupDto, ChangeUserGroupMembers>()
            .Map(d => d.UserGroupId, s => s.UserGroupId)
            .Map(d => d.UserIds, s => s.UserIds);

        config.NewConfig<CreateUserGroupDto, CreateUserGroup>()
            .Map(d => d.Name, s => s.Name)
            .Map(d => d.AdminId, s => s.AdminId)
            .Map(d => d.MemberIds, s => s.MemberIds)
            .Map(d => d.ParentGroupIds, s => s.ParentIds)
            .Map(d => d.ChildGroupIds, s => s.ChildIds);

        config.NewConfig<UserGroup, UserGroupSummaryDto>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Name, s => s.Name);

        config.NewConfig<UserGroupDetails, UserGroupDetailsDto>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Name, s => s.Name)
            .Map(d => d.Admin, s => s.Admin.Adapt<UserSummaryDto>()) 
            .Map(d => d.Members, s => s.MemberRights.Adapt<List<UserSummaryDto>>()) 
            .Map(d => d.Parents, s => s.ParentGroups.Adapt<List<UserGroupSummaryDto>>()) 
            .Map(d => d.Children, s => s.ChildrenGroups.Adapt<List<UserGroupSummaryDto>>());

        config.NewConfig<User, UserSummaryDto>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.FirstName, s => s.FirstName)
            .Map(d => d.SecondName, s => s.SecondName)
            .Map(d => d.Patronymic, s => s.Patronymic);
        
        config.NewConfig<SingleMemberRights, UserSummaryDto>()
            .Map(d => d.Id, s => s.User.Id)
            .Map(d => d.FirstName, s => s.User.FirstName)
            .Map(d => d.SecondName, s => s.User.SecondName)
            .Map(d => d.Patronymic, s => s.User.Patronymic);
        
        config.NewConfig<UpdateUserGroupDto, EditUserGroup>()
            .ConstructUsing(s =>
                new EditUserGroup(
                    s.Id, 
                    s.Name, 
                    s.AdminChanged, 
                    s.AdminId, 
                    s.MembersChanged ? s.MemberIds.Adapt<UpdateIdentifierList>() : null));
    }
}