using BulletInBoardServer.DataAccess;
using BulletInBoardServer.Models.Attachments.Surveys;
using BulletInBoardServer.Models.Attachments.Surveys.Voters;
using BulletInBoardServer.Services.Surveys.VotersGetting;
using BulletInBoardServer.Services.Surveys.Voting;
using Microsoft.EntityFrameworkCore;

namespace BulletInBoardServer.Services.Surveys;

/// <summary>
/// Сервисный класс для взаимодействия с опросами
/// </summary>
public class SurveyService(ApplicationDbContext dbContext)
{
    /// <summary>
    /// Проголосовать в опросе
    /// </summary>
    /// <param name="voterId">Идентификатор голосующего пользователя</param>
    /// <param name="surveyId">Идентификатор опроса, в котором пользователь голосует</param>
    /// <param name="votes">Голоса пользователя в каждом из вопросов опроса</param>
    public void Vote(Guid voterId, Guid surveyId, SurveyVotes votes)
    {
        var votingService = new VotingService(dbContext);
        votingService.Vote(voterId, surveyId, votes);
    }
    
    /// <summary>
    /// Получить структурированный список проголосовавших в опросе пользователей
    /// </summary>
    /// <param name="surveyId">Идентификатор опроса</param>
    /// <returns>Структурированный список проголосовавших в опросе пользователей</returns>
    public SurveyVotersBase GetSurveyVoters(Guid surveyId)
    {
        var survey = LoadSurvey(surveyId);
        var getter = SurveyVotersGetterBase.ResolveVotersGetter(survey);
        return getter.GetVoters();
    }

    /// <summary>
    /// Закрыть опрос
    /// </summary>
    /// <param name="surveyId">Идентификатор закрываемого опроса</param>
    /// <remarks>Закрываемый опрос должен быть открыт</remarks>
    public void CloseSurvey(Guid surveyId)
    {
        var survey = LoadSurvey(surveyId);
        survey.Close();

        dbContext.SaveChanges();
    }



    private Survey LoadSurvey(Guid surveyId)
    {
        try
        {
            return dbContext.Surveys
                .Where(s => s.Id == surveyId)
                .Include(s => s.Voters)
                .Include(s => s.Questions)
                .ThenInclude(q => q.Answers)
                .ThenInclude(a => a.Participation)
                .ThenInclude(p => p.User)
                .Single();
        }
        catch (InvalidOperationException err)
        {
            throw new InvalidOperationException("Не удалось загрузить опрос из БД", err);
        }
    }
}