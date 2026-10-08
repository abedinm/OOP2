using System.Text.Json;
using SenseAssist.UI;

namespace SenseAssist.Services;

public sealed class AppSettings
{
    public bool SpeechEnabled { get; set; } = true;
    public int SpeechRate { get; set; }
    public bool HighContrast { get; set; }
    public bool LargeText { get; set; }
    public bool VisualAlerts { get; set; } = true;
    public bool ReadOnNavigate { get; set; } = true;

    static readonly string FilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SenseAssist", "settings.json");

    static AppSettings? current;
    public static AppSettings Current => current ??= Load();

    static AppSettings Load()
    {
        try
        {
            if (File.Exists(FilePath))
                return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(FilePath)) ?? new AppSettings();
        }
        catch (Exception ex)
        {
            AppLog.Warn("Settings file unreadable, using defaults", ex);
        }
        return new AppSettings();
    }

    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        File.WriteAllText(FilePath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
        ApplyToRuntime();
    }

    public void ApplyToRuntime()
    {
        Speech.Enabled = SpeechEnabled;
        Speech.Rate = SpeechRate;
        Theme.HighContrast = HighContrast;
        Theme.LargeText = LargeText;
    }
}
