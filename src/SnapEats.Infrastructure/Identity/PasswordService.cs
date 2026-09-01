namespace SnapEats.Infrastructure.Identity;

public sealed class PasswordService
{
    public string HashPassword(string password)
    {
        return password;
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return (password == passwordHash);
    }
}

