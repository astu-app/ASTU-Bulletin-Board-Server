using System;
using System.Collections.Generic;
using BulletInBoardServer.Controllers.AnnouncementsController.Models;
using BulletInBoardServer.Domain.Models.Announcements.Exceptions;
using BulletInBoardServer.Services.Services.AnnouncementCategories.Exceptions;
using BulletInBoardServer.Services.Services.Announcements;
using BulletInBoardServer.Services.Services.Announcements.Exceptions;
using BulletInBoardServer.Services.Services.Announcements.Infrastructure;
using BulletInBoardServer.Services.Services.Attachments.Exceptions;
using BulletInBoardServer.Services.Services.Audience.Exceptions;
using BulletInBoardServer.Services.Services.Exceptions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Serilog;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

namespace BulletInBoardServer.Controllers.AnnouncementsController.Controllers;

/// <inheritdoc />
public class AnnouncementsApiControllerImpl(AnnouncementService service)
    : AnnouncementsApiController
{
    private readonly ILogger _logger = Log.ForContext<AnnouncementService>();



    /// <inheritdoc />
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
         *   announcementCategoriesDoNotExist +
         *   attachmentsDoNotExist +
         *   pieceOfAudienceDoesNotExist +
         * 409:
         *   delayedPublishingMomentIsInPast +
         *   delayedHidingMomentIsInPast +
         *   delayedPublishingMomentAfterDelayedHidingMoment +
         * 500 +
         */

        var requesterId = Guid.Empty; // todo id пользователя

        var validationResult = ValidateModel(dto);
        if (validationResult is not null)
            return validationResult;

        var createAnnouncement = dto.Adapt<CreateAnnouncement>();
        try
        {
            var announcement = service.Create(requesterId, createAnnouncement);

            _logger.Information(
                "Пользователь {UserId} создал объявление {AnnouncementId}",
                requesterId,
                announcement.Id);

            return Created(new Uri("/api/announcements/get-details"), announcement.Id);
        }
        catch (AnnouncementAudienceNullOrEmptyException err)
        {
            LogWarning(400, "Создание объявления", nameof(CreateAnnouncementResponses.AudienceNullOrEmpty),
                requesterId, err.Message);
            return BadRequest(ConstructAnswerWithOnlyCode(CreateAnnouncementResponses.AudienceNullOrEmpty));
        }
        catch (AnnouncementContentNullOrEmptyException err)
        {
            LogWarning(400, "Создание объявления", nameof(CreateAnnouncementResponses.ContentNullOrEmpty),
                requesterId, err.Message);
            return BadRequest(ConstructAnswerWithOnlyCode(CreateAnnouncementResponses.ContentNullOrEmpty));
        }
        catch (AttachmentDoesNotExist err)
        {
            LogWarning(404, "Создание объявления", nameof(CreateAnnouncementResponses.AttachmentsDoNotExist),
                requesterId, err.Message);
            return NotFound(ConstructAnswerWithOnlyCode(CreateAnnouncementResponses.AttachmentsDoNotExist));
        }
        catch (PieceOfAudienceDoesNotExist err)
        {
            LogWarning(404, "Создание объявления", nameof(CreateAnnouncementResponses.PieceOfAudienceDoesNotExist),
                requesterId, err.Message);
            return NotFound(ConstructAnswerWithOnlyCode(CreateAnnouncementResponses.PieceOfAudienceDoesNotExist));
        }
        catch (AnnouncementCategoriesDoNotExist err)
        {
            LogWarning(404, "Создание объявления",
                nameof(CreateAnnouncementResponses.AnnouncementCategoriesDoNotExist), requesterId, err.Message);
            return NotFound(
                ConstructAnswerWithOnlyCode(CreateAnnouncementResponses.AnnouncementCategoriesDoNotExist));
        }
        catch (DelayedPublishingMomentComesInPastException err)
        {
            LogWarning(409, "Создание объявления",
                nameof(CreateAnnouncementResponses.DelayedPublishingMomentIsInPast), requesterId, err.Message);
            return Conflict(
                ConstructAnswerWithOnlyCode(CreateAnnouncementResponses.DelayedPublishingMomentIsInPast));
        }
        catch (DelayedHidingMomentComesInPastException err)
        {
            LogWarning(409, "Создание объявления", nameof(CreateAnnouncementResponses.DelayedHidingMomentIsInPast),
                requesterId, err.Message);
            return Conflict(ConstructAnswerWithOnlyCode(CreateAnnouncementResponses.DelayedHidingMomentIsInPast));
        }
        catch (DelayedPublishingAfterDelayedHidingException err)
        {
            LogWarning(409, "Создание объявления",
                nameof(CreateAnnouncementResponses.DelayedPublishingMomentAfterDelayedHidingMoment), requesterId,
                err.Message);
            return Conflict(ConstructAnswerWithOnlyCode(CreateAnnouncementResponses
                .DelayedPublishingMomentAfterDelayedHidingMoment));
        }
        catch (Exception err)
        {
            LogError(err, 500, "Создание объявления", requesterId);
            return Problem();
        }


        IActionResult? ValidateModel(CreateAnnouncementDto dto_)
        {
            if (dto_.UserIds is null || dto_.UserIds.Count == 0)
                return BadRequest(ConstructAnswerWithOnlyCode(CreateAnnouncementResponses.AudienceNullOrEmpty));

            return null;
        }
    }

    /// <inheritdoc />
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

        var requesterId = Guid.Empty; // todo id пользователя

        try
        {
            service.Delete(requesterId, announcementId);

            _logger.Information("Пользователь {RequesterId} удалил объявление {AnnouncementId}",
                requesterId, announcementId);

            return Ok();
        }
        catch (OperationNotAllowedException err)
        {
            LogWarning(403, "Удаление объявления",
                nameof(DeleteAnnouncementResponses.AnnouncementDeletionForbidden), requesterId, err.Message);
            return StatusCode(403,
                ConstructAnswerWithOnlyCode(DeleteAnnouncementResponses.AnnouncementDeletionForbidden));
        }
        catch (AnnouncementDoesNotExist err)
        {
            LogWarning(404, "Удаление объявления", nameof(DeleteAnnouncementResponses.AnnouncementDoesNotExist),
                requesterId, err.Message);
            return NotFound(ConstructAnswerWithOnlyCode(DeleteAnnouncementResponses.AnnouncementDoesNotExist));
        }
        catch (Exception err)
        {
            LogError(err, 500, "Удаление объявления", requesterId);
            return Problem();
        }
    }

    /// <inheritdoc />
    public override IActionResult GetAnnouncementDetails([FromBody] Guid announcementId)
    {
        /*
         * 200 +
         * 403:
         *   detailsAccessForbidden +
         * 409:
         *   announcementDoesNotExist +
         * 500 +
         */

        // var requesterId = Guid.Empty; // todo id пользователя
        var requesterId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"); // debug

        try
        {
            var announcement = service.GetDetails(requesterId, announcementId);

            _logger.Information(
                "Пользователь {RequesterId} получил подробности объявления {AnnouncementId}",
                requesterId, announcementId);

            return Ok(announcement.Adapt<AnnouncementDetailsDto>());
        }
        catch (OperationNotAllowedException err)
        {
            LogWarning(403, "Получение деталей объявления",
                nameof(GetAnnouncementDetailsResponses.DetailsAccessForbidden), requesterId, err.Message);
            return StatusCode(403,
                ConstructAnswerWithOnlyCode(GetAnnouncementDetailsResponses.DetailsAccessForbidden));
        }
        catch (AnnouncementDoesNotExist err)
        {
            LogWarning(404, "Получение деталей объявления",
                nameof(GetAnnouncementDetailsResponses.AnnouncementDoesNotExist), requesterId, err.Message);
            return NotFound(ConstructAnswerWithOnlyCode(GetAnnouncementDetailsResponses.AnnouncementDoesNotExist));
        }
        catch (Exception err)
        {
            LogError(err, 500, "Получение деталей объявления", requesterId);
            return Problem();
        }
    }

    /// <inheritdoc />
    public override IActionResult GetDelayedHiddenAnnouncementList()
    {
        /*
         * 200 +
         * 403:
         *     getDelayedHiddenAnnouncementListAccessForbidden
         * 500 +
         */

        var requesterId = Guid.Empty; // todo id пользователя;

        try
        {
            var announcements = service.GetDelayedHidingAnnouncementsForUser(requesterId);

            _logger.Information("Пользователь {RequesterId} запросил список объявлений с отложенным сокрытием",
                requesterId);

            return Ok(announcements.Adapt<IEnumerable<AnnouncementSummaryDto>>());
        }
        catch (Exception err)
        {
            LogError(err, 500, "Получение списка объявлений с отложенным сокрытием", requesterId);
            return Problem();
        }
    }

    /// <inheritdoc />
    public override IActionResult GetDelayedPublishingAnnouncementList()
    {
        /*
         * 200 +
         * 403:
         *     getDelayedPublishingAnnouncementListResponses
         * 500 +
         */

        var requesterId = Guid.Empty; // todo id пользователя;

        try
        {
            var announcements = service.GetDelayedPublicationAnnouncements(requesterId);

            _logger.Information("Пользователь {RequesterId} запросил список объявлений с отложенной публикацией",
                requesterId);

            return Ok(announcements.Adapt<IEnumerable<AnnouncementSummaryDto>>());
        }
        catch (Exception err)
        {
            LogError(err, 500, "Получение списка объявлений с отложенной публикацией", requesterId);
            return Problem();
        }
    }

    /// <inheritdoc />
    public override IActionResult GetHiddenAnnouncementList()
    {
        /*
         * 200 +
         * 403:
         *     hiddenAnnouncementsListAccessForbidden
         * 500 +
         */

        var requesterId = Guid.Empty; // todo id пользователя;

        try
        {
            var announcements = service.GetHiddenAnnouncements(requesterId);

            _logger.Information("Пользователь {RequesterId} запросил список скрытых объявлений", requesterId);

            return Ok(announcements.Adapt<IEnumerable<AnnouncementSummaryDto>>());
        }
        catch (Exception err)
        {
            LogError(err, 500, "Получение списка скрытых объявлений", requesterId);
            return Problem();
        }
    }

    /// <inheritdoc />
    public override IActionResult GetPostedAnnouncementList()
    {
        /*
         * 200 +
         * 403:
         *     postedAnnouncementsListAccessForbidden
         * 500 +
         */

        var requesterId = Guid.Empty; // todo id пользователя;

        try
        {
            var announcements = service.GetPublishedAnnouncements(requesterId);

            _logger.Information("Пользователь {RequesterId} запросил список опубликованных объявлений",
                requesterId);

            return Ok(announcements.Adapt<IEnumerable<AnnouncementSummaryDto>>());
        }
        catch (Exception err)
        {
            LogError(err, 500, "Получение списка опубликованных объявлений", requesterId);
            return Problem();
        }
    }

    /// <inheritdoc />
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

        var requesterId = Guid.Empty; // todo id пользователя

        try
        {
            service.Hide(requesterId, announcementId, DateTime.Now);

            _logger.Information("Пользователь {RequesterId} скрыл объявление {AnnouncementId}",
                requesterId, announcementId);

            return Ok();
        }
        catch (OperationNotAllowedException err)
        {
            LogWarning(403, "Сокрытие объявления",
                nameof(HidePostedAnnouncementResponses.AnnouncementHidingForbidden), requesterId, err.Message);
            return StatusCode(403,
                ConstructAnswerWithOnlyCode(HidePostedAnnouncementResponses.AnnouncementHidingForbidden));
        }
        catch (AnnouncementDoesNotExist err)
        {
            LogWarning(404, "Сокрытие объявления",
                nameof(HidePostedAnnouncementResponses.AnnouncementDoesNotExist), requesterId, err.Message);
            return NotFound(ConstructAnswerWithOnlyCode(HidePostedAnnouncementResponses.AnnouncementDoesNotExist));
        }
        catch (AnnouncementAlreadyHiddenException err)
        {
            LogWarning(409, "Сокрытие объявления",
                nameof(HidePostedAnnouncementResponses.AnnouncementAlreadyHidden), requesterId, err.Message);
            return Conflict(
                ConstructAnswerWithOnlyCode(HidePostedAnnouncementResponses.AnnouncementAlreadyHidden));
        }
        catch (AnnouncementNotYetPublishedException err)
        {
            LogWarning(409, "Сокрытие объявления",
                nameof(HidePostedAnnouncementResponses.AnnouncementNotYetPublished), requesterId, err.Message);
            return Conflict(
                ConstructAnswerWithOnlyCode(HidePostedAnnouncementResponses.AnnouncementNotYetPublished));
        }
        catch (Exception err)
        {
            LogError(err, 500, "Сокрытие объявления", requesterId);
            return Problem();
        }
    }

    /// <inheritdoc />
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

        var requesterId = Guid.Empty; // todo id пользователя

        try
        {
            service.Publish(requesterId, announcementId, DateTime.Now); // todo id пользователя

            _logger.Information(
                "Пользователь {RequesterId} немедленно опубликовал объявление {AnnouncementId}, ожидающее отложенную публикацию",
                requesterId, announcementId);

            return Ok();
        }
        catch (OperationNotAllowedException err)
        {
            LogWarning(403, "Немедленная публикация объявления, ожидающего отложенной публикации",
                nameof(PublishImmediatelyDelayedAnnouncementResponses.ImmediatePublishingForbidden), requesterId,
                err.Message);
            return StatusCode(403,
                ConstructAnswerWithOnlyCode(PublishImmediatelyDelayedAnnouncementResponses
                    .ImmediatePublishingForbidden));
        }
        catch (AnnouncementDoesNotExist err)
        {
            LogWarning(404, "Немедленная публикация объявления, ожидающего отложенной публикации",
                nameof(PublishImmediatelyDelayedAnnouncementResponses.AnnouncementDoesNotExist), requesterId,
                err.Message);
            return NotFound(ConstructAnswerWithOnlyCode(PublishImmediatelyDelayedAnnouncementResponses
                .AnnouncementDoesNotExist));
        }
        catch (Exception err)
        {
            LogError(err, 500, "Немедленная публикация объявления, ожидающего отложенной публикации", requesterId);
            return Problem();
        }
    }

    /// <inheritdoc />
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

        var requesterId = Guid.Empty; // todo id пользователя

        try
        {
            service.Restore(requesterId, announcementId, DateTime.Now);

            _logger.Information("Пользователь {RequesterId} восстановил скрытое объявление {AnnouncementId}",
                requesterId, announcementId);

            return Ok();
        }
        catch (OperationNotAllowedException err)
        {
            LogWarning(403, "Восстановление скрытого объявления",
                nameof(RestoreHiddenAnnouncementResponses.RestoreForbidden), requesterId, err.Message);
            return StatusCode(403,
                ConstructAnswerWithOnlyCode(RestoreHiddenAnnouncementResponses.RestoreForbidden));
        }
        catch (AnnouncementDoesNotExist err)
        {
            LogWarning(404, "Восстановление скрытого объявления",
                nameof(RestoreHiddenAnnouncementResponses.AnnouncementDoesNotExist), requesterId, err.Message);
            return NotFound(
                ConstructAnswerWithOnlyCode(RestoreHiddenAnnouncementResponses.AnnouncementDoesNotExist));
        }
        catch (AnnouncementNotHiddenException err)
        {
            LogWarning(404, "Восстановление скрытого объявления",
                nameof(RestoreHiddenAnnouncementResponses.AnnouncementNotHidden), requesterId, err.Message);
            return NotFound(ConstructAnswerWithOnlyCode(RestoreHiddenAnnouncementResponses.AnnouncementNotHidden));
        }
        catch (Exception err)
        {
            LogError(err, 500, "Восстановление скрытого объявления", requesterId);
            return Problem();
        }
    }

    /// <inheritdoc />
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
         *   announcementCategoriesDoNotExist +
         *   attachmentsDoNotExist +
         * 409:
         *   delayedPublishingMomentIsInPast +
         *   delayedHidingMomentIsInPast +
         *   autoHidingAnAlreadyHiddenAnnouncement +
         *   autoPublishingPublishedAndNonHiddenAnnouncement +
         *   cannotDetachSurvey +
         * 500 +
         */

        var requesterId = Guid.Empty; // todo id пользователя

        try
        {
            var editAnnouncement = updateAnnouncementDto.Adapt<EditAnnouncement>();
            service.Edit(requesterId, editAnnouncement);

            _logger.Information("Пользователь {RequesterId} отредактировал объявление {AnnouncementId}",
                requesterId, updateAnnouncementDto.Id);

            return Ok();
        }
        catch (AnnouncementContentEmptyException err)
        {
            LogWarning(400, "Редактирование объявления", nameof(UpdateAnnouncementResponses.ContentEmpty),
                requesterId, err.Message);
            return BadRequest(ConstructAnswerWithOnlyCode(UpdateAnnouncementResponses.ContentEmpty));
        }
        catch (AnnouncementAudienceEmptyException err)
        {
            LogWarning(400, "Редактирование объявления", nameof(UpdateAnnouncementResponses.AudienceEmpty),
                requesterId, err.Message);
            return BadRequest(ConstructAnswerWithOnlyCode(UpdateAnnouncementResponses.AudienceEmpty));
        }
        catch (OperationNotAllowedException err)
        {
            LogWarning(403, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.AnnouncementEditingForbidden), requesterId, err.Message);
            return StatusCode(403,
                ConstructAnswerWithOnlyCode(UpdateAnnouncementResponses.AnnouncementEditingForbidden));
        }
        catch (AnnouncementDoesNotExist err)
        {
            LogWarning(404, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.AnnouncementDoesNotExist), requesterId, err.Message);
            return NotFound(ConstructAnswerWithOnlyCode(UpdateAnnouncementResponses.AnnouncementDoesNotExist));
        }
        catch (AnnouncementCategoriesDoNotExist err)
        {
            LogWarning(404, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.AnnouncementCategoriesDoesNotExist), requesterId, err.Message);
            return NotFound(
                ConstructAnswerWithOnlyCode(UpdateAnnouncementResponses.AnnouncementCategoriesDoesNotExist));
        }
        catch (AttachmentDoesNotExist err)
        {
            LogWarning(404, "Редактирование объявления", nameof(UpdateAnnouncementResponses.AttachmentsDoNotExist),
                requesterId, err.Message);
            return NotFound(ConstructAnswerWithOnlyCode(UpdateAnnouncementResponses.AttachmentsDoNotExist));
        }
        catch (DelayedPublishingMomentComesInPastException err)
        {
            LogWarning(409, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.DelayedPublishingMomentIsInPast), requesterId, err.Message);
            return Conflict(
                ConstructAnswerWithOnlyCode(UpdateAnnouncementResponses.DelayedPublishingMomentIsInPast));
        }
        catch (DelayedHidingMomentComesInPastException err)
        {
            LogWarning(409, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.DelayedHidingMomentIsInPast), requesterId, err.Message);
            return Conflict(ConstructAnswerWithOnlyCode(UpdateAnnouncementResponses.DelayedHidingMomentIsInPast));
        }
        catch (AutoPublishAnAlreadyPublishedAnnouncementException err)
        {
            LogWarning(409, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.AutoPublishingPublishedAndNonHiddenAnnouncement), requesterId,
                err.Message);
            return Conflict(ConstructAnswerWithOnlyCode(UpdateAnnouncementResponses
                .AutoPublishingPublishedAndNonHiddenAnnouncement));
        }
        catch (AutoHidingAnAlreadyHiddenAnnouncementException err)
        {
            LogWarning(409, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.AutoHidingAnAlreadyHiddenAnnouncement), requesterId,
                err.Message);
            return Conflict(
                ConstructAnswerWithOnlyCode(UpdateAnnouncementResponses.AutoHidingAnAlreadyHiddenAnnouncement));
        }
        catch (CannotDetachSurveyException err)
        {
            LogWarning(409, "Редактирование объявления",
                nameof(UpdateAnnouncementResponses.CannotDetachSurvey), requesterId, err.Message);
            return Conflict(ConstructAnswerWithOnlyCode(UpdateAnnouncementResponses.CannotDetachSurvey));
        }
        catch (Exception err)
        {
            LogError(err, 500, "Редактирование объявления", requesterId);
            return Problem();
        }
    }



    private void LogWarning(int httpStatusCode, string operation, string reasonCode, Guid requesterId,
        string? additionalMessage = null) =>
        _logger.Warning(
            "HTTP-код: {HttpStatusCode}; Операция: {Operation}; Код причины: {ReasonCode}; ID пользователя: {RequesterId}; Дополнительная информация: {AdditionalMessage}",
            httpStatusCode, operation, reasonCode, requesterId, additionalMessage);

    private void LogError(Exception err, int httpStatusCode, string operation, Guid requesterId) =>
        _logger.Error(
            err, "HTTP-код: {HttpStatusCode}; Операция: {Operation}; ID пользователя: {RequesterId}",
            httpStatusCode, operation, requesterId);

    private static object ConstructAnswerWithOnlyCode<TEnum>(TEnum code)
        where TEnum : Enum =>
        new { Code = code };
}