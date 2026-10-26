using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;

namespace SmokeTest;

// Checks the whole OOP2 toolchain: WinForms UI + ADO.NET -> SQL Server on the Mac.
public class MainForm : Form
{
    private readonly Label statusLabel = new() { Dock = DockStyle.Top, Height = 40, Padding = new Padding(8), Text = "Click the button to test the database connection." };
    private readonly Button testButton = new() { Dock = DockStyle.Top, Height = 40, Text = "Test SQL Server" };
    private readonly DataGridView grid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

    public MainForm()
    {
        Text = "OOP2 Smoke Test";
        Width = 640;
        Height = 420;
        Controls.Add(grid);
        Controls.Add(testButton);
        Controls.Add(statusLabel);
        testButton.Click += (_, _) => RunTest();
    }

    private void RunTest()
    {
        try
        {
            using var conn = new SqlConnection(ConnectionString());
            conn.Open();

            Execute(conn, """
                IF OBJECT_ID('dbo.SmokeTest') IS NULL
                    CREATE TABLE dbo.SmokeTest (Id INT IDENTITY PRIMARY KEY, Machine NVARCHAR(100), RunAt DATETIME2 DEFAULT SYSDATETIME());
                """);

            using (var insert = new SqlCommand("INSERT INTO dbo.SmokeTest (Machine) VALUES (@machine)", conn))
            {
                insert.Parameters.AddWithValue("@machine", Environment.MachineName);
                insert.ExecuteNonQuery();
            }

            var table = new DataTable();
            using (var adapter = new SqlDataAdapter("SELECT Id, Machine, RunAt FROM dbo.SmokeTest ORDER BY Id DESC", conn))
                adapter.Fill(table);
            grid.DataSource = table;

            statusLabel.Text = $"Connected to {conn.DataSource} / {conn.Database} - {table.Rows.Count} row(s).";
        }
        catch (Exception ex)
        {
            statusLabel.Text = "Failed: " + ex.Message;
        }
    }

    private static void Execute(SqlConnection conn, string sql)
    {
        using var cmd = new SqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
    }

    // The sa password lives in the repo's gitignored .env. Check the repo root recorded at build
    // time (bin/ is outside the repo on Windows), then walk up from the exe.
    private static string ConnectionString()
    {
        var repoRoot = typeof(MainForm).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(a => a.Key == "RepoRoot")?.Value;
        var dir = !string.IsNullOrEmpty(repoRoot) && File.Exists(Path.Combine(repoRoot, ".env"))
            ? new DirectoryInfo(repoRoot)
            : new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, ".env")))
            dir = dir.Parent;
        if (dir == null)
            throw new FileNotFoundException(".env not found above " + AppContext.BaseDirectory);

        var password = File.ReadAllLines(Path.Combine(dir.FullName, ".env"))
            .Select(line => line.Split('=', 2))
            .First(parts => parts[0] == "MSSQL_SA_PASSWORD")[1];

        return new SqlConnectionStringBuilder
        {
            DataSource = "172.16.37.1,1433",
            InitialCatalog = "OOP2",
            UserID = "sa",
            Password = password,
            TrustServerCertificate = true,
        }.ConnectionString;
    }
}
