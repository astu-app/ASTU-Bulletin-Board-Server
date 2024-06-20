using BulletInBoardServer.Domain.Models.Announcements;
using BulletInBoardServer.Domain.Models.Attachments.Surveys;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Answers;
using BulletInBoardServer.Domain.Models.Attachments.Surveys.Questions;
using BulletInBoardServer.Domain.Models.JoinEntities;
using BulletInBoardServer.Domain.Models.UserGroups;
using BulletInBoardServer.Domain.Models.Users;
using static BulletInBoardServer.Domain.DemoDbFiller.DemoDataIds;

// ReSharper disable InconsistentNaming

namespace BulletInBoardServer.Domain.DemoDbFiller;

public class DatabaseFiller(ApplicationDbContext dbContext)
{
    private readonly Dictionary<Guid, object> _dbEntities = [];



    public void FillWithTestData()
    {
        AddUsers();
        AddUserGroups();

        AddSurveys();
        AddAnnouncements();

        dbContext.SaveChanges();
    }



    private void AddDbEntity<T>(Guid id, T entity) where T : notnull =>
        _dbEntities[id] = entity;

    private T GetDbEntity<T>(Guid id) =>
        (T)_dbEntities[id];

    /// <summary>
    /// Добавление пользователей
    /// </summary>
    private void AddUsers()
    {
        AddRectoryMembers();
        AddViceRectorForAcademicAffairsMembers();
        AddInstituteOfInformationTechnologiesAndCommunicationsMembers();
        AddAutomatedInformationProcessingAndControlMembers();
        AddAutomatedInformationProcessingAndControl_Group1Members();
        AddAutomatedInformationProcessingAndControl_Group2Members();
        AddAppliedInformaticsMembers();
        AddAppliedInformatics_Group1Members();
        AddAppliedInformatics_Group2Members();
        return;
        
        
        
        // Ректорат
        void AddRectoryMembers()
        {
            // Админ
            AddMember(Rectory_AdminId, "Александр", "Неваленный", "Николаевич");
        }
        
        // Проректор по учебной работе
        void AddViceRectorForAcademicAffairsMembers()
        {
            // Админ
            AddMember(ViceRectorForAcademicAffairs_AdminId, "Ирина", "Квятковская", "Юрьевна");

            // Заместитель
            AddMember(ViceRectorForAcademicAffairsDeputy_UserId, "Софья", "Родина", "Ильинична");
        }
        
        // ИИТИК
        void AddInstituteOfInformationTechnologiesAndCommunicationsMembers() 
        {
            // Админ
            AddMember(InstituteOfInformationTechnologiesAndCommunications_AdminId, "Сергей", "Белов", "Валерьевич");

            // Обычные участники
            AddMember(InstituteOfInformationTechnologiesAndCommunications_Member1, "София", "Зубова", "Савельевна");
            AddMember(InstituteOfInformationTechnologiesAndCommunications_Member2, "Владислав", "Петров", "Степанович");
            AddMember(InstituteOfInformationTechnologiesAndCommunications_Member3, "Евгений", "Чернов", "Романович");
        }
        
        // Кафедра АСОИУ
        void AddAutomatedInformationProcessingAndControlMembers() 
        {
            // Админ
            AddMember(AutomatedInformationProcessingAndControl_AdminId, "Татьяна", "Хоменко", "Владимировна");

            // Обычные участники
            AddMember(AutomatedInformationProcessingAndControl_Member1, "Николай", "Куркурин", "Дмитриевич");
            AddMember(AutomatedInformationProcessingAndControl_Member2, "Аделя", "Мамлеева", "Рифкатовна");
            AddMember(AutomatedInformationProcessingAndControl_Member3, "Валерий", "Лаптев", "Викторович");
        }
        
        // АСОИУ - группа 1
        void AddAutomatedInformationProcessingAndControl_Group1Members() 
        {
            // Обычные участники
            AddMember(AutomatedInformationProcessingAndControl_Group1_Member1, "Олег", "Гаврилов", "Михайлович");
            AddMember(AutomatedInformationProcessingAndControl_Group1_Member2, "Дарья", "Устинова", "Максимовна");
            AddMember(AutomatedInformationProcessingAndControl_Group1_Member3, "Михаил", "Андреев", "Романович");
        }

        // АСОИУ - группа 2
        void AddAutomatedInformationProcessingAndControl_Group2Members() 
        {
            // Обычные участники
            AddMember(AutomatedInformationProcessingAndControl_Group2_Member1, "Егор", "Ершов", "Артёмович");
            AddMember(AutomatedInformationProcessingAndControl_Group2_Member2, "София", "Богданова", "Владимировна");
            AddMember(AutomatedInformationProcessingAndControl_Group2_Member3, "Анастасия", "Калачева", "Макаровна");
        }
        
        // Кафедра ПИ
        void AddAppliedInformaticsMembers() 
        {
            // Админ
            AddMember(AppliedInformatics_AdminId, "Ирина", "Бондарева", "Олеговна");

            // Обычные участники
            AddMember(AppliedInformatics_Member1, "Анна", "Ханова", "Алексеевна");
            AddMember(AppliedInformatics_Member2, "Ольга", "Еременко", "Олеговна");
            AddMember(AppliedInformatics_Member3, "Наталья", "Ганюкова", "Павловна");
        }
        
        // ПИ - группа 1
        void AddAppliedInformatics_Group1Members() 
        {
            // Обычные участники
            AddMember(AppliedInformatics_Group1_Member1, "Мария", "Литвинова", "Кирилловна");
            AddMember(AppliedInformatics_Group1_Member2, "Ульяна", "Власова", "Георгиевна");
            AddMember(AppliedInformatics_Group1_Member3, "Александр", "Косарев", "Артёмович");
        }
        
        // ПИ - группа 2
        void AddAppliedInformatics_Group2Members() 
        {
            // Обычные участники
            AddMember(AppliedInformatics_Group2_Member1, "Давид", "Денисов", "Михайлович");
            AddMember(AppliedInformatics_Group2_Member2, "Фёдор", "Мальцев", "Русланович");
            AddMember(AppliedInformatics_Group2_Member3, "Виктория", "Волкова", "Марковна");
        }
        
        void AddMember(Guid id, string firstName, string secondName, string? patronymic)
        {
            var user = new User(
                id: id,
                firstName: firstName,
                secondName: secondName,
                patronymic: patronymic
            );
            dbContext.Users.Add(user);
            AddDbEntity(user.Id, user);
        }
    }

