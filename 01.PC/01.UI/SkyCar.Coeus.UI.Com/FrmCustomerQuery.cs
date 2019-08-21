using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common.QCModel;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Framework.WindowUI;

namespace SkyCar.Coeus.UI.Common
{
    /// <summary>
    /// 客户查询
    /// </summary>
    public partial class FrmCustomerQuery : BaseForm
    {
        #region 全局变量
        /// <summary>
        /// 客户查询BLL
        /// </summary>
        private BLLBase _bll = new BLLBase(Trans.COM);

        /// <summary>
        /// 翻页按钮ToolBar
        /// </summary>
        private UltraToolbarsManager _toolBarPaging = new UltraToolbarsManager();

        /// <summary>
        /// Grid的数据源
        /// </summary>
        List<CustomerQueryUIModel> ListGridDS = new List<CustomerQueryUIModel>();
        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters = new Dictionary<string, object>();

        /// <summary>
        /// 列表选择模式，默认单选，多选时可以选择多项。
        /// </summary>
        private CustomEnums.CustomeSelectionMode ListSelectionMode { get; set; }

        /// <summary>
        /// 来源画面名称
        /// </summary>
        private ComViewParamValue.CustomerQuerySourForm _sourceFormName;

        #region 下拉框数据源

        /// <summary>
        /// 客户类型（普通客户、一般汽修商户、平台内汽修商）
        /// </summary>
        List<ComComboBoxDataSourceTC> _customerTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 汽修商户
        /// </summary>
        List<MDLPIS_AutoFactoryCustomer> _autoFactoryList = new List<MDLPIS_AutoFactoryCustomer>();
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
        public List<CustomerQueryUIModel> SelectedGridList = new List<CustomerQueryUIModel>();
        #endregion

        #region 系统事件

        /// <summary>
        /// 客户查询
        /// </summary>
        /// <param name="paramCustomeSelectionMode">选择模式，默认单选</param>
        public FrmCustomerQuery(CustomEnums.CustomeSelectionMode paramCustomeSelectionMode = CustomEnums.CustomeSelectionMode.Single)
        {
            ListSelectionMode = paramCustomeSelectionMode;

            InitializeComponent();
        }
        /// <summary>
        /// 客户查询
        /// </summary>
        /// <param name="paramWindowParameters">传入的Dictionary参数</param>
        /// <param name="paramCustomeSelectionMode">选择模式，默认单选</param>
        public FrmCustomerQuery(Dictionary<string, object> paramWindowParameters, CustomEnums.CustomeSelectionMode paramCustomeSelectionMode = CustomEnums.CustomeSelectionMode.Single)
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
        private void FrmCustomerQuery_Load(object sender, EventArgs e)
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
                #region 客户类型

                if (_viewParameters.ContainsKey(ComViewParamKey.CustomerType.ToString()))
                {
                    cbWhere_CustomerType.Text = _viewParameters[ComViewParamKey.CustomerType.ToString()] as string;
                    cbWhere_CustomerType.Enabled = false;
                }

                #endregion

                #region 汽修商户

                if (_viewParameters.ContainsKey(ComViewParamKey.AutoFactoryName.ToString()))
                {
                    mcbWhere_AutoFactoryName.SelectedText = _viewParameters[ComViewParamKey.AutoFactoryName.ToString()] as string;
                    mcbWhere_AutoFactoryName.Enabled = false;
                }
                if (_viewParameters.ContainsKey(ComViewParamKey.AutoFactoryCode.ToString()))
                {
                    mcbWhere_AutoFactoryName.SelectedValue = _viewParameters[ComViewParamKey.AutoFactoryCode.ToString()] as string;
                }

                #endregion

                #region 来源画面名称

                if (_viewParameters.ContainsKey(ComViewParamKey.CustomerQuerySourForm.ToString()))
                {
                    _sourceFormName = (ComViewParamValue.CustomerQuerySourForm)_viewParameters[ComViewParamKey.CustomerQuerySourForm.ToString()];
                }

                #endregion
            }
            #endregion

