using System.Drawing.Drawing2D;

namespace SenseAssist.UI.Controls;

/// <summary>On/off switch. Still a CheckBox underneath, so screen readers announce it as checked/unchecked.</summary>
public class ToggleSwitch : CheckBox
{
    public ToggleSwitch(string text, bool isOn = false)
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Text = text;
        AccessibleName = text;
        Checked = isOn;
        Font = Theme.Font(11f);
        AutoSize = false;
        Height = Theme.Px(40);
        Width = Theme.Px(480);
        Cursor = Cursors.Hand;
        Margin = new Padding(0, 4, 0, 4);
    }

    protected override void OnCheckedChanged(EventArgs e) { Invalidate(); base.OnCheckedChanged(e); }
    protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
    protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Theme.Surface);

        float w = Theme.Px(46), h = Theme.Px(26);
        var track = new RectangleF(3, (Height - h) / 2f, w, h);
        using (var path = Draw.Rounded(track, h / 2f))
        {
            using var brush = new SolidBrush(Checked ? Theme.Accent : Theme.SwitchOff);
            g.FillPath(brush, path);
            if (Focused)
            {
                using var ring = new Pen(Theme.FocusRing, 3f);
                g.DrawPath(ring, path);
            }
        }

        var knob = h - 6;
        var knobX = Checked ? track.Right - knob - 3 : track.X + 3;
        using (var brush = new SolidBrush(Theme.HighContrast && Checked ? Color.Black : Color.White))
            g.FillEllipse(brush, knobX, track.Y + 3, knob, knob);

        var textX = (int)(track.Right + 14);
        TextRenderer.DrawText(g, Text, Font, new Rectangle(textX, 0, Width - textX, Height), Theme.Text,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }
}
