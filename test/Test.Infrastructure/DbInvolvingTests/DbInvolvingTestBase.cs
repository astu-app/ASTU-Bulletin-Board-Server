using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.TestDbFiller;

namespace Test.Infrastructure.DbInvolvingTests;

public class DbInvolvingTestBase
{
    protected readonly ApplicationDbContext DbContext;



    protected DbInvolvingTestBase()
    {
        var database = new DatabaseCreator();
        Task.Run(database.InitializeAsync).Wait();

        DbContext = database.CreateContext();

        var filler = new DatabaseFiller(DbContext);
        filler.FillWithTestData();
    }
}