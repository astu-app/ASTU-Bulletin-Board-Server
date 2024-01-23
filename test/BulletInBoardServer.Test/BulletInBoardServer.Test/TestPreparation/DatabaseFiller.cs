using BulletInBoardServer.Data;
using BulletInBoardServer.Models.Announcements;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Answers;
using BulletInBoardServer.Models.Announcements.Attachments.Surveys.Questions;
using BulletInBoardServer.Models.UserGroups;
using BulletInBoardServer.Models.Users;
using static BulletInBoardServer.Test.TestPreparation.TestDataIds;
// ReSharper disable InconsistentNaming

namespace BulletInBoardServer.Test.TestPreparation;

public static class DatabaseFiller
{
    public static void FillWithTestData(this ApplicationDbContext dbContext)
    {
        // ////////////////////////////// Add users
        var mainUsergroupAdmin = new User(
            id: MainUsergroupAdminId,
            firstName: "admin",
            secondName: "admin",
            patronymic: "admin"
        ); 
        dbContext.Users.Add(mainUsergroupAdmin);
        
        var usualUser_1 = new User(
            id: UsualUser_1_Id,
            firstName: "usual user 1",
            secondName: "usual user 1",
            patronymic: "usual user 1"
        ); 
        dbContext.Users.Add(usualUser_1);
        
        // ////////////////////////////// Add usergroups
        // //// Create member rights
        var mainUsergroupRights = new GroupMemberRights();

        // //// Create usergroups
        var mainUsergroup = new UserGroup(
            id: MainUsergroupId, 
            name: "main usergroup", 
            adminId: MainUsergroupAdminId, 
            memberRights: mainUsergroupRights,
            []
        );
        dbContext.UserGroups.Add(mainUsergroup);
        
        // //// Add members
        mainUsergroupRights.Add(new SingleMemberRights(mainUsergroupAdmin, mainUsergroup));
        mainUsergroupRights.Add(new SingleMemberRights(usualUser_1, mainUsergroup));
        foreach (var mainUsergroupRight in mainUsergroupRights) 
            dbContext.MemberRights.Add(mainUsergroupRight);
        
        // ////////////////////////////// Add announcements
        var announcement = new Announcement(
            id: AnnouncementId,
            content: "content",
            author: mainUsergroupAdmin,
            categories: [],
            audience: [mainUsergroup],
            publishedAt: DateTime.Now,
            hiddenAt: null,
            autoPublishingAt: null,
            autoHidingAt: null,
            attachments: []
        );
        dbContext.Announcements.Add(announcement);
        
                // ////////////////////////////// Add surveys
        // //// Create answers
        var answer_1_OfPublicSurvey = new Answer(Answer_1_OfPublicSurvey, Question_1_WithSingleChoice_OfPublicSurvey, "ответ 1 публичного опроса");
        var answer_2_OfPublicSurvey = new Answer(Answer_2_OfPublicSurvey, Question_1_WithSingleChoice_OfPublicSurvey, "ответ 2 публичного опроса");
        var answer_3_OfPublicSurvey = new Answer(Answer_3_OfPublicSurvey, Question_1_WithSingleChoice_OfPublicSurvey, "ответ 3 публичного опроса");
        var answer_4_OfPublicSurvey = new Answer(Answer_4_OfPublicSurvey, Question_1_WithSingleChoice_OfPublicSurvey, "ответ 4 публичного опроса");
        var answer_5_OfPublicSurvey = new Answer(Answer_5_OfPublicSurvey, Question_1_WithSingleChoice_OfPublicSurvey, "ответ 5 публичного опроса");
        var answer_6_OfPublicSurvey = new Answer(Answer_6_OfPublicSurvey, Question_1_WithSingleChoice_OfPublicSurvey, "ответ 6 публичного опроса");
        dbContext.Answers.Add(answer_1_OfPublicSurvey);
        dbContext.Answers.Add(answer_2_OfPublicSurvey);
        dbContext.Answers.Add(answer_3_OfPublicSurvey);
        dbContext.Answers.Add(answer_4_OfPublicSurvey);
        dbContext.Answers.Add(answer_5_OfPublicSurvey);
        dbContext.Answers.Add(answer_6_OfPublicSurvey);
        
        // //// Create questions
        var question_1_OfPublicSurvey = new Question(
            id: Question_1_WithSingleChoice_OfPublicSurvey, 
            surveyId: PublicSurveyId, 
            "вопрос 1 с единственным выбором публичного опроса", 
            isMultipleChoiceAllowed: false,
            answers: [
                answer_1_OfPublicSurvey,
                answer_2_OfPublicSurvey,
                answer_3_OfPublicSurvey,
            ]
        );
        dbContext.Questions.Add(question_1_OfPublicSurvey);
        
        var question_2_OfPublicSurvey = new Question(
            id: Question_2_WithMultipleChoice_OfPublicSurvey, 
            surveyId: PublicSurveyId, 
            "вопрос 2 с множественным выбором публичного опроса", 
            isMultipleChoiceAllowed: true,
            answers: [
                answer_4_OfPublicSurvey,
                answer_5_OfPublicSurvey,
                answer_6_OfPublicSurvey,
            ]
        );
        dbContext.Questions.Add(question_2_OfPublicSurvey);
        
        // //// Create surveys
        var survey = new Survey(
            id: PublicSurveyId,
            announcements: [announcement],
            isOpen: true,
            isAnonymous: false,
            autoClosingAt: null,
            questions: [question_1_OfPublicSurvey, question_2_OfPublicSurvey]
        );
        announcement.Attach(survey);
        dbContext.Surveys.Add(survey);
        
        dbContext.SaveChanges();
    }
}