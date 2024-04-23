using System.Collections.Generic;
using System.Linq;
using BulletInBoardServer.Controllers.SurveysController.Models;
using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using BulletInBoardServer.Services.Services.Surveys.Models;
using Mapster;

namespace BulletInBoardServer.Controllers.SurveysController.Config.Mapping;

// ReSharper disable once UnusedType.Global - will be used on startup while IRegisters scanning
/// <inheritdoc />
public class SurveyMapsterConfig : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateSurveyDto, CreateSurvey>()
            .Map(d => d.IsAnonymous, s => s.IsAnonymous)
            .Map(d => d.AutoClosingAt, s => s.VoteUntil)
            .Map(d => d.Questions, s => s.Questions.Adapt<IEnumerable<CreateQuestion>>()); 

        config.NewConfig<CreateQuestionDto, CreateQuestion>()
            .Map(d => d.Serial, s => s.Serial)
            .Map(d => d.Content, s => s.Content)
            .Map(d => d.IsMultipleChoiceAllowed, s => s.IsMultipleChoiceAllowed)
            .Map(d => d.Answers, s => s.Answers); 

        config.NewConfig<CreateAnswerDto, CreateAnswer>()
            .Map(d => d.Serial, s => s.Serial)
            .Map(d => d.Content, s => s.Content);
        
        config.NewConfig<VoteInSurveyDto, SurveyVotes>()
            .ConstructUsing(src =>
                new SurveyVotes(src.QuestionVotes.ToDictionary(
                    qv => qv.QuestionId,
                    qv => qv.AnswerIds.ToList())));

    }
}