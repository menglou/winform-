namespace SkyCar.Coeus.Common.CustomControl
{
    sealed partial class SkyCarPictureExpand
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.toolTipPanelPicture = new System.Windows.Forms.ToolTip(this.components);
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.btnExport = new Infragistics.Win.Misc.UltraButton();
            this.btnUpload = new Infragistics.Win.Misc.UltraButton();
            this.pictureBox = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.gbPictureExpand = new Infragistics.Win.Misc.UltraGroupBox();
            this.tableLayoutPanelPicture = new System.Windows.Forms.TableLayoutPanel();
            this.gbPicture = new Infragistics.Win.Misc.UltraGroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ckCheckd = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            ((System.ComponentModel.ISupportInitialize)(this.gbPictureExpand)).BeginInit();
            this.gbPictureExpand.SuspendLayout();
            this.tableLayoutPanelPicture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbPicture)).BeginInit();
            this.gbPicture.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckCheckd)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            appearance1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(157)))), ((int)(((byte)(232)))));
            appearance1.ImageBackground = global::SkyCar.Coeus.Common.CustomControl.Properties.Resources.Delete;
            this.btnDelete.Appearance = appearance1;
            this.btnDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2010Button;
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDelete.Font = new System.Drawing.Font("微软雅黑", 9.5F);
            this.btnDelete.Location = new System.Drawing.Point(170, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(15, 17);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnExport
            // 
            appearance2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(157)))), ((int)(((byte)(232)))));
            appearance2.ImageBackground = global::SkyCar.Coeus.Common.CustomControl.Properties.Resources.Export;
            this.btnExport.Appearance = appearance2;
            this.btnExport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2010Button;
            this.btnExport.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExport.Font = new System.Drawing.Font("微软雅黑", 9.5F);
            this.btnExport.Location = new System.Drawing.Point(123, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(15, 17);
            this.btnExport.TabIndex = 2;
            this.btnExport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnUpload
            // 
            appearance3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(157)))), ((int)(((byte)(232)))));
            appearance3.ImageBackground = global::SkyCar.Coeus.Common.CustomControl.Properties.Resources.upload;
            this.btnUpload.Appearance = appearance3;
            this.btnUpload.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2010Button;
            this.btnUpload.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnUpload.Font = new System.Drawing.Font("微软雅黑", 9.5F);
            this.btnUpload.Location = new System.Drawing.Point(76, 3);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(15, 17);
            this.btnUpload.TabIndex = 1;
            this.btnUpload.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // pictureBox
            // 
            appearance4.BorderColor = System.Drawing.Color.Gray;
            this.pictureBox.Appearance = appearance4;
            this.pictureBox.BorderShadowColor = System.Drawing.Color.Empty;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(3, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(182, 103);
            this.pictureBox.TabIndex = 5;
            this.pictureBox.DoubleClick += new System.EventHandler(this.pictureBox_DoubleClick);
            this.pictureBox.MouseHover += new System.EventHandler(this.pictureBox_MouseHover);
            // 
            // gbPictureExpand
            // 
            this.gbPictureExpand.Controls.Add(this.tableLayoutPanelPicture);
            this.gbPictureExpand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPictureExpand.Location = new System.Drawing.Point(0, 0);
            this.gbPictureExpand.Name = "gbPictureExpand";
            this.gbPictureExpand.Size = new System.Drawing.Size(200, 150);
            this.gbPictureExpand.TabIndex = 6;
            // 
            // tableLayoutPanelPicture
            // 
            this.tableLayoutPanelPicture.ColumnCount = 1;
            this.tableLayoutPanelPicture.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPicture.Controls.Add(this.gbPicture, 0, 0);
            this.tableLayoutPanelPicture.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanelPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPicture.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelPicture.Name = "tableLayoutPanelPicture";
            this.tableLayoutPanelPicture.RowCount = 2;
            this.tableLayoutPanelPicture.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanelPicture.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelPicture.Size = new System.Drawing.Size(194, 144);
            this.tableLayoutPanelPicture.TabIndex = 7;
            // 
            // gbPicture
            // 
            this.gbPicture.Controls.Add(this.pictureBox);
            this.gbPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPicture.Location = new System.Drawing.Point(3, 3);
            this.gbPicture.Name = "gbPicture";
            this.gbPicture.Size = new System.Drawing.Size(188, 109);
            this.gbPicture.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.btnDelete, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnExport, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnUpload, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.ckCheckd, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 118);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(188, 23);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // ckCheckd
            // 
            this.ckCheckd.Dock = System.Windows.Forms.DockStyle.Left;
            this.ckCheckd.Location = new System.Drawing.Point(3, 3);
            this.ckCheckd.Name = "ckCheckd";
            this.ckCheckd.Size = new System.Drawing.Size(20, 17);
            this.ckCheckd.TabIndex = 4;
            this.ckCheckd.CheckedChanged += new System.EventHandler(this.ckCheckd_CheckedChanged);
            // 
            // SkyCarPictureExpand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.gbPictureExpand);
            this.MaximumSize = new System.Drawing.Size(200, 150);
            this.MinimumSize = new System.Drawing.Size(160, 120);
            this.Name = "SkyCarPictureExpand";
            this.Size = new System.Drawing.Size(200, 150);
            this.Load += new System.EventHandler(this.SkyCarPictureExpand_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gbPictureExpand)).EndInit();
            this.gbPictureExpand.ResumeLayout(false);
            this.tableLayoutPanelPicture.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbPicture)).EndInit();
            this.gbPicture.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ckCheckd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTipPanelPicture;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private Infragistics.Win.Misc.UltraButton btnExport;
        private Infragistics.Win.Misc.UltraButton btnUpload;
        private Infragistics.Win.UltraWinEditors.UltraPictureBox pictureBox;
        private Infragistics.Win.Misc.UltraGroupBox gbPictureExpand;
        private Infragistics.Win.Misc.UltraGroupBox gbPicture;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ckCheckd;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPicture;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}
