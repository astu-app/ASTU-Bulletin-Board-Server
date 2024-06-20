namespace BulletInBoardServer.Domain.DemoDbFiller;

public static class DemoDataIds
{
    #region пользователи

    // ректорат
    /// <summary>Неваленный Александр Николаевич</summary>
    public static readonly Guid Rectory_AdminId = Guid.Parse("b9e6d582-b567-44be-a37c-d4f8ee62978d");
    
    // проректор по учебной работе
    /// <summary>Квятковская Ирина Юрьевна</summary>
    public static readonly Guid ViceRectorForAcademicAffairs_AdminId = Guid.Parse("5a4a7fd3-4d8b-4be3-a9d5-30b561321997");
    /// <summary>Заместитель Квятковской Ирины Юрьевны</summary>
    public static readonly Guid ViceRectorForAcademicAffairsDeputy_UserId = Guid.Parse("d9ecd024-8f84-472d-a43f-c537287dd4d4");
    
    // ИИТИК
    /// <summary>Белов Сергей Валерьевич</summary>
    public static readonly Guid InstituteOfInformationTechnologiesAndCommunications_AdminId = Guid.Parse("9b84cd6f-b352-440c-833b-90a940baf2ca");
    // участники
    public static readonly Guid InstituteOfInformationTechnologiesAndCommunications_Member1 = Guid.Parse("df0c40c7-bb65-46fe-8b7b-7c9b014b855b");
    public static readonly Guid InstituteOfInformationTechnologiesAndCommunications_Member2 = Guid.Parse("49f2ae2d-27b5-45dc-819e-ee870c5389c3");
    public static readonly Guid InstituteOfInformationTechnologiesAndCommunications_Member3 = Guid.Parse("813d431b-61cd-4543-92aa-a587cbe6b89d");
    
    // кафедра АСОИУ
    /// <summary>Хоменко Татьяна Владимировна</summary>
    public static readonly Guid AutomatedInformationProcessingAndControl_AdminId = Guid.Parse("9874d734-fbc3-427e-9fa3-1eeec9285547");
    // кафедра
    public static readonly Guid AutomatedInformationProcessingAndControl_Member1 = Guid.Parse("f542de80-88a4-4249-ba92-09a55935185f");
    public static readonly Guid AutomatedInformationProcessingAndControl_Member2 = Guid.Parse("2f597879-14c3-4c96-b21e-86a3b8f2bc35");
    public static readonly Guid AutomatedInformationProcessingAndControl_Member3 = Guid.Parse("e87a6777-b610-4125-947e-0130e64ec1dc");
    // группа 1
    public static readonly Guid AutomatedInformationProcessingAndControl_Group1_Member1 = Guid.Parse("f92c13e8-875b-46d9-9fcb-6fb0500230eb");
    public static readonly Guid AutomatedInformationProcessingAndControl_Group1_Member2 = Guid.Parse("219387a4-76d1-47df-8362-7e0026f09b6a");
    public static readonly Guid AutomatedInformationProcessingAndControl_Group1_Member3 = Guid.Parse("9f4a1163-346e-4bd8-adb2-0acd3038480c");
    // группа 2
    public static readonly Guid AutomatedInformationProcessingAndControl_Group2_Member1 = Guid.Parse("ad098f84-7c82-4bab-9b3b-b24a2a794950");
    public static readonly Guid AutomatedInformationProcessingAndControl_Group2_Member2 = Guid.Parse("76299823-d636-4926-bcdd-c21db7b77418");
    public static readonly Guid AutomatedInformationProcessingAndControl_Group2_Member3 = Guid.Parse("69df8ccf-21e0-48ce-8c81-364c9e7da049");
    
    // кафедра ПИ
    /// <summary>Бондарева Ирина Олеговна</summary>
    public static readonly Guid AppliedInformatics_AdminId = Guid.Parse("fb975a3b-3335-468a-8648-559d30febbde");
    // участники
    public static readonly Guid AppliedInformatics_Member1 = Guid.Parse("391476cb-0f93-4836-91ca-f9071e79e495");
    public static readonly Guid AppliedInformatics_Member2 = Guid.Parse("9ebe3a2e-2e73-405f-8a5b-338971981fa6");
    public static readonly Guid AppliedInformatics_Member3 = Guid.Parse("0dfbff97-5d28-4f59-b0ff-2e55ed10e737");
    // группа 1
    public static readonly Guid AppliedInformatics_Group1_Member1 = Guid.Parse("6e603da7-9a2e-4f46-85bb-bf7e79d4f7c9");
    public static readonly Guid AppliedInformatics_Group1_Member2 = Guid.Parse("9b50e0ec-8b7d-4542-a005-75352ce6687f");
    public static readonly Guid AppliedInformatics_Group1_Member3 = Guid.Parse("201471f3-2de4-4be8-becd-a1ec0eab6bfd");
    // группа 2
    public static readonly Guid AppliedInformatics_Group2_Member1 = Guid.Parse("1c8b2f58-2b1b-414f-b2c9-544a6a155c46");
    public static readonly Guid AppliedInformatics_Group2_Member2 = Guid.Parse("33dbd95b-d256-4f04-9eb6-6b15aa17ea0c");
    public static readonly Guid AppliedInformatics_Group2_Member3 = Guid.Parse("07a8be64-4350-4f04-a2ba-eaef506fc29b");

