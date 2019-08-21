namespace SkyCar.Coeus.Ult.Entrance
{
    partial class FrmLogin
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            this.txtEMPNO = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtUserPwd = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblLogin = new Infragistics.Win.Misc.UltraLabel();
            this.lblClose = new Infragistics.Win.Misc.UltraLabel();
            this.ultraVersionNo = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.checkBoxRememberAccount = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.checkBoxRememberPwd = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtUserName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblSyncOrg = new Infragistics.Win.Misc.UltraLabel();
            this.lblClearCache = new Infragistics.Win.Misc.UltraLabel();
            this.lblTitle = new Infragistics.Win.Misc.UltraLabel();
            this.comboxOrg = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtEMPNO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserPwd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxRememberAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxRememberPwd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName)).BeginInit();
            this.SuspendLayout();
            // 
            // txtEMPNO
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.Transparent;
            appearance1.BorderColor = System.Drawing.Color.Transparent;
            appearance1.BorderColor2 = System.Drawing.Color.Transparent;
            this.txtEMPNO.Appearance = appearance1;
            this.txtEMPNO.AutoSize = false;
            this.txtEMPNO.BackColor = System.Drawing.Color.White;
            this.txtEMPNO.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.txtEMPNO.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.txtEMPNO.Font = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtEMPNO.Location = new System.Drawing.Point(366, 152);
            this.txtEMPNO.MaxLength = 50;
            this.txtEMPNO.Name = "txtEMPNO";
            this.txtEMPNO.Size = new System.Drawing.Size(166, 27);
            this.txtEMPNO.TabIndex = 2;
            this.txtEMPNO.Tag = "";
            this.txtEMPNO.ValueChanged += new System.EventHandler(this.txtEMPNO_ValueChanged);
            this.txtEMPNO.Enter += new System.EventHandler(this.txtEMPNO_Enter);
            this.txtEMPNO.Leave += new System.EventHandler(this.txtEMPNO_Leave);
            // 
            // txtUserPwd
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.BackColor2 = System.Drawing.Color.Transparent;
            appearance2.BorderColor = System.Drawing.Color.Transparent;
            appearance2.BorderColor2 = System.Drawing.Color.Transparent;
            this.txtUserPwd.Appearance = appearance2;
            this.txtUserPwd.AutoSize = false;
            this.txtUserPwd.BackColor = System.Drawing.Color.White;
            this.txtUserPwd.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.txtUserPwd.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.txtUserPwd.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserPwd.Location = new System.Drawing.Point(147, 241);
            this.txtUserPwd.MaxLength = 50;
            this.txtUserPwd.Name = "txtUserPwd";
            this.txtUserPwd.PasswordChar = '*';
            this.txtUserPwd.Size = new System.Drawing.Size(166, 27);
            this.txtUserPwd.TabIndex = 4;
            this.txtUserPwd.ValueChanged += new System.EventHandler(this.txtUserPwd_ValueChanged);
            this.txtUserPwd.Enter += new System.EventHandler(this.txtUserPwd_Enter);
            this.txtUserPwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserPwd_KeyDown);
            this.txtUserPwd.Leave += new System.EventHandler(this.txtUserPwd_Leave);
            // 
            // lblLogin
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Middle";
            this.lblLogin.Appearance = appearance3;
            this.lblLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblLogin.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLogin.Location = new System.Drawing.Point(219, 289);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(203, 32);
            this.lblLogin.TabIndex = 7;
            this.lblLogin.Click += new System.EventHandler(this.lblLogin_Click);
            // 
            // lblClose
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            this.lblClose.Appearance = appearance4;
            this.lblClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblClose.Location = new System.Drawing.Point(620, 1);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(21, 20);
            this.lblClose.TabIndex = 7;
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            // 
            // ultraVersionNo
            // 
            appearance5.BackColor = System.Drawing.Color.Transparent;
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.TextHAlignAsString = "Right";
            appearance5.TextVAlignAsString = "Top";
            this.ultraVersionNo.Appearance = appearance5;
            this.ultraVersionNo.AutoSize = true;
            this.ultraVersionNo.Cursor = System.Windows.Forms.Cursors.Default;
            this.ultraVersionNo.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ultraVersionNo.Location = new System.Drawing.Point(554, 103);
            this.ultraVersionNo.Name = "ultraVersionNo";
            this.ultraVersionNo.Size = new System.Drawing.Size(58, 17);
            this.ultraVersionNo.TabIndex = 3;
            this.ultraVersionNo.Text = "1.0.0.0  ";
            // 
            // ultraLabel1
            // 
            appearance6.BackColor = System.Drawing.Color.Transparent;
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.TextHAlignAsString = "Right";
            appearance6.TextVAlignAsString = "Top";
            this.ultraLabel1.Appearance = appearance6;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.ultraLabel1.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ultraLabel1.Location = new System.Drawing.Point(114, 83);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(395, 51);
            this.ultraLabel1.TabIndex = 8;
            this.ultraLabel1.Text = "云汽配管理(Coeus)";
            // 
            // ultraLabel3
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.ForeColor = System.Drawing.Color.White;
            appearance7.TextHAlignAsString = "Right";
            appearance7.TextVAlignAsString = "Top";
            this.ultraLabel3.Appearance = appearance7;
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ultraLabel3.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ultraLabel3.Location = new System.Drawing.Point(88, 218);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(57, 27);
            this.ultraLabel3.TabIndex = 10;
            this.ultraLabel3.Text = "密  码";
            // 
            // checkBoxRememberAccount
            // 
            this.checkBoxRememberAccount.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxRememberAccount.BackColorInternal = System.Drawing.Color.Transparent;
            this.checkBoxRememberAccount.Location = new System.Drawing.Point(328, 251);
            this.checkBoxRememberAccount.Name = "checkBoxRememberAccount";
            this.checkBoxRememberAccount.Size = new System.Drawing.Size(77, 20);
            this.checkBoxRememberAccount.TabIndex = 5;
            this.checkBoxRememberAccount.Text = "记住账号";
            this.checkBoxRememberAccount.CheckedChanged += new System.EventHandler(this.checkBoxRememberAccount_CheckedChanged);
            // 
            // checkBoxRememberPwd
            // 
            this.checkBoxRememberPwd.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxRememberPwd.BackColorInternal = System.Drawing.Color.Transparent;
            this.checkBoxRememberPwd.Location = new System.Drawing.Point(434, 251);
            this.checkBoxRememberPwd.Name = "checkBoxRememberPwd";
            this.checkBoxRememberPwd.Size = new System.Drawing.Size(75, 20);
            this.checkBoxRememberPwd.TabIndex = 6;
            this.checkBoxRememberPwd.Text = "记住密码";
            this.checkBoxRememberPwd.CheckedChanged += new System.EventHandler(this.checkBoxRememberPwd_CheckedChanged);
            // 
            // txtUserName
            // 
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.BackColor2 = System.Drawing.Color.Transparent;
            appearance8.BorderColor = System.Drawing.Color.Transparent;
            appearance8.BorderColor2 = System.Drawing.Color.Transparent;
            this.txtUserName.Appearance = appearance8;
            this.txtUserName.AutoSize = false;
            this.txtUserName.BackColor = System.Drawing.Color.White;
            this.txtUserName.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.txtUserName.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.txtUserName.Font = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserName.Location = new System.Drawing.Point(147, 153);
            this.txtUserName.MaxLength = 50;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(166, 27);
            this.txtUserName.TabIndex = 1;
            this.txtUserName.Enter += new System.EventHandler(this.txtUserName_Enter);
            this.txtUserName.Leave += new System.EventHandler(this.txtUserName_Leave);
            // 
            // lblSyncOrg
            // 
            appearance9.BackColor = System.Drawing.Color.Transparent;
            appearance9.TextHAlignAsString = "Center";
            appearance9.TextVAlignAsString = "Middle";
            this.lblSyncOrg.Appearance = appearance9;
            this.lblSyncOrg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblSyncOrg.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSyncOrg.Location = new System.Drawing.Point(330, 204);
            this.lblSyncOrg.Name = "lblSyncOrg";
            this.lblSyncOrg.Size = new System.Drawing.Size(95, 20);
            this.lblSyncOrg.TabIndex = 7;
            this.lblSyncOrg.Click += new System.EventHandler(this.btnSyncOrg_Click);
            // 
            // lblClearCache
            // 
            appearance10.BackColor = System.Drawing.Color.Transparent;
            appearance10.TextHAlignAsString = "Center";
            appearance10.TextVAlignAsString = "Middle";
            this.lblClearCache.Appearance = appearance10;
            this.lblClearCache.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblClearCache.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblClearCache.Location = new System.Drawing.Point(439, 204);
            this.lblClearCache.Name = "lblClearCache";
            this.lblClearCache.Size = new System.Drawing.Size(96, 21);
            this.lblClearCache.TabIndex = 7;
            this.lblClearCache.Click += new System.EventHandler(this.btnClearCache_Click);
            // 
            // lblTitle
            // 
            appearance11.BackColor = System.Drawing.Color.Transparent;
            appearance11.ForeColor = System.Drawing.Color.White;
            appearance11.TextHAlignAsString = "Right";
            appearance11.TextVAlignAsString = "Top";
            this.lblTitle.Appearance = appearance11;
            this.lblTitle.Cursor = System.Windows.Forms.Cursors.NoMove2D;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.Location = new System.Drawing.Point(6, 1);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(606, 51);
            this.lblTitle.TabIndex = 8;
            this.lblTitle.Text = "                                           ";
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseDown);
            this.lblTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseMove);
            this.lblTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseUp);
            // 
            // comboxOrg
            // 
            this.comboxOrg.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboxOrg.FormattingEnabled = true;
            this.comboxOrg.Location = new System.Drawing.Point(147, 200);
            this.comboxOrg.Name = "comboxOrg";
            this.comboxOrg.Size = new System.Drawing.Size(166, 27);
            this.comboxOrg.TabIndex = 11;
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(643, 340);
            this.Controls.Add(this.comboxOrg);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.checkBoxRememberPwd);
            this.Controls.Add(this.checkBoxRememberAccount);
            this.Controls.Add(this.ultraLabel3);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.ultraVersionNo);
            this.Controls.Add(this.lblClose);
            this.Controls.Add(this.lblClearCache);
            this.Controls.Add(this.lblSyncOrg);
            this.Controls.Add(this.lblLogin);
            this.Controls.Add(this.txtUserPwd);
            this.Controls.Add(this.txtEMPNO);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "云汽配管理";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtEMPNO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserPwd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxRememberAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxRememberPwd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtEMPNO;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserPwd;
        private Infragistics.Win.Misc.UltraLabel lblLogin;
        private Infragistics.Win.Misc.UltraLabel lblClose;
        private Infragistics.Win.Misc.UltraLabel ultraVersionNo;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxRememberAccount;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxRememberPwd;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserName;
        private Infragistics.Win.Misc.UltraLabel lblSyncOrg;
        private Infragistics.Win.Misc.UltraLabel lblClearCache;
        private Infragistics.Win.Misc.UltraLabel lblTitle;
        private System.Windows.Forms.ComboBox comboxOrg;
    }
}