    /// <summary>
    /// Добавление групп пользователей
    /// </summary>
    private void AddUserGroups()
    {
        // Ректорат
        AddRectory();
        // Проректор по учебной работе
        AddViceRectorForAcademicAffairs();
        // ИИТИК
        AddInstituteOfInformationTechnologiesAndCommunications();
        
        // АСОИУ
        AddAutomatedInformationProcessingAndControl();
        // АСОИУ группа 1
        AddAutomatedInformationProcessingAndControl_Group1();
        // АСОИУ группа 2
        AddAutomatedInformationProcessingAndControl_Group2();
        
        // ПИ
        AddAppliedInformatics();
        // ПИ группа 1
        AddAppliedInformatics_Group1();
        // ПИ группа 2
        AddAppliedInformatics_Group2();
        

        AddUserGroupConnections();
        return;


        void AddRectory()
        {
            var groupId = Rectory_GroupId;
            var adminId = Rectory_AdminId;
            var groupName = "Ректорат";
            
            // Create member rights
            var rights = new GroupMemberRights();

            // Create usergroup
            var usergroup = new UserGroup(
                id: groupId,
                name: groupName,
                adminId: adminId,
                memberRights: rights,
                childrenGroups: []
            );
            dbContext.UserGroups.Add(usergroup);
            AddDbEntity(usergroup.Id, usergroup);
        }
        
        void AddViceRectorForAcademicAffairs() 
        {
            var groupId = ViceRectorForAcademicAffairs_GroupId;
            var adminId = ViceRectorForAcademicAffairs_AdminId;
            var groupName = "Проректор по учебной работе";
            
            // Create member rights
            var rights = new GroupMemberRights();

            // Create usergroup
            var usergroup = new UserGroup(
                id: groupId,
                name: groupName,
                adminId: adminId,
                memberRights: rights,
                childrenGroups: []
            );
            dbContext.UserGroups.Add(usergroup);
            AddDbEntity(usergroup.Id, usergroup);

            // Add members
            var member_rights = new SingleMemberRights(GetDbEntity<User>(ViceRectorForAcademicAffairsDeputy_UserId), usergroup, true, true, true, true, true, true, true, true, true, true);
            rights.Add(member_rights);
            dbContext.MemberRights.Add(member_rights);
        }
        
        void AddInstituteOfInformationTechnologiesAndCommunications() 
        {
            var groupId = InstituteOfInformationTechnologiesAndCommunications_GroupId;
            var adminId = InstituteOfInformationTechnologiesAndCommunications_AdminId;
            var groupName = "ИИТИК";
            
            // Create member rights
            var rights = new GroupMemberRights();

            // Create usergroup
            var usergroup = new UserGroup(
                id: groupId,
                name: groupName,
                adminId: adminId,
                memberRights: rights,
                childrenGroups: []
            );
            dbContext.UserGroups.Add(usergroup);
            AddDbEntity(usergroup.Id, usergroup);

            // Add members
            // member 1
            var member_1_rights = new SingleMemberRights(GetDbEntity<User>(InstituteOfInformationTechnologiesAndCommunications_Member1), usergroup, true);
            rights.Add(member_1_rights);
            dbContext.MemberRights.Add(member_1_rights);
            // member 2
            var member_2_rights = new SingleMemberRights(GetDbEntity<User>(InstituteOfInformationTechnologiesAndCommunications_Member2), usergroup, true);
            rights.Add(member_2_rights);
            dbContext.MemberRights.Add(member_2_rights);
            // member 3
            var member_3_rights = new SingleMemberRights(GetDbEntity<User>(InstituteOfInformationTechnologiesAndCommunications_Member3), usergroup, true);
            rights.Add(member_3_rights);
            dbContext.MemberRights.Add(member_3_rights);
        }
        
        void AddAutomatedInformationProcessingAndControl() 
        {
            var groupId = AutomatedInformationProcessingAndControl_GroupId;
            var adminId = AutomatedInformationProcessingAndControl_AdminId;
            var groupName = "Кафедра АСОИУ";
            
            // Create member rights
            var rights = new GroupMemberRights();

            // Create usergroup
            var usergroup = new UserGroup(
                id: groupId,
                name: groupName,
                adminId: adminId,
                memberRights: rights,
                childrenGroups: []
            );
            dbContext.UserGroups.Add(usergroup);
            AddDbEntity(usergroup.Id, usergroup);

            // Add members
            // member 1
            var member_1_rights = new SingleMemberRights(GetDbEntity<User>(AutomatedInformationProcessingAndControl_Member1), usergroup, true);
            rights.Add(member_1_rights);
            dbContext.MemberRights.Add(member_1_rights);
            // member 2
            var member_2_rights = new SingleMemberRights(GetDbEntity<User>(AutomatedInformationProcessingAndControl_Member2), usergroup, true);
            rights.Add(member_2_rights);
            dbContext.MemberRights.Add(member_2_rights);
            // member 3
            var member_3_rights = new SingleMemberRights(GetDbEntity<User>(AutomatedInformationProcessingAndControl_Member3), usergroup, true);
            rights.Add(member_3_rights);
            dbContext.MemberRights.Add(member_3_rights);
        }
        
        void AddAutomatedInformationProcessingAndControl_Group1() 
        {
            var groupId = AutomatedInformationProcessingAndControl_Group1_GroupId;
            var adminId = (Guid?) null;
            var groupName = "ДИПРб-41";
            
            // Create member rights
            var rights = new GroupMemberRights();

            // Create usergroup
            var usergroup = new UserGroup(
                id: groupId,
                name: groupName,
                adminId: adminId,
                memberRights: rights,
                childrenGroups: []
            );
            dbContext.UserGroups.Add(usergroup);
            AddDbEntity(usergroup.Id, usergroup);

            // Add members
            // member 1
            var member_1_rights = new SingleMemberRights(GetDbEntity<User>(AutomatedInformationProcessingAndControl_Group1_Member1), usergroup, true);
            rights.Add(member_1_rights);
            dbContext.MemberRights.Add(member_1_rights);
            // member 2
            var member_2_rights = new SingleMemberRights(GetDbEntity<User>(AutomatedInformationProcessingAndControl_Group1_Member2), usergroup, true);
            rights.Add(member_2_rights);
            dbContext.MemberRights.Add(member_2_rights);
            // member 3
            var member_3_rights = new SingleMemberRights(GetDbEntity<User>(AutomatedInformationProcessingAndControl_Group1_Member3), usergroup, true);
            rights.Add(member_3_rights);
            dbContext.MemberRights.Add(member_3_rights);
        }
        
        void AddAutomatedInformationProcessingAndControl_Group2() 
        {
            var groupId = AutomatedInformationProcessingAndControl_Group2_GroupId;
            var adminId = (Guid?) null;
            var groupName = "ДИНРб-41";
            
            // Create member rights
            var rights = new GroupMemberRights();

            // Create usergroup
            var usergroup = new UserGroup(
                id: groupId,
                name: groupName,
                adminId: adminId,
                memberRights: rights,
                childrenGroups: []
            );
            dbContext.UserGroups.Add(usergroup);
            AddDbEntity(usergroup.Id, usergroup);

            // Add members
            // member 1
            var member_1_rights = new SingleMemberRights(GetDbEntity<User>(AutomatedInformationProcessingAndControl_Group2_Member1), usergroup, true);
            rights.Add(member_1_rights);
            dbContext.MemberRights.Add(member_1_rights);
            // member 2
            var member_2_rights = new SingleMemberRights(GetDbEntity<User>(AutomatedInformationProcessingAndControl_Group2_Member2), usergroup, true);
            rights.Add(member_2_rights);
            dbContext.MemberRights.Add(member_2_rights);
            // member 3
            var member_3_rights = new SingleMemberRights(GetDbEntity<User>(AutomatedInformationProcessingAndControl_Group2_Member3), usergroup, true);
            rights.Add(member_3_rights);
            dbContext.MemberRights.Add(member_3_rights);
        }
        
        void AddAppliedInformatics() 
        {
            var groupId = AppliedInformatics_GroupId;
            var adminId = AppliedInformatics_AdminId;
            var groupName = "Кафедра ПИ";
            
            // Create member rights
            var rights = new GroupMemberRights();

            // Create usergroup
            var usergroup = new UserGroup(
                id: groupId,
                name: groupName,
                adminId: adminId,
                memberRights: rights,
                childrenGroups: []
            );
            dbContext.UserGroups.Add(usergroup);
            AddDbEntity(usergroup.Id, usergroup);

            // Add members
            // member 1
            var member_1_rights = new SingleMemberRights(GetDbEntity<User>(AppliedInformatics_Member1), usergroup, true);
            rights.Add(member_1_rights);
            dbContext.MemberRights.Add(member_1_rights);
            // member 2
            var member_2_rights = new SingleMemberRights(GetDbEntity<User>(AppliedInformatics_Member2), usergroup, true);
            rights.Add(member_2_rights);
            dbContext.MemberRights.Add(member_2_rights);
            // member 3
            var member_3_rights = new SingleMemberRights(GetDbEntity<User>(AppliedInformatics_Member3), usergroup, true);
            rights.Add(member_3_rights);
            dbContext.MemberRights.Add(member_3_rights);
        }
        
        void AddAppliedInformatics_Group1() 
        {
            var groupId = AppliedInformatics_Group1_GroupId;
            var adminId = (Guid?) null;
            var groupName = "ДИИЭБ-41";
            
            // Create member rights
            var rights = new GroupMemberRights();

            // Create usergroup
            var usergroup = new UserGroup(
                id: groupId,
                name: groupName,
                adminId: adminId,
                memberRights: rights,
                childrenGroups: []
            );
            dbContext.UserGroups.Add(usergroup);
            AddDbEntity(usergroup.Id, usergroup);

            // Add members
            // member 1
            var member_1_rights = new SingleMemberRights(GetDbEntity<User>(AppliedInformatics_Group1_Member1), usergroup, true);
            rights.Add(member_1_rights);
            dbContext.MemberRights.Add(member_1_rights);
            // member 2
            var member_2_rights = new SingleMemberRights(GetDbEntity<User>(AppliedInformatics_Group1_Member2), usergroup, true);
            rights.Add(member_2_rights);
            dbContext.MemberRights.Add(member_2_rights);
            // member 3
            var member_3_rights = new SingleMemberRights(GetDbEntity<User>(AppliedInformatics_Group1_Member3), usergroup, true);
            rights.Add(member_3_rights);
            dbContext.MemberRights.Add(member_3_rights);
        }
        
        void AddAppliedInformatics_Group2() 
        {
            var groupId = AppliedInformatics_Group2_GroupId;
            var adminId = (Guid?) null;
            var groupName = "ДИИЭБ-41";
            
            // Create member rights
            var rights = new GroupMemberRights();

            // Create usergroup
            var usergroup = new UserGroup(
                id: groupId,
                name: groupName,
                adminId: adminId,
                memberRights: rights,
                childrenGroups: []
            );
            dbContext.UserGroups.Add(usergroup);
            AddDbEntity(usergroup.Id, usergroup);

            // Add members
            // member 1
            var member_1_rights = new SingleMemberRights(GetDbEntity<User>(AppliedInformatics_Group2_Member1), usergroup, true);
            rights.Add(member_1_rights);
            dbContext.MemberRights.Add(member_1_rights);
            // member 2
            var member_2_rights = new SingleMemberRights(GetDbEntity<User>(AppliedInformatics_Group2_Member2), usergroup, true);
            rights.Add(member_2_rights);
            dbContext.MemberRights.Add(member_2_rights);
            // member 3
            var member_3_rights = new SingleMemberRights(GetDbEntity<User>(AppliedInformatics_Group2_Member3), usergroup, true);
            rights.Add(member_3_rights);
            dbContext.MemberRights.Add(member_3_rights);
        }
        
        void AddUserGroupConnections()
        {
            var connections = new ChildUseGroup[]
            {
                new(Rectory_GroupId, ViceRectorForAcademicAffairs_GroupId),
                new(ViceRectorForAcademicAffairs_GroupId, InstituteOfInformationTechnologiesAndCommunications_GroupId),
                
                new(InstituteOfInformationTechnologiesAndCommunications_GroupId, AutomatedInformationProcessingAndControl_GroupId),
                new(AutomatedInformationProcessingAndControl_GroupId, AutomatedInformationProcessingAndControl_Group1_GroupId),
                new(AutomatedInformationProcessingAndControl_GroupId, AutomatedInformationProcessingAndControl_Group2_GroupId),
                
                new(InstituteOfInformationTechnologiesAndCommunications_GroupId, AppliedInformatics_GroupId),
                new(AppliedInformatics_GroupId, AppliedInformatics_Group1_GroupId),
                new(AppliedInformatics_GroupId, AppliedInformatics_Group2_GroupId),
            };
            dbContext.ChildUseGroups.AddRange(connections);
        }
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
            var answer_1 = new Answer(Answer_1_OfPublicSurvey_1_Id, 0,
                Question_1_WithSingleChoice_OfPublicSurvey_1_Id, "ответ 1 вопроса 1 публичного опроса 1");
            var answer_2 = new Answer(Answer_2_OfPublicSurvey_1_Id, 1,
                Question_1_WithSingleChoice_OfPublicSurvey_1_Id, "ответ 2 вопроса 1 публичного опроса 1");
            var answer_3 = new Answer(Answer_3_OfPublicSurvey_1_Id, 2,
                Question_1_WithSingleChoice_OfPublicSurvey_1_Id, "ответ 3 вопроса 1 публичного опроса 1");
            var answer_4 = new Answer(Answer_4_OfPublicSurvey_1_Id, 3,
                Question_2_WithMultipleChoice_OfPublicSurvey_1_Id, "ответ 4 вопроса 2 публичного опроса 1");
            var answer_5 = new Answer(Answer_5_OfPublicSurvey_1_Id, 4,
                Question_2_WithMultipleChoice_OfPublicSurvey_1_Id, "ответ 5 вопроса 2 публичного опроса 1");
            var answer_6 = new Answer(Answer_6_OfPublicSurvey_1_Id, 5,
                Question_2_WithMultipleChoice_OfPublicSurvey_1_Id, "ответ 6 вопроса 2 публичного опроса 1");
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
                serial: 0,
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
                serial: 1,
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
                resultsOpenBeforeClosing: true,
                autoClosingAt: null,
                questions: [question_1, question_2],
                voteFinishedAt: null
            );
            dbContext.Surveys.Add(survey);
            AddDbEntity(survey.Id, survey);
        }

        void AddAnonymousSurvey_1()
        {
            // Create answers
            var answer_1 = new Answer(Answer_1_OfAnonymousSurvey_1_Id, 0,
                Question_1_WithSingleChoice_OfAnonymousSurvey_1_Id, "ответ 1 вопроса 1 анонимного опроса");
            var answer_2 = new Answer(Answer_2_OfAnonymousSurvey_1_Id, 1,
                Question_1_WithSingleChoice_OfAnonymousSurvey_1_Id, "ответ 2 вопроса 1 анонимного опроса");
            var answer_3 = new Answer(Answer_3_OfAnonymousSurvey_1_Id, 2,
                Question_1_WithSingleChoice_OfAnonymousSurvey_1_Id, "ответ 3 вопроса 1 анонимного опроса");
            var answer_4 = new Answer(Answer_4_OfAnonymousSurvey_1_Id, 3,
                Question_2_WithMultipleChoice_OfAnonymousSurvey_1_Id, "ответ 4 вопроса 2 анонимного опроса");
            var answer_5 = new Answer(Answer_5_OfAnonymousSurvey_1_Id, 4,
                Question_2_WithMultipleChoice_OfAnonymousSurvey_1_Id, "ответ 5 вопроса 2 анонимного опроса");
            var answer_6 = new Answer(Answer_6_OfAnonymousSurvey_1_Id, 5,
                Question_2_WithMultipleChoice_OfAnonymousSurvey_1_Id, "ответ 6 вопроса 2 анонимного опроса");
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
                serial: 0,
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
                serial: 1,
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
                resultsOpenBeforeClosing: true,
                autoClosingAt: null,
                questions: [question_1, question_2],
                voteFinishedAt: null
            );
            dbContext.Surveys.Add(survey);
            AddDbEntity(survey.Id, survey);
        }

        void AddClosedAnonymousSurvey_1()
        {
            // Create answers
            var answer_1 = new Answer(Answer_1_OfClosedAnonymousSurvey_1_Id, 0,
                Question_1_WithSingleChoice_OfClosedAnonymousSurvey_1_Id,
                "ответ 1 вопроса 1 закрытого анонимного опроса");
            var answer_2 = new Answer(Answer_2_OfClosedAnonymousSurvey_1_Id, 1,
                Question_1_WithSingleChoice_OfClosedAnonymousSurvey_1_Id,
                "ответ 2 вопроса 1 закрытого анонимного опроса");
            dbContext.Answers.Add(answer_1);
            dbContext.Answers.Add(answer_2);
            AddDbEntity(answer_1.Id, answer_1);
            AddDbEntity(answer_2.Id, answer_2);

            // Create questions
            var question_1 = new Question(
                id: Question_1_WithSingleChoice_OfClosedAnonymousSurvey_1_Id,
                serial: 0,
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
                resultsOpenBeforeClosing: true,
                autoClosingAt: null,
                questions: [question_1],
                voteFinishedAt: null
            );
            dbContext.Surveys.Add(survey);
            AddDbEntity(survey.Id, survey);
        }

        void AddPublicSurvey_2()
        {
            // Create answers
            var answer_1 = new Answer(Answer_1_OfPublicSurvey_2_Id, 0,
                Question_1_WithSingleChoice_OfPublicSurvey_2_Id, "ответ 1 вопроса 1 публичного опроса 1");
            var answer_2 = new Answer(Answer_2_OfPublicSurvey_2_Id, 1,
                Question_1_WithSingleChoice_OfPublicSurvey_2_Id, "ответ 2 вопроса 1 публичного опроса 1");
            dbContext.Answers.Add(answer_1);
            dbContext.Answers.Add(answer_2);
            AddDbEntity(answer_1.Id, answer_1);
            AddDbEntity(answer_2.Id, answer_2);

            // Create questions
            var question_1 = new Question(
                id: Question_1_WithSingleChoice_OfPublicSurvey_2_Id,
                serial: 0,
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
                resultsOpenBeforeClosing: true,
                autoClosingAt: null,
                questions: [question_1],
                voteFinishedAt: null
            );
            dbContext.Surveys.Add(survey);
            AddDbEntity(survey.Id, survey);
        }

        void AddSurveyExpectsAutoClosing()
        {
            // Create answers
            var answer_1 = new Answer(Answer_1_OfSurveyExpectsAutoClosing_Id, 0,
                Question_1_WithSingleChoice_OfSurveyExpectsAutoClosing_Id,
                "ответ 1 вопроса, ожидающего автоматическое закрытие, результаты которого не доступны до закрытия");
            var answer_2 = new Answer(Answer_2_OfSurveyExpectsAutoClosing_Id, 1,
                Question_1_WithSingleChoice_OfSurveyExpectsAutoClosing_Id,
                "ответ 2 вопроса, ожидающего автоматическое закрытие, результаты которого не доступны до закрытия");
            dbContext.Answers.Add(answer_1);
            dbContext.Answers.Add(answer_2);
            AddDbEntity(answer_1.Id, answer_1);
            AddDbEntity(answer_2.Id, answer_2);

            // Create questions
            var question_1 = new Question(
                id: Question_1_WithSingleChoice_OfSurveyExpectsAutoClosing_Id,
                serial: 0,
                surveyId: PublicSurvey_2_Id,
                "вопрос 1 опроса, ожидающего автоматическое закрытие, результаты которого не доступны до закрытия",
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
                resultsOpenBeforeClosing: false,
                autoClosingAt: DateTime.Now.AddHours(12),
                questions: [question_1],
                voteFinishedAt: null
            );
            dbContext.Surveys.Add(survey);
            AddDbEntity(survey.Id, survey);
        }
    }

    /// <summary>
    /// Добавление объявлений
    /// </summary>
    private void AddAnnouncements()
    {
        // todo продумать нормальные объявления
        var mainUsergroupAdmin = GetDbEntity<User>(Rectory_AdminId); 
        var usualUser_1 = GetDbEntity<User>(ViceRectorForAcademicAffairs_AdminId);

        AddAnnouncementWithPublicSurvey_1();
        AddAnnouncementWithAnonymousSurvey_1();
        AddAnnouncementWithClosedAnonymousSurvey_1();
        AddFullyFilledAnnouncement_1();
        AddAnnouncementWithFilledDelayedMoments();
        AddHiddenAnnouncementWithDisabledDelayedMoments();
        AddHiddenAnnouncementWithEnabledDelayedMoments();
        AddPublishedAnnouncementWithEnabledDelayedHiding();
        AddPublishedAnnouncementWithDisabledDelayedHiding();
        AddAnnouncementWithSurveyExpectsAutoClosing();
        return;


        void AddAnnouncementWithPublicSurvey_1()
        {
            var announcement = new Announcement(
                id: AnnouncementWithPublicSurvey_1_Id,
                content: "Объявление с публичным опросом",
                author: mainUsergroupAdmin,
                audience: [mainUsergroupAdmin, usualUser_1],
                firstlyPublishedAt: DateTime.Now,
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
                audience: [mainUsergroupAdmin, usualUser_1],
                firstlyPublishedAt: DateTime.Now,
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
                audience: [mainUsergroupAdmin, usualUser_1],
                firstlyPublishedAt: DateTime.Now,
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
                audience: [mainUsergroupAdmin, usualUser_1],
                firstlyPublishedAt: DateTime.Now,
                publishedAt: null, // не может содержать другое значение, так как объявление скрыто
                hiddenAt: DateTime.Now.Subtract(TimeSpan.FromHours(1)),
                delayedPublishingAt: null,
                delayedHidingAt: null,
                attachments: [GetDbEntity<Survey>(PublicSurvey_2_Id)]
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
                audience: [mainUsergroupAdmin, usualUser_1],
                firstlyPublishedAt: null,
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
                content:
                "Скрытое объявление с выключенными моментами отложенной публикации и отложенного сокрытия",
                author: mainUsergroupAdmin,
                audience: [mainUsergroupAdmin, usualUser_1],
                firstlyPublishedAt: DateTime.Now, 
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
                audience: [mainUsergroupAdmin],
                firstlyPublishedAt: DateTime.Now,
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
                audience: [mainUsergroupAdmin],
                firstlyPublishedAt: DateTime.Now.Subtract(TimeSpan.FromHours(12)),
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
                content: "Опубликованное объявление без заданного момента отложенного сокрытия",
                author: mainUsergroupAdmin,
                audience: [mainUsergroupAdmin],
                firstlyPublishedAt: DateTime.Now.Subtract(TimeSpan.FromHours(12)),
                publishedAt: DateTime.Now.Subtract(TimeSpan.FromHours(12)),
                hiddenAt: null,
                delayedPublishingAt: null,
                delayedHidingAt: null,
                attachments: []
            );
            dbContext.Announcements.Add(announcement);
            AddDbEntity(announcement.Id, announcement);
        }
        
        void AddAnnouncementWithSurveyExpectsAutoClosing()
        {
            var announcement = new Announcement(
                id: PublishedAnnouncementWithSurveyExpectsAutoClosingId,
                content: "Опубликованное объявление c опросом, ожидающим автоматического закрытия, результаты которого не доступны до закрытия",
                author: mainUsergroupAdmin,
                audience: [mainUsergroupAdmin],
                firstlyPublishedAt: DateTime.Now.Subtract(TimeSpan.FromHours(12)),
                publishedAt: DateTime.Now.Subtract(TimeSpan.FromHours(12)),
                hiddenAt: null,
                delayedPublishingAt: null,
                delayedHidingAt: null,
                attachments: [GetDbEntity<Survey>(SurveyExpectsAutoClosingId)]
            );
            dbContext.Announcements.Add(announcement);
            AddDbEntity(announcement.Id, announcement);
        }
    }
}