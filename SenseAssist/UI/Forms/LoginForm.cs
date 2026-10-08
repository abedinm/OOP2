using SenseAssist.Models;
using SenseAssist.Services;
using SenseAssist.UI.Controls;

namespace SenseAssist.UI.Forms;

public sealed class LoginForm : Form
{
    static readonly string[] NeedsOptions = ["Seeing", "Hearing", "Seeing and hearing"];

    readonly AuthService auth = new();
    readonly ModernButton tabSignIn, tabRegister, signInButton, createButton;
    readonly FlowLayoutPanel signInFlow, registerFlow;
    readonly InputBox loginUser, loginPass, regName, regUser, regPass, regConfirm;
    readonly ComboBox regNeeds;
    readonly Label heading, subheading, error, caption;
    bool registerMode;

    public User? SignedInUser { get; private set; }

    public LoginForm()
    {
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Text = "SenseAssist — Sign in";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        ClientSize = new Size(Theme.Px(1040), Theme.Px(680));
        BackColor = Theme.Background;
        Font = Theme.Font(10.5f);
        KeyPreview = true;

        // ---- Left: brand
        var hero = new HeroPanel { Dock = DockStyle.Left, Width = Theme.Px(420) };
        var y = 64;
        AddHeroLabel(hero, Glyph.Access, Theme.Icons(40), ref y, 20);
        AddHeroLabel(hero, "SenseAssist", Theme.Font(30, FontStyle.Bold), ref y, 6);
        AddHeroLabel(hero, "Your accessible daily companion — reminders, contacts and alerts you can hear and see.", Theme.Font(12.5f), ref y, 36);
        AddFeature(hero, Glyph.Speak, "Reads every screen and alert aloud", ref y);
        AddFeature(hero, Glyph.Caption, "Live captions for everything it says", ref y);
        AddFeature(hero, Glyph.Alert, "Spoken + flashing reminder alerts", ref y);
        AddFeature(hero, Glyph.Keyboard, "Keyboard-first, screen-reader friendly", ref y);

        // ---- Right: sign-in card
        var right = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Background, Padding = new Padding(Theme.Px(56), Theme.Px(34), Theme.Px(56), 8) };
        caption = new Label
        {
            Dock = DockStyle.Bottom,
            Height = Theme.Px(46),
            Font = Theme.Font(11f, FontStyle.Bold),
            ForeColor = Theme.Muted,
            BackColor = Theme.Background,
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true,
            AccessibleName = "Captions",
        };

        var card = new CardPanel { Dock = DockStyle.Fill, Padding = new Padding(Theme.Px(34), Theme.Px(26), Theme.Px(34), Theme.Px(18)) };
        heading = new Label { Dock = DockStyle.Top, Height = Theme.Px(50), Font = Theme.Font(22, FontStyle.Bold), ForeColor = Theme.Text, BackColor = Theme.Surface };
        subheading = new Label { Dock = DockStyle.Top, Height = Theme.Px(30), Font = Theme.Font(10.5f), ForeColor = Theme.Muted, BackColor = Theme.Surface };

        var tabs = new FlowLayoutPanel { Dock = DockStyle.Top, Height = Theme.Px(64), BackColor = Theme.Surface, WrapContents = false, Padding = new Padding(0, 10, 0, 8) };
        tabSignIn = new ModernButton("Sign in", Glyph.User) { Width = Theme.Px(170), AccessibleDescription = "Control I" };
        tabRegister = new ModernButton("Create account", Glyph.Add, ButtonKind.Secondary) { Width = Theme.Px(190), AccessibleDescription = "Control R" };
        tabSignIn.Click += (_, _) => ShowTab(register: false);
        tabRegister.Click += (_, _) => ShowTab(register: true);
        tabs.Controls.Add(tabSignIn);
        tabs.Controls.Add(tabRegister);

        error = new Label { Dock = DockStyle.Bottom, Height = Theme.Px(34), ForeColor = Theme.Danger, BackColor = Theme.Surface, Font = Theme.Font(10.5f, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft };

        // Sign-in fields
        signInFlow = FormKit.VerticalFlow();
        loginUser = new InputBox("Username", placeholder: "e.g. minhazul");
        loginPass = new InputBox("Password", password: true);
        var showPassword = new ToggleSwitch("Show password");
        showPassword.CheckedChanged += (_, _) => loginPass.Box.UseSystemPasswordChar = !showPassword.Checked;
        signInButton = new ModernButton("Sign in", Glyph.Check) { Height = Theme.Px(50), Margin = new Padding(0, 14, 0, 6) };
        signInButton.Click += (_, _) => SignIn();
        var toRegister = new ModernButton("New here? Create an account", "", ButtonKind.Ghost);
        toRegister.Click += (_, _) => ShowTab(register: true);
        FormKit.AddField(signInFlow, "Username", loginUser);
        FormKit.AddField(signInFlow, "Password", loginPass);
        signInFlow.Controls.Add(showPassword);
        signInFlow.Controls.Add(signInButton);
        signInFlow.Controls.Add(toRegister);

        // Register fields
        registerFlow = FormKit.VerticalFlow();
        regName = new InputBox("Full name", placeholder: "e.g. Minhazul Abedin");
        regUser = new InputBox("Choose a username");
        regPass = new InputBox("Choose a password, at least 6 characters", password: true);
        regConfirm = new InputBox("Confirm password", password: true);
        regNeeds = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = Theme.Font(11.5f), FlatStyle = FlatStyle.Flat, AccessibleName = "I need help with" };
        regNeeds.Items.AddRange(["Seeing — I am blind or have low vision", "Hearing — I use a hearing aid", "Both seeing and hearing"]);
        regNeeds.SelectedIndex = 2;
        createButton = new ModernButton("Create account", Glyph.Check) { Height = Theme.Px(50), Margin = new Padding(0, 14, 0, 6) };
        createButton.Click += (_, _) => Register();
        FormKit.AddField(registerFlow, "Full name", regName);
        FormKit.AddField(registerFlow, "Username", regUser);
        FormKit.AddField(registerFlow, "Password", regPass);
        FormKit.AddField(registerFlow, "Confirm password", regConfirm);
        FormKit.AddField(registerFlow, "I need help with", regNeeds);
        registerFlow.Controls.Add(createButton);

