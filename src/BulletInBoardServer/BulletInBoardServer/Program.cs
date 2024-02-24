using System.Reflection;
using BulletInBoardServer.Domain;
using BulletInBoardServer.Services.Services.Announcements;
using BulletInBoardServer.Services.Services.Announcements.DelayedOperations;
using BulletInBoardServer.Services.Services.Announcements.ServiceCore;
using BulletInBoardServer.Services.Services.Surveys;
using BulletInBoardServer.Services.Services.Surveys.DelayedOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using AnnouncementsInputFormatterStream =
    BulletInBoardServer.Controllers.AnnouncementsController.Formatters.InputFormatterStream;
using AnnouncementsBasePathFilter = BulletInBoardServer.Controllers.AnnouncementsController.Filters.BasePathFilter;
using AnnouncementsGeneratePathParamsValidationFilter =
    BulletInBoardServer.Controllers.AnnouncementsController.Filters.GeneratePathParamsValidationFilter;

using PingInputFormatterStream = BulletInBoardServer.Controllers.PingController.Formatters.InputFormatterStream;
using PingBasePathFilter = BulletInBoardServer.Controllers.PingController.Filters.BasePathFilter;
using PingGeneratePathParamsValidationFilter =
    BulletInBoardServer.Controllers.PingController.Filters.GeneratePathParamsValidationFilter;

using SurveysInputFormatterStream = BulletInBoardServer.Controllers.SurveysController.Formatters.InputFormatterStream;

const string apiVersion = "0.0.2";

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
    options.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);

    options.SwaggerDoc(apiVersion, new OpenApiInfo
    {
        Title = "API Шлюз. Система информирования",
        Description = "API Шлюз системы информирования (ASP.NET Core 8.0)"
    });
    options.IncludeXmlComments(
        $"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{Assembly.GetEntryAssembly()!.GetName().Name}.xml");

    // Sets the basePath property in the OpenAPI document generated
    options.DocumentFilter<AnnouncementsBasePathFilter>("/api");
    options.DocumentFilter<PingBasePathFilter>("/api");

    // Include DataAnnotation attributes on Controller Action parameters as OpenAPI validation rules (e.g required, pattern, ..)
    options.OperationFilter<AnnouncementsGeneratePathParamsValidationFilter>();
    options.OperationFilter<PingGeneratePathParamsValidationFilter>();
});

var connectionString = builder.Configuration.GetConnectionString("MainDatabase") ??
                       throw new ApplicationException("Не удалось подключить строку подключения к базу данных");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

RegisterAnnouncementService();
RegisterSurveyService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => options.RouteTemplate = "openapi/{documentName}/openapi.json");
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "openapi";
        c.SwaggerEndpoint($"/openapi/{apiVersion}/openapi.json", "API Шлюз. Система информирования");
    });
}

InitDelayedAnnouncementOperationsDispatcher();
InitAutomaticSurveyOperationsDispatcher();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

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

void InitDelayedAnnouncementOperationsDispatcher()
{
    var serviceScopeFactory = GetServiceFactory();
    using var scope = serviceScopeFactory.CreateScope();

    var services = scope.ServiceProvider;

    var dbContext = GetDbContext(services);
    var publicationService = services.GetService<DelayedPublicationAnnouncementService>() ??
                             throw new ApplicationException(
                                 "Сервис работы с объявлениями, ожидающими отложенной публикации, не зарегистрирован");
    var hidingService = services.GetService<DelayedHidingAnnouncementService>() ??
                        throw new ApplicationException(
                            "Сервис работы с объявлениями, ожидающими отложенного сокрытия, не зарегистрирован");
    DelayedAnnouncementOperationsDispatcher.Init(dbContext, publicationService, hidingService);
}

void InitAutomaticSurveyOperationsDispatcher()
{
    var serviceScopeFactory = GetServiceFactory();
    using var scope = serviceScopeFactory.CreateScope();

    var services = scope.ServiceProvider;

    var dbContext = GetDbContext(services);
    var closingService = services.GetService<AutoClosingSurveyService>() ??
                         throw new ApplicationException(
                             "Сервис работы с опросами, ожидающими автоматического закрытия, не зарегистрирован");
    AutomaticSurveyOperationsDispatcher.Init(dbContext, closingService);
}

IServiceScopeFactory GetServiceFactory() =>
    app.Services.GetService<IServiceScopeFactory>() ??
    throw new ApplicationException($"Не удалось получить экземпляр {nameof(IServiceScopeFactory)}");

ApplicationDbContext GetDbContext(IServiceProvider services) =>
    services.GetService<ApplicationDbContext>() ??
    throw new ApplicationException("Контекст базы данных не зарегистрирован");