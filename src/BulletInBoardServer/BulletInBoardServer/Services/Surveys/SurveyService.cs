using BulletInBoardServer.Data;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;

namespace BulletInBoardServer.Services.Surveys;

/// <summary>
/// Сервисный класс для взаимодействия с опросами
/// </summary>
/// <param name="dbContext">Контекст базы данных</param>
public class SurveyService(ApplicationDbContext dbContext)
{
    /// <summary>
    /// Закрыть опрос
    /// </summary>
    /// <param name="surveyId">Идентификатор закрываемого опроса</param>
    /// <remarks>Закрываемый опрос должен быть открыт</remarks>
    public void CloseSurvey(Guid surveyId)
    {
        var survey = GetSurveyOrThrow(surveyId);
        survey.Close();

        dbContext.SaveChangesAsync();
    }
    
    public PublicSurveyVoters GetPublicSurveyVoters(Guid surveyId)
    {
        var getter = new PublicSurveyVotersGetter(dbContext);
        return getter.GetVoters(surveyId);
    }

    public AnonymousSurveyVoters GetAnonymousSurveyVoters(Guid surveyId)
    {
        var getter = new AnonymousSurveyVotersGetter(dbContext);
        return getter.GetVoters(surveyId);
    }
    
    

    private Survey GetSurveyOrThrow(Guid surveyId)
    {
        try
        {
            var survey = dbContext.Surveys.Single(s => s.Id == surveyId);
            return survey;
        }
        catch (InvalidOperationException err)
        {
            throw new InvalidOperationException("Не удалось загрузить опрос из бд", err);
        }
    }
}