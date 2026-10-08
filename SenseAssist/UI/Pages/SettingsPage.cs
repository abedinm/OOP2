using SenseAssist.Services;
using SenseAssist.UI.Controls;

namespace SenseAssist.UI.Pages;

public sealed class SettingsPage : PageBase
{
    readonly ToggleSwitch speak, contrast, large, visual, readRows;
    readonly TrackBar rate;

    public SettingsPage(Action onSaved)
    {
        var s = AppSettings.Current;
        var card = new CardPanel { Dock = DockStyle.Fill, Padding = new Padding(Theme.Px(30), Theme.Px(22), Theme.Px(30), Theme.Px(22)) };
        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, BackColor = Theme.Surface };

        Section(flow, "Speech", "SenseAssist reads screens, list rows and alerts aloud.", first: true);
        speak = Add(flow, new ToggleSwitch("Speak aloud", s.SpeechEnabled));
        flow.Controls.Add(new Label { Text = "Speech speed  (slow ← → fast)", AutoSize = true, Font = Theme.Font(10.5f), ForeColor = Theme.Text, BackColor = Theme.Surface, Margin = new Padding(2, 10, 0, 0) });
        rate = new TrackBar { Minimum = -5, Maximum = 5, Value = Math.Clamp(s.SpeechRate, -5, 5), Width = Theme.Px(380), BackColor = Theme.Surface, AccessibleName = "Speech speed", TickFrequency = 1 };
        flow.Controls.Add(rate);
        var test = new ModernButton("Test voice", Glyph.Speak, ButtonKind.Secondary) { Margin = new Padding(0, 4, 0, 0) };
        test.Click += (_, _) =>
        {
            Speech.Rate = rate.Value;
            Speech.Say("This is how SenseAssist sounds at this speed.");
        };
        flow.Controls.Add(test);

        Section(flow, "Display", "Make everything easier to see.");
        contrast = Add(flow, new ToggleSwitch("High contrast — yellow on black", s.HighContrast));
        large = Add(flow, new ToggleSwitch("Large text", s.LargeText));

        Section(flow, "Alerts", "How you are told that a reminder is due.");
        visual = Add(flow, new ToggleSwitch("Flash the screen and captions for alerts", s.VisualAlerts));
        readRows = Add(flow, new ToggleSwitch("Read list rows aloud as I move through them", s.ReadOnNavigate));

        var save = new ModernButton("Save settings", Glyph.Save) { Margin = new Padding(0, Theme.Px(22), 0, 0), Height = Theme.Px(48) };
        save.Click += (_, _) =>
        {
            s.SpeechEnabled = speak.Checked;
            s.SpeechRate = rate.Value;
            s.HighContrast = contrast.Checked;
            s.LargeText = large.Checked;
            s.VisualAlerts = visual.Checked;
            s.ReadOnNavigate = readRows.Checked;
            s.Save();
            onSaved();
        };
        flow.Controls.Add(save);

        card.Controls.Add(flow);
        Controls.Add(card);
    }

    public override string Title => "Settings";
    public override string Subtitle => "Speech, contrast, text size and alerts — saved on this computer.";
    public override string Summary => "Use Tab to move between options and Space to switch them on or off. Save settings applies them.";
    public override string Help => Summary;
    public override Control? FirstFocus => speak;

    static ToggleSwitch Add(FlowLayoutPanel flow, ToggleSwitch toggle)
    {
        flow.Controls.Add(toggle);
        return toggle;
    }

    static void Section(FlowLayoutPanel flow, string title, string description, bool first = false)
    {
        flow.Controls.Add(new Label { Text = title, AutoSize = true, Font = Theme.Font(14f, FontStyle.Bold), ForeColor = Theme.Text, BackColor = Theme.Surface, Margin = new Padding(0, first ? 0 : Theme.Px(22), 0, 2) });
        flow.Controls.Add(new Label { Text = description, AutoSize = true, Font = Theme.Font(10f), ForeColor = Theme.Muted, BackColor = Theme.Surface, Margin = new Padding(1, 0, 0, 8) });
    }
}
