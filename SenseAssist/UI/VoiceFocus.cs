using SenseAssist.Services;

namespace SenseAssist.UI;

/// <summary>
/// Built-in self-voicing: speaks the name (and value) of whatever control receives focus,
/// so the app is usable by blind users even without a separate screen reader.
/// </summary>
public static class VoiceFocus
{
    public static bool Muted { get; set; }

    public static void Attach(Control root)
    {
        foreach (Control control in root.Controls)
        {
            if (control is TextBox or ComboBox or DateTimePicker or CheckBox or Button or TrackBar)
                control.Enter += Announce;
            Attach(control);
        }
    }

    /// <summary>Move focus without announcing it (used when the app already says something better).</summary>
    public static void QuietFocus(Control control)
    {
        Muted = true;
        control.Focus();
        Muted = false;
    }

    static void Announce(object? sender, EventArgs e)
    {
        if (!Muted && sender is Control control)
            Speech.Say(Describe(control), caption: false);
    }

    static string Describe(Control c) => c switch
    {
        TextBox t when t.UseSystemPasswordChar => $"{t.AccessibleName}, password field.",
        TextBox t => $"{t.AccessibleName}, edit field. {(t.Text.Length > 0 ? t.Text : "Empty.")}",
        ComboBox box => $"{box.AccessibleName}, list. {box.Text}",
        DateTimePicker picker => $"{picker.AccessibleName}. {picker.Value:dddd, MMMM d, h:mm tt}",
        CheckBox check => $"{check.AccessibleName}, {(check.Checked ? "on" : "off")}. Press space to change.",
        TrackBar bar => $"{bar.AccessibleName}, {bar.Value}",
        _ => $"{c.AccessibleName ?? c.Text}, button. {c.AccessibleDescription}",
    };
}
