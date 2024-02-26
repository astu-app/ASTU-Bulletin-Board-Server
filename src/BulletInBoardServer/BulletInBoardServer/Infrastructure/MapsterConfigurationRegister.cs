using Mapster;

namespace BulletInBoardServer.Infrastructure;

/// <summary>
/// 
/// </summary>
public static class MapsterConfigurationRegister
{
    /// <summary>
    /// Метод запускает обнаружение и регистрацию конфигураций маппинга Mapster
    /// </summary>
    /// <param name="app">Фиктивный параметр, используемый для однообразия конфигурации в классе Startup</param>
    public static void RegisterMapsterConfiguration(this WebApplication app) => 
        TypeAdapterConfig.GlobalSettings.Scan(AppDomain.CurrentDomain.GetAssemblies());
}