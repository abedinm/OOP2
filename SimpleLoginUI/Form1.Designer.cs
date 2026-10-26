namespace SimpleLoginUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelBrand = new Panel();
            lblFooter = new Label();
            lblBrandText = new Label();
            lblBrandTitle = new Label();
            lblLogo = new Label();
            lblTitle = new Label();
            lblSubtitle = new Label();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            chkShowPassword = new CheckBox();
            lnkForgot = new LinkLabel();
            lblError = new Label();
            btnLogin = new Button();
            lblNoAccount = new Label();
            lnkSignup = new LinkLabel();
            panelBrand.SuspendLayout();
            SuspendLayout();
            // 
            // panelBrand
            // 
            panelBrand.BackColor = Color.FromArgb(79, 70, 229);
            panelBrand.Controls.Add(lblFooter);
            panelBrand.Controls.Add(lblBrandText);
            panelBrand.Controls.Add(lblBrandTitle);
            panelBrand.Controls.Add(lblLogo);
            panelBrand.Dock = DockStyle.Left;
            panelBrand.Location = new Point(0, 0);
            panelBrand.Name = "panelBrand";
            panelBrand.Size = new Size(340, 500);
            panelBrand.TabIndex = 0;
            // 
            // lblFooter
            // 
            lblFooter.AutoSize = true;
            lblFooter.Font = new Font("Segoe UI", 9F);
            lblFooter.ForeColor = Color.FromArgb(199, 210, 254);
            lblFooter.Location = new Point(40, 455);
            lblFooter.Name = "lblFooter";
            lblFooter.Size = new Size(150, 15);
            lblFooter.TabIndex = 3;
            lblFooter.Text = "AIUB  ·  OOP2  ·  Section C";
            // 
            // lblBrandText
            // 
            lblBrandText.Font = new Font("Segoe UI", 11F);
            lblBrandText.ForeColor = Color.FromArgb(224, 231, 255);
            lblBrandText.Location = new Point(40, 230);
            lblBrandText.Name = "lblBrandText";
            lblBrandText.Size = new Size(270, 70);
            lblBrandText.TabIndex = 2;
            lblBrandText.Text = "Sign in to see your courses, lab tasks and results — all in one place.";
            // 
            // lblBrandTitle
            // 
            lblBrandTitle.AutoSize = true;
            lblBrandTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblBrandTitle.ForeColor = Color.White;
            lblBrandTitle.Location = new Point(34, 178);
            lblBrandTitle.Name = "lblBrandTitle";
            lblBrandTitle.Size = new Size(228, 45);
            lblBrandTitle.TabIndex = 1;
            lblBrandTitle.Text = "Student Portal";
            // 
            // lblLogo
            // 
            lblLogo.BackColor = Color.White;
            lblLogo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblLogo.ForeColor = Color.FromArgb(79, 70, 229);
            lblLogo.Location = new Point(40, 108);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(58, 58);
            lblLogo.TabIndex = 0;
            lblLogo.Text = "SP";
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitle.Location = new Point(400, 62);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(201, 37);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "Welcome back";
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubtitle.Location = new Point(404, 104);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(229, 19);
            lblSubtitle.TabIndex = 2;
            lblSubtitle.Text = "Please sign in to your account.";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblUsername.ForeColor = Color.FromArgb(51, 65, 85);
            lblUsername.Location = new Point(404, 152);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(70, 17);
            lblUsername.TabIndex = 3;
            lblUsername.Text = "Username";
            // 
            // txtUsername
            // 
            txtUsername.BackColor = Color.FromArgb(248, 250, 252);
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Font = new Font("Segoe UI", 12F);
            txtUsername.ForeColor = Color.FromArgb(15, 23, 42);
            txtUsername.Location = new Point(406, 176);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Enter your username";
            txtUsername.Size = new Size(400, 29);
            txtUsername.TabIndex = 4;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lblPassword.ForeColor = Color.FromArgb(51, 65, 85);
            lblPassword.Location = new Point(404, 224);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(66, 17);
            lblPassword.TabIndex = 5;
            lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.FromArgb(248, 250, 252);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 12F);
            txtPassword.ForeColor = Color.FromArgb(15, 23, 42);
            txtPassword.Location = new Point(406, 248);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Enter your password";
            txtPassword.Size = new Size(400, 29);
            txtPassword.TabIndex = 6;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // chkShowPassword
            // 
            chkShowPassword.AutoSize = true;
            chkShowPassword.Cursor = Cursors.Hand;
            chkShowPassword.Font = new Font("Segoe UI", 9.5F);
            chkShowPassword.ForeColor = Color.FromArgb(71, 85, 105);
            chkShowPassword.Location = new Point(408, 290);
            chkShowPassword.Name = "chkShowPassword";
            chkShowPassword.Size = new Size(112, 21);
            chkShowPassword.TabIndex = 7;
            chkShowPassword.Text = "Show password";
            chkShowPassword.UseVisualStyleBackColor = true;
            chkShowPassword.CheckedChanged += chkShowPassword_CheckedChanged;
            // 
            // lnkForgot
            // 
            lnkForgot.ActiveLinkColor = Color.FromArgb(67, 56, 202);
            lnkForgot.AutoSize = true;
            lnkForgot.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lnkForgot.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkForgot.LinkColor = Color.FromArgb(79, 70, 229);
            lnkForgot.Location = new Point(694, 291);
            lnkForgot.Name = "lnkForgot";
            lnkForgot.Size = new Size(112, 17);
            lnkForgot.TabIndex = 8;
            lnkForgot.TabStop = true;
            lnkForgot.Text = "Forgot password?";
            lnkForgot.LinkClicked += lnkForgot_LinkClicked;
            // 
            // lblError
            // 
            lblError.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblError.ForeColor = Color.FromArgb(220, 38, 38);
            lblError.Location = new Point(404, 322);
            lblError.Name = "lblError";
            lblError.Size = new Size(402, 22);
            lblError.TabIndex = 9;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(79, 70, 229);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(406, 350);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(400, 46);
            btnLogin.TabIndex = 10;
            btnLogin.Text = "Sign in";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            btnLogin.MouseEnter += btnLogin_MouseEnter;
            btnLogin.MouseLeave += btnLogin_MouseLeave;
            // 
            // lblNoAccount
            // 
            lblNoAccount.AutoSize = true;
            lblNoAccount.Font = new Font("Segoe UI", 9.5F);
            lblNoAccount.ForeColor = Color.FromArgb(100, 116, 139);
            lblNoAccount.Location = new Point(494, 418);
            lblNoAccount.Name = "lblNoAccount";
            lblNoAccount.Size = new Size(150, 17);
            lblNoAccount.TabIndex = 11;
            lblNoAccount.Text = "Don't have an account?";
            // 
            // lnkSignup
            // 
            lnkSignup.ActiveLinkColor = Color.FromArgb(67, 56, 202);
            lnkSignup.AutoSize = true;
            lnkSignup.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lnkSignup.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkSignup.LinkColor = Color.FromArgb(79, 70, 229);
            lnkSignup.Location = new Point(646, 418);
            lnkSignup.Name = "lnkSignup";
            lnkSignup.Size = new Size(73, 17);
            lnkSignup.TabIndex = 12;
            lnkSignup.TabStop = true;
            lnkSignup.Text = "Create one";
            lnkSignup.LinkClicked += lnkSignup_LinkClicked;
            // 
            // Form1
            // 
            AcceptButton = btnLogin;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(860, 500);
            Controls.Add(lnkSignup);
            Controls.Add(lblNoAccount);
            Controls.Add(btnLogin);
            Controls.Add(lblError);
            Controls.Add(lnkForgot);
            Controls.Add(chkShowPassword);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(txtUsername);
            Controls.Add(lblUsername);
            Controls.Add(lblSubtitle);
            Controls.Add(lblTitle);
            Controls.Add(panelBrand);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Student Portal — Sign in";
            panelBrand.ResumeLayout(false);
            panelBrand.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panelBrand;
        private Label lblFooter;
        private Label lblBrandText;
        private Label lblBrandTitle;
        private Label lblLogo;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private CheckBox chkShowPassword;
        private LinkLabel lnkForgot;
        private Label lblError;
        private Button btnLogin;
        private Label lblNoAccount;
        private LinkLabel lnkSignup;
    }
}
