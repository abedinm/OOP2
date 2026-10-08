using System.Security.Cryptography;
using SenseAssist.Data;
using SenseAssist.Models;

namespace SenseAssist.Services;

/// <summary>Thrown for problems the user can fix (bad input, wrong password).</summary>
public sealed class InputException(string message) : Exception(message);

public sealed class AuthService
{
    readonly UserRepository users = new();

    public User Register(string fullName, string username, string password, string confirm, string needs)
    {
        fullName = fullName.Trim();
        username = username.Trim().ToLowerInvariant();

        if (fullName.Length < 2)
            throw new InputException("Please enter your full name.");
        if (username.Length < 3 || !username.All(ch => char.IsLetterOrDigit(ch) || ch is '.' or '_'))
            throw new InputException("Username must be at least 3 letters or numbers.");
        if (password.Length < 6)
            throw new InputException("Password must be at least 6 characters.");
        if (password != confirm)
            throw new InputException("The two passwords do not match.");
        if (users.FindByUsername(username) is not null)
            throw new InputException("That username is already taken.");

        var salt = RandomNumberGenerator.GetBytes(16);
        var user = new User
        {
            FullName = fullName,
            Username = username,
            Needs = needs,
            PasswordSalt = salt,
            PasswordHash = Hash(password, salt),
        };
        users.Add(user);
        ActivityLogger.Log(user.Id, "Register", $"Account created ({needs})");
        return user;
    }

    public User Login(string username, string password)
    {
        var user = users.FindByUsername(username.Trim().ToLowerInvariant());
        if (user is null || !CryptographicOperations.FixedTimeEquals(Hash(password, user.PasswordSalt), user.PasswordHash))
        {
            ActivityLogger.Log(user?.Id, "Login failed", $"Username '{username.Trim()}'");
            throw new InputException("Wrong username or password.");
        }

        ActivityLogger.Log(user.Id, "Login", "Signed in");
        return user;
    }

    public void Logout(User user) => ActivityLogger.Log(user.Id, "Logout", "Signed out");

    static byte[] Hash(string password, byte[] salt) =>
        Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);
}
