using LibraryAPI.Data.Interfaces;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ILibraryDbContext _dbContext;

        public UsersRepository(ILibraryDbContext dbContext) =>
            _dbContext = dbContext;

        public Task<bool> Exists(string username, CancellationToken cancellationToken = default) =>
            _dbContext.Users.AnyAsync(x => x.Username == username, cancellationToken);

        public async Task AddUser(User newUser, CancellationToken cancellationToken = default)
        {
            if (await Exists(newUser.Username, cancellationToken))
            {
                throw new ArgumentException("User Already Exists");
            }
            await _dbContext.Users.AddAsync(newUser, cancellationToken);
        }

        public async Task<User?> GetUserByUsername(string username, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken: cancellationToken);
            if (user is null)
            {
                throw new KeyNotFoundException("User Not Found");
            }
            return user;
        }

        public async Task DeleteUser(string username, CancellationToken cancellationToken)
        {
            var user = await GetUserByUsername(username, cancellationToken);
            if (user != null) _dbContext.Users.Remove(user);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _dbContext.SaveChangesAsync(cancellationToken);
    }
}
