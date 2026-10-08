using SenseAssist.Models;
using SenseAssist.UI.Controls;

namespace SenseAssist.UI.Forms;

public sealed class ReminderDialog : EditDialogBase
{
    readonly Reminder? original;
    readonly InputBox title = new("Reminder title", placeholder: "e.g. Take blood pressure tablet");
    readonly InputBox details = new("Details", multiline: true, placeholder: "Optional notes, read aloud with the reminder");
    readonly DateTimePicker due = new()
    {
        Format = DateTimePickerFormat.Custom,
        CustomFormat = "dddd, d MMMM yyyy    h:mm tt",
        Font = Theme.Font(11.5f),
        AccessibleName = "Due date and time",
    };
    readonly ComboBox category = new()
    {
        DropDownStyle = ComboBoxStyle.DropDownList,
        Font = Theme.Font(11.5f),
        FlatStyle = FlatStyle.Flat,
        AccessibleName = "Category",
    };

    public Reminder? Result { get; private set; }

    public ReminderDialog(Reminder? existing)
        : base(existing is null ? "Add reminder" : "Edit reminder", "Reminders are read aloud and flashed on screen when they are due.")
    {
        original = existing;
        category.Items.AddRange(["Medicine", "Appointment", "Hearing aid battery", "Daily task", "Other"]);

        AddField("Title", title);
        AddField("When", due);
        AddField("Category", category);
        AddField("Details", details);

        if (existing is null)
        {
            var nextHour = DateTime.Now.AddHours(1);
            due.Value = new DateTime(nextHour.Year, nextHour.Month, nextHour.Day, nextHour.Hour, 0, 0);
            category.SelectedIndex = 0;
        }
        else
        {
            title.Text = existing.Title;
            details.Text = existing.Details;
            due.Value = existing.DueAt;
            category.SelectedItem = category.Items.Contains(existing.Category) ? existing.Category : "Other";
        }
    }

    protected override bool TryBuild(out string problem)
    {
        if (title.Text.Trim().Length == 0)
        {
            problem = "Please give the reminder a title.";
            title.Box.Focus();
            return false;
        }

        Result = new Reminder
        {
            Id = original?.Id ?? 0,
            UserId = original?.UserId ?? 0,
            IsDone = original?.IsDone ?? false,
            Title = title.Text.Trim(),
            Details = details.Text.Trim(),
            DueAt = due.Value,
            Category = category.Text,
        };
        problem = "";
        return true;
    }
}
