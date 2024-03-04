using BulletInBoardServer.Services.Services.Common.Models;

namespace BulletInBoardServer.Services.Services.UserGroups.Models;

public class EditUserGroup(
    Guid id,
    string? name,
    bool adminChanged,
    Guid? adminId,
    UpdateIdentifierList? memberIds)
{
    public Guid Id { get; } = id;

    public string? Name { get; } = name;

    public bool AdminChanged { get; } = adminChanged;
    public Guid? AdminId { get; } = adminId;

    public UpdateIdentifierList? MemberIds { get; } = memberIds;
}