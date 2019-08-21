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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            this.checkBoxRememberAccount = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.checkBoxRememberPwd = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.comboxOrg = new System.Windows.Forms.ComboBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnSyncOrg = new System.Windows.Forms.Button();
            this.btnClearCache = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblVersionNo = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblCopyRight = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtEMPNO = new System.Windows.Forms.TextBox();
            this.txtUserPwd = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxRememberAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxRememberPwd)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxRememberAccount
            // 
            this.checkBoxRememberAccount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBoxRememberAccount.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxRememberAccount.BackColorInternal = System.Drawing.Color.Transparent;
            this.checkBoxRememberAccount.Location = new System.Drawing.Point(330, 122);
            this.checkBoxRememberAccount.Name = "checkBoxRememberAccount";
            this.checkBoxRememberAccount.Size = new System.Drawing.Size(80, 25);
            this.checkBoxRememberAccount.TabIndex = 7;
            this.checkBoxRememberAccount.Text = "记住账号";
            this.checkBoxRememberAccount.CheckedChanged += new System.EventHandler(this.checkBoxRememberAccount_CheckedChanged);
            // 
            // checkBoxRememberPwd
            // 
            this.checkBoxRememberPwd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxRememberPwd.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxRememberPwd.BackColorInternal = System.Drawing.Color.Transparent;
            this.checkBoxRememberPwd.Location = new System.Drawing.Point(423, 122);
            this.checkBoxRememberPwd.Name = "checkBoxRememberPwd";
            this.checkBoxRememberPwd.Size = new System.Drawing.Size(80, 25);
            this.checkBoxRememberPwd.TabIndex = 8;
            this.checkBoxRememberPwd.Text = "记住密码";
            this.checkBoxRememberPwd.CheckedChanged += new System.EventHandler(this.checkBoxRememberPwd_CheckedChanged);
            // 
            // comboxOrg
            // 
            this.comboxOrg.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboxOrg.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboxOrg.FormattingEnabled = true;
            this.comboxOrg.Location = new System.Drawing.Point(123, 71);
            this.comboxOrg.Name = "comboxOrg";
            this.comboxOrg.Size = new System.Drawing.Size(144, 28);
            this.comboxOrg.TabIndex = 2;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(157)))), ((int)(((byte)(232)))));
            this.tableLayoutPanel1.SetColumnSpan(this.btnLogin, 4);
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(123, 173);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(344, 36);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "登  录";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnSyncOrg
            // 
            this.btnSyncOrg.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSyncOrg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(157)))), ((int)(((byte)(232)))));
            this.btnSyncOrg.FlatAppearance.BorderSize = 0;
            this.btnSyncOrg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncOrg.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnSyncOrg.ForeColor = System.Drawing.Color.White;
            this.btnSyncOrg.Location = new System.Drawing.Point(323, 71);
            this.btnSyncOrg.Name = "btnSyncOrg";
            this.btnSyncOrg.Size = new System.Drawing.Size(90, 28);
            this.btnSyncOrg.TabIndex = 5;
            this.btnSyncOrg.Text = "同步组织";
            this.btnSyncOrg.UseVisualStyleBackColor = false;
            this.btnSyncOrg.Click += new System.EventHandler(this.btnSyncOrg_Click);
            // 
            // btnClearCache
            // 
            this.btnClearCache.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnClearCache.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(157)))), ((int)(((byte)(232)))));
            this.btnClearCache.FlatAppearance.BorderSize = 0;
            this.btnClearCache.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearCache.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnClearCache.ForeColor = System.Drawing.Color.White;
            this.btnClearCache.Location = new System.Drawing.Point(423, 71);
            this.btnClearCache.Name = "btnClearCache";
            this.btnClearCache.Size = new System.Drawing.Size(90, 28);
            this.btnClearCache.TabIndex = 6;
            this.btnClearCache.Text = "清除缓存";
            this.btnClearCache.UseVisualStyleBackColor = false;
            this.btnClearCache.Click += new System.EventHandler(this.btnClearCache_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(66, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 16;
            this.label1.Text = "用户名";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(280, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 20);
            this.label2.TabIndex = 17;
            this.label2.Text = "工号";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(80, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 20);
            this.label3.TabIndex = 18;
            this.label3.Text = "密码";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(80, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 20);
            this.label4.TabIndex = 19;
            this.label4.Text = "组织";
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 20F);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(196, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(100, 40);
            this.lblTitle.TabIndex = 21;
            this.lblTitle.Text = "云汽配";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVersionNo
            // 
            this.lblVersionNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblVersionNo.AutoSize = true;
            this.lblVersionNo.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblVersionNo.ForeColor = System.Drawing.Color.White;
            this.lblVersionNo.Location = new System.Drawing.Point(302, 20);
            this.lblVersionNo.Name = "lblVersionNo";
            this.lblVersionNo.Size = new System.Drawing.Size(51, 20);
            this.lblVersionNo.TabIndex = 22;
            this.lblVersionNo.Text = "版本号";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblCopyRight, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnLogin, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.label2, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboxOrg, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnClearCache, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSyncOrg, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxRememberAccount, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxRememberPwd, 5, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtUserName, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtEMPNO, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtUserPwd, 2, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 87);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(598, 256);
            this.tableLayoutPanel1.TabIndex = 25;
            // 
            // lblCopyRight
            // 
            this.lblCopyRight.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.lblCopyRight, 6);
            this.lblCopyRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCopyRight.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblCopyRight.Location = new System.Drawing.Point(3, 216);
            this.lblCopyRight.Name = "lblCopyRight";
            this.lblCopyRight.Size = new System.Drawing.Size(592, 40);
            this.lblCopyRight.TabIndex = 23;
            this.lblCopyRight.Text = "label5";
            this.lblCopyRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtUserName
            // 
            this.txtUserName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtUserName.Font = new System.Drawing.Font("微软雅黑", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserName.Location = new System.Drawing.Point(123, 21);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(144, 28);
            this.txtUserName.TabIndex = 0;
            this.txtUserName.Enter += new System.EventHandler(this.txtUserName_Enter);
            this.txtUserName.Leave += new System.EventHandler(this.txtUserName_Leave);
            // 
            // txtEMPNO
            // 
            this.txtEMPNO.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel1.SetColumnSpan(this.txtEMPNO, 2);
            this.txtEMPNO.Font = new System.Drawing.Font("微软雅黑", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtEMPNO.Location = new System.Drawing.Point(323, 21);
            this.txtEMPNO.Name = "txtEMPNO";
            this.txtEMPNO.Size = new System.Drawing.Size(144, 28);
            this.txtEMPNO.TabIndex = 24;
            this.txtEMPNO.TextChanged += new System.EventHandler(this.txtEMPNO_TextChanged);
            this.txtEMPNO.Enter += new System.EventHandler(this.txtEMPNO_Enter);
            this.txtEMPNO.Leave += new System.EventHandler(this.txtEMPNO_Leave);
            // 
            // txtUserPwd
            // 
            this.txtUserPwd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtUserPwd.Font = new System.Drawing.Font("微软雅黑", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserPwd.Location = new System.Drawing.Point(123, 121);
            this.txtUserPwd.Name = "txtUserPwd";
            this.txtUserPwd.Size = new System.Drawing.Size(144, 28);
            this.txtUserPwd.TabIndex = 25;
            this.txtUserPwd.TextChanged += new System.EventHandler(this.txtUserPwd_TextChanged);
            this.txtUserPwd.Enter += new System.EventHandler(this.txtUserPwd_Enter);
            this.txtUserPwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserPwd_KeyDown);
            this.txtUserPwd.Leave += new System.EventHandler(this.txtUserPwd_Leave);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(196)))), ((int)(((byte)(241)))));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.lblTitle, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblVersionNo, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 28);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(598, 60);
            this.tableLayoutPanel2.TabIndex = 25;
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderWidth = 1;
            this.CapitionLogo = ((System.Drawing.Image)(resources.GetObject("$this.CapitionLogo")));
            this.CaptionHeight = 28;
            this.ClientSize = new System.Drawing.Size(600, 344);
            this.ControlBoxSize = new System.Drawing.Size(32, 28);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 344);
            this.MinimumSize = new System.Drawing.Size(600, 344);
            this.Name = "FrmLogin";
            this.ResizeEnable = false;
            this.Text = "登录系统";
            this.Activated += new System.EventHandler(this.FrmLogin_Activated);
            this.Deactivate += new System.EventHandler(this.FrmLogin_Deactivate);
            this.Load += new System.EventHandler(this.frmLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxRememberAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkBoxRememberPwd)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxRememberAccount;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor checkBoxRememberPwd;
        private System.Windows.Forms.ComboBox comboxOrg;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnSyncOrg;
        private System.Windows.Forms.Button btnClearCache;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblVersionNo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblCopyRight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtEMPNO;
        private System.Windows.Forms.TextBox txtUserPwd;
    }
}