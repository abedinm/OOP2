namespace SenseAssist.Models;

public sealed class Contact : Entity
{
    public string Name { get; set; } = "";
    public string Relation { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Notes { get; set; } = "";
    public bool IsEmergency { get; set; }

    public override string Describe()
    {
        var kind = IsEmergency ? "Emergency contact" : "Contact";
        var relation = Relation.Length > 0 ? $", {Relation}" : "";
        // Spell the number digit by digit so speech reads "0 1 7 1..." instead of a huge number.
        var digits = string.Join(" ", Phone.Where(char.IsDigit));
        return $"{kind}: {Name}{relation}. Phone {digits}. {Notes}".Trim();
    }
}
