using System.ComponentModel;

namespace BulletInBoardServer.Services.Services.Surveys.DelayedOperations;

/// <summary>
/// Сервис, осуществляющий автоматическое закрытие опросов
/// </summary>
public class AutomaticSurveyClosingService : BackgroundWorker
{
    /// <summary>
    /// Частота проверки наступления момента закрытия в миллисекундах
    /// </summary>
    private const int ClosingMomentOccurenceCheckingFrequencyInMsecs = 1000;

    private readonly Guid _surveyId;

    private readonly DateTime _closeAt;
    private readonly AutoClosingSurveyService _closingSurveyService;



    /// <summary>
    /// Сервис автоматического сокрытия опросов
    /// </summary>
    /// <param name="surveyId">Id опроса, который требуется закрыть</param>
    /// <param name="closeAt">Момент автоматического закрытия опроса</param>
    /// <param name="closingSurveyService">Сервис работы с опросами, ожидающими автоматическое закрытие</param>
    public AutomaticSurveyClosingService(Guid surveyId, DateTime closeAt,
        AutoClosingSurveyService closingSurveyService)
    {
        _surveyId = surveyId;
        _closeAt = closeAt;
        _closingSurveyService = closingSurveyService;
    }



    protected override void OnDoWork(DoWorkEventArgs e)
    {
        e.Result = _surveyId;

        while (DateTime.Now < _closeAt)
        {
            if (CancellationPending)
                return;

            Thread.Sleep(ClosingMomentOccurenceCheckingFrequencyInMsecs);
        }

        _closingSurveyService.CloseAutomatically(_surveyId);
    }
}