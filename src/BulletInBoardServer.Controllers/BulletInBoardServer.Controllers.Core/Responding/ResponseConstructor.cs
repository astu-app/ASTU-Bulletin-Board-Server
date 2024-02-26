namespace BulletInBoardServer.Controllers.Core.Responding;

public static class ResponseConstructor
{
    public static object ConstructResponseWithOnlyCode<TEnum>(TEnum code)
        where TEnum : Enum =>
        new { Code = code };
}