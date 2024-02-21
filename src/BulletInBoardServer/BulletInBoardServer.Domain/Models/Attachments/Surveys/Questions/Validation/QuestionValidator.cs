using BulletInBoardServer.Domain.Models.Attachments.Surveys.Answers;

namespace BulletInBoardServer.Domain.Models.Attachments.Surveys.Questions.Validation;

public static class QuestionValidator
{
    public static void AllQuestionsValidOrThrow(QuestionList questions)
    {
        ArgumentNullException.ThrowIfNull(questions);
        if (questions.Count == 0)
            throw new ArgumentException("Список вопросов не может быть пустым");

        foreach (var question in questions)
        {
            QuestionContentValidOrThrow(question.Content);
            AllAnswersValidOrThrow(question.Answers);
        }
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

        foreach (var answer in answers)
            AnswerContentValidOrThrow(answer.Content);
    }

    public static void AnswerContentValidOrThrow(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Контент варианта ответа не может быть пустым");
    }
}