﻿using BulletInBoardServer.DataAccess;
using BulletInBoardServer.Models.AnnouncementCategories;
using BulletInBoardServer.Models.Announcements;
using BulletInBoardServer.Models.Attachments.Surveys;
using BulletInBoardServer.Models.Attachments.Surveys.Answers;
using BulletInBoardServer.Models.Attachments.Surveys.Questions;
using BulletInBoardServer.Models.UserGroups;
using BulletInBoardServer.Models.Users;
using static BulletInBoardServer.Test.Infrastructure.DbInvolvingTests.TestDataIds;
using File = BulletInBoardServer.Models.Attachments.File;

// ReSharper disable InconsistentNaming

namespace BulletInBoardServer.Test.Infrastructure.DbInvolvingTests;

public class DatabaseFiller(ApplicationDbContext dbContext)
{
    private readonly Dictionary<Guid, object> _dbEntities = [];
    
        
        
    public void FillWithTestData()
    {
        AddUsers();
        AddUserGroups();
        
        AddSurveys();
        AddFiles();
        AddAnnouncementCategories();
        AddAnnouncements();
        
        dbContext.SaveChanges();
    }



    private void AddDbEntity<T>(Guid id, T entity) where T : notnull =>
        _dbEntities[id] = entity;

    private T GetDbEntity<T>(Guid id) =>
        (T) _dbEntities[id];

    /// <summary>
    /// Добавление пользователей
    /// </summary>
    private void AddUsers()
    {
        var mainUsergroupAdmin = new User(
            id: MainUsergroupAdminId,
            firstName: "admin",
            secondName: "admin",
            patronymic: "admin"
        );
        dbContext.Users.Add(mainUsergroupAdmin);
        AddDbEntity(mainUsergroupAdmin.Id, mainUsergroupAdmin);

        var usualUser_1 = new User(
            id: UsualUser_1_Id,
            firstName: "usual user 1",
            secondName: "usual user 1",
            patronymic: "usual user 1"
        );
        dbContext.Users.Add(usualUser_1);
        AddDbEntity(usualUser_1.Id, usualUser_1);
        
        var usualUser_2 = new User(
            id: UsualUser_2_Id,
            firstName: "usual user 2",
            secondName: "usual user 2",
            patronymic: "usual user 2"
        );
        dbContext.Users.Add(usualUser_2);
        AddDbEntity(usualUser_2.Id, usualUser_2);
    }

    /// <summary>
    /// Добавление групп пользователей
    /// </summary>
    private void AddUserGroups()
    {
        // Create member rights
        var mainUsergroupRights = new GroupMemberRights();

        // Create usergroups
        var mainUsergroup = new UserGroup(
            id: MainUsergroupId, 
            name: "main usergroup", 
            adminId: MainUsergroupAdminId, 
            memberRights: mainUsergroupRights,
            childrenGroups: []
        );
        dbContext.UserGroups.Add(mainUsergroup);
        AddDbEntity(mainUsergroup.Id, mainUsergroup);
        
        // Add members
        var mainUserGroupAdminRights = new SingleMemberRights(GetDbEntity<User>(MainUsergroupAdminId), mainUsergroup);
        var usualUser_1_Rights = new SingleMemberRights(GetDbEntity<User>(UsualUser_1_Id), mainUsergroup);
        mainUsergroupRights.Add(mainUserGroupAdminRights);
        mainUsergroupRights.Add(usualUser_1_Rights);
        dbContext.MemberRights.Add(mainUserGroupAdminRights);
        dbContext.MemberRights.Add(usualUser_1_Rights);
    }

