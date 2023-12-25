using BulletInBoardServer.Models.Announcements.Attachments.Surveys;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;
using BulletInBoardServer.Services.Surveys.Building;

namespace BulletInBoardServer.Test.Models.Announcements;

public class SurveyBuilderTest
{
    [Fact]
    public void SetQuestions_NoQuestions_Throws()
    {
        var builder = new SurveyBuilder();
        var questions = new QuestionBuildingList();

        var setQuestions = () => builder.SetQuestions(questions);
        setQuestions.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void SetQuestions_QuestionWithLessThanTwoAnswers_Throws()
    {
        var builder = new SurveyBuilder();
        var noAnswers = new QuestionBuildingList {
            new ("1", [])
        };
        var singleAnswer = new QuestionBuildingList {
            new ("1", ["1"])
        };

        var setZeroAnswers = () => builder.SetQuestions(noAnswers);
        var setSingleAnswer = () => builder.SetQuestions(singleAnswer);
        
        setZeroAnswers.Should().Throw<ArgumentException>().WithMessage("Вариантов ответов не может быть меньше двух");
        setSingleAnswer.Should().Throw<ArgumentException>().WithMessage("Вариантов ответов не может быть меньше двух");
    }
    
    [Fact]
    public void SetQuestions_QuestionWithEmptyContent_Throws()
    {
        var builder = new SurveyBuilder();
        var questions = new QuestionBuildingList {
            new (string.Empty, ["1", "2"])
        };

        var setQuestions = () => builder.SetQuestions(questions);
        setQuestions.Should().Throw<ArgumentException>().WithMessage("Контент не может быть пустым");
    }



    [Fact]
    public void Build_NoQuestions_Throw()
    {
        var builder = new SurveyBuilder();

        var build = () => builder.Build();
        build.Should().Throw<InvalidOperationException>().WithMessage("Список вопросов должен быть задан");
    }

    [Theory]
    [MemberData(nameof(CorrectQuestions_TestData))]
    public void Build_CorrectQuestions_Success(QuestionBuildingList questions, Survey expected)
    {
        var builder = new SurveyBuilder();

        builder.SetQuestions(questions);
        var survey = builder.Build();
        
        survey.Should().BeEquivalentTo(expected);
    }

    public static IEnumerable<object[]> CorrectQuestions_TestData()
    {
        yield return new object[]
        {
            new QuestionBuildingList { new ("q1", ["a1", "a2"]), },
            new Survey(
                Guid.Empty, 
                // Guid.Empty, 
                true, 
                false, 
                false, 
                null,
                [
                    new Question(Guid.Empty, Guid.Empty,
                        "q1",
                        [
                            new Answer(Guid.Empty, Guid.Empty, "a1", 0),
                            new Answer(Guid.Empty, Guid.Empty, "a2", 0),
                        ],
                        0,
                        false, false)
                ]
            )
        };
        yield return new object[]
        {
            new QuestionBuildingList
            {
                new ("q1", ["a1", "a2"]),
                new ("q2", ["a1", "a2"]),
            },
            new Survey(
                Guid.Empty, 
                // Guid.Empty, 
                true, 
                false, 
                false, 
                null,
                [
                    new Question(Guid.Empty, Guid.Empty,
                        "q1",
                        [
                            new Answer(Guid.Empty, Guid.Empty, "a1", 0),
                            new Answer(Guid.Empty, Guid.Empty, "a2", 0)
                        ],
                        0,
                        false, false),

                    new Question(Guid.Empty, Guid.Empty,
                        "q2",
                        [
                            new Answer(Guid.Empty, Guid.Empty,"a1", 0),
                            new Answer(Guid.Empty, Guid.Empty,"a2", 0)
                        ],
                        0,
                        false, false)
                ]
            )
        };
    }
}