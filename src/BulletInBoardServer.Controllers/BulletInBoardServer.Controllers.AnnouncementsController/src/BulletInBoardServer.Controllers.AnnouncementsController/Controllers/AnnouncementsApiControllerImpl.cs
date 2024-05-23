using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BulletInBoardServer.Controllers.AnnouncementsController.Models;
using BulletInBoardServer.Controllers.Core.Logging;
using BulletInBoardServer.Controllers.Core.Responding;
using BulletInBoardServer.Domain.Models.Announcements.Exceptions;
using BulletInBoardServer.Services.Services.Announcements;
using BulletInBoardServer.Services.Services.Announcements.Exceptions;
using BulletInBoardServer.Services.Services.Announcements.Models;
using BulletInBoardServer.Services.Services.Attachments.Exceptions;
using BulletInBoardServer.Services.Services.Audience.Exceptions;
using BulletInBoardServer.Services.Services.Common.Exceptions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Serilog;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace BulletInBoardServer.Controllers.AnnouncementsController.Controllers;

/// <summary>
/// Контроллер объявлений
/// </summary>
public class AnnouncementsApiControllerImpl : AnnouncementsApiController
{
    private readonly AnnouncementService _announcementService;
    

    private readonly ILogger _logger = Log.ForContext<AnnouncementService>();
    private readonly LoggingHelper _loggingHelper;



    /// <summary>
    /// Контроллер объявлений
    /// </summary>
    public AnnouncementsApiControllerImpl(AnnouncementService announcementService)
    {
        _announcementService = announcementService;
        _loggingHelper = new LoggingHelper(_logger);
    }



