namespace BulletInBoardServer.Test.TestPreparation;
// ReSharper disable InconsistentNaming

public static class TestDataIds
{
    // пользователи
    public static readonly Guid MainUsergroupAdminId = Guid.Parse("cf48c46f-0f61-433d-ac9b-fe7a81263ffc");
    public static readonly Guid UsualUser_1_Id = Guid.Parse("0166b8c0-0da2-4ad3-80bd-15f51335f109");
    
    // группы пользователей
    public static readonly Guid MainUsergroupId = Guid.Parse("c9a4f026-0632-452f-9d5f-ddb7973db240");
    
    // объявления
    public static readonly Guid AnnouncementId = Guid.Parse("1345da23-300f-404d-a126-76f4283bce1b");
    
    // публичный опрос
    public static readonly Guid PublicSurveyId = Guid.Parse("533ae58f-0c1c-4a54-b9b9-32d2a9967c82");
    
    public static readonly Guid Question_1_WithSingleChoice_OfPublicSurvey = Guid.Parse("7fad24a4-6fc0-44ea-97c6-19fad4e30404");
    public static readonly Guid Answer_1_OfPublicSurvey = Guid.Parse("60799cdb-5760-48e4-9431-dcb98cad55fd");
    public static readonly Guid Answer_2_OfPublicSurvey = Guid.Parse("40702480-ee45-4b7e-86fb-0c6d9bcb5808");
    public static readonly Guid Answer_3_OfPublicSurvey = Guid.Parse("bae09123-05b0-4373-895c-b094ac886884");
    
    public static readonly Guid Question_2_WithMultipleChoice_OfPublicSurvey = Guid.Parse("2680df25-9e4d-40f3-9d38-10663ab1b47f");
    public static readonly Guid Answer_4_OfPublicSurvey = Guid.Parse("4f7c09e6-82bf-4cf7-84cf-fd0c1c45924e");
    public static readonly Guid Answer_5_OfPublicSurvey = Guid.Parse("cd1affd1-90f1-4967-bba2-bb8f081a8bb1");
    public static readonly Guid Answer_6_OfPublicSurvey = Guid.Parse("2b6f2789-2c2c-4cb2-8421-57f7af293331");

    // анонимный опрос
    public static readonly Guid AnonymousSurveyId = Guid.Parse("a0699239-eb54-4482-9407-a96ca7a3b9d5");
    
    public static readonly Guid Question_1_WithSingleChoice_OfAnonymousSurvey = Guid.Parse("7fad24a4-6fc0-44ea-97c6-19fad4e30404");
    public static readonly Guid Answer_1_OfAnonymousSurvey = Guid.Parse("60799cdb-5760-48e4-9431-dcb98cad55fd");
    public static readonly Guid Answer_2_OfAnonymousSurvey = Guid.Parse("40702480-ee45-4b7e-86fb-0c6d9bcb5808");
    public static readonly Guid Answer_3_OfAnonymousSurvey = Guid.Parse("bae09123-05b0-4373-895c-b094ac886884");
    
    public static readonly Guid Question_2_WithMultipleChoice_OfAnonymousSurvey = Guid.Parse("2680df25-9e4d-40f3-9d38-10663ab1b47f");
    public static readonly Guid Answer_4_OfAnonymousSurvey = Guid.Parse("4f7c09e6-82bf-4cf7-84cf-fd0c1c45924e");
    public static readonly Guid Answer_5_OfAnonymousSurvey = Guid.Parse("cd1affd1-90f1-4967-bba2-bb8f081a8bb1");
    public static readonly Guid Answer_6_OfAnonymousSurvey = Guid.Parse("2b6f2789-2c2c-4cb2-8421-57f7af293331");
}