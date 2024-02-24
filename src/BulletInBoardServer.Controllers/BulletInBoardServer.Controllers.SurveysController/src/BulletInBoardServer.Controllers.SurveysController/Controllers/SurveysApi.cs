/*
 * API Шлюз. Опросы
 *
 * API шлюза для управления опросами
 *
 * The version of the OpenAPI document: 0.0.2
 *
 * Generated by: https://openapi-generator.tech
 */

using System;
using BulletInBoardServer.Controllers.SurveysController.Attributes;
using BulletInBoardServer.Controllers.SurveysController.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulletInBoardServer.Controllers.SurveysController.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public abstract class SurveysApiController : ControllerBase
    { 
        /// <summary>
        /// Закрыть опрос
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("/api/surveys/close-survey")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 403, type: typeof(CloseSurveyForbidden))]
        [ProducesResponseType(statusCode: 404, type: typeof(CloseSurveyNotFound))]
        [ProducesResponseType(statusCode: 409, type: typeof(CloseSurveyConflict))]
        public abstract IActionResult CloseSurvey([FromBody]Guid body);

        /// <summary>
        /// Создать опрос
        /// </summary>
        /// <param name="createSurveyDto"></param>
        /// <response code="201">Created</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("/api/surveys/create")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 201, type: typeof(CreateSurveyCreated))]
        [ProducesResponseType(statusCode: 403, type: typeof(CreateSurveyForbidden))]
        [ProducesResponseType(statusCode: 409, type: typeof(CreateSurveyConflict))]
        public abstract IActionResult CreateSurvey([FromBody]CreateSurveyDto createSurveyDto);

        /// <summary>
        /// Скачать результаты опроса
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/api/surveys/download-results")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(DownloadSurveyResultsOk))]
        [ProducesResponseType(statusCode: 403, type: typeof(DownloadSurveyResultsForbidden))]
        [ProducesResponseType(statusCode: 404, type: typeof(DownloadSurveyResultsNotFound))]
        [ProducesResponseType(statusCode: 409, type: typeof(DownloadSurveyResultsConflict))]
        public abstract IActionResult DownloadSurveyResults([FromBody]Guid body);

        /// <summary>
        /// Получить детали выбранного опроса
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/api/surveys/get-details")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetSurveyDetails200Response))]
        [ProducesResponseType(statusCode: 403, type: typeof(GetSurveyDetailsForbidden))]
        [ProducesResponseType(statusCode: 404, type: typeof(GetSurveyDetailsNotFound))]
        [ProducesResponseType(statusCode: 409, type: typeof(GetSurveyDetailsConflict))]
        public abstract IActionResult GetSurveyDetails([FromBody]Guid? body);

        /// <summary>
        /// Получить результаты опроса
        /// </summary>
        /// <param name="body"></param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/api/surveys/get-results")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 200, type: typeof(GetSurveysResultsOk))]
        [ProducesResponseType(statusCode: 403, type: typeof(GetSurveysResultsForbidden))]
        [ProducesResponseType(statusCode: 404, type: typeof(GetSurveysResultsNotFound))]
        [ProducesResponseType(statusCode: 409, type: typeof(GetSurveysResultsConflict))]
        public abstract IActionResult GetSurveysResults([FromBody]Guid body);

        /// <summary>
        /// Проголосовать в вопросе
        /// </summary>
        /// <param name="voteInSurveyDto"></param>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("/api/surveys/vote")]
        [Consumes("application/json")]
        [ValidateModelState]
        [ProducesResponseType(statusCode: 403, type: typeof(VoteInQuestionForbidden))]
        [ProducesResponseType(statusCode: 404, type: typeof(VoteInQuestionNotFound))]
        [ProducesResponseType(statusCode: 409, type: typeof(VoteInQuestionConflict))]
        public abstract IActionResult VoteInQuestion([FromBody]VoteInSurveyDto voteInSurveyDto);
    }
}
