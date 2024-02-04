using BulletInBoardServer.Models.Attachments.Surveys.Answers;
using BulletInBoardServer.Models.Attachments.Surveys.Questions;

namespace BulletInBoardServer.Services.Surveys.Validating;

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

    private static void QuestionContentValidOrThrow(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Контент не может быть пустым");
    }
    
    private static void AllAnswersValidOrThrow(AnswerList answers)
    {
        ArgumentNullException.ThrowIfNull(answers);
        if (answers.Count < 2)
            throw new ArgumentException("Вариантов ответов не может быть меньше двух");
        
        foreach (var answer in answers) 
            AnswerContentValidOrThrow(answer.Content);
    }
    
    private static void AnswerContentValidOrThrow(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Контент не может быть пустым");
    }
}