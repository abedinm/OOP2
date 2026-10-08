using System.Speech.Synthesis;

namespace SenseAssist.Services;

/// <summary>
/// Text-to-speech for blind and low-vision users. Every message is also raised as an
/// <see cref="Announced"/> event so it can be shown as a caption for hard-of-hearing users.
/// </summary>
public static class Speech
{
    static SpeechSynthesizer? synthesizer;
    static bool unavailable;

    public static bool Enabled { get; set; } = true;
    public static int Rate { get; set; }

    public static event Action<string>? Announced;

    public static void Say(string text, bool caption = true)
    {
        if (string.IsNullOrWhiteSpace(text))
            return;
        if (caption)
            Announced?.Invoke(text);
        if (!Enabled || unavailable)
            return;

        try
        {
            if (synthesizer is null)
            {
                synthesizer = new SpeechSynthesizer();
                synthesizer.SetOutputToDefaultAudioDevice();
            }
            synthesizer.Rate = Math.Clamp(Rate, -10, 10);
            synthesizer.SpeakAsyncCancelAll();
            synthesizer.SpeakAsync(text);
        }
        catch (Exception ex)
        {
            unavailable = true;
            AppLog.Warn("Text-to-speech unavailable", ex);
        }
    }

    public static void Stop()
    {
        try { synthesizer?.SpeakAsyncCancelAll(); }
        catch { /* nothing to stop */ }
    }
}
