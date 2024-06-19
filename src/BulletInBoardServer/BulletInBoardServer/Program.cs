using BulletInBoardServer.Controllers.AnnouncementsController.Controllers;
using BulletInBoardServer.Controllers.SurveysController.Controllers;
using BulletInBoardServer.Controllers.UserGroupsController.Controllers;
using BulletInBoardServer.Domain;
using BulletInBoardServer.Infrastructure;
using BulletInBoardServer.Services.Services.Announcements;
using BulletInBoardServer.Services.Services.Announcements.DelayedOperations;
using BulletInBoardServer.Services.Services.Announcements.ServiceCore;
using BulletInBoardServer.Services.Services.Notifications;
using BulletInBoardServer.Services.Services.Surveys;
using BulletInBoardServer.Services.Services.Surveys.DelayedOperations;
using BulletInBoardServer.Services.Services.UserGroups;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using AnnouncementsInputFormatterStream = BulletInBoardServer.Controllers.AnnouncementsController.Formatters.InputFormatterStream;
using SurveysInputFormatterStream = BulletInBoardServer.Controllers.SurveysController.Formatters.InputFormatterStream;
using UserGroupInputFormatterStream = BulletInBoardServer.Controllers.UserGroupsController.Formatters.InputFormatterStream;

const string apiVersion = "0.0.3";
var controllerClasses = new[]
{
    typeof(AnnouncementsApiController), 
    typeof(SurveysApiController), 
    typeof(UserGroupsApiController),
};

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, new AnnouncementsInputFormatterStream());
    options.InputFormatters.Insert(1, new SurveysInputFormatterStream());
    options.InputFormatters.Insert(3, new UserGroupInputFormatterStream());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
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

// todo add docker
builder.Services.AddCors(options => options.AddPolicy("CORS_ip",
    policy => policy.WithOrigins(
            "http://localhost:5010",
            "https://localhost:7222",
            "http://192.168.1.11:5010",
            "https://192.168.1.11:7222")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()));

var connectionString = GetConnectionString();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddHealthChecks();

RegisterAnnouncementService();
RegisterSurveyService();
RegisterUserGroupService();
RegisterNotificationService();

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

app.UseCors("CORS_ip");
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/ping");

var initDelayedAnnouncementOperationsTask = Task.Run(InitDelayedAnnouncementOperationsDispatcherAsync);
var initAutomaticSurveyOperationsTask = Task.Run(InitAutomaticSurveyOperationsDispatcherAsync);

app.Run();

Task.WaitAll(initDelayedAnnouncementOperationsTask, initAutomaticSurveyOperationsTask);
return;


string GetConnectionString()
{
    if (builder.Environment.IsProduction())
        return ConstructConnectionStringFromEnvironment();

    return builder.Configuration.GetConnectionString("MainDatabase") ??
           throw new ApplicationException("Не удалось получить строку подключения к базе данных");
}

string ConstructConnectionStringFromEnvironment()
{
    var host = GetEnvironmentVariableOrThrow("DATABASE_HOST");
    var port = GetEnvironmentVariableOrThrow("DATABASE_PORT");
    var database = GetEnvironmentVariableOrThrow("DATABASE_NAME");
    var username = GetEnvironmentVariableOrThrow("DATABASE_USERNAME");
    var password = GetEnvironmentVariableOrThrow("DATABASE_PASSWORD");

    var connectionString_ =
        $"Host = {host}; Port = {port}; Database = {database}; Username = {username}; Password = {password};";
    return connectionString_;
}

void RegisterAnnouncementService() =>
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

void RegisterSurveyService() =>
    builder.Services
        .AddScoped<SurveyService>()
        .AddScoped<IAutomaticSurveyOperationsDispatcher, AutomaticSurveyOperationsDispatcher>()
        .AddScoped<AutoClosingSurveyService>();

void RegisterUserGroupService() => 
    builder.Services.AddScoped<UserGroupService>();

void RegisterNotificationService()
{
    NotificationServiceConfig config;
    if (builder.Environment.IsProduction())
    {
        var token = GetEnvironmentVariableOrThrow("NOTIFICATION_TOKEN");  
        var host = GetEnvironmentVariableOrThrow("NOTIFICATION_SERVER_HOST");
        config = new NotificationServiceConfig(token, host);
    }
    else
    {
        config = GetNotificationConfigFromAppConfiguration();
    }

    builder.Services.AddScoped<NotificationService>(_ =>
        new NotificationService(config.AppToken, config.ServerHost));
}

NotificationServiceConfig GetNotificationConfigFromAppConfiguration() =>
    builder.Configuration
        .GetSection("Notifications")
        .Get<NotificationServiceConfig>() 
    ?? throw new ApplicationException("Не удалось получить конфигурацию сервера уведомлений");

string GetEnvironmentVariableOrThrow(string variableName) =>
    Environment.GetEnvironmentVariable(variableName) ??
    throw new ApplicationException($"Переменная окружения {variableName} не задана");

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
    
    
    
record NotificationServiceConfig(string AppToken, string ServerHost);