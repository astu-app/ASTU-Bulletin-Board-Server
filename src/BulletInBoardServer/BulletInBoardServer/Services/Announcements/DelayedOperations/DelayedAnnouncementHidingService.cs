using System.ComponentModel;
using BulletInBoardServer.Services.Announcements.ServiceCore;

namespace BulletInBoardServer.Services.Announcements.DelayedOperations;

/// <summary>
/// Сервис, осуществляющий отложенное сокрытие объявлений
/// </summary>
public class DelayedAnnouncementHidingService : BackgroundWorker
{
    /// <summary>
    /// Частота проверки наступления момента сокрытия в миллисекундах
    /// </summary>
    private const int HidingMomentOccurenceCheckingFrequencyInMsecs = 1000;
    
    private readonly Guid _announcementId;

    private readonly DateTime _hideAt;
    private readonly DelayedHidingAnnouncementService _hidingAnnouncementService;



    /// <summary>
    /// Сервис, осуществляющий отложенное сокрытие объявлений
    /// </summary>
    /// <param name="announcementId">Id объявления, которое требуется сокрыть</param>
    /// <param name="hideAt">Момент отложенного сокрытия объявления</param>
    /// <param name="hidingAnnouncementService">Сервис, осуществляющий отложенное сокрытие объявлений</param>
    public DelayedAnnouncementHidingService(Guid announcementId, DateTime hideAt,
        DelayedHidingAnnouncementService hidingAnnouncementService)
    {
        _announcementId = announcementId;
        _hideAt = hideAt;
        _hidingAnnouncementService = hidingAnnouncementService;
    }



    protected override void OnDoWork(DoWorkEventArgs e)
    {
        e.Result = _announcementId;

        while (DateTime.Now < _hideAt)
        {
            if (CancellationPending)
                return;

            Thread.Sleep(HidingMomentOccurenceCheckingFrequencyInMsecs);
        }

        _hidingAnnouncementService.HideAutomatically(_announcementId, _hideAt);
    }
}