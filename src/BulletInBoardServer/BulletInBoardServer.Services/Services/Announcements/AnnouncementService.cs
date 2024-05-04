using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Domain.Models.Announcements.Exceptions;
using BulletInBoardServer.Services.Services.Announcements.Exceptions;
using BulletInBoardServer.Services.Services.Announcements.Models;
using BulletInBoardServer.Services.Services.Announcements.ServiceCore;
using BulletInBoardServer.Services.Services.Common.Exceptions;
using BulletInBoardServer.Services.Services.UserGroups;

namespace BulletInBoardServer.Services.Services.Announcements;

/// <summary>
/// Класс, агрегирующий операции непосредственно над объявлениями
/// </summary>
public class AnnouncementService(
    GeneralOperationsService generalOperationsService,
    PublishedAnnouncementService publishedAnnouncementService,
    HiddenAnnouncementService hiddenAnnouncementService,
    DelayedPublicationAnnouncementService delayedPublicationAnnouncementService,
    DelayedHidingAnnouncementService delayedHidingAnnouncementService,
    UserGroupService userGroupService)
{
    /* ********************************** Общие операции *********************************** */

    /// <summary>
    /// Метод создает объявление
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию</param>
    /// <param name="createAnnouncement">Объект со сведениями о создаваемом объявлении</param>
    /// <returns>Созданное объявление</returns>
    /// <exception cref="AnnouncementContentNullOrEmptyException">Текстовое содержимое объявления null, пустой или состоит только из пробельных символов</exception>
    /// <exception cref="AnnouncementAudienceNullOrEmptyException">Аудитория объявления null или пуста</exception>
    /// <exception cref="InvalidOperationException">Момент отложенной публикации или сокрытия не были перенесены в создаваемое объявление</exception>
    public Announcement Create(Guid requesterId, CreateAnnouncement createAnnouncement) =>
        generalOperationsService.Create(requesterId, createAnnouncement);

    /// <summary>
    /// Метод возвращает объявление со всеми связанными сущностями
    /// </summary>
    /// <param name="requesterId"></param>
    /// <param name="announcementId"></param>
    /// <returns>Объявления с загруженными связанными сущностями</returns>
    /// <exception cref="AnnouncementDoesNotExistException">Объявление отсутствует в БД</exception>
    /// <exception cref="OperationNotAllowedException">Пользователь не имеет права  выполнения операции</exception>
    public Announcement GetDetails(Guid requesterId, Guid announcementId) =>
        generalOperationsService.GetDetails(requesterId, announcementId);

    /// <summary>
    /// Метод возвращает данные для редактирования указанного объявления
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию</param>
    /// <param name="announcementId">Id объявления, данные для редактирования которого овзвращаются</param>
    /// <exception cref="AnnouncementDoesNotExistException">Объявление отсутствует в БД</exception>
    /// <exception cref="OperationNotAllowedException">Пользователь не имеет права  выполнения операции</exception>
    /// <returns>Данные для редактирования объявления</returns>
    public UpdateAnnouncementContent GetContentForAnnouncementUpdating(Guid requesterId, Guid announcementId)
    {
        var details = GetDetails(requesterId, announcementId);
        var audienceHierarchy = userGroupService.GetUsergroupHierarchy(requesterId);

        return new UpdateAnnouncementContent(details, audienceHierarchy);
    }

    /// <summary>
    /// Метод редактирует объявление
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию</param>
    /// <param name="editAnnouncement">Объект с измененными свойствами объявления</param>
    /// <exception cref="OperationNotAllowedException">Пользователь не имеет права выполнить операцию</exception>
    /// <exception cref="AnnouncementContentEmptyException">Нельзя установить текстовое содержимое, которое является null, пустым или состоит только из пробельных символов</exception>
    /// <exception cref="AnnouncementAudienceEmptyException">Нельзя установить пустую аудиторию объявления</exception>
    /// <exception cref="CannotDetachSurveyException">От объявления нельзя открепить опрос</exception>
    public void Edit(Guid requesterId, EditAnnouncement editAnnouncement) =>
        generalOperationsService.Edit(requesterId, editAnnouncement);

    /// <summary>
    /// Метод удаляем заданное объявление
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию</param>
    /// <param name="announcementId">Id заданного объявления</param>
    /// <exception cref="AnnouncementDoesNotExistException">Объявление отсутствует в БД</exception>
    /// <exception cref="OperationNotAllowedException">Пользователь не имеет права  выполнения операции</exception>
    public void Delete(Guid requesterId, Guid announcementId) =>
        generalOperationsService.Delete(requesterId, announcementId);

    /// <summary>
    /// Метод немедленно публикует заданное объявление
    /// </summary>
    /// <param name="requesterId">Id пользователя, запросившего операцию</param>
    /// <param name="announcementId">Id заданного объявления</param>
    /// <param name="publishedAt">Время публикации объявления</param>
    /// <exception cref="AnnouncementDoesNotExistException">Объявление отсутствует в БД</exception>
    /// <exception cref="OperationNotAllowedException">Пользователь не имеет права  выполнения операции</exception>
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
    /// <exception cref="AnnouncementDoesNotExistException">Объявление отсутствует в БД</exception>
    /// <exception cref="OperationNotAllowedException">Пользователь не имеет права  выполнения операции</exception>
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
    /// <exception cref="AnnouncementDoesNotExistException">Объявление отсутствует в БД</exception>
    /// <exception cref="OperationNotAllowedException">Пользователь не имеет права  выполнения операции</exception>
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