using SenseAssist.Services;
using SenseAssist.UI.Controls;

namespace SenseAssist.UI.Forms;

/// <summary>Common look and behaviour for add/edit dialogs: header, fields, Save/Cancel, Ctrl+S, spoken errors.</summary>
public abstract class EditDialogBase : Form
{
    protected readonly FlowLayoutPanel Fields;
    readonly Label error;
    readonly ModernButton saveButton;
    readonly string spokenIntro;
    Control? firstField;

    protected EditDialogBase(string title, string subtitle)
    {
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Text = title;
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = MinimizeBox = false;
        ShowInTaskbar = false;
        BackColor = Theme.Surface;
        Font = Theme.Font(10.5f);
        ClientSize = new Size(Theme.Px(560), Theme.Px(660));
        KeyPreview = true;
        spokenIntro = $"{title}. {subtitle}";

        var header = new Panel { Dock = DockStyle.Top, Height = Theme.Px(92), BackColor = Theme.Surface, Padding = new Padding(Theme.Px(28), Theme.Px(20), Theme.Px(28), 0) };
        header.Controls.Add(new Label { Text = subtitle, Dock = DockStyle.Top, Height = Theme.Px(28), Font = Theme.Font(10f), ForeColor = Theme.Muted, BackColor = Theme.Surface });
        header.Controls.Add(new Label { Text = title, Dock = DockStyle.Top, Height = Theme.Px(38), Font = Theme.Font(17f, FontStyle.Bold), ForeColor = Theme.Text, BackColor = Theme.Surface });

        Fields = FormKit.VerticalFlow();
        Fields.Padding = new Padding(Theme.Px(28), 0, Theme.Px(28), 0);
        Fields.Resize += (_, _) => FormKit.Stretch(Fields);

        error = new Label { Dock = DockStyle.Bottom, Height = Theme.Px(30), ForeColor = Theme.Danger, BackColor = Theme.Surface, Font = Theme.Font(10.5f, FontStyle.Bold), Padding = new Padding(Theme.Px(28), 0, 0, 0), TextAlign = ContentAlignment.MiddleLeft };

        var footer = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = Theme.Px(78), FlowDirection = FlowDirection.RightToLeft, BackColor = Theme.SurfaceAlt, Padding = new Padding(Theme.Px(20), Theme.Px(18), Theme.Px(20), 0) };
        saveButton = new ModernButton("Save", Glyph.Save) { Width = Theme.Px(130), AccessibleDescription = "Control S" };
        var cancel = new ModernButton("Cancel", "", ButtonKind.Secondary) { Width = Theme.Px(110), DialogResult = DialogResult.Cancel, AccessibleDescription = "Escape" };
        saveButton.Click += (_, _) => TrySave();
        footer.Controls.Add(saveButton);
        footer.Controls.Add(cancel);
        CancelButton = cancel;

        Controls.Add(Fields);
        Controls.Add(error);
        Controls.Add(footer);
        Controls.Add(header);
        Fields.BringToFront();
    }

    /// <summary>Validate the fields and build the result. Return false with a friendly message if something is wrong.</summary>
    protected abstract bool TryBuild(out string problem);

    protected void AddField(string label, Control input)
    {
        FormKit.AddField(Fields, label, input);
        firstField ??= input is InputBox box ? box.Box : input;
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        FormKit.Stretch(Fields);
        VoiceFocus.Attach(this);
        if (firstField != null)
            VoiceFocus.QuietFocus(firstField);
        Speech.Say($"{spokenIntro} Press Control S to save or Escape to cancel. You are in the {firstField?.AccessibleName} field.");
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == (Keys.Control | Keys.S))
        {
            TrySave();
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    void TrySave()
    {
        if (TryBuild(out var problem))
        {
            DialogResult = DialogResult.OK;
            return;
        }
        error.Text = "⚠  " + problem;
        Speech.Say(problem);
    }
}