            if (cbWhere_CustomerType.Text == CustomerTypeEnum.Name.PTNQXSH)
            {
                gdGrid.DisplayLayout.Bands[0].Columns["AutoFactoryName"].Hidden = false;
                gdGrid.DisplayLayout.Bands[0].Columns["AutoFactoryCode"].Hidden = false;
                gdGrid.DisplayLayout.Bands[0].Columns["AutoFactoryOrgCode"].Hidden = false;
            }
            else
            {
                gdGrid.DisplayLayout.Bands[0].Columns["AutoFactoryName"].Hidden = true;
                gdGrid.DisplayLayout.Bands[0].Columns["AutoFactoryCode"].Hidden = true;
                gdGrid.DisplayLayout.Bands[0].Columns["AutoFactoryOrgCode"].Hidden = true;
            }

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
            SelectedGridList = new List<CustomerQueryUIModel>();
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
                //请至少勾选一条数据
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
            if (cbWhere_CustomerType.Text == CustomerTypeEnum.Name.PTNQXSH
                && (string.IsNullOrEmpty(mcbWhere_AutoFactoryName.SelectedText)
                || string.IsNullOrEmpty(mcbWhere_AutoFactoryName.SelectedValue)))
            {
                //请选择汽修商户
                MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.AUTOFACTORY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<CustomerQueryUIModel> resultCustomerList = new List<CustomerQueryUIModel>();
            _bll.QueryForList(SQLID.COMM_SQL11, new CustomerQueryQCModel
            {
                PageSize = PageSize,
                PageIndex = PageIndex,
                //客户类型
                WHERE_CustomerType = cbWhere_CustomerType.Text.Trim(),
                //客户名称
                WHERE_CustomerName = txtWhere_CustomerName.Text.Trim(),
                //汽修商户名称
                WHERE_AutoFactoryName = mcbWhere_AutoFactoryName.SelectedText,
                //组织ID
                WHERE_OrgID = LoginInfoDAX.UserID == SysConst.SUPER_ADMIN ? null : LoginInfoDAX.OrgID,
            }, resultCustomerList);

            if (cbWhere_CustomerType.Text == CustomerTypeEnum.Name.PTNQXSH)
            {
                ListGridDS = new List<CustomerQueryUIModel>();

                //根据指定的汽修商户数据库信息获取Venus组织列表
                List<MDLSM_Organization> tempVenusOrgList = new List<MDLSM_Organization>();
                BLLCom.QueryAutoFactoryCustomerOrgList(mcbWhere_AutoFactoryName.SelectedValue, tempVenusOrgList);

                foreach (var loopVenusOrg in tempVenusOrgList)
                {
                    foreach (var loopCustomer in resultCustomerList)
                    {
                        if (loopCustomer.AutoFactoryOrgCode == loopVenusOrg.Org_Code
                            && loopCustomer.CustomerName == loopVenusOrg.Org_ShortName)
                        {
                            CustomerQueryUIModel authorityVenusOrg = new CustomerQueryUIModel()
                            {
                                CustomerType = loopCustomer.CustomerType,
                                CustomerID = loopCustomer.CustomerID,
                                CustomerName = loopCustomer.CustomerName,
                                AutoFactoryCode = loopCustomer.AutoFactoryCode,
                                AutoFactoryName = loopCustomer.AutoFactoryName,
                                AutoFactoryOrgID = loopVenusOrg.Org_ID,
                                AutoFactoryOrgCode = loopCustomer.AutoFactoryOrgCode,
                                CustomerPhoneNo = loopCustomer.CustomerPhoneNo,
                                CustomerAddress = loopCustomer.CustomerAddress,
                                CreditAmount = loopCustomer.CreditAmount,
                                DebtAmount = loopCustomer.DebtAmount,
                                PaymentTypeCode = loopCustomer.PaymentTypeCode,
                                PaymentTypeName = loopCustomer.PaymentTypeName,
                                BillingTypeCode = loopCustomer.BillingTypeCode,
                                BillingTypeName = loopCustomer.BillingTypeName,
                                DeliveryTypeCode = loopCustomer.DeliveryTypeCode,
                                DeliveryTypeName = loopCustomer.DeliveryTypeName,
                                DeliveryByID = loopCustomer.DeliveryByID,
                                DeliveryByName = loopCustomer.DeliveryByName,
                                DeliveryByPhone = loopCustomer.DeliveryByPhone,
                                IsEndSales = loopCustomer.IsEndSales,
                                AutoPartsPriceType = loopCustomer.AutoPartsPriceType,
                                Wal_ID = loopCustomer.Wal_ID,
                                Wal_No = loopCustomer.Wal_No,
                                Wal_Org_ID = loopCustomer.Wal_Org_ID,
                                Wal_Org_Name = loopCustomer.Wal_Org_Name,
                                Wal_AvailableBalance = loopCustomer.Wal_AvailableBalance,
                                RowID = loopCustomer.RowID,
                                RecordCount = loopCustomer.RecordCount,
                                //汽修商户组织信息
                                AutoFactoryOrgInfo = loopCustomer.CustomerID + SysConst.Semicolon_DBC + loopVenusOrg.Org_ID + SysConst.Semicolon_DBC + loopCustomer.AutoFactoryOrgCode,
                            };
                            ListGridDS.Add(authorityVenusOrg);
                        }
                    }
                }
            }
            else
            {
                _bll.CopyModelList(resultCustomerList, ListGridDS);
            }

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

            if (cbWhere_CustomerType.Enabled)
            {
                //客户类别
                cbWhere_CustomerType.Items.Clear();
            }
            if (mcbWhere_AutoFactoryName.Enabled)
            {
                //汽修商户名称
                mcbWhere_AutoFactoryName.Clear();
            }
            //客户名称
            txtWhere_CustomerName.Clear();

            lblWhere_CustomerName.Focus();
            #endregion

            //清空Grid
            ListGridDS = new List<CustomerQueryUIModel>();
            gdGrid.DataSource = ListGridDS;
            gdGrid.DataBind();

            #region 初始化下拉框

            if (cbWhere_CustomerType.Enabled)
            {
                //客户类型（普通客户、一般汽修商户、平台内汽修商）
                _customerTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.CustomerType);
                cbWhere_CustomerType.DisplayMember = SysConst.EN_TEXT;
                cbWhere_CustomerType.ValueMember = SysConst.EN_Code;
                cbWhere_CustomerType.DataSource = _customerTypeList;
                cbWhere_CustomerType.DataBind();
            }

