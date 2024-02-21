using System;
using BulletInBoardServer.Controllers.AnnouncementsController.Models;
using BulletInBoardServer.Services.Services.Announcements;
using BulletInBoardServer.Services.Services.Announcements.Infrastructure;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BulletInBoardServer.Controllers.AnnouncementsController.Controllers;

/// <inheritdoc />
public class AnnouncementsApiControllerImpl(AnnouncementService service)
    : AnnouncementsApiController
{
    /// <inheritdoc />
    public override IActionResult CreateAnnouncement([FromBody] CreateAnnouncementDto dto)
    {
        var validationResult = ValidateModel(dto);
        if (validationResult is not null)
            return validationResult;

        var createAnnouncement = dto.Adapt<CreateAnnouncement>();
        // try
        // {
        var announcement = service.Create(Guid.Empty, createAnnouncement); // todo id пользователя

        var response = new
        {
            Code = CreateAnnouncementResponses.Created,
            Content = announcement.Id
        };
        return Created(new Uri("/api/announcements/get-details"), response);
        // }
        // catch()


        //TODO: Uncomment the next line to return response 201 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(201, default(CreateAnnouncementCreated));

        //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(400, default(CreateAnnouncementBadRequest));

        //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(401, default(CreateAnnouncementUnauthorized));

        //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(403, default(CreateAnnouncementForbidden));

        //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(404, default(CreateAnnouncementNotFound));

        //TODO: Uncomment the next line to return response 409 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(409, default(CreateAnnouncementConflict));
        string exampleJson = null;
        exampleJson = "{\r\n  \"content\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\"\r\n}";

        var example = exampleJson != null
            ? JsonConvert.DeserializeObject<CreateAnnouncementCreated>(exampleJson)
            : default(CreateAnnouncementCreated);
        //TODO: Change the data returned
        return new ObjectResult(example);


        IActionResult? ValidateModel(CreateAnnouncementDto dto_)
        {
            if (dto_.UserIds is null || dto_.UserIds.Count == 0)
                return BadRequest(new { code = CreateAnnouncementResponses.UsergroupListNullOrEmpty });

            return null;
        }
    }

    /// <inheritdoc />
    public override IActionResult DeleteAnnouncement([FromBody] Guid id)
    {
        // service.Delete(Guid.Empty, id); // todo id пользователя
        //
        // var response = new DeleteAnnouncementOk { Code = DeleteAnnouncementResponses.Ok };
        // return Ok(response);

        //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(200, default(DeleteAnnouncementOk));
        //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(400, default(DeleteAnnouncementBadRequest));
        //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(401, default(DeleteAnnouncementUnauthorized));
        //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(403, default(DeleteAnnouncementForbidden));
        //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(404, default(DeleteAnnouncementNotFound));
        string exampleJson = null;
        exampleJson = "{ }";

        var example = exampleJson != null
            ? JsonConvert.DeserializeObject<DeleteAnnouncementOk>(exampleJson)
            : default(DeleteAnnouncementOk);
        //TODO: Change the data returned
        return new ObjectResult(example);
    }

    /// <inheritdoc />
    public override IActionResult GetAnnouncementDetails([FromBody] Guid id)
    {
        // var announcement = service.GetDetails(Guid.Empty, id); // todo id пользователя
        var announcement = service.GetDetails(Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc"), id); // todo id пользователя
        
        var response = MakeOkResult();
        return Ok(response);


        GetAnnouncementDetailsOk MakeOkResult()
            => new ()
            {
                Code = GetAnnouncementDetailsResponses.Ok,
                Content = announcement.Adapt<AnnouncementDetailsDto>()
            };

        //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(200, default(GetAnnouncementDetailsOk));
        //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(400, default(GetAnnouncementDetailsBadRequest));
        //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(401, default(GetAnnouncementDetailsUnauthorized));
        //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(403, default(GetAnnouncementDetailsForbidden));
        //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(404, default(GetAnnouncementDetailsNotFound));
        string exampleJson = null;
        exampleJson =
            "{\r\n  \"content\" : {\r\n    \"hiddenAt\" : \"2000-01-23T04:56:07.000+00:00\",\r\n    \"audienceSize\" : 0,\r\n    \"publishedAt\" : \"2000-01-23T04:56:07.000+00:00\",\r\n    \"authorName\" : \"authorName\",\r\n    \"viewsCount\" : 0,\r\n    \"surveys\" : [ {\r\n      \"isAnonymous\" : false,\r\n      \"isOpen\" : true,\r\n      \"questions\" : [ {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      }, {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      } ],\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"voteFinishedAt\" : \"2000-01-23T04:56:07.000+00:00\"\r\n    }, {\r\n      \"isAnonymous\" : false,\r\n      \"isOpen\" : true,\r\n      \"questions\" : [ {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      }, {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      } ],\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"voteFinishedAt\" : \"2000-01-23T04:56:07.000+00:00\"\r\n    } ],\r\n    \"delayedHidingAt\" : \"2000-01-23T04:56:07.000+00:00\",\r\n    \"delayedPublishingAt\" : \"2000-01-23T04:56:07.000+00:00\",\r\n    \"files\" : [ {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    }, {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    } ],\r\n    \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n    \"categories\" : [ {\r\n      \"color\" : \"color\",\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\"\r\n    }, {\r\n      \"color\" : \"color\",\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\"\r\n    } ],\r\n    \"content\" : \"content\"\r\n  }\r\n}";

        var example = exampleJson != null
            ? JsonConvert.DeserializeObject<GetAnnouncementDetailsOk>(exampleJson)
            : default(GetAnnouncementDetailsOk);
        //TODO: Change the data returned
        return new ObjectResult(example);
    }

    /// <inheritdoc />
    public override IActionResult GetDelayedHiddenAnnouncementList()
    {
        // var announcements = service.GetDelayedHidingAnnouncementsForUser(Guid.Empty); // todo id пользователя
        //
        // var response = new GetDelayedHiddenAnnouncementListOk
        // {
        //     Code = GetDelayedHiddenAnnouncementListResponses.Ok,
        //     Content = announcements.Adapt<List<AnnouncementSummaryDto>>()
        // };
        // return Task.FromResult(response);

        //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(200, default(GetDelayedHiddenAnnouncementListOk));
        //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(401, default(GetDelayedHiddenAnnouncementListUnauthorized));
        //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(403, default(GetDelayedHiddenAnnouncementListForbidden));
        string exampleJson = null;
        exampleJson =
            "{\r\n  \"content\" : [ {\r\n    \"audienceSize\" : 0,\r\n    \"publishedAt\" : \"2000-01-23T04:56:07.000+00:00\",\r\n    \"authorName\" : \"authorName\",\r\n    \"viewsCount\" : 0,\r\n    \"files\" : [ {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    }, {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    } ],\r\n    \"survey\" : {\r\n      \"isAnonymous\" : false,\r\n      \"isOpen\" : true,\r\n      \"questions\" : [ {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      }, {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      } ],\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"voteFinishedAt\" : \"2000-01-23T04:56:07.000+00:00\"\r\n    },\r\n    \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n    \"content\" : \"content\"\r\n  }, {\r\n    \"audienceSize\" : 0,\r\n    \"publishedAt\" : \"2000-01-23T04:56:07.000+00:00\",\r\n    \"authorName\" : \"authorName\",\r\n    \"viewsCount\" : 0,\r\n    \"files\" : [ {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    }, {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    } ],\r\n    \"survey\" : {\r\n      \"isAnonymous\" : false,\r\n      \"isOpen\" : true,\r\n      \"questions\" : [ {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      }, {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      } ],\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"voteFinishedAt\" : \"2000-01-23T04:56:07.000+00:00\"\r\n    },\r\n    \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n    \"content\" : \"content\"\r\n  } ]\r\n}";

        var example = exampleJson != null
            ? JsonConvert.DeserializeObject<GetDelayedHiddenAnnouncementListOk>(exampleJson)
            : default(GetDelayedHiddenAnnouncementListOk);
        //TODO: Change the data returned
        return new ObjectResult(example);
    }

    /// <inheritdoc />
    public override IActionResult GetDelayedPublishingAnnouncementList()
    {
        // var announcements = service.GetDelayedPublicationAnnouncements(Guid.Empty); // todo id пользователя
        //
        // var response = new GetDelayedPublishingAnnouncementListOk
        // {
        //     Code = GetDelayedPublishingAnnouncementListResponses.Ok,
        //     Content = announcements.Adapt<List<AnnouncementSummaryDto>>() 
        // };
        // return Task.FromResult(response);

        //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(200, default(GetDelayedPublishingAnnouncementListOk));
        //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(401, default(GetDelayedPublishingAnnouncementListUnauthorized));
        //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(403, default(GetDelayedPublishingAnnouncementListForbidden));
        string exampleJson = null;
        exampleJson =
            "{\r\n  \"content\" : [ {\r\n    \"audienceSize\" : 0,\r\n    \"publishedAt\" : \"2000-01-23T04:56:07.000+00:00\",\r\n    \"authorName\" : \"authorName\",\r\n    \"viewsCount\" : 0,\r\n    \"files\" : [ {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    }, {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    } ],\r\n    \"survey\" : {\r\n      \"isAnonymous\" : false,\r\n      \"isOpen\" : true,\r\n      \"questions\" : [ {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      }, {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      } ],\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"voteFinishedAt\" : \"2000-01-23T04:56:07.000+00:00\"\r\n    },\r\n    \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n    \"content\" : \"content\"\r\n  }, {\r\n    \"audienceSize\" : 0,\r\n    \"publishedAt\" : \"2000-01-23T04:56:07.000+00:00\",\r\n    \"authorName\" : \"authorName\",\r\n    \"viewsCount\" : 0,\r\n    \"files\" : [ {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    }, {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    } ],\r\n    \"survey\" : {\r\n      \"isAnonymous\" : false,\r\n      \"isOpen\" : true,\r\n      \"questions\" : [ {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      }, {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      } ],\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"voteFinishedAt\" : \"2000-01-23T04:56:07.000+00:00\"\r\n    },\r\n    \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n    \"content\" : \"content\"\r\n  } ]\r\n}";

        var example = exampleJson != null
            ? JsonConvert.DeserializeObject<GetDelayedPublishingAnnouncementListOk>(exampleJson)
            : default(GetDelayedPublishingAnnouncementListOk);
        //TODO: Change the data returned
        return new ObjectResult(example);
    }

    /// <inheritdoc />
    public override IActionResult GetHiddenAnnouncementList()
    {
        // var announcements = service.GetHiddenAnnouncements(Guid.Empty); // todo id пользователя
        //
        // var response = new GetHiddenAnnouncementListOk
        // {
        //     Code = GetHiddenAnnouncementListResponses.Ok,
        //     Content = announcements.Adapt<List<AnnouncementSummaryDto>>() 
        // };
        // return Task.FromResult(response);

        //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(200, default(GetHiddenAnnouncementListOk));
        //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(401, default(GetHiddenAnnouncementListUnauthorized));
        //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(403, default(GetHiddenAnnouncementListForbidden));
        string exampleJson = null;
        exampleJson =
            "{\r\n  \"content\" : [ {\r\n    \"audienceSize\" : 0,\r\n    \"publishedAt\" : \"2000-01-23T04:56:07.000+00:00\",\r\n    \"authorName\" : \"authorName\",\r\n    \"viewsCount\" : 0,\r\n    \"files\" : [ {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    }, {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    } ],\r\n    \"survey\" : {\r\n      \"isAnonymous\" : false,\r\n      \"isOpen\" : true,\r\n      \"questions\" : [ {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      }, {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      } ],\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"voteFinishedAt\" : \"2000-01-23T04:56:07.000+00:00\"\r\n    },\r\n    \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n    \"content\" : \"content\"\r\n  }, {\r\n    \"audienceSize\" : 0,\r\n    \"publishedAt\" : \"2000-01-23T04:56:07.000+00:00\",\r\n    \"authorName\" : \"authorName\",\r\n    \"viewsCount\" : 0,\r\n    \"files\" : [ {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    }, {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    } ],\r\n    \"survey\" : {\r\n      \"isAnonymous\" : false,\r\n      \"isOpen\" : true,\r\n      \"questions\" : [ {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      }, {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      } ],\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"voteFinishedAt\" : \"2000-01-23T04:56:07.000+00:00\"\r\n    },\r\n    \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n    \"content\" : \"content\"\r\n  } ]\r\n}";

        var example = exampleJson != null
            ? JsonConvert.DeserializeObject<GetHiddenAnnouncementListOk>(exampleJson)
            : default(GetHiddenAnnouncementListOk);
        //TODO: Change the data returned
        return new ObjectResult(example);
    }

    /// <inheritdoc />
    public override IActionResult GetPostedAnnouncementList()
    {
        // var announcements = service.GetPublishedAnnouncements(Guid.Empty); // todo id пользователя
        //
        // var response = new GetPostedAnnouncementListOk
        // {
        //     Code = GetPostedAnnouncementListResponses.Ok,
        //     Content = announcements.Adapt<List<AnnouncementSummaryDto>>() 
        // };
        // return Task.FromResult(response);

        //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(200, default(GetPostedAnnouncementListOk));
        //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(401, default(GetPostedAnnouncementListUnauthorized));
        //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(403, default(GetPostedAnnouncementListForbidden));
        string exampleJson = null;
        exampleJson =
            "{\r\n  \"content\" : [ {\r\n    \"audienceSize\" : 0,\r\n    \"publishedAt\" : \"2000-01-23T04:56:07.000+00:00\",\r\n    \"authorName\" : \"authorName\",\r\n    \"viewsCount\" : 0,\r\n    \"files\" : [ {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    }, {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    } ],\r\n    \"survey\" : {\r\n      \"isAnonymous\" : false,\r\n      \"isOpen\" : true,\r\n      \"questions\" : [ {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      }, {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      } ],\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"voteFinishedAt\" : \"2000-01-23T04:56:07.000+00:00\"\r\n    },\r\n    \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n    \"content\" : \"content\"\r\n  }, {\r\n    \"audienceSize\" : 0,\r\n    \"publishedAt\" : \"2000-01-23T04:56:07.000+00:00\",\r\n    \"authorName\" : \"authorName\",\r\n    \"viewsCount\" : 0,\r\n    \"files\" : [ {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    }, {\r\n      \"name\" : \"name\",\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"type\" : \"mediafile\"\r\n    } ],\r\n    \"survey\" : {\r\n      \"isAnonymous\" : false,\r\n      \"isOpen\" : true,\r\n      \"questions\" : [ {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      }, {\r\n        \"votersAmount\" : 0,\r\n        \"answers\" : [ {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        }, {\r\n          \"votersAmount\" : 0,\r\n          \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n          \"content\" : \"content\"\r\n        } ],\r\n        \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n        \"isMultipleChoiceAllowed\" : false,\r\n        \"content\" : \"content\"\r\n      } ],\r\n      \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n      \"voteFinishedAt\" : \"2000-01-23T04:56:07.000+00:00\"\r\n    },\r\n    \"id\" : \"046b6c7f-0b8a-43b9-b35d-6489e6daee91\",\r\n    \"content\" : \"content\"\r\n  } ]\r\n}";

        var example = exampleJson != null
            ? JsonConvert.DeserializeObject<GetPostedAnnouncementListOk>(exampleJson)
            : default(GetPostedAnnouncementListOk);
        //TODO: Change the data returned
        return new ObjectResult(example);
    }

    /// <inheritdoc />
    public override IActionResult HidePostedAnnouncement([FromBody] Guid body)
    {
        // service.Hide(Guid.Empty, id, DateTime.Now); // todo id пользователя
        //
        // var response = new HidePostedAnnouncementOk { Code = HidePostedAnnouncementResponses.Ok };
        // return Task.FromResult(response);

        //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(200, default(HidePostedAnnouncementOk));
        //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(400, default(HidePostedAnnouncementBadRequest));
        //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(401, default(HidePostedAnnouncementUnauthorized));
        //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(403, default(HidePostedAnnouncementForbidden));
        //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(404, default(HidePostedAnnouncementNotFound));
        //TODO: Uncomment the next line to return response 409 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(409, default(HidePostedAnnouncementConflict));
        string exampleJson = null;
        exampleJson = "{ }";

        var example = exampleJson != null
            ? JsonConvert.DeserializeObject<HidePostedAnnouncementOk>(exampleJson)
            : default(HidePostedAnnouncementOk);
        //TODO: Change the data returned
        return new ObjectResult(example);
    }

    /// <inheritdoc />
    public override IActionResult PublishImmediatelyDelayedPublishingAnnouncement([FromBody] Guid body)
    {
        // service.Publish(Guid.Empty, id, DateTime.Now); // todo id пользователя
        //
        // var response = new PublishImmediatelyDelayedPublishingAnnouncementOk
        //     { Code = PublishImmediatelyDelayedAnnouncementResponses.Ok };
        // return Task.FromResult(response);

        //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(200, default(PublishImmediatelyDelayedPublishingAnnouncementOk));
        //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(400, default(PublishImmediatelyDelayedPublishingAnnouncementBadRequest));
        //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(401, default(PublishImmediatelyDelayedPublishingAnnouncementUnauthorized));
        //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(403, default(PublishImmediatelyDelayedPublishingAnnouncementForbidden));
        //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(404, default(PublishImmediatelyDelayedPublishingAnnouncementNotFound));
        //TODO: Uncomment the next line to return response 409 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(409, default(PublishImmediatelyDelayedPublishingAnnouncementConflict));
        string exampleJson = null;
        exampleJson = "{ }";

        var example = exampleJson != null
            ? JsonConvert.DeserializeObject<PublishImmediatelyDelayedPublishingAnnouncementOk>(exampleJson)
            : default(PublishImmediatelyDelayedPublishingAnnouncementOk);
        //TODO: Change the data returned
        return new ObjectResult(example);
    }

    /// <inheritdoc />
    public override IActionResult RestoreHiddenAnnouncement([FromBody] Guid body)
    {
        // service.Restore(Guid.Empty, id, DateTime.Now); // todo id пользователя
        //
        // var response = new RestoreHiddenAnnouncementOk { Code = UnhideHiddenAnnouncementResponses.Ok };
        // return Task.FromResult(response);

        //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(200, default(RestoreHiddenAnnouncementOk));
        //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(400, default(RestoreHiddenAnnouncementBadRequest));
        //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(401, default(RestoreHiddenAnnouncementUnauthorized));
        //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(403, default(RestoreHiddenAnnouncementForbidden));
        //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(404, default(RestoreHiddenAnnouncementNotFound));
        //TODO: Uncomment the next line to return response 409 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(409, default(RestoreHiddenAnnouncementConflict));
        string exampleJson = null;
        exampleJson = "{ }";

        var example = exampleJson != null
            ? JsonConvert.DeserializeObject<RestoreHiddenAnnouncementOk>(exampleJson)
            : default(RestoreHiddenAnnouncementOk);
        //TODO: Change the data returned
        return new ObjectResult(example);
    }

    /// <inheritdoc />
    public override IActionResult UpdateAnnouncement([FromBody] UpdateAnnouncementDto updateAnnouncementDto)
    {
        // var editAnnouncement = dto.Adapt<EditAnnouncement>(); 
        // service.Edit(Guid.Empty, editAnnouncement); // todo id пользователя
        //
        // var response = new UpdateAnnouncementOk { Code = UpdateAnnouncementResponses.Ok };
        // return Task.FromResult(response);

        //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(200, default(UpdateAnnouncementOk));
        //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(400, default(UpdateAnnouncementBadRequest));
        //TODO: Uncomment the next line to return response 401 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(401, default(UpdateAnnouncementUnauthorized));
        //TODO: Uncomment the next line to return response 403 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(403, default(UpdateAnnouncementForbidden));
        //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(404, default(UpdateAnnouncementNotFound));
        //TODO: Uncomment the next line to return response 409 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
        // return StatusCode(409, default(UpdateAnnouncementConflict));
        string exampleJson = null;
        exampleJson = "{ }";

        var example = exampleJson != null
            ? JsonConvert.DeserializeObject<UpdateAnnouncementOk>(exampleJson)
            : default(UpdateAnnouncementOk);
        //TODO: Change the data returned
        return new ObjectResult(example);
    }
}