using BulletInBoardServer.Domain.Models.UserGroups.Exceptions;
using BulletInBoardServer.Services.Services.Common.Models;
using BulletInBoardServer.Services.Services.UserGroups;
using BulletInBoardServer.Services.Services.UserGroups.Exceptions;
using BulletInBoardServer.Services.Services.UserGroups.Models;
using BulletInBoardServer.Services.Services.Users.Exceptions;
using FluentAssertions;
using JetBrains.Annotations;
using Test.Infrastructure.DbInvolvingTests;
using static BulletInBoardServer.Domain.TestDbFiller.TestDataIds;

namespace BulletInBoardServer.Services.Test.Services.UserGroups;

[TestSubject(typeof(UserGroupService))]
public class UserGroupServiceTest : DbInvolvingTestBase
{
    private readonly UserGroupService _userGroupService;



    public UserGroupServiceTest() =>
        _userGroupService = new UserGroupService(DbContext);



    // ///////////////////////// Create

    [Fact]
    public void Create_NewGroupDoesntCreateCycle_ShouldNotThrow()
    {
        var create = new CreateUserGroup
        {
            Name = "New group",
            ChildGroupIds = [UserGroup_4_Id],
            ParentGroupIds = [UserGroup_7_Id],
        };

        var action = () => _userGroupService.Create(create);

        action.Should().NotThrow();
    }

    [Fact]
    public void Create_NewGroupCreatesCycle_ShouldThrows()
    {
        var create = new CreateUserGroup
        {
            Name = "New group",
            ChildGroupIds = [UserGroup_5_Id, UserGroup_4_Id],
            ParentGroupIds = [UserGroup_3_Id, UserGroup_6_Id],
        };

        var action = () => _userGroupService.Create(create);

        action.Should().ThrowExactly<UserGroupCreatesCycleException>();
    }

    [Fact]
    public void Create_MemberNotPresentedInDb_ShouldThrow()
    {
        var create = new CreateUserGroup
        {
            Name = "New group",
            MemberIds = new[] { Guid.NewGuid() },
        };

        var action = () => _userGroupService.Create(create);

        action.Should().ThrowExactly<UserDoesNotExistException>();
    }

    [Fact]
    public void Create_AdminNotPresentedInDb_ShouldThrow()
    {
        var create = new CreateUserGroup
        {
            Name = "New group",
            AdminId = Guid.NewGuid(),
        };

        var action = () => _userGroupService.Create(create);

        action.Should().ThrowExactly<UserDoesNotExistException>();
    }

    [Fact]
    public void Create_ChildUserGroupNotPresentedInDb_ShouldThrow()
    {
        var create = new CreateUserGroup
        {
            Name = "New group",
            ChildGroupIds = new[] { Guid.NewGuid() },
        };

        var action = () => _userGroupService.Create(create);

        action.Should().ThrowExactly<UserGroupDoesNotExistException>();
    }

    [Fact]
    public void Create_ParentUserGroupNotPresentedInDb_ShouldThrow()
    {
        var create = new CreateUserGroup
        {
            Name = "New group",
            ParentGroupIds = new[] { Guid.NewGuid() },
        };

        var action = () => _userGroupService.Create(create);

        action.Should().ThrowExactly<UserGroupDoesNotExistException>();
    }

    [Fact]
    public void Create_NameIsNull_ShouldThrow()
    {
        var create = new CreateUserGroup
        {
            Name = null!,
            ParentGroupIds = new[] { Guid.NewGuid() },
        };

        var action = () => _userGroupService.Create(create);

        action.Should().ThrowExactly<UserGroupNameEmptyException>();
    }

    [Fact]
    public void Create_NameIsEmpty_ShouldThrow()
    {
        var create = new CreateUserGroup
        {
            Name = string.Empty,
            ParentGroupIds = new[] { Guid.NewGuid() },
        };

        var action = () => _userGroupService.Create(create);

        action.Should().ThrowExactly<UserGroupNameEmptyException>();
    }

    [Fact]
    public void Create_NameIsWhitespace_ShouldThrow()
    {
        var create = new CreateUserGroup
        {
            Name = "   ",
            ParentGroupIds = new[] { Guid.NewGuid() },
        };

        var action = () => _userGroupService.Create(create);

        action.Should().ThrowExactly<UserGroupNameEmptyException>();
    }



