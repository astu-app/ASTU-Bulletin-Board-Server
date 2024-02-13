using BulletInBoardServer.DataAccess;
using BulletInBoardServer.Models.Attachments.Surveys;

namespace BulletInBoardServer.Services.Surveys.DelayedOperations;

public class AutoClosingSurveyService(
    ApplicationDbContext dbContext,
    IAutomaticSurveyOperationsDispatcher dispatcher)
{
    public void CloseAutomatically(Guid surveyId)
    {
        var survey = GetSurveySummary(surveyId);
        
        dispatcher.DisableAutoClosing(surveyId);

        survey.Close();
        dbContext.SaveChanges();
        Console.Out.WriteLine("closed automatically"); // debug
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