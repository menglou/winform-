using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.FM;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.FM;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.ComModel;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinTabControl;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Coeus.UIModel.FM.APModel;

namespace SkyCar.Coeus.UI.FM
{
    /// <summary>
    /// 应付管理
    /// </summary>
    public partial class FrmAccountPayableBillManager : BaseFormCardListDetail<AccountPayableBillManagerUIModel, AccountPayableBillManagerQCModel, MDLFM_AccountPayableBill>
    {
        #region 全局变量

        /// <summary>
        /// 应付管理BLL
        /// </summary>
        private AccountPayableBillManagerBLL _bll = new AccountPayableBillManagerBLL();
        /// <summary>
        /// 历史应付单Grid数据源
        /// </summary>
        private List<AccountPayableBillManagerUIModel> _historyAPBillGridDS = new List<AccountPayableBillManagerUIModel>();
        /// <summary>
        /// 收付款单Grid数据源
        /// </summary>
        private List<ReceiptAndPayUIModel> _receiptAndPayBillGridDS = new List<ReceiptAndPayUIModel>();

        #region 下拉框数据源
        /// <summary>
        /// 单据方向
        /// </summary>
        List<ComComboBoxDataSourceTC> _billDirectionList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 来源类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _accountPayableBillSourceTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 单据状态
        /// </summary>
        List<ComComboBoxDataSourceTC> _accountPayableBillStatusList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 审核状态
        /// </summary>
        List<ComComboBoxDataSourceTC> _approvalStatusList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 收款对象类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _receiveObjectTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>SetDetailDSToCardCtrls
        /// 所有客户数据源
        /// </summary>
        List<ComClientUIModel> _tempAllClientList = new List<ComClientUIModel>();
        /// <summary>
        /// 客户数据源
        /// </summary>
        List<ComClientUIModel> _clientList = new List<ComClientUIModel>();
        #endregion

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmAccountPayableBillManager构造方法
        /// </summary>
        public FrmAccountPayableBillManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAccountPayableBillManager_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（动作，导航）
            base.ToolBarActionAndNavigate = toolBarActionAndNavigate;
            //基类.工具栏（翻页）
            base.ToolBarPaging = toolBarManagerListTabPaging;
            //查询委托（基类控制翻页用）
            base.ExecuteQuery = QueryAction;
            //工具栏（动作）单击事件
            this.toolBarActionAndNavigate.ToolClick += new ToolClickEventHandler(base.toolBarActionAndNavigate_ToolClick);
            //工具栏（翻页）单击事件
            this.toolBarManagerListTabPaging.ToolClick += new ToolClickEventHandler(base.ToolBarPaging_ToolClick);
            //工具栏（翻页）[当前页]值改变事件
            this.toolBarManagerListTabPaging.ToolValueChanged += new ToolEventHandler(base.ToolBarPaging_ToolValueChanged);
            #region 设置页面大小文本框
            TextBoxTool pageSizeOfListTextBox = null;
            foreach (var loopToolControl in this.toolBarManagerListTabPaging.Tools)
            {
                if (loopToolControl.Key == SysConst.EN_PAGESIZE)
                {
                    pageSizeOfListTextBox = (TextBoxTool)loopToolControl;
                }
            }
            if (pageSizeOfListTextBox != null)
            {
                pageSizeOfListTextBox.Text = PageSize.ToString();
                pageSizeOfListTextBox.AfterToolExitEditMode += PageSizeTextBoxTool_AfterToolExitEditMode;
            }
            #endregion

            _tempAllClientList = BLLCom.GetAllCustomerList(LoginInfoDAX.OrgID);

