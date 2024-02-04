using BulletInBoardServer.Models.Attachments.Surveys;
using BulletInBoardServer.Models.Attachments.Surveys.Answers;
using BulletInBoardServer.Models.Attachments.Surveys.Voters;
using BulletInBoardServer.Services.Surveys;
using BulletInBoardServer.Test.Infrastructure.DbInvolvingTests;
using Microsoft.EntityFrameworkCore;
using static BulletInBoardServer.Test.Infrastructure.DbInvolvingTests.TestDataIds;

namespace BulletInBoardServer.Test.Services.Surveys;

public class SurveyServiceTest : DbInvolvingTestBase
{
    // ///////////////////////// Vote
    [Fact]
    public void Vote_VoteInSurvey_CountersIncreaseCorrectly()
    {
        var service = new SurveyService(DbContext);
        var votes = new SurveyVotes(new Dictionary<Guid, List<Guid>>
        {
            [Question_1_WithSingleChoice_OfPublicSurvey_1_Id] = [Answer_1_OfPublicSurvey_1_Id],
            [Question_2_WithMultipleChoice_OfPublicSurvey_1_Id] = [Answer_4_OfPublicSurvey_1_Id, Answer_5_OfPublicSurvey_1_Id]
        });

        service.Vote(MainUsergroupAdminId, PublicSurvey_1_Id, votes);

        LoadSurvey(PublicSurvey_1_Id).VotersCount.Should().Be(1);
        LoadAnswer(Answer_1_OfPublicSurvey_1_Id).VotersCount.Should().Be(1);
        LoadAnswer(Answer_2_OfPublicSurvey_1_Id).VotersCount.Should().Be(0);
        LoadAnswer(Answer_3_OfPublicSurvey_1_Id).VotersCount.Should().Be(0);
        LoadAnswer(Answer_4_OfPublicSurvey_1_Id).VotersCount.Should().Be(1);
        LoadAnswer(Answer_5_OfPublicSurvey_1_Id).VotersCount.Should().Be(1);
        LoadAnswer(Answer_6_OfPublicSurvey_1_Id).VotersCount.Should().Be(0);
    }

    [Fact]
    public void Vote_VoteInPublicSurvey_UserSelectionsAreSavedInDatabase()
    {
        var service = new SurveyService(DbContext);
        var votes = new SurveyVotes(new Dictionary<Guid, List<Guid>>
        {
            [Question_1_WithSingleChoice_OfPublicSurvey_1_Id] = [Answer_1_OfPublicSurvey_1_Id],
            [Question_2_WithMultipleChoice_OfPublicSurvey_1_Id] = [Answer_4_OfPublicSurvey_1_Id, Answer_5_OfPublicSurvey_1_Id]
        });
        
        service.Vote(MainUsergroupAdminId, PublicSurvey_1_Id, votes);
        
        LoadAnswer(Answer_1_OfPublicSurvey_1_Id).Participation.Should().NotBeEmpty();
        LoadAnswer(Answer_4_OfPublicSurvey_1_Id).Participation.Should().NotBeEmpty();
        LoadAnswer(Answer_5_OfPublicSurvey_1_Id).Participation.Should().NotBeEmpty();
    }

    [Fact]
    public void Vote_VoteInAnonymousSurvey_UserSelectionsAreNotSavedInDatabase()
    {
        var service = new SurveyService(DbContext);
        var votes = new SurveyVotes(new Dictionary<Guid, List<Guid>>
        {
            [Question_1_WithSingleChoice_OfAnonymousSurvey_1_Id] = [Answer_2_OfAnonymousSurvey_1_Id],
            [Question_2_WithMultipleChoice_OfAnonymousSurvey_1_Id] = [Answer_5_OfAnonymousSurvey_1_Id, Answer_6_OfAnonymousSurvey_1_Id]
        });
        
        service.Vote(MainUsergroupAdminId, AnonymousSurvey_1_Id, votes);
        
        LoadAnswer(Answer_2_OfAnonymousSurvey_1_Id).Participation.Should().BeEmpty();
        LoadAnswer(Answer_5_OfAnonymousSurvey_1_Id).Participation.Should().BeEmpty();
        LoadAnswer(Answer_6_OfAnonymousSurvey_1_Id).Participation.Should().BeEmpty();
    }

