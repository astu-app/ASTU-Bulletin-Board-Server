using BulletInBoardServer.Controllers.CategoriesController.Models;
using BulletInBoardServer.Services.Services.AnnouncementCategories.Models;
using Mapster;

namespace BulletInBoardServer.Controllers.CategoriesController.Config.Mapster;

// ReSharper disable once UnusedType.Global - will be used on startup while IRegisters scanning
/// <inheritdoc />
public class AnnouncementCategoryMapsterConfig : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateAnnouncementCategoryDto, CreateCategory>()
            .Map(d => d.Name, s => s.Name)
            .Map(d => d.Color, s => s.ColorHex);

        config.NewConfig<AnnouncementCategoryDetailsDto, CategorySummary>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Name, s => s.Name)
            .Map(d => d.Color, s => s.Color);

        config.NewConfig<UpdateAnnouncementCategoryDto, EditCategory>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Name, s => s.Name)
            .Map(d => d.Color, s => s.ColorHex);
    }
}