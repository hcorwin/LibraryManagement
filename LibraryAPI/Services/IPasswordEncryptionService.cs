namespace LibraryAPI.Services;

public interface IPasswordEncryptionService
{
    string HashPassword(string password, byte[] salt);
    byte[] GenerateSalt();
    bool IsPasswordEqual(string password, string hash, byte[] salt);
}