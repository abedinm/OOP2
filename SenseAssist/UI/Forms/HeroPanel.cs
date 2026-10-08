using System.Drawing.Drawing2D;

namespace SenseAssist.UI.Forms;

/// <summary>Gradient brand panel on the sign-in screen.</summary>
sealed class HeroPanel : Panel
{
    public HeroPanel()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        AccessibleName = "SenseAssist. Your accessible daily companion.";
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using (var gradient = new LinearGradientBrush(ClientRectangle, Theme.HeroStart, Theme.HeroEnd, 60f))
            g.FillRectangle(gradient, ClientRectangle);

        if (Theme.HighContrast)
        {
            using var pen = new Pen(Color.Yellow, 3);
            g.DrawLine(pen, Width - 2, 0, Width - 2, Height);
            return;
        }

        // Soft decorative circles.
        using var bubble = new SolidBrush(Color.FromArgb(26, 255, 255, 255));
        g.FillEllipse(bubble, Width - 170, -70, 260, 260);
        g.FillEllipse(bubble, -90, Height - 210, 250, 250);
        g.FillEllipse(bubble, Width - 120, Height - 140, 90, 90);
    }
}
