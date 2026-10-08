using SenseAssist.Models;
using SenseAssist.UI.Controls;

namespace SenseAssist.UI.Forms;

public sealed class ContactDialog : EditDialogBase
{
    readonly Contact? original;
    readonly InputBox name = new("Name", placeholder: "e.g. Dr. Rahman");
    readonly InputBox phone = new("Phone number", placeholder: "e.g. 01712 345678");
    readonly InputBox notes = new("Notes", multiline: true, placeholder: "Optional, e.g. best time to call");
    readonly ComboBox relation = new()
    {
        DropDownStyle = ComboBoxStyle.DropDown,
        Font = Theme.Font(11.5f),
        FlatStyle = FlatStyle.Flat,
        AccessibleName = "Relation",
    };
    readonly ToggleSwitch emergency = new("Emergency contact");

    public Contact? Result { get; private set; }

    public ContactDialog(Contact? existing)
        : base(existing is null ? "Add contact" : "Edit contact", "Emergency contacts are listed first and read out clearly.")
    {
        original = existing;
        relation.Items.AddRange(["Family", "Friend", "Caregiver", "Doctor", "Audiologist", "Neighbour"]);

        AddField("Name", name);
        AddField("Phone", phone);
        AddField("Relation", relation);
        AddField("Notes", notes);
        Fields.Controls.Add(emergency);
        emergency.Margin = new Padding(0, 14, 0, 0);

        if (existing != null)
        {
            name.Text = existing.Name;
            phone.Text = existing.Phone;
            relation.Text = existing.Relation;
            notes.Text = existing.Notes;
            emergency.Checked = existing.IsEmergency;
        }
    }

    protected override bool TryBuild(out string problem)
    {
        var phoneText = phone.Text.Trim();
        if (name.Text.Trim().Length == 0)
        {
            problem = "Please enter a name.";
            name.Box.Focus();
            return false;
        }
        if (phoneText.Count(char.IsDigit) < 5 || phoneText.Any(ch => !char.IsDigit(ch) && ch is not (' ' or '+' or '-' or '(' or ')')))
        {
            problem = "Please enter a phone number using digits.";
            phone.Box.Focus();
            return false;
        }

        Result = new Contact
        {
            Id = original?.Id ?? 0,
            UserId = original?.UserId ?? 0,
            Name = name.Text.Trim(),
            Phone = phoneText,
            Relation = relation.Text.Trim(),
            Notes = notes.Text.Trim(),
            IsEmergency = emergency.Checked,
        };
        problem = "";
        return true;
    }
}
