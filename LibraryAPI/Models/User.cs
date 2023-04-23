namespace LibraryAPI.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Admin { get; set; }

    public string Salt { get; set; } = null!;
}
