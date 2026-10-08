using SenseAssist.Models;

namespace SenseAssist.Data;

public sealed class UserRepository
{
    public User? FindByUsername(string username)
    {
        using var connection = Db.Open();
        using var command = Db.Command(connection,
            "SELECT Id, Username, FullName, Needs, PasswordHash, PasswordSalt FROM dbo.Users WHERE Username = @username",
            ("@username", username));
        using var reader = command.ExecuteReader();
        if (!reader.Read())
            return null;

        return new User
        {
            Id = reader.GetInt32(0),
            Username = reader.GetString(1),
            FullName = reader.GetString(2),
            Needs = reader.GetString(3),
            PasswordHash = (byte[])reader[4],
            PasswordSalt = (byte[])reader[5],
        };
    }

    public void Add(User user)
    {
        using var connection = Db.Open();
        using var command = Db.Command(connection,
            "INSERT INTO dbo.Users (Username, FullName, Needs, PasswordHash, PasswordSalt) OUTPUT INSERTED.Id VALUES (@username, @name, @needs, @hash, @salt)",
            ("@username", user.Username), ("@name", user.FullName), ("@needs", user.Needs), ("@hash", user.PasswordHash), ("@salt", user.PasswordSalt));
        user.Id = (int)command.ExecuteScalar()!;
    }
}