    // ///////////////////////// Edit

    [Fact]
    public void Edit_NameIsEmpty_ShouldThrow()
    {
        var edit = new EditUserGroup(
            id: UserGroup_1_Id,
            name: string.Empty, // тестируем установку имени
            adminChanged: false,
            adminId: null,
            memberIds: null
        );

        var action = () => _userGroupService.Edit(edit);

        action.Should().ThrowExactly<UserGroupNameEmptyException>();
    }

    [Fact]
    public void Edit_NameIsWhitespace_ShouldThrow()
    {
        var edit = new EditUserGroup(
            id: UserGroup_1_Id,
            name: "   ", // тестируем установку имени
            adminChanged: false,
            adminId: null,
            memberIds: null
        );

        var action = () => _userGroupService.Edit(edit);

        action.Should().ThrowExactly<UserGroupNameEmptyException>();
    }

    [Fact]
    public void Edit_AdminNotPresentedInDb_ShouldThrow()
    {
        var edit = new EditUserGroup(
            id: UserGroup_1_Id,
            name: null,
            adminChanged: true,
            adminId: Guid.NewGuid(), // тестируем установку админа
            memberIds: null
        );

        var action = () => _userGroupService.Edit(edit);

        action.Should().ThrowExactly<UserDoesNotExistException>();
    }

    [Fact]
    public void Edit_AddMemberNotPresentedInDb_ShouldThrow()
    {
        var memberIds = new UpdateIdentifierList
        {
            ToAdd = new[] { Guid.NewGuid() },
        };
        var edit = new EditUserGroup(
            id: UserGroup_1_Id,
            name: null,
            adminChanged: false,
            adminId: null,
            memberIds: memberIds // тестируем добавление пользователей
        );

        var action = () => _userGroupService.Edit(edit);

        action.Should().ThrowExactly<UserDoesNotExistException>();
    }



    // ///////////////////////// AddMembers
    [Fact]
    public void AddMembers_AddMemberThatIsAdmin_ShouldThrow()
    {
        var memberIds = new[] { UserGroup_1_AdminId };
        DbContext.ChangeTracker.Clear();

        var action = () => _userGroupService.AddMembers(new ChangeUserGroupMembers(UserGroup_1_Id, memberIds));

        action.Should().ThrowExactly<UserIsAdminException>();
    }

    [Fact]
    public void AddMembers_MemberAlreadyInUserGroup_ShouldThrow()
    {
        var memberIds = new[] { UsualUser_1_Id };
        DbContext.ChangeTracker.Clear();

        var action = () => _userGroupService.AddMembers(new ChangeUserGroupMembers(UserGroup_1_Id, memberIds));

        action.Should().ThrowExactly<UserIsAlreadyMemberOfUserGroup>();
    }

    [Fact]
    public void AddMembers_MembersNotPresentedInDb_ShouldThrow()
    {
        var memberIds = new[] { Guid.NewGuid() };
        DbContext.ChangeTracker.Clear();

        var action = () => _userGroupService.AddMembers(new ChangeUserGroupMembers(UserGroup_1_Id, memberIds));

        action.Should().ThrowExactly<UserDoesNotExistException>();
    }

    [Fact]
    public void AddMembers_UserGroupNotPresentedInDb_ShouldThrow()
    {
        var memberIds = new[] { UsualUser_2_Id };
        DbContext.ChangeTracker.Clear();

        var action = () => _userGroupService.AddMembers(new ChangeUserGroupMembers(Guid.NewGuid(), memberIds));

        action.Should().ThrowExactly<UserGroupDoesNotExistException>();
    }



    // ///////////////////////// DeleteMembers

    [Fact]
    public void DeleteMembers_DeleteMemberThatIsAdmin_ShouldThrow()
    {
        var memberIds = new[] { UserGroup_1_AdminId };
        DbContext.ChangeTracker.Clear();

        var action = () => _userGroupService.DeleteMembers(new ChangeUserGroupMembers(UserGroup_1_Id, memberIds));

        action.Should().ThrowExactly<UserIsAdminException>();
    }
}