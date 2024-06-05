namespace BulletInBoardServer.Services.Services.UserGroups.Models;

public class EditUserGroup(
    Guid id,
    string? name,
    bool adminChanged,
    Guid? adminId,
    UpdateMemberList? members)
{
    public Guid Id { get; } = id;

    public string? Name { get; } = name;

    public bool AdminChanged { get; } = adminChanged;
    public Guid? AdminId { get; } = adminId;

    public UpdateMemberList? Members { get; } = members;
}