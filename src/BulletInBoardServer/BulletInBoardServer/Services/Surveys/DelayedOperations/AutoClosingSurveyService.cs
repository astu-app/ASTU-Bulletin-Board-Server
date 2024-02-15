using BulletInBoardServer.DataAccess;
using BulletInBoardServer.Models.Attachments.Surveys;

namespace BulletInBoardServer.Services.Surveys.DelayedOperations;

public class AutoClosingSurveyService(ApplicationDbContext dbContext)
{
    public void CloseAutomatically(Guid surveyId)
    {
        // Автоматическое сокрытие отменяется в диспетчере, вызывающем этот метод
        var survey = GetSurveySummary(surveyId);
        survey.Close();
        dbContext.SaveChanges();
    }



    private Survey GetSurveySummary(Guid surveyId) 
    {
        try
        {
            return dbContext.Surveys.Single(s => s.Id == surveyId);
        }
        catch (InvalidOperationException err)
        {
            throw new InvalidOperationException("Не удалось загрузить опрос из БД", err);
        }
    }
}