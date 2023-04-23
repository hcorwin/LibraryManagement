using LibraryAPI.Data.Interfaces;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Modules
{
    public static class UsersModule
    {
        public static void AddUsersEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/users/login", async (string username, string password, IUsersRepository db, IPasswordEncryptionService es) =>
            {
                var existingUser = await db.GetUserByUsername(username);
                return es.IsPasswordEqual(password, existingUser?.Password, Convert.FromBase64String(existingUser?.Salt)) ? Results.Ok() : Results.BadRequest();
            });

            app.MapPut("/users/register", async (string username, string password, IUsersRepository db, IPasswordEncryptionService es) =>
            {
                if (await db.Exists(username))
                    return Results.BadRequest();
                var salt = es.GenerateSalt();
                var newUser = new User
                {
                    Username = username,
                    Password = es.HashPassword(password, salt),
                    Salt = Convert.ToBase64String(salt),
                    Admin = false
                };
                await db.AddUser(newUser);
                await db.SaveChangesAsync();
                return Results.Ok();
            });
        }
    }
}
