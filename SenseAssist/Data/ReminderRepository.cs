using SenseAssist.Models;

namespace SenseAssist.Data;

public sealed class ReminderRepository : IRepository<Reminder>
{
    public List<Reminder> GetAll(int userId)
    {
        using var connection = Db.Open();
        using var command = Db.Command(connection,
            "SELECT Id, UserId, Title, Details, DueAt, Category, IsDone FROM dbo.Reminders WHERE UserId = @user ORDER BY IsDone, DueAt",
            ("@user", userId));
        using var reader = command.ExecuteReader();

        var reminders = new List<Reminder>();
        while (reader.Read())
        {
            reminders.Add(new Reminder
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                Title = reader.GetString(2),
                Details = reader.IsDBNull(3) ? "" : reader.GetString(3),
                DueAt = reader.GetDateTime(4),
                Category = reader.GetString(5),
                IsDone = reader.GetBoolean(6),
            });
        }
        return reminders;
    }

    public void Add(Reminder item)
    {
        using var connection = Db.Open();
        using var command = Db.Command(connection,
            "INSERT INTO dbo.Reminders (UserId, Title, Details, DueAt, Category, IsDone) OUTPUT INSERTED.Id VALUES (@user, @title, @details, @due, @category, @done)",
            ("@user", item.UserId), ("@title", item.Title), ("@details", item.Details), ("@due", item.DueAt), ("@category", item.Category), ("@done", item.IsDone));
        item.Id = (int)command.ExecuteScalar()!;
    }

    public void Update(Reminder item)
    {
        using var connection = Db.Open();
        using var command = Db.Command(connection,
            "UPDATE dbo.Reminders SET Title = @title, Details = @details, DueAt = @due, Category = @category, IsDone = @done WHERE Id = @id AND UserId = @user",
            ("@title", item.Title), ("@details", item.Details), ("@due", item.DueAt), ("@category", item.Category), ("@done", item.IsDone), ("@id", item.Id), ("@user", item.UserId));
        command.ExecuteNonQuery();
    }

    public void Delete(int id, int userId)
    {
        using var connection = Db.Open();
        using var command = Db.Command(connection, "DELETE FROM dbo.Reminders WHERE Id = @id AND UserId = @user", ("@id", id), ("@user", userId));
        command.ExecuteNonQuery();
    }
}
