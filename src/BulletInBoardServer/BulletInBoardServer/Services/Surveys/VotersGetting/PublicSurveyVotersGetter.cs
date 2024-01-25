using BulletInBoardServer.Models.Announcements.Attachments.Surveys;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Voters;

namespace BulletInBoardServer.Services.Surveys.VotersGetting;

/// <summary>
/// Сервисный класс, позволяющий получать структурированный список пользователей,
/// проголосовавших в неанонимном опросе
/// </summary>
public sealed class PublicSurveyVotersGetter(Survey survey) : SurveyVotersGetterBase(survey)
{
    protected override void AnonymityCorrectOrThrow()
    {
        if (Survey.IsAnonymous)
            throw new InvalidOperationException("Указан id анонимного опроса");
    }

    protected override PublicSurveyVoters GetSurveyVoters()
    {
        var voters = new PublicSurveyVoters(Survey.Id, Survey.Voters, new List<QuestionVoters>());

        foreach (var question in Survey.Questions)
            voters.EveryQuestionVoters.Add(GetQuestionVoters(question));

        return voters;
    }

    private static QuestionVoters GetQuestionVoters(Question question)
    {
        var questionVoters = new QuestionVoters(question.Id, new List<AnswerVoters>());

        foreach (var answer in question.Answers)
        {
            var answerVoterList = answer.Participation
                .Select(p => p.User)
                .ToVoters();

            var answerVoters = new AnswerVoters(answer.Id, answerVoterList);
            questionVoters.AnswerListVoters.Add(answerVoters);
        }

        return questionVoters;
    }
}