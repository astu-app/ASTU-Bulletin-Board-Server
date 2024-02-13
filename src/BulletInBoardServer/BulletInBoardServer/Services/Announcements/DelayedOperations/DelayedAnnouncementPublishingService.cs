using System.ComponentModel;
using BulletInBoardServer.Services.Announcements.ServiceCore;

namespace BulletInBoardServer.Services.Announcements.DelayedOperations;

/// <summary>
/// Сервис, осуществляющий отложенную публикацию объявлений
/// </summary>
public class DelayedAnnouncementPublishingService : BackgroundWorker
{
    /// <summary>
    /// Частота проверки наступления момента публикации в миллисекундах
    /// </summary>
    private const int PublicationMomentOccurenceCheckingFrequencyInMsecs = 1000;
    
    private readonly Guid _announcementId;

    private readonly DateTime _publishAt;
    private readonly DelayedPublicationAnnouncementService _publishingService;



    /// <summary>
    /// Сервис, осуществляющий отложенную публикацию объявлений
    /// </summary>
    /// <param name="announcementId">Id объявления, которое требуется опубликовать</param>
    /// <param name="publishAt">Момент отложенной публикации объявления</param>
    /// <param name="publishingService">Сервис работы с объявлениями, ожидающими отложенную публикацию</param>
    public DelayedAnnouncementPublishingService(Guid announcementId, DateTime publishAt,
        DelayedPublicationAnnouncementService publishingService)
    {
        _announcementId = announcementId;
        _publishAt = publishAt;
        _publishingService = publishingService;
    }



    protected override void OnDoWork(DoWorkEventArgs e)
    {
        e.Result = _announcementId;

        while (DateTime.Now < _publishAt)
        {
            if (CancellationPending)
                return;

            Thread.Sleep(PublicationMomentOccurenceCheckingFrequencyInMsecs);
        }

        _publishingService.PublishAutomatically(_announcementId, _publishAt);
    }
}