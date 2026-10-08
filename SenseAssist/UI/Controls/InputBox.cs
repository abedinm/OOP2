using System.Diagnostics.CodeAnalysis;
using System.Drawing.Drawing2D;

namespace SenseAssist.UI.Controls;

/// <summary>Rounded text field. The inner TextBox carries the accessible name for screen readers.</summary>
public class InputBox : Panel
{
    public TextBox Box { get; }

    public InputBox(string accessibleName, bool password = false, bool multiline = false, string placeholder = "")
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Theme.Surface;
        Height = Theme.Px(multiline ? 110 : 46);
        Padding = multiline ? new Padding(12, 10, 8, 8) : new Padding(14, 0, 12, 0);

        Box = new TextBox
        {
            BorderStyle = BorderStyle.None,
            Dock = DockStyle.Fill,
            Font = Theme.Font(11.5f),
            BackColor = Theme.Surface,
            ForeColor = Theme.Text,
            AccessibleName = accessibleName,
            PlaceholderText = placeholder,
            UseSystemPasswordChar = password,
            Multiline = multiline,
            AcceptsReturn = multiline,
            ScrollBars = multiline ? ScrollBars.Vertical : ScrollBars.None,
        };
        Controls.Add(Box);
        Box.GotFocus += (_, _) => Invalidate();
        Box.LostFocus += (_, _) => Invalidate();
        Click += (_, _) => Box.Focus();
    }

    [AllowNull]
    public override string Text
    {
        get => Box.Text;
        set => Box.Text = value;
    }

    protected override void OnLayout(LayoutEventArgs e)
    {
        // Vertically centre a single-line box inside the rounded field.
        if (!Box.Multiline)
        {
            var top = Math.Max(0, (Height - Box.PreferredHeight) / 2);
            if (Padding.Top != top)
                Padding = new Padding(14, top, 12, 0);
        }
        base.OnLayout(e);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Theme.Surface);

        var focused = Box.Focused;
        using var path = Draw.Rounded(new RectangleF(1, 1, Width - 3f, Height - 3f), 10);
        using var brush = new SolidBrush(Theme.Surface);
        using var pen = new Pen(focused ? Theme.Accent : Theme.Border, focused ? 2.5f : 1.5f);
        g.FillPath(brush, path);
        g.DrawPath(pen, path);
    }
}