    /// <summary>
    /// Добавление опросов со связанными вопросами и вариантами ответов
    /// </summary>
    private void AddSurveys()
    {
        AddPublicSurvey_1();
        AddAnonymousSurvey_1();
        AddClosedAnonymousSurvey_1();
        AddPublicSurvey_2();
        AddSurveyExpectsAutoClosing();
        return;



        void AddPublicSurvey_1()
        {
            // Create answers
            var answer_1 = new Answer(Answer_1_OfPublicSurvey_1_Id, Question_1_WithSingleChoice_OfPublicSurvey_1_Id, "ответ 1 вопроса 1 публичного опроса 1");
            var answer_2 = new Answer(Answer_2_OfPublicSurvey_1_Id, Question_1_WithSingleChoice_OfPublicSurvey_1_Id, "ответ 2 вопроса 1 публичного опроса 1");
            var answer_3 = new Answer(Answer_3_OfPublicSurvey_1_Id, Question_1_WithSingleChoice_OfPublicSurvey_1_Id, "ответ 3 вопроса 1 публичного опроса 1");
            var answer_4 = new Answer(Answer_4_OfPublicSurvey_1_Id, Question_2_WithMultipleChoice_OfPublicSurvey_1_Id, "ответ 4 вопроса 2 публичного опроса 1");
            var answer_5 = new Answer(Answer_5_OfPublicSurvey_1_Id, Question_2_WithMultipleChoice_OfPublicSurvey_1_Id, "ответ 5 вопроса 2 публичного опроса 1");
            var answer_6 = new Answer(Answer_6_OfPublicSurvey_1_Id, Question_2_WithMultipleChoice_OfPublicSurvey_1_Id, "ответ 6 вопроса 2 публичного опроса 1");
            dbContext.Answers.Add(answer_1);
            dbContext.Answers.Add(answer_2);
            dbContext.Answers.Add(answer_3);
            dbContext.Answers.Add(answer_4);
            dbContext.Answers.Add(answer_5);
            dbContext.Answers.Add(answer_6);
            AddDbEntity(answer_1.Id, answer_1);
            AddDbEntity(answer_2.Id, answer_2);
            AddDbEntity(answer_3.Id, answer_3);
            AddDbEntity(answer_4.Id, answer_4);
            AddDbEntity(answer_5.Id, answer_5);
            AddDbEntity(answer_6.Id, answer_6);

            // Create questions
            var question_1 = new Question(
                id: Question_1_WithSingleChoice_OfPublicSurvey_1_Id,
                surveyId: PublicSurvey_1_Id,
                "вопрос 1 с единственным выбором публичного опроса 1",
                isMultipleChoiceAllowed: false,
                answers:
                [
                    answer_1,
                    answer_2,
                    answer_3,
                ]
            );
            dbContext.Questions.Add(question_1);
            AddDbEntity(question_1.Id, question_1);

            var question_2 = new Question(
                id: Question_2_WithMultipleChoice_OfPublicSurvey_1_Id,
                surveyId: PublicSurvey_1_Id,
                "вопрос 2 с множественным выбором публичного опроса 1",
                isMultipleChoiceAllowed: true,
                answers:
                [
                    answer_4,
                    answer_5,
                    answer_6,
                ]
            );
            dbContext.Questions.Add(question_2);
            AddDbEntity(question_2.Id, question_2);

            // Create survey
            var survey = new Survey(
                id: PublicSurvey_1_Id,
                announcements: [],
                isOpen: true,
                isAnonymous: false,
                autoClosingAt: null,
                questions: [question_1, question_2]
            );
            dbContext.Surveys.Add(survey);
            AddDbEntity(survey.Id, survey);
        }

        void AddAnonymousSurvey_1()
        {
            // Create answers
            var answer_1 = new Answer(Answer_1_OfAnonymousSurvey_1_Id, Question_1_WithSingleChoice_OfAnonymousSurvey_1_Id, "ответ 1 вопроса 1 анонимного опроса");
            var answer_2 = new Answer(Answer_2_OfAnonymousSurvey_1_Id, Question_1_WithSingleChoice_OfAnonymousSurvey_1_Id, "ответ 2 вопроса 1 анонимного опроса");
            var answer_3 = new Answer(Answer_3_OfAnonymousSurvey_1_Id, Question_1_WithSingleChoice_OfAnonymousSurvey_1_Id, "ответ 3 вопроса 1 анонимного опроса");
            var answer_4 = new Answer(Answer_4_OfAnonymousSurvey_1_Id, Question_2_WithMultipleChoice_OfAnonymousSurvey_1_Id, "ответ 4 вопроса 2 анонимного опроса");
            var answer_5 = new Answer(Answer_5_OfAnonymousSurvey_1_Id, Question_2_WithMultipleChoice_OfAnonymousSurvey_1_Id, "ответ 5 вопроса 2 анонимного опроса");
            var answer_6 = new Answer(Answer_6_OfAnonymousSurvey_1_Id, Question_2_WithMultipleChoice_OfAnonymousSurvey_1_Id, "ответ 6 вопроса 2 анонимного опроса");
            dbContext.Answers.Add(answer_1);
            dbContext.Answers.Add(answer_2);
            dbContext.Answers.Add(answer_3);
            dbContext.Answers.Add(answer_4);
            dbContext.Answers.Add(answer_5);
            dbContext.Answers.Add(answer_6);
            AddDbEntity(answer_1.Id, answer_1);
            AddDbEntity(answer_2.Id, answer_2);
            AddDbEntity(answer_3.Id, answer_3);
            AddDbEntity(answer_4.Id, answer_4);
            AddDbEntity(answer_5.Id, answer_5);
            AddDbEntity(answer_6.Id, answer_6);

            // Create questions
            var question_1 = new Question(
                id: Question_1_WithSingleChoice_OfAnonymousSurvey_1_Id,
                surveyId: AnonymousSurvey_1_Id,
                "вопрос 1 с единственным выбором анонимного опроса",
                isMultipleChoiceAllowed: false,
                answers:
                [
                    answer_1,
                    answer_2,
                    answer_3,
                ]
            );
            dbContext.Questions.Add(question_1);
            AddDbEntity(question_1.Id, question_1);

            var question_2 = new Question(
                id: Question_2_WithMultipleChoice_OfAnonymousSurvey_1_Id,
                surveyId: AnonymousSurvey_1_Id,
                "вопрос 2 с множественным выбором анонимного опроса",
                isMultipleChoiceAllowed: true,
                answers:
                [
                    answer_4,
                    answer_5,
                    answer_6,
                ]
            );
            dbContext.Questions.Add(question_2);
            AddDbEntity(question_2.Id, question_2);

            // Create survey
            var survey = new Survey(
                id: AnonymousSurvey_1_Id,
                announcements: [],
                isOpen: true,
                isAnonymous: true,
                autoClosingAt: null,
                questions: [question_1, question_2]
            );
            dbContext.Surveys.Add(survey);
            AddDbEntity(survey.Id, survey);
        }

        void AddClosedAnonymousSurvey_1()
        {
            // Create answers
            var answer_1 = new Answer(Answer_1_OfClosedAnonymousSurvey_1_Id, Question_1_WithSingleChoice_OfClosedAnonymousSurvey_1_Id, "ответ 1 вопроса 1 закрытого анонимного опроса");
            var answer_2 = new Answer(Answer_2_OfClosedAnonymousSurvey_1_Id, Question_1_WithSingleChoice_OfClosedAnonymousSurvey_1_Id, "ответ 2 вопроса 1 закрытого анонимного опроса");
            dbContext.Answers.Add(answer_1);
            dbContext.Answers.Add(answer_2);
            AddDbEntity(answer_1.Id, answer_1);
            AddDbEntity(answer_2.Id, answer_2);

            // Create questions
            var question_1 = new Question(
                id: Question_1_WithSingleChoice_OfClosedAnonymousSurvey_1_Id,
                surveyId: ClosedAnonymousSurvey_1_Id,
                "вопрос 1 с единственным выбором закрытого анонимного опроса",
                isMultipleChoiceAllowed: false,
                answers:
                [
                    answer_1,
                    answer_2,
                ]
            );
            dbContext.Questions.Add(question_1);
            AddDbEntity(question_1.Id, question_1);

            // Create survey
            var survey = new Survey(
                id: ClosedAnonymousSurvey_1_Id,
                announcements: [],
                isOpen: false,
                isAnonymous: true,
                autoClosingAt: null,
                questions: [question_1]
            );
            dbContext.Surveys.Add(survey);
            AddDbEntity(survey.Id, survey);
        }

        void AddPublicSurvey_2()
        {
            // Create answers
            var answer_1 = new Answer(Answer_1_OfPublicSurvey_2_Id, Question_1_WithSingleChoice_OfPublicSurvey_2_Id, "ответ 1 вопроса 1 публичного опроса 1");
            var answer_2 = new Answer(Answer_2_OfPublicSurvey_2_Id, Question_1_WithSingleChoice_OfPublicSurvey_2_Id, "ответ 2 вопроса 1 публичного опроса 1");
            dbContext.Answers.Add(answer_1);
            dbContext.Answers.Add(answer_2);
            AddDbEntity(answer_1.Id, answer_1);
            AddDbEntity(answer_2.Id, answer_2);

            // Create questions
            var question_1 = new Question(
                id: Question_1_WithSingleChoice_OfPublicSurvey_2_Id,
                surveyId: PublicSurvey_2_Id,
                "вопрос 1 с единственным выбором публичного опроса 2",
                isMultipleChoiceAllowed: false,
                answers: [answer_1, answer_2,]
            );
            dbContext.Questions.Add(question_1);
            AddDbEntity(question_1.Id, question_1);
            
            // Create survey
            var survey = new Survey(
                id: PublicSurvey_2_Id,
                announcements: [],
                isOpen: true,
                isAnonymous: false,
                autoClosingAt: null,
                questions: [question_1]
            );
            dbContext.Surveys.Add(survey);
            AddDbEntity(survey.Id, survey);
        }

        void AddSurveyExpectsAutoClosing()
        {
            // Create answers
            var answer_1 = new Answer(Answer_1_OfSurveyExpectsAutoClosing_Id, Question_1_WithSingleChoice_OfSurveyExpectsAutoClosing_Id, "ответ 1 вопроса, ожидающего автоматическое закрытие");
            var answer_2 = new Answer(Answer_2_OfSurveyExpectsAutoClosing_Id, Question_1_WithSingleChoice_OfSurveyExpectsAutoClosing_Id, "ответ 2 вопроса, ожидающего автоматическое закрытие");
            dbContext.Answers.Add(answer_1);
            dbContext.Answers.Add(answer_2);
            AddDbEntity(answer_1.Id, answer_1);
            AddDbEntity(answer_2.Id, answer_2);

            // Create questions
            var question_1 = new Question(
                id: Question_1_WithSingleChoice_OfSurveyExpectsAutoClosing_Id,
                surveyId: PublicSurvey_2_Id,
                "вопрос 1 опроса, ожидающего автоматическое закрытие",
                isMultipleChoiceAllowed: false,
                answers: [answer_1, answer_2,]
            );
            dbContext.Questions.Add(question_1);
            AddDbEntity(question_1.Id, question_1);
            
            // Create survey
            var survey = new Survey(
                id: SurveyExpectsAutoClosingId,
                announcements: [],
                isOpen: true,
                isAnonymous: false,
                autoClosingAt: DateTime.Now.AddHours(12), 
                questions: [question_1]
            );
            dbContext.Surveys.Add(survey);
            AddDbEntity(survey.Id, survey);
        }
    }

