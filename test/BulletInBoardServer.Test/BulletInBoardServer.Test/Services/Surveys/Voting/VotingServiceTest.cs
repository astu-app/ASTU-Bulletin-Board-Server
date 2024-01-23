using BulletInBoardServer.Data;
using BulletInBoardServer.Test.TestPreparation;

namespace BulletInBoardServer.Test.Services.Surveys.Voting;

public class VotingServiceTest
{
    private ApplicationDbContext _dbContext = null!;

    public VotingServiceTest() => 
        Task.Run(InitDbContext).Wait();



    [Fact]
    public void Vote()
    {
        
    }



    private async Task InitDbContext()
    {
        var database = new DatabaseCreator();
        await database.InitializeAsync();
        
        _dbContext = database.CreateContext();
        _dbContext.FillWithTestData();
    }
}