using BulletInBoardServer.DataAccess;
using BulletInBoardServer.Services.Announcements;
using BulletInBoardServer.Services.Announcements.DelayedOperations;
using BulletInBoardServer.Services.Announcements.ServiceCore;
using BulletInBoardServer.Services.Surveys;
using BulletInBoardServer.Services.Surveys.DelayedOperations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("MainDatabase") ??
                       throw new ApplicationException("Не удалось подключить строку подключения к базу данных");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

RegisterAnnouncementService();
RegisterSurveyService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
    var publicationService = services.GetService<DelayedPublicationAnnouncementService>() ?? throw new ApplicationException("Сервис работы с объявлениями, ожидающими отложенной публикации, не зарегистрирован");
    var hidingService = services.GetService<DelayedHidingAnnouncementService>() ?? throw new ApplicationException("Сервис работы с объявлениями, ожидающими отложенного сокрытия, не зарегистрирован");
    DelayedAnnouncementOperationsDispatcher.Init(dbContext, publicationService, hidingService);
}

void InitAutomaticSurveyOperationsDispatcher()
{
    var serviceScopeFactory = GetServiceFactory();
    using var scope = serviceScopeFactory.CreateScope();
    
    var services = scope.ServiceProvider;

    var dbContext = GetDbContext(services);
    var closingService = services.GetService<AutoClosingSurveyService>() ?? throw new ApplicationException("Сервис работы с опросами, ожидающими автоматического закрытия, не зарегистрирован");
    AutomaticSurveyOperationsDispatcher.Init(dbContext, closingService);
}

IServiceScopeFactory GetServiceFactory() =>
    app.Services.GetService<IServiceScopeFactory>() ?? throw new ApplicationException($"Не удалось получить экземпляр {nameof(IServiceScopeFactory)}");
    
ApplicationDbContext GetDbContext(IServiceProvider services) =>
    services.GetService<ApplicationDbContext>() ?? throw new ApplicationException("Контекст базы данных не зарегистрирован");
    