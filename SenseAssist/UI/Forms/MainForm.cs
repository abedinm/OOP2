using SenseAssist.Data;
using SenseAssist.Models;
using SenseAssist.Services;
using SenseAssist.UI.Controls;
using SenseAssist.UI.Pages;

namespace SenseAssist.UI.Forms;

public sealed class MainForm : Form
{
    static readonly string[] PageNames = ["Reminders", "Contacts", "Activity log", "Settings"];
    static readonly string[] PageGlyphs = [Glyph.Reminder, Glyph.Contacts, Glyph.Log, Glyph.Settings];

    readonly User user;
    readonly AuthService auth = new();
    readonly ReminderRepository reminders = new();
    readonly HashSet<int> alerted = [];
    readonly System.Windows.Forms.Timer reminderTimer = new() { Interval = 20_000 };
    readonly System.Windows.Forms.Timer flashTimer = new() { Interval = 350 };

    ModernButton[] navButtons = [];
    Panel content = new();
    CardPanel captionBar = new();
    Label pageTitle = new(), pageSubtitle = new(), captionText = new();
    PageBase? page;
    int pageIndex;
    int flashTicks;

    public bool LoggedOut { get; private set; }

    public MainForm(User user)
    {
        this.user = user;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Text = $"SenseAssist — {user.FullName}";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1100, 700);
        Size = new Size(1320, 840);
        KeyPreview = true;

        BuildUi();

