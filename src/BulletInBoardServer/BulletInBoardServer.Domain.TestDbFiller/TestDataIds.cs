namespace BulletInBoardServer.Domain.TestDbFiller;

public static class TestDataIds
{
    #region пользователи

    public static readonly Guid MainUsergroupAdminId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc");
    public static readonly Guid UserGroup_1_AdminId = Guid.Parse("64b5d14e-84d2-450f-814e-afa9579752b2");
    public static readonly Guid UserGroup_2_AdminId = Guid.Parse("c0f3471b-09e7-4bcf-8bb8-d5673251aff7");
    public static readonly Guid UserGroup_3_AdminId = Guid.Parse("9634c12a-c12b-4b4f-b576-8e203a73b949");
    public static readonly Guid UserGroup_4_AdminId = Guid.Parse("504f5c5f-f1a9-4662-b18d-289868fbb32d");
    public static readonly Guid UserGroup_5_AdminId = Guid.Parse("4fbe3acb-3e6a-4617-b9f8-0eb0092197a8");
    public static readonly Guid UserGroup_6_AdminId = Guid.Parse("df293744-21fe-4dbe-a2bb-0f64a202514a");
    public static readonly Guid UserGroup_7_AdminId = Guid.Parse("2ddf9134-1fc6-45e1-92ba-c46ae24f9c06");
    
    
    public static readonly Guid UsualUser_1_Id = Guid.Parse("0166b8c0-0da2-4ad3-80bd-15f51335f109");
    public static readonly Guid UsualUser_2_Id = Guid.Parse("26ee62dc-6b8c-4001-9a35-79a99a8944f3");

    #endregion

    #region группы пользователей

    public static readonly Guid MainUsergroupId = Guid.Parse("c9a4f026-0632-452f-9d5f-ddb7973db240");
    public static readonly Guid UserGroup_1_Id = Guid.Parse("3ef77770-7660-4aa8-9525-bc784260fcf9");
    public static readonly Guid UserGroup_2_Id = Guid.Parse("2eb35d1e-ce66-4d3d-b1d4-50a53f60c110");
    public static readonly Guid UserGroup_3_Id = Guid.Parse("2278c2c9-adde-40c2-9d68-a22d09dd7ec1");
    public static readonly Guid UserGroup_4_Id = Guid.Parse("a6eb28f4-09c2-4a3e-b6ed-bfcd46d89da9");
    public static readonly Guid UserGroup_5_Id = Guid.Parse("a25a6df2-a520-464a-a4a3-91b1b4de5c21");
    public static readonly Guid UserGroup_6_Id = Guid.Parse("e5dbc353-cd28-4aaa-b7b6-e0cb2e71324f");
    public static readonly Guid UserGroup_7_Id = Guid.Parse("42bbd96b-ddab-4e85-8ec5-70e1158e18ca");

    #endregion

    #region опросы

    // публичный опрос 1
    public static readonly Guid PublicSurvey_1_Id = Guid.Parse("533ae58f-0c1c-4a54-b9b9-32d2a9967c82");

    public static readonly Guid Question_1_WithSingleChoice_OfPublicSurvey_1_Id =
        Guid.Parse("7fad24a4-6fc0-44ea-97c6-19fad4e30404");

    public static readonly Guid Answer_1_OfPublicSurvey_1_Id = Guid.Parse("60799cdb-5760-48e4-9431-dcb98cad55fd");
    public static readonly Guid Answer_2_OfPublicSurvey_1_Id = Guid.Parse("40702480-ee45-4b7e-86fb-0c6d9bcb5808");
    public static readonly Guid Answer_3_OfPublicSurvey_1_Id = Guid.Parse("bae09123-05b0-4373-895c-b094ac886884");

    public static readonly Guid Question_2_WithMultipleChoice_OfPublicSurvey_1_Id =
        Guid.Parse("2680df25-9e4d-40f3-9d38-10663ab1b47f");

    public static readonly Guid Answer_4_OfPublicSurvey_1_Id = Guid.Parse("4f7c09e6-82bf-4cf7-84cf-fd0c1c45924e");
    public static readonly Guid Answer_5_OfPublicSurvey_1_Id = Guid.Parse("cd1affd1-90f1-4967-bba2-bb8f081a8bb1");
    public static readonly Guid Answer_6_OfPublicSurvey_1_Id = Guid.Parse("2b6f2789-2c2c-4cb2-8421-57f7af293331");

    // анонимный опрос 1
    public static readonly Guid AnonymousSurvey_1_Id = Guid.Parse("a0699239-eb54-4482-9407-a96ca7a3b9d5");

    public static readonly Guid Question_1_WithSingleChoice_OfAnonymousSurvey_1_Id =
        Guid.Parse("64c9ca2b-c93a-4a2e-a0f0-93d23f812f23");

    public static readonly Guid Answer_1_OfAnonymousSurvey_1_Id =
        Guid.Parse("e69a6cdb-8d6d-494b-9fd9-f213d939dc95");

    public static readonly Guid Answer_2_OfAnonymousSurvey_1_Id =
        Guid.Parse("5dfc61aa-cc59-4bc1-b5e1-43541d5f89eb");

    public static readonly Guid Answer_3_OfAnonymousSurvey_1_Id =
        Guid.Parse("c924b4e2-f25e-4ab9-bc50-e0094efaeea1");

