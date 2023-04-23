using LibraryAPI.Models;

namespace LibraryAPI.Data.Interfaces;

public interface IUsersRepository
{
    Task<bool> Exists(string username, CancellationToken cancellationToken = default);
    Task AddUser(User newUser, CancellationToken cancellationToken = default);
    Task<User?> GetUserByUsername(string username, CancellationToken cancellationToken = default);
    Task DeleteUser(string username, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}