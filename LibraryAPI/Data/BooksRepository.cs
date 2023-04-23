using LibraryAPI.Data.Interfaces;
using LibraryAPI.Models;
using LibraryAPI.Models.ModelExtensions;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data
{
    public sealed class BooksRepository : IBooksRepository
    {
        private readonly ILibraryDbContext _dbContext;

        public BooksRepository(ILibraryDbContext dbContext) =>
            _dbContext = dbContext;


        public async Task<List<Book>> GetAllBooks(CancellationToken cancellationToken = default) =>
            await _dbContext.Books.ToListAsync(cancellationToken);

        public Task<bool> Exists(int id, CancellationToken cancellationToken = default) =>
            _dbContext.Books.AnyAsync(x => x.Id == id, cancellationToken);

        public async Task<Book> GetBookById(int id, CancellationToken cancellationToken = default)
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (book is null)
            {
                throw new KeyNotFoundException("Id not found");
            }

            return book;
        }

        public async Task AddBook(Book book, CancellationToken cancellationToken = default)
        {
            if (await Exists(book.Id, cancellationToken))
            {
                throw new ArgumentException("Book already exists");
            }
            await _dbContext.Books.AddAsync(book, cancellationToken);
        }

        public async Task CheckoutBook(Book book, string username, CancellationToken cancellationToken = default)
        {
            var bookToCheckout = await GetBookById(book.Id, cancellationToken);
            if (!bookToCheckout.Available)
            {
                throw new ArgumentException("Book isn't available");
            }
            bookToCheckout.Checkout(username);
        }

        public async Task ReturnBook(int id, CancellationToken cancellationToken = default)
        {
            var bookToReturn = await GetBookById(id, cancellationToken);
            bookToReturn.Return();
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default) => 
            _dbContext.SaveChangesAsync(cancellationToken);

    }
}
