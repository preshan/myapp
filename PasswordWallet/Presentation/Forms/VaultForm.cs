using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using PasswordWallet.Business;
using PasswordWallet.Core.Contracts;
using PasswordWallet.Core.Models;

namespace PasswordWallet.Presentation.Forms
{
    public partial class VaultForm : XtraForm
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private readonly WalletApplicationContext _context;
        private string _clipboardUsername;
        private string _clipboardPassword;
        private string _link = string.Empty;
        private string _highlightUrl = string.Empty;

        public VaultForm(WalletApplicationContext context)
        {
            _context = context;
            InitializeComponent();
        }

        private ICredentialRepository Credentials
        {
            get { return _context.UnitOfWork.Credentials; }
        }

        private void backstageViewButtonItem1_ItemClick(object sender, BackstageViewItemEventArgs e)
        {
            WalletApplicationContext.End();
            var login = new LoginForm(WalletApplicationContext.StartNew());
            login.Show();
            Hide();
        }

        private void FillList(ListBoxControl listBox, IList names)
        {
            listBox.Items.Clear();
            listBox.Items.AddRange(names);
        }

        private void FillAll(ListBoxControl listBox)
        {
            FillList(listBox, Credentials.GetAllNames());
        }

        private void SearchByName(ButtonEdit textBox, ListBoxControl listBox)
        {
            FillList(listBox, Credentials.SearchByPrefix(textBox.Text ?? string.Empty, CredentialSearchField.Name));
        }

        private void DisplayCredential(Credential entry, RichTextBox target)
        {
            if (entry == null)
            {
                target.ResetText();
                return;
            }

            _clipboardUsername = entry.Username;
            _clipboardPassword = entry.Password;
            _highlightUrl = entry.Url ?? string.Empty;

            if (string.IsNullOrEmpty(entry.Url) && string.IsNullOrEmpty(entry.Notes))
                target.Text = "Name\t:" + entry.Name + "\n\nUser Name\t: " + entry.Username + "\n\nPassword\t: " + entry.Password;
            else if (string.IsNullOrEmpty(entry.Url))
                target.Text = "Name\t:" + entry.Name + "\n\nUser Name\t: " + entry.Username + "\n\nPassword\t: " + entry.Password + "\n\nOther\t\t: " + entry.Notes;
            else if (string.IsNullOrEmpty(entry.Notes))
                target.Text = "Name\t:" + entry.Name + "\n\nUser Name\t: " + entry.Username + "\n\nPassword\t: " + entry.Password + "\n\nURL\t\t: " + entry.Url;
            else
                target.Text = "Name\t:" + entry.Name + "\n\nUser Name\t: " + entry.Username + "\n\nPassword\t: " + entry.Password + "\n\nURL\t\t: " + entry.Url + "\n\nOther\t\t: " + entry.Notes;
        }

        private void buttonEdit1_TextChanged(object sender, EventArgs e)
        {
            SearchByName(buttonEdit1, listBoxControl1);
        }

        private void navButton2_ElementClick_1(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            Close();
        }

        private void backstageViewClientControl1_Load(object sender, EventArgs e)
        {
            FillAll(listBoxControl1);
            buttonEdit1.Focus();
            backstageViewClientControl1.BackColor = Color.DarkViolet;
        }

