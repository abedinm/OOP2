namespace SenseAssist.UI;

/// <summary>Helpers for vertical "label above field" forms.</summary>
public static class FormKit
{
    public static FlowLayoutPanel VerticalFlow() => new()
    {
        Dock = DockStyle.Fill,
        FlowDirection = FlowDirection.TopDown,
        WrapContents = false,
        AutoScroll = true,
        BackColor = Theme.Surface,
    };

    public static void AddField(FlowLayoutPanel flow, string label, Control input)
    {
        flow.Controls.Add(new Label
        {
            Text = label,
            AutoSize = true,
            Font = Theme.Font(10f, FontStyle.Bold),
            ForeColor = Theme.Text,
            BackColor = Theme.Surface,
            Margin = new Padding(2, 12, 0, 6),
        });
        input.Margin = new Padding(0, 0, 0, 4);
        flow.Controls.Add(input);
    }

    /// <summary>Make every non-label child as wide as the flow (call on Resize).</summary>
    public static void Stretch(FlowLayoutPanel flow)
    {
        var scrollbar = flow.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0;
        var width = flow.ClientSize.Width - flow.Padding.Horizontal - scrollbar - 4;
        foreach (Control child in flow.Controls)
        {
            if (child is Label { AutoSize: true })
                continue;
            child.Width = Math.Max(120, width - child.Margin.Horizontal);
        }
    }
}
