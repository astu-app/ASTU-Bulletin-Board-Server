using BulletInBoardServer.Data;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;
using BulletInBoardServer.Services.Surveys;
using BulletInBoardServer.Test.TestPreparation;
using Microsoft.EntityFrameworkCore;
using static BulletInBoardServer.Test.TestPreparation.TestDataIds;

namespace BulletInBoardServer.Test.Services.Surveys;

public class SurveyServiceTest
{
    private ApplicationDbContext _dbContext = null!;

    public SurveyServiceTest() => 
        Task.Run(InitDbContext).Wait();
    
    

    // ///////////////////////// Vote
    [Fact]
    public void Vote_VoteInSurvey_CountersIncreaseCorrectly()
    {
        var service = new SurveyService(_dbContext);
        var votes = new SurveyVotes(new Dictionary<Guid, List<Guid>>
        {
            [Question_1_WithSingleChoice_OfPublicSurvey] = [Answer_1_OfPublicSurvey],
            [Question_2_WithMultipleChoice_OfPublicSurvey] = [Answer_4_OfPublicSurvey, Answer_5_OfPublicSurvey]
        });

        service.Vote(MainUsergroupAdminId, PublicSurveyId, votes);

        LoadSurvey(PublicSurveyId).VotersCount.Should().Be(1);
        LoadAnswer(Answer_1_OfPublicSurvey).VotersCount.Should().Be(1);
        LoadAnswer(Answer_2_OfPublicSurvey).VotersCount.Should().Be(0);
        LoadAnswer(Answer_3_OfPublicSurvey).VotersCount.Should().Be(0);
        LoadAnswer(Answer_4_OfPublicSurvey).VotersCount.Should().Be(1);
        LoadAnswer(Answer_5_OfPublicSurvey).VotersCount.Should().Be(1);
        LoadAnswer(Answer_6_OfPublicSurvey).VotersCount.Should().Be(0);
    }

    [Fact]
    public void Vote_VoteInPublicSurvey_UserSelectionsAreSavedInDatabase()
    {
        var service = new SurveyService(_dbContext);
        var votes = new SurveyVotes(new Dictionary<Guid, List<Guid>>
        {
            [Question_1_WithSingleChoice_OfPublicSurvey] = [Answer_1_OfPublicSurvey],
            [Question_2_WithMultipleChoice_OfPublicSurvey] = [Answer_4_OfPublicSurvey, Answer_5_OfPublicSurvey]
        });
        
        service.Vote(MainUsergroupAdminId, PublicSurveyId, votes);
        
        LoadAnswer(Answer_1_OfPublicSurvey).Participation.Should().NotBeEmpty();
        LoadAnswer(Answer_4_OfPublicSurvey).Participation.Should().NotBeEmpty();
        LoadAnswer(Answer_5_OfPublicSurvey).Participation.Should().NotBeEmpty();
    }

    [Fact]
    public void Vote_VoteInAnonymousSurvey_UserSelectionsAreNotSavedInDatabase()
    {
        var service = new SurveyService(_dbContext);
        var votes = new SurveyVotes(new Dictionary<Guid, List<Guid>>
        {
            [Question_1_WithSingleChoice_OfAnonymousSurvey] = [Answer_2_OfAnonymousSurvey],
            [Question_2_WithMultipleChoice_OfAnonymousSurvey] = [Answer_5_OfAnonymousSurvey, Answer_6_OfAnonymousSurvey]
        });
        
        service.Vote(MainUsergroupAdminId, AnonymousSurveyId, votes);
        
        LoadAnswer(Answer_2_OfAnonymousSurvey).Participation.Should().BeEmpty();
        LoadAnswer(Answer_5_OfAnonymousSurvey).Participation.Should().BeEmpty();
        LoadAnswer(Answer_6_OfAnonymousSurvey).Participation.Should().BeEmpty();
    }

