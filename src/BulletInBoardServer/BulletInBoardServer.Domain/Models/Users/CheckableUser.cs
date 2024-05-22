namespace BulletInBoardServer.Domain.Models.Users;

public class CheckableUser : User
{
    public bool IsChecked { get; set; }



    public CheckableUser(User user, bool isChecked)
        : base(user.Id, user.FirstName, user.SecondName, user.Patronymic)
    {
        IsChecked = isChecked;
    }
}