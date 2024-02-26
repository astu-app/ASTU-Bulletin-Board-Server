using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using Microsoft.Extensions.DependencyInjection;

namespace BulletInBoardServer.Services.Services.Surveys.DelayedOperations;

public class AutoClosingSurveyService(IServiceScopeFactory scopeFactory)
{
    public void CloseAutomatically(Guid surveyId)
    {
        // Автоматическое сокрытие отменяется в диспетчере, вызывающем этот метод

        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>() ??
                        throw new ApplicationException(
                            $"{nameof(ApplicationDbContext)} не зарегистрирован в качестве сервиса");
        
        var survey = GetSurveySummary(surveyId, dbContext);
        survey.Close();
        dbContext.SaveChanges();
    }



    private static Survey GetSurveySummary(Guid surveyId, ApplicationDbContext dbContext)
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