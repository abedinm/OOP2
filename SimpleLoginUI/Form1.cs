namespace SimpleLoginUI
{
    public partial class Form1 : Form
    {
        private readonly Color accent = Color.FromArgb(79, 70, 229);
        private readonly Color accentHover = Color.FromArgb(67, 56, 202);

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                lblError.Text = "Please enter your username.";
                txtUsername.Focus();
                return;
            }
            if (txtPassword.Text.Length < 6)
            {
                lblError.Text = "Password must be at least 6 characters.";
                txtPassword.Focus();
                return;
            }

            MessageBox.Show($"Welcome, {txtUsername.Text.Trim()}!", "Signed in",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void btnLogin_MouseEnter(object sender, EventArgs e)
        {
            btnLogin.BackColor = accentHover;
        }

        private void btnLogin_MouseLeave(object sender, EventArgs e)
        {
            btnLogin.BackColor = accent;
        }

        private void lnkForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Please contact your administrator to reset your password.", "Forgot password",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lnkSignup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Account registration is coming soon.", "Create account",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
