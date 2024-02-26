namespace BulletInBoardServer.Services.Services.Surveys.Exceptions;

public class PresentedVotesDoesntMatchQuestionAnswersException : InvalidOperationException
{
    public PresentedVotesDoesntMatchQuestionAnswersException()
        : base("В списке представленных ответов присутствуют не относящиеся к вопросу")
    {
    }
}