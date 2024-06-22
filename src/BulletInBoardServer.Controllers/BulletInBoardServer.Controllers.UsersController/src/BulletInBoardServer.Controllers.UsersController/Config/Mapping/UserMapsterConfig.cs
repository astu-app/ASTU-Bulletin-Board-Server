using BulletInBoardServer.Controllers.UsersController.Models;
using BulletInBoardServer.Domain.Models.Users;
using Mapster;

namespace BulletInBoardServer.Controllers.UsersController.Config.Mapping;

// ReSharper disable once UnusedType.Global - will be used on startup while IRegisters scanning
/// <inheritdoc />
public class UserMapsterConfig : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserSummaryDto, User>()
            .ConstructUsing(s => new User(s.Id, s.FirstName, s.SecondName, s.Patronymic));
    }
}