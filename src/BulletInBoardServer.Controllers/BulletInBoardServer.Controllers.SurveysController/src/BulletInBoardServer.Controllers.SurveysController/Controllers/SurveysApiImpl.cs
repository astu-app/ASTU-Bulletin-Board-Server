using System;
using BulletInBoardServer.Controllers.SurveysController.Models;
using BulletInBoardServer.Services.Services.Surveys;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace BulletInBoardServer.Controllers.SurveysController.Controllers;

/// <inheritdoc />
public class SurveysApiControllerImpl(SurveyService service) 
    : SurveysApiController
{
    private readonly ILogger _logger = Log.ForContext<SurveyService>();

    
    
    /// <inheritdoc />
    public override IActionResult CloseSurvey(Guid surveyId)
    {
        //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(200);
        //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(400);
        //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(401);
        //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(403, default(CloseSurveyForbidden));
        //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(404, default(CloseSurveyNotFound));
        //TODO: Uncomment the next line to return response 409 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(409, default(CloseSurveyConflict));
        //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(500);
        
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override IActionResult CreateSurvey(CreateSurveyDto createSurveyDto)
    {

        //TODO: Uncomment the next line to return response 201 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(201, default(CreateSurveyCreated));
        //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(401);
        //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(403, default(CreateSurveyForbidden));
        //TODO: Uncomment the next line to return response 409 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(409, default(CreateSurveyConflict));
        //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(500);
        string exampleJson = null;
        exampleJson = "{\r\n  \"content\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\"\r\n}";
            
        var example = exampleJson != null
            ? JsonConvert.DeserializeObject<CreateSurveyCreated>(exampleJson)
            : default(CreateSurveyCreated);
        //TODO: Change the data returned
        return new ObjectResult(example);    }

    /// <inheritdoc />
    public override IActionResult DownloadSurveyResults(Guid body)
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
        return new ObjectResult(example);    }

    /// <inheritdoc />
    public override IActionResult GetSurveyDetails(Guid? body)
    {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(GetSurveyDetails200Response));
            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);
            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);
            //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(403, default(GetSurveyDetailsForbidden));
            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404, default(GetSurveyDetailsNotFound));
            //TODO: Uncomment the next line to return response 409 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(409, default(GetSurveyDetailsConflict));
            //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(500);
            string exampleJson = null;
            exampleJson = "{\r\n  \"content\" : {\r\n    \"isAnonymous\" : false,\r\n    \"isOpen\" : true,\r\n    \"questions\" : [ {\r\n      \"votersAmount\" : 0,\r\n      \"answers\" : [ {\r\n        \"votersAmount\" : 0,\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"content\" : \"content\"\r\n      }, {\r\n        \"votersAmount\" : 0,\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"content\" : \"content\"\r\n      } ],\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"isMultipleChoiceAllowed\" : false,\r\n      \"content\" : \"content\"\r\n    }, {\r\n      \"votersAmount\" : 0,\r\n      \"answers\" : [ {\r\n        \"votersAmount\" : 0,\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"content\" : \"content\"\r\n      }, {\r\n        \"votersAmount\" : 0,\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"content\" : \"content\"\r\n      } ],\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"isMultipleChoiceAllowed\" : false,\r\n      \"content\" : \"content\"\r\n    } ],\r\n    \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n    \"voteFinishedAt\" : \"2000-01-23T04:56:07.000+00:00\"\r\n  }\r\n}";
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<GetSurveyDetails200Response>(exampleJson)
            : default(GetSurveyDetails200Response);
            //TODO: Change the data returned
            return new ObjectResult(example);    }

    /// <inheritdoc />
    public override IActionResult GetSurveysResults(Guid body)
    {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(GetSurveysResultsOk));
            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);
            //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(401);
            //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(403, default(GetSurveysResultsForbidden));
            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404, default(GetSurveysResultsNotFound));
            //TODO: Uncomment the next line to return response 409 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(409, default(GetSurveysResultsConflict));
            //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(500);
            string exampleJson = null;
            exampleJson = "{\r\n  \"content\" : {\r\n    \"voters\" : [ {\r\n      \"name\" : \"name\",\r\n      \"id\" : 0\r\n    }, {\r\n      \"name\" : \"name\",\r\n      \"id\" : 0\r\n    } ],\r\n    \"answerResults\" : [ {\r\n      \"questionId\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"voterIds\" : [ 6, 6 ]\r\n    }, {\r\n      \"questionId\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"voterIds\" : [ 6, 6 ]\r\n    } ]\r\n  }\r\n}";
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<GetSurveysResultsOk>(exampleJson)
            : default(GetSurveysResultsOk);
            //TODO: Change the data returned
            return new ObjectResult(example);    }

    /// <inheritdoc />
    public override IActionResult VoteInQuestion(VoteInSurveyDto voteInSurveyDto)
    {
        //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(200);
        //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(400);
        //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(401);
        //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(403, default(VoteInQuestionForbidden));
        //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(404, default(VoteInQuestionNotFound));
        //TODO: Uncomment the next line to return response 409 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(409, default(VoteInQuestionConflict));
        //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(500);

        throw new NotImplementedException();    }
}