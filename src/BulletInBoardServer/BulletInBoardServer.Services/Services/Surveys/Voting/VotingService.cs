using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Answers;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Questions;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.SurveyParticipation;
using BulletInBoardServer.Services.Services.Surveys.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BulletInBoardServer.Services.Services.Surveys.Voting;

/// <summary>
/// Сервис голосования в опросах
/// </summary>
/// <param name="dbContext">Контекст базы данных</param>
public class VotingService(ApplicationDbContext dbContext)
{
    private Guid _voterId;
    private Guid _surveyId;

    private Survey _survey = null!; // устанавливается при каждом запуске единственного публичного метода Vote()

    private SurveyVotes
        _votes = null!; // устанавливается при каждом запуске единственного публичного метода Vote()



    /// <summary>
    /// Проголосовать в опросе
    /// </summary>
    /// <param name="voterId">Идентификатор голосующего пользователя</param>
    /// <param name="surveyId">Идентификатор опроса, в котором пользователь голосует</param>
    /// <param name="votes">Голоса пользователя в каждом из вопросов опроса</param>
    /// <exception cref="SurveyDoesNotExistException">Опрос не существует в базе данных</exception>
    /// <exception cref="SurveyAlreadyVotedException">Пользователь уже проголосовал в указанном опросе</exception>
    /// <exception cref="SurveyClosedException">Попытка проголосовать в закрытом опросе</exception>
    /// <exception cref="MultipleSelectionInSingleChoiceQuestionException">Попытка выбрать несколько вариантов ответов в вопросе без множественного выбора</exception>
    /// <exception cref="PresentedQuestionsDoesntMatchSurveyQuestionsException">Список представленных вопросов не соответствует фактическому списку вопросов опроса</exception>
    /// <exception cref="PresentedVotesDoesntMatchQuestionAnswersException">Список представленных вариантов ответов не соответствует фактическому списку вариантов ответов соответствующего вопроса</exception>
    public void Vote(Guid voterId, Guid surveyId, SurveyVotes votes)
    {
        SetServiceState(voterId, surveyId, votes);

        _survey = LoadSurvey();

        SurveyOpenOrThrow();
        ParticipantVotesForFirstTimeOrThrow();

        AllQuestionsReferToSurveyOrThrow();
        VotesValidOrThrow();

        _survey.IncreaseVotersCount();
        VoteForAllQuestions();

        dbContext.SaveChanges();
    }



    private void SetServiceState(Guid voterId, Guid surveyId, SurveyVotes votes)
    {
        _voterId = voterId;
        _surveyId = surveyId;
        _votes = votes;
    }

    private void ParticipantVotesForFirstTimeOrThrow()
    {
        var voted = _survey.Voters.Any(u => u.Id == _voterId);
        if (voted)
            throw new SurveyAlreadyVotedException();
    }

    private Survey LoadSurvey()
    {
        try
        {
            var survey = dbContext.Surveys
                .Include(survey => survey.Voters)
                .Include(survey => survey.Questions)
                .ThenInclude(question => question.Answers)
                .Single(s => s.Id == _surveyId);
            return survey;
        }
        catch (InvalidOperationException err)
        {
            throw new InvalidOperationException("Не удалось загрузить опрос из БД", err);
        }
    }

    private void SurveyOpenOrThrow()
    {
        if (!_survey.IsOpen) throw new SurveyClosedException();
    }

    private void AllQuestionsReferToSurveyOrThrow()
    {
        var votesQuestionIds = _votes.GetQuestionsIds();
        var surveyQuestionIds = _survey.Questions.Select(q => q.Id);
        var allRefer = !surveyQuestionIds.Except(votesQuestionIds).Any();
        if (!allRefer) throw new PresentedQuestionsDoesntMatchSurveyQuestionsException();
    }

    private void VotesValidOrThrow()
    {
        foreach (var question in _survey.Questions)
            AnswersValidForQuestionOrThrow(question, _votes.GetVotes(question.Id));
    }

    private static void AnswersValidForQuestionOrThrow(Question question, IEnumerable<Guid> answerIds)
    {
        var answerIdsList = answerIds.ToList();

        if (answerIdsList.Count == 0)
            throw new ArgumentException("Список вариантов ответов не может быть пустым");

        if (!question.IsMultipleChoiceAllowed && answerIdsList.Count > 1)
            throw new MultipleSelectionInSingleChoiceQuestionException();

        if (!AllVotesReferToQuestion(question, answerIdsList))
            throw new PresentedVotesDoesntMatchQuestionAnswersException();
    }

    private static bool AllVotesReferToQuestion(Question question, IEnumerable<Guid> answerIds)
    {
        var questionAnswerIds = question.Answers.Select(a => a.Id);
        var refer = !answerIds.Except(questionAnswerIds).Any();
        return refer;
    }

    private void VoteForAllQuestions()
    {
        var participation = new Participation(Guid.NewGuid(), _voterId, _survey.Id);
        dbContext.Participation.Add(participation);

        foreach (var question in _survey.Questions)
        {
            var questionVotes = _votes.GetVotes(question.Id);
            VoteForSingleQuestion(participation, questionVotes);
        }
    }

    private void VoteForSingleQuestion(Participation participation, IEnumerable<Guid> answerToVoteIds)
    {
        foreach (var answerId in answerToVoteIds)
        {
            var answer = GetAnswer(answerId);
            answer.IncreaseVotersCount();

            if (!participation.IsAnonymous)
                dbContext.UserSelections.Add(new UserSelection(participation, answerId));
        }
    }

    private Answer GetAnswer(Guid id)
    {
        foreach (var question in _survey.Questions)
        {
            var answer = question.Answers.SingleOrDefault(a => a.Id == id);
            if (answer is not null)
                return answer;
        }

        throw new AnswerDoesNotExistException();
    }
}