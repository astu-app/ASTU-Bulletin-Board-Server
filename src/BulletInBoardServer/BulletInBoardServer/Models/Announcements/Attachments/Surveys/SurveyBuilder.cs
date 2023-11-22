namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys;

public class SurveyBuilder
{
    private Guid? _id;
    private bool? _isOpen;
    private bool? _isAnonymous;
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
        _isAnonymous = isAnonymous;
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
        if (questions is null)
            throw new ArgumentNullException(nameof(questions));
        if (!questions.Any())
            throw new ArgumentException("Список вопросов не может быть пустым");
        
        _questions = questions;
        return this;
    }
    
    public Survey Build()
    {
        if (_isOpen is null)
            throw new InvalidOperationException("Открытость опроса должна быть задана");
        if (_isAnonymous is null)
            throw new InvalidOperationException("Анонимность опроса должна быть задана");
        if (_isMultipleChoiceAllowed is null)
            throw new InvalidOperationException(
                "Возможность выбора в опросе нескольки хвариантов ответов должна быть задана");
        if (_questions is null)
            throw new InvalidOperationException("Список вопросов должен быть задан");
        
        return new Survey(
            _id ?? Guid.NewGuid(),
            _isOpen.Value,
            _isAnonymous.Value,
            _isMultipleChoiceAllowed.Value,
            _autoClosingAt,
            _questions
        );
    }
}