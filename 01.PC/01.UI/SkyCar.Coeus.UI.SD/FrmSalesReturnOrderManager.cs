using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinEditors;
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
    /// 销售退货管理
    /// </summary>
    public partial class FrmSalesReturnOrderManager : BaseFormCardListDetail<SalesReturnOrderManagerUIModel, SalesReturnOrderManagerQCModel, MDLSD_SalesOrder>
    {
        #region 全局变量

        /// <summary>
        /// 销售退货管理BLL
        /// </summary>
        private SalesReturnOrderManagerBLL _bll = new SalesReturnOrderManagerBLL();

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
        /// 销售订单所有来源类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _allSalesOrderSourceTypeList = new List<ComComboBoxDataSourceTC>();
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
        /// 客户
        /// </summary>
        List<CustomerQueryUIModel> _customerList = new List<CustomerQueryUIModel>();
        /// <summary>
        /// 配件价格类别
        /// </summary>
        ObservableCollection<CodeTableValueTextModel> _autoPartsPriceTypeList = new ObservableCollection<CodeTableValueTextModel>();
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
        /// 界面属性值发生变化时不予检查的属性列表
        /// </summary>
        List<string> _skipPropertyList = new List<string>();

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmSalesReturnOrderManager构造方法
        /// </summary>
        public FrmSalesReturnOrderManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSalesReturnOrderManager_Load(object sender, EventArgs e)
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
            _skipPropertyList.Add("ARB_AccountReceivableAmount");
            _skipPropertyList.Add("ARB_ReceivedAmount");
            _skipPropertyList.Add("ARB_UnReceivedAmount");
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
                accountReceivableAmount = accountReceivableAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountReceivableBill.Code.ARB_AccountReceivableAmount].Value?.ToString());
                receivedAmount = receivedAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountReceivableBill.Code.ARB_ReceivedAmount].Value?.ToString());
            }
            
            ((TextBoxTool)(toolBarPaging.Tools["txtTotalAmount"])).Text = Convert.ToString(totalAmount);
            ((TextBoxTool)(toolBarPaging.Tools["txtAccountReceivableAmount"])).Text = Convert.ToString(accountReceivableAmount);
            ((TextBoxTool)(toolBarPaging.Tools["txtReceivedAmount"])).Text = Convert.ToString(receivedAmount);
        }

        /// <summary>
        /// 列表】Grid的AfterHeaderCheckStateChanged事件
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
                accountReceivableAmount = accountReceivableAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountReceivableBill.Code.ARB_AccountReceivableAmount].Value?.ToString());
                receivedAmount = receivedAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountReceivableBill.Code.ARB_ReceivedAmount].Value?.ToString());
            }

            ((TextBoxTool)(toolBarPaging.Tools["txtTotalAmount"])).Text = Convert.ToString(totalAmount);
            ((TextBoxTool)(toolBarPaging.Tools["txtAccountReceivableAmount"])).Text = Convert.ToString(accountReceivableAmount);
            ((TextBoxTool)(toolBarPaging.Tools["txtReceivedAmount"])).Text = Convert.ToString(receivedAmount);
        }

        #endregion

        #region Tab改变事件

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

                //[在线支付]、[转结算]按钮可用
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
        /// 客户类型ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_SO_CustomerTypeCode_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbWhere_SO_CustomerTypeCode.Text)
                || cbWhere_SO_CustomerTypeCode.Value == null)
            {
                return;
            }
            mcbWhere_AutoFactoryName.Clear();
            if (_autoFactoryList != null)
            {
                var autoFactoryList = _autoFactoryList.Where(x => x.AFC_IsPlatform == true).ToList();
                mcbWhere_AutoFactoryName.DisplayMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Name;
                mcbWhere_AutoFactoryName.ValueMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Code;
                mcbWhere_AutoFactoryName.DataSource = autoFactoryList;
            }
            if (cbWhere_SO_CustomerTypeCode.Text != CustomerTypeEnum.Name.PTNQXSH && cbWhere_SO_CustomerTypeCode.Value != null)
            {
                mcbWhere_AutoFactoryName.Enabled = false;

            }
            else
            {
                mcbWhere_AutoFactoryName.Enabled = true;
            }
            if (_customerList != null)
            {
                var tempCustomerList = _customerList.Where(x => x.CustomerType == cbWhere_SO_CustomerTypeCode.Text).ToList();
                mcbWhere_SO_CustomerName.DisplayMember = "CustomerName";
                mcbWhere_SO_CustomerName.ValueMember = "CustomerID";
                mcbWhere_SO_CustomerName.DataSource = tempCustomerList;
            }
        }
        /// <summary>
        /// 汽修商户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbWhere_AutoFactoryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_customerList != null)
            {
                var tempCustomerList = _customerList.Where(x => x.AutoFactoryCode == mcbWhere_AutoFactoryName.SelectedValue).ToList();
                mcbWhere_SO_CustomerName.DisplayMember = "CustomerName";
                mcbWhere_SO_CustomerName.ValueMember = "CustomerID";
                mcbWhere_SO_CustomerName.DataSource = tempCustomerList;
            }
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
        }

        /// <summary>
        /// 【详情】来源单号EditorButtonClick事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSO_SourceNo_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            if (cbSO_SourceTypeCode.Value == null)
            {
                //请先选择来源类型，再选择来源单号
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0034, new object[] { MsgParam.SOURCE_TYPE, SystemTableColumnEnums.SD_SalesOrder.Name.SO_SourceNo }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            //汽修商户
            mcbAutoFactoryName.Clear();
            //汽修商户组织ID
            txtAROrgID.Clear();
            //汽修商户组织Code
            txtAROrgCode.Clear();
            //信用额度
            txtCreditAmount.Clear();
            //欠款金额
            txtDebtAmount.Clear();
            //配件价格类别
            cbSO_AutoPartsPriceType.Clear();
            //钱包账号
            txtWal_No.Clear();
            //钱包可用余额
            txtWal_AvailableBalance.Clear();
            cbSO_CustomerTypeCode.Clear();
            txtSO_SourceNo.Clear();
            txtSO_CustomerID.Clear();
            txtSO_CustomerName.Clear();
            mcbSO_SalesByName.Clear();

            //根据销售订单.[来源类型]查询相应销售订单
            List<ComComboBoxDataSourceTC> paramSourceTypeList = new List<ComComboBoxDataSourceTC>();
            if (cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.ZDXSTH)
            {
                //主动销售退货
                paramSourceTypeList = _allSalesOrderSourceTypeList.Where(x => x.Text == SalesOrderSourceTypeEnum.Name.ZDXS || x.Text == SalesOrderSourceTypeEnum.Name.XSYC).ToList();
            }
            else if (cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.SGCJTH)
            {
                //手工创建退货
                paramSourceTypeList = _allSalesOrderSourceTypeList.Where(x => x.Text == SalesOrderSourceTypeEnum.Name.SGCJ).ToList();
            }
            else if (cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.ZXXSTH)
            {
                //在线销售退货
                paramSourceTypeList = _allSalesOrderSourceTypeList.Where(x => x.Text == SalesOrderSourceTypeEnum.Name.ZXXS).ToList();
            }
            //仅查询[审核状态]为[已审核]的销售订单
            var paramApprovetatusList = _approveStatusList.Where(x => x.Text == ApprovalStatusEnum.Name.YSH).ToList();

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                {ComViewParamKey.ApprovalStatus.ToString(), paramApprovetatusList},
                {ComViewParamKey.SourceType.ToString(), paramSourceTypeList}
            };

            FrmSalesOrderQuery frmSalesOrderQuery = new FrmSalesOrderQuery(paramViewParameters)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = frmSalesOrderQuery.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            if (frmSalesOrderQuery.SelectedGridList == null || frmSalesOrderQuery.SelectedGridList.Count != 1)
            {
                return;
            }
            //选择的[销售订单]
            MDLSD_SalesOrder resultSalesOrder = new MDLSD_SalesOrder();
            resultSalesOrder = frmSalesOrderQuery.SelectedGridList[0];

            //获取客户信息
            if (resultSalesOrder.SO_CustomerID != txtSO_CustomerID.Text)
            {
                List<CustomerQueryUIModel> resultCustomerList = new List<CustomerQueryUIModel>();
                resultCustomerList = BLLCom.GetCustomerInfo(resultSalesOrder.SO_CustomerTypeName, resultSalesOrder.SO_CustomerID, resultSalesOrder.SO_CustomerName);
                if (resultCustomerList.Count == 1)
                {
                    //汽修商户
                    mcbAutoFactoryName.SelectedValue = resultCustomerList[0].AutoFactoryCode;
                    //汽修商户组织ID
                    txtAROrgID.Text = resultCustomerList[0].AutoFactoryOrgID;
                    //汽修商户组织Code
                    txtAROrgCode.Text = resultCustomerList[0].AutoFactoryOrgCode;
                    //信用额度
                    txtCreditAmount.Text = resultCustomerList[0].CreditAmount?.ToString();
                    //欠款金额
                    txtDebtAmount.Text = resultCustomerList[0].DebtAmount?.ToString();
                    //配件价格类别
                    cbSO_AutoPartsPriceType.Text = resultCustomerList[0].AutoPartsPriceType;
                    cbSO_AutoPartsPriceType.Value = resultCustomerList[0].AutoPartsPriceType;
                    //钱包账号
                    txtWal_No.Text = resultCustomerList[0].Wal_No;
                    //钱包可用余额
                    txtWal_AvailableBalance.Text = resultCustomerList[0].Wal_AvailableBalance?.ToString();
                }
            }

            cbSO_CustomerTypeCode.Text = resultSalesOrder.SO_CustomerTypeName;
            cbSO_CustomerTypeCode.Value = resultSalesOrder.SO_CustomerTypeCode;
            txtSO_SourceNo.Text = resultSalesOrder.SO_No;
            txtSO_CustomerID.Text = resultSalesOrder.SO_CustomerID;
            txtSO_CustomerName.Text = resultSalesOrder.SO_CustomerName;
            mcbSO_SalesByName.SelectedValue = resultSalesOrder.SO_SalesByID;

            //客户已配置配件价格类别的场合，配件价格类别不可更改
            if (!string.IsNullOrEmpty(cbSO_AutoPartsPriceType.Text))
            {
                cbSO_AutoPartsPriceType.Enabled = false;
            }
            else
            {
                cbSO_AutoPartsPriceType.Enabled = true;
            }

            #region 根据来源单号加载明细列表

            List<MDLSD_SalesOrderDetail> resultSalesOrderDetailList = new List<MDLSD_SalesOrderDetail>();
            _bll.QueryForList<MDLSD_SalesOrderDetail, MDLSD_SalesOrderDetail>(new MDLSD_SalesOrderDetail
            {
                WHERE_SOD_SO_ID = resultSalesOrder.SO_ID,
                WHERE_SOD_IsValid = true
            }, resultSalesOrderDetailList);

            //新增销售订单明细
            InsertSalesOrderDetail(InsertSalesOrderDetailType.SalesOrder, resultSalesOrderDetailList);

            #endregion
        }

        /// <summary>
        /// 【详情】客户类型ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSO_CustomerTypeCode_ValueChanged(object sender, EventArgs e)
        {
            if (cbSO_CustomerTypeCode.Value == null)
            {
                return;
            }
            txtSO_CustomerID.Clear();
            txtSO_CustomerName.Clear();
            mcbAutoFactoryName.Clear();

            if (cbSO_CustomerTypeCode.Text == CustomerTypeEnum.Name.PTNQXSH)
            {
                //来源类型为{平台内汽修商户}的场合，显示[汽修商户]
                lblAutoFactoryName.Visible = true;
                mcbAutoFactoryName.Visible = true;
            }
            else
            {
                //来源类型不是{平台内汽修商户}的场合，不显示[汽修商户]
                lblAutoFactoryName.Visible = false;
                mcbAutoFactoryName.Visible = false;
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
                //修改数量
                if (e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty)
                {
                    //退货的场合，退货数量不能超过原签收数量
                    if (curActiveRow.Cells["OriginalSignQty"].Value != null)
                    {
                        if ((decimal)e.Cell.Value > (decimal)curActiveRow.Cells["OriginalSignQty"].Value)
                        {
                            //最大退货数量为原签收数量
                            e.Cell.Value = (decimal)curActiveRow.Cells["OriginalSignQty"].Value;
                        }
                    }
                }
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

            //#region Cell为[单价/数量/税率]

            ////如果勾选了含税，则需要只要输入了税率 则需要计算税额  计算公式：税额 = 单价 * 税率 * 数量
            //if (e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice
            //    || e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate
            //    || e.Cell.Column.Key == SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty)
            //{
            //    if (curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_PriceIsIncludeTax].Text == SysConst.True)
            //    {
            //        //计算税额
            //        if (BLLCom.IsDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Value?.ToString())
            //            && BLLCom.IsDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Value?.ToString())
            //            && BLLCom.IsDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Value?.ToString()))
            //        {
            //            curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TotalTax].Value = Math.Round(
            //                Convert.ToDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Value?.ToString()) *
            //                Convert.ToDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_TaxRate].Value?.ToString()) *
            //                Convert.ToDecimal(curActiveRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].Value?.ToString()), 2);
            //        }
            //    }
            //}
            //#endregion

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

            //设置详情页面控件是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();
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
            base.ConditionDS = new SalesReturnOrderManagerQCModel()
            {
                //SqlId
                SqlId = SQLID.SD_SalesReturnOrderManager_SQL01,
                //单据编号
                WHERE_SO_No = txtWhere_SO_No.Text.Trim(),
                //组织ID
                WHERE_SO_Org_ID = LoginInfoDAX.OrgID,
                //来源类型编码
                WHERE_SO_SourceTypeName = cbWhere_SO_SourceTypeCode.Text,
                //客户类型编码
                WHERE_SO_CustomerTypeName = cbWhere_SO_CustomerTypeCode.Text,
                //汽修商户名称
                WHERE_AutoFactoryName = mcbWhere_AutoFactoryName.SelectedText,
                //客户名称
                WHERE_SO_CustomerName = mcbWhere_SO_CustomerName.SelectedText,
                //单据状态编码
                WHERE_SO_StatusName = cbWhere_SO_StatusCode.Text,
                //审核状态编码
                WHERE_SO_ApprovalStatusName = cbWhere_SO_ApprovalStatusCode.Text,
            };
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
            List<SalesReturnOrderManagerUIModel> resultAllList = new List<SalesReturnOrderManagerUIModel>();
            _bll.QueryForList(SQLID.SD_SalesOrder_SQL04, new SalesReturnOrderManagerQCModel()
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
                WHERE_SO_CustomerTypeName = cbWhere_SO_CustomerTypeCode.Text,
                //汽修商户名称
                WHERE_AutoFactoryName = mcbWhere_AutoFactoryName.SelectedText,
                //客户名称
                WHERE_SO_CustomerName = mcbWhere_SO_CustomerName.SelectedText,
                //单据状态编码
                WHERE_SO_StatusName = cbWhere_SO_StatusCode.Text,
                //审核状态编码
                WHERE_SO_ApprovalStatusName = cbWhere_SO_ApprovalStatusCode.Text,
            }, resultAllList);
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

            if (_isHasInventory)
            {
                //生成出库明细
                GetStockOutDetail();
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

            #region 获取退货入库信息

            //待退货入库明细列表
            List<ReturnStockInDetailUIModel> returnStockInDetailList = new List<ReturnStockInDetailUIModel>();
            foreach (var loopReturnDetail in _detailGridDS)
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
                        SOD_RejectQty = loopReturnDetail.SOD_Qty,
                        //默认入库总数量为退货总数量
                        SID_Qty = loopReturnDetail.SOD_Qty,
                        SID_Amount = Math.Round((loopReturnDetail.SOD_Qty ?? 0) * (loopReturnStockOutDetail.INV_PurchaseUnitPrice ?? 0), 2),

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
            List<SalesReturnDetailUIModel> returnSalesOrderDetailList = new List<SalesReturnDetailUIModel>();
            _bll.CopyModelList(_detailGridDS, returnSalesOrderDetailList);
            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //确认退货画面的来源方式：审核退货的销售订单
                {SDViewParamKey.ConfirmReturnSourType.ToString(),SDViewParamValue.ConfirmReturnSourType.ApproveReturnSalesOrder},
                //销售退货明细
                {SDViewParamKey.SalesOrderReturnDetail.ToString(), returnSalesOrderDetailList},
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
            #endregion

            bool saveApprove = _bll.ApproveDetailDS(HeadDS, _detailGridDS, _stockOutDetailDS, returnStockInDetailList, _isHasInventory);
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

                FrmViewAndPrintSalesReturn frmViewAndPrintSalesOrder = new FrmViewAndPrintSalesReturn(argsViewParams)
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
            //选中的[销售单]列表
            List<SalesReturnOrderManagerUIModel> selectedSalesReturnOrderList = new List<SalesReturnOrderManagerUIModel>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 当前选中Tab为【详情】
                if (HeadDS == null || string.IsNullOrEmpty(HeadDS.SO_No) || HeadDS.SO_StatusName != SalesOrderStatusEnum.Name.JYCG || HeadDS.ARB_UnReceivedAmount >= 0)
                {
                    //请选择一个交易成功并且未付清的销售订单信息进行转结算
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + SalesOrderStatusEnum.Name.JYCG + MsgParam.AND + MsgParam.NOTYET + MsgParam.PAY_FULL + MsgParam.OF + SystemTableEnums.Name.SD_SalesOrder + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                selectedSalesReturnOrderList.Add(HeadDS);

                #endregion
            }
            else
            {
                #region 当前选中Tab为【列表】

                var checkedGrid = HeadGridDS.Where(x => x.IsChecked == true).ToList();

                if (checkedGrid.Count == 0)
                {
                    //请选择一个交易成功并且未付清的销售订单信息进行转结算
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + SalesOrderStatusEnum.Name.JYCG + MsgParam.AND + MsgParam.NOTYET + MsgParam.PAY_FULL + MsgParam.OF + SystemTableEnums.Name.SD_SalesOrder + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var JYCGAndPayFullList =
                    checkedGrid.Where(x => x.SO_StatusName != SalesOrderStatusEnum.Name.JYCG ||
                            x.ARB_UnReceivedAmount >= 0).ToList();
                if (JYCGAndPayFullList.Count > 0)
                {
                    //请选择交易成功并且未付清的销售订单信息进行转结算
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { SalesOrderStatusEnum.Name.JYCG + MsgParam.AND + MsgParam.NOTYET + MsgParam.PAY_FULL + MsgParam.OF + SystemTableEnums.Name.SD_SalesOrder + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var loopCheckedItem in JYCGAndPayFullList)
                    {
                        loopCheckedItem.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }

                var firstCheckedItem = checkedGrid.FirstOrDefault(x => x.SO_StatusName == SalesOrderStatusEnum.Name.JYCG && x.ARB_UnReceivedAmount < 0);
                if (firstCheckedItem == null)
                {
                    //请至少勾选一条交易成功并且未付清的销售订单信息进行转结算
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { SalesOrderStatusEnum.Name.JYCG + MsgParam.AND + MsgParam.NOTYET + MsgParam.PAY_FULL + MsgParam.OF + SystemTableEnums.Name.SD_SalesOrder, SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                _bll.CopyModelList(checkedGrid, selectedSalesReturnOrderList);

                #endregion
            }

            #region 访问数据库，获取应收单数据

            //传入的待收款的[销售订单]列表
            SalesOrderDataSet.SalesOrderDataTable salesOrderDataTable = new SalesOrderDataSet.SalesOrderDataTable();

            foreach (var loopSalesOrderDetail in selectedSalesReturnOrderList)
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

            #region 转付款

            //待确认付款的销售订单信息列表
            List<BusinessPayConfirmWindowModel> salesOrderToPayList = new List<BusinessPayConfirmWindowModel>();
            foreach (var loopSalesOrderToPay in resultSalesOrderToSettlementList)
            {
                BusinessPayConfirmWindowModel salesOrderToPay = new BusinessPayConfirmWindowModel
                {
                    IsBusinessSourceAccountPayableBill = false,
                    BusinessID = loopSalesOrderToPay.BusinessID,
                    BusinessNo = loopSalesOrderToPay.BusinessNo,
                    BusinessOrgID = loopSalesOrderToPay.BusinessOrgID,
                    BusinessOrgName = loopSalesOrderToPay.BusinessOrgName,
                    BusinessSourceTypeName = loopSalesOrderToPay.BusinessSourceTypeName,
                    BusinessSourceTypeCode = loopSalesOrderToPay.BusinessSourceTypeCode,
                    BusinessSourceNo = loopSalesOrderToPay.BusinessSourceNo,
                    ReceiveObjectTypeName = loopSalesOrderToPay.PayObjectTypeName,
                    ReceiveObjectTypeCode = loopSalesOrderToPay.PayObjectTypeCode,
                    ReceiveObjectID = loopSalesOrderToPay.PayObjectID,
                    ReceiveObjectName = loopSalesOrderToPay.PayObjectName,
                    Wal_ID = loopSalesOrderToPay.Wal_ID,
                    Wal_No = loopSalesOrderToPay.Wal_No,
                    Wal_AvailableBalance = loopSalesOrderToPay.Wal_AvailableBalance,
                    Wal_VersionNo = loopSalesOrderToPay.Wal_VersionNo,
                    //应付单相关
                    APB_ID = loopSalesOrderToPay.ARB_ID,
                    APB_No = loopSalesOrderToPay.ARB_No,
                    APB_BillDirectCode = loopSalesOrderToPay.ARB_BillDirectCode,
                    APB_BillDirectName = loopSalesOrderToPay.ARB_BillDirectName,
                    APB_SourceTypeCode = loopSalesOrderToPay.ARB_SourceTypeCode,
                    APB_SourceTypeName = loopSalesOrderToPay.ARB_SourceTypeName,
                    APB_SourceBillNo = loopSalesOrderToPay.ARB_SrcBillNo,
                    APB_Org_ID = loopSalesOrderToPay.ARB_Org_ID,
                    APB_Org_Name = loopSalesOrderToPay.ARB_Org_Name,
                    APB_AccountPayableAmount = loopSalesOrderToPay.ARB_AccountReceivableAmount,
                    APB_PaidAmount = loopSalesOrderToPay.ARB_ReceivedAmount,
                    APB_UnpaidAmount = loopSalesOrderToPay.ARB_UnReceiveAmount,
                    APB_BusinessStatusCode = loopSalesOrderToPay.ARB_BusinessStatusCode,
                    APB_BusinessStatusName = loopSalesOrderToPay.ARB_BusinessStatusName,
                    APB_ApprovalStatusCode = loopSalesOrderToPay.ARB_ApprovalStatusCode,
                    APB_ApprovalStatusName = loopSalesOrderToPay.ARB_ApprovalStatusName,
                    APB_CreatedBy = loopSalesOrderToPay.ARB_CreatedBy,
                    APB_CreatedTime = loopSalesOrderToPay.ARB_CreatedTime,
                    APB_VersionNo = loopSalesOrderToPay.ARB_VersionNo,
                };
                //if (loopSalesOrderToPay.ARB_BillDirectName == BillDirectionEnum.Name.MINUS)
                //{
                //    salesOrderToPay.PayableTotalAmount = loopSalesOrderToPay.ARB_AccountReceivableAmount;
                //    salesOrderToPay.PayTotalAmount = loopSalesOrderToPay.ARB_ReceivedAmount;
                //    salesOrderToPay.UnPayTotalAmount = loopSalesOrderToPay.ARB_UnReceiveAmount;
                //}
                //else
                //{
                salesOrderToPay.PayableTotalAmount = -loopSalesOrderToPay.ARB_AccountReceivableAmount;
                salesOrderToPay.PayTotalAmount = -loopSalesOrderToPay.ARB_ReceivedAmount;
                salesOrderToPay.UnPayTotalAmount = -loopSalesOrderToPay.ARB_UnReceiveAmount;
                //}
                salesOrderToPayList.Add(salesOrderToPay);
            }

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //业务单确认付款
                {ComViewParamKey.BusinessPaymentConfirm.ToString(), salesOrderToPayList}
            };

            //跳转[业务单确认付款弹出窗]
            FrmBusinessPayConfirmWindow frmBusinessPayConfirmWindow = new FrmBusinessPayConfirmWindow(paramViewParameters)
            {
                StartPosition = FormStartPosition.CenterScreen
            };

            DialogResult dialogResult = frmBusinessPayConfirmWindow.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                QueryAction();
            }

            #endregion
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
            txtSO_CustomerID.Clear();
            //客户名称
            txtSO_CustomerName.Clear();
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
            cbSO_AutoPartsPriceType.Value = null;
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
            txtAROrgID.Clear();
            //汽修商组织Code
            txtAROrgCode.Clear();
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
            #endregion

            #region 初始化下拉框

            //客户类型
            _customerTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.CustomerType);
            cbSO_CustomerTypeCode.DisplayMember = SysConst.EN_TEXT;
            cbSO_CustomerTypeCode.ValueMember = SysConst.EN_Code;
            cbSO_CustomerTypeCode.DataSource = _customerTypeList;
            cbSO_CustomerTypeCode.DataBind();

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
            _allSalesOrderSourceTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.SalesOrderSourceType);
            //新增时只考虑【手工创建退货，在线销售退货，主动销售退货】
            if (_allSalesOrderSourceTypeList != null)
            {
                _salesOrderSourceTypeList = _allSalesOrderSourceTypeList.Where(
                        x => x.Text == SalesOrderSourceTypeEnum.Name.SGCJTH
                        || x.Text == SalesOrderSourceTypeEnum.Name.ZXXSTH
                        || x.Text == SalesOrderSourceTypeEnum.Name.ZDXSTH)
                        .ToList();
                cbSO_SourceTypeCode.DisplayMember = SysConst.EN_TEXT;
                cbSO_SourceTypeCode.ValueMember = SysConst.EN_Code;
                cbSO_SourceTypeCode.DataSource = _salesOrderSourceTypeList;
                cbSO_SourceTypeCode.DataBind();
            }

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
            cbSO_AutoPartsPriceType.DisplayMember = SysConst.EN_TEXT;
            cbSO_AutoPartsPriceType.ValueMember = SysConst.Value;
            cbSO_AutoPartsPriceType.DataSource = _autoPartsPriceTypeList;
            cbSO_AutoPartsPriceType.DataBind();
            
            //业务员
            _salesByList = CacheDAX.Get(CacheDAX.ConfigDataKey.SystemUser) as List<MDLSM_User>;
            mcbSO_SalesByName.DisplayMember = SystemTableColumnEnums.SM_User.Code.User_Name;
            mcbSO_SalesByName.ValueMember = SystemTableColumnEnums.SM_User.Code.User_ID;
            mcbSO_SalesByName.DataSource = _salesByList;
            #endregion

            #region 相关默认设置
            //默认[来源类型]为[手工创建]
            cbSO_SourceTypeCode.Value = SalesOrderSourceTypeEnum.Code.ZDXSTH;
            cbSO_SourceTypeCode.Text = SalesOrderSourceTypeEnum.Name.ZDXSTH;

            //默认[客户类型]为[平台内汽修商]
            cbSO_CustomerTypeCode.Value = CustomerTypeEnum.Code.PTNQXSH;
            cbSO_CustomerTypeCode.Text = CustomerTypeEnum.Name.PTNQXSH;

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
            cbWhere_SO_CustomerTypeCode.Value = null;
            //汽修商户名称
            mcbWhere_AutoFactoryName.Clear();
            //客户名称
            mcbWhere_SO_CustomerName.Clear();
            //来源类型
            cbWhere_SO_SourceTypeCode.Value = null;
            //单据状态
            cbWhere_SO_StatusCode.Value = null;
            //审核状态
            cbWhere_SO_ApprovalStatusCode.Value = null;
            //给 单据编号 设置焦点
            lblWhere_SO_No.Focus();
            #endregion

            #region Grid初始化

            //清空Grid
            HeadGridDS = new BindingList<SalesReturnOrderManagerUIModel>();
            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();

            #endregion

            #endregion

            #region 初始化下拉框

            //客户类型
            cbWhere_SO_CustomerTypeCode.DisplayMember = SysConst.EN_TEXT;
            cbWhere_SO_CustomerTypeCode.ValueMember = SysConst.EN_Code;
            cbWhere_SO_CustomerTypeCode.DataSource = _customerTypeList;
            cbWhere_SO_CustomerTypeCode.DataBind();

            //汽修商户名称
            if (_autoFactoryList != null)
            {
                var autoFactoryList = _autoFactoryList.Where(x => x.AFC_IsPlatform == true).ToList();
                mcbWhere_AutoFactoryName.DisplayMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Name;
                mcbWhere_AutoFactoryName.ValueMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Code;
                mcbWhere_AutoFactoryName.DataSource = autoFactoryList;
            }

            //客户名称
            _customerList = CacheDAX.Get(CacheDAX.ConfigDataKey.Customer) as List<CustomerQueryUIModel>;
            mcbWhere_SO_CustomerName.DisplayMember = "CustomerName";
            mcbWhere_SO_CustomerName.ValueMember = "CustomerID";
            mcbWhere_SO_CustomerName.DataSource = _customerList;

            //来源类型
            cbWhere_SO_SourceTypeCode.DisplayMember = SysConst.EN_TEXT;
            cbWhere_SO_SourceTypeCode.ValueMember = SysConst.EN_Code;
            cbWhere_SO_SourceTypeCode.DataSource = _salesOrderSourceTypeList;
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
            cbSO_CustomerTypeCode.Value = HeadDS.SO_CustomerTypeCode;
            //客户类型名称
            cbSO_CustomerTypeCode.Text = HeadDS.SO_CustomerTypeName;
            //客户名称
            txtSO_CustomerName.Text = HeadDS.SO_CustomerName;
            //客户ID
            txtSO_CustomerID.Text = HeadDS.SO_CustomerID;
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
            cbSO_AutoPartsPriceType.Text = HeadDS.SO_AutoPartsPriceType;
            cbSO_AutoPartsPriceType.Value = HeadDS.SO_AutoPartsPriceType;
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
            //汽修商户
            mcbAutoFactoryName.SelectedValue = HeadDS.AutoFactoryCode;
            //汽修商户组织ID
            txtAROrgID.Text = HeadDS.AROrgID;
            //汽修商户组织编码
            txtAROrgCode.Text = HeadDS.AROrgCode;
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
            _bll.QueryForList(SQLID.SD_SalesOrder_SQL01, new SalesReturnOrderManagerQCModel
            {
                WHERE_SOD_SO_ID = txtSO_ID.Text.Trim()
            }, _detailGridDS);

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

            //来源单号不能为空
            if (string.IsNullOrEmpty(txtSO_SourceNo.Text))
            {
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, MsgParam.SOURCE_NO), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, MsgParam.RETURN_QTY, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, MsgParam.RETURN_PRICE, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (!BLLCom.IsDecimal(gdDetail.Rows[i].Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].Value.ToString()))
                {
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0024, new object[] { i + 1, SystemTableColumnEnums.SD_SalesOrderDetail.Name.SOD_UnitPrice, MsgParam.DECIMAL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
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
            HeadDS = new SalesReturnOrderManagerUIModel()
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
                SO_CustomerTypeCode = cbSO_CustomerTypeCode.Value?.ToString() ?? "",
                //客户类型名称
                SO_CustomerTypeName = cbSO_CustomerTypeCode.Text,
                //客户名称
                SO_CustomerName = txtSO_CustomerName.Text.Trim(),
                //客户ID
                SO_CustomerID = txtSO_CustomerID.Text.Trim(),
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
                SO_AutoPartsPriceType = cbSO_AutoPartsPriceType.Text,
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
                AROrgID = txtAROrgID.Text,
                //汽修商户组织编码
                AROrgCode = txtAROrgCode.Text,
                //汽修商户组织名称
                AROrgName = txtSO_CustomerName.Text.Trim(),
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
            if (cbSO_ApprovalStatusCode.Text == ApprovalStatusEnum.Name.YSH)
            {
                #region [销售订单].[审核状态]为{已审核}的场合

                //单头不可编辑
                cbSO_SourceTypeCode.Enabled = false;
                txtSO_SourceNo.Enabled = false;
                txtSO_Remark.Enabled = false;

                //明细列表不可删除、更新
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = false;
                //明细列表[选择]、[数量]、[单价]、[汽修商组织]、[汽修商仓库]、[汽修商仓位]、[备注]列不可编辑
                gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].CellActivation = Activation.ActivateOnly;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].CellActivation = Activation.ActivateOnly;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].CellActivation = Activation.ActivateOnly;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].CellActivation = Activation.ActivateOnly;

                if (cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.ZDXSTH)
                {
                    //[销售明细]列表中显示[汽修商组织]、[汽修商仓库]、[汽修商仓位]、[批次号（汽修）]列
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInOrgName].Hidden = false;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInWarehouseName].Hidden = false;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInBinName].Hidden = false;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_BatchNoNew].Hidden = false;
                }
                else
                {
                    //[销售明细]列表中不显示[汽修商组织]、[汽修商仓库]、[汽修商仓位]、[批次号（汽修）]列
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInOrgName].Hidden = true;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInWarehouseName].Hidden = true;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInBinName].Hidden = true;
                    gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_BatchNoNew].Hidden = true;
                }

                #endregion
            }
            else
            {
                #region 销售订单未保存 或 [销售订单].[审核状态]为{待审核}的场合

                //根据[是否存在明细]控制单头是否可编辑
                SetDetailByIsExistDetail();
                txtSO_Remark.Enabled = true;

                //明细列表可删除、更新
                toolbarsManagerDetail.Tools[SysConst.EN_DEL].SharedProps.Enabled = true;
                //明细列表[选择]、[数量]、[单价]、[备注]列可编辑
                gdDetail.DisplayLayout.Bands[0].Columns[SysConst.IsChecked].CellActivation = Activation.AllowEdit;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Qty].CellActivation = Activation.AllowEdit;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_UnitPrice].CellActivation = Activation.AllowEdit;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].CellActivation = Activation.AllowEdit;

                //[销售明细]列表中不显示[汽修商组织]、[汽修商仓库]、[汽修商仓位]、[批次号（汽修）]列
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInOrgName].Hidden = true;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInWarehouseName].Hidden = true;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_StockInBinName].Hidden = true;
                gdDetail.DisplayLayout.Bands[0].Columns[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_BatchNoNew].Hidden = true;

                #endregion
            }
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
                case InsertSalesOrderDetailType.SalesOrder:

                    #region 销售订单方式添加

                    List<MDLSD_SalesOrderDetail> selectedSalesOrderList = paramInsertSalesOrderDetialList as List<MDLSD_SalesOrderDetail>;
                    if (selectedSalesOrderList != null)
                    {
                        foreach (var loopSalesOrder in selectedSalesOrderList)
                        {
                            //条码相同的配件合并
                            SalesOrderDetailUIModel sameSalesOrderDetail = salesOrderDetailListToAdd.FirstOrDefault(x => x.SOD_Barcode == loopSalesOrder.SOD_Barcode) ?? _detailGridDS.FirstOrDefault(x => x.SOD_Barcode == loopSalesOrder.SOD_Barcode);
                            if (sameSalesOrderDetail == null)
                            {
                                //添加[销售明细]
                                SalesOrderDetailUIModel tempSalesOrderDetail = new SalesOrderDetailUIModel
                                {
                                    SOD_Barcode = loopSalesOrder.SOD_Barcode,
                                    SOD_Name = loopSalesOrder.SOD_Name,
                                    SOD_Specification = loopSalesOrder.SOD_Specification,
                                    SOD_UOM = loopSalesOrder.SOD_UOM,
                                    //默认销售数量（退货数量）为[销售订单明细].[签收数量]
                                    SOD_Qty = loopSalesOrder.SOD_SignQty,
                                    //原签收数量
                                    OriginalSignQty = loopSalesOrder.SOD_SignQty,
                                    //单价
                                    SOD_UnitPrice = loopSalesOrder.SOD_UnitPrice,
                                    SOD_SalePriceRateIsChangeable = loopSalesOrder.SOD_SalePriceRateIsChangeable,
                                    SOD_SalePriceRate = loopSalesOrder.SOD_SalePriceRate,
                                    SOD_PriceIsIncludeTax = loopSalesOrder.SOD_PriceIsIncludeTax,
                                    SOD_TaxRate = loopSalesOrder.SOD_TaxRate,
                                    SOD_StockInWarehouseID = loopSalesOrder.SOD_StockInWarehouseID,
                                    SOD_StockInWarehouseName = loopSalesOrder.SOD_StockInWarehouseName,
                                    SOD_StockInBinID = loopSalesOrder.SOD_StockInBinID,
                                    SOD_StockInBinName = loopSalesOrder.SOD_StockInBinName,
                                };
                                tempSalesOrderDetail.SOD_TotalAmount = Math.Round((tempSalesOrderDetail.SOD_Qty ?? 0) *
                                                                       (tempSalesOrderDetail.SOD_UnitPrice ?? 0), 2);
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

            foreach (var loopAddDetail in salesOrderDetailListToAdd)
            {
                //明细的汽修商组织和单头一致
                if (cbSO_SourceTypeCode.Text == SalesOrderSourceTypeEnum.Name.ZDXSTH)
                {
                    loopAddDetail.SOD_StockInOrgID = txtAROrgID.Text;
                    loopAddDetail.SOD_StockInOrgCode = txtAROrgCode.Text.Trim();
                    loopAddDetail.SOD_StockInOrgName = txtSO_CustomerName.Text.Trim();
                }
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
            }
        }

        /// <summary>
        /// 根据[是否存在明细]控制单头是否可编辑
        /// </summary>
        private void SetDetailByIsExistDetail()
        {
            if (_detailGridDS.Count == 0)
            {
                cbSO_SourceTypeCode.Enabled = true;
                txtSO_SourceNo.Enabled = true;
            }
            else
            {
                cbSO_SourceTypeCode.Enabled = false;
                txtSO_SourceNo.Enabled = false;
            }
        }

        /// <summary>
        /// 设置动作按钮状态
        /// </summary>
        private void SetActionEnableByStatus()
        {
            if (cbSO_ApprovalStatusCode.Text == ApprovalStatusEnum.Name.YSH)
            {
                //[审核状态]为[已审核]的场合，[保存]、[删除]、[审核]不可用，
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                SetActionEnable(SystemActionEnum.Code.PRINT, true);

                if (cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.YGB
                    || cbSO_StatusCode.Text == SalesOrderStatusEnum.Name.JYCG)
                {
                    //[在线支付]不可用
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.ONLINEPAY, true, false);
                    //[转结算]可用
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, true);
                }
                else
                {
                    //[在线支付]可用
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.ONLINEPAY, true, true);
                    //[转结算]不可用
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, false);
                }
            }
            else
            {
                //新增或[审核状态]为[待审核]的场合，[保存]、[删除]、[审核]可用，[打印]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, true);
                SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtSO_ID.Text));
                SetActionEnable(SystemActionEnum.Code.APPROVE, !string.IsNullOrEmpty(txtSO_ID.Text));
                SetActionEnable(SystemActionEnum.Code.PRINT, false);

                //[在线支付]、[转结算]不可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.ONLINEPAY, true, false);
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, false);
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
                if (cbSO_ApprovalStatusCode.Text == ApprovalStatusEnum.Name.YSH)
                {
                    #region 明细中[数量]、[单价]、[备注]列不可编辑

                    #region 销售数量

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

                    #region 备注

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Activation = Activation.ActivateOnly;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Appearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Appearance.BorderColor = CustomEnums.ColumnBorderColor.NoEditableColor;

                    #endregion

                    #endregion
                }
                else
                {
                    #region 明细中[数量]、[单价]、[备注]列不可编辑

                    #region 销售数量

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

                    #region 备注

                    //设置单元格 
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Activation = Activation.AllowEdit;
                    //设置单元格背景色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Appearance.BackColor = CustomEnums.CellBackColor.EditableColor;
                    //设置单元格边框颜色
                    loopGridRow.Cells[SystemTableColumnEnums.SD_SalesOrderDetail.Code.SOD_Remark].Appearance.BorderColor = CustomEnums.ColumnBorderColor.EditableColor;

                    #endregion

                    #endregion

                }

            }
            #endregion
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
                #region [销售订单]未保存 或 [销售订单].[审核状态]为[待审核]的场合，根据来源单获取[待出库明细]

                _bll.QueryForList(SQLID.SD_ProactiveSales_SQL13, new MDLSD_SalesOrder
                {
                    WHERE_SO_No = txtSO_SourceNo.Text.Trim(),
                }, _stockOutDetailDS);
                #endregion
            }
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
