using System;
using System.Collections.Generic;
using System.Linq;
using BulletInBoardServer.Controllers.UserGroupsController.Models;
using BulletInBoardServer.Domain.Models.UserGroups;
using BulletInBoardServer.Domain.Models.Users;
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
        config.NewConfig<AddMembersToUsergroupDto, AddUserGroupMembers>() 
            .Map(d => d.UserGroupId, s => s.UserGroupId)
            .Map(d => d.Members, s => s.Members);
        
        config.NewConfig<DeleteUsersFromUsergroupDto, DeleteUserGroupMembers>()
            .Map(d => d.UserGroupId, s => s.UserGroupId)
            .Map(d => d.MemberIds, s => s.MemberIds);

        config.NewConfig<CreateUserGroupDto, CreateUserGroup>()
            .Map(d => d.Name, s => s.Name)
            .Map(d => d.AdminId, s => s.AdminId)
            .Map(d => d.Members, s => s.Members)
            .Map(d => d.ParentGroupIds, s => s.ParentIds)
            .Map(d => d.ChildGroupIds, s => s.ChildIds);

        config.NewConfig<UserIdWithMemberRightsDto, SingleMemberRights>()
            .ConstructUsing(src =>
                new SingleMemberRights(
                    src.UserId,
                    src.UsergroupId ?? Guid.Empty,
                    src.Rights.CanViewAnnouncements,
                    src.Rights.CanCreateAnnouncements,
                    src.Rights.CanCreateSurveys,
                    src.Rights.CanViewUserGroupDetails,
                    src.Rights.CanCreateUserGroups,
                    src.Rights.CanEditUserGroups,
                    src.Rights.CanEditMembers,
                    src.Rights.CanEditMemberRights,
                    src.Rights.CanEditUserGroupAdmin,
                    src.Rights.CanDeleteUserGroup
                )
            );

        config.NewConfig<SingleMemberRights, UserSummaryWithMemberRightsDto>()
            .Map(d => d.User, s => s.User)
            .Map(d => d.Rights.CanViewAnnouncements, s => s.CanViewAnnouncements)
            .Map(d => d.Rights.CanCreateAnnouncements, s => s.CanCreateAnnouncements)
            .Map(d => d.Rights.CanCreateSurveys, s => s.CanCreateSurveys)
            .Map(d => d.Rights.CanViewUserGroupDetails, s => s.CanViewUserGroupDetails)
            .Map(d => d.Rights.CanCreateUserGroups, s => s.CanCreateUserGroups)
            .Map(d => d.Rights.CanEditUserGroups, s => s.CanEditUserGroups)
            .Map(d => d.Rights.CanEditMembers, s => s.CanEditMembers)
            .Map(d => d.Rights.CanEditMemberRights, s => s.CanEditMemberRights)
            .Map(d => d.Rights.CanEditUserGroupAdmin, s => s.CanEditUserGroupAdmin)
            .Map(d => d.Rights.CanDeleteUserGroup, s => s.CanDeleteUserGroup);

        config.NewConfig<UserGroup, UserGroupSummaryDto>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Name, s => s.Name)
            .Map(d => d.AdminName, s => s.Admin.FullName);

        config.NewConfig<UserGroupDetails, UserGroupDetailsDto>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Name, s => s.Name)
            .Map(d => d.Admin, s => s.Admin.Adapt<UserSummaryDto>()) 
            .Map(d => d.Members, s => s.MemberRights.Adapt<List<UserSummaryWithMemberRightsDto>>()) 
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
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Name, s => s.Name)
            .Map(d => d.AdminChanged, s => s.AdminChanged)
            .Map(d => d.AdminId, s => s.AdminId)
            .Map(d => d.Members, s => s.Members);

        config.NewConfig<UpdateMemberListDto, UpdateMemberList>()
            .Map(d => d.IdsToRemove, s => s.IdsToRemove)
            .Map(d => d.NewMembers, s => s.NewMembers);

        config.NewConfig<UserGroupList, UsergroupHierarchyDto>()
            .Map(d => d.Roots, s => s)
            .Map(d => d.Usergroups, s => s);

        config.NewConfig<UserGroupList, List<UserGroupSummaryWithMembersDto>>()
            .MapWith(userGroups => userGroups
                .SelectMany(GetAllUserGroupsFromHierarchy)
                .DistinctBy(ug => ug.Summary.Id)
                .ToList());
        
        config.NewConfig<UserGroup, UserGroupSummaryWithMembersDto>()
            .Map(d => d.Summary.Id, s => s.Id)
            .Map(d => d.Summary.Name, s => s.Name)
            .Map(d => d.Summary.AdminName, s => s.Admin.FullName)
            .Map(d => d.Members, s => s.MemberRights);
        
        config.NewConfig<UserGroup, UserGroupHierarchyNodeDto>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Children, s => s.ChildrenGroups)
            .PreserveReference(true);

        config.NewConfig<SingleMemberRights, UserSummaryDto>()
            .Map(d => d.Id, s => s.User.Id)
            .Map(d => d.FirstName, s => s.User.FirstName)
            .Map(d => d.SecondName, s => s.User.SecondName)
            .Map(d => d.Patronymic, s => s.User.Patronymic);

        config.NewConfig<SingleMemberRights, CheckableUserSummaryDto>()
            .Map(d => d.Id, s => s.User.Id)
            .Map(d => d.FirstName, s => s.User.FirstName)
            .Map(d => d.SecondName, s => s.User.SecondName)
            .Map(d => d.Patronymic, s => s.User.Patronymic)
            .Map(d => d.IsChecked, s => false);

        config.NewConfig<CreateUserGroupContent, GetUsergroupCreateContentDto>()
            .Map(d => d.Users, s => s.Users)
            .Map(d => d.Usergroups, s => s.UserGroups);
        
        config.NewConfig<UpdateUserGroupContent, ContentForUserGroupEditingDto>()
            .Map(d => d.Id, s => s.UserGroup.Id)
            .Map(d => d.Name, s => s.UserGroup.Name)
            .Map(d => d.Admin, s => s.UserGroup.Admin)
            .Map(d => d.Members, s => s.UserGroup.MemberRights)
            .Map(d => d.Users, s => s.PotentialMembers);
            // .Map(d => d.Parents, s => s.UserGroup.ParentGroups) // remove
            // .Map(d => d.Children, s => s.UserGroup.ChildrenGroups)
            // .Map(d => d.Usergroups, s => s.PotentialRelatives);
    }



    private static List<UserGroupSummaryWithMembersDto> GetAllUserGroupsFromHierarchy(UserGroup userGroup)
    {
        var list = new List<UserGroupSummaryWithMembersDto>
        {
            userGroup.Adapt<UserGroupSummaryWithMembersDto>(),
        };
        foreach (var child in userGroup.ChildrenGroups.OrderBy(ug => ug.Name))
            list.AddRange(GetAllUserGroupsFromHierarchy(child));

        return list;
    }
}