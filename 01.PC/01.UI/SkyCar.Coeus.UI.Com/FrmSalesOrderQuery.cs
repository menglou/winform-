using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Framework.WindowUI;

namespace SkyCar.Coeus.UI.Common
{
    /// <summary>
    /// 销售订单查询
    /// </summary>
    public partial class FrmSalesOrderQuery : BaseForm
    {
        #region 全局变量
        /// <summary>
        /// 销售订单查询BLL
        /// </summary>
        private BLLBase _bll = new BLLBase(Trans.COM);

        /// <summary>
        /// 翻页按钮ToolBar
        /// </summary>
        private UltraToolbarsManager _toolBarPaging = new UltraToolbarsManager();

        /// <summary>
        /// Grid的数据源
        /// </summary>
        List<SalesOrderQueryUIModel> ListGridDS = new List<SalesOrderQueryUIModel>();
        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters = new Dictionary<string, object>();

        /// <summary>
        /// 列表选择模式，默认单选，多选时可以选择多项。
        /// </summary>
        private CustomEnums.CustomeSelectionMode ListSelectionMode { get; set; }

        #region 下拉框数据源
        /// <summary>
        /// 客户类型数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _customerTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 来源类型数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _salesOrderSourceTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 单据状态数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _salesOrderStatusList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 审核状态数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _approvalStatusList = new List<ComComboBoxDataSourceTC>();
        #endregion

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
        /// 选中项的父级值(在两极联动时使用)
        /// </summary>
        public string SelectedItemParentValue { get; set; }

        /// <summary>
        /// 选中项的父级描述(在两极联动时使用)
        /// </summary>
        public string SelectedItemParentText { get; set; }

        /// <summary>
        /// 选中项列表
        /// </summary>
        public List<MDLSD_SalesOrder> SelectedGridList = new List<MDLSD_SalesOrder>();

        #endregion

        #region 系统事件

