using System;
using System.Collections.Generic;
using BulletInBoardServer.Controllers.CategoriesController.Models;
using BulletInBoardServer.Controllers.Core.Logging;
using BulletInBoardServer.Controllers.Core.Responding;
using BulletInBoardServer.Domain.Models.AnnouncementCategories.Exceptions;
using BulletInBoardServer.Services.Services.AnnouncementCategories;
using BulletInBoardServer.Services.Services.AnnouncementCategories.Exceptions;
using BulletInBoardServer.Services.Services.AnnouncementCategories.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BulletInBoardServer.Controllers.CategoriesController.Controllers;

/// <summary>
/// Контроллер категорий объявлений
/// </summary>
public class AnnouncementCategoriesApiControllerImpl : AnnouncementCategoriesApiController
{
    private readonly ILogger _logger;
    private readonly LoggingHelper _loggingHelper;
    private readonly AnnouncementCategoryService _service;

    /// <summary>
    /// Контроллер категорий объявлений
    /// </summary>
    public AnnouncementCategoriesApiControllerImpl(AnnouncementCategoryService service)
    {
        _service = service;

        _logger = Log.ForContext<AnnouncementCategoriesApiControllerImpl>();
        _loggingHelper = new LoggingHelper(_logger);
    }



    /// <summary>
    /// Создать категорию объявлений
    /// </summary>
    /// <param name="dto"></param>
    /// <response code="201">Created</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult CreateAnnouncementCategory(CreateAnnouncementCategoryDto dto)
    {
        /*
         * 201 +
         * 400
         *   colorIsInvalidHex +
         *   nameIsNullOrEmpty +
         * 403
         *    announcementCategoryCreationForbidden
         * 500 +
         */
        
        var requesterId = Guid.Empty; // todo id пользователя

        try
        {
            var create = dto.Adapt<CreateCategory>();
            var category = _service.Create(create);

            _logger.Information("Пользователь {RequesterId} создал категорию объявлений {CategoryId}", 
                requesterId, category.Id);

            return Created("/api/announcement-categories/get-details", category.Id);
        }
        catch (ColorIsInvalidHexException err)
        {
            _loggingHelper.LogWarning(400, "Создание категории объявлений",
                nameof(CreateAnnouncementCategoryResponses.ColorIsInvalidHex), requesterId, err.Message);
            return BadRequest(
                ResponseConstructor.ConstructResponseWithOnlyCode(CreateAnnouncementCategoryResponses
                    .ColorIsInvalidHex));
        }
        catch (AnnouncementCategoryNameIsNullOrWhitespace err)
        {
            _loggingHelper.LogWarning(400, "Создание категории объявлений",
                nameof(CreateAnnouncementCategoryResponses.NameIsNullOrEmpty), requesterId,
                err.Message);
            return BadRequest(
                ResponseConstructor.ConstructResponseWithOnlyCode(CreateAnnouncementCategoryResponses
                    .NameIsNullOrEmpty));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Создание категории объявлений", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Удалить категорию объявлений
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult DeleteAnnouncementCategory(Guid id)
    {
        /*
         * 200 +
         * 403
         *    announcementCategoryDeletionForbidden
         * 404
         *    announcementCategoryDoesNotExist +
         * 500
         */
        
        var requesterId = Guid.Empty;

        try
        {
            _service.Delete(id);

            _logger.Information("Пользователь {RequesterId} удалил категорию объявлений {CategoryId}",
                requesterId, id);

            return Ok();
        }
        catch (AnnouncementCategoryDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Удаление категории объявлений",
                nameof(DeleteAnnouncementCategoryResponses.AnnouncementCategoryDoesNotExist), requesterId,
                err.Message);
            return NotFound(ResponseConstructor.ConstructResponseWithOnlyCode(DeleteAnnouncementCategoryResponses
                .AnnouncementCategoryDoesNotExist));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Удаление категории объявлений", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Получить список всех категорий объявлений
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult GetAllAnnouncementCategories()
    {
        /*
         * 200 +
         * 403
         *    announcementCategoryListAccessForbidden
         * 500 +
         */
        
        var requesterId = Guid.Empty; // todo id пользователя
        
        try
        {
            var categories = _service.GetList();

            _logger.Information("Пользователь {RequesterId} запросил список всех категорий объявлений",
                requesterId);

            var dtos = categories.Adapt<IEnumerable<CategorySummary>>(); 
            return Ok(dtos);
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Получение списка категорий объявлений", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Редактировать категорию объявлений
    /// </summary>
    /// <param name="dto"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult UpdateAnnouncementCategory(UpdateAnnouncementCategoryDto dto)
    {
        /*
         * 200 +
         * 400
         *   colorIsInvalidHex +
         *   nameIsNullOrEmpty +
         * 403
         *   announcementCategoryUpdatingForbidden +
         * 404
         *   announcementCategoryDoesNotExist
         * 500
         */

        var requesterId = Guid.Empty; // todo id пользователя
        
        try
        {
            var edit = dto.Adapt<EditCategory>();
            _service.Edit(edit);

            _logger.Information("Пользователь {RequesterId} отредактировал категорию объявлений {CategoryId}",
                requesterId,
                edit.Id);

            return Ok();
        }
        catch (ColorIsInvalidHexException err)
        {
            _loggingHelper.LogWarning(400, "Редактирование категории объявлений",
                nameof(UpdateAnnouncementCategoryResponses.ColorIsInvalidHex), requesterId, err.Message);
            return BadRequest(
                ResponseConstructor.ConstructResponseWithOnlyCode(UpdateAnnouncementCategoryResponses
                    .ColorIsInvalidHex));
        }
        catch (AnnouncementCategoryNameIsNullOrWhitespace err)
        {
            _loggingHelper.LogWarning(400, "Редактирование категории объявлений",
                nameof(UpdateAnnouncementCategoryResponses.NameIsNullOrEmpty), requesterId,
                err.Message);
            return BadRequest(
                ResponseConstructor.ConstructResponseWithOnlyCode(UpdateAnnouncementCategoryResponses
                    .NameIsNullOrEmpty));
        }
        catch (AnnouncementCategoryDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Редактирование категории объявлений",
                nameof(UpdateAnnouncementCategoryResponses.AnnouncementCategoryDoesNotExist), requesterId,
                err.Message);
            return NotFound(ResponseConstructor.ConstructResponseWithOnlyCode(UpdateAnnouncementCategoryResponses
                .AnnouncementCategoryDoesNotExist));
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Редактирование категории объявлений", requesterId);
            return Problem();
        }
    }
    
        /// <summary>
    /// Получить список всех категорий объявлений с отмеченными подписками для текущего пользователя
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult GetAnnouncementCategoriesSubscriptions()
    {
        /*
         * 200 +
         * 403
         *   getSubscriptionsListForbidden
         * 500 +
         */

        var requesterId = Guid.Empty; // todo id пользователя

        try
        {
            var subscriptions = _service.GetSubscriptions(requesterId);

            _logger.Information(
                "Пользователь {RequesterId} запросил список категорий объявлений, на которые он подписан",
                requesterId);

            var dtos = subscriptions.Adapt<IEnumerable<CategorySummary>>();
            return Ok(dtos);
        }
        catch (Exception err)
        {
            _loggingHelper.LogError(err, 500, "Получение списка подписок на категории объявлений", requesterId);
            return Problem();
        }
    }

    /// <summary>
    /// Обновить список подписок текущего пользователя
    /// </summary>
    /// <param name="dto"></param>
    /// <response code="200">Ok</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    /// <response code="500">Internal Server Error</response>
    public override IActionResult UpdateAnnouncementCategoriesSubscriptions(
        UpdateAnnouncementCategoriesSubscriptionsDto dto)
    {
        /*
         * 200 +
         * 403
         *   updateSubscriptionsForbidden
         * 404
         *   categoriesDoNotExist
         * 500
         */

        var requesterId = Guid.Empty; // todo id пользователя

        try
        {
            var update = dto.Adapt<UpdateSubscriptions>();
            _service.UpdateSubscriptions(requesterId, update);

            _logger.Information("Пользователь {RequesterId} обновил список своих подписок", requesterId);

            return Ok();
        }
        catch (AnnouncementCategoryDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Обновление подписок на категории объявлений",
                nameof(UpdateAnnouncementCategoriesSubscriptionsResponses.CategoriesDoNotExist), requesterId,
                err.Message);
            return NotFound(ResponseConstructor.ConstructResponseWithOnlyCode(
                UpdateAnnouncementCategoriesSubscriptionsResponses.CategoriesDoNotExist));
        }
    }
}