    #endregion

    #region группы пользователей
    /// <summary>Ректорат</summary>
    public static readonly Guid Rectory_GroupId = Guid.Parse("e901b21c-20d8-47da-8ae6-5bcb57cd983e");
    /// <summary>Проректор по учебной работе</summary>
    public static readonly Guid ViceRectorForAcademicAffairs_GroupId = Guid.Parse("a1d90cc2-e366-4669-b59c-1e967781788f");
    /// <summary>ИИТИК</summary>
    public static readonly Guid InstituteOfInformationTechnologiesAndCommunications_GroupId = Guid.Parse("fdfb143b-ea64-4a5b-8fd1-a3fde24b7808");
    
    /// <summary>Кафедра АСОИУ</summary>
    public static readonly Guid AutomatedInformationProcessingAndControl_GroupId = Guid.Parse("3e935b16-3257-4dd7-91de-72cc1ea201f4");
    /// <summary>Студенческая группа 1 кафедры АСОИУ</summary>
    public static readonly Guid AutomatedInformationProcessingAndControl_Group1_GroupId = Guid.Parse("520a0edd-c6e8-49cb-b65e-2d2447a7d81f");
    /// <summary>Студенческая группа 2 кафедры АСОИУ</summary>
    public static readonly Guid AutomatedInformationProcessingAndControl_Group2_GroupId = Guid.Parse("2d6cfc32-5f7c-4b65-9427-f9432979d9e9");
    
    /// <summary>Кафедра ПИ</summary>
    public static readonly Guid AppliedInformatics_GroupId = Guid.Parse("d6830e5b-4c53-4bc4-8b29-48ff9aaa95db");
    /// <summary>Студенческая группа 1 кафедры ПИ</summary>
    public static readonly Guid AppliedInformatics_Group1_GroupId = Guid.Parse("d24b138f-ed0b-4630-8893-d680580aa225");
    /// <summary>Студенческая группа 2 кафедры ПИ</summary>
    public static readonly Guid AppliedInformatics_Group2_GroupId = Guid.Parse("5b1cb76c-4c31-4fde-aaea-a750e3997c56");
    

    #endregion

    #region опросы

    // публичный опрос 1
    public static readonly Guid PublicSurvey_1_Id = Guid.Parse("533ae58f-0c1c-4a54-b9b9-32d2a9967c82");
    
    public static readonly Guid Question_1_WithSingleChoice_OfPublicSurvey_1_Id = Guid.Parse("7fad24a4-6fc0-44ea-97c6-19fad4e30404");
    public static readonly Guid Answer_1_OfPublicSurvey_1_Id = Guid.Parse("60799cdb-5760-48e4-9431-dcb98cad55fd");
    public static readonly Guid Answer_2_OfPublicSurvey_1_Id = Guid.Parse("40702480-ee45-4b7e-86fb-0c6d9bcb5808");
    public static readonly Guid Answer_3_OfPublicSurvey_1_Id = Guid.Parse("bae09123-05b0-4373-895c-b094ac886884");

    public static readonly Guid Question_2_WithMultipleChoice_OfPublicSurvey_1_Id = Guid.Parse("2680df25-9e4d-40f3-9d38-10663ab1b47f");
    public static readonly Guid Answer_4_OfPublicSurvey_1_Id = Guid.Parse("4f7c09e6-82bf-4cf7-84cf-fd0c1c45924e");
    public static readonly Guid Answer_5_OfPublicSurvey_1_Id = Guid.Parse("cd1affd1-90f1-4967-bba2-bb8f081a8bb1");
    public static readonly Guid Answer_6_OfPublicSurvey_1_Id = Guid.Parse("2b6f2789-2c2c-4cb2-8421-57f7af293331");

    // анонимный опрос 1
    public static readonly Guid AnonymousSurvey_1_Id = Guid.Parse("a0699239-eb54-4482-9407-a96ca7a3b9d5");

    public static readonly Guid Question_1_WithSingleChoice_OfAnonymousSurvey_1_Id = Guid.Parse("64c9ca2b-c93a-4a2e-a0f0-93d23f812f23");
    public static readonly Guid Answer_1_OfAnonymousSurvey_1_Id = Guid.Parse("e69a6cdb-8d6d-494b-9fd9-f213d939dc95");
    public static readonly Guid Answer_2_OfAnonymousSurvey_1_Id = Guid.Parse("5dfc61aa-cc59-4bc1-b5e1-43541d5f89eb");
    public static readonly Guid Answer_3_OfAnonymousSurvey_1_Id = Guid.Parse("c924b4e2-f25e-4ab9-bc50-e0094efaeea1");
    
