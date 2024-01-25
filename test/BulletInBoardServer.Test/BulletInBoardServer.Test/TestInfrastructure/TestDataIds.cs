namespace BulletInBoardServer.Test.TestInfrastructure;
// ReSharper disable InconsistentNaming

public static class TestDataIds
{
    #region пользователи
    public static readonly Guid MainUsergroupAdminId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc");
    public static readonly Guid UsualUser_1_Id = Guid.Parse("0166b8c0-0da2-4ad3-80bd-15f51335f109");
    #endregion
    

    
    #region группы пользователей
    public static readonly Guid MainUsergroupId = Guid.Parse("c9a4f026-0632-452f-9d5f-ddb7973db240");
    #endregion
    
    
    
    #region объявление с публичным опросом
    public static readonly Guid AnnouncementWithPublicSurveyId = Guid.Parse("1345da23-300f-404d-a126-76f4283bce1b");
    
    public static readonly Guid PublicSurveyId = Guid.Parse("533ae58f-0c1c-4a54-b9b9-32d2a9967c82");
    
    public static readonly Guid Question_1_WithSingleChoice_OfPublicSurvey = Guid.Parse("7fad24a4-6fc0-44ea-97c6-19fad4e30404");
    public static readonly Guid Answer_1_OfPublicSurvey = Guid.Parse("60799cdb-5760-48e4-9431-dcb98cad55fd");
    public static readonly Guid Answer_2_OfPublicSurvey = Guid.Parse("40702480-ee45-4b7e-86fb-0c6d9bcb5808");
    public static readonly Guid Answer_3_OfPublicSurvey = Guid.Parse("bae09123-05b0-4373-895c-b094ac886884");
    
    public static readonly Guid Question_2_WithMultipleChoice_OfPublicSurvey = Guid.Parse("2680df25-9e4d-40f3-9d38-10663ab1b47f");
    public static readonly Guid Answer_4_OfPublicSurvey = Guid.Parse("4f7c09e6-82bf-4cf7-84cf-fd0c1c45924e");
    public static readonly Guid Answer_5_OfPublicSurvey = Guid.Parse("cd1affd1-90f1-4967-bba2-bb8f081a8bb1");
    public static readonly Guid Answer_6_OfPublicSurvey = Guid.Parse("2b6f2789-2c2c-4cb2-8421-57f7af293331");
    #endregion объявление с публичным опросом

    
    
    #region объявление с анонимным опросом
    public static readonly Guid AnnouncementWithAnonymousSurveyId = Guid.Parse("a55e4833-5dae-4464-904c-4971d5092bb8");

    public static readonly Guid AnonymousSurveyId = Guid.Parse("a0699239-eb54-4482-9407-a96ca7a3b9d5");
    
    public static readonly Guid Question_1_WithSingleChoice_OfAnonymousSurvey = Guid.Parse("64c9ca2b-c93a-4a2e-a0f0-93d23f812f23");
    public static readonly Guid Answer_1_OfAnonymousSurvey = Guid.Parse("e69a6cdb-8d6d-494b-9fd9-f213d939dc95");
    public static readonly Guid Answer_2_OfAnonymousSurvey = Guid.Parse("5dfc61aa-cc59-4bc1-b5e1-43541d5f89eb");
    public static readonly Guid Answer_3_OfAnonymousSurvey = Guid.Parse("c924b4e2-f25e-4ab9-bc50-e0094efaeea1");
    
    public static readonly Guid Question_2_WithMultipleChoice_OfAnonymousSurvey = Guid.Parse("547b161b-eab6-422d-8e57-c848a0ed8349");
    public static readonly Guid Answer_4_OfAnonymousSurvey = Guid.Parse("f28b0ff9-a7ec-49af-8e34-3e9058c2fb88");
    public static readonly Guid Answer_5_OfAnonymousSurvey = Guid.Parse("b537150e-d89c-4009-9dbf-0c68ed46e566");
    public static readonly Guid Answer_6_OfAnonymousSurvey = Guid.Parse("a9ce2ea2-8cb6-4459-b200-12003b9d16df");
    #endregion объявление с анонимным опросом

    
    
    #region объявление с закрытым анонимным опросом
    public static readonly Guid AnnouncementWithClosedAnonymousSurveyId = Guid.Parse("dfc9aeb8-0c53-49ff-bf93-86d9cf290440");

    public static readonly Guid ClosedAnonymousSurveyId = Guid.Parse("af497d4e-cd62-4ac2-a6a7-dfd0b06ec83b");
    
    public static readonly Guid Question_1_WithSingleChoice_OfClosedAnonymousSurvey = Guid.Parse("b31b140e-e155-4c81-a912-5fc0b6076930");
    public static readonly Guid Answer_1_OfClosedAnonymousSurvey = Guid.Parse("4e714b95-3112-4a33-aa55-e6f548770a1d");
    public static readonly Guid Answer_2_OfClosedAnonymousSurvey = Guid.Parse("e10f0e7b-c2be-4a69-a90f-9ac72b6b393f");
    #endregion
}