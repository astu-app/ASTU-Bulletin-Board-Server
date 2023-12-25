using BulletInBoardServer.Data;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;

namespace BulletInBoardServer.Services.Surveys;

/// <summary>
/// Базовый класс для сервисных классов получения списка проголосовавших в опросе
/// </summary>
/// <param name="dbContext">Контекст базы данных</param>
public abstract class SurveyVotersGetterBase(ApplicationDbContext dbContext)
{
    protected readonly ApplicationDbContext DbContext = dbContext;



    protected abstract QuestionList GetSurveyQuestions(Guid surveyId);



    protected bool IsSurveyAnonymous(Guid surveyId)
    {
        var survey = DbContext.Surveys.Single(s => s.Id == surveyId);
        return survey.IsAnonymous;
    }
}