        Speech.Announced += OnAnnounced;
        reminderTimer.Tick += (_, _) => CheckDueReminders();
        flashTimer.Tick += (_, _) => FlashStep();
        reminderTimer.Start();
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        Navigate(0, greeting: $"Welcome, {user.FirstName}. ");
        CheckDueReminders();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        reminderTimer.Stop();
        flashTimer.Stop();
        Speech.Announced -= OnAnnounced;
        base.OnFormClosed(e);
    }

    // ---------------------------------------------------------------- layout

    void BuildUi()
    {
        SuspendLayout();
        Controls.Clear();
        BackColor = Theme.Background;
        Font = Theme.Font(10.5f);

        var sidebar = BuildSidebar();

        var main = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Background, Padding = new Padding(28, 22, 28, 18) };
        content = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Background, Padding = new Padding(0, 14, 0, 14) };

        var header = new Panel { Dock = DockStyle.Top, Height = Theme.Px(74), BackColor = Theme.Background };
        pageTitle = new Label { AutoSize = true, Font = Theme.Font(22f, FontStyle.Bold), ForeColor = Theme.Text, BackColor = Theme.Background, Location = new Point(0, 0) };
        pageSubtitle = new Label { AutoSize = true, Font = Theme.Font(10.5f), ForeColor = Theme.Muted, BackColor = Theme.Background, Location = new Point(3, Theme.Px(46)) };
        var actions = new FlowLayoutPanel { Dock = DockStyle.Right, Width = Theme.Px(330), FlowDirection = FlowDirection.RightToLeft, WrapContents = false, BackColor = Theme.Background, Padding = new Padding(0, 6, 0, 0) };
        var help = new ModernButton("Help", Glyph.Help, ButtonKind.Secondary) { AccessibleDescription = "F1", Margin = new Padding(10, 0, 0, 0) };
        help.Click += (_, _) => SpeakHelp();
        var read = new ModernButton("Read aloud", Glyph.Speak, ButtonKind.Ghost) { AccessibleDescription = "Control R" };
        read.Click += (_, _) => ReadCurrent();
        actions.Controls.Add(help);
        actions.Controls.Add(read);
        header.Controls.Add(pageTitle);
        header.Controls.Add(pageSubtitle);
        header.Controls.Add(actions);

        // Live captions: everything that is spoken also appears here, large, for hard-of-hearing users.
        captionBar = new CardPanel { Dock = DockStyle.Bottom, Height = Theme.Px(66), FillColor = Theme.CaptionBack, BorderColor = Theme.HighContrast ? Color.Yellow : Theme.CaptionBack, Radius = 14, Padding = new Padding(18, 0, 18, 0) };
        captionText = new Label
        {
            Dock = DockStyle.Fill,
            Text = "Captions appear here — everything SenseAssist says is also written down.",
            Font = Theme.Font(13f, FontStyle.Bold),
            ForeColor = Theme.CaptionText,
            BackColor = Color.Transparent,
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true,
            AccessibleName = "Captions",
            AccessibleRole = AccessibleRole.StatusBar,
        };
        var ccIcon = new Label { Dock = DockStyle.Left, Width = Theme.Px(44), Text = Glyph.Caption, Font = Theme.Icons(18f), ForeColor = Theme.CaptionText, BackColor = Color.Transparent, TextAlign = ContentAlignment.MiddleLeft };
        captionBar.Controls.Add(captionText);
        captionBar.Controls.Add(ccIcon);

        main.Controls.Add(content);
        main.Controls.Add(captionBar);
        main.Controls.Add(header);
        Controls.Add(main);
        Controls.Add(sidebar);
        ResumeLayout(true);
        VoiceFocus.Attach(sidebar);
        VoiceFocus.Attach(header);
    }

    Panel BuildSidebar()
    {
        var width = Theme.Px(252);
        var sidebar = new Panel { Dock = DockStyle.Left, Width = width, BackColor = Theme.Sidebar, Padding = new Padding(14, 18, 14, 16) };

        var brand = new Panel { Dock = DockStyle.Top, Height = Theme.Px(62), BackColor = Theme.Sidebar };
        brand.Controls.Add(new Label { Text = Glyph.Access, Font = Theme.Icons(22f), ForeColor = Theme.HighContrast ? Color.Yellow : Color.FromArgb(165, 180, 252), BackColor = Theme.Sidebar, AutoSize = true, Location = new Point(6, 12) });
        brand.Controls.Add(new Label { Text = "SenseAssist", Font = Theme.Font(17f, FontStyle.Bold), ForeColor = Theme.SidebarTitle, BackColor = Theme.Sidebar, AutoSize = true, Location = new Point(Theme.Px(46), 10) });

        var profile = new Panel { Dock = DockStyle.Top, Height = Theme.Px(78), BackColor = Theme.Sidebar };
        profile.Controls.Add(new Avatar(user.FullName) { Location = new Point(6, 14), Size = new Size(Theme.Px(46), Theme.Px(46)) });
        profile.Controls.Add(new Label { Text = user.FullName, Font = Theme.Font(11f, FontStyle.Bold), ForeColor = Theme.SidebarTitle, BackColor = Theme.Sidebar, AutoSize = true, Location = new Point(Theme.Px(64), 14) });
        profile.Controls.Add(new Label { Text = NeedsLabel(user.Needs), Font = Theme.Font(9.5f), ForeColor = Theme.SidebarText, BackColor = Theme.Sidebar, AutoSize = true, Location = new Point(Theme.Px(64), Theme.Px(38)) });

        var menuLabel = new Label { Text = "MENU", Dock = DockStyle.Top, Height = Theme.Px(30), Font = Theme.Font(8.5f, FontStyle.Bold), ForeColor = Theme.SidebarText, BackColor = Theme.Sidebar, Padding = new Padding(8, 10, 0, 0) };

        var nav = new FlowLayoutPanel { Dock = DockStyle.Top, FlowDirection = FlowDirection.TopDown, WrapContents = false, BackColor = Theme.Sidebar, Height = PageNames.Length * Theme.Px(52) + 6 };
        navButtons = new ModernButton[PageNames.Length];
        for (var i = 0; i < PageNames.Length; i++)
        {
            var index = i;
            var button = new ModernButton(PageNames[i], PageGlyphs[i], ButtonKind.Nav)
            {
                Width = width - 28,
                Margin = new Padding(0, 0, 0, 6),
                AccessibleDescription = $"Control {i + 1}",
            };
            button.Click += (_, _) => Navigate(index);
            nav.Controls.Add(button);
            navButtons[i] = button;
        }

        var logout = new ModernButton("Log out", Glyph.Logout, ButtonKind.NavDanger) { Dock = DockStyle.Bottom, AccessibleDescription = "Control L" };
        logout.Click += (_, _) => Logout();
        var hint = new Label
        {
            Text = "F1 help · Ctrl+R read · Esc stop",
            Dock = DockStyle.Bottom,
            Height = Theme.Px(34),
            Font = Theme.Font(8.5f),
            ForeColor = Theme.SidebarText,
            BackColor = Theme.Sidebar,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(6, 0, 0, 0),
        };

        sidebar.Controls.Add(nav);
        sidebar.Controls.Add(menuLabel);
        sidebar.Controls.Add(profile);
        sidebar.Controls.Add(brand);
        sidebar.Controls.Add(hint);
        sidebar.Controls.Add(logout);
        return sidebar;
    }

    static string NeedsLabel(string needs) => needs switch
    {
        "Seeing" => "Vision support",
        "Hearing" => "Hearing support",
        _ => "Vision + hearing support",
    };

    // ---------------------------------------------------------------- navigation

    void Navigate(int index, string greeting = "")
    {
        pageIndex = index;
        for (var i = 0; i < navButtons.Length; i++)
            navButtons[i].Selected = i == index;

        page?.Dispose();
        page = index switch
        {
            0 => new RemindersPage(user),
            1 => new ContactsPage(user),
            2 => new ActivityPage(user),
            _ => new SettingsPage(ApplySettings),
        };
        page.Dock = DockStyle.Fill;
        content.Controls.Clear();
        content.Controls.Add(page);
        pageTitle.Text = page.Title;
        pageSubtitle.Text = page.Subtitle;

        page.RefreshData();
        VoiceFocus.Attach(page);
        if (page.FirstFocus is { } first)
            VoiceFocus.QuietFocus(first);
        Speech.Say($"{greeting}{page.Title} page. {page.Summary} Press F1 for help.");
    }

    void ApplySettings()
    {
        ActivityLogger.Log(user.Id, "Settings", "Accessibility settings changed");
        BuildUi();
        Navigate(pageIndex);
        Speech.Say("Settings saved and applied.");
    }

    // ---------------------------------------------------------------- keyboard

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Control | Keys.D1: Navigate(0); return true;
            case Keys.Control | Keys.D2: Navigate(1); return true;
            case Keys.Control | Keys.D3: Navigate(2); return true;
            case Keys.Control | Keys.D4: Navigate(3); return true;
            case Keys.Control | Keys.N: page?.AddNew(); return true;
            case Keys.Control | Keys.E: page?.EditSelected(); return true;
            case Keys.Control | Keys.D when page is RemindersPage reminderPage: reminderPage.ToggleDone(); return true;
            case Keys.Control | Keys.R: ReadCurrent(); return true;
            case Keys.Control | Keys.L: Logout(); return true;
            case Keys.F1: SpeakHelp(); return true;
            case Keys.Escape: Speech.Stop(); return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    void ReadCurrent() => Speech.Say(page?.ReadCurrent() ?? "");

    void SpeakHelp() => Speech.Say(
        $"{page?.Help} Shortcuts: Control 1 to 4 switch pages: reminders, contacts, activity log and settings. " +
        "Control N adds, Control E or Enter edits, Delete removes, Control D marks a reminder done. " +
        "Control R reads the selected item. Escape stops speaking. Control L logs out.");

    // ---------------------------------------------------------------- captions & alerts

    void OnAnnounced(string text)
    {
        if (InvokeRequired)
            BeginInvoke(() => OnAnnounced(text));
        else
            captionText.Text = text;
    }

    void CheckDueReminders()
    {
        List<Reminder> due;
        try
        {
            var now = DateTime.Now;
            due = reminders.GetAll(user.Id).Where(r => !r.IsDone && r.DueAt <= now && r.DueAt >= now.AddMinutes(-15)).ToList();
        }
        catch (Exception ex)
        {
            AppLog.Warn("Reminder check failed", ex);
            return;
        }

        foreach (var reminder in due.Where(r => alerted.Add(r.Id)))
        {
            ActivityLogger.Log(user.Id, "Alert", $"Reminder due: {reminder.Title}");
            Alert($"Reminder due now: {reminder.Title}. {reminder.Details}");
            if (page is RemindersPage)
                page.RefreshData();
        }
    }

    void Alert(string message)
    {
        Speech.Say(message);
        if (!AppSettings.Current.VisualAlerts)
            return;
        NativeMethods.Flash(Handle);
        flashTicks = 12;
        flashTimer.Start();
    }

    void FlashStep()
    {
        captionBar.FillColor = flashTicks % 2 == 0 ? Theme.Warning : Theme.CaptionBack;
        captionBar.Invalidate(true);
        if (--flashTicks <= 0)
        {
            flashTimer.Stop();
            captionBar.FillColor = Theme.CaptionBack;
            captionBar.Invalidate(true);
        }
    }

    // ---------------------------------------------------------------- logout

    void Logout()
    {
        Speech.Say("Log out of SenseAssist? Press Y for yes or N for no.");
        if (MessageBox.Show("Do you want to log out?", "Log out", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        {
            Speech.Say("Still signed in.");
            return;
        }
        auth.Logout(user);
        LoggedOut = true;
        Speech.Say($"Goodbye, {user.FirstName}. You are signed out.");
        Close();
    }
}
