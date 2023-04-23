using LibraryAPI.Models;

namespace LibraryAPITests.TestDataHelper
{
    public static class BookTestDataHelper
    {
        public static List<Book> GetBookList() =>
            new()
            {
                new Book
                {
                    Id = 1,
                    Title = "test",
                    Author = "test",
                    LastCheckedOutDate = DateTime.Now.AddDays(10),
                    LastReturnedDate = DateTime.Now.AddMonths(-1),
                    CheckedOutBy = null,
                    Available = true
                },
                new Book
                {
                    Id = 2,
                    Title = "tes1",
                    Author = "test2",
                    LastCheckedOutDate = DateTime.Now.AddDays(10),
                    LastReturnedDate = DateTime.Now.AddMonths(-1),
                    CheckedOutBy = "testing",
                    Available = false
                }
            };
    }
}
