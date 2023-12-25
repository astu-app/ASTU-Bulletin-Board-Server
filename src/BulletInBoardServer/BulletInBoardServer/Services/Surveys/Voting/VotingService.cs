using BulletInBoardServer.Data;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.QuestionParticipation;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;
using Microsoft.EntityFrameworkCore;

namespace BulletInBoardServer.Services.Surveys.Voting;

/// <summary>
/// Сервис голосования в опросах
/// </summary>
/// <param name="dbContext">Контекст базы данных</param>
public class VotingService(ApplicationDbContext dbContext)
{
    /// <summary>
    /// Голосование в вопросе
    /// </summary>
    /// <param name="userId">Идентификатор голосующего пользователя</param>
    /// <param name="questionId">Идентификатор вопроса, в котором пользователь голосует</param>
    /// <param name="answerIds">Набор идентификаторов выбранных вариантов ответов</param>
    /// <remarks>
    /// Список вариантов ответов должен содержать ровно один элемент в случае,
    /// если в опросе запрещен множественный выбор, и не меньше в противном случае
    /// </remarks>
    public void Vote(Guid userId, Guid questionId, IEnumerable<Guid> answerIds)
    {
        // todo переработать под голосование в опросе
        var answerIdList = answerIds.ToList();
        var question = GetQuestionOrThrow(questionId);
        var survey = GetSurveyOrThrow(question.SurveyId);
        QuestionOpenOrThrow(question);
        AnswersValidOrThrow(answerIdList, question);
        
        survey.IncreaseVotersCount();
        VoteForAnswers(userId, answerIdList, question);

        dbContext.SaveChangesAsync();
    }



    private static void QuestionOpenOrThrow(Question question)
    {
        if (!question.IsOpen)
            throw new InvalidOperationException("Нельзя проголосовать в закрытом вопросе");
    }
    
    private static void AnswersValidOrThrow(IReadOnlyCollection<Guid> answerIdList, Question question)
    {
        if (answerIdList.Count == 0)
            throw new ArgumentException("Список вариантов ответов не может быть пустым");

        if (!question.IsMultipleChoiceAllowed && answerIdList.Count > 1)
            throw new ArgumentException(
                "Нельзя проголосовать за несколько вариантов ответов в вопросе с единственным выбором");

        if (!IsDbContainsAllAnswers(answerIdList, question))
            throw new InvalidOperationException("Один или несколько вариантов ответов не относятся к опросу");
    }

    private void VoteForAnswers(Guid userId, IEnumerable<Guid> answerIds, Question question)
    {
        var participation = new Participation(userId, question.Id);
        dbContext.Participation.Add(participation);

        foreach (var answerId in answerIds)
        {
            var answer = GetAnswerOrThrow(answerId);
            answer.IncreaseVotersCount();

            if (!question.IsAnonymous)
                dbContext.UserSelections.Add(new UserSelection(participation, answerId));
        }
    }
    
    private Question GetQuestionOrThrow(Guid questionId)
    {
        try
        {
            var question = dbContext.Questions
                .Include(question => question.Answers)
                .Single(s => s.Id == questionId);

            return question;
        }
        catch (InvalidOperationException err)
        {
            throw new InvalidOperationException("Не удалось загрузить вопрос из бд", err);
        }
    }

    private Survey GetSurveyOrThrow(Guid surveyId)
    {
        try
        {
            var survey = dbContext.Surveys.Single(s => s.Id == surveyId);
            return survey;
        }
        catch (InvalidOperationException err)
        {
            throw new InvalidOperationException("Не удалось загрузить опрос из бд", err);
        }
    }

    private Answer GetAnswerOrThrow(Guid answerId)
    {
        try
        {
            var user = dbContext.Answers.Single(answer => answer.Id == answerId);
            return user;
        }
        catch (InvalidOperationException err)
        {
            throw new InvalidOperationException("Не удалось загрузить пользователя из бд", err);
        }
    }

    private static bool IsDbContainsAllAnswers(IEnumerable<Guid> answerIdList, Question question)
    {
        var dbContainsAllAnswerIds =
            answerIdList.Except(
                    question.Answers
                        .Select(a => a.Id))
                .Any();
        return dbContainsAllAnswerIds;
    }
}