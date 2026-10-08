namespace SenseAssist.Models;

public sealed class Reminder : Entity
{
    public string Title { get; set; } = "";
    public string Details { get; set; } = "";
    public DateTime DueAt { get; set; }
    public string Category { get; set; } = "Other";
    public bool IsDone { get; set; }

    public bool IsOverdue => !IsDone && DueAt < DateTime.Now;

    public override string Describe()
    {
        var status = IsDone ? "Done." : IsOverdue ? "Overdue." : "";
        return $"{Category} reminder: {Title}. Due {DueAt:dddd, MMMM d 'at' h:mm tt}. {status} {Details}".Trim();
    }
}
