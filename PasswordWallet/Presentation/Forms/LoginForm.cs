using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using PasswordWallet.Business;

namespace PasswordWallet.Presentation.Forms
{
    public partial class LoginForm : XtraForm
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private readonly WalletApplicationContext _context;

        public LoginForm(WalletApplicationContext context)
        {
            _context = context;
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            try
            {
                string hint = _context.MasterPassword.GetHint();
                if (!string.IsNullOrEmpty(hint))
                    labelControl3.Text += ": " + hint;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonEdit2_Toggled(object sender, EventArgs e)
        {
            buttonEdit1.Properties.UseSystemPasswordChar = !buttonEdit2.IsOn;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new AboutForm().Show();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            AttemptLogin();
        }

        private void AttemptLogin()
        {
            string password = buttonEdit1.Text ?? string.Empty;
            if (string.IsNullOrEmpty(password))
            {
                XtraMessageBox.Show("Please enter your master password.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                buttonEdit1.Focus();
                return;
            }

            try
            {
                if (!_context.MasterPassword.Validate(password))
                {
                    XtraMessageBox.Show("Login failed.\nPassword incorrect.", "Access Denied",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    buttonEdit1.Focus();
                    return;
                }

                _context.Unlock(password);

                var vault = new VaultForm(_context);
                vault.Show();
                Hide();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WalletApplicationContext.End();
            Application.Exit();
        }

        private void labelControl2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                simpleButton1.PerformClick();
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
            buttonEdit1.Focus();
        }

        private void navButton2_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            Close();
        }
    }
}
