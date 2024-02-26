using BulletInBoardServer.Controllers.AnnouncementsController.Controllers;
using BulletInBoardServer.Controllers.PingController.Controllers;
using BulletInBoardServer.Controllers.SurveysController.Controllers;
using BulletInBoardServer.Domain;
using BulletInBoardServer.Infrastructure;
using BulletInBoardServer.Services.Services.Announcements;
using BulletInBoardServer.Services.Services.Announcements.DelayedOperations;
using BulletInBoardServer.Services.Services.Announcements.ServiceCore;
using BulletInBoardServer.Services.Services.Surveys;
using BulletInBoardServer.Services.Services.Surveys.DelayedOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using AnnouncementsInputFormatterStream = BulletInBoardServer.Controllers.AnnouncementsController.Formatters.InputFormatterStream;
using PingInputFormatterStream = BulletInBoardServer.Controllers.PingController.Formatters.InputFormatterStream;
using SurveysInputFormatterStream = BulletInBoardServer.Controllers.SurveysController.Formatters.InputFormatterStream;

const string apiVersion = "0.0.2";
var controllerClasses = new[]
{
    typeof(AnnouncementsApiController), 
    typeof(SurveysApiController), 
    typeof(PingApiController)
};

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, new AnnouncementsInputFormatterStream());
    options.InputFormatters.Insert(1, new PingInputFormatterStream());
    options.InputFormatters.Insert(2, new SurveysInputFormatterStream());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(apiVersion, new OpenApiInfo
    {
        Title = "API Шлюз. Система информирования",
        Description = "API Шлюз системы информирования (ASP.NET Core 8.0)",
    });
    
    foreach (var controller in controllerClasses)
    {
        var assembly = controller.Assembly;
        var assemblyDirectory = new FileInfo(assembly.Location).Directory!.FullName;
        var assemblyName = assembly.GetName().Name;
        var docFile = Path.Join(assemblyDirectory, $"{assemblyName}.xml");

        options.IncludeXmlComments(docFile);
    }
});

var connectionString = builder.Configuration.GetConnectionString("MainDatabase") ??
                       throw new ApplicationException("Не удалось подключить строку подключения к базу данных");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

RegisterAnnouncementService();
RegisterSurveyService();

var app = builder.Build();

app.RegisterMapsterConfiguration();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => options.RouteTemplate = "openapi/{documentName}/openapi.json");
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "openapi";
        options.SwaggerEndpoint($"/openapi/{apiVersion}/openapi.json", "API Шлюз. Система информирования");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

Task.Run(InitDelayedAnnouncementOperationsDispatcherAsync);
Task.Run(InitAutomaticSurveyOperationsDispatcherAsync);

app.Run();
return;


void RegisterAnnouncementService()
{
    builder.Services
        .AddScoped<AnnouncementService>()
        .AddScoped<IDelayedAnnouncementOperationsDispatcher, DelayedAnnouncementOperationsDispatcher>()
        .AddScoped<GeneralOperationsService>()
        .AddScoped<PublishedAnnouncementService>()
        .AddScoped<HiddenAnnouncementService>()
        .AddScoped<DelayedPublicationAnnouncementService>()
        .AddScoped<DelayedHidingAnnouncementService>()
        .AddScoped<DelayedPublicationAnnouncementService>()
        .AddScoped<DelayedHidingAnnouncementService>();
}

void RegisterSurveyService()
{
    builder.Services
        .AddScoped<SurveyService>()
        .AddScoped<IAutomaticSurveyOperationsDispatcher, AutomaticSurveyOperationsDispatcher>()
        .AddScoped<AutoClosingSurveyService>();
}

Task InitDelayedAnnouncementOperationsDispatcherAsync()
{
    var serviceScopeFactory = GetServiceFactory();
    using var scope = serviceScopeFactory.CreateScope();

    var services = scope.ServiceProvider;

    var dbContext = GetDbContext(services);
    var publicationService = GetPublicationService();
    var hidingService = GetHidingService();
    DelayedAnnouncementOperationsDispatcher.Init(dbContext, publicationService, hidingService);
    
    return Task.CompletedTask;



    DelayedPublicationAnnouncementService GetPublicationService() =>
        services.GetService<DelayedPublicationAnnouncementService>() ??
        throw new ApplicationException(
            "Сервис работы с объявлениями, ожидающими отложенной публикации, не зарегистрирован");

    DelayedHidingAnnouncementService GetHidingService() =>
        services.GetService<DelayedHidingAnnouncementService>() ??
        throw new ApplicationException(
            "Сервис работы с объявлениями, ожидающими отложенного сокрытия, не зарегистрирован");
}

Task InitAutomaticSurveyOperationsDispatcherAsync()
{
    var serviceScopeFactory = GetServiceFactory();
    using var scope = serviceScopeFactory.CreateScope();

    var services = scope.ServiceProvider;

    var dbContext = GetDbContext(services);
    var closingService = GetClosingService();
    AutomaticSurveyOperationsDispatcher.Init(dbContext, closingService);

    return Task.CompletedTask;



    AutoClosingSurveyService GetClosingService() =>
        services.GetService<AutoClosingSurveyService>() ??
        throw new ApplicationException(
            "Сервис работы с опросами, ожидающими автоматического закрытия, не зарегистрирован");
}

IServiceScopeFactory GetServiceFactory() =>
    app.Services.GetService<IServiceScopeFactory>() ??
    throw new ApplicationException($"Не удалось получить экземпляр {nameof(IServiceScopeFactory)}");

ApplicationDbContext GetDbContext(IServiceProvider services) =>
    services.GetService<ApplicationDbContext>() ??
    throw new ApplicationException("Контекст базы данных не зарегистрирован");