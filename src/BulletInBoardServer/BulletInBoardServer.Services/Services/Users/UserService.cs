using BulletInBoardServer.Domain;
using BulletInBoardServer.Domain.Models.Users;
using BulletInBoardServer.Services.Services.Users.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BulletInBoardServer.Services.Services.Users;

public class UserService(ApplicationDbContext dbContent)
{
    public void RegisterUser(User user)
    {
        dbContent.Users.Add(user);
        TrySaveChanges();
    }



    private void TrySaveChanges()
    {
        try
        {
            dbContent.SaveChanges();
        }
        catch (DbUpdateException err)
        {
            if (err.InnerException is not PostgresException inner)
                throw;

            switch (inner)
            {
                case { SqlState: "23505", ConstraintName: "users_pkey" }:
                    throw new UserAlreadyExistsException(err);
                default:
                    throw;
            }
        }
    }
}