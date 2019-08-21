namespace SkyCar.Coeus.UI.Common
{
    partial class FrmAutoPartsTypeQuery
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
            Infragistics.Win.UltraWinTree.UltraTreeNode ultraTreeNode1 = new Infragistics.Win.UltraWinTree.UltraTreeNode();
            Infragistics.Win.UltraWinTree.UltraTreeNode ultraTreeNode2 = new Infragistics.Win.UltraWinTree.UltraTreeNode();
            Infragistics.Win.UltraWinTree.UltraTreeNode ultraTreeNode3 = new Infragistics.Win.UltraWinTree.UltraTreeNode();
            Infragistics.Win.UltraWinTree.UltraTreeNode ultraTreeNode4 = new Infragistics.Win.UltraWinTree.UltraTreeNode();
            Infragistics.Win.UltraWinTree.UltraTreeNode ultraTreeNode5 = new Infragistics.Win.UltraWinTree.UltraTreeNode();
            Infragistics.Win.UltraWinTree.UltraTreeNode ultraTreeNode6 = new Infragistics.Win.UltraWinTree.UltraTreeNode();
            Infragistics.Win.UltraWinTree.UltraTreeNode ultraTreeNode7 = new Infragistics.Win.UltraWinTree.UltraTreeNode();
            Infragistics.Win.UltraWinTree.UltraTreeNode ultraTreeNode8 = new Infragistics.Win.UltraWinTree.UltraTreeNode();
            Infragistics.Win.UltraWinTree.UltraTreeNode ultraTreeNode9 = new Infragistics.Win.UltraWinTree.UltraTreeNode();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAutoPartsTypeQuery));
            this.btnCancle = new Infragistics.Win.Misc.UltraButton();
            this.btnConfirm = new Infragistics.Win.Misc.UltraButton();
            this.grpResult = new Infragistics.Win.Misc.UltraGroupBox();
            this.panelGbGrid = new Infragistics.Win.Misc.UltraPanel();
            this.treeAutoPartsType = new Infragistics.Win.UltraWinTree.UltraTree();
            this.txtWhere_APT_Name = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblWhere_APT_Name = new Infragistics.Win.Misc.UltraLabel();
            this.btnClear = new Infragistics.Win.Misc.UltraButton();
            this.btnSearch = new Infragistics.Win.Misc.UltraButton();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.grpCondition = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.grpResult)).BeginInit();
            this.grpResult.SuspendLayout();
            this.panelGbGrid.ClientArea.SuspendLayout();
            this.panelGbGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeAutoPartsType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWhere_APT_Name)).BeginInit();
            this.ultraExpandableGroupBoxPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpCondition)).BeginInit();
            this.grpCondition.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancle
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(157)))), ((int)(((byte)(232)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance1.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance1.ForeColor = System.Drawing.Color.White;
            this.btnCancle.Appearance = appearance1;
            this.btnCancle.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancle.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancle.Location = new System.Drawing.Point(613, 3);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(144, 38);
            this.btnCancle.TabIndex = 1;
            this.btnCancle.Text = "取消(&Q)";
            this.btnCancle.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnConfirm
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(157)))), ((int)(((byte)(232)))));
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Appearance = appearance2;
            this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConfirm.Enabled = false;
            this.btnConfirm.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.Location = new System.Drawing.Point(233, 3);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(144, 38);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "确定(&D)";
            this.btnConfirm.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // grpResult
            // 
            this.grpResult.Controls.Add(this.panelGbGrid);
            this.grpResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpResult.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpResult.Location = new System.Drawing.Point(3, 73);
            this.grpResult.Name = "grpResult";
            this.grpResult.Size = new System.Drawing.Size(990, 445);
            this.grpResult.TabIndex = 1;
            this.grpResult.Text = "查询结果";
            // 
            // panelGbGrid
            // 
            // 
            // panelGbGrid.ClientArea
            // 
            this.panelGbGrid.ClientArea.Controls.Add(this.treeAutoPartsType);
            this.panelGbGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGbGrid.Location = new System.Drawing.Point(3, 23);
            this.panelGbGrid.Name = "panelGbGrid";
            this.panelGbGrid.Size = new System.Drawing.Size(984, 419);
            this.panelGbGrid.TabIndex = 4;
            // 
            // treeAutoPartsType
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BackColor2 = System.Drawing.Color.Transparent;
            this.treeAutoPartsType.Appearance = appearance3;
            this.treeAutoPartsType.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.treeAutoPartsType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeAutoPartsType.ExpansionIndicatorSize = new System.Drawing.Size(20, 20);
            this.treeAutoPartsType.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeAutoPartsType.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.treeAutoPartsType.Location = new System.Drawing.Point(0, 0);
            this.treeAutoPartsType.Name = "treeAutoPartsType";
            this.treeAutoPartsType.NodeConnectorColor = System.Drawing.SystemColors.ControlDark;
            this.treeAutoPartsType.NodeConnectorStyle = Infragistics.Win.UltraWinTree.NodeConnectorStyle.Inset;
            ultraTreeNode1.Text = "Node0";
            ultraTreeNode4.Text = "Node8";
            ultraTreeNode3.Nodes.AddRange(new Infragistics.Win.UltraWinTree.UltraTreeNode[] {
            ultraTreeNode4});
            ultraTreeNode3.Text = "Node4";
            ultraTreeNode2.Nodes.AddRange(new Infragistics.Win.UltraWinTree.UltraTreeNode[] {
            ultraTreeNode3});
            ultraTreeNode2.Text = "Node1";
            ultraTreeNode6.Text = "1312";
            ultraTreeNode5.Nodes.AddRange(new Infragistics.Win.UltraWinTree.UltraTreeNode[] {
            ultraTreeNode6});
            ultraTreeNode5.Text = "Node2";
            ultraTreeNode7.Text = "123";
            ultraTreeNode8.Text = "Node5";
            ultraTreeNode9.Text = "Node6";
            this.treeAutoPartsType.Nodes.AddRange(new Infragistics.Win.UltraWinTree.UltraTreeNode[] {
            ultraTreeNode1,
            ultraTreeNode2,
            ultraTreeNode5,
            ultraTreeNode7,
            ultraTreeNode8,
            ultraTreeNode9});
            this.treeAutoPartsType.Size = new System.Drawing.Size(984, 419);
            this.treeAutoPartsType.TabIndex = 0;
            this.treeAutoPartsType.AfterSelect += new Infragistics.Win.UltraWinTree.AfterNodeSelectEventHandler(this.treeAutoPartsType_AfterSelect);
            this.treeAutoPartsType.DoubleClick += new System.EventHandler(this.treeAutoPartsType_DoubleClick);
            // 
            // txtWhere_APT_Name
            // 
            this.txtWhere_APT_Name.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtWhere_APT_Name.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txtWhere_APT_Name.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtWhere_APT_Name.Location = new System.Drawing.Point(103, 5);
            this.txtWhere_APT_Name.Name = "txtWhere_APT_Name";
            this.txtWhere_APT_Name.Size = new System.Drawing.Size(120, 28);
            this.txtWhere_APT_Name.TabIndex = 0;
            this.txtWhere_APT_Name.ValueChanged += new System.EventHandler(this.txtWhere_APT_Name_ValueChanged);
            this.txtWhere_APT_Name.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtWhere_APT_Name_KeyDown);
            // 
            // lblWhere_APT_Name
            // 
            this.lblWhere_APT_Name.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblWhere_APT_Name.AutoSize = true;
            this.lblWhere_APT_Name.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWhere_APT_Name.Location = new System.Drawing.Point(34, 8);
            this.lblWhere_APT_Name.Name = "lblWhere_APT_Name";
            this.lblWhere_APT_Name.Size = new System.Drawing.Size(63, 21);
            this.lblWhere_APT_Name.TabIndex = 123;
            this.lblWhere_APT_Name.Text = "配件类别";
            // 
            // btnClear
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(157)))), ((int)(((byte)(232)))));
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance4.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance4.ForeColor = System.Drawing.Color.White;
            this.btnClear.Appearance = appearance4;
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClear.Location = new System.Drawing.Point(905, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(76, 32);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "清空(&C)";
            this.btnClear.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSearch
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(157)))), ((int)(((byte)(232)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance5.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance5.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Appearance = appearance5;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSearch.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.Location = new System.Drawing.Point(825, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(74, 32);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "查询(&Q)";
            this.btnSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.tableLayoutPanel3);
            this.ultraExpandableGroupBoxPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(3, 23);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(984, 38);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 8;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableLayoutPanel3.Controls.Add(this.txtWhere_APT_Name, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblWhere_APT_Name, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnClear, 7, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnSearch, 6, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(984, 38);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // grpCondition
            // 
            this.grpCondition.Controls.Add(this.ultraExpandableGroupBoxPanel1);
            this.grpCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCondition.ExpandedSize = new System.Drawing.Size(990, 64);
            this.grpCondition.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpCondition.Location = new System.Drawing.Point(3, 3);
            this.grpCondition.Name = "grpCondition";
            this.grpCondition.Size = new System.Drawing.Size(990, 64);
            this.grpCondition.TabIndex = 0;
            this.grpCondition.Text = "查询条件";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.grpCondition, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grpResult, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 27);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(996, 571);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.btnCancle, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnConfirm, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 524);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(990, 44);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // FrmAutoPartsTypeQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CapitionLogo = global::SkyCar.Coeus.UI.Common.Properties.Resources.SearchWhite;
            this.CaptionFont = new System.Drawing.Font("微软雅黑", 11F);
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmAutoPartsTypeQuery";
            this.Text = "配件类别查询";
            this.Load += new System.EventHandler(this.FrmAutoPartTypeQuery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grpResult)).EndInit();
            this.grpResult.ResumeLayout(false);
            this.panelGbGrid.ClientArea.ResumeLayout(false);
            this.panelGbGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeAutoPartsType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWhere_APT_Name)).EndInit();
            this.ultraExpandableGroupBoxPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpCondition)).EndInit();
            this.grpCondition.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnCancle;
        private Infragistics.Win.Misc.UltraButton btnConfirm;
        private Infragistics.Win.Misc.UltraGroupBox grpResult;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtWhere_APT_Name;
        private Infragistics.Win.Misc.UltraLabel lblWhere_APT_Name;
        private Infragistics.Win.Misc.UltraButton btnClear;
        private Infragistics.Win.Misc.UltraButton btnSearch;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpCondition;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Infragistics.Win.Misc.UltraPanel panelGbGrid;
        private Infragistics.Win.UltraWinTree.UltraTree treeAutoPartsType;
    }
}