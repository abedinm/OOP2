using System.Drawing.Drawing2D;

namespace SenseAssist.UI.Controls;

public enum ButtonKind { Primary, Secondary, Danger, Ghost, Nav, NavDanger }

/// <summary>Rounded, icon + text button with a thick, high-visibility focus ring for keyboard users.</summary>
public class ModernButton : Button
{
    bool hover, pressed, selected;
    ButtonKind kind;

    public string Glyph { get; }
    public ButtonKind Kind { get => kind; set { kind = value; Invalidate(); } }
    public bool Selected { get => selected; set { selected = value; Invalidate(); } }

    public ModernButton(string text, string glyph = "", ButtonKind kind = ButtonKind.Primary)
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        this.kind = kind;
        Glyph = glyph;
        Text = text;
        AccessibleName = text;
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        Cursor = Cursors.Hand;

        var nav = kind is ButtonKind.Nav or ButtonKind.NavDanger;
        Font = Theme.Font(nav ? 11f : 10.5f, nav ? FontStyle.Regular : FontStyle.Bold);
        Height = Theme.Px(nav ? 46 : 42);
        Width = TextRenderer.MeasureText(text, Font).Width + (glyph.Length > 0 ? Theme.Px(60) : Theme.Px(36));
        Margin = new Padding(0, 0, 10, 0);
    }

    protected override void OnMouseEnter(EventArgs e) { hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { hover = pressed = false; Invalidate(); base.OnMouseLeave(e); }
    protected override void OnMouseDown(MouseEventArgs e) { pressed = true; Invalidate(); base.OnMouseDown(e); }
    protected override void OnMouseUp(MouseEventArgs e) { pressed = false; Invalidate(); base.OnMouseUp(e); }
    protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
    protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Theme.Background);

        var (back, fore, border) = Palette();
        using (var path = Draw.Rounded(new RectangleF(1.5f, 1.5f, Width - 3f, Height - 3f), 10))
        {
            if (back.A > 0)
            {
                using var brush = new SolidBrush(back);
                g.FillPath(brush, path);
            }
            if (border.A > 0)
            {
                using var pen = new Pen(border, 1.5f);
                g.DrawPath(pen, path);
            }
            if (Focused)
            {
                using var ring = new Pen(Theme.FocusRing, 3f);
                g.DrawPath(ring, path);
            }
        }

        var nav = Kind is ButtonKind.Nav or ButtonKind.NavDanger;
        var iconWidth = Glyph.Length > 0 ? Theme.Px(22) : 0;
        var gap = iconWidth > 0 ? 10 : 0;
        var textWidth = TextRenderer.MeasureText(g, Text, Font).Width;
        var x = nav ? 18 : Math.Max(8, (Width - iconWidth - gap - textWidth) / 2);

        const TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine;
        if (iconWidth > 0)
            TextRenderer.DrawText(g, Glyph, Theme.Icons(nav ? 13f : 12f), new Rectangle(x, 0, iconWidth, Height), fore, flags | TextFormatFlags.HorizontalCenter);
        TextRenderer.DrawText(g, Text, Font, new Rectangle(x + iconWidth + gap, 0, Width - x - iconWidth - gap - 4, Height), fore, flags | TextFormatFlags.EndEllipsis);
    }

    (Color Back, Color Fore, Color Border) Palette()
    {
        var none = Color.Empty;
        var active = hover || pressed;
        return Kind switch
        {
            ButtonKind.Primary => (active ? Theme.AccentHover : Theme.Accent, Theme.OnAccent, none),
            ButtonKind.Danger => (active ? ControlPaint.Dark(Theme.Danger, 0.1f) : Theme.Danger, Theme.HighContrast ? Color.Black : Color.White, none),
            ButtonKind.Secondary => (active ? Theme.AccentSoft : Theme.Surface, Theme.Text, Theme.Border),
            ButtonKind.Ghost => (active ? Theme.AccentSoft : none, Theme.Accent, none),
            ButtonKind.Nav => (Selected ? Theme.SidebarSelected : active ? Theme.SidebarHover : none, Selected ? Theme.SidebarSelectedText : Theme.SidebarText, none),
            _ => (active ? Theme.SidebarHover : none, Theme.HighContrast ? Theme.Danger : Color.FromArgb(252, 165, 165), none),
        };
    }
}