    public static readonly Guid Question_2_WithMultipleChoice_OfAnonymousSurvey_1_Id =
        Guid.Parse("547b161b-eab6-422d-8e57-c848a0ed8349");

    public static readonly Guid Answer_4_OfAnonymousSurvey_1_Id =
        Guid.Parse("f28b0ff9-a7ec-49af-8e34-3e9058c2fb88");

    public static readonly Guid Answer_5_OfAnonymousSurvey_1_Id =
        Guid.Parse("b537150e-d89c-4009-9dbf-0c68ed46e566");

    public static readonly Guid Answer_6_OfAnonymousSurvey_1_Id =
        Guid.Parse("a9ce2ea2-8cb6-4459-b200-12003b9d16df");

    // закрытый анонимный опрос 1
    public static readonly Guid ClosedAnonymousSurvey_1_Id = Guid.Parse("af497d4e-cd62-4ac2-a6a7-dfd0b06ec83b");

    public static readonly Guid Question_1_WithSingleChoice_OfClosedAnonymousSurvey_1_Id =
        Guid.Parse("b31b140e-e155-4c81-a912-5fc0b6076930");

    public static readonly Guid Answer_1_OfClosedAnonymousSurvey_1_Id =
        Guid.Parse("4e714b95-3112-4a33-aa55-e6f548770a1d");

    public static readonly Guid Answer_2_OfClosedAnonymousSurvey_1_Id =
        Guid.Parse("e10f0e7b-c2be-4a69-a90f-9ac72b6b393f");

    // публичный опрос 2
    public static readonly Guid PublicSurvey_2_Id = Guid.Parse("ed4c0055-7ca3-4e24-bf30-85bafc67a95b");

    public static readonly Guid Question_1_WithSingleChoice_OfPublicSurvey_2_Id =
        Guid.Parse("9b798d5a-ba0f-4268-a4a6-f4aa06d24cf1");

    public static readonly Guid Answer_1_OfPublicSurvey_2_Id = Guid.Parse("d0420c46-5d0b-4907-af5b-52ae1f31aa38");
    public static readonly Guid Answer_2_OfPublicSurvey_2_Id = Guid.Parse("68d83c63-1d56-43d8-bf12-f37dc4502bb3");

    // опрос, ожидающий автоматическое закрытие
    public static readonly Guid SurveyExpectsAutoClosingId = Guid.Parse("9348b68c-52f8-46ae-902d-0501b2d31f03");

    public static readonly Guid Question_1_WithSingleChoice_OfSurveyExpectsAutoClosing_Id =
        Guid.Parse("a4d30949-a510-4f28-9606-2852995ee823");

    public static readonly Guid Answer_1_OfSurveyExpectsAutoClosing_Id =
        Guid.Parse("c8462a11-a3f1-4b72-bc39-62983f42d6c7");

    public static readonly Guid Answer_2_OfSurveyExpectsAutoClosing_Id =
        Guid.Parse("228c41be-bfd5-4bd8-85f9-536c915dd5a5");

    #endregion опросы



    #region объявления

    // объявление с публичным опросом
    public static readonly Guid AnnouncementWithPublicSurvey_1_Id =
        Guid.Parse("1345da23-300f-404d-a126-76f4283bce1b");

    // объявление с анонимным опросом
    public static readonly Guid AnnouncementWithAnonymousSurvey_1_Id =
        Guid.Parse("a55e4833-5dae-4464-904c-4971d5092bb8");

    // объявление с закрытым анонимным опросом
    public static readonly Guid AnnouncementWithClosedAnonymousSurvey_1_Id =
        Guid.Parse("dfc9aeb8-0c53-49ff-bf93-86d9cf290440");

    // полностью заполненное объявление
    public static readonly Guid FullyFilledAnnouncement_1_Id = Guid.Parse("920d895b-0465-4100-ad18-273f4f4c8817");

    // объявление с заданными отложенной публикацией и отложенным сокрытием
    public static readonly Guid AnnouncementWithFilledDelayedMomentsId =
        Guid.Parse("be90dc5e-1b0c-4e7f-8f8a-b9ef2edcc993");

    // скрытое объявление с выключенными моментами отложенной публикации и отложенного сокрытия
    public static readonly Guid HiddenAnnouncementWithDisabledDelayedMomentsId =
        Guid.Parse("ecfe9b27-f2e4-4553-a6f7-67b8d5cb8ea4");

    // скрытое объявление с включенным моментом отложенной публикации
    public static readonly Guid HiddenAnnouncementWithEnabledDelayedPublishingId =
        Guid.Parse("86616371-2247-4494-8964-2478e6e0e85f");

    // опубликованное объявление с заданным моментом отложенного сокрытия
    public static readonly Guid PublishedAnnouncementWithEnabledDelayedHidingId =
        Guid.Parse("e4bda77c-3653-4479-a600-da664ed21d3a");

    // опубликованное объявление без заданного момента отложенного сокрытия
    public static readonly Guid PublishedAnnouncementWithDisabledDelayedHidingId =
        Guid.Parse("39a4e588-3055-4216-93a3-5a9d5ed98102");
    
    // опубликованное объявление с опросом, ожидающим автоматического закрытия, результаты которого закрыты до закрытия 
    public static readonly Guid PublishedAnnouncementWithSurveyExpectsAutoClosingId = 
        Guid.Parse("668d1d10-a9c9-4b49-844c-8cc00488a0a9");

    #endregion объявления
}