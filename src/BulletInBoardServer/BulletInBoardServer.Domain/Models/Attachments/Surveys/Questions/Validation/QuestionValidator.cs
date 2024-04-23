using BulletInBoardServer.Domain.Models.Attachments.Surveys.Answers;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Exceptions;

namespace BulletInBoardServer.Domain.Models.Attachments.Surveys.Questions.Validation;

public static class QuestionValidator
{
    public static void AllQuestionsValidOrThrow(QuestionList questions)
    {
        ArgumentNullException.ThrowIfNull(questions);
        if (questions.Count == 0)
            throw new ArgumentException("Список вопросов не может быть пустым");

        AllQuestionSerialsUniqueOrThrow(questions);
        foreach (var question in questions)
        {
            QuestionContentValidOrThrow(question.Content);
            AllAnswersValidOrThrow(question.Answers);
        }
    }

    public static void AllQuestionSerialsUniqueOrThrow(QuestionList questions)
    {
        var serials = questions.Select(question => question.Serial);
        if (!AreAllSerialsUnique(serials))
            throw new SurveyContainsQuestionSerialsDuplicates(
                "Среди порядковых номеров вопросов не должно быть повторений");
    }
    
    public static void QuestionContentValidOrThrow(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Контент вопроса не может быть пустым");
    }

    public static void AllAnswersValidOrThrow(AnswerList answers)
    {
        ArgumentNullException.ThrowIfNull(answers);
        if (answers.Count < 2)
            throw new ArgumentException("Вариантов ответов не может быть меньше двух");

        AllAnswerSerialsUniqueOrThrow(answers);
        foreach (var answer in answers)
            AnswerContentValidOrThrow(answer.Content);
    }

    public static void AllAnswerSerialsUniqueOrThrow(AnswerList answers)
    {
        var serials = answers.Select(answer => answer.Serial);
        if (!AreAllSerialsUnique(serials))
            throw new QuestionContainsAnswersSerialsDuplicates(
                "Среди порядковых номеров вариантов ответов не должно быть повторений");
    }

    public static void AnswerContentValidOrThrow(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Контент варианта ответа не может быть пустым");
    }

    public static bool AreAllSerialsUnique(IEnumerable<int> serials)
    {
        var checkSerials = new HashSet<int>();
        return serials.All(serial => checkSerials.Add(serial));
    }
}