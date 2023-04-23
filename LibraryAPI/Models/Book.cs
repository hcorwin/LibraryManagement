namespace LibraryAPI.Models;

public class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public DateTime? LastCheckedOutDate { get; set; }

    public DateTime? LastReturnedDate { get; set; }

    public string? CheckedOutBy { get; set; }

    public bool Available { get; set; }
}


