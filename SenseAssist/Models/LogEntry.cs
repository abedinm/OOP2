namespace SenseAssist.Models;

public sealed class LogEntry
{
    public int Id { get; set; }
    public DateTime At { get; set; }
    public string Action { get; set; } = "";
    public string Details { get; set; } = "";
}
