using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.COM;
using SkyCar.Coeus.BLL.SD;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.SD;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Coeus.UIModel.SD.APModel;
using SkyCar.Coeus.UIModel.SD.UIModel;
using System.Collections.ObjectModel;

namespace SkyCar.Coeus.UI.SD
{
    /// <summary>
    /// 销售订单管理
    /// </summary>
    public partial class FrmSalesOrderManager : BaseFormCardListDetail<SalesOrderManagerUIModel, SalesOrderManagerQCModel, MDLSD_SalesOrder>
    {
        #region 全局变量

        /// <summary>
        /// 销售订单管理BLL
        /// </summary>
        private SalesOrderManagerBLL _bll = new SalesOrderManagerBLL();

        #region Grid数据源

        /// <summary>
        /// 销售订单明细数据源
        /// </summary>
        private SkyCarBindingList<SalesOrderDetailUIModel, MDLSD_SalesOrderDetail> _detailGridDS = new SkyCarBindingList<SalesOrderDetailUIModel, MDLSD_SalesOrderDetail>();
        /// <summary>
        /// 出库明细数据源
        /// </summary>
        private List<SalesStockOutDetailUIModel> _stockOutDetailDS = new List<SalesStockOutDetailUIModel>();

        #endregion

        #region 下拉框数据源

        /// <summary>
        /// 销售订单来源类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _salesOrderSourceTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 客户类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _customerTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 销售订单单据状态
        /// </summary>
        List<ComComboBoxDataSourceTC> _salesOrderStatusList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 审核状态
        /// </summary>
        List<ComComboBoxDataSourceTC> _approveStatusList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 汽修商户
        /// </summary>
        List<MDLPIS_AutoFactoryCustomer> _autoFactoryList = new List<MDLPIS_AutoFactoryCustomer>();
        /// <summary>
        /// 配件价格类别
        /// </summary>
        ObservableCollection<CodeTableValueTextModel> _autoPartsPriceTypeList = new ObservableCollection<CodeTableValueTextModel>();
        /// <summary>
        /// 所有客户数据源
        /// </summary>
        List<ComClientUIModel> _tempAllClientList = new List<ComClientUIModel>();
        /// <summary>
        /// 【详情】客户数据源
        /// </summary>
        List<ComClientUIModel> _clientList = new List<ComClientUIModel>();
        /// <summary>
        /// 【列表】客户数据源
        /// </summary>
        List<ComClientUIModel> _whereClientList = new List<ComClientUIModel>();
        /// <summary>
        /// 业务员数据源
        /// </summary>
        List<MDLSM_User> _salesByList = new List<MDLSM_User>();

        #endregion

        /// <summary>
        /// 是否启用进销存模块
        /// </summary>
        private bool _isHasInventory = false;
        /// <summary>
        /// 客户配置的配件价格类别
        /// </summary>
        private string _autoPartsPriceTypeOfCustomer;

        /// <summary>
        /// 客户组织ID
        /// </summary>
        private string _clientOrgID = string.Empty;

        /// <summary>
        /// 客户组织编码
        /// </summary>
        private string _clientOrgCode = string.Empty;

        /// <summary>
        /// 异动类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _inventoryTransTypeList = new List<ComComboBoxDataSourceTC>();

        /// <summary>
        /// 添加销售明细Func
        /// </summary>
        private Func<List<PickPartsQueryUIModel>, bool> _pickPartsDetailFunc;

        /// <summary>
        /// 界面属性值发生变化时不予检查的属性列表
        /// </summary>
        List<string> _skipPropertyList = new List<string>();

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmSalesOrderManager构造方法
        /// </summary>
        public FrmSalesOrderManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSalesOrderManager_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（动作）
            base.ToolBarActionAndNavigate = toolBarActionAndNavigate;
            //基类.工具栏（翻页）
            base.ToolBarPaging = toolBarPaging;
            //查询委托（基类控制翻页用）
            base.ExecuteQuery = QueryAction;
            //工具栏（动作）单击事件
            this.toolBarActionAndNavigate.ToolClick += new ToolClickEventHandler(base.toolBarActionAndNavigate_ToolClick);
            //工具栏（翻页）单击事件
            this.toolBarPaging.ToolClick += new ToolClickEventHandler(base.ToolBarPaging_ToolClick);
            //工具栏（翻页）[当前页]值改变事件
            this.toolBarPaging.ToolValueChanged += new ToolEventHandler(base.ToolBarPaging_ToolValueChanged);

            #region 设置页面大小文本框
            TextBoxTool pageSizeOfList = null;
            foreach (var loopToolControl in this.toolBarPaging.Tools)
            {
                if (loopToolControl.Key == SysConst.EN_PAGESIZE)
                {
                    pageSizeOfList = (TextBoxTool)loopToolControl;
                }
            }
            if (pageSizeOfList != null)
            {
                pageSizeOfList.Text = PageSize.ToString();
                pageSizeOfList.AfterToolExitEditMode += PageSizeTextBoxTool_AfterToolExitEditMode;
            }
            #endregion

            #region 客户

            _tempAllClientList = BLLCom.GetAllCustomerList(LoginInfoDAX.OrgID);
            _clientList = new List<ComClientUIModel>();
            foreach (var loopClient in _tempAllClientList)
            {
                //不显示其他组织的客户
                if (!string.IsNullOrEmpty(loopClient.OrgID) && loopClient.OrgID != LoginInfoDAX.OrgID)
                {
                    continue;
                }
                if (loopClient.ClientType == AmountTransObjectTypeEnum.Name.REGULARCUSTOMER)
                {
                    loopClient.ClientType = CustomerTypeEnum.Name.PTKH;
                    _whereClientList.Add(loopClient);

                    //过滤[终止销售]的客户
                    if (loopClient.IsEndSales != true)
                    {
                        _clientList.Add(loopClient);
                    }
                }
                else if (loopClient.ClientType == AmountTransObjectTypeEnum.Name.GENERALAUTOFACTORY)
                {
                    loopClient.ClientType = CustomerTypeEnum.Name.YBQXSH;
                    _whereClientList.Add(loopClient);

                    //过滤[终止销售]的客户
                    if (loopClient.IsEndSales != true)
                    {
                        _clientList.Add(loopClient);
                    }
                }
                else if (loopClient.ClientType == AmountTransObjectTypeEnum.Name.PLATFORMAUTOFACTORY)
                {
                    loopClient.ClientType = CustomerTypeEnum.Name.PTNQXSH;
                    _whereClientList.Add(loopClient);

                    //过滤[终止销售]的客户
                    if (loopClient.IsEndSales != true)
                    {
                        _clientList.Add(loopClient);
                    }
                }
            }
            mcbClientName.ExtraDisplayMember = "ClientType";
            mcbClientName.DisplayMember = "ClientName";
            mcbClientName.ValueMember = "ClientID";
            mcbClientName.DataSource = _clientList;

            mcbWhereClientName.ExtraDisplayMember = "ClientType";
            mcbWhereClientName.DisplayMember = "ClientName";
            mcbWhereClientName.ValueMember = "ClientID";
            mcbWhereClientName.DataSource = _whereClientList;

            #endregion

            //是否启用进销存模块
            _isHasInventory = AutoPartsComFunction.IsHasInventory();

            //初始化【详情】Tab内控件
            InitializeDetailTabControls();
            //初始化【列表】Tab内控件
            InitializeListTabControls();

            //设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //根据选中的Tab，设置动作按钮[是否可用]（在系统权限的基础上进行控制）
            base.SetActionEnableBySelectedTab(SysConst.EN_LIST);
            #endregion

            //[列表]页不允许删除
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
            }
            //[打印]不可用
            SetActionEnable(SystemActionEnum.Code.PRINT, false);

            _pickPartsDetailFunc = HandlePickPartsDetail;

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            AcceptUIModelChanges();

            #region 界面发生变化时不予检查的属性值

