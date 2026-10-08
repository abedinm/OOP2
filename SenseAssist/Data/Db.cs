using Microsoft.Data.SqlClient;

namespace SenseAssist.Data;

/// <summary>Connection handling and schema creation for the SenseAssist database.</summary>
public static class Db
{
    public const string Server = "172.16.37.1,1433";
    public const string Database = "SenseAssist";

    static string? connectionString;

    public static SqlConnection Open()
    {
        var connection = new SqlConnection(connectionString ??= Build(Database));
        connection.Open();
        return connection;
    }

    public static SqlCommand Command(SqlConnection connection, string sql, params (string Name, object? Value)[] parameters)
    {
        var command = new SqlCommand(sql, connection);
        foreach (var (name, value) in parameters)
            command.Parameters.AddWithValue(name, value ?? DBNull.Value);
        return command;
    }

    public static void EnsureSchema()
    {
        using (var master = new SqlConnection(Build("master")))
        {
            master.Open();
            using var create = new SqlCommand($"IF DB_ID('{Database}') IS NULL CREATE DATABASE [{Database}]", master);
            create.ExecuteNonQuery();
        }

        using var connection = Open();
        using var command = new SqlCommand(SchemaSql, connection);
        command.ExecuteNonQuery();
    }

    static string Build(string database) => new SqlConnectionStringBuilder
    {
        DataSource = Server,
        InitialCatalog = database,
        UserID = "sa",
        Password = FindPassword(),
        TrustServerCertificate = true,
        ConnectTimeout = 10,
    }.ConnectionString;

    // The sa password lives in the repo's gitignored .env (seen in the VM through the Z: drive).
    static string FindPassword()
    {
        var fromEnvironment = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
        if (!string.IsNullOrEmpty(fromEnvironment))
            return fromEnvironment;

        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir != null; dir = dir.Parent)
        {
            var envFile = Path.Combine(dir.FullName, ".env");
            if (!File.Exists(envFile))
                continue;
            var line = File.ReadAllLines(envFile).FirstOrDefault(l => l.StartsWith("MSSQL_SA_PASSWORD="));
            if (line != null)
                return line["MSSQL_SA_PASSWORD=".Length..];
        }
        throw new InvalidOperationException("Database password not found. Expected MSSQL_SA_PASSWORD in the repo's .env file.");
    }

    const string SchemaSql = """
        IF OBJECT_ID('dbo.Users') IS NULL
            CREATE TABLE dbo.Users (
                Id INT IDENTITY PRIMARY KEY,
                Username NVARCHAR(50) NOT NULL UNIQUE,
                FullName NVARCHAR(100) NOT NULL,
                Needs NVARCHAR(30) NOT NULL,
                PasswordHash VARBINARY(32) NOT NULL,
                PasswordSalt VARBINARY(16) NOT NULL,
                CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME());

        IF OBJECT_ID('dbo.Reminders') IS NULL
            CREATE TABLE dbo.Reminders (
                Id INT IDENTITY PRIMARY KEY,
                UserId INT NOT NULL REFERENCES dbo.Users(Id) ON DELETE CASCADE,
                Title NVARCHAR(120) NOT NULL,
                Details NVARCHAR(1000) NULL,
                DueAt DATETIME2 NOT NULL,
                Category NVARCHAR(40) NOT NULL,
                IsDone BIT NOT NULL DEFAULT 0);

        IF OBJECT_ID('dbo.Contacts') IS NULL
            CREATE TABLE dbo.Contacts (
                Id INT IDENTITY PRIMARY KEY,
                UserId INT NOT NULL REFERENCES dbo.Users(Id) ON DELETE CASCADE,
                Name NVARCHAR(100) NOT NULL,
                Relation NVARCHAR(50) NULL,
                Phone NVARCHAR(30) NOT NULL,
                Notes NVARCHAR(500) NULL,
                IsEmergency BIT NOT NULL DEFAULT 0);

        IF OBJECT_ID('dbo.ActivityLog') IS NULL
            CREATE TABLE dbo.ActivityLog (
                Id INT IDENTITY PRIMARY KEY,
                UserId INT NULL,
                Action NVARCHAR(40) NOT NULL,
                Details NVARCHAR(400) NULL,
                At DATETIME2 NOT NULL);
        """;
}
