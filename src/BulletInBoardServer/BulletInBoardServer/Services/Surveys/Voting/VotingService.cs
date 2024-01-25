using BulletInBoardServer.DataAccess;
using BulletInBoardServer.Models.Attachments.Surveys;
using BulletInBoardServer.Models.Attachments.Surveys.Answers;
using BulletInBoardServer.Models.Attachments.Surveys.Questions;
using BulletInBoardServer.Models.Attachments.Surveys.SurveyParticipation;
using Microsoft.EntityFrameworkCore;

namespace BulletInBoardServer.Services.Surveys.Voting;

/// <summary>
/// Сервис голосования в опросах
/// </summary>
/// <param name="dbContext">Контекст базы данных</param>
public class VotingService(ApplicationDbContext dbContext)
{
    private Guid _voterId;
    private Guid _surveyId; 
    
    private Survey _survey = null!; // устанавливается при каждом запуске единственного публичного метода Vote()
    private SurveyVotes _votes = null!; // устанавливается при каждом запуске единственного публичного метода Vote()
    
    
    
    /// <summary>
    /// Проголосовать в опросе
    /// </summary>
    /// <param name="voterId">Идентификатор голосующего пользователя</param>
    /// <param name="surveyId">Идентификатор опроса, в котором пользователь голосует</param>
    /// <param name="votes">Голоса пользователя в каждом из вопросов опроса</param>
    public void Vote(Guid voterId, Guid surveyId, SurveyVotes votes)
    {
        SetServiceState(voterId, surveyId, votes);
        
        _survey = LoadSurveyOrThrow();
        SurveyOpenOrThrow();
        
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
    
    private Survey LoadSurveyOrThrow()
    {
        try
        {
            var survey = dbContext.Surveys
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
        if (!_survey.IsOpen)
            throw new InvalidOperationException("Нельзя проголосовать в закрытом опросе");
    }
    
    private void AllQuestionsReferToSurveyOrThrow()
    {
        var votesQuestionIds = _votes.GetQuestionsIds();
        var surveyQuestionIds = _survey.Questions.Select(q => q.Id); 
        var allRefer = surveyQuestionIds.SequenceEqual(votesQuestionIds);
        if (!allRefer)
            throw new InvalidOperationException("Список предоставленных вопросов не соответствует списку вопросов опроса");
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
            throw new InvalidOperationException("В вопросе без множественного выбора нельзя выбрать несколько вариантов ответов");
        
        if (!AllVotesReferToQuestion(question, answerIdsList))
            throw new InvalidOperationException("В списке представленных ответов присутствуют не относящиеся к вопросу");
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

    private void VoteForSingleQuestion(Participation participation, IEnumerable<Guid> answerIds)
    {
        foreach (var answerId in answerIds)
        {
            var answer = GetAnswerOrThrow(answerId);
            answer.IncreaseVotersCount();

            if (!participation.IsAnonymous)
                dbContext.UserSelections.Add(new UserSelection(participation, answerId));
        }
    }

    private Answer GetAnswerOrThrow(Guid answerId)
    {
        try
        {
            var answer = dbContext.Answers.Single(answer => answer.Id == answerId);
            return answer;
        }
        catch (InvalidOperationException err)
        {
            throw new InvalidOperationException("Не удалось загрузить пользователя из БД", err);
        }
    }
}