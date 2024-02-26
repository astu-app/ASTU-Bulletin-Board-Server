namespace BulletInBoardServer.Services.Services.Surveys.Exceptions;

public class PresentedVotesDoesntMatchQuestionAnswersException()
    : InvalidOperationException("В списке представленных ответов присутствуют не относящиеся к вопросу");