    public static readonly Guid Question_2_WithMultipleChoice_OfAnonymousSurvey_1_Id = Guid.Parse("547b161b-eab6-422d-8e57-c848a0ed8349");
    public static readonly Guid Answer_4_OfAnonymousSurvey_1_Id = Guid.Parse("f28b0ff9-a7ec-49af-8e34-3e9058c2fb88");
    public static readonly Guid Answer_5_OfAnonymousSurvey_1_Id = Guid.Parse("b537150e-d89c-4009-9dbf-0c68ed46e566");
    public static readonly Guid Answer_6_OfAnonymousSurvey_1_Id = Guid.Parse("a9ce2ea2-8cb6-4459-b200-12003b9d16df");

    // закрытый анонимный опрос 1
    public static readonly Guid ClosedAnonymousSurvey_1_Id = Guid.Parse("af497d4e-cd62-4ac2-a6a7-dfd0b06ec83b");

    public static readonly Guid Question_1_WithSingleChoice_OfClosedAnonymousSurvey_1_Id = Guid.Parse("b31b140e-e155-4c81-a912-5fc0b6076930");
    public static readonly Guid Answer_1_OfClosedAnonymousSurvey_1_Id = Guid.Parse("4e714b95-3112-4a33-aa55-e6f548770a1d");
    public static readonly Guid Answer_2_OfClosedAnonymousSurvey_1_Id = Guid.Parse("e10f0e7b-c2be-4a69-a90f-9ac72b6b393f");

    // публичный опрос 2
    public static readonly Guid PublicSurvey_2_Id = Guid.Parse("ed4c0055-7ca3-4e24-bf30-85bafc67a95b");

    public static readonly Guid Question_1_WithSingleChoice_OfPublicSurvey_2_Id = Guid.Parse("9b798d5a-ba0f-4268-a4a6-f4aa06d24cf1");
    public static readonly Guid Answer_1_OfPublicSurvey_2_Id = Guid.Parse("d0420c46-5d0b-4907-af5b-52ae1f31aa38");
    public static readonly Guid Answer_2_OfPublicSurvey_2_Id = Guid.Parse("68d83c63-1d56-43d8-bf12-f37dc4502bb3");

    // опрос, ожидающий автоматическое закрытие
    public static readonly Guid SurveyExpectsAutoClosingId = Guid.Parse("9348b68c-52f8-46ae-902d-0501b2d31f03");

    public static readonly Guid Question_1_WithSingleChoice_OfSurveyExpectsAutoClosing_Id = Guid.Parse("a4d30949-a510-4f28-9606-2852995ee823");
    public static readonly Guid Answer_1_OfSurveyExpectsAutoClosing_Id = Guid.Parse("c8462a11-a3f1-4b72-bc39-62983f42d6c7");
    public static readonly Guid Answer_2_OfSurveyExpectsAutoClosing_Id = Guid.Parse("228c41be-bfd5-4bd8-85f9-536c915dd5a5");

    #endregion опросы



    #region объявления

    // объявление с публичным опросом
    public static readonly Guid AnnouncementWithPublicSurvey_1_Id = Guid.Parse("1345da23-300f-404d-a126-76f4283bce1b");

    // объявление с анонимным опросом
    public static readonly Guid AnnouncementWithAnonymousSurvey_1_Id = Guid.Parse("a55e4833-5dae-4464-904c-4971d5092bb8");

    // объявление с закрытым анонимным опросом
    public static readonly Guid AnnouncementWithClosedAnonymousSurvey_1_Id = Guid.Parse("dfc9aeb8-0c53-49ff-bf93-86d9cf290440");

    // полностью заполненное объявление
    public static readonly Guid FullyFilledAnnouncement_1_Id = Guid.Parse("920d895b-0465-4100-ad18-273f4f4c8817");

    // объявление с заданными отложенной публикацией и отложенным сокрытием
    public static readonly Guid AnnouncementWithFilledDelayedMomentsId = Guid.Parse("be90dc5e-1b0c-4e7f-8f8a-b9ef2edcc993");

    // скрытое объявление с выключенными моментами отложенной публикации и отложенного сокрытия
    public static readonly Guid HiddenAnnouncementWithDisabledDelayedMomentsId = Guid.Parse("ecfe9b27-f2e4-4553-a6f7-67b8d5cb8ea4");

    // скрытое объявление с включенным моментом отложенной публикации
    public static readonly Guid HiddenAnnouncementWithEnabledDelayedPublishingId = Guid.Parse("86616371-2247-4494-8964-2478e6e0e85f");

    // опубликованное объявление с заданным моментом отложенного сокрытия
    public static readonly Guid PublishedAnnouncementWithEnabledDelayedHidingId = Guid.Parse("e4bda77c-3653-4479-a600-da664ed21d3a");

    // опубликованное объявление без заданного момента отложенного сокрытия
    public static readonly Guid PublishedAnnouncementWithDisabledDelayedHidingId = Guid.Parse("39a4e588-3055-4216-93a3-5a9d5ed98102");
    
    // опубликованное объявление с опросом, ожидающим автоматического закрытия, результаты которого закрыты до закрытия 
    public static readonly Guid PublishedAnnouncementWithSurveyExpectsAutoClosingId = Guid.Parse("668d1d10-a9c9-4b49-844c-8cc00488a0a9");

    #endregion объявления
}