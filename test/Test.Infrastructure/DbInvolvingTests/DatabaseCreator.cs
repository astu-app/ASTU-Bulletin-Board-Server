using BulletInBoardServer.Domain;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace Test.Infrastructure.DbInvolvingTests;

public class DatabaseCreator : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .WithResourceMapping(
            new DirectoryInfo("../../../../../src/BulletInBoardServer/initdb"),
            "/docker-entrypoint-initdb.d")
        .Build();



    public Task InitializeAsync() =>
        _postgres.StartAsync();

    public Task DisposeAsync() =>
        _postgres.DisposeAsync().AsTask();

    public ApplicationDbContext CreateContext() =>
        new(
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(_postgres.GetConnectionString())
                .LogTo(Console.WriteLine)
                .EnableSensitiveDataLogging()
                .Options
        );
}