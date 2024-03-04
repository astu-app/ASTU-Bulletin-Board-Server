namespace BulletInBoardServer.Domain.Models.UserGroups.Exceptions;

public class UserGroupNameEmptyException(Exception? innerException = null) : ArgumentException(
    "Название группы пользователей не может быть null, быть пустым или состоять только из пробельных символов",
    innerException);