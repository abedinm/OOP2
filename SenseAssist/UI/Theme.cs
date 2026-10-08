using System.Drawing.Drawing2D;

namespace SenseAssist.UI;

/// <summary>Colours and fonts. High contrast switches to yellow-on-black; large text scales everything up.</summary>
public static class Theme
{
    public static bool HighContrast { get; set; }
    public static bool LargeText { get; set; }
    public static float Scale => LargeText ? 1.2f : 1f;

    static Color Pick(Color normal, Color contrast) => HighContrast ? contrast : normal;

    public static Color Background => Pick(Color.FromArgb(241, 244, 250), Color.Black);
    public static Color Surface => Pick(Color.White, Color.Black);
    public static Color SurfaceAlt => Pick(Color.FromArgb(248, 250, 253), Color.FromArgb(22, 22, 22));
    public static Color Border => Pick(Color.FromArgb(226, 232, 240), Color.White);
    public static Color Text => Pick(Color.FromArgb(15, 23, 42), Color.White);
    public static Color Muted => Pick(Color.FromArgb(100, 116, 139), Color.FromArgb(235, 235, 235));
    public static Color Accent => Pick(Color.FromArgb(79, 70, 229), Color.Yellow);
    public static Color AccentHover => Pick(Color.FromArgb(67, 56, 202), Color.Gold);
    public static Color AccentSoft => Pick(Color.FromArgb(238, 242, 255), Color.FromArgb(70, 70, 0));
    public static Color OnAccent => Pick(Color.White, Color.Black);
    public static Color Danger => Pick(Color.FromArgb(220, 38, 38), Color.FromArgb(255, 110, 110));
    public static Color Success => Pick(Color.FromArgb(5, 150, 105), Color.Lime);
    public static Color Warning => Pick(Color.FromArgb(217, 119, 6), Color.Orange);
    public static Color Sidebar => Pick(Color.FromArgb(15, 23, 42), Color.Black);
    public static Color SidebarHover => Pick(Color.FromArgb(30, 41, 59), Color.FromArgb(60, 60, 0));
    public static Color SidebarSelected => Pick(Color.FromArgb(79, 70, 229), Color.Yellow);
    public static Color SidebarText => Pick(Color.FromArgb(203, 213, 225), Color.Yellow);
    public static Color SidebarSelectedText => Pick(Color.White, Color.Black);
    public static Color SidebarTitle => Pick(Color.White, Color.Yellow);
    public static Color SwitchOff => Pick(Color.FromArgb(203, 213, 225), Color.Gray);
    public static Color FocusRing => Pick(Color.FromArgb(245, 158, 11), Color.Cyan);
    public static Color CaptionBack => Pick(Color.FromArgb(15, 23, 42), Color.Black);
    public static Color CaptionText => Pick(Color.White, Color.Yellow);
    public static Color HeroStart => Pick(Color.FromArgb(79, 70, 229), Color.Black);
    public static Color HeroEnd => Pick(Color.FromArgb(147, 51, 234), Color.Black);
    public static Color HeroText => Pick(Color.White, Color.Yellow);

    public const string FontName = "Segoe UI";
    public static string IconFontName { get; } =
        FontFamily.Families.Any(f => f.Name == "Segoe Fluent Icons") ? "Segoe Fluent Icons" : "Segoe MDL2 Assets";

    static readonly Dictionary<(string, float, FontStyle), Font> Fonts = new();

    public static Font Font(float size, FontStyle style = FontStyle.Regular) => Get(FontName, size * Scale, style);
    public static Font Icons(float size) => Get(IconFontName, size * Scale, FontStyle.Regular);

    static Font Get(string name, float size, FontStyle style)
    {
        var key = (name, size, style);
        if (!Fonts.TryGetValue(key, out var font))
            Fonts[key] = font = new Font(name, size, style);
        return font;
    }

    public static int Px(int pixels) => (int)(pixels * Scale);
}

/// <summary>Segoe MDL2 / Fluent icon code points.</summary>
public static class Glyph
{
    public const string Access = "";
    public const string Reminder = "";
    public const string Contacts = "";
    public const string Log = "";
    public const string Settings = "";
    public const string Logout = "";
    public const string Add = "";
    public const string Edit = "";
    public const string Delete = "";
    public const string Save = "";
    public const string Speak = "";
    public const string Check = "";
    public const string Refresh = "";
    public const string Caption = "";
    public const string Alert = "";
    public const string User = "";
    public const string Help = "";
    public const string Keyboard = "";
}

public static class Draw
{
    public static GraphicsPath Rounded(RectangleF r, float radius)
    {
        var d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
        var path = new GraphicsPath();
        if (d <= 0)
        {
            path.AddRectangle(r);
            return path;
        }
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
