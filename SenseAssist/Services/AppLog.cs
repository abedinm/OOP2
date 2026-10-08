namespace SenseAssist.Services;

/// <summary>Plain-text log file in %LOCALAPPDATA%\SenseAssist\logs, one file per day.</summary>
public static class AppLog
{
    static readonly object Gate = new();

    public static string Folder { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SenseAssist", "logs");

    public static void Info(string message) => Write("INFO", message);
    public static void Warn(string message, Exception? ex = null) => Write("WARN", ex is null ? message : $"{message}: {ex}");
    public static void Error(string message, Exception? ex = null) => Write("ERROR", ex is null ? message : $"{message}: {ex}");

    static void Write(string level, string message)
    {
        try
        {
            lock (Gate)
            {
                Directory.CreateDirectory(Folder);
                File.AppendAllText(
                    Path.Combine(Folder, $"app-{DateTime.Now:yyyyMMdd}.log"),
                    $"{DateTime.Now:HH:mm:ss.fff} [{level}] {message}{Environment.NewLine}");
            }
        }
        catch
        {
            // Logging must never crash the app.
        }
    }
}
