using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.UIModel.Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Framework.WindowUI;

namespace SkyCar.Coeus.UI.Common
{
    /// <summary>
    /// 供应商查询
    /// </summary>
    public partial class FrmSupplierQuery : BaseForm
    {
        #region 全局变量
        /// <summary>
        /// 供应商查询BLL
        /// </summary>
        private BLLBase _bll = new BLLBase(Trans.COM);

        /// <summary>
        /// 翻页按钮ToolBar
        /// </summary>
        private UltraToolbarsManager _toolBarPaging = new UltraToolbarsManager();

        /// <summary>
        /// Grid的数据源
        /// </summary>
        List<SupplierQueryUIModel> ListGridDS = new List<SupplierQueryUIModel>();

        /// <summary>
        /// 列表选择模式，默认单选，多选时可以选择多项。
        /// </summary>
        private CustomEnums.CustomeSelectionMode ListSelectionMode { get; set; }

        /// <summary>
        /// 数据源存在选中项值对应的属性
        /// </summary>
        bool _dataSourceHasSelectedValueProperty = false;

        /// <summary>
        /// 数据源存在选中项描述对应的属性
        /// </summary>
        bool _dataSourceHasSelectedTextProperty = false;

        #region 列表Grid翻页用

        /// <summary>
        /// 翻页ToolBar
        /// </summary>
        private UltraToolbarsManager ToolBarPaging
        {
            get { return _toolBarPaging; }
            set
            {
                _toolBarPaging = value;
                //初始化翻页
                SetBarPaging(TotalRecordCount);
            }
        }

        /// <summary>
        /// 执行子类中的查询方法
        /// </summary>
        private Action ExecuteQuery;
        /// <summary>
        /// 当前页面索引值
        /// </summary>
        private int PageIndex = 1;
        /// <summary>
        /// 总记录条数
        /// </summary>
        private int TotalRecordCount = 0;
        /// <summary>
        /// 页面大小
        /// </summary>
        private int PageSize = 20;
        /// <summary>
        /// 总页面数
        /// </summary>
        private int TotalPageCount = 0;
        #endregion

        #endregion

        #region 公共属性

        /// <summary>
        /// 选中项的值对应的属性名
        /// </summary>
        public string SelectedValuePropertyName { get; set; }
        /// <summary>
        /// 选中项的描述对应的属性名
        /// </summary>
        public string SelectedTextPropertyName { get; set; }

        /// <summary>
        /// 选中项的父级值(在两极联动时使用)
        /// </summary>
        public string SelectedItemParentValue { get; set; }

        /// <summary>
        /// 选中项的值
        /// </summary>
        public string SelectedValue { get; set; }
        /// <summary>
        /// 选中项的描述
        /// </summary>
        public string SelectedText { get; set; }
        #endregion

        #region 系统事件

