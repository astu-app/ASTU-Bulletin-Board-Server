using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using BulletInBoardServer.Services.Services.Surveys.VotersGetting.Models;

namespace BulletInBoardServer.Services.Services.Surveys.VotersGetting;

/// <summary>
/// Сервисный класс, позволяющий получать структурированный список пользователей,
/// проголосовавших в анонимном опросе
/// </summary>
public class AnonymousSurveyVotersGetter(Survey survey) : SurveyVotersGetterBase(survey)
{
    protected override void AnonymityCorrectOrThrow()
    {
        if (!Survey.IsAnonymous)
            throw new InvalidOperationException("Указан id от неанонимного опроса");
    }

    protected override AnonymousSurveyVoters GetSurveyVoters()
    {
        var voters = new AnonymousSurveyVoters(Survey.Id, Survey.Voters);
        return voters;
    }
}