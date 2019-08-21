﻿namespace SkyCar.Coeus.UI.PIS
{
    partial class FrmViewAndPrintTransferBill
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmViewAndPrintTransferBill));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.TransferBillUIModelToPrintBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TransferBillDetailUIModelToPrintBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TransferBillUIModelToPrintBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TransferBillDetailUIModelToPrintBindingSource)).BeginInit();
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
            reportDataSource1.Name = "TransferBill";
            reportDataSource1.Value = this.TransferBillUIModelToPrintBindingSource;
            reportDataSource2.Name = "TransferBillDetail";
            reportDataSource2.Value = this.TransferBillDetailUIModelToPrintBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "SkyCar.Coeus.UI.PIS.RdlcFiles.PIS_ViewAndPrintTransferBill.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(3, 3);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(1014, 733);
            this.reportViewer1.TabIndex = 0;
            // 
            // TransferBillUIModelToPrintBindingSource
            // 
            this.TransferBillUIModelToPrintBindingSource.DataSource = typeof(SkyCar.Coeus.UIModel.PIS.TransferBillUIModelToPrint);
            // 
            // TransferBillDetailUIModelToPrintBindingSource
            // 
            this.TransferBillDetailUIModelToPrintBindingSource.DataSource = typeof(SkyCar.Coeus.UIModel.PIS.TransferBillDetailUIModelToPrint);
            // 
            // FrmViewAndPrintTransferBill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CapitionLogo = ((System.Drawing.Image)(resources.GetObject("$this.CapitionLogo")));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FrmViewAndPrintTransferBill";
            this.Text = "打印调拨单据";
            this.Load += new System.EventHandler(this.FrmViewAndPrintTransferBill_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TransferBillUIModelToPrintBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TransferBillDetailUIModelToPrintBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource TransferBillUIModelToPrintBindingSource;
        private System.Windows.Forms.BindingSource TransferBillDetailUIModelToPrintBindingSource;
    }
}