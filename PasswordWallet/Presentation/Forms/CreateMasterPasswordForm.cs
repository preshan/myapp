using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using PasswordWallet.Business;

namespace PasswordWallet.Presentation.Forms
{
    public partial class CreateMasterPasswordForm : XtraForm
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private readonly WalletApplicationContext _context;

        public CreateMasterPasswordForm(WalletApplicationContext context)
        {
            _context = context;
            InitializeComponent();
        }

        private void CreateMasterPasswordForm_Load(object sender, EventArgs e)
        {
        }

        private void navButton2_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            Close();
        }

        private void CreateMasterPasswordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WalletApplicationContext.End();
            Application.Exit();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(password.Text))
            {
                XtraMessageBox.Show("Please enter password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                password.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(confPw.Text))
            {
                XtraMessageBox.Show("Please enter Password again \n to confirm password", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                confPw.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(passHint.Text))
            {
                XtraMessageBox.Show("Please enter password hint", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                passHint.Focus();
                return;
            }
            if (password.Text != confPw.Text)
            {
                XtraMessageBox.Show("Please check confirmation \n password again", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                confPw.Focus();
                return;
            }

            try
            {
                _context.MasterPassword.Create(password.Text, passHint.Text);
                XtraMessageBox.Show(
                    "You have successfully added a master password.\nDO NOT FORGET your password",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Hide();
                new LoginForm(_context).Show();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) simpleButton5.PerformClick();
        }

        private void confPw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) simpleButton5.PerformClick();
        }

        private void passHint_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) simpleButton5.PerformClick();
        }
    }
}
