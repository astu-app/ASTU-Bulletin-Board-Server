using System.Collections.ObjectModel;

namespace BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;

public class QuestionList : Collection<Question>
// public class QuestionList : IEnumerable<Question>
{
    // public IEnumerable<Question> _questions = new List<Question>();
    public QuestionList(IList<Question> questions) : base(questions)
    {
    }


    // public QuestionList(IEnumerable<Question> questions)
    // {
    //     _questions = questions;
    // }
    //
    // public QuestionList()
    // {
    //     
    // }
    
    
    
    public QuestionList()
    {
    }
    // public IEnumerator<Question> GetEnumerator() => 
    //     _questions.GetEnumerator();
    //
    // IEnumerator IEnumerable.GetEnumerator() => 
    //     GetEnumerator();
}