namespace LibraryAPI.Models.ModelExtensions
{
    public static class BookExtensions
    {
        public static void Update(this Book originalBook, Book updatedBook)
        {
            originalBook.Author = updatedBook.Author;
            originalBook.Title = updatedBook.Title;
            originalBook.Available = updatedBook.Available;
            originalBook.CheckedOutBy = updatedBook.CheckedOutBy;
            originalBook.LastCheckedOutDate = updatedBook.LastCheckedOutDate;
            originalBook.LastReturnedDate = updatedBook.LastReturnedDate;
        }

        public static void Checkout(this Book book, string username)
        {
            book.CheckedOutBy = username;
            book.LastCheckedOutDate = DateTime.Now;
            book.LastReturnedDate = null;
            book.Available = false;
        }

        public static void Return(this Book book)
        {
            book.Available = true;
            book.LastCheckedOutDate = null;
            book.LastReturnedDate = DateTime.Now;
            book.CheckedOutBy = null;
        }
    }
}
