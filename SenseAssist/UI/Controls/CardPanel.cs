using System.Drawing.Drawing2D;

namespace SenseAssist.UI.Controls;

/// <summary>Rounded white surface with a soft border.</summary>
public class CardPanel : Panel
{
    public int Radius { get; set; } = 16;
    public Color? FillColor { get; set; }
    public Color? BorderColor { get; set; }

    public CardPanel()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Theme.Surface;
        Padding = new Padding(20);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Theme.Background);
        if (Width < 4 || Height < 4)
            return;

        using var path = Draw.Rounded(new RectangleF(0.5f, 0.5f, Width - 2f, Height - 2f), Radius);
        using var brush = new SolidBrush(FillColor ?? Theme.Surface);
        using var pen = new Pen(BorderColor ?? Theme.Border, Theme.HighContrast ? 2f : 1f);
        g.FillPath(brush, path);
        g.DrawPath(pen, path);
    }
}
