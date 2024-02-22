using System.Text.Json;
using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.TestDbFiller;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

var connectionStringJson = File.ReadAllText("../../../settings.json");
var connectionString = JsonSerializer.Deserialize<ConnectionStringContainer>(connectionStringJson) ?? throw new ApplicationException("Не удалось прочитать строку подключения");

var dbContext = new ApplicationDbContext(
    new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseNpgsql(connectionString.ConnectionString)
        .LogTo(Console.WriteLine)
        .EnableSensitiveDataLogging()
        .Options
);

var t1 = DateTime.Now;
var filler = new DatabaseFiller(dbContext);
filler.FillWithTestData();
var t2 = DateTime.Now;

Console.Out.WriteLine($"\n\n\nБаза данных успешно заполнена за {t2 - t1}\n\n");



namespace BulletInBoardServer.Domain.TestDbFiller
{
    internal record ConnectionStringContainer(string ConnectionString);
}