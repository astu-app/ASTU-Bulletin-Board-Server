using BulletInBoardServer.Models.Announcements;
using BulletInBoardServer.Services.Announcements.Infrastructure;
using BulletInBoardServer.Services.Announcements.ServiceCore;

namespace BulletInBoardServer.Services.Announcements;

/// <summary>
/// Класс, агрегирующий операции непосредственно над объявлениями
/// </summary>
public class AnnouncementService(
    GeneralOperationsService generalOperationsService, 
    PublishedAnnouncementService publishedAnnouncementService,
    HiddenAnnouncementService hiddenAnnouncementService,
    DelayedPublicationAnnouncementService delayedPublicationAnnouncementService,
    DelayedHidingAnnouncementService delayedHidingAnnouncementService)
{
    /* ********************************** Общие операции *********************************** */

    /// <summary>
    /// Метод создает объявление
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию</param>
    /// <param name="createAnnouncement">Объект со сведениями о создаваемом объявлении</param>
    /// <returns>Созданное объявление</returns>
    public Announcement Create(Guid requesterId, CreateAnnouncement createAnnouncement) => 
        generalOperationsService.Create(requesterId, createAnnouncement);

    /// <summary>
    /// Метод возвращает объявление со всеми связанными сущностями
    /// </summary>
    /// <param name="requesterId"></param>
    /// <param name="announcementId"></param>
    /// <returns></returns>
    public Announcement GetDetails(Guid requesterId, Guid announcementId) => 
        generalOperationsService.GetDetails(requesterId, announcementId);

    /// <summary>
    /// Метод редактирует объявление
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию</param>
    /// <param name="editAnnouncement">Объект с измененными свойствами объявления</param>
    public void Edit(Guid requesterId, EditAnnouncement editAnnouncement) => 
        generalOperationsService.Edit(requesterId, editAnnouncement);

    /// <summary>
    /// Метод удаляем заданное объявление
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию</param>
    /// <param name="announcementId">Id заданного объявления</param>
    public void Delete(Guid requesterId, Guid announcementId) => 
        generalOperationsService.Delete(requesterId, announcementId);

    /// <summary>
    /// Метод немедленно публикует заданное объявление
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию</param>
    /// <param name="announcementId">Id заданного объявления</param>
    /// <param name="publishedAt">Время публикации объявления</param>
    public void Publish(Guid requesterId, Guid announcementId, DateTime publishedAt) => 
        generalOperationsService.PublishManually(requesterId, announcementId, publishedAt);



    /* ***************************** Опубликованные объявления ***************************** */
    
    
    
    /// <summary>
    /// Метод возвращает список опубликованных для пользователя объявлений
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <returns>Список с краткой информацией об объявлениях</returns>
    public IEnumerable<AnnouncementSummary> GetPublishedAnnouncements(Guid requesterId) => 
        publishedAnnouncementService.GetListForUser(requesterId);

    /// <summary>
    /// Метод скрывает указанное объявление
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <param name="announcementId">Id объявления, которое требуется скрыть</param>
    /// <param name="hiddenAt">Момент сокрытия объявления</param>
    public void Hide(Guid requesterId, Guid announcementId, DateTime hiddenAt) => 
        publishedAnnouncementService.HideManually(requesterId, announcementId, hiddenAt);



    /* ******************************** Скрытые объявления ********************************* */



    /// <summary>
    /// Метод возвращает список скрытых объявлений для указанного пользователя
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <returns>Список с краткой информацией об объявлениях</returns>
    public IEnumerable<AnnouncementSummary> GetHiddenAnnouncements(Guid requesterId) => 
        hiddenAnnouncementService.GetListForUser(requesterId);

    /// <summary>
    /// Метод восстанавливает скрытое объявление
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <param name="announcementId">Id восстанавливаемого объявления</param>
    /// <param name="restoredAt">Время восстановления объявления</param>
    public void Restore(Guid requesterId, Guid announcementId, DateTime restoredAt) => 
        hiddenAnnouncementService.Restore(requesterId, announcementId, restoredAt);



    /* ******************************* Отложенные объявления ******************************* */



    /// <summary>
    /// Метод возвращает список объявлений, ожидающих отложенной автоматической публикации,
    /// для заданного пользователя
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <returns>Список с краткой информацией об объявлениях</returns>
    public IEnumerable<AnnouncementSummary> GetDelayedPublicationAnnouncements(Guid requesterId) => 
        delayedPublicationAnnouncementService.GetDelayedPublicationAnnouncementListForUser(requesterId);

    /// <summary>
    /// Метод возвращает список объявлений, ожидающих отложенного автоматического сокрытия,
    /// для заданного пользователя
    /// </summary>
    /// <param name="requesterId">Id запросившего операцию пользователя</param>
    /// <returns>Список с краткой информацией об объявлениях</returns>
    public IEnumerable<AnnouncementSummary> GetDelayedHidingAnnouncementsForUser(Guid requesterId) => 
        delayedHidingAnnouncementService.GetDelayedHiddenAnnouncementListForUser(requesterId);
}