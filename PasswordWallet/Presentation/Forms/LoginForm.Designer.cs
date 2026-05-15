namespace PasswordWallet.Presentation.Forms
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.buttonEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.buttonEdit2 = new DevExpress.XtraEditors.ToggleSwitch();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.labelControl1.Location = new System.Drawing.Point(12, 24);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(103, 25);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Password";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Sitka Text", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.labelControl3.Location = new System.Drawing.Point(14, 110);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(32, 21);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "Hint";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.linkLabel1.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.linkLabel1.Location = new System.Drawing.Point(248, 161);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(46, 14);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "About";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.DimGray;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // buttonEdit1
            // 
            this.buttonEdit1.Location = new System.Drawing.Point(12, 76);
            this.buttonEdit1.Name = "buttonEdit1";
            this.buttonEdit1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEdit1.Properties.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("buttonEdit1.Properties.Appearance.Image")));
            this.buttonEdit1.Properties.Appearance.Options.UseFont = true;
            this.buttonEdit1.Properties.Appearance.Options.UseImage = true;
            this.buttonEdit1.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Dark Dark Gray";
            this.buttonEdit1.Properties.LookAndFeel.TouchUIMode = DevExpress.LookAndFeel.TouchUIMode.False;
            this.buttonEdit1.Properties.UseSystemPasswordChar = true;
            this.buttonEdit1.Size = new System.Drawing.Size(282, 30);
            this.buttonEdit1.TabIndex = 2;
            this.buttonEdit1.ToolTip = "Enter your \r\npassword";
            this.buttonEdit1.EditValueChanged += new System.EventHandler(this.buttonEdit1_EditValueChanged);
            this.buttonEdit1.TextChanged += new System.EventHandler(this.buttonEdit1_TextChanged);
            this.buttonEdit1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.buttonEdit1_KeyDown);
            this.buttonEdit1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonEdit1_MouseDown);
            // 
            // buttonEdit2
            // 
            this.buttonEdit2.Location = new System.Drawing.Point(194, 46);
            this.buttonEdit2.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEdit2.Name = "buttonEdit2";
            this.buttonEdit2.Properties.Appearance.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.buttonEdit2.Properties.Appearance.Options.UseForeColor = true;
            this.buttonEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.buttonEdit2.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.buttonEdit2.Properties.GlyphVAlignment = DevExpress.Utils.VertAlignment.Default;
            this.buttonEdit2.Properties.LookAndFeel.TouchUIMode = DevExpress.LookAndFeel.TouchUIMode.False;
            this.buttonEdit2.Properties.OffText = "Hide";
            this.buttonEdit2.Properties.OnText = "Show";
            this.buttonEdit2.Size = new System.Drawing.Size(100, 24);
            this.buttonEdit2.TabIndex = 1;
            this.buttonEdit2.Toggled += new System.EventHandler(this.buttonEdit2_Toggled);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton1.Appearance.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Appearance.Options.UseForeColor = true;
            this.simpleButton1.Location = new System.Drawing.Point(226, 112);
            this.simpleButton1.LookAndFeel.SkinName = "Visual Studio 2013 Dark";
            this.simpleButton1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(68, 33);
            this.simpleButton1.TabIndex = 3;
            this.simpleButton1.Text = "OK";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.simpleButton1);
            this.groupControl1.Controls.Add(this.buttonEdit2);
            this.groupControl1.Controls.Add(this.linkLabel1);
            this.groupControl1.Controls.Add(this.buttonEdit1);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(308, 178);
            this.groupControl1.TabIndex = 5;
            this.groupControl1.Text = "Login";
            // 
            // frmLogin
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Appearance.ForeColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(308, 178);
            this.Controls.Add(this.groupControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Visual Studio 2013 Dark";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Password Wallet - Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLogin_FormClosing);
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.Shown += new System.EventHandler(this.frmLogin_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private DevExpress.XtraEditors.TextEdit buttonEdit1;
        private DevExpress.XtraEditors.ToggleSwitch buttonEdit2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
    }
}