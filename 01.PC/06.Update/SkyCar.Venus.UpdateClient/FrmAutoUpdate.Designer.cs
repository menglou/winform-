namespace SkyCar.Coeus.UpdateClient
{
    partial class FrmAutoUpdate
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
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.progressBarValue = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
            this.txtProgressContent = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblProgress = new Infragistics.Win.Misc.UltraLabel();
            this.lblProgressVaule = new Infragistics.Win.Misc.UltraLabel();
            this.lblSpeed = new Infragistics.Win.Misc.UltraLabel();
            this.lblSpeedValue = new Infragistics.Win.Misc.UltraLabel();
            this.lblPercentage = new Infragistics.Win.Misc.UltraLabel();
            this.lblPercentageValue = new Infragistics.Win.Misc.UltraLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblUpdateLog = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.txtProgressContent)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBarValue
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.progressBarValue, 7);
            this.progressBarValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarValue.Location = new System.Drawing.Point(3, 196);
            this.progressBarValue.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.progressBarValue.Name = "progressBarValue";
            this.progressBarValue.Size = new System.Drawing.Size(578, 30);
            this.progressBarValue.TabIndex = 1;
            this.progressBarValue.Text = "[Formatted]";
            // 
            // txtProgressContent
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtProgressContent, 7);
            this.txtProgressContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProgressContent.Location = new System.Drawing.Point(3, 5);
            this.txtProgressContent.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtProgressContent.Multiline = true;
            this.txtProgressContent.Name = "txtProgressContent";
            this.txtProgressContent.Size = new System.Drawing.Size(578, 181);
            this.txtProgressContent.TabIndex = 2;
            // 
            // lblProgress
            // 
            this.lblProgress.Location = new System.Drawing.Point(3, 236);
            this.lblProgress.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(34, 20);
            this.lblProgress.TabIndex = 3;
            this.lblProgress.Text = "进度";
            // 
            // lblProgressVaule
            // 
            appearance5.ForeColor = System.Drawing.Color.Green;
            this.lblProgressVaule.Appearance = appearance5;
            this.lblProgressVaule.Location = new System.Drawing.Point(43, 236);
            this.lblProgressVaule.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.lblProgressVaule.Name = "lblProgressVaule";
            this.lblProgressVaule.Size = new System.Drawing.Size(113, 20);
            this.lblProgressVaule.TabIndex = 4;
            this.lblProgressVaule.Text = "0.00/0.00M";
            // 
            // lblSpeed
            // 
            this.lblSpeed.Location = new System.Drawing.Point(162, 236);
            this.lblSpeed.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(34, 20);
            this.lblSpeed.TabIndex = 5;
            this.lblSpeed.Text = "速度";
            // 
            // lblSpeedValue
            // 
            appearance6.ForeColor = System.Drawing.Color.Red;
            this.lblSpeedValue.Appearance = appearance6;
            this.lblSpeedValue.Location = new System.Drawing.Point(202, 236);
            this.lblSpeedValue.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.lblSpeedValue.Name = "lblSpeedValue";
            this.lblSpeedValue.Size = new System.Drawing.Size(113, 20);
            this.lblSpeedValue.TabIndex = 6;
            this.lblSpeedValue.Text = "0KB/S";
            // 
            // lblPercentage
            // 
            this.lblPercentage.Location = new System.Drawing.Point(321, 236);
            this.lblPercentage.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.lblPercentage.Name = "lblPercentage";
            this.lblPercentage.Size = new System.Drawing.Size(49, 20);
            this.lblPercentage.TabIndex = 7;
            this.lblPercentage.Text = "百分比";
            // 
            // lblPercentageValue
            // 
            appearance4.ForeColor = System.Drawing.Color.Blue;
            this.lblPercentageValue.Appearance = appearance4;
            this.lblPercentageValue.Location = new System.Drawing.Point(376, 236);
            this.lblPercentageValue.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.lblPercentageValue.Name = "lblPercentageValue";
            this.lblPercentageValue.Size = new System.Drawing.Size(113, 20);
            this.lblPercentageValue.TabIndex = 8;
            this.lblPercentageValue.Text = "0.00%";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tableLayoutPanel1.Controls.Add(this.lblUpdateLog, 6, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblProgress, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtProgressContent, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPercentageValue, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.progressBarValue, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblProgressVaule, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblPercentage, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblSpeed, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblSpeedValue, 3, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 261);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // lblUpdateLog
            // 
            this.lblUpdateLog.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblUpdateLog.Location = new System.Drawing.Point(499, 236);
            this.lblUpdateLog.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.lblUpdateLog.Name = "lblUpdateLog";
            this.lblUpdateLog.Size = new System.Drawing.Size(77, 20);
            this.lblUpdateLog.TabIndex = 9;
            this.lblUpdateLog.Text = "更新日志";
            this.lblUpdateLog.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblUpdateLog_MouseDown);
            // 
            // FrmAutoUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(584, 261);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "FrmAutoUpdate";
            this.Text = "正在为您更新,可能需要几分钟的时间,请耐心等待......";
            this.Load += new System.EventHandler(this.FrmAutoUpdate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtProgressContent)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.UltraWinProgressBar.UltraProgressBar progressBarValue;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtProgressContent;
        private Infragistics.Win.Misc.UltraLabel lblProgress;
        private Infragistics.Win.Misc.UltraLabel lblProgressVaule;
        private Infragistics.Win.Misc.UltraLabel lblSpeed;
        private Infragistics.Win.Misc.UltraLabel lblSpeedValue;
        private Infragistics.Win.Misc.UltraLabel lblPercentage;
        private Infragistics.Win.Misc.UltraLabel lblPercentageValue;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Infragistics.Win.Misc.UltraLabel lblUpdateLog;
    }
}