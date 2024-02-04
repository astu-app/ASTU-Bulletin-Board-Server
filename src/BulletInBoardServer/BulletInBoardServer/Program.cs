using BulletInBoardServer.DataAccess;
using BulletInBoardServer.Services.Announcements.DelayedOperations;
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

builder.Services.AddSingleton<IDelayedAnnouncementOperationsDispatcher, DelayedAnnouncementOperationsDispatcher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

InitDelayedAnnouncementOperationsDispatcher();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
return;


void InitDelayedAnnouncementOperationsDispatcher()
{
    var serviceScopeFactory = app.Services.GetService<IServiceScopeFactory>() ?? throw new ApplicationException($"Не удалось получить экземпляр {nameof(IServiceScopeFactory)}");
    using var scope = serviceScopeFactory.CreateScope();
    
    var services = scope.ServiceProvider;

    var dispatcher = services.GetService<DelayedAnnouncementOperationsDispatcher>() ?? throw new ApplicationException("Диспетчер отложенных объявлений не зарегистрирован");
    dispatcher.Init();
}