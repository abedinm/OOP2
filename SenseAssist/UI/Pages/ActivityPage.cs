using SenseAssist.Data;
using SenseAssist.Models;
using SenseAssist.Services;
using SenseAssist.UI.Controls;

namespace SenseAssist.UI.Pages;

/// <summary>Read-only history of the user's sign-ins, changes and alerts.</summary>
public sealed class ActivityPage : PageBase
{
    readonly User user;
    readonly DataGridView grid = new();
    readonly ModernButton refreshButton;
    List<LogEntry> entries = [];

    public ActivityPage(User user)
    {
        this.user = user;
        var card = new CardPanel { Dock = DockStyle.Fill, Padding = new Padding(18, 16, 18, 18) };
        var bar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = Theme.Px(58), BackColor = Theme.Surface, WrapContents = false };

        refreshButton = new ModernButton("Refresh", Glyph.Refresh, ButtonKind.Secondary);
        refreshButton.Click += (_, _) => { RefreshData(); Speech.Say(Summary); };
        var clear = new ModernButton("Clear history", Glyph.Delete, ButtonKind.Danger);
        clear.Click += (_, _) => ClearHistory();
        var read = new ModernButton("Read aloud", Glyph.Speak, ButtonKind.Ghost);
        read.Click += (_, _) => Speech.Say(ReadCurrent());
        bar.Controls.AddRange([refreshButton, clear, read]);

        GridStyler.Style(grid);
        grid.Dock = DockStyle.Fill;
        grid.AccessibleName = "Activity history";
        GridStyler.AddColumn(grid, "When", 22);
        GridStyler.AddColumn(grid, "Activity", 18);
        GridStyler.AddColumn(grid, "Details", 60);
        grid.SelectionChanged += (_, _) =>
        {
            if (grid.Focused && AppSettings.Current.ReadOnNavigate)
                Speech.Say(ReadCurrent());
        };

        card.Controls.Add(grid);
        card.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 8, BackColor = Theme.Surface });
        card.Controls.Add(bar);
        Controls.Add(card);
    }

    public override string Title => "Activity log";
    public override string Subtitle => "Every sign-in, change and alert is recorded here.";
    public override string Summary => $"{entries.Count} recent activit{(entries.Count == 1 ? "y" : "ies")}.";
    public override string Help => "Activity log page. Move through the list with the arrow keys to hear each entry. Clear history deletes your log.";
    public override Control? FirstFocus => grid.Rows.Count > 0 ? grid : refreshButton;

    public override string ReadCurrent() =>
        grid.CurrentRow?.Tag is LogEntry e ? $"{e.Action}, {e.At:dddd h:mm tt}. {e.Details}" : Summary;

    public override void RefreshData()
    {
        try
        {
            entries = ActivityLogger.Recent(user.Id);
        }
        catch (Exception ex)
        {
            AppLog.Error("Loading activity failed", ex);
            Speech.Say("Could not load the activity log.");
            return;
        }

        grid.Rows.Clear();
        foreach (var entry in entries)
        {
            var row = grid.Rows[grid.Rows.Add(entry.At.ToString("ddd d MMM, h:mm tt"), entry.Action, entry.Details)];
            row.Tag = entry;
            row.Cells[1].Style.Font = Theme.Font(10.5f, FontStyle.Bold);
            row.Cells[1].Style.ForeColor = entry.Action switch
            {
                "Delete" or "Login failed" => Theme.Danger,
                "Alert" => Theme.Warning,
                "Add" or "Register" => Theme.Success,
                _ => Theme.Accent,
            };
        }
    }

    void ClearHistory()
    {
        Speech.Say("Clear your whole activity history? Press Y for yes or N for no.");
        if (MessageBox.Show("Delete your whole activity history?", "Clear history", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        {
            Speech.Say("History kept.");
            return;
        }
        try
        {
            ActivityLogger.Clear(user.Id);
            ActivityLogger.Log(user.Id, "Clear log", "Activity history cleared");
            RefreshData();
            Speech.Say("Activity history cleared.");
        }
        catch (Exception ex)
        {
            AppLog.Error("Clearing activity failed", ex);
            Speech.Say("Could not clear the history.");
        }
    }
}
