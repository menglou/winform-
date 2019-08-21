using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using SkyCar.Coeus.Common.Const;
using ToolTip = System.Windows.Forms.ToolTip;

namespace SkyCar.Coeus.Common.CustomControl
{
    /// <summary>
    /// 扩展下拉框
    /// </summary>
    public class SkyCarMultiComboBox : ComboBox
    {
        #region 成员变量
        private const int WM_LBUTTONDOWN = 0x201, WM_LBUTTONDBLCLK = 0x203;

        ToolStripControlHost _dataGridViewHost;

        ToolStripControlHost _textBoxHost;

        ToolStripDropDown _dropDownBox;

        EditorButton _clearTextButton = new EditorButton();

        private string pleaseSelectText = "--请选择--";

        /// <summary>
        /// 是否选中字段名
        /// </summary>
        private string checkColumnName = "IsChecked";
        /// <summary>
        /// 显示内容字段名
        /// </summary>
        private string displayColumnName = "ItemDesc";
        /// <summary>
        /// 额外显示内容字段名
        /// </summary>
        private string extraDisplayColumnName = "ExtraItemDesc";
        /// <summary>
        /// 显示内容长度字段名
        /// </summary>
        private string ItemDescLenColumnName = "ItemDescLen";
        /// <summary>
        /// 显示内容全部拼音字段名
        /// </summary>
        private string itemDescAllSpellCodeColumnName = "ItemDescAllSpellCode";
        /// <summary>
        /// 显示内容全部拼音首字母字段名
        /// </summary>
        private string ItemDescAllFirstSpellCodeColumnName = "ItemDescAllFirstSpellCode";
        /// <summary>
        /// 选项值字段名
        /// </summary>
        private string valueColumnName = "ItemValue";
        /// <summary>
        /// 是否匹配字段名
        /// </summary>
        private string isMatchedColumnName = "IsMatched";
        /// <summary>
        /// 匹配值字段名
        /// </summary>
        private string matchIndexColumnName = "MatchIndex";
        /// <summary>
        /// 全选值
        /// </summary>
        private string selectAllCellValue = "AllSelected";
        /// <summary>
        /// 空行值
        /// </summary>
        private string blankCellValue = Guid.NewGuid().ToString().ToUpper();
        /// <summary>
        /// 全选时显示文本
        /// </summary>
        private string selectAllCellText = "全选";
        /// <summary>
        /// 多选时显示文本
        /// </summary>
        private string multipleSelectText = "多选";
        /// <summary>
        /// 数据源Table
        /// </summary>
        private DataTable dtOfDataSource = null;


        private string m_sKeyWords = string.Empty;
        private string m_sDisplayMember = string.Empty;

        private bool m_blDropShow = false;

        public event ComBoxAfterSelectorEventHandler AfterSelector;

        /// <summary>
        /// 选中项索引变化
        /// </summary>
        [Description("选中项索引变化"), Browsable(true), Category("N8")]
        public new event EventHandler SelectedIndexChanged;

        /// <summary>
        /// 失去焦点
        /// </summary>
        [Description("失去焦点"), Browsable(true), Category("N8")]
        public event EventHandler ComboBoxLostFocus;

        /// <summary>
        /// 数据源Model类型
        /// </summary>
        private Type ItemSourceModelType { get; set; }
        /// <summary>
        /// 存在额外信息列
        /// </summary>
        private bool _existsExtraItemDesc;
        #endregion

        #region 构造方法
        public SkyCarMultiComboBox()
        {
            this.LostFocus += SkyCarComboBoxLostFocus;
            this.MouseEnter += SkyCarMultiComboBox_MouseEnter;
            DrawSkyCarComboBox();
        }

        #endregion

        #region 属性
        private bool _isReadOnly;
        /// <summary>
        /// 是否只读
        /// </summary>
        [Description("是否只读"), Browsable(true), Category("N8"), DefaultValue(false)]
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set
            {
                _isReadOnly = value;
                this.BackColor = _isReadOnly ? Color.LightGray : Color.White;
                if (TextBoxOfFilter != null)
                {
                    TextBoxOfFilter.ReadOnly = this.IsReadOnly;
                    TextBoxOfFilter.BackColor = _isReadOnly ? Color.LightGray : Color.White;
                }
            }
        }

        /// <summary>
        /// 是否显示数据源的列名
        /// </summary>
        [Description("是否显示数据源的列名"), Browsable(true), Category("N8"), DefaultValue(false)]
        public bool ShowColumnsHeader { set; get; } = false;
        /// <summary>
        /// 支持多选,默认不支持
        /// </summary>
        [Description("支持多选"), Browsable(true), Category("N8"), DefaultValue(false)]
        public bool SupportMultipleSelect { get; set; } = false;
        /// <summary>
        /// 接受输入值为选中值
        /// </summary>
        [Description("接受输入值为选中值"), Browsable(true), Category("N8"), DefaultValue(false)]
        public bool AcceptInputValue { get; set; } = false;
        /// <summary>
        /// 分隔字符，默认为 ;
        /// </summary>
        [Description("分隔字符，默认为 ;"), Browsable(true), Category("N8"), DefaultValue(';')]
        public char SplitChar { get; set; } = ';';

        private StringBuilder _selectedValue = new StringBuilder(5000);
        /// <summary>
        /// 选中值串，以SplitChar分隔
        /// </summary>
        [Description("选中值串，以SplitChar分隔"), Browsable(true), Category("N8")]
        public new string SelectedValue
        {
            get
            {
                return AcceptInputValue && _selectedValue.ToString().Trim().Length.Equals(0) ?
                    (this.TextBoxOfFilter.Text.Replace(pleaseSelectText, string.Empty).Trim().Length > 0 ? (SupportMultipleSelect ? SplitChar.ToString() : string.Empty) + this.TextBoxOfFilter.Text.Replace(pleaseSelectText, string.Empty).Trim() + (SupportMultipleSelect ? SplitChar.ToString() : string.Empty) : string.Empty)
                : ((SupportMultipleSelect && _selectedValue.ToString().Trim().Length > 0 ? "" : string.Empty) + _selectedValue.ToString());
            }
            set
            {
                this._selectedValue = new StringBuilder(value);
                this._selectedText = new StringBuilder();
                if (dtOfDataSource != null)
                {
                    SelectedTextExtra = String.Empty;
                    if (!SupportMultipleSelect)
                    {
                        DataRow[] drList = dtOfDataSource.Select(valueColumnName + "='" + this._selectedValue + "'");
                        if (drList.Length.Equals(1))
                        {
                            lastTextChangedValue = drList[0][displayColumnName].ToString();
                            this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                            this._selectedText = new StringBuilder(drList[0][displayColumnName].ToString());
                            if (_existsExtraItemDesc)
                            {
                                foreach (DataGridViewRow loopGridViewRow in DataGridViewOfDropdown.Rows)
                                {
                                    if ((string)loopGridViewRow.Cells[valueColumnName].Value == this._selectedValue.ToString())
                                    {
                                        SelectedTextExtra = (string)loopGridViewRow.Cells[extraDisplayColumnName].Value;
                                        break;
                                    }
                                }
                            }
                            if (SelectedIndexChanged != null)
                            {
                                SelectedIndexChanged(this, new EventArgs());
                            }
                        }
                        else
                        {
                            lastTextChangedValue = !AcceptInputValue ? string.Empty : this._selectedValue.ToString();
                            this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                        }
                    }
                    else
                    {
                        string[] selectedValueList = this._selectedValue.ToString().Split(SplitChar);
                        var tempDataSource = dtOfDataSource.AsEnumerable().ToList().Where(x => selectedValueList.Contains(x[valueColumnName].ToString()) && x[valueColumnName].ToString() != blankCellValue).Select(x => x).ToList();
                        tempDataSource.ForEach(x => x[checkColumnName] = true);
                        tempDataSource.ForEach(x => this._selectedText.Append(x[displayColumnName].ToString() + SplitChar.ToString()));
                        SetDisplayText();

                        if (tempDataSource.Count > 0)
                        {
                            int noSelectedNum = this.GetDataTableFromDataSource(DataGridViewOfDropdown.DataSource).Select(checkColumnName + "= 0"
                                            + " AND " + valueColumnName + "<>'" + selectAllCellValue
                                            + "' AND " + valueColumnName + "<>'" + blankCellValue
                                            + "'").Length;
                            DataRow[] drSelectAll = dtOfDataSource.Select(valueColumnName + "='" + selectAllCellValue + "'");
                            if (noSelectedNum == 0 && drSelectAll.Length.Equals(1))
                            {
                                drSelectAll[0][checkColumnName] = noSelectedNum == 0;
                                if (!lastTextChangedValue.Equals(selectAllCellText))
                                {
                                    changeTextByManualy = false;
                                    lastTextChangedValue = selectAllCellText;
                                    this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                                    if (SelectedIndexChanged != null)
                                    {
                                        SelectedIndexChanged(this, new EventArgs());
                                    }
                                }
                            }
                            else
                            {
                                if (tempDataSource.Count > 1)
                                {
                                    tempDataSource.ForEach(x => this._selectedText.Append(x[displayColumnName].ToString() + SplitChar.ToString()));
                                    lastTextChangedValue = multipleSelectText;
                                    this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                                }
                                else
                                {
                                    lastTextChangedValue = tempDataSource[0][displayColumnName].ToString();
                                    this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                                }
                                if (SelectedIndexChanged != null)
                                {
                                    SelectedIndexChanged(this, new EventArgs());
                                }
                            }
                            if (this._selectedValue.ToString().Length > 0)
                            {
                                this._selectedValue = new StringBuilder(5000);
                            }
                            if (this._selectedText.ToString().Length > 0)
                            {
                                this._selectedText = new StringBuilder(5000);
                            }
                            if (tempDataSource.Count == 1)
                            {
                                tempDataSource.ForEach(x => this._selectedValue.Append(x[valueColumnName].ToString()));
                                tempDataSource.ForEach(x => this._selectedText.Append(x[displayColumnName].ToString()));
                            }
                            else
                            {
                                tempDataSource.ForEach(x => this._selectedValue.Append(x[valueColumnName].ToString() + SplitChar.ToString()));
                                tempDataSource.ForEach(x => this._selectedText.Append(x[displayColumnName].ToString() + SplitChar.ToString()));
                            }

                        }
                        else
                        {
                            lastTextChangedValue = string.Empty;
                            this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                            this._selectedText = new StringBuilder();
                        }
                    }

                }
            }
        }