    [Fact]
    public void Vote_SelectMultipleAnswersInSingleChoiceQuestion_Throws()
    {
        var service = new SurveyService(DbContext);
        var votes = new SurveyVotes(new Dictionary<Guid, List<Guid>>
        {
            // в опросе 1 можно выбрать только один вариант, этот сценарий приведет к падению
            [Question_1_WithSingleChoice_OfPublicSurvey_1_Id] = [Answer_1_OfPublicSurvey_1_Id, Answer_1_OfPublicSurvey_1_Id],
            // в опросе 2 можно выбрать несколько вариантов
            [Question_2_WithMultipleChoice_OfPublicSurvey_1_Id] = [Answer_4_OfPublicSurvey_1_Id, Answer_5_OfPublicSurvey_1_Id]
        });
        
        var vote = () => service.Vote(MainUsergroupAdminId, PublicSurvey_1_Id, votes);

        vote.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Vote_VoteInClosedSurvey_Throws()
    {
        var service = new SurveyService(DbContext);
        var votes = new SurveyVotes(new Dictionary<Guid, List<Guid>>
        {
            [Question_1_WithSingleChoice_OfClosedAnonymousSurvey_1_Id] = [Answer_1_OfClosedAnonymousSurvey_1_Id],
        });
        
        var vote = () => service.Vote(MainUsergroupAdminId, ClosedAnonymousSurvey_1_Id, votes);

        vote.Should().Throw<InvalidOperationException>();
    }
    
    
    
    // ///////////////////////// Vote
    [Fact]
    public void GetSurveyVoters_GetPublicSurveyVoters_CorrectResult()
    {
        //// arrange
        var service = new SurveyService(DbContext);
        var votes = new SurveyVotes(new Dictionary<Guid, List<Guid>>
        {
            [Question_1_WithSingleChoice_OfPublicSurvey_1_Id] = [Answer_1_OfPublicSurvey_1_Id],
            [Question_2_WithMultipleChoice_OfPublicSurvey_1_Id] = [Answer_4_OfPublicSurvey_1_Id, Answer_5_OfPublicSurvey_1_Id]
        });
        
        // Для подготовки теста используется другой метод тестируемого сервиса, так как создавать еще один набор
        // тестовых данных займет много времени. Используемый метод тоже тестируется 
        service.Vote(MainUsergroupAdminId, PublicSurvey_1_Id, votes);

        var survey = LoadSurvey(PublicSurvey_1_Id);
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
        var voters = service.GetSurveyVoters(PublicSurvey_1_Id);
        
        //// assert
        voters.Should().BeEquivalentTo(expectedVoters);
    }

    [Fact]
    public void GetSurveyVoters_GetAnonymousSurveyVoters_CorrectResult()
    {
        //// arrange
        var service = new SurveyService(DbContext);
        var votes = new SurveyVotes(new Dictionary<Guid, List<Guid>>
        {
            [Question_1_WithSingleChoice_OfAnonymousSurvey_1_Id] = [Answer_1_OfAnonymousSurvey_1_Id],
            [Question_2_WithMultipleChoice_OfAnonymousSurvey_1_Id] = [Answer_4_OfAnonymousSurvey_1_Id, Answer_5_OfAnonymousSurvey_1_Id]
        });
        
        // Для подготовки теста используется другой метод тестируемого сервиса, так как создавать еще один набор
        // тестовых данных займет много времени. Используемый метод тоже тестируется 
        service.Vote(MainUsergroupAdminId, AnonymousSurvey_1_Id, votes);

        var survey = LoadSurvey(PublicSurvey_1_Id);
        var expectedVoters = new AnonymousSurveyVoters(survey.Id, survey.Voters);
        
        //// act
        var voters = service.GetSurveyVoters(PublicSurvey_1_Id);
        
        //// assert
        voters.Should().BeEquivalentTo(expectedVoters);
    }



    private Survey LoadSurvey(Guid surveyId) =>
        DbContext.Surveys
            .Where(s => s.Id == surveyId)
            .Include(s => s.Voters)
            .Include(s => s.Questions)
            .ThenInclude(q => q.Answers)
            .Single();

    private Answer LoadAnswer(Guid answerId) =>
        DbContext.Answers.Single(a => a.Id == answerId);
}