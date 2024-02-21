using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Answers;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Questions;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Voters;
using BulletInBoardServer.Services.Services.Surveys.DelayedOperations;
using BulletInBoardServer.Services.Services.Surveys.Infrastructure;
using BulletInBoardServer.Services.Services.Surveys.VotersGetting;
using BulletInBoardServer.Services.Services.Surveys.Voting;
using Microsoft.EntityFrameworkCore;

namespace BulletInBoardServer.Services.Services.Surveys;

/// <summary>
/// Сервисный класс для взаимодействия с опросами
/// </summary>
public class SurveyService(
    ApplicationDbContext dbContext,
    IAutomaticSurveyOperationsDispatcher dispatcher)
{
    /// <summary>
    /// Создать опрос
    /// </summary>
    /// <param name="newSurvey">Параметры нового опроса</param>
    public Survey Create(CreateSurvey newSurvey)
    {
        var surveyId = Guid.NewGuid();

        var questions = (
                from newQuestion in newSurvey.Questions
                let questionId = Guid.NewGuid()
                let answers = CreateAnswers(newQuestion.Answers, questionId)
                select new Question(
                    id: questionId,
                    surveyId: surveyId,
                    content: newQuestion.Content,
                    isMultipleChoiceAllowed: newQuestion.IsMultipleChoiceAllowed,
                    answers: answers))
            .AsQuestionList();

        var survey = new Survey(
            id: surveyId,
            announcements: [],
            isOpen: newSurvey.IsOpen,
            isAnonymous: newSurvey.IsAnonymous,
            autoClosingAt: newSurvey.AutoClosingAt,
            questions: questions
        );

        dbContext.Surveys.Add(survey);
        dbContext.SaveChanges();

        if (survey.ExpectsAutoClosing)
            dispatcher.ConfigureAutoClosing(
                survey.Id,
                survey.AutoClosingAt ??
                throw new InvalidOperationException(
                    $"{nameof(survey.AutoClosingAt)} не может быть null"));

        return survey;
    }

    /// <summary>
    /// Проголосовать в опросе
    /// </summary>
    /// <param name="voterId">Идентификатор голосующего пользователя</param>
    /// <param name="surveyId">Идентификатор опроса, в котором пользователь голосует</param>
    /// <param name="votes">Голоса пользователя в каждом из вопросов опроса</param>
    public void Vote(Guid voterId, Guid surveyId, SurveyVotes votes)
    {
        var votingService = new VotingService(dbContext);
        votingService.Vote(voterId, surveyId, votes);
    }

    /// <summary>
    /// Получить структурированный список проголосовавших в опросе пользователей
    /// </summary>
    /// <param name="surveyId">Идентификатор опроса</param>
    /// <returns>Структурированный список проголосовавших в опросе пользователей</returns>
    public SurveyVotersBase GetSurveyVoters(Guid surveyId)
    {
        var survey = LoadFullSurvey(surveyId);
        var getter = SurveyVotersGetterBase.ResolveVotersGetter(survey);
        return getter.GetVoters();
    }

    /// <summary>
    /// Закрыть опрос
    /// </summary>
    /// <param name="surveyId">Идентификатор закрываемого опроса</param>
    /// <remarks>Закрываемый опрос должен быть открыт</remarks>
    public void CloseSurvey(Guid surveyId)
    {
        var survey = LoadSurveySummary(surveyId);

        if (survey.ExpectsAutoClosing)
            dispatcher.DisableAutoClosing(surveyId);

        survey.Close();
        dbContext.SaveChanges();
    }



    /// <summary>
    /// Создать варианты ответов
    /// </summary>
    /// <param name="newAnswers">Параметры новых вариантов ответов</param>
    /// <param name="questionId">Id вопроса, к которому прикрепляются варианты ответов</param>
    /// <returns></returns>
    private static AnswerList CreateAnswers(IEnumerable<CreateAnswer> newAnswers, Guid questionId)
    {
        var answers = new AnswerList();
        foreach (var answer in newAnswers)
            answers.Add(new Answer(
                id: Guid.NewGuid(),
                questionId: questionId,
                content: answer.Content));

        return answers;
    }

    /// <summary>
    /// Метод загружает опрос полностью, включая все его вопросы, варианты ответов и проголосовавших пользователей
    /// </summary>
    /// <param name="surveyId">Идентификатор опроса</param>
    /// <returns>Загруженный опрос со связанными сущностями</returns>
    /// <exception cref="InvalidOperationException">Не удалось загрузить опрос из БД</exception>
    private Survey LoadFullSurvey(Guid surveyId)
    {
        try
        {
            return dbContext.Surveys
                .Where(s => s.Id == surveyId)
                .Include(s => s.Voters)
                .Include(s => s.Questions)
                .ThenInclude(q => q.Answers)
                .ThenInclude(a => a.Participation)
                .ThenInclude(p => p.User)
                .Single();
        }
        catch (InvalidOperationException err)
        {
            throw new InvalidOperationException("Не удалось загрузить опрос из БД", err);
        }
    }

    /// <summary>
    /// Метод загружает только класс опроса, не включая связанные сущности
    /// </summary>
    /// <param name="surveyId">Идентификатор опроса</param>
    /// <returns>Загруженный опрос без связанных сущностей</returns>
    /// <exception cref="InvalidOperationException">Не удалось загрузить опрос из БД</exception>
    private Survey LoadSurveySummary(Guid surveyId)
    {
        try
        {
            return dbContext.Surveys.Single(s => s.Id == surveyId);
        }
        catch (InvalidOperationException err)
        {
            throw new InvalidOperationException("Не удалось загрузить опрос из БД", err);
        }
    }
}