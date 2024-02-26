using System;
using BulletInBoardServer.Controllers.Core.Logging;
using BulletInBoardServer.Controllers.Core.Responding;
using BulletInBoardServer.Controllers.SurveysController.Models;
using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Exceptions;
using BulletInBoardServer.Services.Services.Surveys;
using BulletInBoardServer.Services.Services.Surveys.Exceptions;
using BulletInBoardServer.Services.Services.Surveys.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace BulletInBoardServer.Controllers.SurveysController.Controllers;

/// <inheritdoc />
public class SurveysApiControllerImpl : SurveysApiController
{
    private readonly SurveyService _service;

    private readonly ILogger _logger = Log.ForContext<SurveyService>();
    private readonly LoggingHelper _loggingHelper;



    /// <inheritdoc />
    public SurveysApiControllerImpl(SurveyService service)
    {
        _service = service;
        _loggingHelper = new LoggingHelper(_logger);
    }



    /// <inheritdoc />
    public override IActionResult CloseSurvey(Guid surveyId)
    {
        /*
         * 200 +
         * 403
         *   surveyClosingForbidden
         * 404
         *   surveyDoesNotExist +
         * 409
         *   surveyAlreadyClosed +
         */

        var requesterId = Guid.Empty; // todo id пользователя

        try
        {
            _service.CloseSurvey(surveyId);

            _logger.Information("Пользователь {RequesterId} закрыл опрос {SurveyId}", requesterId, surveyId);

            return Ok();
        }
        catch (SurveyDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Закрытие опроса", nameof(CloseSurveyResponses.SurveyDoesNotExist),
                requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(CloseSurveyResponses.SurveyDoesNotExist));
        }
        catch (SurveyAlreadyClosedException err)
        {
            _loggingHelper.LogWarning(404, "Закрытие опроса", nameof(CloseSurveyResponses.SurveyAlreadyClosed),
                requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(CloseSurveyResponses.SurveyAlreadyClosed));
        }
    }

    /// <inheritdoc />
    public override IActionResult CreateSurvey(CreateSurveyDto createSurveyDto)
    {
        /*
         * 201 +
         * 403
         *   createSurveyForbidden
         */
        
        var requesterId = Guid.Empty; // todo id пользователя

        var createSurvey = createSurveyDto.Adapt<CreateSurvey>();
        var survey = _service.Create(createSurvey);

        _logger.Information("Пользователь {RequesterId} создал опрос {SurveyId}", requesterId, survey.Id);
        
        return Created("/api/announcements/get-details", survey.Id);
    }

    /// <inheritdoc />
    public override IActionResult DownloadSurveyResults(DownloadSurveyResultsRequestDto downloadSurveyResultsRequestDto)
    {
        //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(200, default(DownloadSurveyResultsOk));
        //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(400);
        //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(401);
        //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(403, default(DownloadSurveyResultsForbidden));
        //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(404, default(DownloadSurveyResultsNotFound));
        //TODO: Uncomment the next line to return response 409 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(409, default(DownloadSurveyResultsConflict));
        //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(500);
        string exampleJson = null;
        exampleJson = "{\r\n  \"content\" : \"\"\r\n}";
        exampleJson = "Custom MIME type example not yet supported: application/octet-stream";

        var example = exampleJson != null
            ? JsonConvert.DeserializeObject<DownloadSurveyResultsOk>(exampleJson)
            : default(DownloadSurveyResultsOk);
        //TODO: Change the data returned
        return new ObjectResult(example);
    }

    /// <inheritdoc />
    public override IActionResult VoteInSurvey(VoteInSurveyDto voteInSurveyDto)
    {
        /*
         * 200 +
         * 403
         *   votingForbidden
         * 404
         *   surveyDoesNotExist +
         * 409
         *   surveyClosed +
         *   surveyAlreadyVoted +
         *   cannotSelectMultipleAnswersInSingleChoiceQuestion +
         *   presentedQuestionsDoesntMatchSurveyQuestions +
         *   presentedVotesDoesntMatchQuestionAnswers +
         */
        
        // var requesterId = Guid.Empty; // todo id пользователя
        var requesterId = Guid.Parse("26ee62dc-6b8c-4001-9a35-79a99a8944f3"); // debug

        try
        {
            var surveyVotes = voteInSurveyDto.Adapt<SurveyVotes>(); 
            
            _service.Vote(requesterId, voteInSurveyDto.SurveyId, surveyVotes);

            return Ok();
        }
        catch (SurveyDoesNotExistException err)
        {
            _loggingHelper.LogWarning(404, "Голосование в опросе",
                nameof(VoteInSurveyResponses.SurveyDoesNotExist), requesterId, err.Message);
            return NotFound(
                ResponseConstructor.ConstructResponseWithOnlyCode(VoteInSurveyResponses.SurveyDoesNotExist));
        }
        catch (SurveyClosedException err)
        {
            _loggingHelper.LogWarning(409, "Голосование в опросе",
                nameof(VoteInSurveyResponses.SurveyClosed), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(VoteInSurveyResponses.SurveyClosed));
        }
        catch (SurveyAlreadyVotedException err)
        {
            _loggingHelper.LogWarning(409, "Голосование в опросе",
                nameof(VoteInSurveyResponses.SurveyAlreadyVoted), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(VoteInSurveyResponses.SurveyAlreadyVoted));
        }
        catch (MultipleSelectionInSingleChoiceQuestionException err)
        {
            _loggingHelper.LogWarning(409, "Голосование в опросе",
                nameof(VoteInSurveyResponses.CannotSelectMultipleAnswersInSingleChoiceQuestion), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(VoteInSurveyResponses.CannotSelectMultipleAnswersInSingleChoiceQuestion));
        }
        catch (PresentedQuestionsDoesntMatchSurveyQuestionsException err) 
        {
            _loggingHelper.LogWarning(409, "Голосование в опросе",
                nameof(VoteInSurveyResponses.PresentedQuestionsDoesntMatchSurveyQuestions), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(VoteInSurveyResponses.PresentedQuestionsDoesntMatchSurveyQuestions));
        }
        catch (PresentedVotesDoesntMatchQuestionAnswersException err)
        {
            _loggingHelper.LogWarning(409, "Голосование в опросе",
                nameof(VoteInSurveyResponses.PresentedVotesDoesntMatchQuestionAnswers), requesterId, err.Message);
            return Conflict(
                ResponseConstructor.ConstructResponseWithOnlyCode(VoteInSurveyResponses.PresentedVotesDoesntMatchQuestionAnswers));
        }
    }
}