        /// <summary>
        /// 供应商查询
        /// </summary>
        /// <param name="paramSelectedValuePropertyName">选中项的值对应数据源中的属性名</param>
        /// <param name="paramSelectedTextPropertyName">选中项的描述对应数据源中的属性名</param>
        /// <param name="paramSelectedValue">默认选中项的值</param>
        /// <param name="paramCustomeSelectionMode">选择模式，默认单选</param>
        public FrmSupplierQuery(string paramSelectedValuePropertyName,
            string paramSelectedTextPropertyName = null,
            string paramSelectedValue = null,
            CustomEnums.CustomeSelectionMode paramCustomeSelectionMode = CustomEnums.CustomeSelectionMode.Single)
        {
            if (string.IsNullOrEmpty(paramSelectedValuePropertyName))
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0021, new object[] { MsgParam.paramSelectedValuePropertyName }), MessageBoxButtons.OK, MessageBoxIcon.Information );
                return;
            }
            SelectedValuePropertyName = paramSelectedValuePropertyName;
            SelectedValue = paramSelectedValue;

            //paramSelectedTextPropertyName为空时，用paramSelectedValuePropertyName代替
            if (string.IsNullOrEmpty(paramSelectedTextPropertyName))
            {
                SelectedTextPropertyName = paramSelectedValuePropertyName;
                SelectedText = paramSelectedValue;
            }
            else
            {
                SelectedTextPropertyName = paramSelectedTextPropertyName;
            }

            ListSelectionMode = paramCustomeSelectionMode;

            InitializeComponent();
        }

        public override sealed string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSupplierQuery_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（翻页）
            ToolBarPaging = toolBarManagerListTabPaging;
            //查询委托（基类控制翻页用）
            ExecuteQuery = QueryAction;
            //工具栏（翻页）单击事件
            toolBarManagerListTabPaging.ToolClick += new ToolClickEventHandler(ToolBarPaging_ToolClick);
            //工具栏（翻页）[当前页]值改变事件
            toolBarManagerListTabPaging.ToolValueChanged += new ToolEventHandler(ToolBarPaging_ToolValueChanged);
            //初始化【列表】Tab内控件
            InitializeListTabControls();
            #endregion

        }

        /// <summary>
        /// 单元格的值变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_CellChange(object sender, CellEventArgs e)
        {
            //提交数据到数据源
            gdGrid.UpdateData();
            if (e.Cell != null && e.Cell.Row != null)
            {
                bool curRowIsChecked = Convert.ToBoolean(e.Cell.Row.Cells[SysConst.IsChecked].Value);

                if (curRowIsChecked)
                {
                    object relatedSourItem = null;
                    //选中当前行，且为单选模式时，取消选中其他行
                    if (ListSelectionMode == CustomEnums.CustomeSelectionMode.Single)
                    {
                        string curRowID = e.Cell.Row.Cells[SysConst.RowID].Value != null ? e.Cell.Row.Cells[SysConst.RowID].Value.ToString() : string.Empty;

                        foreach (var loopSourceItem in ListGridDS)
                        {
                            if (loopSourceItem.RowID != curRowID)
                            {
                                if (loopSourceItem.IsChecked)
                                {
                                    loopSourceItem.IsChecked = false;
                                }
                            }
                            else
                            {
                                relatedSourItem = loopSourceItem;
                            }
                        }
                        this.gdGrid.Refresh();
                    }

                    #region 验证数据源是否存在选中项相关属性
                    if (!_dataSourceHasSelectedValueProperty || !_dataSourceHasSelectedTextProperty)
                    {
                        if (relatedSourItem != null)
                        {
                            Type tp = relatedSourItem.GetType();

                            foreach (PropertyInfo pi in tp.GetProperties())
                            {
                                //过滤索引器
                                if (SysConst.EN_ITEM.Equals(pi.Name)) continue;

                                if (pi.Name == SelectedValuePropertyName)
                                {
                                    _dataSourceHasSelectedValueProperty = true;
                                }
                                if (pi.Name == SelectedTextPropertyName)
                                {
                                    _dataSourceHasSelectedTextProperty = true;
                                }
                                if (_dataSourceHasSelectedValueProperty && _dataSourceHasSelectedTextProperty)
                                {
                                    break;
                                }
                            }
                            if (!_dataSourceHasSelectedValueProperty)
                            {
                                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0022, new object[] { SelectedValuePropertyName }), MessageBoxButtons.OK, MessageBoxIcon.Information );
                                return;
                            }
                            if (!_dataSourceHasSelectedTextProperty)
                            {
                                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0022, new object[] { SelectedTextPropertyName }), MessageBoxButtons.OK, MessageBoxIcon.Information );
                                return;
                            }
                        }
                    }
                    #endregion

                }
                GenerateSelectedValueAndText();
            }
        }

        /// <summary>
        /// 双击单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            //是否允许获取Grid数据
            if (!IsAllowSetGridDataToCard())
            {
                return;
            }
            object relatedSourItem = null;
            string tempRowID = e.Cell.Row.Cells[SysConst.RowID].Value != null ? e.Cell.Row.Cells[SysConst.RowID].Value.ToString() : string.Empty;
            foreach (var loopSourceItem in ListGridDS)
            {
                if (loopSourceItem.RowID == tempRowID)
                {
                    relatedSourItem = loopSourceItem;
                    if (!loopSourceItem.IsChecked)
                    {
                        loopSourceItem.IsChecked = true;
                        this.gdGrid.Refresh();
                    }
                }
                else
                {
                    //双击当前行，且为单选模式时，取消选中其他行
                    if (ListSelectionMode == CustomEnums.CustomeSelectionMode.Single)
                    {
                        if (loopSourceItem.IsChecked)
                        {
                            loopSourceItem.IsChecked = false;
                        }
                    }
                }
            }

            #region 验证数据源是否存在选中项相关属性
            if (!_dataSourceHasSelectedValueProperty || !_dataSourceHasSelectedTextProperty)
            {
                if (relatedSourItem != null)
                {
                    Type tp = relatedSourItem.GetType();

                    foreach (PropertyInfo pi in tp.GetProperties())
                    {
                        //过滤索引器
                        if (SysConst.EN_ITEM.Equals(pi.Name)) continue;

                        if (pi.Name == SelectedValuePropertyName)
                        {
                            _dataSourceHasSelectedValueProperty = true;
                        }
                        if (pi.Name == SelectedTextPropertyName)
                        {
                            _dataSourceHasSelectedTextProperty = true;
                        }
                        if (_dataSourceHasSelectedValueProperty && _dataSourceHasSelectedTextProperty)
                        {
                            break;
                        }
                    }
                    if (!_dataSourceHasSelectedValueProperty)
                    {
                        MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0022, new object[] { SelectedValuePropertyName }), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (!_dataSourceHasSelectedTextProperty)
                    {
                        MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0022, new object[] { SelectedTextPropertyName }), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            #endregion

            GenerateSelectedValueAndText();

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// [清空]按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            //初始化【列表】Tab内控件
            InitializeListTabControls();
        }
        /// <summary>
        /// [查询]按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //查询
            QueryAction();
        }

        /// <summary>
        /// Grid的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_DoubleClick(object sender, EventArgs e)
        {
            //是否允许获取Grid数据
            if (!IsAllowSetGridDataToCard())
            {
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 清除已选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearSelectedItem_Click(object sender, EventArgs e)
        {
            foreach (var loopSourceItem in ListGridDS)
            {
                loopSourceItem.IsChecked = false;
            }
            this.gdGrid.Refresh();
            SelectedValue = string.Empty;
            SelectedText = string.Empty;
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedValue)
                || string.IsNullOrEmpty(SelectedText))
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0038, new object[] { }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region 重写基类方法

        /// <summary>
        /// 查询
        /// </summary>
        public void QueryAction()
        {
            _bll.QueryForList<SupplierQueryUIModel>(SQLID.COMM_SQL17, new SupplierQueryQCModel()
            {
                PageSize = PageSize,
                PageIndex = PageIndex,
                //供应商名称
                WHERE_SUPP_Name = txtWhere_SUPP_Name.Text.Trim()
            }, ListGridDS);

            #region 设置选中项
            if (!string.IsNullOrEmpty(SelectedValue))
            {
                foreach (var loopSourceItem in ListGridDS)
                {
                    Type tp = loopSourceItem.GetType();
                    string tempSelectedValue = string.Empty;
                    foreach (PropertyInfo pi in tp.GetProperties())
                    {
                        //过滤索引器
                        if (SysConst.EN_ITEM.Equals(pi.Name)) continue;

                        if (pi.Name == SelectedValuePropertyName)
                        {
                            object tempPropertyValue = pi.GetValue(loopSourceItem, null);
                            tempSelectedValue = tempPropertyValue != null ? tempPropertyValue.ToString() : string.Empty;
                            if (!string.IsNullOrEmpty(tempSelectedValue))
                            {
                                if ((SysConst.Semicolon_DBC + SelectedValue + SysConst.Semicolon_DBC).Contains(SysConst.Semicolon_DBC + tempSelectedValue + SysConst.Semicolon_DBC))
                                {
                                    loopSourceItem.IsChecked = true;
                                }
                            }

                        }
                    }
                }
            }
            #endregion

            gdGrid.DataSource = ListGridDS;
            gdGrid.DataBind();

            if (ListGridDS.Count > 0)
            {
                dynamic subObject = ListGridDS[0];

                TotalRecordCount = subObject.RecordCount;
            }
            else
            {
                TotalRecordCount = 0;
            }
            //设置翻页控件
            SetBarPaging(TotalRecordCount);
            //设置单元格是否可以编辑
            SetPayConfirmWindowStyle();
        }
        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 查询条件初始化
            //供应商
            txtWhere_SUPP_Name.Clear();
            //给 供应商 设置焦点
            lblWhere_SUPP_Name.Focus();

            #endregion

            //清空Grid
            ListGridDS = new List<SupplierQueryUIModel>();
            gdGrid.DataSource = ListGridDS;
            gdGrid.DataBind();
        }

        /// <summary>
        /// 生成选中项的值和描述
        /// </summary>
        void GenerateSelectedValueAndText()
        {
            StringBuilder selectedValueStringBuilder = new StringBuilder();
            StringBuilder selectedTextStringBuilder = new StringBuilder();
            foreach (var loopSourceItem in ListGridDS)
            {
                if (!loopSourceItem.IsChecked)
                {
                    continue;
                }

                Type tp = loopSourceItem.GetType();
                string tempSelectedValue = string.Empty;
                string tempSelectedText = string.Empty;
                foreach (PropertyInfo pi in tp.GetProperties())
                {
                    //过滤索引器
                    if (SysConst.EN_ITEM.Equals(pi.Name)) continue;

                    if (pi.Name == SelectedValuePropertyName)
                    {
                        object tempPropertyValue = pi.GetValue(loopSourceItem, null);
                        tempSelectedValue = tempPropertyValue != null ? tempPropertyValue.ToString() : string.Empty;
                    }
                    if (pi.Name == SelectedTextPropertyName)
                    {
                        object tempPropertyValue = pi.GetValue(loopSourceItem, null);
                        tempSelectedText = tempPropertyValue != null ? tempPropertyValue.ToString() : string.Empty;
                    }
                }
                if (string.IsNullOrEmpty(selectedValueStringBuilder.ToString()))
                {
                    selectedValueStringBuilder.Append(tempSelectedValue);
                }
                else
                {
                    selectedValueStringBuilder.Append(SysConst.Semicolon_DBC + tempSelectedValue);
                }

                if (string.IsNullOrEmpty(selectedTextStringBuilder.ToString()))
                {
                    selectedTextStringBuilder.Append(tempSelectedText);
                }
                else
                {
                    selectedTextStringBuilder.Append(SysConst.Semicolon_DBC + tempSelectedText);
                }

            }

            SelectedValue = selectedValueStringBuilder.ToString();
            SelectedText = selectedTextStringBuilder.ToString();
        }

        /// <summary>
        /// 是否允许获取Grid数据
        /// </summary>
        /// <returns>true:允许；false：不允许</returns>
        private bool IsAllowSetGridDataToCard()
        {
            if (gdGrid.ActiveRow == null || gdGrid.ActiveRow.Index < 0)
            {
                return false;
            }
            if (gdGrid.DisplayLayout.Bands[0].SortedColumns.Count > 0)
            {
                foreach (UltraGridColumn loopColumn in gdGrid.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (loopColumn.IsGroupByColumn)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #region Grid翻页相关方法

        #region 公共
        /// <summary>
        /// 设置翻页控件
        /// <para>窗体加载或初始化时调用</para>
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        private void SetBarPaging(int paramTotalRecordCount)
        {
            var funcName = "SetBarPaging";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            //重新计算[总页数]，设置最新[总记录条数]
            SetTotalPageCountAndTotalRecordCount(paramTotalRecordCount);
            //设置翻页按钮状态
            SetPageButtonStatus();
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ToolBarPaging_ToolClick(object sender, ToolClickEventArgs e)
        {
            var funcName = "ToolBarPaging_ToolClick";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPaging != null)
            {
                switch (e.Tool.Key)
                {
                    //第一页
                    case SysConst.EN_FIRSTPAGE:
                        ((TextBoxTool)(ToolBarPaging.Tools[SysConst.EN_PAGEINDEX])).Text =
                            SysConst.NUMBER_ONE.ToString();
                        break;
                    // 前一页
                    case SysConst.EN_FORWARDPAGE:
                        ((TextBoxTool)(ToolBarPaging.Tools[SysConst.EN_PAGEINDEX])).Text = (PageIndex - 1).ToString();
                        break;
                    // 下一页
                    case SysConst.EN_NEXTPAGE:
                        ((TextBoxTool)(ToolBarPaging.Tools[SysConst.EN_PAGEINDEX])).Text = (PageIndex + 1).ToString();
                        break;
                    // 最后一页
                    case SysConst.EN_LASTPAGE:
                        ((TextBoxTool)(ToolBarPaging.Tools[SysConst.EN_PAGEINDEX])).Text = TotalPageCount.ToString();
                        break;
                }
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }
        /// <summary>
        /// 翻页ToolBar的值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolBarPaging_ToolValueChanged(object sender, ToolEventArgs e)
        {
            var funcName = "ToolBarPaging_ToolValueChanged";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (SysConst.EN_PAGEINDEX.Equals(e.Tool.Key))
            {
                string strValue = ((TextBoxTool)(e.Tool)).Text.Trim();
                int tmpPageIndex = 0;
                if (!int.TryParse(strValue, out tmpPageIndex) && tmpPageIndex <= 0)
                {
                    ((TextBoxTool)(e.Tool)).Text = SysConst.NUMBER_ONE.ToString();
                    ((TextBoxTool)(e.Tool)).SelectionLength = 1;
                    return;
                }
                else if (tmpPageIndex > TotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = TotalPageCount.ToString();
                    ((TextBoxTool)(e.Tool)).SelectionLength = TotalPageCount.ToString().Length;
                    return;
                }

                #region 当前页码设置
                if (Convert.ToInt32(strValue) <= 0)
                {
                    ((TextBoxTool)(e.Tool)).Text = SysConst.NUMBER_ONE.ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) >= TotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(TotalPageCount).ToString();
                }
                else if (Convert.ToInt32(strValue) > 0 && Convert.ToInt32(strValue) < TotalPageCount)
                {
                    ((TextBoxTool)(e.Tool)).Text = Convert.ToInt32(strValue).ToString();
                }
                #endregion

                PageIndex = tmpPageIndex;
                if (tmpPageIndex <= 0)
                {
                    PageIndex = 1;
                }
                ExecuteQuery?.Invoke();
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }
        #endregion

        #region 私有
        /// <summary>
        /// 设置总页数和总记录条数
        /// </summary>
        /// <param name="paramTotalRecordCount">总记录条数</param>
        private void SetTotalPageCountAndTotalRecordCount(int paramTotalRecordCount)
        {
            var funcName = "SetTotalPageCountAndTotalRecordCount";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPaging != null)
            {
                if (paramTotalRecordCount > 0)
                {
                    TotalRecordCount = paramTotalRecordCount;
                    int? remainder = TotalRecordCount % PageSize;
                    if (remainder > 0)
                    {
                        TotalPageCount = TotalRecordCount / PageSize + 1;
                    }
                    else
                    {
                        TotalPageCount = TotalRecordCount / PageSize;
                    }
                }
                else
                {
                    PageIndex = 1;
                    TotalPageCount = 1;
                    TotalRecordCount = 0;
                }
                ((TextBoxTool)(ToolBarPaging.Tools[SysConst.EN_PAGEINDEX])).Text = PageIndex.ToString();
                ToolBarPaging.Tools[SysConst.EN_PAGECOUNT].SharedProps.Caption = TotalPageCount.ToString();
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }

        /// <summary>
        /// 设置翻页按钮状态
        /// </summary>
        private void SetPageButtonStatus()
        {
            var funcName = "SetPageButtonStatus";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (ToolBarPaging != null)
            {
                if (PageIndex == 0 || TotalRecordCount == 0)
                {
                    ToolBarPaging.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;

                    return;
                }
                if (PageIndex == 1 && TotalRecordCount <= PageSize)
                {
                    ToolBarPaging.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (PageIndex == 1 && TotalRecordCount > PageSize)
                {
                    ToolBarPaging.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = false;
                }
                else if (PageIndex != 1 && TotalRecordCount > PageSize * PageIndex)
                {
                    ToolBarPaging.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else if (PageIndex != 1 && TotalRecordCount <= PageSize * PageIndex)
                {
                    ToolBarPaging.Tools[SysConst.EN_NEXTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_LASTPAGE].SharedProps.Enabled = false;
                    ToolBarPaging.Tools[SysConst.EN_FIRSTPAGE].SharedProps.Enabled = true;
                    ToolBarPaging.Tools[SysConst.EN_FORWARDPAGE].SharedProps.Enabled = true;
                }
                else
                {
                    throw (new Exception("非预期的场合。。。"));
                }
            }
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
        }
        #endregion

        #endregion

        /// <summary>
        /// 设置单元格是否可以编辑
        /// </summary>
        private void SetPayConfirmWindowStyle()
        {
            #region 设置Grid数据颜色
            gdGrid.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;

            #endregion
        }

        #endregion
    }
}
