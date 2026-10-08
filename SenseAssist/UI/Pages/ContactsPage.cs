using SenseAssist.Data;
using SenseAssist.Models;
using SenseAssist.UI.Controls;
using SenseAssist.UI.Forms;

namespace SenseAssist.UI.Pages;

public sealed class ContactsPage(User user) : CrudPage<Contact>(user, new ContactRepository())
{
    public override string Title => "Contacts";
    public override string Subtitle => "Family, carers, your doctor and audiologist. Emergency contacts are listed first.";
    protected override string Noun => "contact";

    public override string Summary
    {
        get
        {
            var emergency = Items.Count(c => c.IsEmergency);
            return $"You have {Items.Count} contact{(Items.Count == 1 ? "" : "s")}, {emergency} marked for emergencies.";
        }
    }

    public override string Help => "Contacts page. Add the people you rely on. Select a contact and press Space to hear their phone number read digit by digit.";

    protected override void ConfigureColumns(DataGridView grid)
    {
        GridStyler.AddColumn(grid, "", 8);
        GridStyler.AddColumn(grid, "Name", 32);
        GridStyler.AddColumn(grid, "Relation", 22);
        GridStyler.AddColumn(grid, "Phone", 24);
    }

    protected override object[] RowValues(Contact c) => [c.IsEmergency ? "★" : "", c.Name, c.Relation, c.Phone];

    protected override void StyleRow(DataGridViewRow row, Contact c)
    {
        row.Cells[0].Style.ForeColor = Theme.Warning;
        row.Cells[0].Style.Font = Theme.Font(13f, FontStyle.Bold);
        row.Cells[3].Style.Font = Theme.Font(10.5f, FontStyle.Bold);
    }

    protected override string DisplayName(Contact c) => c.Name;

    protected override Contact? ShowEditor(Contact? existing)
    {
        using var dialog = new ContactDialog(existing);
        return dialog.ShowDialog(FindForm()) == DialogResult.OK ? dialog.Result : null;
    }
}