            //汽修商户名称
            _autoFactoryList = CacheDAX.Get(CacheDAX.ConfigDataKey.ARMerchant) as List<MDLPIS_AutoFactoryCustomer>;
            if (_autoFactoryList != null)
            {
                var autoFactoryList = _autoFactoryList.Where(x => x.AFC_IsPlatform == true).ToList();
                mcbWhere_AutoFactoryName.DisplayMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Name;
                mcbWhere_AutoFactoryName.ValueMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Code;
                if (mcbWhere_AutoFactoryName.Enabled)
                {
                    mcbWhere_AutoFactoryName.DataSource = autoFactoryList;
                }
            }

            #endregion
        }

        /// <summary>
        /// 生成选中项的值和描述
        /// </summary>
        void GenerateSelectedValueAndText()
        {
            SelectedGridList = new List<CustomerQueryUIModel>();
            foreach (var loopSourceItem in ListGridDS)
            {
                if (!loopSourceItem.IsChecked)
                {
                    continue;
                }
                if (_sourceFormName == ComViewParamValue.CustomerQuerySourForm.SD_FrmSalesOrderManager
                    || _sourceFormName == ComViewParamValue.CustomerQuerySourForm.SD_FrmProactiveSalesManager)
                {
                    //来源画面是[销售订单管理]、[主动销售管理]的场合
                    if (loopSourceItem.IsEndSales == true)
                    {
                        //客户loopSourceItem.CustomerName已终止销售，不能创建销售订单
                        MessageBoxs.Show(Trans.COM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0039, new object[] { loopSourceItem.CustomerName, SystemTableEnums.Name.SD_SalesOrder }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loopSourceItem.IsChecked = false;
                        gdGrid.DataSource = ListGridDS;
                        gdGrid.DataBind();
                    }
                }
                if (loopSourceItem.IsChecked)
                {
                    CustomerQueryUIModel argsCustomerQuery = new CustomerQueryUIModel
                    {
                        CustomerType = loopSourceItem.CustomerType,
                        CustomerID = loopSourceItem.CustomerID,
                        CustomerName = loopSourceItem.CustomerName,
                        AutoFactoryCode = loopSourceItem.AutoFactoryCode,
                        AutoFactoryName = loopSourceItem.AutoFactoryName,
                        AutoFactoryOrgCode = loopSourceItem.AutoFactoryOrgCode,
                        AutoFactoryOrgID = loopSourceItem.AutoFactoryOrgID,
                        CustomerPhoneNo = loopSourceItem.CustomerPhoneNo,
                        CustomerAddress = loopSourceItem.CustomerAddress,
                        CreditAmount = loopSourceItem.CreditAmount,
                        DebtAmount = loopSourceItem.DebtAmount,
                        PaymentTypeCode = loopSourceItem.PaymentTypeCode,
                        PaymentTypeName = loopSourceItem.PaymentTypeName,
                        BillingTypeCode = loopSourceItem.BillingTypeCode,
                        BillingTypeName = loopSourceItem.BillingTypeName,
                        DeliveryTypeCode = loopSourceItem.DeliveryTypeCode,
                        DeliveryTypeName = loopSourceItem.DeliveryTypeName,
                        DeliveryByID = loopSourceItem.DeliveryByID,
                        DeliveryByName = loopSourceItem.DeliveryByName,
                        IsEndSales = loopSourceItem.IsEndSales,
                        AutoPartsPriceType = loopSourceItem.AutoPartsPriceType,

                        Wal_ID = loopSourceItem.Wal_ID,
                        Wal_No = loopSourceItem.Wal_No,
                        Wal_Org_ID = loopSourceItem.Wal_Org_ID,
                        Wal_Org_Name = loopSourceItem.Wal_Org_Name,
                        Wal_AvailableBalance = loopSourceItem.Wal_AvailableBalance,

                        //汽修商户组织信息
                        AutoFactoryOrgInfo = loopSourceItem.CustomerID + SysConst.Semicolon_DBC + loopSourceItem.AutoFactoryOrgID
                        + SysConst.Semicolon_DBC + loopSourceItem.AutoFactoryOrgCode,
                    };
                    SelectedGridList.Add(argsCustomerQuery);
                }
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
