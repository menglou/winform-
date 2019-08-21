namespace SkyCar.Coeus.UI.SD
{
    partial class FrmViewAndPrintLogisticsBill
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmViewAndPrintLogisticsBill));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.LogisticsBillUIModelToPrintBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.LogisticsBillDetailUIModelToPrintBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogisticsBillUIModelToPrintBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogisticsBillDetailUIModelToPrintBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.reportViewer1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 27);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1020, 739);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "LogisticsBill";
            reportDataSource1.Value = this.LogisticsBillUIModelToPrintBindingSource;
            reportDataSource2.Name = "LogisticsBillDetail";
            reportDataSource2.Value = this.LogisticsBillDetailUIModelToPrintBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "SkyCar.Coeus.UI.SD.RdlcFiles.SD_ViewAndPrintLogisticsBill.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(3, 3);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(1014, 733);
            this.reportViewer1.TabIndex = 0;
            // 
            // LogisticsBillUIModelToPrintBindingSource
            // 
            this.LogisticsBillUIModelToPrintBindingSource.DataSource = typeof(SkyCar.Coeus.UIModel.SD.LogisticsBillUIModelToPrint);
            // 
            // LogisticsBillDetailUIModelToPrintBindingSource
            // 
            this.LogisticsBillDetailUIModelToPrintBindingSource.DataSource = typeof(SkyCar.Coeus.UIModel.SD.LogisticsBillDetailUIModelToPrint);
            // 
            // FrmViewAndPrintLogisticsBill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CapitionLogo = ((System.Drawing.Image)(resources.GetObject("$this.CapitionLogo")));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FrmViewAndPrintLogisticsBill";
            this.Text = "打印物流单据";
            this.Load += new System.EventHandler(this.FrmViewAndPrintLogisticsBill_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LogisticsBillUIModelToPrintBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogisticsBillDetailUIModelToPrintBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource LogisticsBillUIModelToPrintBindingSource;
        private System.Windows.Forms.BindingSource LogisticsBillDetailUIModelToPrintBindingSource;
    }
}