    /// <summary>
    /// Создать объявление
    /// </summary>
    /// <param name="dto"></param>
    /// <response code="201">Created</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="409">Conflict</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult CreateAnnouncement([FromBody] CreateAnnouncementDto dto)
    {
        /*
         * 201 +
         * 400:
         *   audienceNullOrEmpty +
         *   contentNullOrEmpty +
         * 403:
         *     announcementCreationForbidden
         * 404:
         *   attachmentsDoNotExist +
         *   pieceOfAudienceDoesNotExist +
         * 409:
         *   delayedPublishingMomentIsInPast +
         *   delayedHidingMomentIsInPast +
         *   delayedPublishingMomentAfterDelayedHidingMoment +
         * 500 +
         */

        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // debug

        var createAnnouncement = dto.Adapt<CreateAnnouncement>();
        try
        {
            var announcement = _announcementService.Create(requesterId, createAnnouncement);

            _logger.Information(
                "Пользователь {UserId} создал объявление {AnnouncementId}", 
                requesterId,
                announcement.Id);

            return Created("/api/announcements/get-details", announcement.Id);
        }
        catch (AnnouncementAudienceNullOrEmptyException err)
        {
            _loggingHelper.LogWarning(400, "Создание объявления",
                nameof(CreateAnnouncementResponses.AudienceNullOrEmpty),
                requesterId, err.Message);
            return BadRequest(
                ResponseConstructor.ConstructResponseWithOnlyCode(CreateAnnouncementResponses.AudienceNullOrEmpty));
        }
        catch (AnnouncementContentNullOrEmptyException err)
        {
            _loggingHelper.LogWarning(400, "Создание объявления",
                nameof(CreateAnnouncementResponses.ContentNullOrEmpty),
                requesterId, err.Message);
            return BadRequest(
                ResponseConstructor.ConstructResponseWithOnlyCode(CreateAnnouncementResponses.ContentNullOrEmpty));
        }
        catch (AttachmentDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Создание объявления",
                nameof(CreateAnnouncementResponses.AttachmentsDoNotExist),
                requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(CreateAnnouncementResponses
                    .AttachmentsDoNotExist));
        }
        catch (PieceOfAudienceDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Создание объявления",
                nameof(CreateAnnouncementResponses.PieceOfAudienceDoesNotExist),
                requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(CreateAnnouncementResponses
                    .PieceOfAudienceDoesNotExist));
        }
        catch (DelayedPublishingMomentComesInPastException err)
        {
            _loggingHelper.LogWarning(409, "Создание объявления",
                nameof(CreateAnnouncementResponses.DelayedPublishingMomentIsInPast), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(CreateAnnouncementResponses
                    .DelayedPublishingMomentIsInPast));
        }
        catch (DelayedHidingMomentComesInPastException err)
        {
            _loggingHelper.LogWarning(409, "Создание объявления",
                nameof(CreateAnnouncementResponses.DelayedHidingMomentIsInPast),
                requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(CreateAnnouncementResponses
                    .DelayedHidingMomentIsInPast));
        }
        catch (DelayedPublishingAfterDelayedHidingException err)
        {
            _loggingHelper.LogWarning(409, "Создание объявления",
                nameof(CreateAnnouncementResponses.DelayedPublishingMomentAfterDelayedHidingMoment), requesterId,
                err.Message);
            return Conflict(ResponseConstructor.ConstructResponseWithOnlyCode(CreateAnnouncementResponses
                .DelayedPublishingMomentAfterDelayedHidingMoment));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Создание объявления", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Удалить объявление
    /// </summary>
    /// <param name="announcementId"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult DeleteAnnouncement([FromBody] Guid announcementId)
    {
        /*
         * 200 +
         * 403:
         *   announcementDeletionForbidden +
         * 409:
         *   announcementDoesNotExist +
         * 500 +
         */

        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // debug

        try
        {
            _announcementService.Delete(requesterId, announcementId);

            _logger.Information("Пользователь {RequesterId} удалил объявление {AnnouncementId}",
                requesterId, announcementId);

            return Ok();
        }
        catch (OperationNotAllowedException err)
        {
            _loggingHelper.LogWarning(403, "Удаление объявления",
                nameof(DeleteAnnouncementResponses.AnnouncementDeletionForbidden), requesterId, err.Message);
            return StatusCode(403,
                ResponseConstructor.ConstructResponseWithOnlyCode(DeleteAnnouncementResponses
                    .AnnouncementDeletionForbidden));
        }
        catch (AnnouncementDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Удаление объявления",
                nameof(DeleteAnnouncementResponses.AnnouncementDoesNotExist),
                requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(DeleteAnnouncementResponses
                    .AnnouncementDoesNotExist));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Удаление объявления", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Получить подробности о выбранном объявлении
    /// </summary>
    /// <param name="announcementId"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult GetAnnouncementDetails([FromRoute (Name = "id")][Required] Guid announcementId)
    {
        /*
         * 200 +
         * 403:
         *   detailsAccessForbidden +
         * 409:
         *   announcementDoesNotExist +
         * 500 +
         */

        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // debug

        try
        {
            var announcement = _announcementService.GetDetails(requesterId, announcementId);

            _logger.Information(
                "Пользователь {RequesterId} получил подробности объявления {AnnouncementId}",
                requesterId, announcementId);

            var dto = announcement.Adapt<AnnouncementDetailsDto>();
            return Ok(dto);
        }
        catch (OperationNotAllowedException err)
        {
            _loggingHelper.LogWarning(403, "Получение деталей объявления",
                nameof(GetAnnouncementDetailsResponses.DetailsAccessForbidden), requesterId, err.Message);
            return StatusCode(403,
                ResponseConstructor.ConstructResponseWithOnlyCode(GetAnnouncementDetailsResponses
                    .DetailsAccessForbidden));
        }
        catch (AnnouncementDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Получение деталей объявления",
                nameof(GetAnnouncementDetailsResponses.AnnouncementDoesNotExist), requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(GetAnnouncementDetailsResponses
                    .AnnouncementDoesNotExist));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Получение деталей объявления", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Получить данные для редактирования объявления
    /// </summary>
    /// <param name="id">Идентификатор объявления</param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult GetAnnouncementUpdateContent([FromRoute (Name = "id")][Required] Guid id)
    {
        /*
         * 200 + 
         * 403:
         *   getAnnouncementUpdateContentForbidden +
         * 409:
         *   announcementDoesNotExist +
         * 500 +
         */

        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // debug

        try
        {
            var content = _announcementService.GetContentForAnnouncementUpdating(requesterId, id);

            _logger.Information(
                "Пользователь {RequesterId} получил данные для редактирования объявления {AnnouncementId}",
                requesterId, id);

            return Ok(content.Adapt<ContentForAnnouncementUpdatingDto>());
        }
        catch (OperationNotAllowedException err)
        {
            _loggingHelper.LogWarning(403, "Получение данных для редактирования объявления",
                nameof(GetAnnouncementUpdateContentResponses.GetAnnouncementUpdateContentForbidden), requesterId,
                err.Message);
            return StatusCode(403,
                ResponseConstructor.ConstructResponseWithOnlyCode(GetAnnouncementUpdateContentResponses
                    .GetAnnouncementUpdateContentForbidden));
        }
        catch (AnnouncementDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Получение данных для редактирования объявления",
                nameof(GetAnnouncementUpdateContentResponses.AnnouncementDoesNotExist), requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(GetAnnouncementUpdateContentResponses
                    .AnnouncementDoesNotExist));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Получение данных для редактирования объявления", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Получить список объявлений, ожидающих отложенное сокрытие
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult GetDelayedHiddenAnnouncementList()
    {
        /*
         * 200 +
         * 403:
         *     getDelayedHiddenAnnouncementListAccessForbidden
         * 500 +
         */

        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // debug

        try
        {
            var announcements = _announcementService.GetDelayedHidingAnnouncementsForUser(requesterId);

            _logger.Information("Пользователь {RequesterId} запросил список объявлений с отложенным сокрытием",
                requesterId);

            return Ok(announcements.Adapt<IEnumerable<AnnouncementSummaryDto>>());
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Получение списка объявлений с отложенным сокрытием", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Получить список объявлений, ожидающих отложенную публикацию
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult GetDelayedPublishingAnnouncementList()
    {
        /*
         * 200 +
         * 403:
         *     getDelayedPublishingAnnouncementListResponses
         * 500 +
         */

        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // debug

        try
        {
            var announcements = _announcementService.GetDelayedPublicationAnnouncements(requesterId);

            _logger.Information("Пользователь {RequesterId} запросил список объявлений с отложенной публикацией",
                requesterId);

            return Ok(announcements.Adapt<IEnumerable<AnnouncementSummaryDto>>());
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Получение списка объявлений с отложенной публикацией", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Получить список скрытых объявлений
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult GetHiddenAnnouncementList()
    {
        /*
         * 200 +
         * 403:
         *     hiddenAnnouncementsListAccessForbidden
         * 500 +
         */

        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // debug

        try
        {
            var announcements = _announcementService.GetHiddenAnnouncements(requesterId);

            _logger.Information("Пользователь {RequesterId} запросил список скрытых объявлений", requesterId);

            return Ok(announcements.Adapt<IEnumerable<AnnouncementSummaryDto>>());
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Получение списка скрытых объявлений", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Получить список опубликованных объявлений
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult GetPostedAnnouncementList()
    {
        /*
         * 200 +
         * 403:
         *     postedAnnouncementsListAccessForbidden
         * 500 +
         */

        // var requesterId = Guid.Empty; // todo id пользователя;
        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // debug

        try
        {
            var announcements = _announcementService.GetPublishedAnnouncements(requesterId);

            _logger.Information("Пользователь {RequesterId} запросил список опубликованных объявлений",
                requesterId);

            return Ok(announcements.Adapt<IEnumerable<AnnouncementSummaryDto>>());
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Получение списка опубликованных объявлений", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Скрыть опубликованное объявление
    /// </summary>
    /// <param name="announcementId"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="409">Conflict</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult HidePostedAnnouncement([FromBody] Guid announcementId)
    {
        /*
         * 200 +
         * 403:
         *   announcementHidingForbidden +
         * 404:
         *   announcementDoesNotExist +
         * 409:
         *   announcementAlreadyHidden +
         *   announcementNotYetPublished +
         * 500 +
         */

        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // debug

        try
        {
            _announcementService.Hide(requesterId, announcementId, DateTime.Now);

            _logger.Information("Пользователь {RequesterId} скрыл объявление {AnnouncementId}",
                requesterId, announcementId);

            return Ok();
        }
        catch (OperationNotAllowedException err)
        {
            _loggingHelper.LogWarning(403, "Сокрытие объявления",
                nameof(HidePostedAnnouncementResponses.AnnouncementHidingForbidden), requesterId, err.Message);
            return StatusCode(403,
                ResponseConstructor.ConstructResponseWithOnlyCode(HidePostedAnnouncementResponses
                    .AnnouncementHidingForbidden));
        }
        catch (AnnouncementDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Сокрытие объявления",
                nameof(HidePostedAnnouncementResponses.AnnouncementDoesNotExist), requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(HidePostedAnnouncementResponses
                    .AnnouncementDoesNotExist));
        }
        catch (AnnouncementAlreadyHiddenException err)
        {
            _loggingHelper.LogWarning(409, "Сокрытие объявления",
                nameof(HidePostedAnnouncementResponses.AnnouncementAlreadyHidden), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(HidePostedAnnouncementResponses
                    .AnnouncementAlreadyHidden));
        }
        catch (AnnouncementNotYetPublishedException err)
        {
            _loggingHelper.LogWarning(409, "Сокрытие объявления",
                nameof(HidePostedAnnouncementResponses.AnnouncementNotYetPublished), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(HidePostedAnnouncementResponses
                    .AnnouncementNotYetPublished));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Сокрытие объявления", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Сразу опубликовать отложенное объявление
    /// </summary>
    /// <param name="announcementId"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult PublishImmediatelyDelayedPublishingAnnouncement([FromBody] Guid announcementId)
    {
        /*
         * 200 +
         * 403:
         *   immediatePublishingForbidden +
         * 404:
         *   announcementDoesNotExist +
         * 500 +
         */

        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // debug

        try
        {
            _announcementService.Publish(requesterId, announcementId, DateTime.Now);

            _logger.Information(
                "Пользователь {RequesterId} немедленно опубликовал объявление {AnnouncementId}, ожидающее отложенную публикацию",
                requesterId, announcementId);

            return Ok();
        }
        catch (OperationNotAllowedException err)
        {
            _loggingHelper.LogWarning(403, "Немедленная публикация объявления, ожидающего отложенной публикации",
                nameof(PublishImmediatelyDelayedAnnouncementResponses.ImmediatePublishingForbidden), requesterId,
                err.Message);
            return StatusCode(403,
                ResponseConstructor.ConstructResponseWithOnlyCode(PublishImmediatelyDelayedAnnouncementResponses
                    .ImmediatePublishingForbidden));
        }
        catch (AnnouncementDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Немедленная публикация объявления, ожидающего отложенной публикации",
                nameof(PublishImmediatelyDelayedAnnouncementResponses.AnnouncementDoesNotExist), requesterId,
                err.Message);
            return NotFound(ResponseConstructor.ConstructResponseWithOnlyCode(
                PublishImmediatelyDelayedAnnouncementResponses
                    .AnnouncementDoesNotExist));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500,
                "Немедленная публикация объявления, ожидающего отложенной публикации", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Восстановить скрытое объявление
    /// </summary>
    /// <param name="announcementId"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="409">Conflict</response>
    /// <response code="500">Internal Server Error</response>>
    public override IActionResult RestoreHiddenAnnouncement([FromBody] Guid announcementId)
    {
        /*
         * 200 +
         * 403:
         *   restoreForbidden +
         * 404:
         *   announcementDoesNotExist +
         * 409:
         *   announcementNotHidden +
         * 500 +
         */

        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // debug

        try
        {
            _announcementService.Restore(requesterId, announcementId, DateTime.Now);

            _logger.Information("Пользователь {RequesterId} восстановил скрытое объявление {AnnouncementId}",
                requesterId, announcementId);

            return Ok();
        }
        catch (OperationNotAllowedException err)
        {
            _loggingHelper.LogWarning(403, "Восстановление скрытого объявления",
                nameof(RestoreHiddenAnnouncementResponses.RestoreForbidden), requesterId, err.Message);
            return StatusCode(403,
                ResponseConstructor.ConstructResponseWithOnlyCode(
                    RestoreHiddenAnnouncementResponses.RestoreForbidden));
        }
        catch (AnnouncementDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Восстановление скрытого объявления",
                nameof(RestoreHiddenAnnouncementResponses.AnnouncementDoesNotExist), requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(RestoreHiddenAnnouncementResponses
                    .AnnouncementDoesNotExist));
        }
        catch (AnnouncementNotHiddenException err)
        {
            _loggingHelper.LogWarning(404, "Восстановление скрытого объявления",
                nameof(RestoreHiddenAnnouncementResponses.AnnouncementNotHidden), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(RestoreHiddenAnnouncementResponses
                    .AnnouncementNotHidden));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Восстановление скрытого объявления", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Редактировать объявление
    /// </summary>
    /// <param name="updateAnnouncementDto"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="409">Conflict</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult UpdateAnnouncement([FromBody] UpdateAnnouncementDto updateAnnouncementDto)
    {
        /*
         * 200 +
         * 400:
         *   contentEmpty +
         *   audienceEmpty +
         * 403:
         *   announcementEditingForbidden +
         * 404:
         *   announcementDoesNotExist +
         *   attachmentsDoNotExist +
         *   pieceOfAudienceDoesNotExist +
         * 409:
         *   delayedPublishingMomentIsInPast +
         *   delayedHidingMomentIsInPast +
         *   autoHidingAnAlreadyHiddenAnnouncement +
         *   autoPublishingPublishedAndNonHiddenAnnouncement +
         *   cannotDetachSurvey +
         * 500 +
         */

        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // debug

        try
        {
            var editAnnouncement = updateAnnouncementDto.Adapt<EditAnnouncement>();
            _announcementService.Edit(requesterId, editAnnouncement);

            _logger.Information("Пользователь {RequesterId} отредактировал объявление {AnnouncementId}",
                requesterId, updateAnnouncementDto.Id);

            return Ok();
        }
        catch (AnnouncementContentEmptyException err)
        {
            _loggingHelper.LogWarning(400, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.ContentEmpty),
                requesterId, err.Message);
            return BadRequest(
                ResponseConstructor.ConstructResponseWithOnlyCode(UpdateAnnouncementResponses.ContentEmpty));
        }
        catch (AnnouncementAudienceEmptyException err)
        {
            _loggingHelper.LogWarning(400, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.AudienceEmpty),
                requesterId, err.Message);
            return BadRequest(
                ResponseConstructor.ConstructResponseWithOnlyCode(UpdateAnnouncementResponses.AudienceEmpty));
        }
        catch (OperationNotAllowedException err)
        {
            _loggingHelper.LogWarning(403, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.AnnouncementEditingForbidden), requesterId, err.Message);
            return StatusCode(403,
                ResponseConstructor.ConstructResponseWithOnlyCode(UpdateAnnouncementResponses
                    .AnnouncementEditingForbidden));
        }
        catch (AnnouncementDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.AnnouncementDoesNotExist), requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(UpdateAnnouncementResponses
                    .AnnouncementDoesNotExist));
        }
        catch (AttachmentDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.AttachmentsDoNotExist),
                requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(UpdateAnnouncementResponses
                    .AttachmentsDoNotExist));
        }
        catch (PieceOfAudienceDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.PieceOfAudienceDoesNotExist),
                requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(UpdateAnnouncementResponses
                    .PieceOfAudienceDoesNotExist));
        }
        catch (DelayedPublishingMomentComesInPastException err)
        {
            _loggingHelper.LogWarning(409, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.DelayedPublishingMomentIsInPast), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(UpdateAnnouncementResponses
                    .DelayedPublishingMomentIsInPast));
        }
        catch (DelayedHidingMomentComesInPastException err)
        {
            _loggingHelper.LogWarning(409, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.DelayedHidingMomentIsInPast), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(UpdateAnnouncementResponses
                    .DelayedHidingMomentIsInPast));
        }
        catch (AutoPublishAnAlreadyPublishedAnnouncementException err)
        {
            _loggingHelper.LogWarning(409, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.AutoPublishingPublishedAndNonHiddenAnnouncement), requesterId,
                err.Message);
            return Conflict(ResponseConstructor.ConstructResponseWithOnlyCode(UpdateAnnouncementResponses
                .AutoPublishingPublishedAndNonHiddenAnnouncement));
        }
        catch (AutoHidingAnAlreadyHiddenAnnouncementException err)
        {
            _loggingHelper.LogWarning(409, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.AutoHidingAnAlreadyHiddenAnnouncement), requesterId,
                err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(UpdateAnnouncementResponses
                    .AutoHidingAnAlreadyHiddenAnnouncement));
        }
        catch (CannotDetachSurveyException err)
        {
            _loggingHelper.LogWarning(409, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.CannotDetachSurvey), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(UpdateAnnouncementResponses.CannotDetachSurvey));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Редактирование объявления", requesterId);
            return Problem();
        }
    }
}