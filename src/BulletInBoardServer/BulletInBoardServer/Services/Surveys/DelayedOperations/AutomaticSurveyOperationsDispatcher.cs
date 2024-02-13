using System.ComponentModel;
using BulletInBoardServer.DataAccess;

namespace BulletInBoardServer.Services.Surveys.DelayedOperations;

public class AutomaticSurveyOperationsDispatcher : IAutomaticSurveyOperationsDispatcher
{
    private Dictionary<Guid, AutomaticSurveyClosingService> _automaticClosingServices = [];

    private readonly ApplicationDbContext _dbContext;

    private readonly AutoClosingSurveyService _closingService;



    public AutomaticSurveyOperationsDispatcher(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _closingService = new AutoClosingSurveyService(dbContext, this);
    }
    
    
    
    public void Init()
    {
        _automaticClosingServices = _dbContext.Surveys
            .Where(a => a.ExpectsAutoClosing)
            .ToDictionary(
                a => a.Id,
                a => new AutomaticSurveyClosingService(a.Id, a.AutoClosingAt!.Value, _closingService));
        // AutoClosingAt не null, если ExpectsAutoClosing true
    }

    public void ConfigureAutoClosing(Guid surveyId, DateTime closeAt)
    {
        var service = new AutomaticSurveyClosingService(surveyId, closeAt, _closingService);
        service.WorkerSupportsCancellation = true;
        service.RunWorkerCompleted += RemoveDelayedClosingServiceFromCollection;

        service.RunWorkerAsync();

        _automaticClosingServices[surveyId] = service;
    }

    public void DisableAutoClosing(Guid surveyId) =>
        _automaticClosingServices[surveyId].CancelAsync();



    private void RemoveDelayedClosingServiceFromCollection(object? sender, RunWorkerCompletedEventArgs e)
    {
        if (e.Result is null)
            throw new ArgumentException(
                "Результат выполнения сервиса автоматического сокрытия опроса не может быть null");

        var announcementId = (Guid)e.Result;
        _automaticClosingServices.Remove(announcementId);
    }
}