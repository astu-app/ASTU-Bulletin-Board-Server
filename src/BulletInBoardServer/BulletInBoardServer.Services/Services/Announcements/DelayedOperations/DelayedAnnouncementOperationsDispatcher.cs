using System.ComponentModel;
using BulletInBoardServer.Domain;
using BulletInBoardServer.Services.Services.Announcements.ServiceCore;

namespace BulletInBoardServer.Services.Services.Announcements.DelayedOperations;

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



    /// <summary>
    /// Метод загружает из базы данных все объявления, ожидающие отложенную публикацию и сокрытие, и сопоставляет
    /// каждому из них соответствующий сервис отложенной операции
    /// </summary>
    /// <param name="dbContext">Контекст базы данных</param>
    /// <param name="publicationService">Сервис отложенной публикации объявлений</param>
    /// <param name="hidingService">Сервис отложенного сокрытия объявлений</param>
    /// <param name="delayImMsecs">Задержка в миллисекундах между загрузкой объявлений с отложенной
    /// публикацией и загрузкой объявлений с отложенным сокрытием</param>
    /// <remarks>
    /// При загрузке объявления, ожидающего одновременно и отложенную публикацию, и отложенное сокрытие, попытка
    /// сокрытия объявления может произойти раньше попытки публикации объявления, из-за чего вылетит ошибка о
    /// невозможности сокрытия объявления раньше его публикации. Для предотвращения момента сокрытия объявления
    /// между инициализацией объявлений с отложенной публикацией и объявлений с отложенным сокрытием добавляется
    /// временной интервал
    /// </remarks>
    public static void Init(ApplicationDbContext dbContext,
        DelayedPublicationAnnouncementService publicationService, DelayedHidingAnnouncementService hidingService,
        int delayImMsecs = 5000)
    {
        _delayedPublishingServices = dbContext.Announcements
            .Where(a => a.ExpectsDelayedPublishing)
            .ToDictionary(
                a => a.Id,
                a => CreateAndRunAutoPublishingService(a.Id, a.DelayedPublishingAt!.Value, publicationService));
        // AutoPublishingAt не null, если ExpectsAutoPublishing true

        Thread.Sleep(delayImMsecs);

        _delayedHidingServices = dbContext.Announcements
            .Where(a => a.ExpectsDelayedHiding)
            .ToDictionary(
                a => a.Id,
                a => CreateAndRunDelayedHidingService(a.Id, a.DelayedHidingAt!.Value, hidingService));
        // AutoHidingAt не null, если ExpectsAutoHiding true
    }



    /* *************************** Автоматическая публикация *************************** */

    public void ConfigureDelayedPublishing(Guid announcementId, DateTime publishAt)
    {
        var service = CreateAndRunAutoPublishingService(announcementId, publishAt, _publicationService);
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

    private static DelayedAnnouncementPublishingService CreateAndRunAutoPublishingService(Guid announcementId,
        DateTime publishAt, DelayedPublicationAnnouncementService publicationService)
    {
        var service = new DelayedAnnouncementPublishingService(announcementId, publishAt, publicationService);
        service.WorkerSupportsCancellation = true;
        service.RunWorkerCompleted += RemoveDelayedPublishingServiceFromCollection;
        
        service.RunWorkerAsync();

        return service;
    }

    private static void RemoveDelayedPublishingServiceFromCollection(object? sender, RunWorkerCompletedEventArgs e)
    {
        if (e.Result is null)
            throw new ArgumentException(
                "Результат выполнения сервиса отложенной публикации объявления не может быть null");

        var announcementId = (Guid)e.Result;
        _delayedPublishingServices.Remove(announcementId);
    }



    /* *************************** Автоматическое сокрытие *************************** */

    public void ConfigureDelayedHiding(Guid announcementId, DateTime hideAt)
    {
        var service = CreateAndRunDelayedHidingService(announcementId, hideAt, _hidingService);
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

    private static DelayedAnnouncementHidingService CreateAndRunDelayedHidingService(Guid announcementId,
        DateTime hideAt, DelayedHidingAnnouncementService hidingService)
    {
        var service = new DelayedAnnouncementHidingService(announcementId, hideAt, hidingService);
        service.WorkerSupportsCancellation = true;
        service.RunWorkerCompleted += RemoveDelayedHidingServiceFromCollection;
        
        service.RunWorkerAsync();

        return service;
    }

    private static void RemoveDelayedHidingServiceFromCollection(object? sender, RunWorkerCompletedEventArgs e)
    {
        if (e.Result is null)
            throw new ArgumentException(
                "Результат выполнения сервиса отложенного сокрытия объявления не может быть null");

        var announcementId = (Guid)e.Result;
        _delayedHidingServices.Remove(announcementId);
    }
}