        /// <summary>
        /// 销售订单查询
        /// </summary>
        /// <param name="paramCustomeSelectionMode">选择模式，默认单选</param>
        /// <param name="paramItemSelectedItemParentValue">默认选中项的父级值</param>
        /// <param name="paramItemSelectedItemParentText">默认选中项的父级描述</param>
        public FrmSalesOrderQuery(CustomEnums.CustomeSelectionMode paramCustomeSelectionMode = CustomEnums.CustomeSelectionMode.Single, string paramItemSelectedItemParentValue = null, string paramItemSelectedItemParentText = null)
        {
            ListSelectionMode = paramCustomeSelectionMode;
            SelectedItemParentValue = paramItemSelectedItemParentValue;
            SelectedItemParentText = paramItemSelectedItemParentText;

            InitializeComponent();
        }
        /// <summary>
        /// 销售订单查询
        /// </summary>
        /// <param name="paramWindowParameters">传入的Dictionary参数</param>
        /// <param name="paramCustomeSelectionMode">选择模式，默认单选</param>
        public FrmSalesOrderQuery(Dictionary<string, object> paramWindowParameters, CustomEnums.CustomeSelectionMode paramCustomeSelectionMode = CustomEnums.CustomeSelectionMode.Single)
        {
            ListSelectionMode = paramCustomeSelectionMode;
            _viewParameters = paramWindowParameters;

            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSalesOrderQuery_Load(object sender, EventArgs e)
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

            #region 处理参数

            if (_viewParameters != null)
            {
                #region 单据状态

                if (_viewParameters.ContainsKey(ComViewParamKey.DocumentStatus.ToString()))
                {
                    _salesOrderStatusList = _viewParameters[ComViewParamKey.DocumentStatus.ToString()] as List<ComComboBoxDataSourceTC>;
                    if (_salesOrderStatusList != null && _salesOrderStatusList.Count > 0)
                    {
                        cbWhere_SO_StatusName.DisplayMember = SysConst.EN_TEXT;
                        cbWhere_SO_StatusName.ValueMember = SysConst.EN_Code;
                        cbWhere_SO_StatusName.DataSource = _salesOrderStatusList;
                        cbWhere_SO_StatusName.DataBind();

                        cbWhere_SO_StatusName.Text = _salesOrderStatusList[0].Text;
                        cbWhere_SO_StatusName.Value = _salesOrderStatusList[0].Code;

                        if (_salesOrderStatusList.Count == 1)
                        {
                            cbWhere_SO_StatusName.Enabled = false;
                        }
                        else
                        {
                            cbWhere_SO_StatusName.Enabled = true;
                        }
                    }
                }

                #endregion

                #region 来源类型

                if (_viewParameters.ContainsKey(ComViewParamKey.SourceType.ToString()))
                {
                    _salesOrderSourceTypeList = _viewParameters[ComViewParamKey.SourceType.ToString()] as List<ComComboBoxDataSourceTC>;
                    if (_salesOrderSourceTypeList != null && _salesOrderSourceTypeList.Count > 0)
                    {
                        cbWhere_SO_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
                        cbWhere_SO_SourceTypeName.ValueMember = SysConst.EN_Code;
                        cbWhere_SO_SourceTypeName.DataSource = _salesOrderSourceTypeList;
                        cbWhere_SO_SourceTypeName.DataBind();

                        cbWhere_SO_SourceTypeName.Text = _salesOrderSourceTypeList[0].Text;
                        cbWhere_SO_SourceTypeName.Value = _salesOrderSourceTypeList[0].Code;

                        if (_salesOrderSourceTypeList.Count == 1)
                        {
                            cbWhere_SO_SourceTypeName.Enabled = false;
                        }
                        else
                        {
                            cbWhere_SO_SourceTypeName.Enabled = true;
                        }
                    }
                }

                #endregion

                #region 审核状态

                if (_viewParameters.ContainsKey(ComViewParamKey.ApprovalStatus.ToString()))
                {
                    _approvalStatusList = _viewParameters[ComViewParamKey.ApprovalStatus.ToString()] as List<ComComboBoxDataSourceTC>;
                    if (_approvalStatusList != null && _approvalStatusList.Count > 0)
                    {
                        cbWhere_SO_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
                        cbWhere_SO_ApprovalStatusName.ValueMember = SysConst.EN_Code;
                        cbWhere_SO_ApprovalStatusName.DataSource = _approvalStatusList;
                        cbWhere_SO_ApprovalStatusName.DataBind();

                        cbWhere_SO_ApprovalStatusName.Text = _approvalStatusList[0].Text;
                        cbWhere_SO_ApprovalStatusName.Value = _approvalStatusList[0].Code;

                        if (_approvalStatusList.Count == 1)
                        {
                            cbWhere_SO_ApprovalStatusName.Enabled = false;
                        }
                        else
                        {
                            cbWhere_SO_ApprovalStatusName.Enabled = true;
                        }
                    }
                }

                #endregion 
            }
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
                        }
                        this.gdGrid.Refresh();
                    }
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
            string tempRowID = e.Cell.Row.Cells[SysConst.RowID].Value != null ? e.Cell.Row.Cells[SysConst.RowID].Value.ToString() : string.Empty;
            foreach (var loopSourceItem in ListGridDS)
            {
                if (loopSourceItem.RowID == tempRowID)
                {
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
            SelectedGridList = new List<MDLSD_SalesOrder>();
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (SelectedGridList.Count == 0)
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0038, new object[] { }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            _bll.QueryForList<SalesOrderQueryUIModel>(SQLID.COMM_SQL26, new SalesOrderQueryQCModel()
            {
                PageSize = PageSize,
                PageIndex = PageIndex,
                //组织
                WHERE_SO_Org_ID = LoginInfoDAX.OrgID,
                //单号
                WHERE_SO_No = txtWhere_SO_No.Text.Trim(),
                //来源类型名称
                WHERE_SO_SourceTypeName = cbWhere_SO_SourceTypeName.Text.Trim(),
                //客户类型名称
                WHERE_SO_CustomerTypeName = cbWhere_SO_CustomerTypeName.Text.Trim(),
                //客户名称
                WHERE_SO_CustomerName = txtWhere_SO_CustomerName.Text.Trim(),
                //单据状态名称
                WHERE_SO_StatusName = cbWhere_SO_StatusName.Text.Trim(),
                //审核状态名称
                WHERE_SO_ApprovalStatusName = cbWhere_SO_ApprovalStatusName.Text.Trim()
            }, ListGridDS);

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
            //单号
            txtWhere_SO_No.Clear();
            //客户名称
            txtWhere_SO_CustomerName.Clear();
            //给 单号 设置焦点
            lblWhere_SO_No.Focus();

            #endregion

            //清空Grid
            ListGridDS = new List<SalesOrderQueryUIModel>();
            gdGrid.DataSource = ListGridDS;
            gdGrid.DataBind();

            #region 初始化下拉框

            if (_customerTypeList.Count == 0)
            {
                cbWhere_SO_CustomerTypeName.Items.Clear();
                //客户类型
                _customerTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.CustomerType);
                cbWhere_SO_CustomerTypeName.DisplayMember = SysConst.EN_TEXT;
                cbWhere_SO_CustomerTypeName.ValueMember = SysConst.EN_Code;
                cbWhere_SO_CustomerTypeName.DataSource = _customerTypeList;
                cbWhere_SO_CustomerTypeName.DataBind();
            }

            if (_salesOrderSourceTypeList.Count == 0)
            {
                cbWhere_SO_SourceTypeName.Items.Clear();
                //来源类型
                _salesOrderSourceTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.SalesOrderSourceType);
                cbWhere_SO_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
                cbWhere_SO_SourceTypeName.ValueMember = SysConst.EN_Code;
                cbWhere_SO_SourceTypeName.DataSource = _salesOrderSourceTypeList;
                cbWhere_SO_SourceTypeName.DataBind();
            }

