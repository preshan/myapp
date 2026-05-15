using System;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using PasswordWallet.Business;
using PasswordWallet.Presentation.Forms;

namespace PasswordWallet.Presentation
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SkinManager.EnableFormSkins();
            UserLookAndFeel.Default.SetSkinStyle("Visual Studio 2013 Dark");

            try
            {
                var context = WalletApplicationContext.StartNew();

                if (context.MasterPassword.IsFirstRun())
                    Application.Run(new CreateMasterPasswordForm(context));
                else
                    Application.Run(new LoginForm(context));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Password Wallet", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                WalletApplicationContext.End();
            }
        }
    }
}