    /// <summary>
    /// Добавление файлов
    /// </summary>
    private void AddFiles()
    {
        var file_1 = new File(File_1_Id, MainUsergroupAdminId, "file 1", "file 1 hash", 0);
        dbContext.Files.Add(file_1);
        AddDbEntity(file_1.Id, file_1);
        
        var file_2 = new File(File_2_Id, MainUsergroupAdminId, "file 2", "file 2 hash", 0);
        dbContext.Files.Add(file_2);
        AddDbEntity(file_2.Id, file_2);
    }

    /// <summary>
    /// Добавление категорий объявлений
    /// </summary>
    private void AddAnnouncementCategories()
    {
        var announcementCategory_1 = new AnnouncementCategory(AnnouncementCategory_1_Id, "Категория 1", "#FFFFFF");
        dbContext.AnnouncementCategories.Add(announcementCategory_1);
        AddDbEntity(announcementCategory_1.Id, announcementCategory_1);
        
        var announcementCategory_2 = new AnnouncementCategory(AnnouncementCategory_2_Id, "Категория 2", "#FFFFFF");
        dbContext.AnnouncementCategories.Add(announcementCategory_2);
        AddDbEntity(announcementCategory_2.Id, announcementCategory_2);
        
        var announcementCategory_3 = new AnnouncementCategory(AnnouncementCategory_3_Id, "Категория 3", "#FFFFFF");
        dbContext.AnnouncementCategories.Add(announcementCategory_3);
        AddDbEntity(announcementCategory_3.Id, announcementCategory_3);
    }

