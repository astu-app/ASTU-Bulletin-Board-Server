using System.ComponentModel;
using BulletInBoardServer.DataAccess;
using BulletInBoardServer.Services.Announcements.ServiceCore;

namespace BulletInBoardServer.Services.Announcements.DelayedOperations;

public class DelayedAnnouncementOperationsDispatcher : IDelayedAnnouncementOperationsDispatcher
{
    private static Dictionary<Guid, DelayedAnnouncementPublishingService> _delayedPublishingServices = [];
    private static Dictionary<Guid, DelayedAnnouncementHidingService> _delayedHidingServices = [];

    private readonly DelayedPublicationAnnouncementService _publicationService;
    private readonly DelayedHidingAnnouncementService _hidingService;

    
    
    public DelayedAnnouncementOperationsDispatcher(DelayedPublicationAnnouncementService publicationService,
        DelayedHidingAnnouncementService hidingService)
    {
        _publicationService = publicationService;
        _hidingService = hidingService;
    }



    public static void Init(ApplicationDbContext dbContext,
        DelayedPublicationAnnouncementService publicationService, DelayedHidingAnnouncementService hidingService)
    {
        _delayedPublishingServices = dbContext.Announcements
            .Where(a => a.ExpectsDelayedPublishing)
            .ToDictionary(
                a => a.Id,
                a => new DelayedAnnouncementPublishingService(a.Id, a.DelayedPublishingAt!.Value, publicationService));
        // AutoPublishingAt не null, если ExpectsAutoPublishing true
        
        _delayedHidingServices = dbContext.Announcements
            .Where(a => a.ExpectsDelayedHiding)
            .ToDictionary(
                a => a.Id,
                a => new DelayedAnnouncementHidingService(a.Id, a.DelayedHidingAt!.Value, hidingService));
        // AutoHidingAt не null, если ExpectsAutoHiding true
    }
    
    
    
    /* *************************** Автоматическая публикация *************************** */

    public void ConfigureDelayedPublishing(Guid announcementId, DateTime publishAt)
    {
        var service = new DelayedAnnouncementPublishingService(announcementId, publishAt, _publicationService);
        service.WorkerSupportsCancellation = true;
        service.RunWorkerCompleted += RemoveDelayedPublishingServiceFromCollection;
        
        service.RunWorkerAsync();

        _delayedPublishingServices[announcementId] = service;
    }

    public void ReconfigureDelayedPublishing(Guid announcementId, DateTime publishAt)
    {
        // Если id уже сохранен, для объявления была сконфигурирована отложенная публикация. Отключаем ее. 
        if (_delayedPublishingServices.ContainsKey(announcementId)) 
            DisableDelayedPublishing(announcementId);
        
        ConfigureDelayedPublishing(announcementId, publishAt);
    }

    public void DisableDelayedPublishing(Guid announcementId) => 
        _delayedPublishingServices[announcementId].CancelAsync();

    private void RemoveDelayedPublishingServiceFromCollection(object? sender, RunWorkerCompletedEventArgs e)
    {
        if (e.Result is null)
            throw new ArgumentException(
                "Результат выполнения сервиса отложенной публикации объявления не может быть null");
        
        var announcementId = (Guid) e.Result;
        _delayedPublishingServices.Remove(announcementId);
    }



    /* *************************** Автоматическое сокрытие *************************** */

    public void ConfigureDelayedHiding(Guid announcementId, DateTime hideAt)
    {
        var service = new DelayedAnnouncementHidingService(announcementId, hideAt, _hidingService);
        service.WorkerSupportsCancellation = true;
        service.RunWorkerCompleted += RemoveDelayedHidingServiceFromCollection;
        
        service.RunWorkerAsync();

        _delayedHidingServices[announcementId] = service;
    }
    
    public void ReconfigureDelayedHiding(Guid announcementId, DateTime hideAt)
    {
        // Если id уже сохранен, для объявления было сконфигурировано отложенное сокрытие. Отключаем его.
        if (_delayedHidingServices.ContainsKey(announcementId)) 
            DisableDelayedHiding(announcementId);
        
        ConfigureDelayedHiding(announcementId, hideAt);
    }

    public void DisableDelayedHiding(Guid announcementId) => 
        _delayedHidingServices[announcementId].CancelAsync();

    private void RemoveDelayedHidingServiceFromCollection(object? sender, RunWorkerCompletedEventArgs e)
    {
        if (e.Result is null)
            throw new ArgumentException(
                "Результат выполнения сервиса отложенного сокрытия объявления не может быть null");
        
        var announcementId = (Guid) e.Result;
        _delayedHidingServices.Remove(announcementId);
    }
}