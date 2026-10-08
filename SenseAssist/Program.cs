using SenseAssist.Data;
using SenseAssist.Services;
using SenseAssist.UI.Forms;

namespace SenseAssist;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) => ShowError(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (_, e) => AppLog.Error("Unhandled exception", e.ExceptionObject as Exception);

        AppLog.Info("Application starting");
        AppSettings.Current.ApplyToRuntime();

        try
        {
            Db.EnsureSchema();
        }
        catch (Exception ex)
        {
            AppLog.Error("Database unavailable", ex);
            Speech.Say("The database could not be reached.");
            MessageBox.Show(
                $"Could not connect to SQL Server at {Db.Server}.\n\nMake sure OrbStack is running on the Mac.\n\n{ex.Message}",
                "SenseAssist", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Sign in -> main window -> (log out) -> sign in again, until the user closes the app.
        while (true)
        {
            using var login = new LoginForm();
            if (login.ShowDialog() != DialogResult.OK || login.SignedInUser is null)
                break;

            using var main = new MainForm(login.SignedInUser);
            main.ShowDialog();
            if (!main.LoggedOut)
                break;
        }

        AppLog.Info("Application exiting");
    }

    static void ShowError(Exception ex)
    {
        AppLog.Error("UI exception", ex);
        Speech.Say("Something went wrong. " + ex.Message);
        MessageBox.Show(ex.Message, "Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