    /// <summary>
    /// Добавление объявлений
    /// </summary>
    private void AddAnnouncements()
    {
        var mainUsergroupAdmin = GetDbEntity<User>(MainUsergroupAdminId);
        var usualUser_1 = GetDbEntity<User>(UsualUser_1_Id);

        AddAnnouncementWithPublicSurvey_1();
        AddAnnouncementWithAnonymousSurvey_1();
        AddAnnouncementWithClosedAnonymousSurvey_1();
        AddFullyFilledAnnouncement_1();
        AddAnnouncementWithFilledDelayedMoments();
        AddHiddenAnnouncementWithDisabledDelayedMoments();
        AddHiddenAnnouncementWithEnabledDelayedMoments();
        AddPublishedAnnouncementWithEnabledDelayedHiding();
        AddPublishedAnnouncementWithDisabledDelayedHiding();
        return;


        void AddAnnouncementWithPublicSurvey_1()
        {
            var announcement = new Announcement(
                id: AnnouncementWithPublicSurvey_1_Id,
                content: "Объявление с публичным опросом",
                author: mainUsergroupAdmin,
                categories: [],
                audience: [mainUsergroupAdmin, usualUser_1],
                publishedAt: DateTime.Now,
                hiddenAt: null,
                delayedPublishingAt: null,
                delayedHidingAt: null,
                attachments: [GetDbEntity<Survey>(PublicSurvey_1_Id)]
            );
            dbContext.Announcements.Add(announcement);
            AddDbEntity(announcement.Id, announcement);
        }

        void AddAnnouncementWithAnonymousSurvey_1()
        {
            var announcement = new Announcement(
                id: AnnouncementWithAnonymousSurvey_1_Id,
                content: "Объявление с анонимным опросом",
                author: mainUsergroupAdmin,
                categories: [],
                audience: [mainUsergroupAdmin, usualUser_1],
                publishedAt: DateTime.Now,
                hiddenAt: null,
                delayedPublishingAt: null,
                delayedHidingAt: null,
                attachments: [GetDbEntity<Survey>(AnonymousSurvey_1_Id)]
            );
            dbContext.Announcements.Add(announcement);
            AddDbEntity(announcement.Id, announcement);
        }

        void AddAnnouncementWithClosedAnonymousSurvey_1()
        {
            var announcement = new Announcement(
                id: AnnouncementWithClosedAnonymousSurvey_1_Id,
                content: "Объявление с закрытым анонимным опросом",
                author: mainUsergroupAdmin,
                categories: [],
                audience: [mainUsergroupAdmin, usualUser_1],
                publishedAt: DateTime.Now,
                hiddenAt: null,
                delayedPublishingAt: null,
                delayedHidingAt: null,
                attachments: [GetDbEntity<Survey>(ClosedAnonymousSurvey_1_Id)]
            );
            dbContext.Announcements.Add(announcement);
            AddDbEntity(announcement.Id, announcement);
        }

        void AddFullyFilledAnnouncement_1()
        {
            var announcement = new Announcement(
                id: FullyFilledAnnouncement_1_Id,
                content: "Полностью заполненное объявление 1",
                author: mainUsergroupAdmin,
                categories: [
                    GetDbEntity<AnnouncementCategory>(AnnouncementCategory_1_Id),
                    GetDbEntity<AnnouncementCategory>(AnnouncementCategory_2_Id),
                ],
                audience: [mainUsergroupAdmin, usualUser_1], 
                publishedAt: DateTime.Now.Subtract(TimeSpan.FromHours(3)),
                hiddenAt: DateTime.Now.Subtract(TimeSpan.FromHours(1)),
                delayedPublishingAt: null,
                delayedHidingAt: null,
                attachments: [
                    GetDbEntity<Survey>(PublicSurvey_2_Id),
                    GetDbEntity<File>(File_1_Id),
                ]
            );
            dbContext.Announcements.Add(announcement);
            AddDbEntity(announcement.Id, announcement);
        }

        void AddAnnouncementWithFilledDelayedMoments()
        {
            var announcement = new Announcement(
                id: AnnouncementWithFilledDelayedMomentsId,
                content: "Объявление с заполненными моментами отложенной публикации и отложенного сокрытия",
                author: mainUsergroupAdmin,
                categories: [],
                audience: [mainUsergroupAdmin, usualUser_1], 
                publishedAt: null,
                hiddenAt: null,
                delayedPublishingAt: DateTime.Now.AddDays(1),
                delayedHidingAt: DateTime.Now.AddDays(2),
                attachments: []
            );
            dbContext.Announcements.Add(announcement);
            AddDbEntity(announcement.Id, announcement);
        }
        
        void AddHiddenAnnouncementWithDisabledDelayedMoments()
        {
            var announcement = new Announcement(
                id: HiddenAnnouncementWithDisabledDelayedMomentsId,
                content: "Скрытое объявление с выключенными моментами отложенной публикации и отложенного сокрытия",
                author: mainUsergroupAdmin,
                categories: [],
                audience: [mainUsergroupAdmin, usualUser_1], 
                publishedAt: null,
                hiddenAt: DateTime.Now.Subtract(TimeSpan.FromHours(12)),
                delayedPublishingAt: null,
                delayedHidingAt: null,
                attachments: []
            );
            dbContext.Announcements.Add(announcement);
            AddDbEntity(announcement.Id, announcement);
        }

        void AddHiddenAnnouncementWithEnabledDelayedMoments()
        {
            var announcement = new Announcement(
                id: HiddenAnnouncementWithEnabledDelayedPublishingId,
                content: "Скрытое объявление с включенным моментом отложенной публикации",
                author: mainUsergroupAdmin,
                categories: [],
                audience: [mainUsergroupAdmin], 
                publishedAt: null,
                hiddenAt: DateTime.Now.Subtract(TimeSpan.FromHours(12)),
                delayedPublishingAt: DateTime.Now.AddDays(1),
                delayedHidingAt: null,
                attachments: []
            );
            dbContext.Announcements.Add(announcement);
            AddDbEntity(announcement.Id, announcement);
        }
        
        void AddPublishedAnnouncementWithEnabledDelayedHiding()
        {
            var announcement = new Announcement(
                id: PublishedAnnouncementWithEnabledDelayedHidingId,
                content: "Опубликованное объявление с заданным моментом отложенного сокрытия",
                author: mainUsergroupAdmin,
                categories: [],
                audience: [mainUsergroupAdmin], 
                publishedAt: DateTime.Now.Subtract(TimeSpan.FromHours(12)),
                hiddenAt: null,
                delayedPublishingAt: null,
                delayedHidingAt: DateTime.Now.AddDays(1),
                attachments: []
            );
            dbContext.Announcements.Add(announcement);
            AddDbEntity(announcement.Id, announcement);
        }

        void AddPublishedAnnouncementWithDisabledDelayedHiding()
        {
            var announcement = new Announcement(
                id: PublishedAnnouncementWithDisabledDelayedHidingId,
                content: "Опубликованное объявление  без заданного момента отложенного сокрытия",
                author: mainUsergroupAdmin,
                categories: [],
                audience: [mainUsergroupAdmin], 
                publishedAt: DateTime.Now.Subtract(TimeSpan.FromHours(12)),
                hiddenAt: null,
                delayedPublishingAt: null,
                delayedHidingAt: null,
                attachments: []
            );
            dbContext.Announcements.Add(announcement);
            AddDbEntity(announcement.Id, announcement);
        }
    }
}