            _skipPropertyList.Add("AutoFactoryCode");
            _skipPropertyList.Add("AutoFactoryName");
            _skipPropertyList.Add("AROrgID");
            _skipPropertyList.Add("AROrgCode");
            _skipPropertyList.Add("AROrgName");
            _skipPropertyList.Add("AROrgPhone");
            _skipPropertyList.Add("AROrgAddress");
            _skipPropertyList.Add("AutoPartsPriceType");
            _skipPropertyList.Add("AccountReceivableAmount");
            _skipPropertyList.Add("ReceivedAmount");
            _skipPropertyList.Add("UnReceiveAmount");
            _skipPropertyList.Add("CreditAmount");
            _skipPropertyList.Add("DebtAmount");
            #endregion
        }

        /// <summary>
        /// 页面大小失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageSizeTextBoxTool_AfterToolExitEditMode(object sender, AfterToolExitEditModeEventArgs e)
        {
            if (!SysConst.EN_PAGESIZE.Equals(e.Tool.Key))
            {
                return;
            }
            string strValue = ((TextBoxTool)(e.Tool)).Text.Trim();
            int tmpPageSize = 0;
            if (!int.TryParse(strValue, out tmpPageSize) || tmpPageSize == 0)
            {
                ((TextBoxTool)(e.Tool)).Text = SysConst.DEFAULT_PAGESIZE.ToString();
                return;
            }
            if (tmpPageSize > SysConst.MAX_PAGESIZE)
            {
                ((TextBoxTool)(e.Tool)).Text = SysConst.MAX_PAGESIZE.ToString();
                return;
            }

            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                PageSize = tmpPageSize;
            }
            else
            {

            }

            ExecuteQuery?.Invoke();
        }

        #region 【列表】Grid相关事件
        
        /// <summary>
        /// 【列表】Grid的Cell的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            //将Grid中选中的数据赋值给【详情】Tab内的对应控件
            SetGridDataToCardCtrls();
        }
        /// <summary>
        /// 【列表】Grid的CellChange事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_CellChange(object sender, CellEventArgs e)
        {
            gdGrid.UpdateData();
            decimal totalAmount = 0;
            decimal accountReceivableAmount = 0;
            decimal receivedAmount = 0;

            foreach (var loopGridRow in gdGrid.Rows)
            {
                if (string.IsNullOrEmpty(loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrder.Code.SO_No].Value?.ToString())
                    || loopGridRow.Cells["IsChecked"].Value == null
                    || (bool)loopGridRow.Cells["IsChecked"].Value == false)
                {
                    continue;
                }
                totalAmount = totalAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrder.Code.SO_TotalAmount].Value?.ToString());
                accountReceivableAmount = accountReceivableAmount + Convert.ToDecimal(loopGridRow.Cells["AccountReceivableAmount"].Value?.ToString());
                receivedAmount = receivedAmount + Convert.ToDecimal(loopGridRow.Cells["ReceivedAmount"].Value?.ToString());
            }

            ((TextBoxTool)(toolBarPaging.Tools["txtTotalAmount"])).Text = Convert.ToString(totalAmount);
            ((TextBoxTool)(toolBarPaging.Tools["txtAccountReceivableAmount"])).Text = Convert.ToString(accountReceivableAmount);
            ((TextBoxTool)(toolBarPaging.Tools["txtReceivedAmount"])).Text = Convert.ToString(receivedAmount);
        }
        
        /// <summary>
        /// 【列表】Grid的AfterHeaderCheckStateChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            gdGrid.UpdateData();
            decimal totalAmount = 0;
            decimal accountReceivableAmount = 0;
            decimal receivedAmount = 0;

            foreach (var loopGridRow in gdGrid.Rows)
            {
                if (string.IsNullOrEmpty(loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrder.Code.SO_No].Value?.ToString())
                    || loopGridRow.Cells["IsChecked"].Value == null
                    || (bool)loopGridRow.Cells["IsChecked"].Value == false)
                {
                    continue;
                }
                totalAmount = totalAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrder.Code.SO_TotalAmount].Value?.ToString());
                accountReceivableAmount = accountReceivableAmount + Convert.ToDecimal(loopGridRow.Cells["AccountReceivableAmount"].Value?.ToString());
                receivedAmount = receivedAmount + Convert.ToDecimal(loopGridRow.Cells["ReceivedAmount"].Value?.ToString());
            }

            ((TextBoxTool)(toolBarPaging.Tools["txtTotalAmount"])).Text = Convert.ToString(totalAmount);
            ((TextBoxTool)(toolBarPaging.Tools["txtAccountReceivableAmount"])).Text = Convert.ToString(accountReceivableAmount);
            ((TextBoxTool)(toolBarPaging.Tools["txtReceivedAmount"])).Text = Convert.ToString(receivedAmount);
        }

        #endregion

        #region Tab改变事件相关事件

        /// <summary>
        /// 选中的Tab改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlFull_SelectedTabChanging(object sender, SelectedTabChangingEventArgs e)
        {
            base.SetActionEnableBySelectedTab(e.Tab.Key);
        }
        /// <summary>
        /// 选中的Tab改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlFull_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            //[列表]页不允许删除
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                //选中【列表】Tab的场合
                //[删除]不可用
                SetActionEnable(SystemActionEnum.Code.DELETE, false);

                //[转物流]、[发起支付]、[转结算]按钮可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOLOGISTICSBILL, true, true);
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.ONLINEPAY, true, true);
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, true);
            }
            else
            {
                //选中【详情】Tab的场合
                //设置动作按钮状态
                SetActionEnableByStatus();
            }
        }
        #endregion

        #region 查询条件相关事件

        /// <summary>
        /// 单据编号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_SO_No_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }

        /// <summary>
        /// 来源类型ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_SO_SourceTypeCode_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 单据状态ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_SO_StatusCode_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 审核状态ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_SO_ApprovalStatusCode_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }

        /// <summary>
        /// 【列表】选择客户名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWhereClientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(mcbWhereClientName.SelectedText))
            {
                ComClientUIModel selectedClient = null;
                foreach (var loopClient in _whereClientList)
                {
                    if (loopClient.ClientType == mcbWhereClientName.SelectedTextExtra &&
                        loopClient.ClientName == mcbWhereClientName.SelectedText)
                    {
                        selectedClient = loopClient;
                        break;
                    }
                }
                if (selectedClient != null)
                {
                    this.mcbWhereClientType.SelectedText = selectedClient.ClientType;
                }
            }
        }

        /// <summary>
        /// [创建时间-终了]ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_SalesOrderCreateEnd_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtWhere_SalesOrderCreateEnd.Value != null &&
                this.dtWhere_SalesOrderCreateEnd.DateTime.Hour == 0 &&
                this.dtWhere_SalesOrderCreateEnd.DateTime.Minute == 0 &&
                this.dtWhere_SalesOrderCreateEnd.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_SalesOrderCreateEnd.DateTime.Year, this.dtWhere_SalesOrderCreateEnd.DateTime.Month, this.dtWhere_SalesOrderCreateEnd.DateTime.Day, 23, 59, 59);
                this.dtWhere_SalesOrderCreateEnd.DateTime = newDateTime;
            }
        }
        #endregion

        #region 单头相关事件

        /// <summary>
        /// 【详情】来源类型ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSO_SourceTypeCode_ValueChanged(object sender, EventArgs e)
        {
            if (cbSO_SourceTypeCode.Value == null)
            {
                return;
            }

            txtSO_SourceNo.Clear();

            if (cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.SGCJ
                || cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.ZXXS)
            {
                //来源类型为[手工创建]或[在线销售]的场合，无[来源单号]
                lblSO_SourceNo.Visible = false;
                txtSO_SourceNo.Visible = false;
            }
            else
            {
                //来源类型为[主动销售]或[销售预测]的场合，有[来源单号]
                lblSO_SourceNo.Visible = true;
                txtSO_SourceNo.Visible = true;
            }
        }

        /// <summary>
        /// 【详情】选择客户名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbClientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //所有人类别为{普通客户}、{一般汽修商}的场合，不显示[汽修商户]
            lblAutoFactoryName.Visible = false;
            mcbAutoFactoryName.Visible = false;

            mcbClientType.Clear();
            mcbAutoFactoryName.Clear();
            _clientOrgID = string.Empty;
            _clientOrgCode = string.Empty;
            txtDebtAmount.Text = string.Empty;

            if (!string.IsNullOrEmpty(mcbClientName.SelectedText))
            {
                ComClientUIModel selectedClient = null;
                foreach (var loopClient in _clientList)
                {
                    if (loopClient.ClientType == mcbClientName.SelectedTextExtra &&
                        loopClient.ClientName == mcbClientName.SelectedText)
                    {
                        selectedClient = loopClient;
                        break;
                    }
                }
                if (selectedClient != null)
                {
                    this.mcbClientType.SelectedText = selectedClient.ClientType;
                    this.mcbAutoPartsPriceType.SelectedText = selectedClient.AutoPartsPriceType;
                    this.mcbAutoPartsPriceType.Enabled = string.IsNullOrEmpty(this.mcbAutoPartsPriceType.SelectedText);
                    this.mcbAutoPartsPriceType.IsReadOnly = !string.IsNullOrEmpty(this.mcbAutoPartsPriceType.SelectedText);
                    if (this.mcbAutoPartsPriceType.Enabled)
                    {
                        this.mcbAutoPartsPriceType.Clear();
                    }
                    if (selectedClient.ClientType == CustomerTypeEnum.Name.PTNQXSH)
                    {
                        //所有人类别为{平台内汽修商}的场合，显示[汽修商户]
                        lblAutoFactoryName.Visible = true;
                        mcbAutoFactoryName.Visible = true;
                        mcbAutoFactoryName.SelectedValue = selectedClient.MerchantCode;
                        if (!string.IsNullOrEmpty(selectedClient.MerchantCode))
                        {
                            //根据指定的汽修商户数据库信息获取Venus组织列表
                            List<MDLSM_Organization> tempVenusOrgList = new List<MDLSM_Organization>();
                            BLLCom.QueryAutoFactoryCustomerOrgList(selectedClient.MerchantCode,
                                new MDLSM_Organization
                                {
                                    WHERE_Org_Code = selectedClient.OrgCode
                                }, tempVenusOrgList);

                            if (tempVenusOrgList.Count > 0)
                            {
                                _clientOrgID = tempVenusOrgList[0].Org_ID;
                                _clientOrgCode = tempVenusOrgList[0].Org_Code;
                            }

                        }
                        txtDebtAmount.Text = BLLCom.GetClientArrears(txtSO_Org_ID.Text, selectedClient.ClientID).ToString();
                    }
                }
            }
        }

        #endregion

        #region 销售明细ToolBar相关事件

        /// <summary>
        /// 【详情】Tab内Grid动作按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolbarsManagerDetail_ToolClick(object sender, ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case SysConst.EN_ADD:
                    //添加
                    AddSalesDetail();
                    break;

                case SysConst.EN_DEL:
                    //删除
                    DeleteSalesDetail();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 销售明细Grid相关事件

        /// <summary>
        /// 销售明细Grid单元格失去焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_CellChange(object sender, CellEventArgs e)
        {
            //当前行
            var curActiveRow = gdDetail.Rows[e.Cell.Row.Index];

            #region Cell为[单价/数量]

            //输入的单价和输入的数量计算总金额 计算公式： 单价 * 数量 = 总金额
            if (e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice
                || e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty)
            {
                gdDetail.UpdateData();
                //计算总金额
                if (BLLCom.IsDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Value?.ToString())
                    && BLLCom.IsDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Value?.ToString()))
                {
                    curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TotalAmount].Value = Math.Round(
                        Convert.ToDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Value?.ToString()) *
                        Convert.ToDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Value?.ToString()), 2);
                }
            }
            #endregion

            #region Cell为[价格是否含税]

            if (e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax)
            {
                if (e.Cell.Text == SysConst.True)
                {
                    //勾选含税
                    curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Activation = Activation.AllowEdit;
                    curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TotalTax].Activation = Activation.AllowEdit;

                    //销售订单明细中勾选了[价格含税]，销售订单自动勾选[价格含税]
                    ckSO_IsPriceIncludeTax.CheckState = CheckState.Checked;
                }
                else
                {
                    //不勾选含税
                    //税率单元格禁用
                    curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Value = Convert.ToDecimal(0);
                    curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Activation = Activation.ActivateOnly;
                    curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TotalTax].Value = Convert.ToDecimal(0);
                    curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TotalTax].Activation = Activation.ActivateOnly;

                    //所有销售订单明细中都不勾选[价格含税]，销售订单不勾选[价格含税]
                    if (_detailGridDS.All(x => x.SOD_PriceIsIncludeTax == false))
                    {
                        ckSO_IsPriceIncludeTax.CheckState = CheckState.Unchecked;
                    }
                }
            }
            #endregion

            #region Cell为[单价/数量/税率]

            //如果勾选了含税，则需要只要输入了税率 则需要计算税额  计算公式：税额 = 单价 * 税率 * 数量
            if (e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice
                || e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate
                || e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty)
            {
                if (curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Text == SysConst.True)
                {
                    //计算税额
                    if (BLLCom.IsDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Value?.ToString())
                        && BLLCom.IsDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Value?.ToString())
                        && BLLCom.IsDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Value?.ToString()))
                    {
                        curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TotalTax].Value = Math.Round(
                            Convert.ToDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Value?.ToString()) *
                            Convert.ToDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Value?.ToString()) *
                            Convert.ToDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Value?.ToString()), 2);
                    }
                }
            }
            #endregion

            #region Cell为[计价基准可改]

            if (e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRateIsChangeable)
            {
                if (e.Cell.Text == SysConst.True)
                {
                    //勾选[计价基准可改]
                    curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Activation = Activation.AllowEdit;
                }
                else
                {
                    //不勾选[计价基准可改]
                    curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Value = Convert.ToDecimal(0);
                    curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Activation = Activation.ActivateOnly;
                }
            }
            #endregion

            #region Cell为[签收数量/拒收数量/丢失数量]

            //[销售数量]
            var tempSalesQty = Convert.ToDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Value ?? "0");

            //验证签收数量
            if (e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty)
            {
                gdDetail.UpdateData();
                if (curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Value != null)
                {
                    if (Convert.ToDecimal(e.Cell.Value ?? "0") > tempSalesQty)
                    {
                        //最大签收数量为[销售数量]
                        e.Cell.Value = tempSalesQty;
                    }
                }
                else
                {
                    e.Cell.Value = Convert.ToDecimal(0);
                }
            }
            //验证拒收数量
            if (e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty)
            {
                gdDetail.UpdateData();
                if (curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Value != null)
                {
                    if (Convert.ToDecimal(e.Cell.Value ?? "0") > tempSalesQty)
                    {
                        //最大拒收数量为[销售数量]
                        e.Cell.Value = tempSalesQty;
                    }
                }
                else
                {
                    e.Cell.Value = Convert.ToDecimal(0);
                }
            }
            //验证丢失数量
            if (e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty)
            {
                gdDetail.UpdateData();
                if (curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Value != null)
                {
                    if (Convert.ToDecimal(e.Cell.Value ?? "0") > tempSalesQty)
                    {
                        //最大丢失数量为[销售数量]
                        e.Cell.Value = tempSalesQty;
                    }
                }
                else
                {
                    e.Cell.Value = Convert.ToDecimal(0);
                }
            }
            #endregion

            gdDetail.UpdateData();

            #region 计算单头各金额

            //明细中总金额之和
            decimal tempTotalDetailAmount = 0;
            //明细中总税额之和
            decimal tempTotalDetailTaxAmount = 0;

            foreach (var loopDeatil in _detailGridDS)
            {
                tempTotalDetailAmount += (loopDeatil.SOD_TotalAmount ?? 0);
                tempTotalDetailTaxAmount += (loopDeatil.SOD_TotalTax ?? 0);
            }
            //单头总金额
            txtSO_TotalAmount.Text = tempTotalDetailAmount.ToString();
            //单头总税额
            txtSO_TotalTax.Text = tempTotalDetailTaxAmount.ToString();
            //单头未税总金额
            txtSO_TotalNetAmount.Text = (tempTotalDetailAmount - tempTotalDetailTaxAmount).ToString();
            //单头税率
            if (tempTotalDetailAmount != 0)
            {
                var tempTaxRate = Math.Round(Convert.ToDecimal((tempTotalDetailTaxAmount / tempTotalDetailAmount)), 2);
                txtSO_TaxRate.Text = tempTaxRate.ToString();
            }
            #endregion
        }
        /// <summary>
        /// 销售明细Grid单击Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_ClickCellButton(object sender, CellEventArgs e)
        {
            var curActiveRow = gdDetail.ActiveRow;

            //[异动类型]为{销售出库}和{销售退货}
            var paramInventoryTransTypeList = _inventoryTransTypeList.Where(x => x.Text == InventoryTransTypeEnum.Name.XSCK || x.Text == InventoryTransTypeEnum.Name.XSTH).ToList();
            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //配件条形码
                {ComViewParamKey.AutoPartsBarcode.ToString(), curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Barcode].Value.ToString()},
                //配件名称
                {ComViewParamKey.AutoPartsName.ToString(), curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Name].Value.ToString()},
                //异动类型
                {PISViewParamKey.InventoryTransType.ToString(), paramInventoryTransTypeList},
                //在加载画面时就进行查询
                {ComViewParamKey.IsQueryAtLoadForm.ToString(), true},
            };

            FrmInventoryTransLogQuery frmInventoryTransLogQuery = new FrmInventoryTransLogQuery(paramViewParameters)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = frmInventoryTransLogQuery.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return;
            }
        }
        /// <summary>
        /// 销售明细Grid双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_DoubleClick(object sender, EventArgs e)
        {
            var curActiveRow = gdDetail.ActiveRow;

            //[异动类型]为{销售出库}和{销售退货}
            var paramInventoryTransTypeList = _inventoryTransTypeList.Where(x => x.Text == InventoryTransTypeEnum.Name.XSCK || x.Text == InventoryTransTypeEnum.Name.XSTH).ToList();
            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //配件条形码
                {ComViewParamKey.AutoPartsBarcode.ToString(), curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Barcode].Value.ToString()},
                //配件名称
                {ComViewParamKey.AutoPartsName.ToString(), curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Name].Value.ToString()},
                //异动类型
                {PISViewParamKey.InventoryTransType.ToString(), paramInventoryTransTypeList},
                //在加载画面时就进行查询
                {ComViewParamKey.IsQueryAtLoadForm.ToString(), true},
            };

            FrmInventoryTransLogQuery frmInventoryTransLogQuery = new FrmInventoryTransLogQuery(paramViewParameters)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = frmInventoryTransLogQuery.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return;
            }
        }
        /// <summary>
        /// 销售明细Grid单元格双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdDetail_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            if (!IsAllowUpdateDetailGrid())
            {
                return;
            }

            var curActiveRow = gdDetail.ActiveRow;

            //[异动类型]为{销售出库}和{销售退货}
            var paramInventoryTransTypeList = _inventoryTransTypeList.Where(x => x.Text == InventoryTransTypeEnum.Name.XSCK || x.Text == InventoryTransTypeEnum.Name.XSTH).ToList();
            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //配件条形码
                {ComViewParamKey.AutoPartsBarcode.ToString(), curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Barcode].Value.ToString()},
                //配件名称
                {ComViewParamKey.AutoPartsName.ToString(), curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Name].Value.ToString()},
                //异动类型
                {PISViewParamKey.InventoryTransType.ToString(), paramInventoryTransTypeList},
                //在加载画面时就进行查询
                {ComViewParamKey.IsQueryAtLoadForm.ToString(), true},
            };

            FrmInventoryTransLogQuery frmInventoryTransLogQuery = new FrmInventoryTransLogQuery(paramViewParameters)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = frmInventoryTransLogQuery.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return;
            }
        }

        /// <summary>
        /// 是否允许更新[出库单明细]列表的数据
        /// </summary>
        /// <returns>true:允许；false：不允许</returns>
        private bool IsAllowUpdateDetailGrid()
        {
            if (gdDetail.ActiveRow == null || gdDetail.ActiveRow.Index < 0)
            {
                return false;
            }
            if (gdDetail.DisplayLayout.Bands[0].SortedColumns.Count > 0)
            {
                foreach (UltraGridColumn loopColumn in gdDetail.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (loopColumn.IsGroupByColumn)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion

        #region 出库明细Grid相关事件

        /// <summary>
        /// 出库明细CellChange事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdStockOutDetail_CellChange(object sender, CellEventArgs e)
        {
            #region Cell为[出库数量]

            var cellIndex = gdStockOutDetail.Rows[e.Cell.Row.Index];
            if (e.Cell.Column.Key == SysConst.StockOutQty)
            {
                gdStockOutDetail.UpdateData();
                if (cellIndex.Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_Qty].Value != null)
                {
                    if ((decimal)e.Cell.Value > (decimal)cellIndex.Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_Qty].Value)
                    {
                        //最大出库数量为当前库存量
                        e.Cell.Value = cellIndex.Cells[SystemTableColumnEnums.PIS_Inventory.Code.INV_Qty].Value;
                    }
                }
            }
            #endregion

            gdStockOutDetail.UpdateData();
        }
        #endregion

        #region GroupBox ExpandedStateChanged事件

        /// <summary>
        /// 单头GroupBox ExpandedStateChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gbBase_ExpandedStateChanged(object sender, EventArgs e)
        {
            //SetGroupBoxHeight();
        }

        /// <summary>
        /// 销售明细GroupBox ExpandedStateChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gbDetail_ExpandedStateChanged(object sender, EventArgs e)
        {
            //SetGroupBoxHeight();
        }

        /// <summary>
        /// 出库明细GroupBox ExpandedStateChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gbStockOutDetail_ExpandedStateChanged(object sender, EventArgs e)
        {
            //SetGroupBoxHeight();
        }

        #endregion

        #endregion

        #region 重写基类方法

        #region 动作按钮

        /// <summary>
        /// 新增
        /// </summary>
        public override void NewAction()
        {
            #region 检查详情是否已保存

            SetCardCtrlsToDetailDS();
            base.NewUIModel = HeadDS;
            if (ViewHasChanged(_skipPropertyList)
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
            {
                //信息尚未保存，确定进行当前操作？
                DialogResult dialogResult = MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            #endregion

            //1.执行基类方法
            base.NewAction();
            //2.初始化【详情】Tab内控件
            InitializeDetailTabControls();
            //3.设置【详情】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;

            //设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();
            //根据[来源类型]设置[来源单号]是否显示
            SetDetailVisible();

            //新增的场合，[保存]、[删除]可用，[审核]、[反审核]、[核实]不可用
            SetActionEnable(SystemActionEnum.Code.SAVE, true);
            SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtSO_ID.Text));
            SetActionEnable(SystemActionEnum.Code.APPROVE, false);
            SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
            SetActionEnable(SystemActionEnum.Code.VERIFY, false);
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveAction()
        {
            //1.前端检查-保存
            if (!ClientCheckForSave())
            {
                return;
            }
            //2.将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();
            //3.执行保存（含服务端检查）
            bool saveResult = _bll.SaveDetailDs(HeadDS, _detailGridDS);
            if (!saveResult)
            {
                //保存失败
                MessageBoxs.Show(Trans.SD, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //4.开始监控List变化
            _detailGridDS.StartMonitChanges();
            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置动作按钮状态
            SetActionEnableByStatus();
            //设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();
            //获取出库明细列表
            GetStockOutDetail();
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public override void DeleteAction()
        {
            //1.前端检查-删除
            if (!ClientCheckForDelete())
            {
                return;
            }
            var argsDetail = new List<MDLSD_SalesOrderDetail>();
            //将HeadDS转换为TBModel对象
            var argsHead = HeadDS.ToTBModelForSaveAndDelete<MDLSD_SalesOrder>();
            //将当前DetailGridDS转换为指定类型的TBModelList
            _detailGridDS.ToTBModelListForUpdateAndDelete<MDLSD_SalesOrderDetail>(argsDetail);
            //过滤明细列表中未保存的数据
            argsDetail = argsDetail.Where(x => !string.IsNullOrEmpty(x.WHERE_SOD_ID)).ToList();
            //2.执行删除
            bool deleteResult = _bll.UnityDelete<MDLSD_SalesOrder, MDLSD_SalesOrderDetail>(argsHead, argsDetail);
            if (!deleteResult)
            {
                //删除失败
                MessageBoxs.Show(Trans.SD, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //删除成功
            MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //3.清空【详情】画面数据
            InitializeDetailTabControls();
            //刷新列表
            RefreshList(true);

            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
        }

        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //1.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //2.设置查询条件（翻页相关属性不用设置）
            base.ConditionDS = new SalesOrderManagerQCModel()
            {
                //SqlId
                SqlId = SQLID.SD_SalesOrder_SQL04,
                //单据编号
                WHERE_SO_No = txtWhere_SO_No.Text.Trim(),
                //组织ID
                WHERE_SO_Org_ID = LoginInfoDAX.OrgID,
                //来源类型编码
                WHERE_SO_SourceTypeName = cbWhere_SO_SourceTypeCode.Text,
                //客户类型编码
                WHERE_SO_CustomerTypeName = mcbWhereClientType.SelectedText,
                //客户名称
                WHERE_SO_CustomerName = mcbWhereClientName.SelectedText,
                //单据状态编码
                WHERE_SO_StatusName = cbWhere_SO_StatusCode.Text,
                //审核状态编码
                WHERE_SO_ApprovalStatusName = cbWhere_SO_ApprovalStatusCode.Text,
            };
            if (dtWhere_SalesOrderCreateStart.Value != null)
            {
                //创建时间-开始
                ConditionDS._CreatedTimeStart = dtWhere_SalesOrderCreateStart.DateTime;
            }
            if (dtWhere_SalesOrderCreateEnd.Value != null)
            {
                //创建时间-终了
                ConditionDS._CreatedTimeEnd = dtWhere_SalesOrderCreateEnd.DateTime;
            }
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = base.HeadGridDS;
            gdGrid.DataBind();
            //5.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdGrid.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 清空
        /// </summary>
        public override void ClearAction()
        {
            //初始化【列表】Tab内控件
            InitializeListTabControls();
        }

        /// <summary>
        /// 关闭画面时检查画面值是否发生变化
        /// </summary>
        /// <returns></returns>
        public override bool ViewHasChangedWhenClose()
        {
            SetCardCtrlsToDetailDS();
            base.NewUIModel = HeadDS;
            if (ViewHasChanged(_skipPropertyList)
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 导出当前页
        /// </summary>
        public override void ExportAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.SD_SalesOrder;
            base.ExportAction(gdGrid, paramGridName);
        }

        /// <summary>
        /// 导出全部
        /// </summary>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.SD_SalesOrder;
            List<SalesOrderManagerUIModel> resultAllList = new List<SalesOrderManagerUIModel>();
            SalesOrderManagerQCModel argsSalesOrderManager = new SalesOrderManagerQCModel()
            {
                PageIndex = 1,
                PageSize = null,
                //单据编号
                WHERE_SO_No = txtWhere_SO_No.Text.Trim(),
                //组织ID
                WHERE_SO_Org_ID = LoginInfoDAX.OrgID,
                //来源类型编码
                WHERE_SO_SourceTypeName = cbWhere_SO_SourceTypeCode.Text,
                //客户类型编码
                WHERE_SO_CustomerTypeName = mcbWhereClientType.SelectedText,
                //客户名称
                WHERE_SO_CustomerName = mcbWhereClientName.SelectedText,
                //单据状态编码
                WHERE_SO_StatusName = cbWhere_SO_StatusCode.Text,
                //审核状态编码
                WHERE_SO_ApprovalStatusName = cbWhere_SO_ApprovalStatusCode.Text,
            };
            if (dtWhere_SalesOrderCreateStart.Value != null)
            {
                //创建时间-开始
                argsSalesOrderManager._CreatedTimeStart = dtWhere_SalesOrderCreateStart.DateTime;
            }
            if (dtWhere_SalesOrderCreateEnd.Value != null)
            {
                //创建时间-终了
                argsSalesOrderManager._CreatedTimeEnd = dtWhere_SalesOrderCreateEnd.DateTime;
            }
            _bll.QueryForList(SQLID.SD_SalesOrder_SQL04, argsSalesOrderManager, resultAllList);
            UltraGrid allGrid = gdGrid;
            allGrid.DataSource = resultAllList;
            allGrid.DataBind();

            base.ExportAllAction(allGrid, paramGridName);

            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();
        }

        /// <summary>
        /// 审核
        /// </summary>
        public override void ApproveAction()
        {
            #region 验证

            //1.前端检查-保存
            if (!ClientCheckForSave())
            {
                return;
            }
            //销售订单未保存,不能审核
            if (string.IsNullOrEmpty(txtSO_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_SalesOrder + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MDLSD_SalesOrder resulSalesOrder = new MDLSD_SalesOrder();
            _bll.QueryForObject<MDLSD_SalesOrder, MDLSD_SalesOrder>(new MDLSD_SalesOrder()
            {
                WHERE_SO_IsValid = true,
                WHERE_SO_ID = txtSO_ID.Text.Trim()
            }, resulSalesOrder);
            //销售订单不存在,不能审核
            if (string.IsNullOrEmpty(resulSalesOrder.SO_ID))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_SalesOrder + MsgParam.NOTEXIST, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //验证欠款
            //信用额度
            decimal creditAmount = Convert.ToDecimal(txtCreditAmount.Text.Trim() == "" ? "0" : txtCreditAmount.Text.Trim());
            //欠款金额
            decimal debtAmount = Convert.ToDecimal(txtDebtAmount.Text.Trim() == "" ? "0" : txtDebtAmount.Text.Trim());
            //欠款金额> 信用额度 的场合，提醒用户
            if (debtAmount > creditAmount)
            {
                //客户：{0}的欠款金额为{1}，超过信用额度{2}，是否确认审核？\r\n单击【确定】审核单据，【取消】返回。
                var isContinueApprove = MessageBoxs.Show(Trans.SD, this.ToString(),
                    MsgHelp.GetMsg(MsgCode.W_0039, new object[]
                    {
                        mcbClientName.SelectedText, debtAmount, creditAmount,
                    }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (isContinueApprove != DialogResult.OK)
                {
                    return;
                }
            }

            if (_isHasInventory)
            {
                //生成出库明细
                GetStockOutDetail();

                if (cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.SGCJ
                    || cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.ZXXS)
                {
                    //有进销存的场合，[销售明细]列表中配件的销售数量要与[出库明细]列表中对应配件的总数量一致
                    foreach (var loopSalesOrderDetail in _detailGridDS)
                    {
                        var tempStockOutDetailList =
                            _stockOutDetailDS.Where(x => x.INV_Barcode == loopSalesOrderDetail.SOD_Barcode).ToList();
                        if (tempStockOutDetailList.Count == 0)
                        {
                            //配件loopSalesOrderDetail.SOD_Name（条形码：loopSalesOrderDetail.SOD_Barcode）的库存不存在，审核失败！
                            MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                            {
                                loopSalesOrderDetail.SOD_Name, loopSalesOrderDetail.SOD_Barcode, MsgParam.NOTEXIST,
                                SystemActionEnum.Name.APPROVE
                            }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        //出库明细中该配件的库存总数量
                        decimal barcodeInventoryToatalQty = 0;
                        //出库明细中该配件的出库总数量
                        decimal barcodeStockOutTotalQty = 0;
                        foreach (var loopStockOutDetail in tempStockOutDetailList)
                        {
                            barcodeInventoryToatalQty += (loopStockOutDetail.INV_Qty ?? 0);
                            barcodeStockOutTotalQty += (loopStockOutDetail.StockOutQty ?? 0);
                        }
                        if (barcodeInventoryToatalQty < loopSalesOrderDetail.SOD_Qty)
                        {
                            //配件loopSalesOrderDetail.SOD_Name（条形码：loopSalesOrderDetail.SOD_Barcode）的库存不足，审核失败！
                            MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                            {
                                loopSalesOrderDetail.SOD_Name, loopSalesOrderDetail.SOD_Barcode, MsgParam.SHORTAGE,
                                SystemActionEnum.Name.APPROVE
                            }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        if (barcodeStockOutTotalQty > loopSalesOrderDetail.SOD_Qty)
                        {
                            //配件loopSalesOrderDetail.SOD_Name的销售数量和出库数量的总和不一致，请调整销售数量或出库数量！
                            MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0033, new object[]
                            {loopSalesOrderDetail.SOD_Name}), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
            }
            #endregion

            //确认审核操作
            DialogResult isConfirmApprove = MessageBoxs.Show(Trans.SD, this.ToString(),
                MsgHelp.GetMsg(MsgCode.W_0014),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isConfirmApprove != DialogResult.OK)
            {
                return;
            }

            SetCardCtrlsToDetailDS();

            bool saveApprove = _bll.ApproveDetailDS(HeadDS, _detailGridDS, _stockOutDetailDS, _isHasInventory);
            //审核失败
            if (!saveApprove)
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //审核成功
            MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.APPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //开始监控List变化
            _detailGridDS.StartMonitChanges();
            //将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();

            //设置动作按钮状态
            SetActionEnableByStatus();

            //更新明细列表
            QueryDetail();
            //更新出库明细列表
            GetStockOutDetail();
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();
        }

        /// <summary>
        /// 反审核
        /// </summary>
        public override void UnApproveAction()
        {
            #region 验证

            //销售订单未保存,不能反审核
            if (string.IsNullOrEmpty(txtSO_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_SalesOrder + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MDLSD_SalesOrder resultSalesOrder = new MDLSD_SalesOrder();
            _bll.QueryForObject<MDLSD_SalesOrder, MDLSD_SalesOrder>(new MDLSD_SalesOrder()
            {
                WHERE_SO_IsValid = true,
                WHERE_SO_ID = txtSO_ID.Text.Trim()
            }, resultSalesOrder);
            //销售订单不存在,不能反审核
            if (string.IsNullOrEmpty(resultSalesOrder.SO_ID)
                || string.IsNullOrEmpty(resultSalesOrder.SO_No))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_SalesOrder + MsgParam.NOTEXIST, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //验证配件是否已被签收
            if (_detailGridDS.Any(x => x.SOD_SignQty > 0))
            {
                //配件已被汽修商户签收，不能反审核
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { MsgParam.AUTOPARTS_SIGNIN, SystemActionEnum.Name.UNAPPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.ZXXS
                || cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.SGCJ)
            {
                #region 正常销售的场合

                //销售订单存在[审核状态]为[已审核] 或者 [单据状态]不是[已生成]的[物流单]，不能反审核
                MDLSD_LogisticsBill resultLogisticsBill = new MDLSD_LogisticsBill();
                _bll.QueryForObject<MDLSD_LogisticsBill, MDLSD_LogisticsBill>(new MDLSD_LogisticsBill()
                {
                    WHERE_LB_IsValid = true,
                    WHERE_LB_SourceNo = txtSO_No.Text.Trim(),
                }, resultLogisticsBill);
                if (!string.IsNullOrEmpty(resultLogisticsBill.LB_ID))
                {
                    //销售订单对应的[物流单].[审核状态]为[已审核]，不能反审核
                    if (resultLogisticsBill.LB_ApprovalStatusName == ApprovalStatusEnum.Name.YSH)
                    {
                        MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0023, new object[] { SystemTableEnums.Name.SD_SalesOrder, SystemTableEnums.Name.SD_LogisticsBill, SystemActionEnum.Name.UNAPPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    //销售订单对应的[物流单].[单据状态]不是[已生成]，不能反审核
                    if (resultLogisticsBill.LB_StatusName != LogisticsBillStatusEnum.Name.YSC)
                    {
                        MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.SD_SalesOrder + MsgParam.CORRESPONDENCE + MsgParam.OF + SystemTableEnums.Name.SD_LogisticsBill + resultLogisticsBill.LB_StatusName, SystemActionEnum.Name.UNAPPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                #endregion
            }

            #endregion

            //确认反审核操作
            DialogResult isConfirmUnApprove = MessageBoxs.Show(Trans.SD, this.ToString(),
                MsgHelp.GetMsg(MsgCode.W_0018),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isConfirmUnApprove != DialogResult.OK)
            {
                return;
            }

            SetCardCtrlsToDetailDS();
            GetStockOutDetail();

            bool saveUnApprove = _bll.UnApproveDetailDS(HeadDS, _detailGridDS, _isHasInventory, _stockOutDetailDS);
            //反审核失败
            if (!saveUnApprove)
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //反审核成功
            MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.UNAPPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //开始监控List变化
            _detailGridDS.StartMonitChanges();
            //将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();

            //设置动作按钮状态
            SetActionEnableByStatus();

            //更新明细列表
            QueryDetail();
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void PrintAction()
        {
            try
            {
                if (HeadDS == null || string.IsNullOrWhiteSpace(HeadDS.SO_No))
                {
                    //请选择销售订单
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, SystemTableEnums.Name.SD_SalesOrder), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //销售订单待审核，不能打印
                if (HeadDS.SO_ApprovalStatusName != ApprovalStatusEnum.Name.YSH)
                {
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, SystemTableEnums.Name.SD_SalesOrder + HeadDS.SO_ApprovalStatusName, SystemActionEnum.Name.PRINT), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //待打印的销售订单
                SalesOrderUIModelToPrint salesOrderToPrint = new SalesOrderUIModelToPrint();
                _bll.CopyModel(HeadDS, salesOrderToPrint);
                //待打印的销售订单明细
                List<SalesOrderDetailUIModelToPrint> salesOrderDetailToPrintsList = new List<SalesOrderDetailUIModelToPrint>();
                _bll.CopyModelList(_detailGridDS, salesOrderDetailToPrintsList);

                foreach (var loopDetail in salesOrderDetailToPrintsList)
                {
                    var stockOutDetaiList =
                        _stockOutDetailDS.Where(
                            x =>
                                x.INV_Barcode == loopDetail.SOD_Barcode &&
                                x.INV_Specification == loopDetail.SOD_Specification &&
                                x.SOBD_UnitSalePrice == loopDetail.SOD_UnitPrice).ToList();

                    //每个明细的出库仓位
                    string stockOutBinName = string.Empty;
                    foreach (var loopStockOutDetail in stockOutDetaiList)
                    {
                        if (string.IsNullOrEmpty(loopStockOutDetail.INV_Barcode)
                            || string.IsNullOrEmpty(loopStockOutDetail.INV_BatchNo)
                            || string.IsNullOrEmpty(loopStockOutDetail.WHB_Name))
                        {
                            continue;
                        }
                        stockOutBinName += loopStockOutDetail.WHB_Name + SysConst.Comma_DBC;
                    }
                    loopDetail.StockOutBinName = stockOutBinName;
                    loopDetail.SOD_Qty = Math.Round((loopDetail.SOD_Qty ?? 0), 0);
                    salesOrderToPrint.TotalSalesQty += (loopDetail.SOD_Qty ?? 0);
                }

                Dictionary<string, object> argsViewParams = new Dictionary<string, object>
                {
                    //销售订单
                    {SDViewParamKey.SalesOrder.ToString(), salesOrderToPrint},
                    //销售订单明细
                    {SDViewParamKey.SalesOrderDetail.ToString(), salesOrderDetailToPrintsList},
                };

                FrmViewAndPrintSalesOrder frmViewAndPrintSalesOrder = new FrmViewAndPrintSalesOrder(argsViewParams)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmViewAndPrintSalesOrder.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// 核实
        /// </summary>
        public override void VerifyAction()
        {
            #region 验证

            //验证明细
            for (int i = 0; i < gdDetail.Rows.Count; i++)
            {
                //验证签收数量
                if (gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Value == null)
                {
                    //为空置为0
                    gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Value =
                        Convert.ToDecimal(0);
                }
                if (!BLLCom.IsDecimal(gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Value.ToString()))
                {
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.SD_SalesOrderDetail.Name.SOD_SignQty, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //验证拒收数量
                if (gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Value == null)
                {
                    //为空置为0
                    gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Value =
                        Convert.ToDecimal(0);
                }
                if (!BLLCom.IsDecimal(gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Value.ToString()))
                {
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.SD_SalesOrderDetail.Name.SOD_RejectQty, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //验证丢失数量
                if (gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Value == null)
                {
                    //为空置为0
                    gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Value =
                        Convert.ToDecimal(0);
                }
                if (!BLLCom.IsDecimal(gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Value.ToString()))
                {
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.SD_SalesOrderDetail.Name.SOD_LoseQty, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //销售数量
                decimal salesQty = 0;
                //签收数量
                decimal signQty = 0;
                //拒收数量
                decimal rejectQty = 0;
                //丢失数量
                decimal loseQty = 0;

                salesQty = Convert.ToDecimal(gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Value ?? "0");
                signQty = Convert.ToDecimal(gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Value ?? "0");
                rejectQty = Convert.ToDecimal(gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Value ?? "0");
                loseQty = Convert.ToDecimal(gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Value ?? "0");

                if (signQty + rejectQty + loseQty != salesQty)
                {
                    //第{0}行，签收数量、拒收数量和丢失数量之和与销售数量不一致，请调整！
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0037, new object[]
                    {i + 1, MsgParam.SIGN_REJECT_LOSE_SUM,SystemTableColumnEnums.SD_SalesOrderDetail.Name.SOD_Qty,string.Empty}), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            //销售订单未保存,不能核实
            if (string.IsNullOrEmpty(txtSO_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_SalesOrder + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.VERIFY
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MDLSD_SalesOrder resulSalesOrder = new MDLSD_SalesOrder();
            _bll.QueryForObject<MDLSD_SalesOrder, MDLSD_SalesOrder>(new MDLSD_SalesOrder()
            {
                WHERE_SO_IsValid = true,
                WHERE_SO_ID = txtSO_ID.Text.Trim()
            }, resulSalesOrder);
            //销售订单不存在,不能核实
            if (string.IsNullOrEmpty(resulSalesOrder.SO_ID) || string.IsNullOrEmpty(resulSalesOrder.SO_No))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_SalesOrder + MsgParam.NOTEXIST, SystemActionEnum.Name.VERIFY
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (resulSalesOrder.SO_StatusName == SalesOrderStatusEnum.Name.YSC
                || resulSalesOrder.SO_StatusName == SalesOrderStatusEnum.Name.JYCG
                || resulSalesOrder.SO_StatusName == SalesOrderStatusEnum.Name.YGB)
            {
                //销售订单的单据状态为resulSalesOrder.SO_StatusName，不能核实
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_SalesOrder +MsgParam.OF+MsgParam.BILL_STATUS+MsgParam.BE+ resulSalesOrder.SO_StatusName, SystemActionEnum.Name.VERIFY
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //验证对应物流单审核状态，未审核不能核实
            var dshLogisticsBillCount = _bll.QueryForCount<int>(new MDLSD_LogisticsBill
            {
                WHERE_LB_SourceNo = resulSalesOrder.SO_No,
                WHERE_LB_ApprovalStatusName = ApprovalStatusEnum.Name.DSH,
                WHERE_LB_IsValid = true
            });
            if (dshLogisticsBillCount > 0)
            {
                //销售订单已存在待审核的物流订单，不能核实
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.SD_SalesOrder +MsgParam.ALREADYEXIST+ApprovalStatusEnum.Name.DSH+MsgParam.OF+SystemTableEnums.Name.SD_LogisticsBill, SystemActionEnum.Name.VERIFY
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //确认核实操作
            DialogResult isVerify = MessageBoxs.Show(Trans.SD, this.ToString(),
                MsgHelp.GetMsg(MsgCode.W_0038), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isVerify != DialogResult.OK)
            {
                return;
            }

            #endregion

            #region 退货流程

            //是否存在退货配件
            bool isExistRejectAutoParts = false;
            //待退货的销售订单明细
            var salesOrderReturnDetailList = _detailGridDS.Where(x => x.SOD_RejectQty > 0).ToList();
            List<SalesReturnDetailUIModel> salesReturnDetailList = new List<SalesReturnDetailUIModel>();
            _bll.CopyModelList(salesOrderReturnDetailList, salesReturnDetailList);
            //待退货入库明细列表
            List<ReturnStockInDetailUIModel> returnStockInDetailList = new List<ReturnStockInDetailUIModel>();
            isExistRejectAutoParts = salesReturnDetailList.Count > 0;
            if (isExistRejectAutoParts == true)
            {
                foreach (var loopReturnDetail in salesReturnDetailList)
                {
                    //退货的配件对应的出库明细列表
                    var returnStockOutDetailList = _stockOutDetailDS.Where(x => x.INV_Barcode == loopReturnDetail.SOD_Barcode).ToList();

                    foreach (var loopReturnStockOutDetail in returnStockOutDetailList)
                    {
                        //退货入库明细
                        ReturnStockInDetailUIModel returnStockInDetail = new ReturnStockInDetailUIModel()
                        {
                            SID_SourceDetailID = loopReturnDetail.SOD_ID,
                            SID_Name = loopReturnStockOutDetail.INV_Name,
                            SID_Barcode = loopReturnStockOutDetail.INV_Barcode,
                            SID_BatchNo = loopReturnStockOutDetail.INV_BatchNo,
                            SID_OEMNo = loopReturnStockOutDetail.INV_OEMNo,
                            SID_ThirdNo = loopReturnStockOutDetail.INV_ThirdNo,
                            SID_Specification = loopReturnStockOutDetail.INV_Specification,
                            SID_UOM = loopReturnDetail.SOD_UOM,
                            SID_SUPP_ID = loopReturnStockOutDetail.INV_SUPP_ID,
                            SID_WH_ID = loopReturnStockOutDetail.INV_WH_ID,
                            SID_WHB_ID = loopReturnStockOutDetail.INV_WHB_ID,
                            SID_UnitCostPrice = loopReturnStockOutDetail.INV_PurchaseUnitPrice,
                            SID_IsValid = true,
                            SID_CreatedBy = LoginInfoDAX.UserName,
                            SID_CreatedTime = BLLCom.GetCurStdDatetime(),
                            SID_UpdatedBy = LoginInfoDAX.UserName,
                            SID_UpdatedTime = BLLCom.GetCurStdDatetime(),

                            //退货总数量
                            SOD_RejectQty = loopReturnDetail.SOD_RejectQty,
                            //默认入库总数量为退货总数量
                            SID_Qty = loopReturnDetail.SOD_RejectQty,
                            SID_Amount = Math.Round((loopReturnDetail.SOD_RejectQty ?? 0) * (loopReturnStockOutDetail.INV_PurchaseUnitPrice ?? 0), 2),

                            SUPP_Name = loopReturnStockOutDetail.SUPP_Name,
                            WH_Name = loopReturnStockOutDetail.WH_Name,
                            WHB_Name = loopReturnStockOutDetail.WHB_Name,

                            APA_Level = loopReturnDetail.APA_Level,
                            APA_Brand = loopReturnDetail.APA_Brand,
                            APA_VehicleBrand = loopReturnDetail.APA_VehicleBrand,
                            APA_VehicleInspire = loopReturnDetail.APA_VehicleInspire,
                            APA_VehicleCapacity = loopReturnDetail.APA_VehicleCapacity,
                            APA_VehicleYearModel = loopReturnDetail.APA_VehicleYearModel,
                            APA_VehicleGearboxTypeCode = loopReturnDetail.APA_VehicleGearboxTypeCode,
                            APA_VehicleGearboxTypeName = loopReturnDetail.APA_VehicleGearboxTypeName,
                        };
                        returnStockInDetailList.Add(returnStockInDetail);
                    }
                }

                Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
                {
                    //确认退货画面的来源方式：核实销售订单
                    {SDViewParamKey.ConfirmReturnSourType.ToString(), SDViewParamValue.ConfirmReturnSourType.VerifySalesOrder},
                    //销售退货明细
                    {SDViewParamKey.SalesOrderReturnDetail.ToString(), salesReturnDetailList},
                    //退货入库明细
                    {SDViewParamKey.ReturnStockInDetail.ToString(), returnStockInDetailList},
                };

                //确认退货信息
                FrmConfirmReturnWindow frmConfirmReturnWindow = new FrmConfirmReturnWindow(paramViewParameters)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmConfirmReturnWindow.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
            }
            #endregion

            #region 赔偿流程

            //待赔偿的销售订单明细
            var salesOrderLoseDetailList = _detailGridDS.Where(x => x.SOD_LoseQty > 0).ToList();
            #endregion

            SetCardCtrlsToDetailDS();

            bool approveResult = _bll.VerifyDetailDS(HeadDS, _detailGridDS, salesReturnDetailList, returnStockInDetailList, salesOrderLoseDetailList);
            if (!approveResult)
            {
                //核实失败
                MessageBoxs.Show(Trans.SD, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //核实成功
            MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.VERIFY }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //开始监控List变化
            _detailGridDS.StartMonitChanges();
            //将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();

            //更新明细列表
            QueryDetail();
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();
        }
        #endregion

        #region 导航按钮

        /// <summary>
        /// 初始化导航按钮
        /// </summary>
        public override void InitializeNavigate()
        {
            base.InitializeNavigate();
            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.ONLINEPAY, true, true);
            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOLOGISTICSBILL, true, true);
            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, true);
        }

        /// <summary>
        /// 转结算
        /// </summary>
        public override void ToSettlementNavigate()
        {
            base.ToSettlementNavigate();
            //选中的待付款的[销售单]列表
            List<SalesOrderManagerUIModel> selectedSalesOrderList = new List<SalesOrderManagerUIModel>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 当前选中Tab为【详情】
                if (HeadDS == null || string.IsNullOrEmpty(HeadDS.SO_No) || HeadDS.SO_StatusName != SalesOrderStatusEnum.Name.JYCG || HeadDS.UnReceiveAmount <= 0)
                {
                    //请选择一个交易成功并且未收清的销售订单信息进行转结算
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + SalesOrderStatusEnum.Name.JYCG + MsgParam.AND + MsgParam.NOTYET + MsgParam.RECEIVE__FULL + MsgParam.OF + SystemTableEnums.Name.SD_SalesOrder + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                selectedSalesOrderList.Add(HeadDS);

                #endregion
            }
            else
            {
                #region 当前选中Tab为【列表】

                var checkedGrid = HeadGridDS.Where(x => x.IsChecked == true).ToList();

                if (checkedGrid.Count == 0)
                {
                    //请选择一个交易成功并且未收清的销售订单信息进行转结算
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + SalesOrderStatusEnum.Name.JYCG + MsgParam.AND + MsgParam.NOTYET + MsgParam.RECEIVE__FULL + MsgParam.OF + SystemTableEnums.Name.SD_SalesOrder + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var JYCGAndPayFullList = checkedGrid.Where(x =>
                             x.SO_StatusName != SalesOrderStatusEnum.Name.JYCG ||
                             x.UnReceiveAmount <= 0).ToList();
                if (JYCGAndPayFullList.Count > 0)
                {
                    //请选择交易成功并且未收清的销售订单信息进行转结算
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { SalesOrderStatusEnum.Name.JYCG + MsgParam.AND + MsgParam.NOTYET + MsgParam.RECEIVE__FULL + MsgParam.OF + SystemTableEnums.Name.SD_SalesOrder + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var loopCheckedItem in JYCGAndPayFullList)
                    {
                        loopCheckedItem.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }

                var firstCheckedItem = checkedGrid.FirstOrDefault(x => x.SO_StatusName == SalesOrderStatusEnum.Name.JYCG && x.UnReceiveAmount > 0);
                if (firstCheckedItem == null)
                {
                    //请至少勾选一条交易成功并且未收清的销售订单信息进行转结算
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SalesOrderStatusEnum.Name.JYCG + MsgParam.AND + MsgParam.NOTYET + MsgParam.RECEIVE__FULL + MsgParam.OF + SystemTableEnums.Name.SD_SalesOrder, SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var differFirstCheckedItem = checkedGrid.Where(x => x.SO_CustomerTypeName != firstCheckedItem.SO_CustomerTypeName
                || x.SO_CustomerID != firstCheckedItem.SO_CustomerID).ToList();
                if (differFirstCheckedItem.Count > 0)
                {
                    //请选择相同来源类型，相同客户的销售订单转结算
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.SAME_SOURCEANDCUSTOMER + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var loopCheckedItem in differFirstCheckedItem)
                    {
                        loopCheckedItem.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }
                _bll.CopyModelList(checkedGrid, selectedSalesOrderList);

                #endregion
            }

            #region 访问数据库，获取应收单数据

            //传入的待收款的[销售订单]列表
            SalesOrderDataSet.SalesOrderDataTable salesOrderDataTable = new SalesOrderDataSet.SalesOrderDataTable();

            foreach (var loopSalesOrderDetail in selectedSalesOrderList)
            {
                if (string.IsNullOrEmpty(loopSalesOrderDetail.SO_No))
                {
                    continue;
                }

                SalesOrderDataSet.SalesOrderRow newSalesOrderDetailRow = salesOrderDataTable.NewSalesOrderRow();
                newSalesOrderDetailRow.SO_Org_ID = loopSalesOrderDetail.SO_Org_ID;
                newSalesOrderDetailRow.SO_No = loopSalesOrderDetail.SO_No;
                newSalesOrderDetailRow.SO_SourceTypeName = loopSalesOrderDetail.SO_SourceTypeName;
                newSalesOrderDetailRow.SO_CustomerTypeName = loopSalesOrderDetail.SO_CustomerTypeName;
                newSalesOrderDetailRow.SO_CustomerID = loopSalesOrderDetail.SO_CustomerID;

                salesOrderDataTable.AddSalesOrderRow(newSalesOrderDetailRow);
            }
            //创建SqlConnection数据库连接对象
            SqlConnection sqlCon = new SqlConnection
            {
                ConnectionString = DBManager.GetConnectionString(DBCONFIG.Coeus)
            };
            //打开数据库连接
            sqlCon.Open();
            //创建并初始化SqlCommand对象
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlCon;

            SalesOrderDataSet.SalesOrderToReceiptDataTable resultSalesOrderToSettlementList =
                new SalesOrderDataSet.SalesOrderToReceiptDataTable();

            try
            {
                cmd.CommandText = "P_SD_GetSalesOrderListToReceipt";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SalesOrderList", SqlDbType.Structured);
                cmd.Parameters[0].Value = salesOrderDataTable;

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(resultSalesOrderToSettlementList);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return;
            }
            finally
            {
                sqlCon.Close();
            }

            #endregion

            #region 转收款

            //待确认收款的销售订单信息列表
            List<BusinessCollectionConfirmUIModel> salesOrderToReceiptList = new List<BusinessCollectionConfirmUIModel>();
            foreach (var loopSalesOrderToReceipt in resultSalesOrderToSettlementList)
            {
                BusinessCollectionConfirmUIModel salesOrderToReceipt = new BusinessCollectionConfirmUIModel
                {
                    IsBusinessSourceAccountPayableBill = false,
                    BusinessID = loopSalesOrderToReceipt.BusinessID,
                    BusinessNo = loopSalesOrderToReceipt.BusinessNo,
                    BusinessOrgID = loopSalesOrderToReceipt.BusinessOrgID,
                    BusinessOrgName = loopSalesOrderToReceipt.BusinessOrgName,
                    BusinessSourceTypeName = loopSalesOrderToReceipt.BusinessSourceTypeName,
                    BusinessSourceTypeCode = loopSalesOrderToReceipt.BusinessSourceTypeCode,
                    BusinessSourceNo = loopSalesOrderToReceipt.BusinessSourceNo,
                    PayObjectTypeName = loopSalesOrderToReceipt.PayObjectTypeName,
                    PayObjectTypeCode = loopSalesOrderToReceipt.PayObjectTypeCode,
                    PayObjectID = loopSalesOrderToReceipt.PayObjectID,
                    PayObjectName = loopSalesOrderToReceipt.PayObjectName,
                    ReceivableTotalAmount = loopSalesOrderToReceipt.ReceivableTotalAmount,
                    ReceiveTotalAmount = loopSalesOrderToReceipt.ReceiveTotalAmount,
                    UnReceiveTotalAmount = loopSalesOrderToReceipt.UnReceiveTotalAmount,
                    Wal_ID = loopSalesOrderToReceipt.Wal_ID,
                    Wal_No = loopSalesOrderToReceipt.Wal_No,
                    Wal_AvailableBalance = loopSalesOrderToReceipt.Wal_AvailableBalance,
                    Wal_VersionNo = loopSalesOrderToReceipt.Wal_VersionNo,
                    //应收单相关
                    ARB_ID = loopSalesOrderToReceipt.ARB_ID,
                    ARB_No = loopSalesOrderToReceipt.ARB_No,
                    ARB_BillDirectCode = loopSalesOrderToReceipt.ARB_BillDirectCode,
                    ARB_BillDirectName = loopSalesOrderToReceipt.ARB_BillDirectName,
                    ARB_SourceTypeCode = loopSalesOrderToReceipt.ARB_SourceTypeCode,
                    ARB_SourceTypeName = loopSalesOrderToReceipt.ARB_SourceTypeName,
                    ARB_SrcBillNo = loopSalesOrderToReceipt.ARB_SrcBillNo,
                    ARB_Org_ID = loopSalesOrderToReceipt.ARB_Org_ID,
                    ARB_Org_Name = loopSalesOrderToReceipt.ARB_Org_Name,
                    ARB_AccountReceivableAmount = loopSalesOrderToReceipt.ARB_AccountReceivableAmount,
                    ARB_ReceivedAmount = loopSalesOrderToReceipt.ARB_ReceivedAmount,
                    ARB_UnReceiveAmount = loopSalesOrderToReceipt.ARB_UnReceiveAmount,
                    ARB_BusinessStatusCode = loopSalesOrderToReceipt.ARB_BusinessStatusCode,
                    ARB_BusinessStatusName = loopSalesOrderToReceipt.ARB_BusinessStatusName,
                    ARB_ApprovalStatusCode = loopSalesOrderToReceipt.ARB_ApprovalStatusCode,
                    ARB_ApprovalStatusName = loopSalesOrderToReceipt.ARB_ApprovalStatusName,
                    ARB_CreatedBy = loopSalesOrderToReceipt.ARB_CreatedBy,
                    ARB_CreatedTime = loopSalesOrderToReceipt.ARB_CreatedTime,
                    ARB_VersionNo = loopSalesOrderToReceipt.ARB_VersionNo,
                };
                if (loopSalesOrderToReceipt.ARB_BillDirectName == BillDirectionEnum.Name.PLUS)
                {
                    salesOrderToReceipt.ReceivableTotalAmount = loopSalesOrderToReceipt.ARB_AccountReceivableAmount;
                    salesOrderToReceipt.ReceiveTotalAmount = loopSalesOrderToReceipt.ARB_ReceivedAmount;
                    salesOrderToReceipt.UnReceiveTotalAmount = loopSalesOrderToReceipt.ARB_UnReceiveAmount;
                }
                else
                {
                    salesOrderToReceipt.ReceivableTotalAmount = -loopSalesOrderToReceipt.ARB_AccountReceivableAmount;
                    salesOrderToReceipt.ReceiveTotalAmount = -loopSalesOrderToReceipt.ARB_ReceivedAmount;
                    salesOrderToReceipt.UnReceiveTotalAmount = -loopSalesOrderToReceipt.ARB_UnReceiveAmount;
                }
                salesOrderToReceiptList.Add(salesOrderToReceipt);
            }

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
                {
                    //业务单确认收款
                    {ComViewParamKey.BusinessCollectionConfirm.ToString(), salesOrderToReceiptList}
                };

            //跳转[业务单确认收款弹出窗]
            FrmBusinessCollectionConfirmWindow frmBusinessCollectionConfirmWindow = new FrmBusinessCollectionConfirmWindow(paramViewParameters)
            {
                StartPosition = FormStartPosition.CenterScreen
            };

            DialogResult dialogResult = frmBusinessCollectionConfirmWindow.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                QueryAction();
            }

            #endregion
        }

        /// <summary>
        /// 转物流
        /// </summary>
        public override void ToLogisticsBillNavigate()
        {
            base.ToLogisticsBillNavigate();

            #region 验证及准备数据

            //要跳转到物流的销售订单
            SalesOrderManagerUIModel curSalesOrderToLogistics = new SalesOrderManagerUIModel();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                //选中【销售】Tab的场合
                _bll.CopyModel(HeadDS, curSalesOrderToLogistics);
            }
            else if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                //选中【销售列表】Tab的场合
                var selectedSalesOrderList = HeadGridDS.Where(x => x.IsChecked == true).ToList();
                if (selectedSalesOrderList.Count == 1)
                {
                    if (selectedSalesOrderList[0].SO_ApprovalStatusName != ApprovalStatusEnum.Name.YSH
                        || selectedSalesOrderList[0].SO_StatusName != SalesOrderStatusEnum.Name.DFH)
                    {
                        //请选择待发货的销售订单转物流
                        MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { SalesOrderStatusEnum.Name.DFH + MsgParam.OF + SystemTableEnums.Name.SD_SalesOrder + SystemNavigateEnum.Name.TOLOGISTICSBILL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        selectedSalesOrderList[0].IsChecked = false;
                        gdGrid.DataSource = HeadGridDS;
                        gdGrid.DataBind();
                        return;
                    }
                    curSalesOrderToLogistics = selectedSalesOrderList[0];
                }
                else
                {
                    var tempCannotToLogistic =
                        selectedSalesOrderList.Where(x => x.SO_StatusName != SalesOrderStatusEnum.Name.DFH).ToList();
                    //请选择一个待发货的销售订单转物流
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + SalesOrderStatusEnum.Name.DFH + MsgParam.OF + SystemTableEnums.Name.SD_SalesOrder + SystemNavigateEnum.Name.TOLOGISTICSBILL }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var loopCannotToLogistic in tempCannotToLogistic)
                    {
                        loopCannotToLogistic.IsChecked = false;
                        gdGrid.DataSource = HeadGridDS;
                        gdGrid.DataBind();
                        return;
                    }
                    return;
                }

                //查询销售订单明细
                _bll.QueryForList(SQLID.SD_SalesOrder_SQL01, new SalesOrderManagerQCModel
                {
                    WHERE_SOD_SO_ID = curSalesOrderToLogistics.SO_ID
                }, _detailGridDS);
            }

            if (string.IsNullOrEmpty(curSalesOrderToLogistics.SO_ID)
                || string.IsNullOrEmpty(curSalesOrderToLogistics.SO_No))
            {
                //没有获取到销售订单，转物流失败
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.SD_SalesOrder, SystemNavigateEnum.Name.TOLOGISTICSBILL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //查询该销售订单是否已存在物流单
            var logisticsBillCount = _bll.QueryForCount<int>(new MDLSD_LogisticsBill
            {
                WHERE_LB_SourceNo = curSalesOrderToLogistics.SO_No,
                WHERE_LB_IsValid = true
            });
            if (logisticsBillCount > 0)
            {
                //curSalesOrderToLogistics.SO_No对应的物流订单已存在，不能重复添加
                MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.E_0006, new object[] { curSalesOrderToLogistics.SO_No + MsgParam.CORRESPONDENCE + MsgParam.OF + SystemTableEnums.Name.SD_LogisticsBill }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion

            //传入的物流单
            MDLSD_LogisticsBill argsLogisticsBill = new MDLSD_LogisticsBill();
            //传入的物流单明细
            List<MDLSD_LogisticsBillDetail> argsLogisticsBillDetailList = new List<MDLSD_LogisticsBillDetail>();

            #region 准备[物流单]数据

            argsLogisticsBill.LB_Org_ID = LoginInfoDAX.OrgID;
            argsLogisticsBill.LB_Org_Name = LoginInfoDAX.OrgShortName;
            //根据销售订单.[来源类型]设置物流单.[来源类型]
            if (cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.SGCJ)
            {
                argsLogisticsBill.LB_SourceTypeName = DeliveryBillSourceTypeEnum.Name.ZJXS;
                argsLogisticsBill.LB_SourceTypeCode = DeliveryBillSourceTypeEnum.Code.ZJXS;
            }
            else if (cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.ZXXS)
            {
                argsLogisticsBill.LB_SourceTypeName = DeliveryBillSourceTypeEnum.Name.ZXXS;
                argsLogisticsBill.LB_SourceTypeCode = DeliveryBillSourceTypeEnum.Code.ZXXS;
            }
            else if (cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.ZDXS)
            {
                argsLogisticsBill.LB_SourceTypeName = DeliveryBillSourceTypeEnum.Name.ZDXS;
                argsLogisticsBill.LB_SourceTypeCode = DeliveryBillSourceTypeEnum.Code.ZDXS;
            }
            argsLogisticsBill.LB_SourceNo = curSalesOrderToLogistics.SO_No;
            argsLogisticsBill.LB_AccountReceivableAmount = curSalesOrderToLogistics.SO_TotalAmount;
            //默认物流单状态为{已生成}
            argsLogisticsBill.LB_StatusName = LogisticsBillStatusEnum.Name.YSC;
            argsLogisticsBill.LB_StatusCode = LogisticsBillStatusEnum.Code.YSC;
            //默认审核状态为{待审核}
            argsLogisticsBill.LB_ApprovalStatusName = ApprovalStatusEnum.Name.DSH;
            argsLogisticsBill.LB_ApprovalStatusCode = ApprovalStatusEnum.Code.DSH;
            //默认物流费付款状态为{未支付}
            argsLogisticsBill.LB_PayStautsName = PaymentStatusEnum.Name.WZF;
            argsLogisticsBill.LB_PayStautsCode = PaymentStatusEnum.Code.WZF;
            //默认物流人员接单时间为当前时间
            argsLogisticsBill.LB_AcceptTime = BLLCom.GetCurStdDatetime();
            argsLogisticsBill.LB_IsValid = true;
            argsLogisticsBill.LB_CreatedBy = LoginInfoDAX.UserName;
            argsLogisticsBill.LB_UpdatedBy = LoginInfoDAX.UserName;
            argsLogisticsBill.LB_CreatedTime = BLLCom.GetCurStdDatetime();
            argsLogisticsBill.LB_UpdatedTime = BLLCom.GetCurStdDatetime();
            argsLogisticsBill.LB_VersionNo = 1;
            //收件人
            argsLogisticsBill.LB_Receiver = curSalesOrderToLogistics.SO_CustomerName;
            //获取客户信息
            List<CustomerQueryUIModel> resultCustomerList = new List<CustomerQueryUIModel>();
            resultCustomerList = BLLCom.GetCustomerInfo(curSalesOrderToLogistics.SO_CustomerTypeName, curSalesOrderToLogistics.SO_CustomerID, curSalesOrderToLogistics.SO_CustomerName);
            if (resultCustomerList.Count == 1)
            {
                //收件人手机号
                argsLogisticsBill.LB_ReceiverPhoneNo = resultCustomerList[0].CustomerPhoneNo;
                //收件人地址
                argsLogisticsBill.LB_ReceiverAddress = resultCustomerList[0].CustomerAddress;
                //物流人员类型名称
                argsLogisticsBill.LB_SourceName = resultCustomerList[0].DeliveryTypeName;
                //物流人员类型编码
                argsLogisticsBill.LB_SourceCode = resultCustomerList[0].DeliveryTypeCode;
                //物流人员ID
                argsLogisticsBill.LB_DeliveryByID = resultCustomerList[0].DeliveryByID;
                //物流人员名称
                argsLogisticsBill.LB_DeliveryBy = resultCustomerList[0].DeliveryByName;
                //物流人员手机号
                argsLogisticsBill.LB_PhoneNo = resultCustomerList[0].DeliveryByPhone;
            }
            #endregion

            #region 准备[物流单明细]数据

            foreach (var loopSalesOrderDetail in _detailGridDS)
            {
                if (string.IsNullOrEmpty(loopSalesOrderDetail.SOD_ID))
                {
                    continue;
                }
                MDLSD_LogisticsBillDetail addLogisticsBillDetail = new MDLSD_LogisticsBillDetail
                {
                    LBD_Barcode = loopSalesOrderDetail.SOD_Barcode,
                    LBD_BatchNoNew = loopSalesOrderDetail.SOD_BatchNoNew,
                    LBD_Name = loopSalesOrderDetail.SOD_Name,
                    LBD_Specification = loopSalesOrderDetail.SOD_Specification,
                    LBD_UOM = loopSalesOrderDetail.SOD_UOM,
                    LBD_DeliveryQty = loopSalesOrderDetail.SOD_Qty,
                    LBD_AccountReceivableAmount = loopSalesOrderDetail.SOD_TotalAmount,
                    //默认物流明细为{已生成}
                    LBD_StatusName = LogisticsBillDetailStatusEnum.Name.YSC,
                    LBD_StatusCode = LogisticsBillDetailStatusEnum.Code.YSC,
                    LBD_IsValid = true,
                    LBD_CreatedBy = LoginInfoDAX.UserName,
                    LBD_UpdatedBy = LoginInfoDAX.UserName,
                    LBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                    LBD_UpdatedTime = BLLCom.GetCurStdDatetime(),
                    LBD_VersionNo = 1
                };
                argsLogisticsBillDetailList.Add(addLogisticsBillDetail);
            }
            #endregion

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //物流单
                {SDViewParamKey.LogisticsBill.ToString(), argsLogisticsBill},
                //物流单明细
                {SDViewParamKey.LogisticsBillDetail.ToString(), argsLogisticsBillDetailList},
            };

            //跳转到[物流单管理]
            SystemFunction.ShowViewFromView(MsgParam.LOGISTICSBILL_MANAGER, ViewClassFullNameConst.SD_FrmLogisticsBillManager, true, PageDisplayMode.TabPage, paramViewParameters, null);
        }

        /// <summary>
        /// 在线支付
        /// </summary>
        public override void OnLinePayNavigate()
        {
            base.OnLinePayNavigate();
            if (string.IsNullOrEmpty(txtSO_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0023, new object[] { SystemActionEnum.Name.SAVE, MsgParam.CONDUCT + MsgParam.ONLINE_PAY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //TODO 跳转支付画面
        }

        #endregion

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //单据编号
            txtSO_No.Clear();
            //组织ID
            txtSO_Org_ID.Clear();
            //来源单号
            txtSO_SourceNo.Clear();
            //汽修商户
            mcbAutoFactoryName.Clear();
            //客户ID
            _clientOrgID = string.Empty;
            _clientOrgCode = string.Empty;
            //客户名称
            mcbClientName.Clear();
            //是否价格含税
            ckSO_IsPriceIncludeTax.CheckState = CheckState.Unchecked;
            //税率
            txtSO_TaxRate.Clear();
            //税额
            txtSO_TotalTax.Clear();
            //总金额
            txtSO_TotalAmount.Clear();
            //未税总金额
            txtSO_TotalNetAmount.Clear();
            //配件价格类别
            mcbAutoPartsPriceType.Clear();
            //业务员
            mcbSO_SalesByName.Clear();
            //备注
            txtSO_Remark.Clear();
            //有效
            ckSO_IsValid.Checked = true;
            ckSO_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtSO_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtSO_CreatedTime.Value = BLLCom.GetCurStdDatetime();
            //修改人
            txtSO_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtSO_UpdatedTime.Value = BLLCom.GetCurStdDatetime();
            //销售订单ID
            txtSO_ID.Clear();
            //版本号
            txtSO_VersionNo.Clear();
            //汽修商组织ID
            _clientOrgID = string.Empty;
            //汽修商组织Code
            _clientOrgCode = string.Empty;
            //汽修商户组织联系方式
            txtAROrgPhone.Clear();
            //汽修商户组织地址
            txtAROrgAddress.Clear();
            //信用额度
            txtCreditAmount.Clear();
            //欠款金额
            txtDebtAmount.Clear();
            //钱包账号
            txtWal_No.Clear();
            //钱包可用余额
            txtWal_AvailableBalance.Clear();

            //给 单据编号 设置焦点
            lblSO_No.Focus();

            //清空[明细]Grid
            _detailGridDS = new SkyCarBindingList<SalesOrderDetailUIModel, MDLSD_SalesOrderDetail>();
            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _detailGridDS.StartMonitChanges();

            //初始化[出库明细]Grid
            _stockOutDetailDS = new List<SalesStockOutDetailUIModel>();
            gdStockOutDetail.DataSource = _stockOutDetailDS;
            gdStockOutDetail.DataBind();
            #endregion

            #region 初始化下拉框
            //客户类型
            _customerTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.CustomerType);
            mcbClientType.DisplayMember = SysConst.EN_TEXT;
            mcbClientType.ValueMember = SysConst.EN_Code;
            mcbClientType.DataSource = _customerTypeList;

            //汽修商户名称
            _autoFactoryList = CacheDAX.Get(CacheDAX.ConfigDataKey.ARMerchant) as List<MDLPIS_AutoFactoryCustomer>;
            if (_autoFactoryList != null)
            {
                var autoFactoryList = _autoFactoryList.Where(x => x.AFC_IsPlatform == true).ToList();
                mcbAutoFactoryName.DisplayMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Name;
                mcbAutoFactoryName.ValueMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Code;
                mcbAutoFactoryName.DataSource = autoFactoryList;
            }

            //来源类型
            _salesOrderSourceTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.SalesOrderSourceType);
            //新增时只考虑【在线销售，手工创建】
            List<ComComboBoxDataSourceTC> tempSalesOrderSourceTypeList =
                _salesOrderSourceTypeList.Where(
                    x => x.Text == SalesOrderSourceTypeEnum.Name.ZXXS
                    || x.Text == SalesOrderSourceTypeEnum.Name.SGCJ)
                    .ToList();
            cbSO_SourceTypeCode.DisplayMember = SysConst.EN_TEXT;
            cbSO_SourceTypeCode.ValueMember = SysConst.EN_Code;
            cbSO_SourceTypeCode.DataSource = tempSalesOrderSourceTypeList;
            cbSO_SourceTypeCode.DataBind();

            //单据状态
            _salesOrderStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.SalesOrderStatus);
            cbSO_StatusCode.DisplayMember = SysConst.EN_TEXT;
            cbSO_StatusCode.ValueMember = SysConst.EN_Code;
            cbSO_StatusCode.DataSource = _salesOrderStatusList;
            cbSO_StatusCode.DataBind();

            //审核状态
            _approveStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.ApprovalStatus);
            cbSO_ApprovalStatusCode.DisplayMember = SysConst.EN_TEXT;
            cbSO_ApprovalStatusCode.ValueMember = SysConst.EN_Code;
            cbSO_ApprovalStatusCode.DataSource = _approveStatusList;
            cbSO_ApprovalStatusCode.DataBind();

            //配件价格类别（从码表获取）
            _autoPartsPriceTypeList = CodeTableHelp.GetEnumForComboBoxWithValueText(CodeType.AutoPartsPriceType);
            mcbAutoPartsPriceType.DisplayMember = SysConst.EN_TEXT;
            mcbAutoPartsPriceType.ValueMember = SysConst.Value;
            mcbAutoPartsPriceType.DataSource = _autoPartsPriceTypeList;

            //业务员
            _salesByList = CacheDAX.Get(CacheDAX.ConfigDataKey.SystemUser) as List<MDLSM_User>;
            mcbSO_SalesByName.DisplayMember = SystemTableColumnEnums.SM_User.Code.User_Name;
            mcbSO_SalesByName.ValueMember = SystemTableColumnEnums.SM_User.Code.User_ID;
            mcbSO_SalesByName.DataSource = _salesByList;

            //异动类型
            _inventoryTransTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.InventoryTransType);
            #endregion

            #region 相关默认设置
            //默认[来源类型]为[手工创建]
            cbSO_SourceTypeCode.Value = SalesOrderSourceTypeEnum.Code.SGCJ;
            cbSO_SourceTypeCode.Text = SalesOrderSourceTypeEnum.Name.SGCJ;

            //默认[客户类型]为[平台内汽修商]
            mcbClientType.SelectedValue = CustomerTypeEnum.Code.PTNQXSH;

            //默认[审核状态]为[待审核]
            cbSO_ApprovalStatusCode.Value = ApprovalStatusEnum.Code.DSH;
            cbSO_ApprovalStatusCode.Text = ApprovalStatusEnum.Name.DSH;

            //默认[单据状态]为[已生成]
            cbSO_StatusCode.Value = SalesOrderStatusEnum.Code.YSC;
            cbSO_StatusCode.Text = SalesOrderStatusEnum.Name.YSC;

            //默认组织为当前组织
            txtSO_Org_ID.Text = LoginInfoDAX.OrgID;
            txtOrg_ShortName.Text = LoginInfoDAX.OrgShortName;

            #endregion

            //解决bug4481
            //if (_isHasInventory == false)
            //{
            //    //无进销存的场合，隐藏[出库明细]
            //    gbStockOutDetail.Visible = false;
            //    gbStockOutDetail.Expanded = false;
            //}
            //else
            //{
            //    //有进销存的场合，显示[出库明细]
            //    gbStockOutDetail.Visible = true;
            //    gbStockOutDetail.Expanded = true;
            //}

        }
        
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //单据编号
            txtWhere_SO_No.Clear();
            //客户类型
            mcbWhereClientType.Clear();
            //客户名称
            mcbWhereClientName.Clear();
            //来源类型
            cbWhere_SO_SourceTypeCode.Value = null;
            //单据状态
            cbWhere_SO_StatusCode.Value = null;
            //审核状态
            cbWhere_SO_ApprovalStatusCode.Value = null;
            //创建时间
            dtWhere_SalesOrderCreateStart.Value = null;
            dtWhere_SalesOrderCreateEnd.Value = null;
            //给 单据编号 设置焦点
            lblWhere_SO_No.Focus();
            #endregion

            #region Grid初始化

            //清空Grid
            HeadGridDS = new BindingList<SalesOrderManagerUIModel>();
            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();

            #endregion

            #endregion

            #region 初始化下拉框

            //客户类型
            mcbWhereClientType.DisplayMember = SysConst.EN_TEXT;
            mcbWhereClientType.ValueMember = SysConst.EN_Code;
            mcbWhereClientType.DataSource = _customerTypeList;

            //来源类型
            var tempSalesOrderSourceTypeList = _salesOrderSourceTypeList.Where(x =>
                         x.Text == SalesOrderSourceTypeEnum.Name.ZDXS || x.Text == SalesOrderSourceTypeEnum.Name.XSYC ||
                         x.Text == SalesOrderSourceTypeEnum.Name.SGCJ || x.Text == SalesOrderSourceTypeEnum.Name.ZXXS)
                    .ToList();
            cbWhere_SO_SourceTypeCode.DisplayMember = SysConst.EN_TEXT;
            cbWhere_SO_SourceTypeCode.ValueMember = SysConst.EN_Code;
            cbWhere_SO_SourceTypeCode.DataSource = tempSalesOrderSourceTypeList;
            cbWhere_SO_SourceTypeCode.DataBind();

            //单据状态
            cbWhere_SO_StatusCode.DisplayMember = SysConst.EN_TEXT;
            cbWhere_SO_StatusCode.ValueMember = SysConst.EN_Code;
            cbWhere_SO_StatusCode.DataSource = _salesOrderStatusList;
            cbWhere_SO_StatusCode.DataBind();

            //审核状态
            cbWhere_SO_ApprovalStatusCode.DisplayMember = SysConst.EN_TEXT;
            cbWhere_SO_ApprovalStatusCode.ValueMember = SysConst.EN_Code;
            cbWhere_SO_ApprovalStatusCode.DataSource = _approveStatusList;
            cbWhere_SO_ApprovalStatusCode.DataBind();
            #endregion
        }

        /// <summary>
        /// 将Grid中选中的数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetGridDataToCardCtrls()
        {
            //判断是否允许将【列表】Grid数据设置到【详情】Tab内的对应控件
            if (!IsAllowSetGridDataToCard())
            {
                return;
            }

            SetCardCtrlsToDetailDS();
            base.NewUIModel = HeadDS;

            var activeRowIndex = gdGrid.ActiveRow.Index;
            //判断Grid内[唯一标识]是否为空
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SD_SalesOrder.Code.SO_ID].Value == null
                || string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SD_SalesOrder.Code.SO_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            HeadDS = base.HeadGridDS.FirstOrDefault(x => x.SO_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.SD_SalesOrder.Code.SO_ID].Value.ToString());
            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.SO_ID))
            {
                return;
            }

            if (HeadDS.SO_SourceTypeName == SalesOrderSourceTypeEnum.Name.ZDXS
                || HeadDS.SO_SourceTypeName == SalesOrderSourceTypeEnum.Name.XSYC)
            {
                cbSO_SourceTypeCode.DisplayMember = SysConst.EN_TEXT;
                cbSO_SourceTypeCode.ValueMember = SysConst.EN_Code;
                cbSO_SourceTypeCode.DataSource = _salesOrderSourceTypeList;
                cbSO_SourceTypeCode.DataBind();
            }
            else
            {
                //来源类型：新增时只考虑【在线销售，手工创建】
                List<ComComboBoxDataSourceTC> tempSalesOrderSourceTypeList =
                    _salesOrderSourceTypeList.Where(x => x.Text == SalesOrderSourceTypeEnum.Name.ZXXS
                        || x.Text == SalesOrderSourceTypeEnum.Name.SGCJ)
                        .ToList();
                cbSO_SourceTypeCode.DisplayMember = SysConst.EN_TEXT;
                cbSO_SourceTypeCode.ValueMember = SysConst.EN_Code;
                cbSO_SourceTypeCode.DataSource = tempSalesOrderSourceTypeList;
                cbSO_SourceTypeCode.DataBind();
            }
            
            if (txtSO_ID.Text != HeadDS.SO_ID
                || (txtSO_ID.Text == HeadDS.SO_ID && txtSO_VersionNo.Text != HeadDS.SO_VersionNo?.ToString()))
            {
                if (txtSO_ID.Text == HeadDS.SO_ID && txtSO_VersionNo.Text != HeadDS.SO_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged(_skipPropertyList)
                || _detailGridDS.InsertList.Count > 0
                || _detailGridDS.UpdateList.Count > 0
                || _detailGridDS.DeleteList.Count > 0)
                {
                    //将放弃之前的修改，是否继续？
                    DialogResult dialogResult = MessageBoxs.Show(Trans.SD, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.ConfirmGiveUpEdit }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dialogResult != DialogResult.OK)
                    {
                        //选中【详情】Tab
                        tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;
                        return;
                    }
                }
                //将DetailDS数据赋值给【详情】Tab内的对应控件
                SetDetailDSToCardCtrls();
            }

            //选中【详情】Tab
            tabControlFull.Tabs[SysConst.EN_DETAIL].Selected = true;

            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //根据[来源类型]设置[来源单号]是否显示
            SetDetailVisible();

            //查询明细Grid数据并绑定
            QueryDetail();

            //获取出库明细列表
            GetStockOutDetail();

            //设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();
        }

        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //单据编号
            txtSO_No.Text = HeadDS.SO_No;
            //组织ID
            txtSO_Org_ID.Text = HeadDS.SO_Org_ID;
            //来源类型编码
            cbSO_SourceTypeCode.Value = HeadDS.SO_SourceTypeCode;
            //来源类型名称
            cbSO_SourceTypeCode.Text = HeadDS.SO_SourceTypeName;
            //来源单号
            txtSO_SourceNo.Text = HeadDS.SO_SourceNo;
            //客户类型编码
            mcbClientType.SelectedValue = HeadDS.SO_CustomerTypeCode;
            //客户名称和ID
            mcbClientName.SelectedValue = HeadDS.SO_CustomerID;
            if (HeadDS.SO_CustomerTypeCode == CustomerTypeEnum.Code.PTNQXSH)
            {
                lblAutoFactoryName.Visible = true;
                mcbAutoFactoryName.Visible = true;
                mcbAutoFactoryName.SelectedValue = HeadDS.AutoFactoryCode;
            }
            else
            {
                lblAutoFactoryName.Visible = false;
                mcbAutoFactoryName.Visible = false;
                mcbAutoFactoryName.Clear();
            }
            //是否价格含税
            if (HeadDS.SO_IsPriceIncludeTax != null)
            {
                ckSO_IsPriceIncludeTax.Checked = HeadDS.SO_IsPriceIncludeTax.Value;
            }
            //税率
            txtSO_TaxRate.Text = HeadDS.SO_TaxRate?.ToString();
            //税额
            txtSO_TotalTax.Text = HeadDS.SO_TotalTax?.ToString();
            //总金额
            txtSO_TotalAmount.Text = HeadDS.SO_TotalAmount?.ToString();
            //未税总金额
            txtSO_TotalNetAmount.Text = HeadDS.SO_TotalNetAmount?.ToString();
            //单据状态编码
            cbSO_StatusCode.Value = HeadDS.SO_StatusCode;
            //单据状态名称
            cbSO_StatusCode.Text = HeadDS.SO_StatusName;
            //审核状态编码
            cbSO_ApprovalStatusCode.Value = HeadDS.SO_ApprovalStatusCode;
            //审核状态名称
            cbSO_ApprovalStatusCode.Text = HeadDS.SO_ApprovalStatusName;
            //配件价格类别
            mcbAutoPartsPriceType.SelectedText = HeadDS.SO_AutoPartsPriceType;
            _autoPartsPriceTypeOfCustomer = HeadDS.AutoPartsPriceType;
            //业务员
            mcbSO_SalesByName.SelectedValue = HeadDS.SO_SalesByID;
            //备注
            txtSO_Remark.Text = HeadDS.SO_Remark;
            //有效
            if (HeadDS.SO_IsValid != null)
            {
                ckSO_IsValid.Checked = HeadDS.SO_IsValid.Value;
            }
            //创建人
            txtSO_CreatedBy.Text = HeadDS.SO_CreatedBy;
            //创建时间
            dtSO_CreatedTime.Value = HeadDS.SO_CreatedTime;
            //修改人
            txtSO_UpdatedBy.Text = HeadDS.SO_UpdatedBy;
            //修改时间
            dtSO_UpdatedTime.Value = HeadDS.SO_UpdatedTime;
            //销售订单ID
            txtSO_ID.Text = HeadDS.SO_ID;
            //版本号
            txtSO_VersionNo.Text = HeadDS.SO_VersionNo?.ToString();

            //组织名称
            txtOrg_ShortName.Text = HeadDS.Org_ShortName;
            //应收金额
            txtARB_AccountReceivableAmount.Text = HeadDS.AccountReceivableAmount?.ToString();
            //已收金额
            txtARB_ReceivedAmount.Text = HeadDS.ReceivedAmount?.ToString();
            //未收金额
            txtARB_UnReceiveAmount.Text = HeadDS.UnReceiveAmount?.ToString();
            //汽修商户组织ID
            _clientOrgID = HeadDS.AROrgID;
            //汽修商户组织编码
            _clientOrgCode = HeadDS.AROrgCode;
            //汽修商户组织联系方式
            txtAROrgPhone.Text = HeadDS.AROrgPhone;
            //汽修商户组织地址
            txtAROrgAddress.Text = HeadDS.AROrgAddress;
            //信用额度
            txtCreditAmount.Text = HeadDS.CreditAmount?.ToString();
            //欠款金额
            txtDebtAmount.Text = HeadDS.DebtAmount?.ToString();
            //钱包账号
            txtWal_No.Text = HeadDS.Wal_No;
            //钱包可用余额
            txtWal_AvailableBalance.Text = HeadDS.Wal_AvailableBalance?.ToString();
        }

        /// <summary>
        /// 查询明细Grid数据并绑定
        /// </summary>
        private void QueryDetail()
        {
            //销售订单明细列表
            _bll.QueryForList(SQLID.SD_SalesOrder_SQL01, new SalesOrderManagerQCModel
            {
                WHERE_SOD_SO_ID = txtSO_ID.Text.Trim()
            }, _detailGridDS);

            #region 设置销售单价是否可编辑

            //当前价格类别在配件价格明细中已配置价格的场合，不能改变单价
            for (int i = 0; i < gdDetail.Rows.Count; i++)
            {
                if (gdDetail.Rows[i].Cells["UnitPriceIsAllowEdit"].Value?.ToString() == SysConst.True)
                {
                    gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Activation = Activation.AllowEdit;
                }
                else
                {
                    gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Activation = Activation.ActivateOnly;
                }
            }

            #endregion

            //3.开始监控List变化（该数据源若有其他调整，请在StartMonitChanges()方法执行前调整）
            _detailGridDS.StartMonitChanges();

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();

            //设置销售订单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
        }

        /// <summary>
        /// 是否允许将【列表】Grid数据设置到【详情】Tab内的对应控件
        /// </summary>
        /// <returns>true:允许；false：不允许</returns>
        private bool IsAllowSetGridDataToCard()
        {
            if (gdGrid.ActiveRow == null || gdGrid.ActiveRow.Index < 0)
            {
                InitializeDetailTabControls();
                return false;
            }
            if (gdGrid.DisplayLayout.Bands[0].SortedColumns.Count > 0)
            {
                foreach (UltraGridColumn loopUltraGridColumn in gdGrid.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (loopUltraGridColumn.IsGroupByColumn)
                    {
                        InitializeDetailTabControls();
                        return false;
                    }
                }
            }
            return true;
        }
       
        /// <summary>
        /// 前端检查-保存
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForSave()
        {
            //来源类型不能为空
            if (string.IsNullOrEmpty(cbSO_SourceTypeCode.Text))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, MsgParam.SOURCE_TYPE), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //客户类型不能为空
            if (string.IsNullOrEmpty(mcbClientType.SelectedText))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, MsgParam.CUST_TYPE), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //客户名称不能为空
            if (string.IsNullOrEmpty(mcbClientName.SelectedText.Trim())
                || string.IsNullOrEmpty(mcbClientName.SelectedValue.Trim()))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, SystemTableColumnEnums.SD_SalesOrder.Name.SO_CustomerName), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //来源类型为{平台内汽修商}的场合，汽修商组织不能为空
            if (mcbClientType.SelectedText == CustomerTypeEnum.Name.PTNQXSH
                && string.IsNullOrEmpty(_clientOrgID))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.AUTOFACTORY_ORG }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            #region 验证销售订单明细的信息

            if (gdDetail.Rows.Count == 0)
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0014, new object[] { SystemTableEnums.Name.SD_SalesOrderDetail }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            for (int i = 0; i < gdDetail.Rows.Count; i++)
            {
                //验证数量
                if (gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Value == null
                    || gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Value.ToString() == "0")
                {
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.SD_SalesOrderDetail.Name.SOD_Qty, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (!BLLCom.IsDecimal(gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Value.ToString()))
                {
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.SD_SalesOrderDetail.Name.SOD_Qty, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //验证单价
                if (gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Value == null)
                {
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.SD_SalesOrderDetail.Name.SOD_UnitPrice, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (!BLLCom.IsDecimal(gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Value.ToString()))
                {
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.SD_SalesOrderDetail.Name.SOD_UnitPrice, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                //如果价格含税
                if (gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Text == SysConst.True)
                {
                    //验证税率
                    if (!BLLCom.IsDecimal(gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Value?.ToString()))
                    {
                        MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.SD_SalesOrderDetail.Name.SOD_TaxRate, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
                //计价基准可改
                if (gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRateIsChangeable].Text == SysConst.True)
                {
                    //验证计价基准
                    if (!BLLCom.IsDecimal(gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Value?.ToString()))
                    {
                        MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.SD_SalesOrderDetail.Name.SOD_SalePriceRate, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }

            #endregion

            return true;
        }

        /// <summary>
        /// 前端检查-删除
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForDelete()
        {
            //确认删除操作
            DialogResult dialogResult = MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.OK)
            {
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            HeadDS = new SalesOrderManagerUIModel()
            {
                //单据编号
                SO_No = txtSO_No.Text.Trim(),
                //组织ID
                SO_Org_ID = string.IsNullOrEmpty(txtSO_Org_ID.Text.Trim()) ? LoginInfoDAX.OrgID : txtSO_Org_ID.Text.Trim(),
                //来源类型编码
                SO_SourceTypeCode = cbSO_SourceTypeCode.Value?.ToString() ?? "",
                //来源类型名称
                SO_SourceTypeName = cbSO_SourceTypeCode.Text,
                //来源单号
                SO_SourceNo = txtSO_SourceNo.Text.Trim(),
                //客户类型编码
                SO_CustomerTypeCode = mcbClientType.SelectedValue,
                //客户类型名称
                SO_CustomerTypeName = mcbClientType.SelectedText,
                //客户名称
                SO_CustomerName = mcbClientName.SelectedText,
                //客户ID
                SO_CustomerID = mcbClientName.SelectedValue,
                //是否价格含税
                SO_IsPriceIncludeTax = ckSO_IsPriceIncludeTax.Checked,
                //税率
                SO_TaxRate = Convert.ToDecimal(txtSO_TaxRate.Text.Trim() == "" ? "0" : txtSO_TaxRate.Text.Trim()),
                //税额
                SO_TotalTax = Convert.ToDecimal(txtSO_TotalTax.Text.Trim() == "" ? "0" : txtSO_TotalTax.Text.Trim()),
                //总金额
                SO_TotalAmount = Convert.ToDecimal(txtSO_TotalAmount.Text.Trim() == "" ? "0" : txtSO_TotalAmount.Text.Trim()),
                //未税总金额
                SO_TotalNetAmount = Convert.ToDecimal(txtSO_TotalNetAmount.Text.Trim() == "" ? "0" : txtSO_TotalNetAmount.Text.Trim()),
                //单据状态编码
                SO_StatusCode = cbSO_StatusCode.Value?.ToString() ?? "",
                //单据状态名称
                SO_StatusName = cbSO_StatusCode.Text.Trim(),
                //审核状态编码
                SO_ApprovalStatusCode = cbSO_ApprovalStatusCode.Value?.ToString() ?? "",
                //审核状态名称
                SO_ApprovalStatusName = cbSO_ApprovalStatusCode.Text.Trim(),
                //配件价格类别
                SO_AutoPartsPriceType = mcbAutoPartsPriceType.SelectedText,
                //业务员
                SO_SalesByID = mcbSO_SalesByName.SelectedValue,
                SO_SalesByName = mcbSO_SalesByName.SelectedText,
                //备注
                SO_Remark = txtSO_Remark.Text.Trim(),
                //有效
                SO_IsValid = ckSO_IsValid.Checked,
                //创建人
                SO_CreatedBy = txtSO_CreatedBy.Text.Trim(),
                //创建时间
                SO_CreatedTime = (DateTime?)dtSO_CreatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //修改人
                SO_UpdatedBy = txtSO_UpdatedBy.Text.Trim(),
                //修改时间
                SO_UpdatedTime = (DateTime?)dtSO_UpdatedTime.Value ?? BLLCom.GetCurStdDatetime(),
                //销售订单ID
                SO_ID = txtSO_ID.Text.Trim(),
                //版本号
                SO_VersionNo = Convert.ToInt64(txtSO_VersionNo.Text.Trim() == "" ? "1" : txtSO_VersionNo.Text.Trim()),

                //组织名称
                Org_ShortName = txtOrg_ShortName.Text.Trim(),
                //信用额度
                CreditAmount = Convert.ToDecimal(txtCreditAmount.Text.Trim() == "" ? "0" : txtCreditAmount.Text.Trim()),
                //欠款金额
                DebtAmount = Convert.ToDecimal(txtDebtAmount.Text.Trim() == "" ? "0" : txtDebtAmount.Text.Trim()),
                //汽修商户编码
                AutoFactoryCode = mcbAutoFactoryName.SelectedValue,
                //汽修商户名称
                AutoFactoryName = mcbAutoFactoryName.SelectedText,
                //汽修商户组织ID
                AROrgID = _clientOrgID,
                //汽修商户组织编码
                AROrgCode = _clientOrgCode,
                //汽修商户组织名称
                AROrgName = mcbClientName.SelectedText,
                //汽修商户组织联系方式
                AROrgPhone = txtAROrgPhone.Text.Trim(),
                //汽修商户组织地址
                AROrgAddress = txtAROrgAddress.Text.Trim(),
                //钱包账号
                Wal_No = txtWal_No.Text.Trim(),
                //钱包可用余额
                Wal_AvailableBalance = Convert.ToDecimal(txtWal_AvailableBalance.Text.Trim() == "" ? "0" : txtWal_AvailableBalance.Text.Trim()),
            };
        }

        /// <summary>
        /// 设置详情页面控件的是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            if (string.IsNullOrEmpty(txtSO_ID.Text.Trim()))
            {
                #region 销售订单未保存

                //根据[是否存在明细]控制单头是否可编辑
                SetDetailByIsExistDetail();
                mcbSO_SalesByName.Enabled = true;
                txtSO_Remark.Enabled = true;

                #region 销售明细列表相关

                //明细列表可添加、删除、更新
                toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = true;
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;

                //[销售明细]列表中不显示[汽修商组织]、[汽修商仓库]、[汽修商仓位]、[批次号（汽修）]列
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInOrgName].Hidden = true;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInWarehouseName].Hidden = true;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInBinName].Hidden = true;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_BatchNoNew].Hidden = true;

                //明细列表[签收数量]、[拒收数量]、[丢失数量]不显示，不可编辑
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Hidden = true;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Hidden = true;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Hidden = true;

                #endregion

                #region 出库明细列表相关

                //[出库明细]列表中[出库数量]可编辑
                SeStockOutDetailStyle(false);

                #endregion

                #region 导航按钮相关

                //导航按钮[转结算]、[转物流]和[在线支付]不可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.ONLINEPAY, true, false);
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOLOGISTICSBILL, true, false);
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, false);

                #endregion

                #endregion
            }
            else
            {
                #region 销售订单已保存

                //根据[来源类型]区分
                if (cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.ZXXS
                    || cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.SGCJ)
                {
                    #region [来源类型]为[在线销售]，[手工创建]的场合

                    //判断[审核状态]
                    if (cbSO_ApprovalStatusCode.Text == ApprovalStatusEnum.Name.DSH)
                    {
                        #region [待审核]的场合

                        //根据[是否存在明细]控制单头是否可编辑
                        SetDetailByIsExistDetail();
                        mcbSO_SalesByName.Enabled = true;
                        txtSO_Remark.Enabled = true;

                        //明细列表可添加、删除、更新
                        toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = true;
                        toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;

                        //明细列表[签收数量]、[拒收数量]、[丢失数量]不显示
                        gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Hidden = true;
                        gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Hidden = true;
                        gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Hidden = true;

                        //[出库明细]列表中[出库数量]可编辑
                        SeStockOutDetailStyle(false);

                        //导航按钮[转结算]、[转物流]和[在线支付]不可用
                        SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.ONLINEPAY, true, false);
                        SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOLOGISTICSBILL, true, false);
                        SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, false);

                        #endregion
                    }
                    else
                    {
                        #region [已审核]的场合

                        //单头不可编辑
                        cbSO_SourceTypeCode.Enabled = false;
                        txtSO_SourceNo.Enabled = false;
                        mcbClientType.Enabled = false;
                        mcbClientName.Enabled = false;
                        mcbAutoFactoryName.Enabled = false;
                        mcbAutoPartsPriceType.Enabled = false;
                        mcbSO_SalesByName.Enabled = false;
                        txtSO_Remark.Enabled = false;

                        //明细列表不可添加、删除、更新
                        toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = false;
                        toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;

                        //明细列表[签收数量]、[拒收数量]、[丢失数量]显示
                        gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Hidden = false;
                        gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Hidden = false;
                        gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Hidden = false;

                        //[出库明细]列表中[出库数量]不可编辑
                        SeStockOutDetailStyle(true);

                        //[单据状态]为{交易成功}的场合，[在线支付]、[转结算]可用，否则不可用
                        SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.JYCG);
                        SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.ONLINEPAY, true, cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.JYCG);
                        //[单据状态]为{待发货}的场合，[转物流]可用，否则不可用
                        SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOLOGISTICSBILL, true, cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.DFH);

                        #endregion
                    }

                    //[销售明细]列表中不显示[汽修商组织]、[汽修商仓库]、[汽修商仓位]、[批次号（汽修）]列
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInOrgName].Hidden = true;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInWarehouseName].Hidden = true;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInBinName].Hidden = true;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_BatchNoNew].Hidden = true;

                    #endregion
                }
                else
                {
                    #region [来源类型]为[主动销售]，[销售预测] 的场合，只能查看不能编辑详情

                    //详情不可编辑
                    cbSO_SourceTypeCode.Enabled = false;
                    txtSO_SourceNo.Enabled = false;
                    mcbClientType.Enabled = false;
                    mcbClientName.Enabled = false;
                    mcbAutoFactoryName.Enabled = false;
                    mcbAutoPartsPriceType.Enabled = false;
                    mcbSO_SalesByName.Enabled = false;
                    txtSO_Remark.Enabled = false;

                    //明细列表不可添加、删除、更新
                    toolbarsManagerDetail.Tools[SysConst.EN_ADD].SharedProps.Enabled = false;
                    toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;

                    #region 修改前代码

                    //[出库明细]列表中[出库数量]不可编辑
                    SeStockOutDetailStyle(true);

                    #endregion

                    //根据[审核状态]判断导航按钮是否可用
                    if (cbSO_ApprovalStatusCode.Text == ApprovalStatusEnum.Name.DSH)
                    {
                        #region [待审核]的场合

                        //导航按钮[转结算]、[转物流]、[在线支付]不可用
                        SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.ONLINEPAY, true, false);
                        SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOLOGISTICSBILL, true, false);
                        SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, false);

                        #endregion
                    }
                    else
                    {
                        #region [已审核]的场合

                        if (cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.JYCG
                            || cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.YGB)
                        {
                            //[销售订单].[单据状态]等于{交易成功}和{已关闭}的场合，导航按钮[在线支付]不可用
                            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.ONLINEPAY, true, false);
                        }
                        else
                        {
                            //[销售订单].[单据状态]不等于{交易成功}和{已关闭}的场合，导航按钮[在线支付]可用
                            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.ONLINEPAY, true, true);
                        }

                        //[单据状态]为{交易成功}的场合，[转结算]可用，否则不可用
                        SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true,
                            cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.JYCG);
                        //[单据状态]为{待发货}的场合，[转物流]可用，否则不可用
                        SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOLOGISTICSBILL, true,
                            cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.DFH);
                        #endregion
                    }

                    //[来源类型]为[主动销售]、[销售预测]的场合，明细列表显示[汽修商组织]、[汽修商仓库]、[汽修商仓位]、[批次号（汽修）]列
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInOrgName].Hidden = false;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInWarehouseName].Hidden = false;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInBinName].Hidden = false;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_BatchNoNew].Hidden = false;
                    #endregion
                }

                #endregion
            }
        }
        
        /// <summary>
        /// 设置单元格是否可以编辑
        /// </summary>
        private void SetSalesOrderDetailStyle()
        {
            #region 设置Grid数据颜色
            gdDetail.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
            foreach (var loopGridRow in gdDetail.Rows)
            {
                if (!string.IsNullOrEmpty(txtSO_ID.Text.Trim()))
                {
                    #region 销售订单已保存

                    //根据[来源类型]区分
                    if (cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.ZXXS
                    || cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.SGCJ)
                    {
                        #region [来源类型]为[在线销售]，[手工创建]的场合

                        //判断[审核状态]
                        if (cbSO_ApprovalStatusCode.Text == ApprovalStatusEnum.Name.DSH)
                        {
                            #region [待审核]的场合

                            #region 明细列表[数量]、[单价]、[计价基准可改]、[计价基准]、[价格是否含税]、[税率]、[备注]列可编辑

                            #region 数量

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Activation = Activation.AllowEdit;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                            #endregion

                            #region 单价

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Activation = Activation.AllowEdit;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                            #endregion

                            #region 计价基准可改

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRateIsChangeable].Activation = Activation.AllowEdit;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRateIsChangeable].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRateIsChangeable].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                            #endregion

                            #region 计价基准

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Activation = Activation.AllowEdit;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                            #endregion

                            #region 价格是否含税

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Activation = Activation.AllowEdit;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                            #endregion

                            #region 税率

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Activation = Activation.AllowEdit;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                            #endregion

                            #region 备注

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Activation = Activation.AllowEdit;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                            #endregion

                            #endregion

                            #region 明细列表[签收数量]、[拒收数量]、[丢失数量]不可编辑

                            #region 签收数量

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Activation = Activation.ActivateOnly;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                            #endregion

                            #region 拒收数量

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Activation = Activation.ActivateOnly;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                            #endregion

                            #region 丢失数量

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Activation = Activation.ActivateOnly;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                            #endregion

                            #endregion

                            #endregion
                        }
                        else
                        {
                            #region [已审核]的场合

                            #region 明细列表[数量]、[单价]、[计价基准可改]、[计价基准]、[价格是否含税]、[税率]、[备注]列不可编辑

                            #region 数量

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Activation = Activation.ActivateOnly;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                            #endregion

                            #region 单价

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Activation = Activation.ActivateOnly;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                            #endregion

                            #region 计价基准可改

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRateIsChangeable].Activation = Activation.ActivateOnly;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRateIsChangeable].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRateIsChangeable].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                            #endregion

                            #region 计价基准

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Activation = Activation.ActivateOnly;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                            #endregion

                            #region 价格是否含税

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Activation = Activation.ActivateOnly;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                            #endregion

                            #region 税率

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Activation = Activation.ActivateOnly;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                            #endregion

                            #region 备注

                            //设置单元格 
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Activation = Activation.ActivateOnly;
                            //设置单元格背景色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                            //设置单元格边框颜色
                            loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                            #endregion

                            #endregion

                            if (cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.JYCG
                            || cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.YGB)
                            {

                                #region {交易成功}、{已关闭}的场合，明细列表[签收数量]、[拒收数量]、[丢失数量]不可编辑

                                #region 签收数量

                                //设置单元格 
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Activation = Activation.ActivateOnly;
                                //设置单元格背景色
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                                //设置单元格边框颜色
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                                #endregion

                                #region 拒收数量

                                //设置单元格 
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Activation = Activation.ActivateOnly;
                                //设置单元格背景色
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                                //设置单元格边框颜色
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                                #endregion

                                #region 丢失数量

                                //设置单元格 
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Activation = Activation.ActivateOnly;
                                //设置单元格背景色
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                                //设置单元格边框颜色
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                                #endregion

                                #endregion

                            }
                            else
                            {
                                #region 不是{交易成功}、{已关闭}的场合，明细列表[签收数量]、[拒收数量]、[丢失数量]可编辑

                                #region 签收数量

                                //设置单元格
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Activation = Activation.AllowEdit;
                                //设置单元格背景色
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                                //设置单元格边框颜色
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                                #endregion

                                #region 拒收数量

                                //设置单元格
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Activation = Activation.AllowEdit;
                                //设置单元格背景色
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                                //设置单元格边框颜色
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                                #endregion

                                #region 丢失数量

                                //设置单元格 
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Activation = Activation.AllowEdit;
                                //设置单元格背景色
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                                //设置单元格边框颜色
                                loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                                #endregion

                                #endregion

                            }

                            #endregion
                        }

                        #endregion
                    }
                    else
                    {
                        #region [来源类型]为[主动销售]，[销售预测] 的场合，只能查看不能编辑详情

                        #region 数量

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Activation = Activation.ActivateOnly;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                        #endregion

                        #region 单价

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Activation = Activation.ActivateOnly;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                        #endregion

                        #region 计价基准可改

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRateIsChangeable].Activation = Activation.ActivateOnly;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRateIsChangeable].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRateIsChangeable].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                        #endregion

                        #region 计价基准

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Activation = Activation.ActivateOnly;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                        #endregion

                        #region 价格是否含税

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Activation = Activation.ActivateOnly;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                        #endregion

                        #region 税率

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Activation = Activation.ActivateOnly;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                        #endregion

                        #region 备注

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Activation = Activation.ActivateOnly;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                        #endregion

                        #region 明细列表[签收数量]、[拒收数量]、[丢失数量]不可编辑

                        #region 签收数量

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Activation = Activation.ActivateOnly;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                        #endregion

                        #region 拒收数量

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Activation = Activation.ActivateOnly;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                        #endregion

                        #region 丢失数量

                        //设置单元格 
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Activation = Activation.ActivateOnly;
                        //设置单元格背景色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                        //设置单元格边框颜色
                        loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                        #endregion

                        #endregion

                        #endregion
                    }

                    #endregion
                }
                else
                {
                    #region 明细列表[选择]、[数量]、[单价]、[计价基准可改]、[计价基准]、[价格是否含税]、[税率]、[备注]列可编辑

                    #region 销售数量

                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 单价

                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 计价基准可改

                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRateIsChangeable].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRateIsChangeable].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRateIsChangeable].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 计价基准

                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 价格是否含税

                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 税率

                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #region 备注

                    //设置单元格可编辑
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #endregion

                    #region 明细列表[签收数量]、[拒收数量]、[丢失数量]不显示，不可编辑

                    #region 签收数量

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SignQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;
                    #endregion

                    #region 拒收数量

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_RejectQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                    #region 丢失数量

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_LoseQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                    #endregion
                }
            }
            #endregion
        }
       
        /// <summary>
        /// 出库单单元格格式
        /// </summary>
        /// <param name="paramIsActivateOnly"></param>
        private void SeStockOutDetailStyle(bool paramIsActivateOnly)
        {
            gdStockOutDetail.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
            foreach (var loopGridRow in gdStockOutDetail.Rows)
            {
                if (paramIsActivateOnly)
                {
                    #region 出库数量

                    //设置单元格 
                    loopGridRow.Cells[SysConst.StockOutQty].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SysConst.StockOutQty].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SysConst.StockOutQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion
                }
                else
                {
                    #region 出库数量

                    //设置单元格 
                    loopGridRow.Cells[SysConst.StockOutQty].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SysConst.StockOutQty].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SysConst.StockOutQty].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion
                }
            }
        }

        #region [销售订单明细]相关

        /// <summary>
        /// 添加销售订单明细【库存】【配件条码和配件名称要具有唯一性】
        /// </summary>
        private void AddSalesDetail()
        {
            #region 验证

            //验证单据审核状态：销售订单已审核，不能添加明细
            if (cbSO_ApprovalStatusCode.Text == ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.SD_SalesOrder + ApprovalStatusEnum.Name.YSH, MsgParam.ADD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //验证客户类型
            if (string.IsNullOrEmpty(mcbClientType.SelectedText))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.CUST_TYPE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //验证客户名称
            if (string.IsNullOrEmpty(mcbClientName.SelectedText.Trim())
                || string.IsNullOrEmpty(mcbClientName.SelectedValue.Trim()))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.SD_SalesOrder.Name.SO_CustomerName }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //来源类型为{平台内汽修商}的场合，汽修商组织不能为空
            if (mcbClientType.SelectedText == CustomerTypeEnum.Name.PTNQXSH
                && string.IsNullOrEmpty(_clientOrgID))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.AUTOFACTORY_ORG }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            #endregion

            //获取添加[销售明细]的方式
            string getSalesOrderDetailType = GetSalesOrderDetailSrouceType();

            if (getSalesOrderDetailType == InsertSalesOrderDetailType.InventoryDirectly)
            {
                #region 库存中获取

                Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
                {
                    {ComViewParamKey.PickAutoPartsFunc.ToString(), _pickPartsDetailFunc},
                };

                FrmPickPartsQuery frmPickPartsQuery = new FrmPickPartsQuery(paramViewParameters, CustomEnums.CustomeSelectionMode.Multiple)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmPickPartsQuery.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                #endregion
            }
            else if (getSalesOrderDetailType == InsertSalesOrderDetailType.AutoPartsArchiveDirectly)
            {
                #region 配件档案中获取

                FrmAutoPartsArchiveQuery frmAutoPartsArchiveQuery = new FrmAutoPartsArchiveQuery(CustomEnums.CustomeSelectionMode.Multiple)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmAutoPartsArchiveQuery.ShowDialog();
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                List<MDLBS_AutoPartsArchive> selectedAutoPartsArchiveList = new List<MDLBS_AutoPartsArchive>();
                selectedAutoPartsArchiveList = frmAutoPartsArchiveQuery.SelectedGridList;

                #endregion

                //新增销售订单明细
                InsertSalesOrderDetail(InsertSalesOrderDetailType.AutoPartsArchiveDirectly, selectedAutoPartsArchiveList);
            }

            #region 计算单头各金额

            //明细总金额
            decimal detailAmount = 0;
            //明细总税额
            decimal detailTaxAmount = 0;

            foreach (var loopDetail in _detailGridDS)
            {
                detailAmount += (loopDetail.SOD_TotalAmount ?? 0);
                detailTaxAmount += (loopDetail.SOD_TotalTax ?? 0);
            }
            //单头总金额
            txtSO_TotalAmount.Text = detailAmount.ToString();
            //单头总税额
            txtSO_TotalTax.Text = detailTaxAmount.ToString();
            //单头未税总金额
            decimal tempNetAmount = detailAmount - detailTaxAmount;
            txtSO_TotalNetAmount.Text = tempNetAmount.ToString();
            //单头税率
            if ((detailAmount) != 0)
            {
                var tempTaxRate = Math.Round(Convert.ToDecimal((detailTaxAmount / detailAmount)), 2);
                txtSO_TaxRate.Text = tempTaxRate.ToString();
            }
            #endregion
        }

        /// <summary>
        /// 删除销售订单明细
        /// </summary>
        private void DeleteSalesDetail()
        {
            //验证单据审核状态：销售订单已审核，不能删除明细
            if (cbSO_ApprovalStatusCode.Text == ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.SD_SalesOrder + ApprovalStatusEnum.Name.YSH, SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //待删除的明细总金额
            decimal deleteDetailAmount = 0;
            //待删除明细总税额
            decimal deleteDetailTaxAmount = 0;

            gdDetail.UpdateData();
            //待删除的销售明细
            List<SalesOrderDetailUIModel> deleteSalesOrderDetailList = _detailGridDS.Where(p => p.IsChecked == true).ToList();

            if (deleteSalesOrderDetailList.Count == 0)
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SystemTableEnums.Name.SD_SalesOrderDetail, SystemActionEnum.Name.DELETE }),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < deleteSalesOrderDetailList.Count; i++)
            {
                //配件名称
                string autoPartName = deleteSalesOrderDetailList[i].SOD_Name;
                //配件条码
                string autoPartBarcode = deleteSalesOrderDetailList[i].SOD_Barcode;

                //待删除的[销售明细]列表
                List<SalesOrderDetailUIModel> deleteSalesOrderDetailCopyList = _detailGridDS.Where(p => p.SOD_Name == autoPartName && p.SOD_Barcode == autoPartBarcode).ToList();

                //待删除的[出库明细]列表
                List<SalesStockOutDetailUIModel> deleteStockOutDetailList = _stockOutDetailDS.Where(x => x.INV_Barcode == autoPartBarcode).ToList();

                if (deleteSalesOrderDetailCopyList.Count > 0)
                {
                    foreach (var loopSalesOrderDetail in deleteSalesOrderDetailCopyList)
                    {
                        deleteDetailAmount += (loopSalesOrderDetail.SOD_TotalAmount ?? 0);
                        deleteDetailTaxAmount += (loopSalesOrderDetail.SOD_TotalTax ?? 0);
                        _detailGridDS.Remove(loopSalesOrderDetail);
                    }
                    foreach (var loopStockOutDetail in deleteStockOutDetailList)
                    {
                        _stockOutDetailDS.Remove(loopStockOutDetail);
                    }
                }
            }
            //根据[是否存在明细]控制单头是否可编辑
            SetDetailByIsExistDetail();

            #region 计算单头各金额

            //原单头总金额
            decimal tempTotalAmount = Convert.ToDecimal(txtSO_TotalAmount.Text.Trim() == "" ? "0" : txtSO_TotalAmount.Text.Trim());
            //原单头总税额
            decimal tempTotalTaxAmount = Convert.ToDecimal(txtSO_TotalTax.Text.Trim() == "" ? "0" : txtSO_TotalTax.Text.Trim());

            //单头总金额
            txtSO_TotalAmount.Text = (tempTotalAmount - deleteDetailAmount).ToString();
            //单头总税额
            txtSO_TotalTax.Text = (tempTotalTaxAmount - deleteDetailTaxAmount).ToString();
            //单头未税总金额
            decimal tempNetAmount = (tempTotalAmount - deleteDetailAmount) - (tempTotalTaxAmount - deleteDetailTaxAmount);
            txtSO_TotalNetAmount.Text = tempNetAmount.ToString();
            //单头税率
            if ((tempTotalAmount - deleteDetailAmount) != 0)
            {
                var tempTaxRate = Math.Round(Convert.ToDecimal(((tempTotalTaxAmount - deleteDetailTaxAmount) / (tempTotalAmount - deleteDetailAmount))), 2);
                txtSO_TaxRate.Text = tempTaxRate.ToString();
            }
            #endregion

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            //设置销售订单明细Grid自适应列宽（根据单元格内容）
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;

            gdStockOutDetail.DataSource = _stockOutDetailDS;
            gdStockOutDetail.DataBind();
            //设置出库明细Grid自适应列宽（根据单元格内容）
            gdStockOutDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }

        /// <summary>
        /// 处理领料明细Func
        /// </summary>
        /// <param name="paramPickPartsDetail"></param>
        /// <returns></returns>
        private bool HandlePickPartsDetail(List<PickPartsQueryUIModel> paramPickPartsDetail)
        {
            InsertSalesOrderDetail(InsertSalesOrderDetailType.InventoryDirectly, paramPickPartsDetail);
            return true;
        }

        /// <summary>
        /// 新增销售订单明细
        /// </summary>
        /// <param name="paramInsertSalesOrderDetailType">销售订单新增方式</param>
        /// <param name="paramInsertSalesOrderDetialList">待新增销售订单明细列表</param>
        private void InsertSalesOrderDetail(string paramInsertSalesOrderDetailType, object paramInsertSalesOrderDetialList)
        {
            if (string.IsNullOrEmpty(paramInsertSalesOrderDetailType) || paramInsertSalesOrderDetialList == null)
            {
                return;
            }
            //待添加的销售明细列表
            List<SalesOrderDetailUIModel> salesOrderDetailListToAdd = new List<SalesOrderDetailUIModel>();

            //配件的条形码String
            string barcodeStr = string.Empty;

            #region 数据处理
            switch (paramInsertSalesOrderDetailType)
            {
                case InsertSalesOrderDetailType.AutoPartsArchiveDirectly:

                    #region 汽配商配件档案方式添加
                    List<MDLBS_AutoPartsArchive> selectedAutoPartsArchiveList = paramInsertSalesOrderDetialList as List<MDLBS_AutoPartsArchive>;
                    if (selectedAutoPartsArchiveList != null)
                    {
                        foreach (var loopAutoPartsArchive in selectedAutoPartsArchiveList)
                        {
                            //条码相同的配件合并
                            SalesOrderDetailUIModel sameSalesOrderDetail = salesOrderDetailListToAdd.FirstOrDefault(x => x.SOD_Barcode == loopAutoPartsArchive.APA_Barcode) ?? _detailGridDS.FirstOrDefault(x => x.SOD_Barcode == loopAutoPartsArchive.APA_Barcode);
                            if (sameSalesOrderDetail == null)
                            {
                                SalesOrderDetailUIModel tempSalesOrderDetail = new SalesOrderDetailUIModel
                                {
                                    SOD_Barcode = loopAutoPartsArchive.APA_Barcode,
                                    SOD_Name = loopAutoPartsArchive.APA_Name,
                                    SOD_Specification = loopAutoPartsArchive.APA_Specification,
                                    SOD_UOM = loopAutoPartsArchive.APA_UOM,
                                    //默认数量为1
                                    SOD_Qty = 1,
                                    APA_Brand = loopAutoPartsArchive.APA_Brand,
                                    APA_Level = loopAutoPartsArchive.APA_Level,
                                    APA_VehicleBrand = loopAutoPartsArchive.APA_VehicleBrand,
                                    APA_VehicleInspire = loopAutoPartsArchive.APA_VehicleInspire,
                                    APA_VehicleCapacity = loopAutoPartsArchive.APA_VehicleCapacity,
                                    APA_VehicleYearModel = loopAutoPartsArchive.APA_VehicleYearModel,
                                    APA_VehicleGearboxTypeCode = loopAutoPartsArchive.APA_VehicleGearboxTypeCode,
                                    APA_VehicleGearboxTypeName = loopAutoPartsArchive.APA_VehicleGearboxTypeName,
                                    INV_ThirdNo = loopAutoPartsArchive.APA_ThirdNo,
                                    INV_OEMNo = loopAutoPartsArchive.APA_OEMNo,
                                };
                                barcodeStr += tempSalesOrderDetail.SOD_Barcode + SysConst.Semicolon_DBC;
                                salesOrderDetailListToAdd.Add(tempSalesOrderDetail);
                            }
                            else
                            {
                                sameSalesOrderDetail.SOD_Qty = sameSalesOrderDetail.SOD_Qty + 1;
                                sameSalesOrderDetail.SOD_TotalAmount = Math.Round((sameSalesOrderDetail.SOD_Qty ?? 0) *
                                                                       (sameSalesOrderDetail.SOD_UnitPrice ?? 0), 2);
                            }
                        }
                    }
                    #endregion
                    break;

                case InsertSalesOrderDetailType.InventoryDirectly:

                    #region 汽配商进销存方式添加

                    List<PickPartsQueryUIModel> selectedPartsInventoryList = paramInsertSalesOrderDetialList as List<PickPartsQueryUIModel>;
                    if (selectedPartsInventoryList != null)
                    {
                        foreach (var loopAutoPartsInventory in selectedPartsInventoryList)
                        {
                            //条码相同的配件合并
                            SalesOrderDetailUIModel sameSalesOrderDetail = salesOrderDetailListToAdd.FirstOrDefault(x => x.SOD_Barcode == loopAutoPartsInventory.INV_Barcode) ?? _detailGridDS.FirstOrDefault(x => x.SOD_Barcode == loopAutoPartsInventory.INV_Barcode);
                            if (sameSalesOrderDetail == null)
                            {
                                //添加[销售明细]
                                SalesOrderDetailUIModel tempSalesOrderDetail = new SalesOrderDetailUIModel
                                {
                                    SOD_Barcode = loopAutoPartsInventory.INV_Barcode,
                                    SOD_Name = loopAutoPartsInventory.INV_Name,
                                    SOD_Specification = loopAutoPartsInventory.INV_Specification,
                                    SOD_UOM = loopAutoPartsInventory.APA_UOM,
                                    //默认数量为1
                                    SOD_Qty = 1,
                                    APA_Brand = loopAutoPartsInventory.APA_Brand,
                                    APA_Level = loopAutoPartsInventory.APA_Level,
                                    APA_VehicleBrand = loopAutoPartsInventory.APA_VehicleBrand,
                                    APA_VehicleInspire = loopAutoPartsInventory.APA_VehicleInspire,
                                    APA_VehicleCapacity = loopAutoPartsInventory.APA_VehicleCapacity,
                                    APA_VehicleYearModel = loopAutoPartsInventory.APA_VehicleYearModel,
                                    APA_VehicleGearboxTypeCode = loopAutoPartsInventory.APA_VehicleGearboxTypeCode,
                                    APA_VehicleGearboxTypeName = loopAutoPartsInventory.APA_VehicleGearboxTypeName,
                                    INV_ThirdNo = loopAutoPartsInventory.INV_ThirdNo,
                                    INV_OEMNo = loopAutoPartsInventory.INV_OEMNo,
                                };
                                barcodeStr += tempSalesOrderDetail.SOD_Barcode + SysConst.Semicolon_DBC;
                                salesOrderDetailListToAdd.Add(tempSalesOrderDetail);
                            }
                            else
                            {
                                sameSalesOrderDetail.SOD_Qty = sameSalesOrderDetail.SOD_Qty + 1;
                                sameSalesOrderDetail.SOD_TotalAmount = Math.Round((sameSalesOrderDetail.SOD_Qty ?? 0) *
                                                                       (sameSalesOrderDetail.SOD_UnitPrice ?? 0), 2);
                            }
                        }
                    }
                    #endregion
                    break;
            }
            #endregion

            //新增的数量
            int insertCount = 0;

            #region 设置明细数据

            if (!string.IsNullOrEmpty(mcbAutoPartsPriceType.SelectedText)
                && !string.IsNullOrEmpty(barcodeStr))
            {
                //根据配件条形码查询配件的价格明细
                List<string> resultAutoPartsPriceList = new List<string>();
                _bll.QueryForList(SQLID.COMM_SQL46, new MDLBS_AutoPartsPriceType
                {
                    WHERE_APPT_Barcode = barcodeStr,
                    WHERE_APPT_Name = mcbAutoPartsPriceType.SelectedText,
                }, resultAutoPartsPriceList);
                if (resultAutoPartsPriceList.Count > 0)
                {
                    foreach (var loopAddDetail in salesOrderDetailListToAdd)
                    {
                        var curBarcodeAndPrice =
                            resultAutoPartsPriceList.FirstOrDefault(x => x.Split(';')[0] == loopAddDetail.SOD_Barcode);
                        if (curBarcodeAndPrice != null)
                        {
                            var barcodeAndPriceArray = curBarcodeAndPrice.Split(';');
                            //根据配件档案中的配置价格获取销售单价
                            loopAddDetail.SOD_UnitPrice = Convert.ToDecimal(barcodeAndPriceArray[1]);
                            //单价不允许编辑
                            loopAddDetail.UnitPriceIsAllowEdit = false;
                        }
                        else
                        {
                            //根据客户上次的销价获取销售单价
                            loopAddDetail.SOD_UnitPrice = BLLCom.GetAutoPartUnitPrice(loopAddDetail.SOD_Barcode,
                                mcbClientName.SelectedValue, _clientOrgID);
                            //单价允许编辑
                            loopAddDetail.UnitPriceIsAllowEdit = true;
                        }

                        //计算销售金额
                        loopAddDetail.SOD_TotalAmount = Math.Round((loopAddDetail.SOD_UnitPrice ?? 0) *
                                                                   (loopAddDetail.SOD_Qty ?? 0), 2);
                        //设置打印次数为1
                        loopAddDetail.PrintCount = 1;
                        //明细的汽修商组织和单头一致
                        loopAddDetail.SOD_StockInOrgID = _clientOrgID;
                        loopAddDetail.SOD_StockInOrgCode = _clientOrgCode;
                        loopAddDetail.SOD_StockInOrgName = mcbClientName.SelectedText;
                        //明细的单据状态和审核状态都和单头一致
                        loopAddDetail.SOD_StatusCode = cbSO_StatusCode.Value?.ToString() ?? "";
                        loopAddDetail.SOD_StatusName = cbSO_StatusCode.Text.Trim();
                        loopAddDetail.SOD_ApprovalStatusCode = cbSO_ApprovalStatusCode.Value?.ToString() ?? "";
                        loopAddDetail.SOD_ApprovalStatusName = cbSO_ApprovalStatusCode.Text.Trim();
                        loopAddDetail.SOD_IsValid = true;
                        loopAddDetail.SOD_CreatedBy = LoginInfoDAX.UserName;
                        loopAddDetail.SOD_CreatedTime = BLLCom.GetCurStdDatetime();
                        loopAddDetail.SOD_UpdatedBy = LoginInfoDAX.UserName;
                        loopAddDetail.SOD_UpdatedTime = BLLCom.GetCurStdDatetime();

                        SalesOrderDetailUIModel salesOrderDetail = new SalesOrderDetailUIModel();
                        _bll.CopyModel(loopAddDetail, salesOrderDetail);
                        _detailGridDS.Insert(0, salesOrderDetail);
                        gdDetail.DataSource = _detailGridDS;
                        gdDetail.DataBind();

                        insertCount++;
                    }
                }
                else
                {
                    foreach (var loopAddDetail in salesOrderDetailListToAdd)
                    {
                        //单价允许编辑
                        loopAddDetail.UnitPriceIsAllowEdit = true;

                        //根据客户上次的销价获取销售单价
                        loopAddDetail.SOD_UnitPrice = BLLCom.GetAutoPartUnitPrice(loopAddDetail.SOD_Barcode,
                    mcbClientName.SelectedValue, _clientOrgID);
                        //计算销售金额
                        loopAddDetail.SOD_TotalAmount = Math.Round((loopAddDetail.SOD_UnitPrice ?? 0) *
                                                                          (loopAddDetail.SOD_Qty ?? 0), 2);
                        //设置打印次数为1
                        loopAddDetail.PrintCount = 1;
                        //明细的汽修商组织和单头一致
                        loopAddDetail.SOD_StockInOrgID = _clientOrgID;
                        loopAddDetail.SOD_StockInOrgCode = _clientOrgCode;
                        loopAddDetail.SOD_StockInOrgName = mcbClientName.SelectedText;
                        //明细的单据状态和审核状态都和单头一致
                        loopAddDetail.SOD_StatusCode = cbSO_StatusCode.Value?.ToString() ?? "";
                        loopAddDetail.SOD_StatusName = cbSO_StatusCode.Text.Trim();
                        loopAddDetail.SOD_ApprovalStatusCode = cbSO_ApprovalStatusCode.Value?.ToString() ?? "";
                        loopAddDetail.SOD_ApprovalStatusName = cbSO_ApprovalStatusCode.Text.Trim();
                        loopAddDetail.SOD_IsValid = true;
                        loopAddDetail.SOD_CreatedBy = LoginInfoDAX.UserName;
                        loopAddDetail.SOD_CreatedTime = BLLCom.GetCurStdDatetime();
                        loopAddDetail.SOD_UpdatedBy = LoginInfoDAX.UserName;
                        loopAddDetail.SOD_UpdatedTime = BLLCom.GetCurStdDatetime();

                        SalesOrderDetailUIModel salesOrderDetail = new SalesOrderDetailUIModel();
                        _bll.CopyModel(loopAddDetail, salesOrderDetail);
                        _detailGridDS.Insert(0, salesOrderDetail);
                        gdDetail.DataSource = _detailGridDS;
                        gdDetail.DataBind();

                        insertCount++;
                    }
                }
            }
            else
            {
                foreach (var loopAddDetail in salesOrderDetailListToAdd)
                {
                    //单价允许编辑
                    loopAddDetail.UnitPriceIsAllowEdit = true;

                    //根据客户上次的销价获取销售单价
                    loopAddDetail.SOD_UnitPrice = BLLCom.GetAutoPartUnitPrice(loopAddDetail.SOD_Barcode,
                mcbClientName.SelectedValue, _clientOrgID);
                    //计算销售金额
                    loopAddDetail.SOD_TotalAmount = Math.Round((loopAddDetail.SOD_UnitPrice ?? 0) *
                                                                      (loopAddDetail.SOD_Qty ?? 0), 2);
                    //设置打印次数为1
                    loopAddDetail.PrintCount = 1;
                    //明细的汽修商组织和单头一致
                    loopAddDetail.SOD_StockInOrgID = _clientOrgID;
                    loopAddDetail.SOD_StockInOrgCode = _clientOrgCode;
                    loopAddDetail.SOD_StockInOrgName = mcbClientName.SelectedText;
                    //明细的单据状态和审核状态都和单头一致
                    loopAddDetail.SOD_StatusCode = cbSO_StatusCode.Value?.ToString() ?? "";
                    loopAddDetail.SOD_StatusName = cbSO_StatusCode.Text.Trim();
                    loopAddDetail.SOD_ApprovalStatusCode = cbSO_ApprovalStatusCode.Value?.ToString() ?? "";
                    loopAddDetail.SOD_ApprovalStatusName = cbSO_ApprovalStatusCode.Text.Trim();
                    loopAddDetail.SOD_IsValid = true;
                    loopAddDetail.SOD_CreatedBy = LoginInfoDAX.UserName;
                    loopAddDetail.SOD_CreatedTime = BLLCom.GetCurStdDatetime();
                    loopAddDetail.SOD_UpdatedBy = LoginInfoDAX.UserName;
                    loopAddDetail.SOD_UpdatedTime = BLLCom.GetCurStdDatetime();

                    SalesOrderDetailUIModel salesOrderDetail = new SalesOrderDetailUIModel();
                    _bll.CopyModel(loopAddDetail, salesOrderDetail);
                    _detailGridDS.Insert(0, salesOrderDetail);
                    gdDetail.DataSource = _detailGridDS;
                    gdDetail.DataBind();

                    insertCount++;
                }
            }
            #endregion

            #region 设置销售单价是否可编辑

            for (int i = 0; i < gdDetail.Rows.Count; i++)
            {
                if (gdDetail.Rows[i].Cells["UnitPriceIsAllowEdit"].Value?.ToString() == SysConst.True)
                {
                    gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Activation = Activation.AllowEdit;
                }
                else
                {
                    gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Activation = Activation.ActivateOnly;
                }
            }

            #endregion

            //控制客户名称是否可编辑
            SetDetailByIsExistDetail();

            //不存在新增的明细时
            if (insertCount == 0)
            {
                return;
            }

            gdDetail.DataSource = _detailGridDS;
            gdDetail.DataBind();
            gdDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].Width = 60;
            //设置单元格是否可以编辑
            SetSalesOrderDetailStyle();

            //对新增的一个和多个进行单元格的特殊处理
            for (int i = insertCount; i > 0; i--)
            {
                if (gdDetail.Rows[gdDetail.Rows.Count - i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Value == null
                    || gdDetail.Rows[gdDetail.Rows.Count - i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Value.ToString() == SysConst.False)
                {
                    //[价格是否含税]为空或False的场合，[税率]不可编辑
                    gdDetail.Rows[gdDetail.Rows.Count - i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Value = SysConst.False;
                    gdDetail.Rows[gdDetail.Rows.Count - i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Activation = Activation.ActivateOnly;
                }
                else
                {
                    //[价格是否含税]为True的场合，[税率]可编辑
                    gdDetail.Rows[gdDetail.Rows.Count - i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Value = SysConst.True;
                    gdDetail.Rows[gdDetail.Rows.Count - i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Activation = Activation.AllowEdit;
                }

                //固定写法：[计价基准可改]默认为False，[计价基准]默认不可编辑
                gdDetail.Rows[gdDetail.Rows.Count - i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRateIsChangeable].Value = SysConst.False;
                gdDetail.Rows[gdDetail.Rows.Count - i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_SalePriceRate].Activation = Activation.ActivateOnly;
            }
        }

        /// <summary>
        /// 根据参数判断获取来源
        /// </summary>
        /// <returns></returns>
        private string GetSalesOrderDetailSrouceType()
        {
            if (_isHasInventory)
            {
                return InsertSalesOrderDetailType.InventoryDirectly;
            }
            else
            {
                return InsertSalesOrderDetailType.AutoPartsArchiveDirectly;
            }
        }

        #endregion

        /// <summary>
        /// 根据[是否存在明细]控制单头是否可编辑
        /// </summary>
        private void SetDetailByIsExistDetail()
        {
            if (_detailGridDS.Count == 0)
            {
                cbSO_SourceTypeCode.Enabled = true;
                txtSO_SourceNo.Enabled = true;
                mcbClientType.Enabled = true;
                mcbClientName.Enabled = true;
                mcbAutoFactoryName.Enabled = true;

                //客户已配置配件价格类别的场合，配件价格类别不可更改
                if (!string.IsNullOrEmpty(mcbAutoPartsPriceType.SelectedText)
                    && mcbAutoPartsPriceType.SelectedText == _autoPartsPriceTypeOfCustomer)
                {
                    mcbAutoPartsPriceType.Enabled = false;
                }
                else
                {
                    mcbAutoPartsPriceType.Enabled = true;
                }
            }
            else
            {
                cbSO_SourceTypeCode.Enabled = false;
                txtSO_SourceNo.Enabled = false;
                mcbClientType.Enabled = false;
                mcbClientName.Enabled = false;
                mcbAutoFactoryName.Enabled = false;
                mcbAutoPartsPriceType.Enabled = false;
            }
        }

        /// <summary>
        /// 设置动作按钮状态
        /// </summary>
        private void SetActionEnableByStatus()
        {
            if (cbSO_SourceTypeCode.Text.Trim() == SalesOrderSourceTypeEnum.Name.ZDXS
                || cbSO_SourceTypeCode.Text.Trim() == SalesOrderSourceTypeEnum.Name.XSYC)
            {
                //[来源类型]为[主动销售]、[销售预测]的场合，销售订单不能保存、删除、审核、反审核、核实、打印
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
                SetActionEnable(SystemActionEnum.Code.VERIFY, false);

                if (cbSO_ApprovalStatusCode.Text == ApprovalStatusEnum.Name.YSH)
                {
                    SetActionEnable(SystemActionEnum.Code.PRINT, true);
                }
                else
                {
                    SetActionEnable(SystemActionEnum.Code.PRINT, false);
                }
            }
            else
            {
                if (cbSO_ApprovalStatusCode.Text == ApprovalStatusEnum.Name.YSH)
                {
                    //[审核状态]为[已审核]的场合
                    //[保存]、[删除]、[审核]不可用，[打印]可用
                    SetActionEnable(SystemActionEnum.Code.SAVE, false);
                    SetActionEnable(SystemActionEnum.Code.DELETE, false);
                    SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                    SetActionEnable(SystemActionEnum.Code.PRINT, true);

                    if (cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.YGB
                        || cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.JYCG)
                    {
                        //[状态]为[已关闭]或[交易成功]的场合，[反审核]、[核实]不可用
                        SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
                        SetActionEnable(SystemActionEnum.Code.VERIFY, false);
                    }
                    else
                    {
                        //[状态]不是[已关闭]或[交易成功]的场合，[反审核]可用不可用
                        SetActionEnable(SystemActionEnum.Code.UNAPPROVE, true);

                        if (cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.DFH
                        || cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.YFH
                        || cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.YQS
                            || cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.BFQS)
                        {
                            //[单据状态]为{待发货}、{已发货}、{部分签收}、{已签收}的场合，[核实]可用
                            SetActionEnable(SystemActionEnum.Code.VERIFY, true);
                        }
                        else
                        {
                            //[单据状态]不是{待发货}、{已发货}、{部分签收}、{已签收}的场合，[核实]不可用
                            SetActionEnable(SystemActionEnum.Code.VERIFY, false);
                        }
                    }
                }
                else
                {
                    //新增或[审核状态]为[待审核]的场合，[保存]、[删除]、[审核]可用，[反审核]、[核实]、[打印]不可用
                    SetActionEnable(SystemActionEnum.Code.SAVE, true);
                    SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtSO_ID.Text));
                    SetActionEnable(SystemActionEnum.Code.APPROVE, !string.IsNullOrEmpty(txtSO_ID.Text));
                    SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
                    SetActionEnable(SystemActionEnum.Code.VERIFY, false);
                    SetActionEnable(SystemActionEnum.Code.PRINT, false);
                }
            }
        }

        /// <summary>
        /// 根据[来源类型]设置[来源单号]是否显示
        /// </summary>
        private void SetDetailVisible()
        {
            //根据[来源类型]设置[来源单号]以及明细列表中[汽修商组织]、[汽修商仓库]、[汽修商仓位]列是否显示
            if (cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.SGCJ
                || cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.ZXXS)
            {
                #region [来源类型]为[手工创建]和[在线销售]的场合，无[来源单号]，明细列表中不显示[汽修商组织]、[汽修商仓库]、[汽修商仓位]列

                //来源类型：新增时只考虑【在线销售，手工创建】
                List<ComComboBoxDataSourceTC> tempSalesOrderSourceTypeList =
                    _salesOrderSourceTypeList.Where(
                        x => x.Text == SalesOrderSourceTypeEnum.Name.ZXXS
                        || x.Text == SalesOrderSourceTypeEnum.Name.SGCJ)
                        .ToList();
                cbSO_SourceTypeCode.DisplayMember = SysConst.EN_TEXT;
                cbSO_SourceTypeCode.ValueMember = SysConst.EN_Code;
                cbSO_SourceTypeCode.DataSource = tempSalesOrderSourceTypeList;
                cbSO_SourceTypeCode.DataBind();

                //[来源单号]不可见
                lblSO_SourceNo.Visible = false;
                txtSO_SourceNo.Visible = false;

                #endregion
            }
            else
            {
                #region [来源类型]为[主动销售]、[销售预测]的场合，有[来源单号]

                cbSO_SourceTypeCode.DisplayMember = SysConst.EN_TEXT;
                cbSO_SourceTypeCode.ValueMember = SysConst.EN_Code;
                cbSO_SourceTypeCode.DataSource = _salesOrderSourceTypeList;
                cbSO_SourceTypeCode.DataBind();

                //[来源单号]可见
                lblSO_SourceNo.Visible = true;
                txtSO_SourceNo.Visible = true;

                #endregion
            }

        }

        /// <summary>
        /// 获取出库明细列表
        /// </summary>
        private void GetStockOutDetail()
        {
            if (_detailGridDS == null)
            {
                return;
            }
            _stockOutDetailDS = new List<SalesStockOutDetailUIModel>();

            if (cbSO_ApprovalStatusCode.Text == ApprovalStatusEnum.Name.YSH)
            {
                #region [销售订单].[审核状态]为[已审核]的场合，显示实际的[出库单明细]

                _bll.QueryForList(SQLID.SD_ProactiveSales_SQL13, new MDLSD_SalesOrder
                {
                    WHERE_SO_No = txtSO_No.Text.Trim(),
                }, _stockOutDetailDS);

                #endregion
            }
            else
            {
                #region [销售订单]未保存 或 [销售订单].[审核状态]为[待审核]的场合，生成[待出库明细]

                //传入的[销售订单明细]列表
                SalesOrderDetailDataSet.SalesOrderDetailDataTable salesOrderDetailDataTable = new SalesOrderDetailDataSet.SalesOrderDetailDataTable();

                foreach (var loopSalesOrderDetail in _detailGridDS)
                {
                    if (string.IsNullOrEmpty(loopSalesOrderDetail.SOD_Barcode)
                        || loopSalesOrderDetail.SOD_Qty == null)
                    {
                        continue;
                    }

                    SalesOrderDetailDataSet.SalesOrderDetailRow newSalesOrderDetailRow = salesOrderDetailDataTable.NewSalesOrderDetailRow();
                    newSalesOrderDetailRow.SO_Org_ID = txtSO_Org_ID.Text;
                    newSalesOrderDetailRow.SOD_Barcode = loopSalesOrderDetail.SOD_Barcode;
                    decimal tempQty = (decimal)loopSalesOrderDetail.SOD_Qty;
                    newSalesOrderDetailRow.SOD_Qty = tempQty;

                    salesOrderDetailDataTable.AddSalesOrderDetailRow(newSalesOrderDetailRow);
                }
                //创建SqlConnection数据库连接对象
                SqlConnection sqlCon = new SqlConnection
                {
                    ConnectionString = DBManager.GetConnectionString(DBCONFIG.Coeus)
                };
                //打开数据库连接
                sqlCon.Open();

                //创建并初始化SqlCommand对象
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlCon;

                //待出库明细列表
                SalesOrderDetailDataSet.StockOutDetailDataTable resultStockOutDetailList = new SalesOrderDetailDataSet.StockOutDetailDataTable();

                try
                {
                    cmd.CommandText = "P_PIS_GetStockOutDetailBySalesOrderDetail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SalesOrderDetailList", SqlDbType.Structured);
                    cmd.Parameters[0].Value = salesOrderDetailDataTable;

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                    sda.Fill(resultStockOutDetailList);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return;
                }
                finally
                {
                    sqlCon.Close();
                }

                foreach (var loopStockOutDetail in resultStockOutDetailList)
                {
                    SalesStockOutDetailUIModel newStockOutDetail = new SalesStockOutDetailUIModel
                    {
                        INV_ID = loopStockOutDetail.INV_ID,
                        INV_Name = loopStockOutDetail.INV_Name,
                        INV_Barcode = loopStockOutDetail.INV_Barcode,
                        INV_BatchNo = loopStockOutDetail.INV_BatchNo,
                        INV_OEMNo = loopStockOutDetail.INV_OEMNo,
                        INV_ThirdNo = loopStockOutDetail.INV_ThirdNo,
                        INV_Specification = loopStockOutDetail.INV_Specification,
                        INV_Qty = loopStockOutDetail.INV_Qty,
                        StockOutQty = loopStockOutDetail.StockOutQty,
                        INV_PurchaseUnitPrice = loopStockOutDetail.INV_PurchaseUnitPrice,
                        INV_WH_ID = loopStockOutDetail.INV_WH_ID,
                        INV_WHB_ID = loopStockOutDetail.INV_WHB_ID,
                        WH_Name = loopStockOutDetail.WH_Name,
                        WHB_Name = loopStockOutDetail.WHB_Name,
                        INV_VersionNo = loopStockOutDetail.INV_VersionNo
                    };
                    _stockOutDetailDS.Add(newStockOutDetail);
                }
                #endregion
            }
            gdStockOutDetail.DataSource = _stockOutDetailDS;
            gdStockOutDetail.DataBind();
            //设置出库明细Grid自适应列宽（根据单元格内容）
            gdStockOutDetail.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }

        /// <summary>
        /// 设置GroupBox高度
        /// </summary>
        private void SetGroupBoxHeight()
        {
            //解决bug4481
            //int expandedCount = 0;

            //if (gbBase.Expanded)
            //{
            //    expandedCount++;
            //}
            //if (gbDetail.Expanded)
            //{
            //    expandedCount++;
            //}
            //if (gbStockOutDetail.Expanded)
            //{
            //    expandedCount++;
            //}

            //int avgHeight = 0;
            //if (expandedCount > 0)
            //{
            //    avgHeight = tableLayoutPanelDetail.Height / expandedCount;
            //}

            //gbBase.Height = gbBase.Expanded ? avgHeight : 25;
            //gbDetail.Height = gbDetail.Expanded ? avgHeight : 25;
            //gbStockOutDetail.Height = gbStockOutDetail.Expanded ? avgHeight : 25;
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="paramIsDelete">是否是删除操作</param>
        private void RefreshList(bool paramIsDelete = false)
        {
            if (paramIsDelete)
            {
                if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
                {
                    var removeList = HeadGridDS.Where(x => x.IsChecked == true).ToList();
                    foreach (var loopRemove in removeList)
                    {
                        HeadGridDS.Remove(loopRemove);
                    }
                }
                else
                {
                    var curHead = HeadGridDS.FirstOrDefault(x => x.SO_ID == HeadDS.SO_ID);
                    if (curHead != null)
                    {
                        HeadGridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = HeadGridDS.FirstOrDefault(x => x.SO_ID == HeadDS.SO_ID);
                if (curHead != null)
                {
                    _bll.CopyModel(HeadDS, curHead);
                }
                else
                {
                    HeadGridDS.Insert(0, HeadDS);
                }
            }

            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }
        #endregion

    }
}
