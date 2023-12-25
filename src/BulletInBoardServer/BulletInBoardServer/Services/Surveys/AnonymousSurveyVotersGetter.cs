using BulletInBoardServer.Data;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;
using Microsoft.EntityFrameworkCore;

namespace BulletInBoardServer.Services.Surveys;

/// <summary>
/// Сервисный класс, позволяющий получать структурированный список пользователей,
/// проголосовавших в анонимном опросе
/// </summary>
/// <param name="dbContext">Контекст базы данных</param>
public class AnonymousSurveyVotersGetter(ApplicationDbContext dbContext)
    : SurveyVotersGetterBase(dbContext)
{
    /// <summary>
    /// Получение списка пользователей, проголосовавших в опросе
    /// </summary>
    /// <param name="surveyId">Идентификатор опроса</param>
    /// <returns>Структурированный список проголосовавших в опросе пользователей</returns>
    /// <exception cref="InvalidOperationException">Указан Id неанонимного опроса</exception>
    public AnonymousSurveyVoters GetVoters(Guid surveyId)
    {
        if (!IsSurveyAnonymous(surveyId))
            throw new InvalidOperationException("Указан id от неанонимного опроса");

        var questions = GetSurveyQuestions(surveyId);
        var voters = GetSurveyVoters(surveyId, questions);

        return voters;
    }



    protected override QuestionList GetSurveyQuestions(Guid surveyId) =>
        new(
            DbContext.Questions
                .Where(q => q.SurveyId == surveyId)
                .Include(q => q.ParticipationList)
                .ThenInclude(p => p.User)
                .ToList()
        );

    private static AnonymousSurveyVoters GetSurveyVoters(Guid surveyId, QuestionList questions)
    {
        var voters = new AnonymousSurveyVoters(surveyId, new List<PrivateQuestionVoters>());

        foreach (var question in questions) 
            voters.QuestionListVoters.Add(GetPrivateQuestionVoters(question));

        return voters;
    }

    private static PrivateQuestionVoters GetPrivateQuestionVoters(Question question)
    {
        var questionVoters = new PrivateQuestionVoters(question.Id, []);
        foreach (var participation in question.ParticipationList)
            questionVoters.Voters.Add(participation.User);

        return questionVoters;
    }
}