        private void listBoxControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxControl1.SelectedItem == null) return;
            DisplayCredential(Credentials.GetByName(listBoxControl1.SelectedItem.ToString()), richTextBox1);
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_link))
                    System.Diagnostics.Process.Start(_link);
            }
            catch (Exception) { }
        }

        private void ColourRrbText(RichTextBox rtb)
        {
            try
            {
                if (string.IsNullOrEmpty(_highlightUrl)) return;
                var pattern = new Regex(Regex.Escape(_highlightUrl));
                foreach (Match match in pattern.Matches(rtb.Text))
                {
                    rtb.Select(match.Index, match.Length);
                    rtb.SelectionColor = Color.Blue;
                    _link = rtb.SelectedText;
                    rtb.Refresh();
                }
            }
            catch (Exception) { }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            ColourRrbText(richTextBox1);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                _context.Credentials.Add(new Credential
                {
                    Name = textEdit1.Text,
                    Url = textEdit2.Text,
                    Username = textEdit3.Text,
                    Password = textEdit4.Text,
                    Notes = textEdit5.Text
                });
                XtraMessageBox.Show("Successfully Added", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textEdit1.ResetText();
                textEdit2.ResetText();
                textEdit3.ResetText();
                textEdit4.ResetText();
                textEdit5.ResetText();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void formReload()
        {
            buttonEdit1.ResetText();
            listBoxControl1.Items.Clear();
            richTextBox1.ResetText();
            FillAll(listBoxControl1);
        }

        private void backstageViewTabItem1_ItemPressed(object sender, BackstageViewItemEventArgs e)
        {
            formReload();
        }

        private void backstageViewClientControl3_Load(object sender, EventArgs e)
        {
            FillAll(listBoxControl2);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (listBoxControl2.SelectedIndex == -1)
            {
                XtraMessageBox.Show("Please select an item \nfirst", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                string originalName = listBoxControl2.SelectedItem.ToString();
                var existing = Credentials.GetByName(originalName);
                var updated = new Credential
                {
                    Name = txtname.Text,
                    Url = txturl.Text,
                    Username = txtuname.Text,
                    Password = txtpw.Text,
                    Notes = txtother.Text
                };

                if (existing != null &&
                    existing.Name == updated.Name &&
                    existing.Url == updated.Url &&
                    existing.Username == updated.Username &&
                    existing.Password == updated.Password &&
                    existing.Notes == updated.Notes)
                {
                    XtraMessageBox.Show("successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _context.Credentials.Update(originalName, updated);
                XtraMessageBox.Show("Successfully Updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listBoxControl2.Items.Clear();
                FillAll(listBoxControl2);
                txtname.Text = "";
                txtuname.Text = "";
                txturl.Text = "";
                txtpw.Text = "";
                txtother.Text = "";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void listBoxControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxControl2.SelectedItem == null) return;
            var entry = Credentials.GetByName(listBoxControl2.SelectedItem.ToString());
            if (entry == null) return;
            txtname.Text = entry.Name;
            txturl.Text = entry.Url;
            txtuname.Text = entry.Username;
            txtpw.Text = entry.Password;
            txtother.Text = entry.Notes;
            _highlightUrl = entry.Url;
        }

        private void backstageViewClientControl4_Load(object sender, EventArgs e)
        {
            FillAll(listBoxControl3);
        }

        private void buttonEdit2_TextChanged(object sender, EventArgs e)
        {
            SearchByName(buttonEdit2, listBoxControl2);
        }

        private void buttonEdit3_TextChanged(object sender, EventArgs e)
        {
            if (!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked)
                radioButton1.PerformClick();

            CredentialSearchField field = CredentialSearchField.Name;
            if (radioButton2.Checked) field = CredentialSearchField.Url;
            else if (radioButton3.Checked) field = CredentialSearchField.Username;

            FillList(listBoxControl3, Credentials.SearchByPrefix(buttonEdit3.Text ?? string.Empty, field));
        }

        private void listBoxControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxControl3.SelectedItem == null) return;
            DisplayCredential(Credentials.GetByName(listBoxControl3.SelectedItem.ToString()), richTextBox2);
        }

        private void backstageViewClientControl5_Load(object sender, EventArgs e)
        {
            FillList(checkedListBoxControl1, Credentials.GetAllNames());
        }

        private void backstageViewTabItem5_ItemPressed(object sender, BackstageViewItemEventArgs e)
        {
            checkedListBoxControl1.Items.Clear();
            FillList(checkedListBoxControl1, Credentials.GetAllNames());
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (checkedListBoxControl1.CheckedItems.Count == 0)
            {
                XtraMessageBox.Show("Please select an item to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var names = new ArrayList();
            for (int i = 0; i < checkedListBoxControl1.Items.Count; i++)
            {
                if (checkedListBoxControl1.GetItemCheckState(i) == CheckState.Checked)
                    names.Add(checkedListBoxControl1.GetItemText(i));
            }

            string text = string.Join(", ", names.ToArray());
            var result = MessageBox.Show("Are you sure you want to delete\n" + text, "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (result != DialogResult.Yes) return;

            foreach (string name in names)
                _context.Credentials.Delete(name.ToString());

            XtraMessageBox.Show("Successfully Deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            checkedListBoxControl1.Items.Clear();
            FillList(checkedListBoxControl1, Credentials.GetAllNames());
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to delete all data\nYou cannot undo this operation!",
                "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (result != DialogResult.Yes) return;

            _context.Credentials.DeleteAll();
            XtraMessageBox.Show("Successfully Deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            checkedListBoxControl1.Items.Clear();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            try
            {
                _context.MasterPassword.Change(currentPw.Text, newPw.Text, passHint.Text);
                _context.UnitOfWork.Lock();
                _context.Unlock(newPw.Text);
                XtraMessageBox.Show("Successfully Updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                currentPw.Text = "";
                newPw.Text = "";
                confPw.Text = "";
                passHint.Text = "";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new AboutForm().Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            WalletApplicationContext.End();
            Application.Exit();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try { Clipboard.SetText(_clipboardUsername); } catch (Exception) { }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            try { Clipboard.SetText(_clipboardPassword); } catch (Exception) { }
        }

        private void backstageViewTabItem4_ItemPressed(object sender, BackstageViewItemEventArgs e)
        {
            listBoxControl3.Items.Clear();
            FillAll(listBoxControl3);
            buttonEdit3.Focus();
        }

        private void backstageViewTabItem2_ItemPressed(object sender, BackstageViewItemEventArgs e)
        {
            textEdit1.Focus();
        }

        private void backstageViewTabItem3_ItemPressed(object sender, BackstageViewItemEventArgs e)
        {
            txtname.Text = "";
            txtuname.Text = "";
            txturl.Text = "";
            txtpw.Text = "";
            txtother.Text = "";
            listBoxControl2.Items.Clear();
            FillAll(listBoxControl2);
            buttonEdit2.Focus();
        }

        private void backstageViewTabItem6_ItemPressed(object sender, BackstageViewItemEventArgs e)
        {
            currentPw.Focus();
        }

        private void navButton3_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            Close();
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

        private void currentPw_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) simpleButton5.PerformClick(); }
        private void newPw_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) simpleButton5.PerformClick(); }
        private void confPw_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) simpleButton5.PerformClick(); }
        private void passHint_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) simpleButton5.PerformClick(); }
        private void checkedListBoxControl1_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) simpleButton3.PerformClick(); }
        private void txtname_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) simpleButton2.PerformClick(); }
        private void txturl_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) simpleButton2.PerformClick(); }
        private void txtuname_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) simpleButton2.PerformClick(); }
        private void txtpw_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) simpleButton2.PerformClick(); }
        private void txtother_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) simpleButton2.PerformClick(); }
        private void textEdit1_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) simpleButton1.PerformClick(); }
        private void textEdit2_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) simpleButton1.PerformClick(); }
        private void textEdit3_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) simpleButton1.PerformClick(); }
        private void textEdit4_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) simpleButton1.PerformClick(); }
        private void textEdit5_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) simpleButton1.PerformClick(); }
        private void labelControl17_Click(object sender, EventArgs e) { }
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
        private void toolStrip1_KeyDown(object sender, KeyEventArgs e) { }
        private void buttonEdit3_EditValueChanged(object sender, EventArgs e) { }
        private void backstageViewTabItem4_SelectedChanged(object sender, BackstageViewItemEventArgs e) { }
        private void backstageViewTabItem1_SelectedChanged(object sender, BackstageViewItemEventArgs e) { }
        private void backstageViewTabItem6_SelectedChanged(object sender, BackstageViewItemEventArgs e) { }
    }
}
