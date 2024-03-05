using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Answers;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Questions;
using BulletInBoardServer.Services.Services.Surveys.DelayedOperations;
using BulletInBoardServer.Services.Services.Surveys.Exceptions;
using BulletInBoardServer.Services.Services.Surveys.Models;
using BulletInBoardServer.Services.Services.Surveys.VotersGetting;
using BulletInBoardServer.Services.Services.Surveys.VotersGetting.Models;
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
            isOpen: true,
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
    /// <exception cref="SurveyDoesNotExistException">Опрос не существует в базе данных</exception>
    /// <exception cref="SurveyAlreadyVotedException">Пользователь уже проголосовал в указанном опросе</exception>
    /// <exception cref="SurveyClosedException">Попытка проголосовать в закрытом опросе</exception>
    /// <exception cref="AnswerDoesNotExistException">Вариант ответа не существует в базе данных</exception>
    /// <exception cref="MultipleSelectionInSingleChoiceQuestionException">Попытка выбрать несколько вариантов ответов в вопросе без множественного выбора</exception>
    /// <exception cref="PresentedQuestionsDoesntMatchSurveyQuestionsException">Список представленных вопросов не соответствует фактическому списку вопросов опроса</exception>
    /// <exception cref="PresentedVotesDoesntMatchQuestionAnswersException">Список представленных вариантов ответов не соответствует фактическому списку вариантов ответов соответствующего вопроса</exception>
    public void Vote(Guid voterId, Guid surveyId, SurveyVotes votes)
    {
        var votingService = new VotingService(dbContext);
        votingService.Vote(voterId, surveyId, votes);
    }
    
    /// <summary>
    /// Метод предоставляет опрос со всеми его вопросами, вариантами ответов и проголосовавшими пользователями
    /// </summary>
    /// <param name="surveyId">Идентификатор опроса</param>
    /// <returns>Загруженный опрос со связанными сущностями</returns>
    /// <exception cref="SurveyDoesNotExistException">Опрос с заданным id не существует в БД</exception>
    /// <exception cref="InvalidOperationException">Не удалось загрузить опрос из БД</exception>
    public Survey GetDetails(Guid surveyId)
    {
        try
        {
            var survey = dbContext.Surveys
                .Where(s => s.Id == surveyId)
                .Include(s => s.Voters)
                .Include(s => s.Questions)
                .ThenInclude(q => q.Answers)
                .ThenInclude(a => a.Participation)
                .ThenInclude(p => p.User)
                .Single();
            
            if (survey is null)
                throw new SurveyDoesNotExistException();

            return survey;
        }
        catch (InvalidOperationException err) when (err is not SurveyDoesNotExistException)
        {
            throw new InvalidOperationException("Не удалось загрузить опрос из БД", err);
        }
    }

    /// <summary>
    /// Получить структурированный список проголосовавших в опросе пользователей
    /// </summary>
    /// <param name="surveyId">Идентификатор опроса</param>
    /// <returns>Структурированный список проголосовавших в опросе пользователей</returns>
    public SurveyVotersBase GetSurveyVoters(Guid surveyId) // todo использовать для выгрузки результатов
    {
        var survey = GetDetails(surveyId);
        var getter = SurveyVotersGetterBase.ResolveVotersGetter(survey);
        return getter.GetVoters();
    }

    /// <summary>
    /// Закрыть опрос
    /// </summary>
    /// <param name="surveyId">Идентификатор закрываемого опроса</param>
    /// <exception cref="SurveyDoesNotExistException">Опрос с заданным id не существует в БД</exception>
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
    /// <returns>Список вариантов ответов</returns>
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
    /// Метод загружает только класс опроса, не включая связанные сущности
    /// </summary>
    /// <param name="surveyId">Идентификатор опроса</param>
    /// <returns>Загруженный опрос без связанных сущностей</returns>
    /// <exception cref="SurveyDoesNotExistException">Опрос с заданным id не существует в БД</exception>
    /// <exception cref="InvalidOperationException">Не удалось загрузить опрос из БД</exception>
    private Survey LoadSurveySummary(Guid surveyId)
    {
        try
        {
            var survey = dbContext.Surveys.SingleOrDefault(s => s.Id == surveyId);
            if (survey is null)
                throw new SurveyDoesNotExistException();

            return survey;
        }
        catch (InvalidOperationException err) when (err is not SurveyDoesNotExistException)
        {
            throw new InvalidOperationException("Не удалось загрузить опрос из БД", err);
        }
    }
}