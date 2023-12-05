using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys;

public class SurveyBuilder
{
    private Guid? _id;
    private bool? _isOpen;
    private bool _isSurveyAnonymous;
    private bool? _isMultipleChoiceAllowed;
    private DateTime? _autoClosingAt;
    private Questions.Questions? _questions;



    public SurveyBuilder SetId(Guid id)
    {
        _id = id;
        return this;
    }
    
    public SurveyBuilder IsOpen(bool isOpen)
    {
        _isOpen = isOpen;
        return this;
    }
    
    public SurveyBuilder IsAnonymous(bool isAnonymous)
    {
        _isSurveyAnonymous = isAnonymous;
        return this;
    }
    
    public SurveyBuilder IsMultiChoiceAllowed(bool allowed)
    {
        _isMultipleChoiceAllowed = allowed;
        return this;
    }
    
    public SurveyBuilder SetAutoClosingMoment(DateTime autoClosingAt)
    {
        _autoClosingAt = autoClosingAt;
        return this;
    }
    
    public SurveyBuilder SetQuestions(Questions.Questions questions)
    {
        AllQuestionsValidOrThrow(questions);
        
        _questions = questions;
        return this;
    }
    
    public Survey Build()
    {
        if (_isOpen is null)
            throw new InvalidOperationException("Открытость опроса должна быть задана");
        if (_isMultipleChoiceAllowed is null)
            throw new InvalidOperationException(
                "Возможность выбора в опросе нескольки хвариантов ответов должна быть задана");
        if (_questions is null)
            throw new InvalidOperationException("Список вопросов должен быть задан");
        
        return new Survey(
            _id ?? Guid.NewGuid(),
            _isOpen.Value,
            _isSurveyAnonymous,
            _isMultipleChoiceAllowed.Value,
            _autoClosingAt,
            _questions
        );
    }
    
    
    
    private void AllQuestionsValidOrThrow(Questions.Questions questions)
    {
        if (questions is null)
            throw new ArgumentNullException(nameof(questions));
        if (!questions.Any())
            throw new ArgumentException("Список вопросов не может быть пустым");
        
        foreach (var question in questions) 
            AllAnswersValidOrThrow(question.Answers);
    }

    private void AllAnswersValidOrThrow(ReadOnlyAnswers answers)
    {
        if (answers is null)
            throw new ArgumentNullException(nameof(answers));
        if (answers.Count < 2)
            throw new ArgumentException("Вариантов ответов не может быть меньше двух");
        
        foreach (var answer in answers)
        {
            var isAnswerAnonymous = answer is AnonymousAnswer;
            if (isAnswerAnonymous != _isSurveyAnonymous)
                throw new InvalidOperationException("Анонимность опроса и варианта ответа не совпадают");
        }
    }
}