    [Fact]
    public void Vote_SelectMultipleAnswersInSingleChoiceQuestion_Throws()
    {
        var service = new SurveyService(_dbContext);
        var votes = new SurveyVotes(new Dictionary<Guid, List<Guid>>
        {
            // в опросе 1 можно выбрать только один вариант, этот сценарий приведет к падению
            [Question_1_WithSingleChoice_OfPublicSurvey] = [Answer_1_OfPublicSurvey, Answer_1_OfPublicSurvey],
            // в опросе 2 можно выбрать несколько вариантов
            [Question_2_WithMultipleChoice_OfPublicSurvey] = [Answer_4_OfPublicSurvey, Answer_5_OfPublicSurvey]
        });
        
        var vote = () => service.Vote(MainUsergroupAdminId, PublicSurveyId, votes);

        vote.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Vote_VoteInClosedSurvey_Throws()
    {
        var service = new SurveyService(_dbContext);
        var votes = new SurveyVotes(new Dictionary<Guid, List<Guid>>
        {
            [Question_1_WithSingleChoice_OfClosedAnonymousSurvey] = [Answer_1_OfClosedAnonymousSurvey],
        });
        
        var vote = () => service.Vote(MainUsergroupAdminId, ClosedAnonymousSurveyId, votes);

        vote.Should().Throw<InvalidOperationException>();
    }
    
    
    
    // ///////////////////////// Vote
    [Fact]
    public void GetSurveyVoters_GetPublicSurveyVoters_CorrectResult()
    {
        //// arrange
        var service = new SurveyService(_dbContext);
        var votes = new SurveyVotes(new Dictionary<Guid, List<Guid>>
        {
            [Question_1_WithSingleChoice_OfPublicSurvey] = [Answer_1_OfPublicSurvey],
            [Question_2_WithMultipleChoice_OfPublicSurvey] = [Answer_4_OfPublicSurvey, Answer_5_OfPublicSurvey]
        });
        
        // Для подготовки теста используется другой метод тестируемого сервиса, так как создавать еще один набор
        // тестовых данных займет много времени. Используемый метод тоже тестируется 
        service.Vote(MainUsergroupAdminId, PublicSurveyId, votes);

        var survey = LoadSurvey(PublicSurveyId);
        var expectedVoters = new PublicSurveyVoters(survey.Id, survey.Voters, []);
        foreach (var question in survey.Questions)
        {
            var answerVoters = new List<AnswerVoters>();
            expectedVoters.EveryQuestionVoters.Add(new QuestionVoters(question.Id, answerVoters));
            foreach (var answer in question.Answers)
            {
                var answerVoterLists = answer.Participation.Select(p => p.User).ToVoters();
                answerVoters.Add(new AnswerVoters(answer.Id, answerVoterLists));
            }
        }
        
        //// act
        var voters = service.GetSurveyVoters(PublicSurveyId);
        
        //// assert
        voters.Should().BeEquivalentTo(expectedVoters);
    }

    [Fact]
    public void GetSurveyVoters_GetAnonymousSurveyVoters_CorrectResult()
    {
        //// arrange
        var service = new SurveyService(_dbContext);
        var votes = new SurveyVotes(new Dictionary<Guid, List<Guid>>
        {
            [Question_1_WithSingleChoice_OfAnonymousSurvey] = [Answer_1_OfAnonymousSurvey],
            [Question_2_WithMultipleChoice_OfAnonymousSurvey] = [Answer_4_OfAnonymousSurvey, Answer_5_OfAnonymousSurvey]
        });
        
        // Для подготовки теста используется другой метод тестируемого сервиса, так как создавать еще один набор
        // тестовых данных займет много времени. Используемый метод тоже тестируется 
        service.Vote(MainUsergroupAdminId, AnonymousSurveyId, votes);

        var survey = LoadSurvey(PublicSurveyId);
        var expectedVoters = new AnonymousSurveyVoters(survey.Id, survey.Voters);
        
        //// act
        var voters = service.GetSurveyVoters(PublicSurveyId);
        
        //// assert
        voters.Should().BeEquivalentTo(expectedVoters);
    }



    private async Task InitDbContext()
    {
        var database = new DatabaseCreator();
        await database.InitializeAsync();
        
        _dbContext = database.CreateContext();
        _dbContext.FillWithTestData();
    }

    private Survey LoadSurvey(Guid surveyId) =>
        _dbContext.Surveys
            .Where(s => s.Id == surveyId)
            .Include(s => s.Voters)
            .Include(s => s.Questions)
            .ThenInclude(q => q.Answers)
            .Single();

    private Answer LoadAnswer(Guid answerId) =>
        _dbContext.Answers.Single(a => a.Id == answerId);
}