            if (_salesOrderStatusList.Count == 0)
            {
                cbWhere_SO_StatusName.Items.Clear();
                //单据状态
                _salesOrderStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.SalesOrderStatus);
                cbWhere_SO_StatusName.DisplayMember = SysConst.EN_TEXT;
                cbWhere_SO_StatusName.ValueMember = SysConst.EN_Code;
                cbWhere_SO_StatusName.DataSource = _salesOrderStatusList;
                cbWhere_SO_StatusName.DataBind();
            }

            if (_approvalStatusList.Count == 0)
            {
                cbWhere_SO_ApprovalStatusName.Items.Clear();
                //审核状态
                _approvalStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.ApprovalStatus);
                cbWhere_SO_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
                cbWhere_SO_ApprovalStatusName.ValueMember = SysConst.EN_Code;
                cbWhere_SO_ApprovalStatusName.DataSource = _approvalStatusList;
                cbWhere_SO_ApprovalStatusName.DataBind();
            }

            #endregion
        }

        /// <summary>
        /// 生成选中项的值和描述
        /// </summary>
        void GenerateSelectedValueAndText()
        {
            SelectedGridList = new List<MDLSD_SalesOrder>();
            foreach (var loopSourceItem in ListGridDS)
            {
                if (!loopSourceItem.IsChecked)
                {
                    continue;
                }
                MDLSD_SalesOrder argsSalesOrder = new MDLSD_SalesOrder
                {
                    SO_ID = loopSourceItem.SO_ID,
                    SO_No = loopSourceItem.SO_No,
                    SO_Org_ID = loopSourceItem.SO_Org_ID,
                    SO_SourceTypeCode = loopSourceItem.SO_SourceTypeCode,
                    SO_SourceTypeName = loopSourceItem.SO_SourceTypeName,
                    SO_SourceNo = loopSourceItem.SO_SourceNo,
                    SO_CustomerTypeCode = loopSourceItem.SO_CustomerTypeCode,
                    SO_CustomerTypeName = loopSourceItem.SO_CustomerTypeName,
                    SO_CustomerID = loopSourceItem.SO_CustomerID,
                    SO_CustomerName = loopSourceItem.SO_CustomerName,
                    SO_IsPriceIncludeTax = loopSourceItem.SO_IsPriceIncludeTax,
                    SO_TaxRate = loopSourceItem.SO_TaxRate,
                    SO_TotalTax = loopSourceItem.SO_TotalTax,
                    SO_TotalAmount = loopSourceItem.SO_TotalAmount,
                    SO_TotalNetAmount = loopSourceItem.SO_TotalNetAmount,
                    SO_StatusCode = loopSourceItem.SO_StatusCode,
                    SO_StatusName = loopSourceItem.SO_StatusName,
                    SO_ApprovalStatusCode = loopSourceItem.SO_ApprovalStatusCode,
                    SO_ApprovalStatusName = loopSourceItem.SO_ApprovalStatusName,
                    SO_SalesByID = loopSourceItem.SO_SalesByID,
                    SO_SalesByName = loopSourceItem.SO_SalesByName,
                    SO_Remark = loopSourceItem.SO_Remark,
                    SO_IsValid = loopSourceItem.SO_IsValid,
                    SO_CreatedBy = loopSourceItem.SO_CreatedBy,
                    SO_CreatedTime = loopSourceItem.SO_CreatedTime,
                    SO_UpdatedBy = loopSourceItem.SO_UpdatedBy,
                    SO_UpdatedTime = loopSourceItem.SO_UpdatedTime,
                    SO_VersionNo = loopSourceItem.SO_VersionNo
                };
                SelectedGridList.Add(argsSalesOrder);
            }
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
