using System.Drawing.Drawing2D;

namespace SenseAssist.UI.Controls;

/// <summary>Round badge with the user's initials.</summary>
public sealed class Avatar : Control
{
    readonly string initials;

    public Avatar(string name)
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        initials = string.Concat(name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(p => char.ToUpper(p[0])));
        AccessibleRole = AccessibleRole.Graphic;
        AccessibleName = $"{name} avatar";
        TabStop = false;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Theme.Sidebar);
        using var brush = new SolidBrush(Theme.Accent);
        g.FillEllipse(brush, 1, 1, Width - 3, Height - 3);
        TextRenderer.DrawText(g, initials, Theme.Font(12f, FontStyle.Bold), ClientRectangle, Theme.OnAccent,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }
}
