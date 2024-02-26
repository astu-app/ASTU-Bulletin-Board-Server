using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using BulletInBoardServer.Services.Services.Surveys.VotersGetting.Models;

namespace BulletInBoardServer.Services.Services.Surveys.VotersGetting;

/// <summary>
/// Базовый класс для сервисных классов получения списка проголосовавших в опросе
/// </summary>
public abstract class SurveyVotersGetterBase(Survey survey)
{
    protected readonly Survey Survey = survey;



    public static SurveyVotersGetterBase ResolveVotersGetter(Survey survey)
    {
        SurveyVotersGetterBase getter = survey.IsAnonymous
            ? new AnonymousSurveyVotersGetter(survey)
            : new PublicSurveyVotersGetter(survey);

        return getter;
    }



    /// <summary>
    /// Получение списка пользователей, проголосовавших в опросе
    /// </summary>
    /// <returns>Структурированный список проголосовавших в опросе пользователей</returns>
    /// <exception cref="InvalidOperationException">Указан Id анонимного опроса</exception>
    public SurveyVotersBase GetVoters()
    {
        AnonymityCorrectOrThrow();

        var voters = GetSurveyVoters();
        return voters;
    }



    protected abstract void AnonymityCorrectOrThrow();

    protected abstract SurveyVotersBase GetSurveyVoters();
}