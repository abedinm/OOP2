using SenseAssist.Data;
using SenseAssist.Models;
using SenseAssist.Services;
using SenseAssist.UI.Controls;
using SenseAssist.UI.Forms;

namespace SenseAssist.UI.Pages;

public sealed class RemindersPage(User user) : CrudPage<Reminder>(user, new ReminderRepository())
{
    public override string Title => "Reminders";
    public override string Subtitle => "Medicine, appointments, hearing-aid batteries — spoken and shown on screen when due.";
    protected override string Noun => "reminder";

    public override string Summary
    {
        get
        {
            var open = Items.Count(r => !r.IsDone);
            var overdue = Items.Count(r => r.IsOverdue);
            return $"You have {open} open reminder{(open == 1 ? "" : "s")}" + (overdue > 0 ? $", {overdue} overdue." : ".");
        }
    }

    public override string Help => "Reminders page. Add a reminder with a title and time; when it is due, SenseAssist reads it aloud and flashes the screen. Mark done when you have finished it.";

    protected override void ConfigureColumns(DataGridView grid)
    {
        GridStyler.AddColumn(grid, "Status", 16);
        GridStyler.AddColumn(grid, "Reminder", 40);
        GridStyler.AddColumn(grid, "Due", 26);
        GridStyler.AddColumn(grid, "Category", 20);
    }

    protected override object[] RowValues(Reminder r) =>
    [
        r.IsDone ? "✔  Done" : r.IsOverdue ? "⚠  Overdue" : "●  Upcoming",
        r.Title,
        r.DueAt.ToString("ddd d MMM, h:mm tt"),
        r.Category,
    ];

    protected override void StyleRow(DataGridViewRow row, Reminder r)
    {
        var status = row.Cells[0].Style;
        status.Font = Theme.Font(10.5f, FontStyle.Bold);
        if (r.IsDone)
        {
            row.DefaultCellStyle.ForeColor = Theme.Muted;
            status.ForeColor = Theme.Success;
        }
        else
        {
            status.ForeColor = r.IsOverdue ? Theme.Danger : Theme.Accent;
        }
    }

    protected override string DisplayName(Reminder r) => r.Title;

    protected override Reminder? ShowEditor(Reminder? existing)
    {
        using var dialog = new ReminderDialog(existing);
        return dialog.ShowDialog(FindForm()) == DialogResult.OK ? dialog.Result : null;
    }

    protected override void AddExtraTools() =>
        Tool("Mark done", Glyph.Check, ButtonKind.Secondary, ToggleDone).AccessibleDescription = "Control D";

    public void ToggleDone()
    {
        if (Selected is not { } reminder)
        {
            Speech.Say("Select a reminder first.");
            return;
        }
        reminder.IsDone = !reminder.IsDone;
        Save(() => Repo.Update(reminder), "Update", reminder.IsDone ? $"Marked done: {reminder.Title}." : $"Marked not done: {reminder.Title}.");
    }
}
