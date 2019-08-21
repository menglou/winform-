namespace SkyCar.Coeus.Ult.Entrance
{
    partial class FrmClientUseLicense
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
            this.btnApplyClientUseLicense = new Infragistics.Win.Misc.UltraButton();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.txtCULName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtCULContactNo = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtCULApplyReason = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtCULRemark = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            ((System.ComponentModel.ISupportInitialize)(this.txtCULName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCULContactNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCULApplyReason)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCULRemark)).BeginInit();
            this.SuspendLayout();
            // 
            // btnApplyClientUseLicense
            // 
            this.btnApplyClientUseLicense.Location = new System.Drawing.Point(81, 250);
            this.btnApplyClientUseLicense.Name = "btnApplyClientUseLicense";
            this.btnApplyClientUseLicense.Size = new System.Drawing.Size(432, 43);
            this.btnApplyClientUseLicense.TabIndex = 0;
            this.btnApplyClientUseLicense.Text = "提交申请";
            this.btnApplyClientUseLicense.Click += new System.EventHandler(this.btnApplyClientUseLicense_Click);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Location = new System.Drawing.Point(60, 23);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel1.TabIndex = 1;
            this.ultraLabel1.Text = "用户姓名";
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Location = new System.Drawing.Point(294, 23);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel2.TabIndex = 2;
            this.ultraLabel2.Text = "联系方式";
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Location = new System.Drawing.Point(38, 146);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel3.TabIndex = 3;
            this.ultraLabel3.Text = "审核失败原因";
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.Location = new System.Drawing.Point(60, 77);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel4.TabIndex = 4;
            this.ultraLabel4.Text = "申请原因";
            // 
            // txtCULName
            // 
            this.txtCULName.Location = new System.Drawing.Point(124, 19);
            this.txtCULName.Name = "txtCULName";
            this.txtCULName.Size = new System.Drawing.Size(143, 21);
            this.txtCULName.TabIndex = 5;
            // 
            // txtCULContactNo
            // 
            this.txtCULContactNo.Location = new System.Drawing.Point(352, 19);
            this.txtCULContactNo.Name = "txtCULContactNo";
            this.txtCULContactNo.Size = new System.Drawing.Size(161, 21);
            this.txtCULContactNo.TabIndex = 6;
            // 
            // txtCULApplyReason
            // 
            this.txtCULApplyReason.Location = new System.Drawing.Point(124, 73);
            this.txtCULApplyReason.Multiline = true;
            this.txtCULApplyReason.Name = "txtCULApplyReason";
            this.txtCULApplyReason.Size = new System.Drawing.Size(389, 63);
            this.txtCULApplyReason.TabIndex = 7;
            // 
            // txtCULRemark
            // 
            this.txtCULRemark.Location = new System.Drawing.Point(124, 142);
            this.txtCULRemark.Multiline = true;
            this.txtCULRemark.Name = "txtCULRemark";
            this.txtCULRemark.ReadOnly = true;
            this.txtCULRemark.Size = new System.Drawing.Size(389, 87);
            this.txtCULRemark.TabIndex = 8;
            // 
            // FrmClientUseLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 315);
            this.Controls.Add(this.txtCULRemark);
            this.Controls.Add(this.txtCULApplyReason);
            this.Controls.Add(this.txtCULContactNo);
            this.Controls.Add(this.txtCULName);
            this.Controls.Add(this.ultraLabel4);
            this.Controls.Add(this.ultraLabel3);
            this.Controls.Add(this.ultraLabel2);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.btnApplyClientUseLicense);
            this.MaximizeBox = false;
            this.Name = "FrmClientUseLicense";
            this.Text = "申请使用许可证";
            ((System.ComponentModel.ISupportInitialize)(this.txtCULName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCULContactNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCULApplyReason)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCULRemark)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnApplyClientUseLicense;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtCULName;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtCULContactNo;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtCULApplyReason;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtCULRemark;
    }
}