namespace BulletInBoardServer.Services.Services.Surveys.Exceptions;

public class MultipleSelectionInSingleChoiceQuestionException()
    : InvalidOperationException("В вопросе без множественного выбора нельзя выбрать несколько вариантов ответов");