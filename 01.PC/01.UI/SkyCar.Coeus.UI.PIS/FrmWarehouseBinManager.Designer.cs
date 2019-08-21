namespace SkyCar.Coeus.UI.PIS
{
    partial class FrmWarehouseBinManager
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
            this.txtWHB_Name = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.txtWHB_Description = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.tableLayoutPanelCondition = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lbl = new Infragistics.Win.Misc.UltraLabel();
            this.txtWHB_CreatedBy = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.dtWHB_CreatedTime = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtWHB_UpdatedTime = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.txtWHB_WH_ID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtWHB_ID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel8 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.txtWHB_UpdatedBy = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel9 = new Infragistics.Win.Misc.UltraLabel();
            this.txtWHB_VersionNo = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtTmp_SID_ID = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.ckWHB_IsValid = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.txtWHB_Name)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWHB_Description)).BeginInit();
            this.tableLayoutPanelCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtWHB_CreatedBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtWHB_CreatedTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtWHB_UpdatedTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWHB_WH_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWHB_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWHB_UpdatedBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWHB_VersionNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTmp_SID_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckWHB_IsValid)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtWHB_Name
            // 
            this.txtWHB_Name.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtWHB_Name.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtWHB_Name.Location = new System.Drawing.Point(83, 33);
            this.txtWHB_Name.Name = "txtWHB_Name";
            this.txtWHB_Name.Size = new System.Drawing.Size(120, 28);
            this.txtWHB_Name.TabIndex = 3;
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ultraLabel1.Location = new System.Drawing.Point(-2, 0);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ultraLabel1.Size = new System.Drawing.Size(74, 21);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "仓位名称";
            // 
            // txtWHB_Description
            // 
            this.txtWHB_Description.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanelCondition.SetColumnSpan(this.txtWHB_Description, 4);
            this.txtWHB_Description.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtWHB_Description.Location = new System.Drawing.Point(315, 33);
            this.txtWHB_Description.Name = "txtWHB_Description";
            this.txtWHB_Description.Size = new System.Drawing.Size(454, 28);
            this.txtWHB_Description.TabIndex = 4;
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ultraLabel2.Location = new System.Drawing.Point(235, 33);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ultraLabel2.Size = new System.Drawing.Size(74, 23);
            this.ultraLabel2.TabIndex = 2;
            this.ultraLabel2.Text = "仓位描述";
            // 
            // tableLayoutPanelCondition
            // 
            this.tableLayoutPanelCondition.ColumnCount = 8;
            this.tableLayoutPanelCondition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelCondition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCondition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelCondition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCondition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelCondition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCondition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelCondition.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCondition.Controls.Add(this.txtWHB_Description, 3, 1);
            this.tableLayoutPanelCondition.Controls.Add(this.ultraLabel2, 2, 1);
            this.tableLayoutPanelCondition.Controls.Add(this.txtWHB_Name, 1, 1);
            this.tableLayoutPanelCondition.Controls.Add(this.btnCancel, 5, 3);
            this.tableLayoutPanelCondition.Controls.Add(this.btnSave, 3, 3);
            this.tableLayoutPanelCondition.Controls.Add(this.lbl, 0, 2);
            this.tableLayoutPanelCondition.Controls.Add(this.txtWHB_CreatedBy, 1, 2);
            this.tableLayoutPanelCondition.Controls.Add(this.ultraLabel4, 2, 2);
            this.tableLayoutPanelCondition.Controls.Add(this.ultraLabel6, 6, 2);
            this.tableLayoutPanelCondition.Controls.Add(this.dtWHB_CreatedTime, 3, 2);
            this.tableLayoutPanelCondition.Controls.Add(this.dtWHB_UpdatedTime, 7, 2);
            this.tableLayoutPanelCondition.Controls.Add(this.txtWHB_WH_ID, 1, 0);
            this.tableLayoutPanelCondition.Controls.Add(this.txtWHB_ID, 3, 0);
            this.tableLayoutPanelCondition.Controls.Add(this.ultraLabel8, 2, 0);
            this.tableLayoutPanelCondition.Controls.Add(this.ultraLabel5, 4, 2);
            this.tableLayoutPanelCondition.Controls.Add(this.txtWHB_UpdatedBy, 5, 2);
            this.tableLayoutPanelCondition.Controls.Add(this.ultraLabel9, 4, 0);
            this.tableLayoutPanelCondition.Controls.Add(this.txtWHB_VersionNo, 5, 0);
            this.tableLayoutPanelCondition.Controls.Add(this.txtTmp_SID_ID, 6, 0);
            this.tableLayoutPanelCondition.Controls.Add(this.ultraLabel7, 0, 0);
            this.tableLayoutPanelCondition.Controls.Add(this.ckWHB_IsValid, 7, 1);
            this.tableLayoutPanelCondition.Controls.Add(this.ultraPanel1, 0, 1);
            this.tableLayoutPanelCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelCondition.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tableLayoutPanelCondition.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelCondition.Name = "tableLayoutPanelCondition";
            this.tableLayoutPanelCondition.RowCount = 4;
            this.tableLayoutPanelCondition.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelCondition.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelCondition.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelCondition.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelCondition.Size = new System.Drawing.Size(928, 140);
            this.tableLayoutPanelCondition.TabIndex = 1;
            // 
            // btnCancel
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(157)))), ((int)(((byte)(232)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance1.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance1.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Appearance = appearance1;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Location = new System.Drawing.Point(547, 103);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 34);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(157)))), ((int)(((byte)(232)))));
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.ForeColor = System.Drawing.Color.White;
            this.btnSave.Appearance = appearance2;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSave.Location = new System.Drawing.Point(315, 103);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 34);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "确定";
            this.btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lbl
            // 
            this.lbl.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbl.Location = new System.Drawing.Point(3, 68);
            this.lbl.Name = "lbl";
            this.lbl.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbl.Size = new System.Drawing.Size(74, 23);
            this.lbl.TabIndex = 16;
            this.lbl.Text = "创建人";
            this.lbl.Visible = false;
            // 
            // txtWHB_CreatedBy
            // 
            this.txtWHB_CreatedBy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtWHB_CreatedBy.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtWHB_CreatedBy.Enabled = false;
            this.txtWHB_CreatedBy.Location = new System.Drawing.Point(83, 66);
            this.txtWHB_CreatedBy.Name = "txtWHB_CreatedBy";
            this.txtWHB_CreatedBy.ReadOnly = true;
            this.txtWHB_CreatedBy.Size = new System.Drawing.Size(110, 28);
            this.txtWHB_CreatedBy.TabIndex = 6;
            this.txtWHB_CreatedBy.TabStop = false;
            this.txtWHB_CreatedBy.Visible = false;
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ultraLabel4.Location = new System.Drawing.Point(235, 68);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ultraLabel4.Size = new System.Drawing.Size(74, 23);
            this.ultraLabel4.TabIndex = 16;
            this.ultraLabel4.Text = "创建时间";
            this.ultraLabel4.Visible = false;
            // 
            // ultraLabel6
            // 
            this.ultraLabel6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ultraLabel6.Location = new System.Drawing.Point(699, 68);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ultraLabel6.Size = new System.Drawing.Size(74, 23);
            this.ultraLabel6.TabIndex = 16;
            this.ultraLabel6.Text = "修改时间";
            this.ultraLabel6.Visible = false;
            // 
            // dtWHB_CreatedTime
            // 
            this.dtWHB_CreatedTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtWHB_CreatedTime.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.dtWHB_CreatedTime.Enabled = false;
            this.dtWHB_CreatedTime.FormatString = "yyyy/MM/dd HH:mm:ss";
            this.dtWHB_CreatedTime.Location = new System.Drawing.Point(315, 66);
            this.dtWHB_CreatedTime.Name = "dtWHB_CreatedTime";
            this.dtWHB_CreatedTime.ReadOnly = true;
            this.dtWHB_CreatedTime.Size = new System.Drawing.Size(145, 28);
            this.dtWHB_CreatedTime.TabIndex = 7;
            this.dtWHB_CreatedTime.TabStop = false;
            this.dtWHB_CreatedTime.Visible = false;
            // 
            // dtWHB_UpdatedTime
            // 
            this.dtWHB_UpdatedTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dtWHB_UpdatedTime.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.dtWHB_UpdatedTime.Enabled = false;
            this.dtWHB_UpdatedTime.FormatString = "yyyy/MM/dd HH:mm:ss";
            this.dtWHB_UpdatedTime.Location = new System.Drawing.Point(779, 66);
            this.dtWHB_UpdatedTime.Name = "dtWHB_UpdatedTime";
            this.dtWHB_UpdatedTime.ReadOnly = true;
            this.dtWHB_UpdatedTime.Size = new System.Drawing.Size(145, 28);
            this.dtWHB_UpdatedTime.TabIndex = 9;
            this.dtWHB_UpdatedTime.TabStop = false;
            this.dtWHB_UpdatedTime.Visible = false;
            // 
            // txtWHB_WH_ID
            // 
            this.txtWHB_WH_ID.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtWHB_WH_ID.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtWHB_WH_ID.Location = new System.Drawing.Point(83, 3);
            this.txtWHB_WH_ID.Name = "txtWHB_WH_ID";
            this.txtWHB_WH_ID.Size = new System.Drawing.Size(110, 28);
            this.txtWHB_WH_ID.TabIndex = 0;
            this.txtWHB_WH_ID.TabStop = false;
            this.txtWHB_WH_ID.Visible = false;
            // 
            // txtWHB_ID
            // 
            this.txtWHB_ID.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtWHB_ID.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtWHB_ID.Location = new System.Drawing.Point(315, 3);
            this.txtWHB_ID.Name = "txtWHB_ID";
            this.txtWHB_ID.Size = new System.Drawing.Size(110, 28);
            this.txtWHB_ID.TabIndex = 1;
            this.txtWHB_ID.TabStop = false;
            this.txtWHB_ID.Visible = false;
            // 
            // ultraLabel8
            // 
            this.ultraLabel8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ultraLabel8.Location = new System.Drawing.Point(235, 5);
            this.ultraLabel8.Name = "ultraLabel8";
            this.ultraLabel8.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ultraLabel8.Size = new System.Drawing.Size(74, 20);
            this.ultraLabel8.TabIndex = 2;
            this.ultraLabel8.Text = "仓位ID";
            this.ultraLabel8.Visible = false;
            // 
            // ultraLabel5
            // 
            this.ultraLabel5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ultraLabel5.Location = new System.Drawing.Point(467, 68);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ultraLabel5.Size = new System.Drawing.Size(74, 23);
            this.ultraLabel5.TabIndex = 16;
            this.ultraLabel5.Text = "修改人";
            this.ultraLabel5.Visible = false;
            // 
            // txtWHB_UpdatedBy
            // 
            this.txtWHB_UpdatedBy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtWHB_UpdatedBy.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtWHB_UpdatedBy.Enabled = false;
            this.txtWHB_UpdatedBy.Location = new System.Drawing.Point(547, 66);
            this.txtWHB_UpdatedBy.Name = "txtWHB_UpdatedBy";
            this.txtWHB_UpdatedBy.ReadOnly = true;
            this.txtWHB_UpdatedBy.Size = new System.Drawing.Size(110, 28);
            this.txtWHB_UpdatedBy.TabIndex = 8;
            this.txtWHB_UpdatedBy.TabStop = false;
            this.txtWHB_UpdatedBy.Visible = false;
            // 
            // ultraLabel9
            // 
            this.ultraLabel9.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ultraLabel9.Location = new System.Drawing.Point(467, 5);
            this.ultraLabel9.Name = "ultraLabel9";
            this.ultraLabel9.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ultraLabel9.Size = new System.Drawing.Size(74, 20);
            this.ultraLabel9.TabIndex = 2;
            this.ultraLabel9.Text = "版本号";
            this.ultraLabel9.Visible = false;
            // 
            // txtWHB_VersionNo
            // 
            this.txtWHB_VersionNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtWHB_VersionNo.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtWHB_VersionNo.Location = new System.Drawing.Point(547, 3);
            this.txtWHB_VersionNo.Name = "txtWHB_VersionNo";
            this.txtWHB_VersionNo.Size = new System.Drawing.Size(110, 28);
            this.txtWHB_VersionNo.TabIndex = 2;
            this.txtWHB_VersionNo.TabStop = false;
            this.txtWHB_VersionNo.Visible = false;
            // 
            // txtTmp_SID_ID
            // 
            this.txtTmp_SID_ID.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtTmp_SID_ID.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtTmp_SID_ID.Location = new System.Drawing.Point(699, 3);
            this.txtTmp_SID_ID.Name = "txtTmp_SID_ID";
            this.txtTmp_SID_ID.Size = new System.Drawing.Size(74, 28);
            this.txtTmp_SID_ID.TabIndex = 15;
            this.txtTmp_SID_ID.Visible = false;
            // 
            // ultraLabel7
            // 
            this.ultraLabel7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ultraLabel7.Location = new System.Drawing.Point(3, 5);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ultraLabel7.Size = new System.Drawing.Size(74, 20);
            this.ultraLabel7.TabIndex = 2;
            this.ultraLabel7.Text = "仓库ID";
            this.ultraLabel7.Visible = false;
            // 
            // ckWHB_IsValid
            // 
            this.ckWHB_IsValid.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ckWHB_IsValid.Checked = true;
            this.ckWHB_IsValid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckWHB_IsValid.Enabled = false;
            this.ckWHB_IsValid.Location = new System.Drawing.Point(779, 35);
            this.ckWHB_IsValid.Name = "ckWHB_IsValid";
            this.ckWHB_IsValid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ckWHB_IsValid.Size = new System.Drawing.Size(87, 20);
            this.ckWHB_IsValid.TabIndex = 5;
            this.ckWHB_IsValid.Text = "有效";
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraLabel3);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraLabel1);
            this.ultraPanel1.Location = new System.Drawing.Point(3, 33);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(74, 24);
            this.ultraPanel1.TabIndex = 19;
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            appearance3.ForeColor = System.Drawing.Color.Red;
            this.ultraLabel3.Appearance = appearance3;
            this.ultraLabel3.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.ultraLabel3.Location = new System.Drawing.Point(67, 4);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(18, 18);
            this.ultraLabel3.TabIndex = 47;
            this.ultraLabel3.Text = "*";
            // 
            // FrmWarehouseBinManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(928, 140);
            this.Controls.Add(this.tableLayoutPanelCondition);
            this.Name = "FrmWarehouseBinManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "仓位管理";
            this.Load += new System.EventHandler(this.FrmWarehouseBinManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtWHB_Name)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWHB_Description)).EndInit();
            this.tableLayoutPanelCondition.ResumeLayout(false);
            this.tableLayoutPanelCondition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtWHB_CreatedBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtWHB_CreatedTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtWHB_UpdatedTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWHB_WH_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWHB_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWHB_UpdatedBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWHB_VersionNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTmp_SID_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckWHB_IsValid)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtWHB_Name;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtWHB_Description;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCondition;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ckWHB_IsValid;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtWHB_WH_ID;
        private Infragistics.Win.Misc.UltraLabel ultraLabel8;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtWHB_ID;
        private Infragistics.Win.Misc.UltraLabel ultraLabel9;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtWHB_VersionNo;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtTmp_SID_ID;
        private Infragistics.Win.Misc.UltraLabel lbl;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtWHB_CreatedBy;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtWHB_UpdatedBy;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtWHB_CreatedTime;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtWHB_UpdatedTime;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
    }
}