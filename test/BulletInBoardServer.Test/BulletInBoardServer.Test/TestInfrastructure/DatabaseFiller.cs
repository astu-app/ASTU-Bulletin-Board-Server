using BulletInBoardServer.DataAccess;
using BulletInBoardServer.Models.Announcements;
using BulletInBoardServer.Models.Attachments.Surveys;
using BulletInBoardServer.Models.Attachments.Surveys.Answers;
using BulletInBoardServer.Models.Attachments.Surveys.Questions;
using BulletInBoardServer.Models.UserGroups;
using BulletInBoardServer.Models.Users;
using static BulletInBoardServer.Test.TestInfrastructure.TestDataIds;
// ReSharper disable InconsistentNaming

namespace BulletInBoardServer.Test.TestInfrastructure;

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
        
        #region Создание объявления с публичным опросом
        // ////////////////////////////// Add announcements
        var announcementWithPublicSurvey = new Announcement(
            id: AnnouncementWithAnonymousSurveyId,
            content: "Объявление с публичным опросом",
            author: mainUsergroupAdmin,
            categories: [],
            audience: [mainUsergroupAdmin, usualUser_1],
            publishedAt: DateTime.Now,
            hiddenAt: null,
            autoPublishingAt: null,
            autoHidingAt: null,
            attachments: []
        );
        dbContext.Announcements.Add(announcementWithPublicSurvey);
        
        // ////////////////////////////// Add surveys
        // //// Create answers
        var answer_1_OfPublicSurvey = new Answer(Answer_1_OfPublicSurvey, Question_1_WithSingleChoice_OfPublicSurvey, "ответ 1 вопроса 1 публичного опроса");
        var answer_2_OfPublicSurvey = new Answer(Answer_2_OfPublicSurvey, Question_1_WithSingleChoice_OfPublicSurvey, "ответ 2 вопроса 1 публичного опроса");
        var answer_3_OfPublicSurvey = new Answer(Answer_3_OfPublicSurvey, Question_1_WithSingleChoice_OfPublicSurvey, "ответ 3 вопроса 1 публичного опроса");
        var answer_4_OfPublicSurvey = new Answer(Answer_4_OfPublicSurvey, Question_2_WithMultipleChoice_OfPublicSurvey, "ответ 4 вопроса 2 публичного опроса");
        var answer_5_OfPublicSurvey = new Answer(Answer_5_OfPublicSurvey, Question_2_WithMultipleChoice_OfPublicSurvey, "ответ 5 вопроса 2 публичного опроса");
        var answer_6_OfPublicSurvey = new Answer(Answer_6_OfPublicSurvey, Question_2_WithMultipleChoice_OfPublicSurvey, "ответ 6 вопроса 2 публичного опроса");
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
        var publicSurvey = new Survey(
            id: PublicSurveyId,
            announcements: [announcementWithPublicSurvey],
            isOpen: true,
            isAnonymous: false,
            autoClosingAt: null,
            questions: [question_1_OfPublicSurvey, question_2_OfPublicSurvey]
        );
        announcementWithPublicSurvey.Attach(publicSurvey);
        dbContext.Surveys.Add(publicSurvey);
        #endregion Создание объявления с публичным опросом
        
        
        
        #region Создание объявления с анонимным опросом
        // ////////////////////////////// Add announcements
        var announcementWithAnonymousSurvey = new Announcement(
            id: AnnouncementWithPublicSurveyId,
            content: "Объявление с анонимным опросом",
            author: mainUsergroupAdmin,
            categories: [],
            audience: [mainUsergroupAdmin, usualUser_1],
            publishedAt: DateTime.Now,
            hiddenAt: null,
            autoPublishingAt: null,
            autoHidingAt: null,
            attachments: []
        );
        dbContext.Announcements.Add(announcementWithAnonymousSurvey);
        
        // ////////////////////////////// Add surveys
        // //// Create answers
        var answer_1_OfAnonymousSurvey = new Answer(Answer_1_OfAnonymousSurvey, Question_1_WithSingleChoice_OfAnonymousSurvey, "ответ 1 вопроса 1 анонимного опроса");
        var answer_2_OfAnonymousSurvey = new Answer(Answer_2_OfAnonymousSurvey, Question_1_WithSingleChoice_OfAnonymousSurvey, "ответ 2 вопроса 1 анонимного опроса");
        var answer_3_OfAnonymousSurvey = new Answer(Answer_3_OfAnonymousSurvey, Question_1_WithSingleChoice_OfAnonymousSurvey, "ответ 3 вопроса 1 анонимного опроса");
        var answer_4_OfAnonymousSurvey = new Answer(Answer_4_OfAnonymousSurvey, Question_2_WithMultipleChoice_OfAnonymousSurvey, "ответ 4 вопроса 2 анонимного опроса");
        var answer_5_OfAnonymousSurvey = new Answer(Answer_5_OfAnonymousSurvey, Question_2_WithMultipleChoice_OfAnonymousSurvey, "ответ 5 вопроса 2 анонимного опроса");
        var answer_6_OfAnonymousSurvey = new Answer(Answer_6_OfAnonymousSurvey, Question_2_WithMultipleChoice_OfAnonymousSurvey, "ответ 6 вопроса 2 анонимного опроса");
        dbContext.Answers.Add(answer_1_OfAnonymousSurvey);
        dbContext.Answers.Add(answer_2_OfAnonymousSurvey);
        dbContext.Answers.Add(answer_3_OfAnonymousSurvey);
        dbContext.Answers.Add(answer_4_OfAnonymousSurvey);
        dbContext.Answers.Add(answer_5_OfAnonymousSurvey);
        dbContext.Answers.Add(answer_6_OfAnonymousSurvey);
        
        // //// Create questions
        var question_1_OfAnonymousSurvey = new Question(
            id: Question_1_WithSingleChoice_OfAnonymousSurvey, 
            surveyId: AnonymousSurveyId, 
            "вопрос 1 с единственным выбором анонимного опроса", 
            isMultipleChoiceAllowed: false,
            answers: [
                answer_1_OfAnonymousSurvey,
                answer_2_OfAnonymousSurvey,
                answer_3_OfAnonymousSurvey,
            ]
        );
        dbContext.Questions.Add(question_1_OfAnonymousSurvey);
        
        var question_2_OfAnonymousSurvey = new Question(
            id: Question_2_WithMultipleChoice_OfAnonymousSurvey, 
            surveyId: AnonymousSurveyId, 
            "вопрос 2 с множественным выбором анонимного опроса", 
            isMultipleChoiceAllowed: true,
            answers: [
                answer_4_OfAnonymousSurvey,
                answer_5_OfAnonymousSurvey,
                answer_6_OfAnonymousSurvey,
            ]
        );
        dbContext.Questions.Add(question_2_OfPublicSurvey);
        
        // //// Create surveys
        var anonymousSurvey = new Survey(
            id: AnonymousSurveyId,
            announcements: [announcementWithAnonymousSurvey],
            isOpen: true,
            isAnonymous: true,
            autoClosingAt: null,
            questions: [question_1_OfAnonymousSurvey, question_2_OfAnonymousSurvey]
        );
        announcementWithAnonymousSurvey.Attach(anonymousSurvey);
        dbContext.Surveys.Add(anonymousSurvey);
        #endregion Создание объявления с анонимным опросом
        
        
        
        #region Создание объявления с закрытыманонимным опросом
        // ////////////////////////////// Add announcements
        var announcementWithClosedPublicSurvey = new Announcement(
            id: AnnouncementWithClosedAnonymousSurveyId,
            content: "Объявление с закрытым анонимным опросом",
            author: mainUsergroupAdmin,
            categories: [],
            audience: [mainUsergroupAdmin, usualUser_1],
            publishedAt: DateTime.Now,
            hiddenAt: null,
            autoPublishingAt: null,
            autoHidingAt: null,
            attachments: []
        );
        dbContext.Announcements.Add(announcementWithClosedPublicSurvey);
        
        // ////////////////////////////// Add surveys
        // //// Create answers
        var answer_1_OfClosedAnonymousSurvey = new Answer(Answer_1_OfClosedAnonymousSurvey, Question_1_WithSingleChoice_OfClosedAnonymousSurvey, "ответ 1 вопроса 1 закрытого анонимного опроса");
        var answer_2_OfClosedAnonymousSurvey = new Answer(Answer_2_OfClosedAnonymousSurvey, Question_1_WithSingleChoice_OfClosedAnonymousSurvey, "ответ 2 вопроса 1 закрытого анонимного опроса");
        dbContext.Answers.Add(answer_1_OfClosedAnonymousSurvey);
        dbContext.Answers.Add(answer_2_OfClosedAnonymousSurvey);
        
        // //// Create questions
        var question_1_OfClosedAnonymousSurvey = new Question(
            id: Question_1_WithSingleChoice_OfClosedAnonymousSurvey, 
            surveyId: ClosedAnonymousSurveyId, 
            "вопрос 1 с единственным выбором закрытого анонимного опроса", 
            isMultipleChoiceAllowed: false,
            answers: [
                answer_1_OfClosedAnonymousSurvey,
                answer_2_OfClosedAnonymousSurvey,
            ]
        );
        dbContext.Questions.Add(question_1_OfClosedAnonymousSurvey);
        
        // //// Create surveys
        var closedAnonymousSurvey = new Survey(
            id: ClosedAnonymousSurveyId,
            announcements: [announcementWithClosedPublicSurvey],
            isOpen: false,
            isAnonymous: true,
            autoClosingAt: null,
            questions: [question_1_OfClosedAnonymousSurvey]
        );
        announcementWithClosedPublicSurvey.Attach(closedAnonymousSurvey);
        dbContext.Surveys.Add(closedAnonymousSurvey);
        #endregion Создание объявления с закрытым анонимным опросом
        
        
        
        dbContext.SaveChanges();
    }
}