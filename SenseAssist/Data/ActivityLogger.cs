using SenseAssist.Models;
using SenseAssist.Services;

namespace SenseAssist.Data;

/// <summary>Records what each user does (sign-ins, changes, alerts) in the database and the log file.</summary>
public static class ActivityLogger
{
    public static void Log(int? userId, string action, string details)
    {
        AppLog.Info($"[user {userId?.ToString() ?? "-"}] {action}: {details}");
        try
        {
            using var connection = Db.Open();
            using var command = Db.Command(connection,
                "INSERT INTO dbo.ActivityLog (UserId, Action, Details, At) VALUES (@user, @action, @details, @at)",
                ("@user", userId), ("@action", action), ("@details", details.Length > 400 ? details[..400] : details), ("@at", DateTime.Now));
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            AppLog.Warn("Could not write activity log", ex);
        }
    }

    public static List<LogEntry> Recent(int userId, int max = 200)
    {
        using var connection = Db.Open();
        using var command = Db.Command(connection,
            "SELECT TOP (@max) Id, At, Action, Details FROM dbo.ActivityLog WHERE UserId = @user ORDER BY At DESC",
            ("@max", max), ("@user", userId));
        using var reader = command.ExecuteReader();

        var entries = new List<LogEntry>();
        while (reader.Read())
        {
            entries.Add(new LogEntry
            {
                Id = reader.GetInt32(0),
                At = reader.GetDateTime(1),
                Action = reader.GetString(2),
                Details = reader.IsDBNull(3) ? "" : reader.GetString(3),
            });
        }
        return entries;
    }

    public static void Clear(int userId)
    {
        using var connection = Db.Open();
        using var command = Db.Command(connection, "DELETE FROM dbo.ActivityLog WHERE UserId = @user", ("@user", userId));
        command.ExecuteNonQuery();
    }
}