        var body = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Surface };
        body.Controls.Add(signInFlow);
        body.Controls.Add(registerFlow);
        signInFlow.Resize += (_, _) => FormKit.Stretch(signInFlow);
        registerFlow.Resize += (_, _) => FormKit.Stretch(registerFlow);

        card.Controls.Add(body);
        card.Controls.Add(error);
        card.Controls.Add(tabs);
        card.Controls.Add(subheading);
        card.Controls.Add(heading);

        right.Controls.Add(card);
        right.Controls.Add(caption);
        Controls.Add(right);
        Controls.Add(hero);
        right.BringToFront();

        Speech.Announced += OnAnnounced;
        FormClosed += (_, _) => Speech.Announced -= OnAnnounced;
        ShowTab(register: false, announce: false);
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        VoiceFocus.Attach(this);
        FormKit.Stretch(signInFlow);
        FormKit.Stretch(registerFlow);
        VoiceFocus.QuietFocus(loginUser.Box);
        Speech.Say("Welcome to SenseAssist. Sign in with your username and password, or press Control R to create an account. Press F1 for help. You are in the username field.");
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.F1:
                Speech.Say("Sign in page. Type your username, press Tab, type your password, then press Enter. " +
                           "Control R opens Create account, Control I returns to Sign in. Escape stops speech.");
                return true;
            case Keys.Control | Keys.R:
                ShowTab(register: true);
                return true;
            case Keys.Control | Keys.I:
                ShowTab(register: false);
                return true;
            case Keys.Escape:
                Speech.Stop();
                return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    void ShowTab(bool register, bool announce = true)
    {
        registerMode = register;
        signInFlow.Visible = !register;
        registerFlow.Visible = register;
        tabSignIn.Kind = register ? ButtonKind.Secondary : ButtonKind.Primary;
        tabRegister.Kind = register ? ButtonKind.Primary : ButtonKind.Secondary;
        heading.Text = register ? "Create your account" : "Welcome back";
        subheading.Text = register ? "It takes less than a minute. Everything is read aloud as you go." : "Sign in to your SenseAssist account.";
        AcceptButton = register ? createButton : signInButton;
        error.Text = "";

        if (!announce)
            return;
        VoiceFocus.QuietFocus(register ? regName.Box : loginUser.Box);
        Speech.Say(register ? "Create account. You are in the full name field." : "Sign in. You are in the username field.");
    }

    void SignIn()
    {
        error.Text = "";
        try
        {
            SignedInUser = auth.Login(loginUser.Text, loginPass.Text);
            Speech.Say($"Welcome, {SignedInUser.FirstName}. You are signed in.");
            DialogResult = DialogResult.OK;
        }
        catch (InputException ex)
        {
            loginPass.Text = "";
            VoiceFocus.QuietFocus(loginPass.Box);
            ShowError(ex.Message + " Please try again.");
        }
        catch (Exception ex)
        {
            AppLog.Error("Sign-in failed", ex);
            ShowError("Could not reach the database. Check that OrbStack is running on the Mac.");
        }
    }

    void Register()
    {
        error.Text = "";
        try
        {
            SignedInUser = auth.Register(regName.Text, regUser.Text, regPass.Text, regConfirm.Text, NeedsOptions[regNeeds.SelectedIndex]);
            Speech.Say($"Account created. Welcome, {SignedInUser.FirstName}.");
            DialogResult = DialogResult.OK;
        }
        catch (InputException ex)
        {
            ShowError(ex.Message);
        }
        catch (Exception ex)
        {
            AppLog.Error("Registration failed", ex);
            ShowError("Could not reach the database. Check that OrbStack is running on the Mac.");
        }
    }

    void ShowError(string message)
    {
        error.Text = "⚠  " + message;
        Speech.Say(message);
    }

    void OnAnnounced(string text) => caption.Text = "CC   " + text;

    static void AddHeroLabel(Control hero, string text, Font font, ref int y, int gapAfter)
    {
        var label = new Label
        {
            Text = text,
            Font = font,
            ForeColor = Theme.HeroText,
            BackColor = Color.Transparent,
            Location = new Point(48, y),
            MaximumSize = new Size(Theme.Px(330), 0),
            AutoSize = true,
        };
        hero.Controls.Add(label);
        y += label.PreferredHeight + gapAfter;
    }

    static void AddFeature(Control hero, string glyph, string text, ref int y)
    {
        hero.Controls.Add(new Label { Text = glyph, Font = Theme.Icons(15), ForeColor = Theme.HeroText, BackColor = Color.Transparent, Location = new Point(50, y + 2), AutoSize = true });
        var label = new Label { Text = text, Font = Theme.Font(11.5f), ForeColor = Theme.HeroText, BackColor = Color.Transparent, Location = new Point(86, y), AutoSize = true };
        hero.Controls.Add(label);
        y += label.PreferredHeight + 18;
    }
}
