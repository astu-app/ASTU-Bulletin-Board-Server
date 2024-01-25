using BulletInBoardServer.Models.Announcements;
using BulletInBoardServer.Models.Attachments.Surveys;
using BulletInBoardServer.Models.Attachments.Surveys.Answers;
using BulletInBoardServer.Models.Attachments.Surveys.Questions;

namespace BulletInBoardServer.Services.Surveys.Building;

using Survey = Survey;

public class SurveyBuilder
{
    private Guid? _id;
    private readonly AnnouncementList _announcements = [];
    private bool _isOpen = true;
    private bool _isAnonymous;
    private DateTime? _autoClosingAt;
    private QuestionBuildingList? _questionBuildings;



    public SurveyBuilder SetId(Guid id)
    {
        _id = id;
        return this;
    }
    
    public SurveyBuilder SetAnnouncements(AnnouncementList announcements)
    {
        foreach (var announcement in announcements) 
            _announcements.Add(announcement);
        
        return this;
    }
    
    public SurveyBuilder IsOpen(bool isOpen)
    {
        _isOpen = isOpen;
        return this;
    }
    
    public SurveyBuilder IsAnonymous(bool isAnonymous)
    {
        _isAnonymous = isAnonymous;
        return this;
    }
    
    public SurveyBuilder SetAutoClosingMoment(DateTime autoClosingAt)
    {
        _autoClosingAt = autoClosingAt;
        return this;
    }
    
    public SurveyBuilder SetQuestions(QuestionBuildingList questions)
    {
        AllQuestionsValidOrThrow(questions);
        
        _questionBuildings = questions;
        return this;
    }
    
    public Survey Build()
    {
        if (_id is null)
            throw new InvalidOperationException("Id опроса должен быть задан");
        if (_announcements is null)
            throw new InvalidOperationException("Id объявления, к которому относится опрос, должен быть задан");
        
        if (_questionBuildings is null)
            throw new InvalidOperationException("Список вопросов должен быть задан");

        var questions = CreateQuestions(_questionBuildings);
        return new Survey(
            _id.Value,
            _announcements,
            _isOpen,
            _isAnonymous,
            _autoClosingAt,
            questions
        );
    }
    
    
    
    private static void AllQuestionsValidOrThrow(QuestionBuildingList questions)
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
    
    private static void AllAnswersValidOrThrow(AnswerListBuilding answers)
    {
        ArgumentNullException.ThrowIfNull(answers);
        if (answers.Count < 2)
            throw new ArgumentException("Вариантов ответов не может быть меньше двух");
        
        foreach (var answer in answers) 
            AnswerContentValidOrThrow(answer);
    }
    
    private static void AnswerContentValidOrThrow(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Контент не может быть пустым");
    }

    private QuestionList CreateQuestions(QuestionBuildingList questionBuildings)
    {
        var questions = new QuestionList();
        foreach (var building in questionBuildings)
        {
            var answers = CreateAnswers(building.Answers);
            var question = new Question(Guid.Empty, Guid.Empty, building.Content, building.IsMultipleChoiceAllowed, answers);
            questions.Add(question);
        }

        return questions;
    }

    private static AnswerList CreateAnswers(AnswerListBuilding answerBuildings)
    {
        var answers = new AnswerList();
        foreach (var building in answerBuildings) 
            answers.Add(new Answer(Guid.Empty, Guid.Empty, building, 0));

        return answers;
    }
}