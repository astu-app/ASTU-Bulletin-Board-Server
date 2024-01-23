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
            new ("1", true, [])
        };
        var singleAnswer = new QuestionBuildingList {
            new ("1", true, ["1"])
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
            new (string.Empty, true, ["1", "2"])
        };

        var setQuestions = () => builder.SetQuestions(questions);
        setQuestions.Should().Throw<ArgumentException>().WithMessage("Контент не может быть пустым");
    }



    [Fact]
    public void Build_NoQuestions_Throw()
    {
        var builder = new SurveyBuilder();
        builder.SetId(Guid.Empty);

        var build = () => builder.Build();
        build.Should().Throw<InvalidOperationException>().WithMessage("Список вопросов должен быть задан");
    }

    [Theory]
    [MemberData(nameof(CorrectQuestions_TestData))]
    public void Build_CorrectQuestions_Success(QuestionBuildingList questions, Survey expected)
    {
        var builder = new SurveyBuilder();
        builder.SetId(Guid.Empty);

        builder.SetQuestions(questions);
        var survey = builder.Build();
        
        survey.Should().BeEquivalentTo(expected);
    }

    public static IEnumerable<object[]> CorrectQuestions_TestData()
    {
        yield return
        [
            new QuestionBuildingList { new ("q1", true, ["a1", "a2"]), },
            new Survey(
                Guid.Empty, 
                [], 
                true, 
                false, 
                null,
                [
                    new Question(Guid.Empty, Guid.Empty,
                        "q1",
                        true,
                        [
                            new Answer(Guid.Empty, Guid.Empty, "a1"),
                            new Answer(Guid.Empty, Guid.Empty, "a2"),
                        ])
                ]
            )
        ];
        yield return
        [
            new QuestionBuildingList
            {
                new ("q1", true, ["a1", "a2"]),
                new ("q2", true, ["a1", "a2"]),
            },
            new Survey(
                Guid.Empty, 
                [], 
                true, 
                false, 
                null,
                [
                    new Question(Guid.Empty, Guid.Empty,
                        "q1",
                        true,
                        [
                            new Answer(Guid.Empty, Guid.Empty, "a1"),
                            new Answer(Guid.Empty, Guid.Empty, "a2")
                        ]),

                    new Question(Guid.Empty, Guid.Empty,
                        "q2",
                        true,
                        [
                            new Answer(Guid.Empty, Guid.Empty,"a1"),
                            new Answer(Guid.Empty, Guid.Empty,"a2")
                        ])
                ]
            )
        ];
    }
}