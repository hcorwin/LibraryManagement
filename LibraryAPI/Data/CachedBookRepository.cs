using LibraryAPI.Data.Interfaces;
using LibraryAPI.Models;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryAPI.Data
{
    public class CachedBookRepository : IBooksRepository
    {
        private readonly IBooksRepository _decorated;
        private readonly IMemoryCache _memoryCache;

        public CachedBookRepository(IBooksRepository decorated, IMemoryCache memoryCache)
        {
            _decorated = decorated;
            _memoryCache = memoryCache;
        }

        public Task<List<Book>> GetAllBooks(CancellationToken cancellationToken = default) =>
            _decorated.GetAllBooks(cancellationToken);

        public Task<bool> Exists(int id, CancellationToken cancellationToken = default) =>
            _decorated.Exists(id, cancellationToken);

        public Task<Book> GetBookById(int id, CancellationToken cancellationToken = default)
        {
            var key = $"book-{id}";

            return _memoryCache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));

                    return _decorated.GetBookById(id, cancellationToken);
                })!;
        }

        public Task AddBook(Book book, CancellationToken cancellationToken = default) =>
            _decorated.AddBook(book, cancellationToken);

        public Task CheckoutBook(Book book, string username, CancellationToken cancellationToken = default) =>
            _decorated.CheckoutBook(book, username, cancellationToken);

        public Task ReturnBook(int id, CancellationToken cancellationToken = default) =>
            _decorated.ReturnBook(id, cancellationToken);

        public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _decorated.SaveChangesAsync(cancellationToken);
    }
}
