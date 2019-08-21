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
    /// 付款单查询
    /// </summary>
    public partial class FrmPayBillQuery : BaseForm
    {
        #region 全局变量
        /// <summary>
        /// 付款单查询BLL
        /// </summary>
        private BLLBase _bll = new BLLBase(Trans.COM);

        /// <summary>
        /// 翻页按钮ToolBar
        /// </summary>
        private UltraToolbarsManager _toolBarPaging = new UltraToolbarsManager();

        /// <summary>
        /// Grid的数据源
        /// </summary>
        List<PayBillQueryUIModel> ListGridDS = new List<PayBillQueryUIModel>();

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
        /// 来源类型数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _PayBillDetailSourceTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 收款对象类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _recObjectTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 单据状态
        /// </summary>
        List<ComComboBoxDataSourceTC> _payBillStatusList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 审核状态
        /// </summary>
        List<ComComboBoxDataSourceTC> _approveStatusList = new List<ComComboBoxDataSourceTC>();
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
        /// 选中项列表
        /// </summary>
        public List<MDLFM_PayBill> SelectedGridList = new List<MDLFM_PayBill>();
        #endregion

        #region 系统事件

        /// <summary>
        /// 付款单查询
        /// </summary>
        /// <param name="paramCustomeSelectionMode">选择模式，默认单选</param>
        public FrmPayBillQuery(CustomEnums.CustomeSelectionMode paramCustomeSelectionMode = CustomEnums.CustomeSelectionMode.Single)
        {
            ListSelectionMode = paramCustomeSelectionMode;

            InitializeComponent();
        }
        /// <summary>
        /// 付款单查询
        /// </summary>
        /// <param name="paramWindowParameters">传入的Dictionary参数</param>
        /// <param name="paramCustomeSelectionMode">选择模式，默认单选</param>
        public FrmPayBillQuery(Dictionary<string, object> paramWindowParameters, CustomEnums.CustomeSelectionMode paramCustomeSelectionMode = CustomEnums.CustomeSelectionMode.Single)
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
        private void FrmPayBillQuery_Load(object sender, EventArgs e)
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
                #region 来源类型

                if (_viewParameters.ContainsKey(ComViewParamKey.SourceType.ToString()))
                {
                    _PayBillDetailSourceTypeList = _viewParameters[ComViewParamKey.SourceType.ToString()] as List<ComComboBoxDataSourceTC>;
                    if (_PayBillDetailSourceTypeList != null && _PayBillDetailSourceTypeList.Count > 0)
                    {
                        cbWhere_PB_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
                        cbWhere_PB_SourceTypeName.ValueMember = SysConst.EN_Code;
                        cbWhere_PB_SourceTypeName.DataSource = _PayBillDetailSourceTypeList;
                        cbWhere_PB_SourceTypeName.DataBind();

                        cbWhere_PB_SourceTypeName.Text = _PayBillDetailSourceTypeList[0].Text;
                        cbWhere_PB_SourceTypeName.Value = _PayBillDetailSourceTypeList[0].Code;

                        if (_PayBillDetailSourceTypeList.Count == 1)
                        {
                            cbWhere_PB_SourceTypeName.Enabled = false;
                        }
                        else
                        {
                            cbWhere_PB_SourceTypeName.Enabled = true;
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
            SelectedGridList = new List<MDLFM_PayBill>();
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
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0038, new object[] { }), MessageBoxButtons.OK, MessageBoxIcon.Information );
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
            _bll.QueryForList<PayBillQueryUIModel>(SQLID.COMM_SQL27, new PayBillQueryQCModel()
            {
                PageSize = PageSize,
                PageIndex = PageIndex,
                //付款组织
                WHERE_PB_Pay_Org_ID = LoginInfoDAX.OrgID,
                //单号
                WHERE_PB_No = txtWhere_PB_No.Text.Trim(),
                //来源类型名称
                //WHERE_PB_SourceTypeName = txtWhere_PB_RecObjectName.Text.Trim(),
                //业务状态名称
                WHERE_PB_BusinessStatusName = cbWhere_PB_BusinessStatusName.Text.Trim(),
                //审核状态名称
                WHERE_PB_ApprovalStatusName = cbWhere_PB_BusinessStatusName.Text.Trim(),
                //收款对象类型名称
                WHERE_PB_RecObjectTypeName = cbWhere_PB_BusinessStatusName.Text.Trim(),
                //收款对象
                WHERE_PB_RecObjectName = txtWhere_PB_RecObjectName.Text.Trim(),
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
            txtWhere_PB_No.Clear();
            //收款对象
            txtWhere_PB_RecObjectName.Clear();
            //给 单号 设置焦点
            lblWhere_PB_No.Focus();

            #endregion

            //清空Grid
            ListGridDS = new List<PayBillQueryUIModel>();
            gdGrid.DataSource = ListGridDS;
            gdGrid.DataBind();

            #region 下拉框初始化

            if (cbWhere_PB_BusinessStatusName.Enabled)
            {
                //收款对象类型
                cbWhere_PB_RecObjectTypeName.Items.Clear();
                //收款对象类型
                _recObjectTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.AmountTransObjectType);
                cbWhere_PB_BusinessStatusName.DisplayMember = SysConst.EN_TEXT;
                cbWhere_PB_BusinessStatusName.ValueMember = SysConst.EN_Code;
                cbWhere_PB_BusinessStatusName.DataSource = _recObjectTypeList;
                cbWhere_PB_BusinessStatusName.DataBind();
            }

            if (cbWhere_PB_BusinessStatusName.Enabled)
            {
                //业务状态
                cbWhere_PB_BusinessStatusName.Items.Clear();
                //单据状态
                _payBillStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.PayBillStatus);
                cbWhere_PB_BusinessStatusName.DisplayMember = SysConst.EN_TEXT;
                cbWhere_PB_BusinessStatusName.ValueMember = SysConst.EN_Code;
                cbWhere_PB_BusinessStatusName.DataSource = _payBillStatusList;
                cbWhere_PB_BusinessStatusName.DataBind();
            }

            if (cbWhere_PB_SourceTypeName.Enabled)
            {
                //来源类型
                cbWhere_PB_SourceTypeName.Items.Clear();
                //来源类型
                _PayBillDetailSourceTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.PayBillDetailSourceType);
                cbWhere_PB_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
                cbWhere_PB_SourceTypeName.ValueMember = SysConst.EN_Code;
                cbWhere_PB_SourceTypeName.DataSource = _PayBillDetailSourceTypeList;
                cbWhere_PB_SourceTypeName.DataBind();
            }

            if (cbWhere_PB_ApprovalStatusName.Enabled)
            {
                //审核状态
                cbWhere_PB_ApprovalStatusName.Items.Clear();
                //审核状态
                _approveStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.ApprovalStatus);
                cbWhere_PB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
                cbWhere_PB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
                cbWhere_PB_ApprovalStatusName.DataSource = _approveStatusList;
                cbWhere_PB_ApprovalStatusName.DataBind();
            }

            #endregion
        }

        /// <summary>
        /// 生成选中项的值和描述
        /// </summary>
        void GenerateSelectedValueAndText()
        {
            SelectedGridList = new List<MDLFM_PayBill>();
            foreach (var loopSourceItem in ListGridDS)
            {
                if (!loopSourceItem.IsChecked)
                {
                    continue;
                }
                MDLFM_PayBill argsPayBill = new MDLFM_PayBill
                {
                    PB_ID = loopSourceItem.PB_ID,
                    PB_No = loopSourceItem.PB_No,
                    //PB_SourceTypeCode = loopSourceItem.PB_SourceTypeCode,
                    //PB_SourceTypeName = loopSourceItem.PB_SourceTypeName,
                    //PB_SrcBillNo = loopSourceItem.PB_SrcBillNo,
                    PB_Pay_Org_ID = loopSourceItem.PB_Pay_Org_ID,
                    PB_Pay_Org_Name = loopSourceItem.PB_Pay_Org_Name,
                    PB_Date = loopSourceItem.PB_Date,
                    PB_RecObjectTypeCode = loopSourceItem.PB_RecObjectTypeCode,
                    PB_RecObjectTypeName = loopSourceItem.PB_RecObjectTypeName,
                    PB_RecObjectID = loopSourceItem.PB_RecObjectID,
                    PB_RecObjectName = loopSourceItem.PB_RecObjectName,
                    PB_PayableTotalAmount = loopSourceItem.PB_PayableTotalAmount,
                    PB_RealPayableTotalAmount = loopSourceItem.PB_RealPayableTotalAmount,
                    PB_PayAccount = loopSourceItem.PB_PayAccount,
                    PB_RecAccount = loopSourceItem.PB_RecAccount,
                    PB_PayTypeCode = loopSourceItem.PB_PayTypeCode,
                    PB_PayTypeName = loopSourceItem.PB_PayTypeName,
                    PB_CertificateNo = loopSourceItem.PB_CertificateNo,
                    PB_CertificatePic = loopSourceItem.PB_CertificatePic,
                    PB_BusinessStatusCode = loopSourceItem.PB_BusinessStatusCode,
                    PB_BusinessStatusName = loopSourceItem.PB_BusinessStatusName,
                    PB_ApprovalStatusCode = loopSourceItem.PB_ApprovalStatusCode,
                    PB_ApprovalStatusName = loopSourceItem.PB_ApprovalStatusName,
                    PB_Remark = loopSourceItem.PB_Remark,
                    PB_IsValid = loopSourceItem.PB_IsValid,
                    PB_CreatedBy = loopSourceItem.PB_CreatedBy,
                    PB_CreatedTime = loopSourceItem.PB_CreatedTime,
                    PB_UpdatedBy = loopSourceItem.PB_UpdatedBy,
                    PB_UpdatedTime = loopSourceItem.PB_UpdatedTime,
                    PB_VersionNo = loopSourceItem.PB_VersionNo
                };
                SelectedGridList.Add(argsPayBill);
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
