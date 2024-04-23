using BulletInBoardServer.Controllers.CategoriesController.Models;
using BulletInBoardServer.Domain.Models.AnnouncementCategories;
using BulletInBoardServer.Services.Services.AnnouncementCategories.Models;
using BulletInBoardServer.Services.Services.Common.Models;
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
        
        config.NewConfig<AnnouncementCategory, AnnouncementCategoryDetailsDto>()
            .TwoWays()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Name, s => s.Name)
            .Map(d => d.Color, s => s.ColorHex);

        config.NewConfig<UpdateAnnouncementCategoryDto, EditCategory>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Name, s => s.Name)
            .Map(d => d.Color, s => s.ColorHex);

        config.NewConfig<UpdateAnnouncementCategoriesSubscriptionsDto, UpdateSubscriptions>()
            .Map(d => d.Update, s => s.ChangedIds);

        config.NewConfig<UpdateIdentifierListDto, UpdateIdentifierList>()
            .Map(d => d.ToAdd, s => s.ToAdd)
            .Map(d => d.ToRemove, s => s.ToRemove);
    }
}