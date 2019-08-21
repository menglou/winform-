namespace SkyCar.Coeus.Ult.Entrance
{
    partial class FrmProgressBar
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
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            this.ultraProgressBar1 = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.SuspendLayout();
            // 
            // ultraProgressBar1
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.BackColor2 = System.Drawing.Color.Transparent;
            appearance7.BackColorDisabled = System.Drawing.Color.Transparent;
            appearance7.BackColorDisabled2 = System.Drawing.Color.Transparent;
            appearance7.BorderColor = System.Drawing.Color.Transparent;
            appearance7.ForeColor = System.Drawing.Color.DarkCyan;
            this.ultraProgressBar1.Appearance = appearance7;
            this.ultraProgressBar1.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.BackColor2 = System.Drawing.Color.Transparent;
            appearance8.BackColorDisabled = System.Drawing.Color.Transparent;
            appearance8.BackColorDisabled2 = System.Drawing.Color.Transparent;
            this.ultraProgressBar1.FillAppearance = appearance8;
            this.ultraProgressBar1.Location = new System.Drawing.Point(27, 96);
            this.ultraProgressBar1.Name = "ultraProgressBar1";
            this.ultraProgressBar1.Size = new System.Drawing.Size(644, 70);
            this.ultraProgressBar1.TabIndex = 2;
            this.ultraProgressBar1.Text = "[Formatted]";
            // 
            // ultraLabel1
            // 
            appearance9.BackColor = System.Drawing.Color.Transparent;
            appearance9.ForeColor = System.Drawing.Color.Green;
            this.ultraLabel1.Appearance = appearance9;
            this.ultraLabel1.Font = new System.Drawing.Font("微软雅黑", 15F);
            this.ultraLabel1.Location = new System.Drawing.Point(239, 47);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(162, 32);
            this.ultraLabel1.TabIndex = 3;
            this.ultraLabel1.Text = "加载中，请稍候";
            // 
            // FrmProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 188);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.ultraProgressBar1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmProgressBar";
            this.ResizeEnable = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmProgressBar_FormClosing);
            this.Load += new System.EventHandler(this.FrmProgressBar_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.UltraWinProgressBar.UltraProgressBar ultraProgressBar1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
    }
}