            //初始化【详情】Tab内控件
            InitializeDetailTabControls();
            //初始化【列表】Tab内控件
            InitializeListTabControls();
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
            //设置历史应付单元格样式
            SetHistoryAPBillStyle();
            //设置收付款单单元格样式
            SetReceiptAndPayStyle();

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            AcceptUIModelChanges();
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
        /// 【列表】Grid单元格改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_CellChange(object sender, CellEventArgs e)
        {
            gdGrid.UpdateData();
            decimal accountPayableAmount = 0;
            decimal paidAmount = 0;
            decimal unpaidAmount = 0;

            foreach (var loopGridRow in gdGrid.Rows)
            {
                if (string.IsNullOrEmpty(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountPayableBill.Code.APB_No].Value?.ToString())
                    || loopGridRow.Cells["IsChecked"].Value == null
                    || (bool)loopGridRow.Cells["IsChecked"].Value == false)
                {
                    continue;
                }
                accountPayableAmount = accountPayableAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountPayableBill.Code.APB_AccountPayableAmount].Value?.ToString());
                paidAmount = paidAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountPayableBill.Code.APB_PaidAmount].Value?.ToString());
                unpaidAmount = unpaidAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountPayableBill.Code.APB_UnpaidAmount].Value?.ToString());
            }

            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtAccountPayableAmount"])).Text = Convert.ToString(accountPayableAmount);
            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtPaidAmount"])).Text = Convert.ToString(paidAmount);
            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtUnpaidAmount"])).Text = Convert.ToString(unpaidAmount);
        }
        /// <summary>
        /// 【列表】Grid的AfterHeaderCheckStateChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            gdGrid.UpdateData();
            decimal accountPayableAmount = 0;
            decimal paidAmount = 0;
            decimal unpaidAmount = 0;

            foreach (var loopGridRow in gdGrid.Rows)
            {
                if (string.IsNullOrEmpty(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountPayableBill.Code.APB_No].Value?.ToString())
                    || loopGridRow.Cells["IsChecked"].Value == null
                    || (bool)loopGridRow.Cells["IsChecked"].Value == false)
                {
                    continue;
                }
                accountPayableAmount = accountPayableAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountPayableBill.Code.APB_AccountPayableAmount].Value?.ToString());
                paidAmount = paidAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountPayableBill.Code.APB_PaidAmount].Value?.ToString());
                unpaidAmount = unpaidAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountPayableBill.Code.APB_UnpaidAmount].Value?.ToString());
            }

            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtAccountPayableAmount"])).Text = Convert.ToString(accountPayableAmount);
            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtPaidAmount"])).Text = Convert.ToString(paidAmount);
            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtUnpaidAmount"])).Text = Convert.ToString(unpaidAmount);
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
            //[列表]页不允许删除，允许[对账]
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.RECONCILIATION, true);

                //导航按钮[转结算]可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, true);
            }
            else
            {
                //设置动作按钮状态
                SetActionEnableByStatus();
            }
        }

        #endregion

        #region 查询条件相关事件

        /// <summary>
        /// 来源类型_ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_APB_SourceTypeName_ValueChanged(object sender, EventArgs e)
        {
            txtWhere_APB_SourceBillNo.Clear();
            if (cbWhere_APB_SourceTypeName.Text == AccountPayableBillSourceTypeEnum.Name.SGCJ)
            {
                lblWhere_APB_SourceBillNo.Visible = false;
                txtWhere_APB_SourceBillNo.Visible = false;
            }
            else
            {
                lblWhere_APB_SourceBillNo.Visible = true;
                txtWhere_APB_SourceBillNo.Visible = true;
            }
        }

        /// <summary>
        /// 应付单号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_APB_No_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //执行查询
                //QueryAction();
            }
        }

        /// <summary>
        /// 来源单号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_APB_SourceBillNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //执行查询
                //QueryAction();
            }
        }

        /// <summary>
        /// 审核状态ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_APB_ApprovalStatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }

        /// <summary>
        /// 有效CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckWhere_APB_IsValid_CheckedChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }

        /// <summary>
        /// 【列表】来源单号EditorButtonClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_APB_SourceBillNo_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(cbWhere_APB_SourceTypeName.Text))
            {
                //请先选择来源类型，再选择来源单号
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0034, new object[] { MsgParam.SOURCE_TYPE, MsgParam.SOURCE_NO }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cbWhere_APB_SourceTypeName.Text == AccountPayableBillSourceTypeEnum.Name.SHYF)
            {
                #region 从[采购订单]中获取[来源单号]
                //仅查询[审核状态]为[已审核]的出库单
                var paramApprovetatusList = _approvalStatusList.Where(x => x.Text == ApprovalStatusEnum.Name.YSH).ToList();

                Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
                {
                    {ComViewParamKey.ApprovalStatus.ToString(), paramApprovetatusList}
                };

                FrmPurchaseOrderQuery frmPurchaseOrderQuery = new FrmPurchaseOrderQuery(paramViewParameters)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmPurchaseOrderQuery.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    txtWhere_APB_SourceBillNo.Text = Convert.ToString(frmPurchaseOrderQuery.SelectedGridList[0].PO_No);
                }

                #endregion
            }

            else if (cbWhere_APB_SourceTypeName.Text == AccountPayableBillSourceTypeEnum.Name.CKYF)
            {
                #region 从[出库单]中获取[来源单号]

                Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
                {
                    {ComViewParamKey.ApprovalStatus.ToString(), ApprovalStatusEnum.Name.YSH},
                    {ComViewParamKey.SourceType.ToString(), StockOutBillSourceTypeEnum.Name.THCK}
                };

                FrmStockOutBillQuery frmStockInBillQuery = new FrmStockOutBillQuery(paramViewParameters, CustomEnums.CustomeSelectionMode.Single)
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                DialogResult dialogResult = frmStockInBillQuery.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    txtWhere_APB_SourceBillNo.Text = Convert.ToString(frmStockInBillQuery.SelectedGridList[0].SOB_No);
                }

                #endregion
            }
        }

        /// <summary>
        /// [创建时间-终了]ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_CreatedTimeEnd_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtWhere_CreatedTimeEnd.Value != null &&
                this.dtWhere_CreatedTimeEnd.DateTime.Hour == 0 &&
                this.dtWhere_CreatedTimeEnd.DateTime.Minute == 0 &&
                this.dtWhere_CreatedTimeEnd.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_CreatedTimeEnd.DateTime.Year, this.dtWhere_CreatedTimeEnd.DateTime.Month, this.dtWhere_CreatedTimeEnd.DateTime.Day, 23, 59, 59);
                this.dtWhere_CreatedTimeEnd.DateTime = newDateTime;
            }
        }

        /// <summary>
        /// [修改时间-终了]事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_UpdatedTimeEnd_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtWhere_UpdatedTimeEnd.Value != null &&
                this.dtWhere_UpdatedTimeEnd.DateTime.Hour == 0 &&
                this.dtWhere_UpdatedTimeEnd.DateTime.Minute == 0 &&
                this.dtWhere_UpdatedTimeEnd.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_UpdatedTimeEnd.DateTime.Year, this.dtWhere_UpdatedTimeEnd.DateTime.Month, this.dtWhere_UpdatedTimeEnd.DateTime.Day, 23, 59, 59);
                this.dtWhere_UpdatedTimeEnd.DateTime = newDateTime;
            }
        }

        #endregion

        #region 详情相关事件

        /// <summary>
        /// [来源类型]ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbAPB_SourceTypeName_ValueChanged(object sender, EventArgs e)
        {
            txtAPB_SourceBillNo.Clear();
            if (cbAPB_SourceTypeName.Text == AccountPayableBillSourceTypeEnum.Name.SGCJ)
            {
                lblAPB_SourceBillNo.Visible = false;
                txtAPB_SourceBillNo.Visible = false;
                lblAPB_SourceBillNoX.Visible = false;
                //来源类型为[手工创建]的场合，隐藏【历史应付单】Tab
                tabControlDetail.Tabs["HistoryAPList"].Visible = false;

            }
            else
            {
                lblAPB_SourceBillNo.Visible = true;
                txtAPB_SourceBillNo.Visible = true;
                lblAPB_SourceBillNoX.Visible = true;
                //来源类型不为[手工创建]的场合，显示【历史应付单】Tab
                tabControlDetail.Tabs["HistoryAPList"].Visible = true;

            }
        }
        /// <summary>
        /// [收款对象类型]ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbAPB_ReceiveObjectTypeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curClientType = mcbAPB_ReceiveObjectName.SelectedTextExtra;
            if (curClientType != mcbAPB_ReceiveObjectTypeName.SelectedText)
            {
                _clientList.Clear();
                if (string.IsNullOrEmpty(mcbAPB_ReceiveObjectTypeName.SelectedText))
                {
                    _bll.CopyModelList(_tempAllClientList, _clientList);
                }
                else
                {
                    foreach (var loopTempClient in _tempAllClientList)
                    {
                        if (loopTempClient.ClientType == mcbAPB_ReceiveObjectTypeName.SelectedText)
                        {
                            _clientList.Add(loopTempClient);
                        }
                    }
                }
                mcbAPB_ReceiveObjectName.DataSource = _clientList;
                mcbAPB_ReceiveObjectName.Clear();
            }
            else if (string.IsNullOrEmpty(mcbAPB_ReceiveObjectTypeName.SelectedText))
            {
                _bll.CopyModelList(_tempAllClientList, _clientList);
                mcbAPB_ReceiveObjectName.DataSource = _clientList;
                mcbAPB_ReceiveObjectName.Clear();
            }
        }

        /// <summary>
        /// [收款对象]SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbAPB_ReceiveObjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //根据选择的对象选中对象类型
            if (!string.IsNullOrEmpty(mcbAPB_ReceiveObjectName.SelectedTextExtra))
            {
                mcbAPB_ReceiveObjectTypeName.SelectedText = mcbAPB_ReceiveObjectName.SelectedTextExtra;
            }
            else if (!string.IsNullOrEmpty(mcbAPB_ReceiveObjectName.SelectedText))
            {
                mcbAPB_ReceiveObjectTypeName.SelectedText = "其他";
            }
        }

        /// <summary>
        /// [应付金额]KeyUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numAPB_AccountPayableAmount_KeyUp(object sender, KeyEventArgs e)
        {
            decimal paidAmount =
                 Convert.ToDecimal(numAPB_PaidAmount.Value ?? 0);
            decimal accountPayableAmount =
                Convert.ToDecimal(numAPB_AccountPayableAmount.Value ?? 0);
            numAPB_UnpaidAmount.Text = Convert.ToString(accountPayableAmount - paidAmount);
        }

        #endregion

        #endregion

        #region 重写基类方法

        /// <summary>
        /// 新增
        /// </summary>
        public override void NewAction()
        {
            #region 检查详情是否已保存

            SetCardCtrlsToDetailDS();
            base.NewUIModel = HeadDS;
            if (ViewHasChanged())
            {
                //信息尚未保存，确定进行当前操作？
                DialogResult dialogResult = MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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

            //5. 设置动作按钮状态
            SetActionEnableByStatus();
            //6.设置详情页面控件的是否可编辑
            SetDetailControl();

            //将最新的值Copy到初始UIModel
            SetCardCtrlsToDetailDS();
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
            //3.将【详情】Tab内控件的值赋值给基类的HeadDS
            SetCardCtrlsToDetailDS();
            //4.执行保存（含服务端检查）
            if (!_bll.SaveDetailDS(HeadDS))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //5.将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //6.设置动作按钮状态
            SetActionEnableByStatus();
            //7.设置详情页面控件的是否可编辑
            SetDetailControl();
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
            //将HeadDS转换为TBModel对象
            var argsHead = HeadDS.ToTBModelForSaveAndDelete<MDLFM_AccountPayableBill>();

            //2.执行删除
            if (!_bll.Delete(argsHead))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //3.清空【详情】画面数据
            InitializeDetailTabControls();
            //刷新列表
            RefreshList(true);

            SetCardCtrlsToDetailDS();
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
            ConditionDS = new AccountPayableBillManagerQCModel()
            {
                //查询用SqlId 
                SqlId = SQLID.FM_AccountPayableBillManager_SQL_01,
                //应付单号
                WHERE_APB_No = txtWhere_APB_No.Text.Trim(),
                //来源类型
                WHERE_APB_SourceTypeName = cbWhere_APB_SourceTypeName.Text.Trim(),
                //来源单号
                WHERE_APB_SourceBillNo = txtWhere_APB_SourceBillNo.Text.Trim(),
                //审核状态
                WHERE_APB_ApprovalStatusName = cbWhere_APB_ApprovalStatusName.Text.Trim(),
                //收款对象类型
                WHERE_APB_ReceiveObjectTypeName = mcbWhere_ARB_PayObjectName.SelectedTextExtra,
                //收款对象
                WHERE_APB_ReceiveObjectName = mcbWhere_ARB_PayObjectName.SelectedText,
                //组织ID
                WHERE_APB_Org_ID = LoginInfoDAX.OrgID,
                //有效
                WHERE_APB_IsValid = ckWhere_APB_IsValid.Checked,
            };

            //业务状态
            if (!string.IsNullOrEmpty(mcbWhere_APB_BusinessStatusName.SelectedText))
            {
                string businessStatusName = string.Empty;
                businessStatusName = SysConst.Semicolon_DBC;
                businessStatusName = businessStatusName + mcbWhere_APB_BusinessStatusName.SelectedText.Trim();
                ConditionDS.WHERE_APB_BusinessStatusName = businessStatusName;
            }

            //创建时间-开始
            if (dtWhere_CreatedTimeStart.Value != null)
            {
                ConditionDS._CreatedTimeStart = dtWhere_CreatedTimeStart.DateTime;
            }
            //创建时间-终了
            if (dtWhere_CreatedTimeEnd.Value != null)
            {
                ConditionDS._CreatedTimeEnd = dtWhere_CreatedTimeEnd.DateTime;
            }
            //修改时间-开始
            if (dtWhere_UpdatedTimeStart.Value != null)
            {
                ConditionDS._UpdatedTimeStart = dtWhere_UpdatedTimeStart.DateTime;
            }
            //修改时间-终了
            if (dtWhere_UpdatedTimeEnd.Value != null)
            {
                ConditionDS._UpdatedTimeEnd = dtWhere_UpdatedTimeEnd.DateTime;
            }
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            //6.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;

        }

        /// <summary>
        /// 清空查询条件
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
            if (ViewHasChanged())
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
            paramGridName = SystemTableEnums.Name.FM_AccountPayableBillDetail;
            base.ExportAction(gdGrid, paramGridName);
        }

        /// <summary>
        /// 导出全部
        /// </summary>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.FM_AccountPayableBillDetail;
            List<AccountPayableBillManagerUIModel> resultAllList = new List<AccountPayableBillManagerUIModel>();
            _bll.QueryForList(SQLID.FM_AccountPayableBillManager_SQL_01, new AccountPayableBillManagerQCModel()
            {
                PageIndex = 1,
                PageSize = null,
                //应付单号
                WHERE_APB_No = txtWhere_APB_No.Text.Trim(),
                //来源类型
                WHERE_APB_SourceTypeName = cbWhere_APB_SourceTypeName.Text.Trim(),
                //来源单号
                WHERE_APB_SourceBillNo = txtWhere_APB_SourceBillNo.Text.Trim(),
                //业务状态
                WHERE_APB_BusinessStatusName = mcbWhere_APB_BusinessStatusName.SelectedText.Trim(),
                //审核状态
                WHERE_APB_ApprovalStatusName = cbWhere_APB_ApprovalStatusName.Text.Trim(),
                //收款对象类型
                WHERE_APB_ReceiveObjectTypeName = mcbWhere_ARB_PayObjectName.SelectedTextExtra,
                //收款对象
                WHERE_APB_ReceiveObjectName = mcbWhere_ARB_PayObjectName.SelectedText,
                //组织ID
                WHERE_APB_Org_ID = LoginInfoDAX.OrgID,
                //有效
                WHERE_APB_IsValid = ckWhere_APB_IsValid.Checked,
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
            base.ApproveAction();
            //1.前端检查-审核
            if (!ClientCheckForApprove())
            {
                return;
            }

            SetCardCtrlsToDetailDS();

            bool saveApprove = _bll.ApproveDetailDS(HeadDS);
            //审核失败
            if (!saveApprove)
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //审核成功
            MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.APPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置动作按钮状态
            SetActionEnableByStatus();
            //设置详情页面控件的是否可编辑
            SetDetailControl();
        }

        /// <summary>
        /// 反审核
        /// </summary>
        public override void UnApproveAction()
        {
            base.UnApproveAction();

            if (!ClientCheckForUnApprove())
            {
                return;
            }

            SetCardCtrlsToDetailDS();

            bool saveUnApprove = _bll.UnApproveDetailDS(HeadDS);
            //反审核失败
            if (!saveUnApprove)
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //反审核成功
            MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.UNAPPROVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //将HeadDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置动作按钮状态
            SetActionEnableByStatus();
            //设置详情页面控件的是否可编辑
            SetDetailControl();
        }

        /// <summary>
        /// 对账
        /// </summary>
        public override void ReconciliationAction()
        {
            //待对账的应付单列表
            List<AccountPayableBillManagerUIModel> accountPayableBillToReconciliation = new List<AccountPayableBillManagerUIModel>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 详情对账

                if (string.IsNullOrEmpty(HeadDS.APB_ID)
                    || string.IsNullOrEmpty(HeadDS.APB_No))
                {
                    //应付单信息为空，不能对账
                    MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.W_0016, new object[] { SystemTableEnums.Name.FM_AccountPayableBill, SystemActionEnum.Name.RECONCILIATION }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //确认对账操作
                DialogResult isConfirmReconciliation = MessageBoxs.Show(Trans.FM, this.ToString(),
                    MsgHelp.GetMsg(MsgCode.W_0040, new object[] { SystemActionEnum.Name.RECONCILIATION }),
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (isConfirmReconciliation != DialogResult.OK)
                {
                    return;
                }

                accountPayableBillToReconciliation.Add(HeadDS);
                #endregion
            }
            else
            {
                #region 列表对账

                gdGrid.UpdateData();
                var checkedAccountPayableBillList = HeadGridDS.Where(x => x.IsChecked == true).ToList();
                if (checkedAccountPayableBillList.Count == 0)
                {
                    //请选择执行中的应付单对账！
                    MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { AccountPayableBillStatusEnum.Name.ZXZ + MsgParam.OF + SystemTableEnums.Name.FM_AccountPayableBill + SystemActionEnum.Name.RECONCILIATION }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //[业务状态]为{执行中}的场合，[对账]可用，否则不可用
                var notZXZBill =
                    checkedAccountPayableBillList.Where(
                        x => x.APB_BusinessStatusName != AccountPayableBillStatusEnum.Name.ZXZ).ToList();
                if (notZXZBill.Count > 0)
                {
                    //请选择执行中的应付单对账！
                    MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { AccountPayableBillStatusEnum.Name.ZXZ + MsgParam.OF + SystemTableEnums.Name.FM_AccountPayableBill + SystemActionEnum.Name.RECONCILIATION }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    foreach (var loopNotZXZBill in notZXZBill)
                    {
                        loopNotZXZBill.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }
                //确认对账操作
                DialogResult isConfirmReconciliation = MessageBoxs.Show(Trans.FM, this.ToString(),
                    MsgHelp.GetMsg(MsgCode.W_0040, new object[] { SystemActionEnum.Name.RECONCILIATION }),
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (isConfirmReconciliation != DialogResult.OK)
                {
                    return;
                }

                accountPayableBillToReconciliation.AddRange(checkedAccountPayableBillList);
                #endregion
            }

            //对账时间
            if (dtAPB_ReconciliationTime.Value == null)
            {
                dtAPB_ReconciliationTime.Value = BLLCom.GetCurStdDatetime();
            }
            SetCardCtrlsToDetailDS();

            bool saveReconciliation = _bll.ReconciliationDetailDS(accountPayableBillToReconciliation);
            if (!saveReconciliation)
            {
                //对账失败
                MessageBoxs.Show(Trans.FM, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //对账成功
            MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.RECONCILIATION }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected && accountPayableBillToReconciliation.Count == 1)
            {
                _bll.CopyModel(accountPayableBillToReconciliation[0], HeadDS);
            }

            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //控制详情是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void PrintAction()
        {
            base.PrintAction();
            if (HeadDS == null || string.IsNullOrWhiteSpace(HeadDS.APB_No))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, SystemTableEnums.Name.FM_AccountPayableBill), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (HeadDS.APB_ApprovalStatusName != ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, SystemTableEnums.Name.FM_AccountPayableBill + cbAPB_ApprovalStatusName.Text, SystemActionEnum.Name.PRINT), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                AccountPayableBillUIModelToPrint accountPayableBillToPrint = new AccountPayableBillUIModelToPrint();
                _bll.CopyModel(HeadDS, accountPayableBillToPrint);

                Dictionary<string, object> argsViewParams = new Dictionary<string, object>
                {
                    {FMViewParamKey.AccountPayableBill.ToString(), accountPayableBillToPrint},
                };

                FrmViewAndPrintAccountPayableBill frmViewAndPrintAccountPayableBill = new FrmViewAndPrintAccountPayableBill
                {
                    ViewParameters = argsViewParams
                };
                frmViewAndPrintAccountPayableBill.ViewParameters = argsViewParams;
                frmViewAndPrintAccountPayableBill.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBoxs.Show(Trans.PIS, this.ToString(), ex.Message.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// 转结算
        /// </summary>
        public override void ToSettlementNavigate()
        {
            base.ToSettlementNavigate();
            //选中的待付款的[应付单]列表
            List<AccountPayableBillManagerUIModel> accountPayableBillList = new List<AccountPayableBillManagerUIModel>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 当前选中Tab为【详情】
                if (HeadDS == null || string.IsNullOrEmpty(HeadDS.APB_No))
                {
                    //请选择一个执行中或已对账的应付单转结算
                    MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + AccountPayableBillStatusEnum.Name.ZXZ + MsgParam.OR + AccountPayableBillStatusEnum.Name.YDZ + MsgParam.OF + SystemTableEnums.Name.FM_AccountPayableBill + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (HeadDS.APB_BusinessStatusName != AccountPayableBillStatusEnum.Name.ZXZ
                    && HeadDS.APB_BusinessStatusName != AccountPayableBillStatusEnum.Name.YDZ)
                {
                    //请选择一个执行中或已对账的应付单转结算
                    MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + AccountPayableBillStatusEnum.Name.ZXZ + MsgParam.OR + AccountPayableBillStatusEnum.Name.YDZ + MsgParam.OF + SystemTableEnums.Name.FM_AccountPayableBill + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (HeadDS.APB_ApprovalStatusName != ApprovalStatusEnum.Name.YSH)
                {
                    //请选择一个已审核的应付单转结算
                    MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + ApprovalStatusEnum.Name.YSH + MsgParam.OF + SystemTableEnums.Name.FM_AccountPayableBill + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                accountPayableBillList.Add(HeadDS);

                #endregion
            }
            else
            {
                #region 当前选中Tab为【列表】

                var checkedGrid = HeadGridDS.Where(x => x.IsChecked == true).ToList();

                if (checkedGrid.Count == 0)
                {
                    //请至少勾选一条执行中或已对账的[应付单]信息进行转结算
                    MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[]
                        { AccountPayableBillStatusEnum.Name.ZXZ +MsgParam.OR + AccountPayableBillStatusEnum.Name.YDZ + MsgParam.OF + SystemTableEnums.Name.FM_AccountPayableBill, SystemNavigateEnum.Name.TOSETTLEMENT }),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var notCheckedZXZOrYDZ = checkedGrid.Where(x => x.APB_BusinessStatusName != AccountPayableBillStatusEnum.Name.ZXZ && x.APB_BusinessStatusName != AccountPayableBillStatusEnum.Name.YDZ).ToList();
                if (notCheckedZXZOrYDZ.Count > 0)
                {
                    //请选择执行中或已对账的应付单进行转结算
                    MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[]
                        { AccountPayableBillStatusEnum.Name.ZXZ +MsgParam.OR + AccountPayableBillStatusEnum.Name.YDZ + MsgParam.OF + SystemTableEnums.Name.FM_AccountPayableBill+SystemNavigateEnum.Name.TOSETTLEMENT }),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    foreach (var loopCheckedItem in notCheckedZXZOrYDZ)
                    {
                        loopCheckedItem.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }
                var notCheckedYSH = checkedGrid.Where(x => x.APB_ApprovalStatusName != ApprovalStatusEnum.Name.YSH).ToList();
                if (notCheckedYSH.Count > 0)
                {
                    //请选择已审核的应付单进行转结算
                    MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[]
                        { ApprovalStatusEnum.Name.YSH + MsgParam.OF + SystemTableEnums.Name.FM_AccountPayableBill+SystemNavigateEnum.Name.TOSETTLEMENT }),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    foreach (var loopCheckedItem in notCheckedYSH)
                    {
                        loopCheckedItem.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }
                var firstCheckedItem = checkedGrid.FirstOrDefault(x => x.APB_ApprovalStatusName == ApprovalStatusEnum.Name.YSH
                                                                && (x.APB_BusinessStatusName == AccountPayableBillStatusEnum.Name.ZXZ || x.APB_BusinessStatusName == AccountPayableBillStatusEnum.Name.YDZ));
                if (firstCheckedItem == null)
                {
                    //请至少勾选一条已审核并且执行中或已对账的应付单信息进行转结算
                    MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[]
                        { ApprovalStatusEnum.Name.YSH + MsgParam.AND + AccountPayableBillStatusEnum.Name.ZXZ +MsgParam.OR+AccountPayableBillStatusEnum.Name.YDZ + MsgParam.OF + SystemTableEnums.Name.FM_AccountPayableBill, SystemNavigateEnum.Name.TOSETTLEMENT }),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var differFirstCheckedItem = checkedGrid.Where(x => x.APB_ReceiveObjectName != firstCheckedItem.APB_ReceiveObjectName
                || x.APB_ReceiveObjectID != firstCheckedItem.APB_ReceiveObjectID).ToList();
                if (differFirstCheckedItem.Count > 0)
                {
                    //请选择相同来源类型，相同客户的应付单转结算
                    MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { ApprovalStatusEnum.Name.YSH + MsgParam.OF + MsgParam.SAME_SOURCEANDCUSTOMER + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var loopCheckedItem in differFirstCheckedItem)
                    {
                        loopCheckedItem.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }
                _bll.CopyModelList(checkedGrid, accountPayableBillList);

                #endregion
            }

            //未付金额
            decimal UnpaidAmount = 0;
            //未收金额
            decimal UnReceiveAmount = 0;
            //循环遍历list，计算出未付金额，未收金额
            foreach (var loopAccountPayableBill in accountPayableBillList)
            {
                if (loopAccountPayableBill.APB_BillDirectName == BillDirectionEnum.Name.PLUS)
                {
                    UnpaidAmount = UnpaidAmount + (loopAccountPayableBill.APB_UnpaidAmount ?? 0);
                }
                else if (loopAccountPayableBill.APB_BillDirectName == BillDirectionEnum.Name.MINUS)
                {
                    UnReceiveAmount = UnReceiveAmount - (loopAccountPayableBill.APB_UnpaidAmount ?? 0);
                }

            }

            #region 判断未付金额，未收金额二者是谁大，未付金额大转付款，未收金额大转收款单

            if (UnpaidAmount >= UnReceiveAmount)
            {
                #region 转付款

                //待确认付款的应付单列表
                List<PayablePayConfirmUIModel> accountPayableBillToPayList = new List<PayablePayConfirmUIModel>();
                foreach (var loopAccountPayableBill in accountPayableBillList)
                {
                    PayablePayConfirmUIModel accountPayableBillToPay = new PayablePayConfirmUIModel
                    {
                        IsBusinessSourceAccountPayableBill = true,
                        BusinessID = loopAccountPayableBill.APB_ID,
                        BusinessNo = loopAccountPayableBill.APB_No,
                        BusinessOrgID = loopAccountPayableBill.APB_Org_ID,
                        BusinessOrgName = loopAccountPayableBill.APB_Org_Name,
                        BusinessSourceTypeName = AmountTransObjectTypeEnum.Name.AUTOPARTSSUPPLIER,
                        BusinessSourceTypeCode = AmountTransObjectTypeEnum.Code.AUTOPARTSSUPPLIER,
                        ReceiveObjectTypeCode = loopAccountPayableBill.APB_ReceiveObjectTypeCode,
                        ReceiveObjectTypeName = loopAccountPayableBill.APB_ReceiveObjectTypeName,
                        ReceiveObjectID = loopAccountPayableBill.APB_ReceiveObjectID,
                        ReceiveObjectName = loopAccountPayableBill.APB_ReceiveObjectName,
                        //应付单相关
                        APB_ID = loopAccountPayableBill.APB_ID,
                        APB_No = loopAccountPayableBill.APB_No,
                        APB_BillDirectCode = loopAccountPayableBill.APB_BillDirectCode,
                        APB_BillDirectName = loopAccountPayableBill.APB_BillDirectName,
                        APB_SourceTypeCode = loopAccountPayableBill.APB_SourceTypeCode,
                        APB_SourceTypeName = loopAccountPayableBill.APB_SourceTypeName,
                        APB_SourceBillNo = loopAccountPayableBill.APB_SourceBillNo,
                        APB_Org_ID = loopAccountPayableBill.APB_Org_ID,
                        APB_Org_Name = loopAccountPayableBill.APB_Org_Name,
                        APB_ReceiveObjectTypeCode = loopAccountPayableBill.APB_ReceiveObjectTypeCode,
                        APB_ReceiveObjectTypeName = loopAccountPayableBill.APB_ReceiveObjectTypeName,
                        APB_ReceiveObjectID = loopAccountPayableBill.APB_ReceiveObjectID,
                        APB_ReceiveObjectName = loopAccountPayableBill.APB_ReceiveObjectName,
                        APB_AccountPayableAmount = loopAccountPayableBill.APB_AccountPayableAmount,
                        APB_PaidAmount = loopAccountPayableBill.APB_PaidAmount,
                        APB_UnpaidAmount = loopAccountPayableBill.APB_UnpaidAmount,
                        APB_BusinessStatusCode = loopAccountPayableBill.APB_BusinessStatusCode,
                        APB_BusinessStatusName = loopAccountPayableBill.APB_BusinessStatusName,
                        APB_ApprovalStatusCode = loopAccountPayableBill.APB_ApprovalStatusCode,
                        APB_ApprovalStatusName = loopAccountPayableBill.APB_ApprovalStatusName,
                        APB_CreatedBy = loopAccountPayableBill.APB_CreatedBy,
                        APB_CreatedTime = loopAccountPayableBill.APB_CreatedTime,
                        APB_VersionNo = loopAccountPayableBill.APB_VersionNo,
                    };

                    //if (loopAccountPayableBill.APB_BillDirectName == BillDirectionEnum.Name.PLUS)
                    //{
                    accountPayableBillToPay.PayableTotalAmount = loopAccountPayableBill.APB_AccountPayableAmount;
                    accountPayableBillToPay.PayTotalAmount = loopAccountPayableBill.APB_PaidAmount;
                    accountPayableBillToPay.UnPayTotalAmount = loopAccountPayableBill.APB_UnpaidAmount;
                    //}
                    //else
                    //{
                    //    accountPayableBillToPay.PayableTotalAmount = -loopAccountPayableBill.APB_AccountPayableAmount;
                    //    accountPayableBillToPay.PayTotalAmount = -loopAccountPayableBill.APB_PaidAmount;
                    //    accountPayableBillToPay.UnPayTotalAmount = -loopAccountPayableBill.APB_UnpaidAmount;
                    //}
                    accountPayableBillToPayList.Add(accountPayableBillToPay);
                }
                Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
                {
                     //应付单确认付款
                    {FMViewParamKey.PayableCashierConfirm.ToString(), accountPayableBillToPayList}
                };

                //跳转[业务单确认付款弹出窗]
                FrmPayablePayConfirmWindow frmBusinessPayConfirmWindow = new FrmPayablePayConfirmWindow(paramViewParameters)
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
            else
            {
                #region 转收款

                //待确认收款的应付单信息列表
                List<ReceiveableCollectionConfirmUIModel> accountPayableToReceiptList = new List<ReceiveableCollectionConfirmUIModel>();
                foreach (var loopAccountPayableBill in accountPayableBillList)
                {
                    ReceiveableCollectionConfirmUIModel accountPayableToReceipt = new ReceiveableCollectionConfirmUIModel
                    {
                        IsBusinessSourceAccountPayableBill = true,
                        BusinessID = loopAccountPayableBill.APB_ID,
                        BusinessNo = loopAccountPayableBill.APB_No,
                        BusinessOrgID = loopAccountPayableBill.APB_Org_ID,
                        BusinessOrgName = loopAccountPayableBill.APB_Org_Name,
                        BusinessSourceTypeName = AmountTransObjectTypeEnum.Name.AUTOPARTSSUPPLIER,
                        BusinessSourceTypeCode = AmountTransObjectTypeEnum.Code.AUTOPARTSSUPPLIER,
                        PayObjectTypeCode = loopAccountPayableBill.APB_ReceiveObjectTypeCode,
                        PayObjectTypeName = loopAccountPayableBill.APB_ReceiveObjectTypeName,
                        PayObjectName = loopAccountPayableBill.APB_ReceiveObjectName,
                        PayObjectID = loopAccountPayableBill.APB_ReceiveObjectID,

                        //应收单相关
                        ARB_ID = loopAccountPayableBill.APB_ID,
                        ARB_No = loopAccountPayableBill.APB_No,
                        ARB_BillDirectCode = loopAccountPayableBill.APB_BillDirectCode,
                        ARB_BillDirectName = loopAccountPayableBill.APB_BillDirectName,
                        ARB_SourceTypeCode = loopAccountPayableBill.APB_SourceTypeCode,
                        ARB_SourceTypeName = loopAccountPayableBill.APB_SourceTypeName,
                        ARB_SrcBillNo = loopAccountPayableBill.APB_SourceBillNo,
                        ARB_Org_ID = loopAccountPayableBill.APB_Org_ID,
                        ARB_Org_Name = loopAccountPayableBill.APB_Org_Name,
                        ARB_PayObjectTypeCode = loopAccountPayableBill.APB_ReceiveObjectTypeCode,
                        ARB_PayObjectTypeName = loopAccountPayableBill.APB_ReceiveObjectTypeName,
                        ARB_PayObjectName = loopAccountPayableBill.APB_ReceiveObjectName,
                        ARB_PayObjectID = loopAccountPayableBill.APB_ReceiveObjectID,
                        ARB_AccountReceivableAmount = loopAccountPayableBill.APB_AccountPayableAmount,
                        ARB_ReceivedAmount = loopAccountPayableBill.APB_PaidAmount,
                        ARB_UnReceiveAmount = loopAccountPayableBill.APB_UnpaidAmount,
                        ARB_BusinessStatusCode = loopAccountPayableBill.APB_BusinessStatusCode,
                        ARB_BusinessStatusName = loopAccountPayableBill.APB_BusinessStatusName,
                        ARB_ApprovalStatusCode = loopAccountPayableBill.APB_ApprovalStatusCode,
                        ARB_ApprovalStatusName = loopAccountPayableBill.APB_ApprovalStatusName,
                        ARB_CreatedBy = loopAccountPayableBill.APB_CreatedBy,
                        ARB_CreatedTime = loopAccountPayableBill.APB_CreatedTime,
                        ARB_VersionNo = loopAccountPayableBill.APB_VersionNo,
                    };
                    //if (loopAccountPayableBill.APB_BillDirectName == BillDirectionEnum.Name.PLUS)
                    //{
                    accountPayableToReceipt.ReceivableTotalAmount = -loopAccountPayableBill.APB_AccountPayableAmount;
                    accountPayableToReceipt.ReceiveTotalAmount = -loopAccountPayableBill.APB_PaidAmount;
                    accountPayableToReceipt.UnReceiveTotalAmount = -loopAccountPayableBill.APB_UnpaidAmount;
                    //}
                    //else
                    //{
                    //    accountPayableToReceipt.ReceivableTotalAmount = loopAccountPayableBill.APB_AccountPayableAmount;
                    //    accountPayableToReceipt.ReceiveTotalAmount = loopAccountPayableBill.APB_PaidAmount;
                    //    accountPayableToReceipt.UnReceiveTotalAmount = loopAccountPayableBill.APB_UnpaidAmount;
                    //}
                    accountPayableToReceiptList.Add(accountPayableToReceipt);
                }

                Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
                {
                    //业务单确认收款
                    {FMViewParamKey.ReceiveableCashierConfirm.ToString(), accountPayableToReceiptList}
                };

                //跳转[业务单确认收款弹出窗]
                FrmReceiveableCollectionConfirmWindow frmBusinessCollectionConfirmWindow = new FrmReceiveableCollectionConfirmWindow(paramViewParameters)
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
            #endregion

        }

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //应付单号
            txtAPB_No.Clear();
            //单据方向
            cbAPB_BillDirectName.Items.Clear();
            //来源类型
            cbAPB_SourceTypeName.Items.Clear();
            //来源单号
            txtAPB_SourceBillNo.Clear();
            //组织名称
            txtAPB_Org_Name.Clear();
            txtAPB_Org_Name.Text = LoginInfoDAX.OrgShortName;
            //收款对象类型名称
            mcbAPB_ReceiveObjectTypeName.Items.Clear();
            //收款对象名称
            mcbAPB_ReceiveObjectName.Clear();
            //应付金额
            numAPB_AccountPayableAmount.Value = null;
            //已付金额
            numAPB_PaidAmount.Value = null;
            //未付金额
            numAPB_UnpaidAmount.Value = null;
            //业务状态
            cbAPB_BusinessStatusName.Items.Clear();
            //审核状态
            cbAPB_ApprovalStatusName.Items.Clear();
            //对账时间
            dtAPB_ReconciliationTime.Value = null;
            //备注
            txtAPB_Remark.Clear();
            //有效
            ckAPB_IsValid.Checked = true;
            ckAPB_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtAPB_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtAPB_CreatedTime.Value = DateTime.Now;
            //修改人
            txtAPB_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtAPB_UpdatedTime.Value = DateTime.Now;
            //应付单ID
            txtAPB_ID.Clear();
            //组织ID
            txtAPB_Org_ID.Clear();
            txtAPB_Org_ID.Text = LoginInfoDAX.OrgID;
            //收款对象ID
            txtAPB_ReceiveObjectID.Clear();
            //版本号
            txtAPB_VersionNo.Clear();
            //给 应付单号 设置焦点
            lblAPB_No.Focus();

            #endregion

            #region 初始化下拉框
            //单据方向【正向，负向】
            _billDirectionList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.BillDirection);
            cbAPB_BillDirectName.DisplayMember = SysConst.EN_TEXT;
            cbAPB_BillDirectName.ValueMember = SysConst.EN_Code;
            cbAPB_BillDirectName.DataSource = _billDirectionList;
            cbAPB_BillDirectName.Text = BillDirectionEnum.Name.PLUS;
            cbAPB_BillDirectName.Value = BillDirectionEnum.Code.PLUS;
            cbAPB_BillDirectName.DataBind();

            //来源类型【收货应付（入库单）】
            _accountPayableBillSourceTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.AccountPayableBillSourceType);

            cbAPB_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbAPB_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbAPB_SourceTypeName.DataSource = _accountPayableBillSourceTypeList.Where(x => x.Text == AccountPayableBillSourceTypeEnum.Name.SGCJ).ToList();
            cbAPB_SourceTypeName.Text = AccountPayableBillSourceTypeEnum.Name.SGCJ;
            cbAPB_SourceTypeName.Value = AccountPayableBillSourceTypeEnum.Code.SGCJ;
            cbAPB_SourceTypeName.DataBind();


            //应付单状态【已生成，执行中，已完成】
            _accountPayableBillStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.AccountPayableBillStatus);
            cbAPB_BusinessStatusName.DisplayMember = SysConst.EN_TEXT;
            cbAPB_BusinessStatusName.ValueMember = SysConst.EN_Code;
            cbAPB_BusinessStatusName.DataSource = _accountPayableBillStatusList;
            cbAPB_BusinessStatusName.Text = AccountPayableBillStatusEnum.Name.YSC;
            cbAPB_BusinessStatusName.Value = AccountPayableBillStatusEnum.Code.YSC;
            cbAPB_BusinessStatusName.DataBind();

            //审核状态【待审核，已审核】
            _approvalStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.ApprovalStatus);
            cbAPB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbAPB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbAPB_ApprovalStatusName.DataSource = _approvalStatusList;
            cbAPB_ApprovalStatusName.Text = ApprovalStatusEnum.Name.DSH;
            cbAPB_ApprovalStatusName.Value = ApprovalStatusEnum.Code.DSH;
            cbAPB_ApprovalStatusName.DataBind();

            //收款对象
            _bll.CopyModelList(_tempAllClientList, _clientList);
            mcbAPB_ReceiveObjectName.ExtraDisplayMember = "ClientType";
            mcbAPB_ReceiveObjectName.DisplayMember = "ClientName";
            mcbAPB_ReceiveObjectName.ValueMember = "ClientID";
            mcbAPB_ReceiveObjectName.DataSource = _clientList;

            //收款对象类型名称
            _receiveObjectTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.AmountTransObjectType);
            mcbAPB_ReceiveObjectTypeName.DisplayMember = SysConst.EN_TEXT;
            mcbAPB_ReceiveObjectTypeName.ValueMember = SysConst.EN_Code;
            mcbAPB_ReceiveObjectTypeName.DataSource = _receiveObjectTypeList;
            //默认客户类型为汽修商户
            mcbAPB_ReceiveObjectTypeName.SelectedValue = AmountTransObjectTypeEnum.Code.AUTOPARTSSUPPLIER;

            #endregion

            #region 初始化详情tab中的grid

            //初始化历史应付单grid
            _historyAPBillGridDS = new List<AccountPayableBillManagerUIModel>();
            gdHistoryAPBill.DataSource = _historyAPBillGridDS;
            gdHistoryAPBill.DataBind();

            //初始化收付款单grid
            _receiptAndPayBillGridDS = new List<ReceiptAndPayUIModel>();
            gdReceiptAndPayBill.DataSource = _receiptAndPayBillGridDS;
            gdReceiptAndPayBill.DataBind();

            #endregion
        }

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 查询条件初始化
            //应付单号
            txtWhere_APB_No.Clear();
            //来源类型
            cbWhere_APB_SourceTypeName.Items.Clear();
            //来源单号
            txtWhere_APB_SourceBillNo.Clear();
            //业务状态
            mcbWhere_APB_BusinessStatusName.Clear();
            //审核状态
            cbWhere_APB_ApprovalStatusName.Items.Clear();
            //收款对象类型
            //cbWhere_APB_ReceiveObjectTypeName.Items.Clear();
            //收款对象
            mcbWhere_ARB_PayObjectName.Clear();
            //有效
            ckWhere_APB_IsValid.Checked = true;
            ckWhere_APB_IsValid.CheckState = CheckState.Checked;
            //创建时间-开始
            dtWhere_CreatedTimeStart.Value = null;
            //创建时间-终了
            dtWhere_CreatedTimeEnd.Value = null;
            //修改时间-开始
            dtWhere_UpdatedTimeStart.Value = null;
            //修改时间-终了
            dtWhere_UpdatedTimeEnd.Value = null;
            //给 应付单号 设置焦点
            lblWhere_APB_No.Focus();
            #endregion

            #region Grid初始化

            HeadGridDS = new BindingList<AccountPayableBillManagerUIModel>();
            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();
            #endregion

            #region 初始化下拉框

            //来源类型【收货应付（入库单）】
            cbWhere_APB_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_APB_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbWhere_APB_SourceTypeName.DataSource = _accountPayableBillSourceTypeList;
            cbWhere_APB_SourceTypeName.DataBind();

            //业务状态【已生成，执行中，已完成】
            mcbWhere_APB_BusinessStatusName.DisplayMember = SysConst.EN_TEXT;
            mcbWhere_APB_BusinessStatusName.ValueMember = SysConst.EN_Code;
            mcbWhere_APB_BusinessStatusName.DataSource = _accountPayableBillStatusList;
            mcbWhere_APB_BusinessStatusName.SelectedValue = AccountPayableBillStatusEnum.Code.YSC + SysConst.Semicolon_DBC + AccountPayableBillStatusEnum.Code.ZXZ + SysConst.Semicolon_DBC + AccountPayableBillStatusEnum.Code.YDZ;

            //审核状态【待审核，已审核】
            cbWhere_APB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_APB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbWhere_APB_ApprovalStatusName.DataSource = _approvalStatusList;
            cbWhere_APB_ApprovalStatusName.DataBind();

            //条件收款对象
            mcbWhere_ARB_PayObjectName.ExtraDisplayMember = "ClientType";
            mcbWhere_ARB_PayObjectName.DisplayMember = "ClientName";
            mcbWhere_ARB_PayObjectName.ValueMember = "ClientID";
            mcbWhere_ARB_PayObjectName.DataSource = _tempAllClientList;

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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.FM_AccountPayableBill.Code.APB_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.FM_AccountPayableBill.Code.APB_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[HeadDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            HeadDS = HeadGridDS.FirstOrDefault(x => x.APB_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.FM_AccountPayableBill.Code.APB_ID].Value);
            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.APB_ID))
            {
                return;
            }

            //设置来源类型数据源
            cbAPB_SourceTypeName.DataSource = _accountPayableBillSourceTypeList;
            cbAPB_SourceTypeName.DataBind();

            if (txtAPB_ID.Text != HeadDS.APB_ID
                || (txtAPB_ID.Text == HeadDS.APB_ID && txtAPB_VersionNo.Text != HeadDS.APB_VersionNo?.ToString()))
            {
                if (txtAPB_ID.Text == HeadDS.APB_ID && txtAPB_VersionNo.Text != HeadDS.APB_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged())
                {
                    //将放弃之前的修改，是否继续？
                    DialogResult dialogResult = MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.ConfirmGiveUpEdit }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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

            //查询历史应付单Grid数据并绑定
            QueryHistoryAPBill();
            //查询收付款单Grid数据并绑定
            QueryReceiptAndPayBill();

            //设置动作按钮状态
            SetActionEnableByStatus();
            //设置详情页面控件的是否可编辑
            SetDetailControl();
        }

        /// <summary>
        /// 将HeadDS数据赋值给【详情】Tab内的对应控件
        /// </summary>APB_SourceTypeCode
        private void SetDetailDSToCardCtrls()
        {
            //应付单号
            txtAPB_No.Text = HeadDS.APB_No;
            //单据方向
            cbAPB_BillDirectName.Text = HeadDS.APB_BillDirectName;
            cbAPB_BillDirectName.Value = HeadDS.APB_BillDirectCode;
            //来源类型
            cbAPB_SourceTypeName.Text = HeadDS.APB_SourceTypeName;
            cbAPB_SourceTypeName.Value = HeadDS.APB_SourceTypeCode;
            //来源单号
            txtAPB_SourceBillNo.Text = HeadDS.APB_SourceBillNo;
            //组织名称
            txtAPB_Org_Name.Text = HeadDS.APB_Org_Name;
            //收款对象类型
            mcbAPB_ReceiveObjectTypeName.SelectedValue = HeadDS.APB_ReceiveObjectTypeCode;
            //收款对象
            if (!string.IsNullOrEmpty(HeadDS.APB_ReceiveObjectID))
            {
                mcbAPB_ReceiveObjectName.SelectedValue = HeadDS.APB_ReceiveObjectID;
            }
            else
            {
                mcbAPB_ReceiveObjectName.SelectedTextExtra = HeadDS.APB_ReceiveObjectTypeName;
                mcbAPB_ReceiveObjectName.SelectedText = HeadDS.APB_ReceiveObjectName;
            }

            //应付金额
            numAPB_AccountPayableAmount.Value = HeadDS.APB_AccountPayableAmount;
            //已付金额
            numAPB_PaidAmount.Value = HeadDS.APB_PaidAmount;
            //未付金额
            numAPB_UnpaidAmount.Value = HeadDS.APB_UnpaidAmount;
            //业务状态
            cbAPB_BusinessStatusName.Text = HeadDS.APB_BusinessStatusName;
            cbAPB_BusinessStatusName.Value = HeadDS.APB_BusinessStatusCode;
            //审核状态
            cbAPB_ApprovalStatusName.Text = HeadDS.APB_ApprovalStatusName;
            cbAPB_ApprovalStatusName.Value = HeadDS.APB_ApprovalStatusCode;
            //对账时间
            dtAPB_ReconciliationTime.Value = HeadDS.APB_ReconciliationTime;
            //备注
            txtAPB_Remark.Text = HeadDS.APB_Remark;
            //有效
            if (HeadDS.APB_IsValid != null)
            {
                ckAPB_IsValid.Checked = HeadDS.APB_IsValid.Value;
            }
            //创建人
            txtAPB_CreatedBy.Text = HeadDS.APB_CreatedBy;
            //创建时间
            dtAPB_CreatedTime.Value = HeadDS.APB_CreatedTime;
            //修改人
            txtAPB_UpdatedBy.Text = HeadDS.APB_UpdatedBy;
            //修改时间
            dtAPB_UpdatedTime.Value = HeadDS.APB_UpdatedTime;
            //应付单ID
            txtAPB_ID.Text = HeadDS.APB_ID;

            //组织ID
            txtAPB_Org_ID.Text = HeadDS.APB_Org_ID;
            //收款对象ID
            txtAPB_ReceiveObjectID.Text = HeadDS.APB_ReceiveObjectID;
            //版本号
            txtAPB_VersionNo.Value = HeadDS.APB_VersionNo;
        }

        /// <summary>
        /// 查询历史应付单列表
        /// </summary>
        private void QueryHistoryAPBill()
        {
            List<AccountPayableBillManagerUIModel> resultAccountPayableBillList = new List<AccountPayableBillManagerUIModel>();
            //查询应付单来源单据的所有应付单
            _bll.QueryForList(SQLID.FM_AccountPayableBillManager_SQL_01, new MDLFM_AccountPayableBill()
            {
                WHERE_APB_SourceBillNo = txtAPB_SourceBillNo.Text.Trim(),
                WHERE_APB_Org_ID = LoginInfoDAX.OrgID,
                WHERE_APB_IsValid = true
            }, resultAccountPayableBillList);

            //历史应付单列表不包括当前应付单
            _historyAPBillGridDS =
                resultAccountPayableBillList.Where(x => x.APB_No != txtAPB_No.Text.Trim()).ToList();
            //历史应付单Grid
            gdHistoryAPBill.DataSource = _historyAPBillGridDS;
            gdHistoryAPBill.DataBind();
            //设置历史应付单Grid自适应列宽（根据单元格内容）
            gdHistoryAPBill.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }

        /// <summary>
        /// 查询收付款单列表
        /// </summary>
        private void QueryReceiptAndPayBill()
        {
            //查询应收单对应的收付款单
            _bll.QueryForList(SQLID.FM_AccountReceivableBillManager_SQL_02, new ReceiptAndPayQCModel
            {
                Where_BillSourceNo = string.IsNullOrEmpty(txtAPB_SourceBillNo.Text.Trim()) ? txtAPB_No.Text.Trim() : txtAPB_SourceBillNo.Text.Trim(),
                Where_TradeObjectID = txtAPB_ReceiveObjectID.Text.Trim(),
            }, _receiptAndPayBillGridDS);

            //收付款单Grid
            gdReceiptAndPayBill.DataSource = _receiptAndPayBillGridDS;
            gdReceiptAndPayBill.DataBind();
            //设置收付款单Grid自适应列宽（根据单元格内容）
            gdReceiptAndPayBill.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
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
                foreach (UltraGridColumn loopColumn in gdGrid.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (loopColumn.IsGroupByColumn)
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
            //验证来源类型
            if (string.IsNullOrEmpty(cbAPB_SourceTypeName.Text))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.SOURCE_TYPE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //验证付款对象
            if (string.IsNullOrEmpty(mcbAPB_ReceiveObjectName.SelectedText))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.RECEIPTOBJECT_NAME }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //验证应收金额
            if (string.IsNullOrEmpty(numAPB_AccountPayableAmount.Text.Trim()))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.FM_AccountPayableBill.Name.APB_AccountPayableAmount }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                numAPB_AccountPayableAmount.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 前端检查-删除
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForDelete()
        {
            if (string.IsNullOrEmpty(txtAPB_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0006, new object[]
                    { SystemTableEnums.Name.FM_AccountPayableBill,SystemActionEnum.Name.DELETE }),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (cbAPB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                    { SystemTableEnums.Name.FM_AccountPayableBill+ApprovalStatusEnum.Name.YSH,SystemActionEnum.Name.DELETE }),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            DialogResult dialogResult = MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.OK)
            {
                return false;
            }
            return true;

        }

        /// <summary>
        /// 前端检查—审核
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForApprove()
        {
            #region 验证
            //应付单未保存,不能审核
            if (string.IsNullOrEmpty(txtAPB_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.FM_AccountPayableBill + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            MDLFM_AccountPayableBill resultAccountPayableBill = new MDLFM_AccountPayableBill();
            _bll.QueryForObject<MDLFM_AccountPayableBill, MDLFM_AccountPayableBill>(new MDLFM_AccountPayableBill()
            {
                WHERE_APB_IsValid = true,
                WHERE_APB_ID = txtAPB_ID.Text.Trim()
            }, resultAccountPayableBill);
            //应付单不存在,不能审核
            if (string.IsNullOrEmpty(resultAccountPayableBill.APB_ID))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.FM_AccountPayableBill + MsgParam.NOTEXIST, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            #endregion

            //确认审核操作
            DialogResult isConfirmApprove = MessageBoxs.Show(Trans.FM, this.ToString(),
                MsgHelp.GetMsg(MsgCode.W_0014),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isConfirmApprove != DialogResult.OK)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 前端检查—反审核
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForUnApprove()
        {
            #region 前段检查—不涉及数据表

            //应付单未保存,不能反审核
            if (string.IsNullOrEmpty(txtAPB_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.FM_AccountPayableBill + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            MDLFM_AccountPayableBill resultAccountPayableBill = new MDLFM_AccountPayableBill();
            _bll.QueryForObject<MDLFM_AccountPayableBill, MDLFM_AccountPayableBill>(new MDLFM_AccountPayableBill()
            {
                WHERE_APB_IsValid = true,
                WHERE_APB_ID = txtAPB_ID.Text.Trim()
            }, resultAccountPayableBill);
            //应付单不存在,不能反审核
            if (string.IsNullOrEmpty(resultAccountPayableBill.APB_ID))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.FM_AccountPayableBill + MsgParam.NOTEXIST, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //应付单不为手工创建，不能反审核
            if (cbAPB_SourceTypeName.Text != AccountPayableBillSourceTypeEnum.Name.SGCJ)
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.FM_AccountPayableBill + MsgParam.BE+cbAPB_SourceTypeName.Text, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            #endregion

            #region 前段检查-涉及数据表

            #region 检查存不存在对应已审核的付款单
            MDLFM_PayBillDetail payBillDetail = new MDLFM_PayBillDetail();
            List<MDLFM_PayBillDetail> payBillDetailList = new List<MDLFM_PayBillDetail>();
            if (cbAPB_SourceTypeName.Text.Trim() == AccountPayableBillSourceTypeEnum.Name.SGCJ)
            {
                payBillDetail.WHERE_PBD_SrcBillNo = txtAPB_No.Text.Trim();
            }
            else
            {
                payBillDetail.WHERE_PBD_SrcBillNo = txtAPB_SourceBillNo.Text.Trim();
            }
            _bll.QueryForList<MDLFM_PayBillDetail>(payBillDetail, payBillDetailList);
            if (payBillDetailList.Count > 0)
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    MsgParam.ALREADYEXIST + MsgParam.CORRESPONDENCE + SystemTableEnums.Name.FM_PayBill,
                    SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            #endregion

            #endregion
            //确认反审核操作
            DialogResult isConfirmUnApprove = MessageBoxs.Show(Trans.FM, this.ToString(),
                MsgHelp.GetMsg(MsgCode.W_0018),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isConfirmUnApprove != DialogResult.OK)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的HeadDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            HeadDS = new AccountPayableBillManagerUIModel()
            {
                //应付单号
                APB_No = txtAPB_No.Text.Trim(),
                //单据方向
                APB_BillDirectName = cbAPB_BillDirectName.Text.Trim(),
                //来源类型
                APB_SourceTypeName = cbAPB_SourceTypeName.Text.Trim(),
                //来源单号
                APB_SourceBillNo = txtAPB_SourceBillNo.Text.Trim(),
                //组织名称
                APB_Org_Name = txtAPB_Org_Name.Text.Trim(),
                //收款对象类型
                APB_ReceiveObjectTypeName = mcbAPB_ReceiveObjectTypeName.SelectedText,
                //收款对象
                APB_ReceiveObjectName = mcbAPB_ReceiveObjectName.SelectedText,
                //应付金额
                APB_AccountPayableAmount = Convert.ToDecimal(numAPB_AccountPayableAmount.Value ?? 0),
                //已付金额
                APB_PaidAmount = Convert.ToDecimal(numAPB_PaidAmount.Value ?? 0),
                //未付金额
                APB_UnpaidAmount = Convert.ToDecimal(numAPB_UnpaidAmount.Value ?? 0),
                //业务状态
                APB_BusinessStatusName = cbAPB_BusinessStatusName.Text.Trim(),
                //审核状态
                APB_ApprovalStatusName = cbAPB_ApprovalStatusName.Text.Trim(),
                //对账时间
                APB_ReconciliationTime = (DateTime?)dtAPB_ReconciliationTime.Value,
                //备注
                APB_Remark = txtAPB_Remark.Text.Trim(),
                //有效
                APB_IsValid = ckAPB_IsValid.Checked,
                //创建人
                APB_CreatedBy = txtAPB_CreatedBy.Text.Trim(),
                //创建时间
                APB_CreatedTime = (DateTime?)dtAPB_CreatedTime.Value ?? DateTime.Now,
                //修改人
                APB_UpdatedBy = txtAPB_UpdatedBy.Text.Trim(),
                //修改时间
                APB_UpdatedTime = (DateTime?)dtAPB_UpdatedTime.Value ?? DateTime.Now,
                //应付单ID
                APB_ID = txtAPB_ID.Text.Trim(),
                //单据方向编码
                APB_BillDirectCode = cbAPB_BillDirectName.Value?.ToString() ?? "",
                //来源类型
                APB_SourceTypeCode = cbAPB_SourceTypeName.Value?.ToString() ?? "",
                //组织ID
                APB_Org_ID = txtAPB_Org_ID.Text.Trim(),
                //收款对象类型编码
                APB_ReceiveObjectTypeCode = mcbAPB_ReceiveObjectTypeName.SelectedValue,
                //收款对象ID
                APB_ReceiveObjectID = mcbAPB_ReceiveObjectName.SelectedValue,
                //业务状态编码
                APB_BusinessStatusCode = cbAPB_BusinessStatusName.Value?.ToString() ?? "",
                //审核状态编码
                APB_ApprovalStatusCode = cbAPB_ApprovalStatusName.Value?.ToString() ?? "",
                //版本号
                APB_VersionNo = Convert.ToInt64(txtAPB_VersionNo.Text.Trim() == "" ? "1" : txtAPB_VersionNo.Text.Trim()),
            };
        }

        /// <summary>
        /// 设置动作按钮状态
        /// </summary>
        private void SetActionEnableByStatus()
        {
            if (cbAPB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                //[审核状态]为[已审核]的场合，[保存]、[删除]、[审核]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                SetActionEnable(SystemActionEnum.Code.UNAPPROVE, true);
                SetActionEnable(SystemActionEnum.Code.PRINT, true);

                if (cbAPB_BusinessStatusName.Text == AccountPayableBillStatusEnum.Name.ZXZ)
                {
                    //[业务状态]为{执行中}的场合，[对账]可用
                    SetActionEnable(SystemActionEnum.Code.RECONCILIATION, true);
                }
                else
                {
                    //[业务状态]不是{执行中}的场合，[对账]不可用
                    SetActionEnable(SystemActionEnum.Code.RECONCILIATION, false);
                }

                if (cbAPB_BusinessStatusName.Text == AccountPayableBillStatusEnum.Name.YDZ
                    || cbAPB_BusinessStatusName.Text == AccountPayableBillStatusEnum.Name.YWC)
                {
                    //[业务状态]为{已对账}或{已完成}的场合，[反审核]不可用
                    SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
                }
                else
                {
                    //[业务状态]不是{已对账、{已完成}的场合，[反审核]可用
                    SetActionEnable(SystemActionEnum.Code.UNAPPROVE, true);
                }

                if (cbAPB_BusinessStatusName.Text == AccountPayableBillStatusEnum.Name.ZXZ
                    || cbAPB_BusinessStatusName.Text == AccountPayableBillStatusEnum.Name.YDZ)
                {
                    //[业务状态]为{执行中}或{已对账}的场合，导航按钮[转结算]可用
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, true);
                }
                else
                {
                    //[业务状态]不是{执行中}或{已对账}的场合，导航按钮[转结算]不可用
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, false);
                }
            }
            else
            {
                //[审核状态]为[待审核]的场合，[反审核]、[打印]不可用
                //新增的场合，[删除]、[审核]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, true);
                SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtAPB_ID.Text));
                SetActionEnable(SystemActionEnum.Code.APPROVE, !string.IsNullOrEmpty(txtAPB_ID.Text));
                SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
                SetActionEnable(SystemActionEnum.Code.PRINT, false);
                SetActionEnable(SystemActionEnum.Code.RECONCILIATION, false);

                //导航按钮[转结算]不可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, false);
            }
        }

        /// <summary>
        /// 设置详情页面控件的是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            if (cbAPB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                #region 应付单.[审核状态]为[已审核] 的场合，详情不可编辑

                txtAPB_SourceBillNo.Enabled = false;
                txtAPB_Remark.Enabled = false;
                mcbAPB_ReceiveObjectTypeName.Enabled = false;
                mcbAPB_ReceiveObjectName.Enabled = false;
                cbAPB_SourceTypeName.Enabled = false;
                numAPB_AccountPayableAmount.Enabled = false;

                //对账时间
                if (cbAPB_BusinessStatusName.Text == AccountPayableBillStatusEnum.Name.ZXZ)
                {
                    //[业务状态]为{执行中}的场合，[对账时间]可编辑
                    dtAPB_ReconciliationTime.Enabled = true;
                    dtAPB_ReconciliationTime.Value = BLLCom.GetCurStdDatetime();
                }
                else
                {
                    //[业务状态]不是{执行中}的场合，[对账时间]不可编辑
                    dtAPB_ReconciliationTime.Enabled = false;
                }
                #endregion
            }
            else
            {
                #region 应付单未保存 或 应付单.[审核状态]为[待审核]的场合，详情可编辑

                txtAPB_Remark.Enabled = true;
                numAPB_AccountPayableAmount.Enabled = true;
                cbAPB_SourceTypeName.Enabled = true;
                txtAPB_SourceBillNo.Enabled = true;
                mcbAPB_ReceiveObjectTypeName.Enabled = true;
                mcbAPB_ReceiveObjectName.Enabled = true;

                //[对账时间]不可编辑
                dtAPB_ReconciliationTime.Enabled = false;
                dtAPB_ReconciliationTime.Value = null;

                #endregion
            }
        }

        /// <summary>
        /// 设置历史应付Grid单元格样式
        /// </summary>
        private void SetHistoryAPBillStyle()
        {
            #region 设置Grid数据颜色
            gdHistoryAPBill.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;

            #endregion
        }

        /// <summary>
        /// 设置收付款单Grid单元格样式
        /// </summary>
        private void SetReceiptAndPayStyle()
        {
            #region 设置Grid数据颜色
            gdReceiptAndPayBill.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;

            #endregion
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
                    var curHead = HeadGridDS.FirstOrDefault(x => x.APB_ID == HeadDS.APB_ID);
                    if (curHead != null)
                    {
                        HeadGridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = HeadGridDS.FirstOrDefault(x => x.APB_ID == HeadDS.APB_ID);
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