        private StringBuilder _selectedText = new StringBuilder(5000);
        /// <summary>
        /// 选中文本串，以SplitChar分隔
        /// </summary>
        [Description("选中文本串，以SplitChar分隔"), Browsable(true), Category("N8")]
        public new string SelectedText
        {
            get
            {
                return AcceptInputValue && _selectedText.ToString().Trim().Length.Equals(0) ?
                    (this.TextBoxOfFilter.Text.Replace(pleaseSelectText, string.Empty).Trim().Length > 0 ? SplitChar + this.TextBoxOfFilter.Text.Replace(pleaseSelectText, string.Empty).Trim() + SplitChar : string.Empty)
                    : ((SupportMultipleSelect && _selectedText.ToString().Trim().Length > 0 ? "" : string.Empty) + _selectedText);
            }
            set
            {
                this._selectedText = new StringBuilder(value);
                if (dtOfDataSource != null)
                {
                    if (!SupportMultipleSelect)
                    {
                        DataRow[] drList = null;
                        if (!_existsExtraItemDesc || string.IsNullOrEmpty(SelectedTextExtra))
                        {
                            drList = dtOfDataSource.Select(displayColumnName + "='" + this._selectedText + "'");
                        }
                        else
                        {
                            drList = dtOfDataSource.Select(displayColumnName + "='" + this._selectedText + "' AND " + extraDisplayColumnName + "='" + SelectedTextExtra + "'");
                        }

                        if (drList.Length.Equals(1))
                        {
                            lastTextChangedValue = drList[0][displayColumnName].ToString();
                            this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                            this._selectedValue = new StringBuilder(drList[0][valueColumnName].ToString());

                            if (_existsExtraItemDesc)
                            {
                                foreach (DataGridViewRow loopGridViewRow in DataGridViewOfDropdown.Rows)
                                {
                                    if (string.IsNullOrEmpty(SelectedTextExtra))
                                    {
                                        if ((string)loopGridViewRow.Cells[displayColumnName].Value == this._selectedText.ToString())
                                        {
                                            SelectedTextExtra = (string)loopGridViewRow.Cells[extraDisplayColumnName].Value;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (loopGridViewRow.Cells[displayColumnName].Value == this._selectedText &&
                                            (string)loopGridViewRow.Cells[extraDisplayColumnName].Value == this._selectedTextExtra)
                                        {
                                            SelectedTextExtra = (string)loopGridViewRow.Cells[extraDisplayColumnName].Value;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            SelectedTextExtra = string.Empty;
                            lastTextChangedValue = !AcceptInputValue ? string.Empty : this._selectedText.ToString();
                            this._selectedValue = new StringBuilder(lastTextChangedValue);
                            this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                        }
                    }
                }

                if (string.IsNullOrEmpty(this._selectedText.ToString()))
                {
                    SelectedTextExtra = string.Empty;
                }
            }
        }

        private string _selectedTextExtra = string.Empty;
        /// <summary>
        /// 额外文本，用以区分相同的显示文本
        /// </summary>
        [Description("额外文本，用以区分相同的显示文本"), Browsable(true), Category("N8")]
        public string SelectedTextExtra
        {
            get
            {
                return _selectedTextExtra;
            }
            set
            {
                _selectedTextExtra = value;
            }
        }

        /// <summary>
        /// 选项值的属性名
        /// </summary>
        [Description("选项值的属性名"), Browsable(true), Category("N8")]
        public new string ValueMember { get; set; } = string.Empty;
        /// <summary>
        /// 选项文本的属性名
        /// </summary>
        [Description("选项文本的属性名"), Browsable(true), Category("N8")]
        public new string DisplayMember { get; set; } = string.Empty;

        [Description("额外的选项文本的字段名"), Browsable(true), Category("N8")]
        public string ExtraDisplayMember { get; set; } = string.Empty;

        [Description("是否显示文本框!"), Browsable(true), Category("N8")]
        public bool RowFilterVisible { set; get; } = true;

        public DataView DataViewOfDropdown
        {
            get
            {
                DataTable dataTable = GetDataTableFromDataSource();
                if (dataTable == null)
                {
                    return null;
                }
                return dataTable.DefaultView;
            }
        }
        [Description("设置DataGridView属性"), Browsable(true), Category("N8")]
        public DataGridView DataGridViewOfDropdown
        {
            get
            {
                return _dataGridViewHost.Control as DataGridView;
            }
        }
        public UltraTextEditor TextBoxOfFilter
        {
            get
            {
                return _textBoxHost != null ? _textBoxHost.Control as UltraTextEditor : null;
            }
        }
        [Description("数据源"), Browsable(true), Category("N8")]
        public new Object DataSource
        {
            set
            {
                if (this.DataGridViewOfDropdown == null)
                {
                    return;
                }
                DataTable newSource = GetDataTableFromDataSource(value);
                if (newSource != null)
                {
                    dtOfDataSource = newSource.Copy();
                    if (_dataGridViewHost != null && DataGridViewOfDropdown != null)
                    {
                        DataGridViewOfDropdown.Dispose();
                    }
                    if (_textBoxHost != null && TextBoxOfFilter != null)
                    {
                        TextBoxOfFilter.Dispose();
                    }
                    _selectedValue = new StringBuilder(5000);
                    _selectedText = new StringBuilder(5000);
                    DrawSkyCarComboBox();
                    if (this.DataGridViewOfDropdown.Columns.Count.Equals(0))
                    {
                        int chkColumnWidth = 0;
                        if (this.SupportMultipleSelect && dtOfDataSource != null)
                        {
                            if (!dtOfDataSource.Columns.Contains(checkColumnName))
                            {
                                DataColumn checkColumn = new DataColumn(checkColumnName, Type.GetType("System.Boolean"));
                                checkColumn.DefaultValue = false;
                                dtOfDataSource.Columns.Add(checkColumn);
                            }
                            if (dtOfDataSource.Select(valueColumnName + "='" + this.selectAllCellValue + "'").Length.Equals(0))
                            {
                                DataRow drSelectAll = dtOfDataSource.NewRow();
                                drSelectAll[checkColumnName] = false;
                                drSelectAll[valueColumnName] = selectAllCellValue;
                                drSelectAll[displayColumnName] = "全选";
                                drSelectAll[isMatchedColumnName] = true;
                                drSelectAll[matchIndexColumnName] = -1;
                                drSelectAll[itemDescAllSpellCodeColumnName] = string.Empty;
                                drSelectAll[ItemDescAllFirstSpellCodeColumnName] = string.Empty;
                                drSelectAll[ItemDescLenColumnName] = 0;

                                dtOfDataSource.Rows.InsertAt(drSelectAll, 0);
                            }
                            if (dtOfDataSource.Select(valueColumnName + "='" + this.blankCellValue + "'").Length.Equals(0))
                            {
                                DataRow drBlank = dtOfDataSource.NewRow();
                                drBlank[checkColumnName] = false;
                                drBlank[valueColumnName] = blankCellValue;
                                drBlank[displayColumnName] = string.Empty;
                                drBlank[isMatchedColumnName] = true;
                                drBlank[matchIndexColumnName] = 10000000000;
                                drBlank[itemDescAllSpellCodeColumnName] = string.Empty;
                                drBlank[ItemDescAllFirstSpellCodeColumnName] = string.Empty;
                                drBlank[ItemDescLenColumnName] = 0;

                                dtOfDataSource.Rows.Add(drBlank);
                            }

                            chkColumnWidth = ShowColumnsHeader ? 40 : 30;
                            this.DataGridViewOfDropdown.ReadOnly = false;
                            DataGridViewCheckBoxColumn checkCol = new DataGridViewCheckBoxColumn();
                            checkCol.Name = checkColumnName;
                            checkCol.DataPropertyName = checkColumnName;
                            checkCol.Width = chkColumnWidth;
                            checkCol.HeaderText = "选择";
                            checkCol.Visible = true;
                            checkCol.ReadOnly = false;
                            this.DataGridViewOfDropdown.Columns.Add(checkCol);
                        }

                        if (!this.DataGridViewOfDropdown.Columns.Contains(valueColumnName))
                        {
                            DataGridViewTextBoxColumn valueCol = new DataGridViewTextBoxColumn();
                            valueCol.Name = valueColumnName;
                            valueCol.HeaderText = ValueMember;
                            valueCol.Visible = false;
                            valueCol.DataPropertyName = valueColumnName;
                            this.DataGridViewOfDropdown.Columns.Add(valueCol);
                        }
                        if (_existsExtraItemDesc && !this.DataGridViewOfDropdown.Columns.Contains(extraDisplayColumnName))
                        {
                            DataGridViewTextBoxColumn extraDisplayCol = new DataGridViewTextBoxColumn();
                            extraDisplayCol.Name = extraDisplayColumnName;
                            extraDisplayCol.HeaderText = ExtraDisplayMember;
                            extraDisplayCol.Visible = true;
                            extraDisplayCol.ReadOnly = true;
                            extraDisplayCol.DataPropertyName = extraDisplayColumnName;

                            //获取数据源中内容最长的数据
                            string maxContant = string.Empty;
                            foreach (DataRow loopDataSource in dtOfDataSource.Rows)
                            {
                                if (loopDataSource[extraDisplayColumnName] == null)
                                {
                                    continue;
                                }
                                string loopContent = loopDataSource[extraDisplayColumnName].ToString();
                                if (loopContent.Length > maxContant.Length)
                                {
                                    maxContant = loopContent;
                                }
                            }

                            Graphics gridViewGraphics = DataGridViewOfDropdown.CreateGraphics();
                            DataGridViewOfDropdown.Font = new Font("微软雅黑", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                            SizeF z = gridViewGraphics.MeasureString(maxContant, DataGridViewOfDropdown.Font);
                            extraDisplayCol.Width = (int)z.Width;

                            this.DataGridViewOfDropdown.Columns.Add(extraDisplayCol);
                        }
                        if (!this.DataGridViewOfDropdown.Columns.Contains(displayColumnName))
                        {
                            DataGridViewTextBoxColumn displayCol = new DataGridViewTextBoxColumn();
                            displayCol.Name = displayColumnName;
                            displayCol.HeaderText = DisplayMember;
                            displayCol.Visible = true;
                            displayCol.ReadOnly = true;
                            displayCol.DataPropertyName = displayColumnName;

                            //获取数据源中内容最长的数据
                            string maxContant = string.Empty;
                            foreach (DataRow loopDataSource in dtOfDataSource.Rows)
                            {
                                if (loopDataSource[displayColumnName] == null)
                                {
                                    continue;
                                }
                                string loopContent = loopDataSource[displayColumnName].ToString();
                                if (loopContent.Length > maxContant.Length)
                                {
                                    maxContant = loopContent;
                                }
                            }

                            Graphics gridViewGraphics = DataGridViewOfDropdown.CreateGraphics();
                            DataGridViewOfDropdown.Font = new Font("微软雅黑", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                            SizeF z = gridViewGraphics.MeasureString(maxContant, DataGridViewOfDropdown.Font);
                            displayCol.Width = (int)z.Width;

                            this.DataGridViewOfDropdown.Columns.Add(displayCol);
                        }
                    }
                    this.DataGridViewOfDropdown.AllowUserToAddRows = false;
                    this.DataGridViewOfDropdown.AutoGenerateColumns = false;
                    DataGridViewOfDropdown.DataSource = dtOfDataSource;
                }
                else
                {
                    DataGridViewOfDropdown.DataSource = null;
                }
            }
            get
            {
                return DataGridViewOfDropdown != null ? DataGridViewOfDropdown.DataSource : null;
            }
        }

        [Description("下拉表格尺寸是否为自动"), Browsable(true), Category("N8")]
        public bool PopupGridAutoSize { set; get; } = false;

        private void SetDisplayText()
        {
            if (_selectedText.ToString().Length > 200)
            {
                multipleSelectText = _selectedText.ToString().Substring(0, 200) + "......";
                selectAllCellText = _selectedText.ToString().Substring(0, 200) + "......";
            }
            else
            {
                multipleSelectText = _selectedText.ToString();
                selectAllCellText = _selectedText.ToString();
            }
        }
        #endregion

        #region 方法
        #region 绘制文本框以及下拉DataGridView
        private void DrawSkyCarComboBox()
        {
            int width = this.Size.Width;
            DataGridView dataGridView = new DataGridView();
            dataGridView.Padding = new Padding(0, 0, 0, 0);
            dataGridView.Margin = new Padding(0, 0, 0, 0);
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.ScrollBars = ScrollBars.Both;
            dataGridView.AutoSize = true;
            dataGridView.BackgroundColor = Color.LightBlue;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.ReadOnly = true;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.ColumnHeadersVisible = ShowColumnsHeader;
            dataGridView.RowHeadersVisible = false;
            dataGridView.MaximumSize = new Size(400, 320);
            dataGridView.RowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridView.RowsDefaultCellStyle.Font = new Font("微软雅黑", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.KeyDown += new KeyEventHandler(dataGridView_KeyDown);
            dataGridView.CellClick += new DataGridViewCellEventHandler(dataGridView_CellClick);
            dataGridView.CellMouseMove += new DataGridViewCellMouseEventHandler(dataGridView_CellMouseMove);
            dataGridView.CellMouseLeave += new DataGridViewCellEventHandler(dataGridView_CellMouseLeave);

            Form frmDataSource = new Form();
            frmDataSource.Padding = new Padding(0, 0, 0, 0);
            frmDataSource.Margin = new Padding(0, 0, 0, 0);
            frmDataSource.FormBorderStyle = FormBorderStyle.None;
            frmDataSource.Controls.Add(dataGridView);
            dataGridView.Dock = DockStyle.Fill;
            frmDataSource.AutoSize = false;
            frmDataSource.Width = width;
            frmDataSource.Height = 300;
            frmDataSource.SuspendLayout();
            _dataGridViewHost = new ToolStripControlHost(dataGridView);
            _dataGridViewHost.Padding = new Padding(0, 0, 0, 0);
            _dataGridViewHost.Margin = new Padding(0, 0, 0, 0);
            _dataGridViewHost.AutoSize = false;
            _dataGridViewHost.BackColor = Color.SteelBlue;

            UltraTextEditor textBox = new UltraTextEditor();
            textBox.Padding = new Padding(0, 0, 0, 0);
            textBox.Margin = new Padding(0, 0, 0, 0);
            this.Text = textBox.Text = pleaseSelectText;

            textBox.ReadOnly = this.IsReadOnly;
            if (textBox.ReadOnly)
            {
                this.BackColor = textBox.BackColor = Color.LightGray;
                textBox.Visible = false;
            }
            else
            {
                this.BackColor = textBox.BackColor = Color.White;
                this.Text = textBox.Text = string.Empty;
            }

            textBox.Width = width;
            textBox.Height = this.Height;
            textBox.ForeColor = Color.Black;
            textBox.Font = new Font("微软雅黑", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            textBox.Appearance.TextHAlign = HAlign.Left;
            textBox.TextChanged += new EventHandler(textBox_TextChanged);
            textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
            if (this.SupportMultipleSelect)
            {
                textBox.MouseEnter += new EventHandler(textBox_MouseEnter);
            }
            textBox.MouseUp += new MouseEventHandler(textBox_MouseUp);
            textBox.MouseDown += new MouseEventHandler(textBox_MouseDown);
            textBox.MouseDoubleClick += new MouseEventHandler(textBox_MouseDoubleClick);
            textBox.Click += TextBox_Click;
            textBox.GotFocus += TextBox_GotFocus;

            _clearTextButton = new EditorButton();
            Infragistics.Win.Appearance clearTextButtonAppearance = new Infragistics.Win.Appearance();
            clearTextButtonAppearance.Image = Properties.Resources.Clear;
            _clearTextButton.Appearance = clearTextButtonAppearance;
            _clearTextButton.Appearance.BackColor = Color.Transparent;
            _clearTextButton.ButtonStyle = UIElementButtonStyle.Office2010Button;
            textBox.ButtonsRight.Add(_clearTextButton);
            textBox.EditorButtonClick += TextBox_EditorButtonClick;

            _textBoxHost = new ToolStripControlHost(textBox);
            _textBoxHost.Padding = new Padding(0, 0, 0, 0);
            _textBoxHost.Margin = new Padding(0, 0, 0, 0);
            _textBoxHost.AutoSize = false;

            _dropDownBox = new ToolStripDropDown();
            _dropDownBox.Padding = new Padding(0, 0, 0, 0);
            _dropDownBox.Margin = new Padding(0, 0, 0, 0);
            _dropDownBox.BackColor = Color.LightBlue;
            _dropDownBox.AutoSize = true;
            _dropDownBox.Height = 330;
            _dropDownBox.Closing += _dropDownBox_Closing;
            _dropDownBox.Items.Add(_textBoxHost);
            _dropDownBox.Items.Add(_dataGridViewHost);

            //this.GotFocus += new EventHandler(TextBoxOfFilter_GotFocus);
            this.LostFocus += new EventHandler(TextBoxOfFilter_LostFocus);
            //this.TextBoxOfFilter.GotFocus += new EventHandler(TextBoxOfFilter_GotFocus);
            //this.TextBoxOfFilter.LostFocus += new EventHandler(TextBoxOfFilter_LostFocus);
        }

        private void _dropDownBox_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            string filterContent = this.TextBoxOfFilter.Text.Trim();

            //非用户筛选内容不处理
            if (filterContent == pleaseSelectText
                || filterContent == multipleSelectText
                || filterContent == selectAllCellText)
            {
                return;
            }
            bool isExist = false;
            foreach (DataRow loopSourceRow in dtOfDataSource.Rows)
            {
                string itemDesc = loopSourceRow[displayColumnName]?.ToString() ?? "";
                if (string.IsNullOrEmpty(itemDesc))
                {
                    continue;
                }
                if ((SupportMultipleSelect == false && itemDesc.Equals(filterContent))
                    || (SupportMultipleSelect == true && (itemDesc + SysConst.Semicolon_DBC).Equals(filterContent)))
                {
                    _selectedValue = new StringBuilder(((ComboBoxDataSet.ComboBoxInfoRow)loopSourceRow).ItemValue);
                    _selectedText = new StringBuilder(((ComboBoxDataSet.ComboBoxInfoRow)loopSourceRow).ItemDesc);

                    changeTextByManualy = false;
                    lastTextChangedValue = _selectedText.ToString();
                    this.Text = lastTextChangedValue;
                    if (this.TextBoxOfFilter != null)
                    {
                        this.TextBoxOfFilter.Text = lastTextChangedValue;
                    }
                    if (this.SelectedValue.Length == 0)
                    {
                        lastTextChangedValue = pleaseSelectText;
                        this.Text = lastTextChangedValue;
                        if (this.TextBoxOfFilter != null)
                        {
                            this.TextBoxOfFilter.Text = lastTextChangedValue;
                        }
                    }
                    isExist = true;
                    break;
                }
            }
            if (!isExist)
            {
                if (!AcceptInputValue)
                {
                    TextBoxOfFilter.Text = null;
                }
                else
                {
                    _selectedValue = new StringBuilder(TextBoxOfFilter.Text);
                    _selectedText = new StringBuilder(TextBoxOfFilter.Text);
                    if (SelectedIndexChanged != null)
                    {
                        SelectedIndexChanged(this, new EventArgs());
                    }
                }
            }
        }

        private void TextBox_GotFocus(object sender, EventArgs e)
        {
            UltraTextEditor tempTextBox = sender as UltraTextEditor;
            if (tempTextBox != null)
            {
                tempTextBox.SelectionStart = tempTextBox.Text.Length;
            }
        }

        private void TextBox_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            Clear();
        }

        private void TextBox_Click(object sender, EventArgs e)
        {
            UltraTextEditor tempTextBox = sender as UltraTextEditor;
            if (tempTextBox != null)
            {
                tempTextBox.SelectionStart = tempTextBox.Text.Length;
            }
        }

        private int lostFocusCount = 0;
        private void SkyCarComboBoxLostFocus(object sender, EventArgs e)
        {
            if (ComboBoxLostFocus != null)
            {
                if (IsReadOnly)
                {
                    ComboBoxLostFocus(this, e);
                }
                else
                {
                    //第一次是聚焦到FilterTextBox，ComboBox失去焦点
                    if (lostFocusCount == 0)
                    {
                        lostFocusCount++;
                    }
                    //第二次是ComboBox失去焦点
                    else if (lostFocusCount == 1)
                    {
                        lostFocusCount = 0;
                        ComboBoxLostFocus(this, e);
                    }
                }
            }
        }

        private void TextBoxOfFilter_GotFocus(object sender, EventArgs e)
        {
            if (SelectedValue.Length == 0 && lastTextChangedValue.Length > 0)
            {
                lastTextChangedValue = string.Empty;
                this.Text = this.TextBoxOfFilter.Text = string.Empty;
            }
        }

        private void TextBoxOfFilter_LostFocus(object sender, EventArgs e)
        {
            if (SelectedValue.Length == 0 && !lastTextChangedValue.Equals(pleaseSelectText))
            {
                if (!IsReadOnly)
                {
                    lastTextChangedValue = pleaseSelectText;
                    this.Text = this.TextBoxOfFilter.Text = pleaseSelectText;
                }
            }
        }
        #endregion

        private void ShowDropDown()
        {
            if (!this.IsReadOnly && _dropDownBox != null && !_dropDownBox.Visible)
            {
                if (DataViewOfDropdown != null)
                {
                    _clearTextButton.Visible = false;
                    _dropDownBox.Items[0].Visible = RowFilterVisible;
                    DataViewOfDropdown.RowFilter = string.Empty;

                    int heightOfGridView = Math.Min(300, dtOfDataSource.Rows.Count * DataGridViewOfDropdown.RowTemplate.Height + (ShowColumnsHeader ? DataGridViewOfDropdown.RowTemplate.Height : 0));
                    DataGridViewOfDropdown.ColumnHeadersVisible = ShowColumnsHeader;
                    DataGridViewOfDropdown.AutoSize = true;
                    DataGridViewOfDropdown.Height = heightOfGridView;

                    ////获取数据源中内容最长的数据
                    string maxContant = string.Empty;
                    foreach (DataRow loopDataSource in dtOfDataSource.Rows)
                    {
                        if (loopDataSource[displayColumnName] == null)
                        {
                            continue;
                        }
                        string loopContent = loopDataSource[displayColumnName].ToString();
                        if (_existsExtraItemDesc)
                        {
                            loopContent += loopDataSource[extraDisplayColumnName].ToString();
                        }
                        if (loopContent.Length > maxContant.Length)
                        {
                            maxContant = loopContent;
                        }
                    }

                    Graphics gridViewGraphics = DataGridViewOfDropdown.CreateGraphics();
                    DataGridViewOfDropdown.Font = new Font("微软雅黑", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    //关键
                    SizeF z = gridViewGraphics.MeasureString(maxContant, DataGridViewOfDropdown.Font);
                    //为了验证是否为显示的字符串长度
                    gridViewGraphics.DrawRectangle(new Pen(Color.Red), 0, 0, z.Width, z.Height);

                    bool noScroolBar = this.DataGridViewOfDropdown.DisplayedRowCount(false) ==
                                       this.DataGridViewOfDropdown.RowCount;
                    if (this.Width <= Convert.ToInt32(z.Width + 40))
                    {
                        DataGridViewOfDropdown.Width = Convert.ToInt32(z.Width + 40);
                        if (noScroolBar)
                        {
                            DataGridViewOfDropdown.Width = DataGridViewOfDropdown.Width + 20;
                        }
                    }
                    else
                    {
                        DataGridViewOfDropdown.Width = this.Size.Width + 3;
                    }
                    DataGridViewOfDropdown.Dock = DockStyle.Fill;
                    TextBoxOfFilter.Width = DataGridViewOfDropdown.Width;
                    _textBoxHost.Size = new Size(TextBoxOfFilter.Width - 3, TextBoxOfFilter.Height);

                    _dataGridViewHost.Size = new Size(DataGridViewOfDropdown.Width - 3, DataGridViewOfDropdown.Height + 10);

                    _dropDownBox.Width = this.Size.Width;
                    _dropDownBox.Height = TextBoxOfFilter.Height + DataGridViewOfDropdown.Height + 15;
                    _dropDownBox.Show(this, 0, 0);
                    int columnsCount = 0;
                    foreach (DataGridViewColumn col in DataGridViewOfDropdown.Columns)
                    {
                        col.Visible = (!col.Name.Equals(valueColumnName) || valueColumnName.Equals(displayColumnName));

                        if (col.Visible && !col.Name.Equals(checkColumnName))
                        {
                            columnsCount = columnsCount + 1;
                        }
                    }
                    int widthOfColumns = this.DataGridViewOfDropdown.Width - (this.SupportMultipleSelect ? 50 : 1);
                    if (columnsCount == 1)
                    {
                        foreach (DataGridViewColumn col in DataGridViewOfDropdown.Columns)
                        {
                            if (col.Visible && !col.Name.Equals(checkColumnName))
                            {
                                col.Width = widthOfColumns / columnsCount;
                            }
                        }
                    }
                    else if (columnsCount > 1)
                    {
                        foreach (DataGridViewColumn col in DataGridViewOfDropdown.Columns)
                        {
                            if (col.Visible && !col.Name.Equals(checkColumnName))
                            {
                                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            }
                        }
                    }

                    TextBoxOfFilter.Focus();
                    _clearTextButton.Visible = !string.IsNullOrEmpty(SelectedValue);
                }

            }
        }

        /// <summary>
        /// 清空下拉列表选中值和文本
        /// </summary>
        public void Clear()
        {
            lastTextChangedValue = pleaseSelectText;
            this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
            _selectedValue = new StringBuilder(5000);
            _selectedText = new StringBuilder(5000);
            SelectedTextExtra = string.Empty;

            if (dtOfDataSource != null && dtOfDataSource.Columns.Contains(checkColumnName))
            {
                dtOfDataSource.AsEnumerable().ToList().ForEach(p => p[checkColumnName] = false);
            }
            if (_dropDownBox.Visible)
            {
                if (this.SelectedValue.Length == 0)
                {
                    lastTextChangedValue = pleaseSelectText;
                    this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                }
                _dropDownBox.Close();
            }
            if (SelectedIndexChanged != null)
            {
                SelectedIndexChanged(this, new EventArgs());
            }
        }

        public void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                SelectedTextExtra = string.Empty;
                string currentCellValue = this.DataGridViewOfDropdown.Rows[e.RowIndex].Cells[valueColumnName].Value.ToString();
                string currentCellText = this.DataGridViewOfDropdown.Rows[e.RowIndex].Cells[displayColumnName].Value.ToString();
                if (!this.SupportMultipleSelect)
                {
                    if (_existsExtraItemDesc)
                    {
                        SelectedTextExtra = this.DataGridViewOfDropdown.Rows[e.RowIndex].Cells[extraDisplayColumnName].Value.ToString();
                    }
                    if (this.DataGridViewOfDropdown.SelectedRows.Count.Equals(1)
                        && !this.DataGridViewOfDropdown.SelectedRows[0].Cells[valueColumnName].Value.ToString().Equals(blankCellValue))
                    {
                        _selectedValue = new StringBuilder(this.DataGridViewOfDropdown.SelectedRows[0].Cells[valueColumnName].Value.ToString());
                        _selectedText = new StringBuilder(this.DataGridViewOfDropdown.SelectedRows[0].Cells[displayColumnName].Value.ToString());

                        changeTextByManualy = false;
                        lastTextChangedValue = _selectedText.ToString();
                        this.Text = lastTextChangedValue;
                        if (this.TextBoxOfFilter != null)
                        {
                            this.TextBoxOfFilter.Text = lastTextChangedValue;
                        }
                        if (this.SelectedValue.Length == 0)
                        {
                            lastTextChangedValue = pleaseSelectText;
                            this.Text = lastTextChangedValue;
                            if (this.TextBoxOfFilter != null)
                            {
                                this.TextBoxOfFilter.Text = lastTextChangedValue;
                            }
                        }
                        if (_dropDownBox != null)
                        {
                            _dropDownBox.Close();
                        }

                        if (SelectedIndexChanged != null)
                        {
                            SelectedIndexChanged(this, new EventArgs());
                        }
                    }
                }
                else
                {
                    string currentColumnName = this.DataGridViewOfDropdown.Columns[e.ColumnIndex].Name;
                    if (currentColumnName.Equals(checkColumnName))
                    {
                        int noSelectedNum = this.GetDataTableFromDataSource(DataGridViewOfDropdown.DataSource).Select(checkColumnName + "= 0"
                                            + " AND " + valueColumnName + "<>'" + selectAllCellValue
                                            + "' AND " + valueColumnName + "<>'" + blankCellValue
                                            + "' AND " + valueColumnName + "<>'" + currentCellValue
                                            + "'").Length;
                        if (!currentCellValue.Equals(selectAllCellValue)
                            && !currentCellValue.Equals(blankCellValue))
                        {
                            DataRow[] drSelectAll = this.GetDataTableFromDataSource(DataGridViewOfDropdown.DataSource).Select(valueColumnName + "='" + selectAllCellValue + "'");
                            if (!Convert.ToBoolean(this.DataGridViewOfDropdown.Rows[e.RowIndex].Cells[currentColumnName].EditedFormattedValue))
                            {
                                string[] selectedValueList = this._selectedValue.ToString().Split(SplitChar);
                                var tempDataSource = dtOfDataSource.AsEnumerable().ToList().Where(x => ((selectedValueList.Contains(x[valueColumnName].ToString()) || x[valueColumnName].ToString() == currentCellValue)) && x[valueColumnName].ToString() != blankCellValue).Select(x => x).ToList();
                                _selectedValue.Clear();
                                _selectedText.Clear();
                                foreach (var loopDataRow in tempDataSource)
                                {
                                    _selectedValue.Append(loopDataRow[valueColumnName] + SplitChar.ToString());
                                    _selectedText.Append(loopDataRow[displayColumnName] + SplitChar.ToString());
                                }
                                SetDisplayText();
                                if (drSelectAll.Length.Equals(1))
                                {
                                    drSelectAll[0][checkColumnName] = noSelectedNum == 0;
                                }
                                if (_selectedValue.ToString().Split(SplitChar).Length - 1 > 1)
                                {
                                    if (noSelectedNum == 0)
                                    {
                                        if (!lastTextChangedValue.Equals(selectAllCellText))
                                        {
                                            changeTextByManualy = false;
                                            lastTextChangedValue = selectAllCellText;
                                            this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                                        }
                                    }
                                    else
                                    {
                                        if (!lastTextChangedValue.Equals(multipleSelectText))
                                        {
                                            changeTextByManualy = false;
                                            lastTextChangedValue = multipleSelectText;
                                            this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!lastTextChangedValue.Equals(_selectedText.ToString()))
                                    {
                                        changeTextByManualy = false;
                                        lastTextChangedValue = _selectedText.ToString();
                                        this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                                    }
                                }
                            }
                            else
                            {
                                if (noSelectedNum.Equals(0))
                                {
                                    var tempDataSource0 = dtOfDataSource.AsEnumerable().ToList().
                                    Where(x => Convert.ToBoolean(x[checkColumnName])
                                        && x[valueColumnName].ToString() != selectAllCellValue
                                        && x[valueColumnName].ToString() != blankCellValue
                                        && !_selectedValue.ToString().Contains(x[valueColumnName].ToString() + SplitChar.ToString())).
                                        Select(x => x).ToList();
                                    tempDataSource0.ForEach(x => _selectedValue.Append(x[valueColumnName].ToString() + SplitChar.ToString()));
                                    tempDataSource0.ForEach(x => _selectedText.Append(x[displayColumnName].ToString() + SplitChar.ToString()));
                                }
                                _selectedValue.Replace(currentCellValue + SplitChar.ToString(), string.Empty);
                                _selectedText.Replace(currentCellText + SplitChar.ToString(), string.Empty);
                                SetDisplayText();

                                if (drSelectAll.Length.Equals(1)
                                    && Convert.ToBoolean(drSelectAll[0][checkColumnName]))
                                {
                                    drSelectAll[0][checkColumnName] = false;
                                }
                                if (_selectedValue.ToString().Split(SplitChar).Length - 1 > 1)
                                {
                                    if (!lastTextChangedValue.Equals(multipleSelectText))
                                    {
                                        changeTextByManualy = false;
                                        lastTextChangedValue = multipleSelectText;
                                        this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                                    }
                                }
                                else
                                {
                                    changeTextByManualy = false;
                                    lastTextChangedValue = _selectedText.ToString();
                                    this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                                }
                            }
                            if (this.DataGridViewOfDropdown.Rows.Count > e.RowIndex)
                            {
                                this.DataGridViewOfDropdown.Rows[e.RowIndex].Cells[currentColumnName].Value = !Convert.ToBoolean(this.DataGridViewOfDropdown.Rows[e.RowIndex].Cells[currentColumnName].EditedFormattedValue);
                                this.DataGridViewOfDropdown.EndEdit();
                            }
                        }
                        else if (currentCellValue.Equals(selectAllCellValue))
                        {
                            if (!Convert.ToBoolean(this.DataGridViewOfDropdown.Rows[e.RowIndex].Cells[currentColumnName].EditedFormattedValue))
                            {
                                _selectedValue = new StringBuilder(5000);
                                _selectedText = new StringBuilder(5000);

                                var tempDataSource0 = dtOfDataSource.AsEnumerable().ToList().
                                    Where(x => x[valueColumnName].ToString() != selectAllCellValue
                                        && x[valueColumnName].ToString() != blankCellValue
                                        //&& !_selectedValue.ToString().Contains(x[valueColumnName].ToString() + SplitChar.ToString())
                                        ).
                                        Select(x => x).ToList();
                                tempDataSource0.ForEach(x => _selectedValue.Append(x[valueColumnName].ToString() + SplitChar.ToString()));
                                tempDataSource0.ForEach(x => _selectedText.Append(x[displayColumnName].ToString() + SplitChar.ToString()));
                                var tempDataSource = dtOfDataSource.AsEnumerable().ToList().Where(x => x[valueColumnName].ToString() != selectAllCellValue && x[valueColumnName].ToString() != blankCellValue).Select(x => x).ToList();
                                tempDataSource.ForEach(x => x[checkColumnName] = true);
                                this.DataGridViewOfDropdown.EndEdit();
                            }
                            else
                            {
                                _selectedValue = new StringBuilder(5000);
                                _selectedText = new StringBuilder(5000);
                                var tempDataSource = dtOfDataSource.AsEnumerable().ToList().Where(x => x[valueColumnName].ToString() != selectAllCellValue && x[valueColumnName].ToString() != blankCellValue).Select(x => x).ToList();
                                tempDataSource.ForEach(x => x[checkColumnName] = false);
                                this.DataGridViewOfDropdown.EndEdit();
                            }
                            SetDisplayText();
                            if (_selectedValue.ToString().Length > 0)
                            {
                                if (!lastTextChangedValue.Equals(selectAllCellText))
                                {
                                    changeTextByManualy = false;
                                    lastTextChangedValue = selectAllCellText;
                                    this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                                }
                            }
                            else
                            {
                                if (!lastTextChangedValue.Equals(_selectedText.ToString()))
                                {
                                    changeTextByManualy = false;
                                    lastTextChangedValue = _selectedText.ToString();
                                    this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                                }
                            }
                            if (this.DataGridViewOfDropdown.Rows.Count > e.RowIndex)
                            {
                                this.DataGridViewOfDropdown.Rows[e.RowIndex].Cells[currentColumnName].Value = !Convert.ToBoolean(this.DataGridViewOfDropdown.Rows[e.RowIndex].Cells[currentColumnName].EditedFormattedValue);
                                this.DataGridViewOfDropdown.EndEdit();
                            }
                        }
                        if (SelectedIndexChanged != null)
                        {
                            SelectedIndexChanged(this, new EventArgs());
                        }
                        SendKeys.Send("{DOWN}");
                        SendKeys.Send("{UP}");
                    }
                }
            }
        }

        Color oldColorOfCell = Color.LightBlue;
        Color newColorOfCell = Color.SkyBlue;
        public void dataGridView_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex != -1 && !this.DataGridViewOfDropdown.Rows[e.RowIndex].DefaultCellStyle.BackColor.Equals(newColorOfCell))
            {
                oldColorOfCell = this.DataGridViewOfDropdown.Rows[e.RowIndex].DefaultCellStyle.BackColor;

                this.DataGridViewOfDropdown.Rows[e.RowIndex].DefaultCellStyle.BackColor = newColorOfCell;
            }
        }

        public void dataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex != -1 && !this.DataGridViewOfDropdown.Rows[e.RowIndex].DefaultCellStyle.BackColor.Equals(oldColorOfCell))
            {
                this.DataGridViewOfDropdown.Rows[e.RowIndex].DefaultCellStyle.BackColor = oldColorOfCell;
            }

        }

        private DataTable GetDataTableFromDataSource()
        {
            object dataSource = DataGridViewOfDropdown.DataSource;
            return GetDataTableFromDataSource(dataSource);
        }
        /// <summary>
        /// 从DataGridView 获取数据表
        /// </summary>
        /// <returns></returns>
        private DataTable GetDataTableFromDataSource(object dataSource)
        {
            if (dataSource is DataTable)
            {
                return (DataTable)dataSource;
            }
            else if (dataSource is DataView)
            {
                return ((DataView)dataSource).Table;
            }
            else if (dataSource is BindingSource)
            {
                object bind = ((BindingSource)dataSource).DataSource;
                if (bind is DataTable)
                {
                    return (DataTable)bind;
                }
                else
                {
                    return ((DataView)bind).Table;
                }
            }
            else if (dataSource is IList)
            {
                IList tempDataSource = dataSource as IList;
                foreach (var loopSourceItem in tempDataSource)
                {
                    if (loopSourceItem != null)
                    {
                        this.ItemSourceModelType = loopSourceItem.GetType();
                        break;
                    }
                }

                ComboBoxDataSet.ComboBoxInfoDataTable comboBoxTable = new ComboBoxDataSet.ComboBoxInfoDataTable();
                //如果DisplayMember为空，认为ItemsSource是一维的IList<object>数组
                if (string.IsNullOrEmpty(DisplayMember))
                {
                    foreach (var loopSourceItem in tempDataSource)
                    {
                        if (loopSourceItem == null) continue;
                        string itemDesc = loopSourceItem.ToString();
                        string itemValue = GetComboBoxItemValue(tempDataSource, loopSourceItem);

                        ComboBoxDataSet.ComboBoxInfoRow comboBoxRow = comboBoxTable.NewComboBoxInfoRow();

                        comboBoxRow.ItemDesc = itemDesc;
                        comboBoxRow.ExtraItemDesc = itemDesc;
                        comboBoxRow.ItemDescAllSpellCode = ChineseSpellCode.GetFullSpellCode(itemDesc);
                        comboBoxRow.ItemDescAllFirstSpellCode = ChineseSpellCode.GetShortSpellCode(itemDesc);
                        comboBoxRow.ItemDescLen = string.IsNullOrEmpty(itemDesc) ? 0 : itemDesc.Length;
                        comboBoxRow.ItemValue = itemValue;

                        comboBoxTable.AddComboBoxInfoRow(comboBoxRow);
                    }
                }
                else
                {
                    //当DisplayMember有特定值时，运用反射获取属性值
                    if (!string.IsNullOrEmpty(DisplayMember) && ItemSourceModelType != null && ItemSourceModelType.GetProperty(DisplayMember) != null)
                    {
                        PropertyInfo propertyInfo = ItemSourceModelType.GetProperty(DisplayMember);
                        PropertyInfo propertyInfoExtra = ItemSourceModelType.GetProperty(ExtraDisplayMember);
                        _existsExtraItemDesc = propertyInfoExtra != null;
                        if (propertyInfo != null)
                        {
                            foreach (var loopSourceItem in tempDataSource)
                            {
                                var propValue = propertyInfo.GetValue(loopSourceItem, null);
                                if (propValue == null || string.IsNullOrEmpty(propValue.ToString())) continue;

                                string propValueExtra = null;
                                if (_existsExtraItemDesc)
                                {
                                    if (propertyInfoExtra != null)
                                    {
                                        propValueExtra = (propertyInfoExtra.GetValue(loopSourceItem, null) ?? string.Empty).ToString();
                                    }
                                }

                                string itemDesc = propValue.ToString();
                                string itemValue = GetComboBoxItemValue(tempDataSource, loopSourceItem);

                                ComboBoxDataSet.ComboBoxInfoRow comboBoxRow = comboBoxTable.NewComboBoxInfoRow();

                                comboBoxRow.ItemDesc = itemDesc;
                                comboBoxRow.ExtraItemDesc = propValueExtra;
                                comboBoxRow.ItemDescAllSpellCode = ChineseSpellCode.GetFullSpellCode(itemDesc);
                                comboBoxRow.ItemDescAllFirstSpellCode = ChineseSpellCode.GetShortSpellCode(itemDesc);
                                comboBoxRow.ItemDescLen = string.IsNullOrEmpty(itemDesc) ? 0 : itemDesc.Length;
                                comboBoxRow.ItemValue = itemValue;

                                comboBoxTable.AddComboBoxInfoRow(comboBoxRow);
                            }
                        }
                    }
                }

                return comboBoxTable;
            }
            else
            {
                return null;
            }
        }

        private string GetComboBoxItemValue(IList paramDataSource, object paramItem)
        {
            if (paramDataSource == null || paramDataSource.Count == 0)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(ValueMember) || ItemSourceModelType == null)
            {
                return paramItem as string;
            }

            PropertyInfo property = ItemSourceModelType.GetProperty(ValueMember);
            if (property == null)
            {
                return string.Empty;
            }
            var temp = property.GetValue(paramItem, null);
            if (temp == null)
            {
                return string.Empty;
            }

            return temp.ToString();
        }
        private void dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.DataGridViewOfDropdown.SelectedRows.Count.Equals(1)
                    && !this.DataGridViewOfDropdown.SelectedRows[0].Cells[valueColumnName].Value.ToString().Equals(blankCellValue))
                {
                    if (!_selectedValue.ToString().Contains(this.DataGridViewOfDropdown.SelectedRows[0].Cells[valueColumnName].Value.ToString() + (this.SupportMultipleSelect ? SplitChar.ToString() : string.Empty)))
                    {
                        if (this.SupportMultipleSelect)
                        {
                            _selectedValue.Append(this.DataGridViewOfDropdown.SelectedRows[0].Cells[valueColumnName].Value.ToString() + (this.SupportMultipleSelect ? SplitChar.ToString() : string.Empty));
                            _selectedText.Append(this.DataGridViewOfDropdown.SelectedRows[0].Cells[displayColumnName].Value.ToString() + (this.SupportMultipleSelect ? SplitChar.ToString() : string.Empty));
                            this.DataGridViewOfDropdown.SelectedRows[0].Cells[checkColumnName].Value = true;
                        }
                        else
                        {
                            _selectedValue = new StringBuilder(this.DataGridViewOfDropdown.SelectedRows[0].Cells[valueColumnName].Value.ToString());
                            _selectedText = new StringBuilder(this.DataGridViewOfDropdown.SelectedRows[0].Cells[displayColumnName].Value.ToString());
                            //SendKeys.Send("{TAB}");
                        }
                        SetDisplayText();
                        if (_selectedValue.ToString().Split(SplitChar).Length - 1 > 1)
                        {
                            if (!lastTextChangedValue.Equals(multipleSelectText))
                            {
                                changeTextByManualy = false;
                                lastTextChangedValue = multipleSelectText;
                                this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                            }
                        }
                        else
                        {
                            if (!lastTextChangedValue.Equals(_selectedText.ToString()))
                            {
                                changeTextByManualy = false;
                                lastTextChangedValue = _selectedText.ToString();
                                this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                            }
                        }
                    }
                    if (this.SelectedValue.Length == 0)
                    {
                        lastTextChangedValue = pleaseSelectText;
                        this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                    }
                    _dropDownBox.Close();
                    if (SelectedIndexChanged != null)
                    {
                        SelectedIndexChanged(this, new EventArgs());
                    }
                }
            }
        }

        protected void OnAfterSelector(object sender, AfterSelectorEventArgs e)
        {
            if (AfterSelector != null)
            {
                AfterSelector(sender, e);
            }
        }

        protected virtual void RaiseAfterSelector(object sender, AfterSelectorEventArgs e)
        {
            OnAfterSelector(sender, e);
        }
        #region 重写方法

        private string GetRowFilterString(string sText)
        {
            string sFilter = string.Empty;
            if (m_sDisplayMember == String.Empty || m_sDisplayMember == null)
            {
                m_sDisplayMember = DataViewOfDropdown.Table.Columns[0].ColumnName;
            }
            if (m_sKeyWords == String.Empty)
            {
                m_sKeyWords = m_sDisplayMember;
            }
            string[] sColumns = m_sKeyWords.Split(',');
            foreach (string sColumn in sColumns)
            {
                sFilter += sColumn + " like " + "'%" + sText + "%'" + " or ";
            }
            sFilter = sFilter.Trim().TrimEnd("or".ToCharArray());
            return sFilter;
        }

        private bool changeTextByManualy = true;
        private string lastTextChangedValue = string.Empty;
        private bool hideNoSelected = false;
        private void textBox_TextChanged(object sender, System.EventArgs e)
        {
            if (!this.TextBoxOfFilter.Text.Trim().Equals(lastTextChangedValue))
            {
                if (!lastTextChangedValue.Equals(selectAllCellText)
                   && !lastTextChangedValue.Equals(multipleSelectText))
                {
                    if (string.IsNullOrEmpty(TextBoxOfFilter.Text.Trim()) && _selectedValue.ToString().Split(SplitChar).Length < 2)
                    {
                        _selectedValue = new StringBuilder(5000);
                        _selectedText = new StringBuilder(5000);
                        if (dtOfDataSource.Columns.Contains(checkColumnName))
                        {
                            dtOfDataSource.AsEnumerable().ToList().ForEach(p => p[checkColumnName] = false);
                        }
                    }
                    changeTextByManualy = true;
                    if (!SupportMultipleSelect && string.IsNullOrEmpty(TextBoxOfFilter.Text.Trim()) && SelectedIndexChanged != null)
                    {
                        SelectedIndexChanged(this, e);
                    }
                }
                this.Text = lastTextChangedValue = this.TextBoxOfFilter.Text.Trim();
                if (!_dropDownBox.Visible)
                {
                    ShowDropDown();
                }
                if (_dropDownBox.Visible && changeTextByManualy)
                {
                    FilterAndSortView(false);
                }
            }
        }

        private void FilterAndSortView(bool hideNoSelected)
        {
            try
            {
                string filterCondition = string.Empty;
                if (hideNoSelected || this.TextBoxOfFilter.Text.Trim().Length > 0)
                {
                    this.DataViewOfDropdown.Sort = (dtOfDataSource.Columns.Contains(matchIndexColumnName) ? matchIndexColumnName : (!string.IsNullOrEmpty(displayColumnName) ? displayColumnName : string.Empty));
                    if (hideNoSelected)
                    {
                        filterCondition = this.checkColumnName + " = 1 OR " + valueColumnName + " = '" + this.blankCellValue + "'";
                        this.DataGridViewOfDropdown.Focus();
                    }
                    else if (this.TextBoxOfFilter.Text.Trim().Length > 0)
                    {
                        string[] selectedValueList = this.TextBoxOfFilter.Text.Trim().Split(SplitChar);
                        if (selectedValueList.Length.Equals(1))
                        {
                            string filterContent = this.TextBoxOfFilter.Text.Trim();
                            foreach (DataRow loopSourceRow in dtOfDataSource.Rows)
                            {
                                if (loopSourceRow[valueColumnName] != null &&
                                    (loopSourceRow[valueColumnName].ToString() == selectAllCellValue ||
                                    loopSourceRow[valueColumnName].ToString() == blankCellValue))
                                {
                                    continue;
                                }

                                int matchIndexAfterFilted = 0;
                                #region 判断是否匹配
                                bool isMatchedItemDesc = false;
                                if (loopSourceRow[displayColumnName].ToString().ToUpper().Contains(filterContent.ToUpper()))
                                {
                                    matchIndexAfterFilted = loopSourceRow[displayColumnName].ToString().ToUpper().ToUpper().IndexOf(filterContent.ToUpper(), StringComparison.Ordinal);
                                    isMatchedItemDesc = true;
                                }
                                bool isMatchedItemDescAllFirstSpellCode = false;
                                if (!isMatchedItemDesc)
                                {
                                    if (loopSourceRow[ItemDescAllFirstSpellCodeColumnName].ToString().ToUpper().Contains(filterContent.ToUpper()))
                                    {
                                        matchIndexAfterFilted = (loopSourceRow[ItemDescAllFirstSpellCodeColumnName].ToString().ToUpper().ToUpper().IndexOf(filterContent.ToUpper(), StringComparison.Ordinal) + 1) * 10;
                                        isMatchedItemDescAllFirstSpellCode = true;
                                    }
                                }
                                bool isMatchedItemDescAllSpellCode = false;
                                if (!isMatchedItemDesc && !isMatchedItemDescAllFirstSpellCode)
                                {
                                    if (loopSourceRow[itemDescAllSpellCodeColumnName].ToString().ToUpper().Contains(filterContent.ToUpper()))
                                    {
                                        matchIndexAfterFilted = (loopSourceRow[itemDescAllSpellCodeColumnName].ToString().ToUpper().ToUpper().IndexOf(filterContent.ToUpper(), StringComparison.Ordinal) + 1) * 100;
                                        isMatchedItemDescAllSpellCode = true;
                                    }
                                }
                                loopSourceRow[isMatchedColumnName] = isMatchedItemDesc || isMatchedItemDescAllFirstSpellCode || isMatchedItemDescAllSpellCode;

                                loopSourceRow[matchIndexColumnName] = matchIndexAfterFilted;
                                #endregion
                            }

                            filterCondition = isMatchedColumnName + " = 1" + " OR " + valueColumnName + " = '" + this.blankCellValue + "'";
                            this.DataViewOfDropdown.Sort = matchIndexColumnName + "," + ItemDescLenColumnName + "," + displayColumnName;
                        }
                    }
                }
                else
                {
                    this.DataViewOfDropdown.Sort = matchIndexColumnName + "," + displayColumnName;
                }

                filterCondition = filterCondition.Replace(pleaseSelectText, string.Empty);
                this.DataViewOfDropdown.RowFilter = filterCondition;
                if (this.DataViewOfDropdown.Count > 0)
                {
                    this.DataGridViewOfDropdown.FirstDisplayedScrollingRowIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.IsReadOnly)
            {
                return;
            }
            if (e.KeyData == Keys.Enter)
            {
                if (!_dropDownBox.Visible)
                {
                    ShowDropDown();
                }
                else if (!DataGridViewOfDropdown.Focused)
                {
                    DataGridViewOfDropdown.Focus();
                    SendKeys.Send("{ENTER}");
                }
            }
            else if (e.KeyData == Keys.Down)
            {
                DataGridViewOfDropdown.Focus();
            }
            else if (this.TextBoxOfFilter.Text.Trim().Equals(pleaseSelectText))
            {
                lastTextChangedValue = string.Empty;
                this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
            }
        }

        ToolTip tipOfTextBox = null;
        private void textBox_MouseEnter(object sender, EventArgs e)
        {
            if (tipOfTextBox == null)
            {
                tipOfTextBox = new ToolTip();
                tipOfTextBox.ForeColor = Color.Black;
                tipOfTextBox.BackColor = SystemColors.Info;
                tipOfTextBox.ToolTipIcon = ToolTipIcon.Info;
                tipOfTextBox.ToolTipTitle = "提醒";
            }
            if (!this.IsReadOnly)
            {
                if (SelectedValue.Split(SplitChar).Length > 2)
                {
                    this.tipOfTextBox.SetToolTip(this.TextBoxOfFilter, !hideNoSelected ? "双击文本框只显示已选择的行" : "双击文本框显示全部");
                }
                this.tipOfTextBox.SetToolTip(this.TextBoxOfFilter, SelectedValue);
            }
            else
            {
                string toopTipContent;
                if (SelectedText.Length > 200)
                {
                    toopTipContent = SelectedText.Substring(0, 200) + "......";
                }
                else
                {
                    toopTipContent = SelectedText;
                }
                this.tipOfTextBox.SetToolTip(this.TextBoxOfFilter, toopTipContent);
            }
        }

        private void SkyCarMultiComboBox_MouseEnter(object sender, EventArgs e)
        {
            if (tipOfTextBox == null)
            {
                tipOfTextBox = new ToolTip();
                tipOfTextBox.ForeColor = Color.Black;
                tipOfTextBox.BackColor = SystemColors.Info;
                tipOfTextBox.ToolTipIcon = ToolTipIcon.Info;
                tipOfTextBox.ToolTipTitle = "提醒";
            }
            if (!this.IsReadOnly)
            {
                if (SelectedValue.Split(SplitChar).Length > 2)
                {
                    this.tipOfTextBox.SetToolTip(this.TextBoxOfFilter, !hideNoSelected ? "双击文本框只显示已选择的行" : "双击文本框显示全部");
                }
            }
            else
            {
                string toopTipContent;
                if (SelectedText.Length > 200)
                {
                    toopTipContent = SelectedText.Substring(0, 200) + "......";
                }
                else
                {
                    toopTipContent = SelectedText;
                }
                tipOfTextBox.ToolTipTitle = "";
                this.tipOfTextBox.SetToolTip(this, toopTipContent);
            }
        }

        private void textBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.IsReadOnly)
            {
                return;
            }

            if (this.TextBoxOfFilter.Text.Trim().Length == 0)
            {
                if (tipOfTextBox == null)
                {
                    tipOfTextBox = new ToolTip();
                    tipOfTextBox.ForeColor = Color.Green;
                    tipOfTextBox.BackColor = SystemColors.Info;
                    tipOfTextBox.ToolTipIcon = ToolTipIcon.Info;
                    tipOfTextBox.ToolTipTitle = "提醒";
                }
                this.tipOfTextBox.SetToolTip(this.TextBoxOfFilter, "输入中文或拼音可快速筛选");
            }
        }

        private void textBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.IsReadOnly)
            {
                return;
            }

            if (e.Button.Equals(MouseButtons.Left)
                && this.TextBoxOfFilter.Text.Equals(pleaseSelectText))
            {
                lastTextChangedValue = string.Empty;
                this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
            }
        }

        private void textBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.IsReadOnly)
            {
                return;
            }

            if (this.SupportMultipleSelect)
            {
                if (!_dropDownBox.Visible)
                {
                    ShowDropDown();
                }
                hideNoSelected = !hideNoSelected;
                if (!hideNoSelected)
                {
                    lastTextChangedValue = string.Empty;
                    this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                }
                else
                {
                    int noSelectedNum = this.GetDataTableFromDataSource(DataGridViewOfDropdown.DataSource).Select(checkColumnName + "= 0"
                                            + " AND " + valueColumnName + "<>'" + selectAllCellValue
                                            + "' AND " + valueColumnName + "<>'" + blankCellValue
                                            + "'").Length;
                    if (_selectedValue.ToString().Split(SplitChar).Length - 1 > 1)
                    {
                        if (noSelectedNum == 0)
                        {
                            if (!lastTextChangedValue.Equals(selectAllCellText))
                            {
                                lastTextChangedValue = selectAllCellText;
                                this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                            }
                        }
                        else
                        {
                            if (!lastTextChangedValue.Equals(multipleSelectText))
                            {
                                lastTextChangedValue = multipleSelectText;
                                this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                            }
                        }
                    }
                    else
                    {
                        if (!lastTextChangedValue.Equals(_selectedText.ToString()))
                        {
                            lastTextChangedValue = _selectedText.ToString();
                            this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                        }
                    }
                }
                FilterAndSortView(hideNoSelected);
            }
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                ShowDropDown();
                if (this.TextBoxOfFilter.Text.Trim().Equals(pleaseSelectText))
                {
                    lastTextChangedValue = string.Empty;
                    this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                }
            }
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            ShowDropDown();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsLetter(e.KeyChar))
            {
                this.Text = this.TextBoxOfFilter.Text = e.KeyChar.ToString();
                this.TextBoxOfFilter.SelectionStart = this.TextBoxOfFilter.Text.Length;
                e.Handled = true;
            }
            base.OnKeyPress(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDBLCLK || m.Msg == WM_LBUTTONDOWN)
            {
                if (m_blDropShow)
                {
                    m_blDropShow = false;
                }
                else
                {
                    m_blDropShow = true;
                }
                if (m_blDropShow)
                {
                    ShowDropDown();
                }
                else
                {
                    if (this.SelectedValue.Length == 0)
                    {
                        lastTextChangedValue = pleaseSelectText;
                        this.Text = this.TextBoxOfFilter.Text = lastTextChangedValue;
                    }
                    _dropDownBox.Close();
                }
                return;
            }
            base.WndProc(ref m);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dropDownBox != null)
                {
                    _dropDownBox.Dispose();
                    _dropDownBox = null;
                }
            }
            base.Dispose(disposing);
        }
        #endregion
        #endregion
    }

    public delegate void ComBoxAfterSelectorEventHandler(object sender, AfterSelectorEventArgs e);

    public class ComBoxAfterSelectorEventArgs : EventArgs
    {
        private int _rowIndex;
        private int _columnIndex;
        private object _value;

        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }
        public int ColumnIndex
        {
            get { return _columnIndex; }
            set { _columnIndex = value; }
        }
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public ComBoxAfterSelectorEventArgs(int rowIndex, int columnIndex, object value)
            : base()
        {
            _rowIndex = rowIndex;
            _columnIndex = columnIndex;
            _value = value;
        }
    }

    public delegate void AfterSelectorEventHandler(object sender, AfterSelectorEventArgs e);

    public class AfterSelectorEventArgs : EventArgs
    {
        private int _rowIndex;
        private int _columnIndex;
        private object _value;

        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }
        public int ColumnIndex
        {
            get { return _columnIndex; }
            set { _columnIndex = value; }
        }
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public AfterSelectorEventArgs(int rowIndex, int columnIndex, object value)
            : base()
        {
            _rowIndex = rowIndex;
            _columnIndex = columnIndex;
            _value = value;
        }
    }
}
