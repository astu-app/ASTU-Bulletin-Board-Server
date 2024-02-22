namespace BulletInBoardServer.Services.Services.Announcements.Exceptions;

public class CannotDetachSurveyException : InvalidOperationException
{
    public CannotDetachSurveyException()
        : base("От объявления нельзя открепить опрос")
    {
        
    }
}