using BulletInBoardServer.Data;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;
using Microsoft.EntityFrameworkCore;

namespace BulletInBoardServer.Services.Surveys;

/// <summary>
/// Сервисный класс, позволяющий получать структурированный список пользователей,
/// проголосовавших в неанонимном опросе
/// </summary>
/// <param name="dbContext">Контекст базы данных</param>
public sealed class PublicSurveyVotersGetter(ApplicationDbContext dbContext)
    : SurveyVotersGetterBase(dbContext)
{
    /// <summary>
    /// Получение списка пользователей, проголосовавших в опросе
    /// </summary>
    /// <param name="surveyId">Идентификатор опроса</param>
    /// <returns>Структурированный список проголосовавших в опросе пользователей</returns>
    /// <exception cref="InvalidOperationException">Указан Id анонимного опроса</exception>
    public PublicSurveyVoters GetVoters(Guid surveyId)
    {
        if (IsSurveyAnonymous(surveyId))
            throw new InvalidOperationException("Указан id анонимного опроса");

        var questions = GetSurveyQuestions(surveyId);
        var voters = GetSurveyVoters(surveyId, questions);

        return voters;
    }



    protected override QuestionList GetSurveyQuestions(Guid surveyId) =>
        DbContext.Questions
            .Where(q => q.SurveyId == surveyId)
            .Include(q => q.Answers)
            .ToQuestions();
    
    private PublicSurveyVoters GetSurveyVoters(Guid surveyId, QuestionList questions)
    {
        var voters = new PublicSurveyVoters
        {
            SurveyId = surveyId, 
            QuestionListVoters = new List<PublicQuestionVoters>()
        };

        foreach (var question in questions) 
            voters.QuestionListVoters.Add(GetQuestionVoters(question));

        return voters;
    }

    private PublicQuestionVoters GetQuestionVoters(Question question)
    {
        var questionVoters = new PublicQuestionVoters
        {
            QuestionId = question.Id, 
            AnswerListVoters = new List<AnswerVoters>()
        };
        
        foreach (var answer in question.Answers)
        {
            var voters = GetPublicAnswerVoters(answer.Id);
            var answerVoters = new AnswerVoters { AnswerId = answer.Id, Voters = voters };
            questionVoters.AnswerListVoters.Add(answerVoters);
        }

        return questionVoters;
    }
    
    private VoterList GetPublicAnswerVoters(Guid answerId) =>
        DbContext.UserSelections
            .Where(us => us.AnswerId == answerId)
            .Include(us => us.Participation)
            .ThenInclude(p => p.User)
            .Select(us => us.Participation.User)
            .ToVoters();
}