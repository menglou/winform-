using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.FM;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.FM;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.Common.UIModel;
using Infragistics.Win.UltraWinEditors;
using SkyCar.Coeus.UIModel.FM.APModel;

namespace SkyCar.Coeus.UI.FM
{
    /// <summary>
    /// 应收管理
    /// </summary>
    public partial class FrmAccountReceivableBillManager : BaseFormCardListDetail<AccountReceivableBillManagerUIModel, AccountReceivableBillManagerQCModel, MDLFM_AccountReceivableBill>
    {
        #region 全局变量

        /// <summary>
        /// 应收管理BLL
        /// </summary>
        private AccountReceivableBillManagerBLL _bll = new AccountReceivableBillManagerBLL();

        #region Grid数据源

        /// <summary>
        /// 历史应收单Grid数据源
        /// </summary>
        private List<AccountReceivableBillManagerUIModel> _historyARBillGridDS = new List<AccountReceivableBillManagerUIModel>();
        /// <summary>
        /// 收付款单Grid数据源
        /// </summary>
        private List<ReceiptAndPayUIModel> _receiptAndPayBillGridDS = new List<ReceiptAndPayUIModel>();

        #endregion

        #region 下拉框数据源

        /// <summary>
        /// 应收单来源类型数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _accountReceivableBillSourceTypeDs = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 应收单单据方向数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _billDirectStatusDs = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 应收单业务状态数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _accountReceivableBillStatusDs = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 审核状态数据源
        /// </summary>
        List<ComComboBoxDataSourceTC> _approvalStatusDs = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 付款对象类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _payObjectTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
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
        /// FrmAccountReceivableBillManager构造方法
        /// </summary>
        public FrmAccountReceivableBillManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAccountReceivableBillManager_Load(object sender, EventArgs e)
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
            //设置历史应收单元格样式
            SetHistoryARBillStyle();
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
        /// 【列表】Grid的CellChange事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_CellChange(object sender, CellEventArgs e)
        {
            gdGrid.UpdateData();
            decimal accountReceivableAmount = 0;
            decimal receivedAmount = 0;
            decimal unReceivedAmount = 0;

            foreach (var loopGridRow in gdGrid.Rows)
            {
                if (string.IsNullOrEmpty(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountReceivableBill.Code.ARB_No].Value?.ToString())
                    || loopGridRow.Cells["IsChecked"].Value == null
                    || (bool)loopGridRow.Cells["IsChecked"].Value == false)
                {
                    continue;
                }
                accountReceivableAmount = accountReceivableAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountReceivableBill.Code.ARB_AccountReceivableAmount].Value?.ToString());
                receivedAmount = receivedAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountReceivableBill.Code.ARB_ReceivedAmount].Value?.ToString());
                unReceivedAmount = unReceivedAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountReceivableBill.Code.ARB_UnReceiveAmount].Value?.ToString());
            }

            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtAccountReceivableAmount"])).Text = Convert.ToString(accountReceivableAmount);
            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtReceivedAmount"])).Text = Convert.ToString(receivedAmount);
            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtUnReceiveAmount"])).Text = Convert.ToString(unReceivedAmount);
        }
        /// <summary>
        /// 【列表】Grid的AfterHeaderCheckStateChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            decimal accountReceivableAmount = 0;
            decimal receivedAmount = 0;
            decimal unReceivedAmount = 0;

            foreach (var loopGridRow in gdGrid.Rows)
            {
                if (string.IsNullOrEmpty(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountReceivableBill.Code.ARB_No].Value?.ToString())
                    || loopGridRow.Cells["IsChecked"].Value == null
                    || (bool)loopGridRow.Cells["IsChecked"].Value == false)
                {
                    continue;
                }
                accountReceivableAmount = accountReceivableAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountReceivableBill.Code.ARB_AccountReceivableAmount].Value?.ToString());
                receivedAmount = receivedAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountReceivableBill.Code.ARB_ReceivedAmount].Value?.ToString());
                unReceivedAmount = unReceivedAmount + Convert.ToDecimal(loopGridRow.Cells[SystemTableColumnEnums.FM_AccountReceivableBill.Code.ARB_UnReceiveAmount].Value?.ToString());
            }

            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtAccountReceivableAmount"])).Text = Convert.ToString(accountReceivableAmount);
            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtReceivedAmount"])).Text = Convert.ToString(receivedAmount);
            ((TextBoxTool)(toolBarManagerListTabPaging.Tools["txtUnReceiveAmount"])).Text = Convert.ToString(unReceivedAmount);
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
        /// 应收单号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_ARB_No_KeyDown(object sender, KeyEventArgs e)
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
        private void cbWhere_ARB_SourceTypeName_ValueChanged(object sender, EventArgs e)
        {
            txtWhere_ARB_SrcBillNo.Clear();
            if (cbWhere_ARB_SourceTypeName.Text == AccountReceivableBillSourceTypeEnum.Name.XSYS
                || cbWhere_ARB_SourceTypeName.Text == AccountReceivableBillSourceTypeEnum.Name.QTYS)
            {
                txtWhere_ARB_SrcBillNo.Visible = true;
                lblWhere_ARB_SrcBillNo.Visible = true;
            }
            else if (cbWhere_ARB_SourceTypeName.Text == AccountReceivableBillSourceTypeEnum.Name.SGCJ)
            {
                txtWhere_ARB_SrcBillNo.Visible = false;
                lblWhere_ARB_SrcBillNo.Visible = false;
            }
        }
        /// <summary>
        /// 来源单号EditorButtonClick事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_ARB_SrcBillNo_EditorButtonClick(object sender, EditorButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(cbWhere_ARB_SourceTypeName.Text))
            {
                //请先选择来源类型，再选择来源单号
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0034, new object[] { MsgParam.SOURCE_TYPE, MsgParam.SOURCE_NO }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cbWhere_ARB_SourceTypeName.Text == AccountReceivableBillSourceTypeEnum.Name.XSYS)
            {
                //仅查询[审核状态]为[已审核]的出库单
                var paramApprovetatusList = _approvalStatusDs.Where(x => x.Text == ApprovalStatusEnum.Name.YSH).ToList();
                Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
                {
                    {ComViewParamKey.ApprovalStatus.ToString(), paramApprovetatusList},
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
                if (frmSalesOrderQuery.SelectedGridList.Count == 1)
                {
                    txtWhere_ARB_SrcBillNo.Text = frmSalesOrderQuery.SelectedGridList[0].SO_No;
                }
            }
            else if (cbWhere_ARB_SourceTypeName.Text == AccountReceivableBillSourceTypeEnum.Name.QTYS)
            {
                //根据销售订单.[来源类型]查询相应销售订单
                List<ComComboBoxDataSourceTC> paramSourceTypeList = new List<ComComboBoxDataSourceTC>();

                List<ComComboBoxDataSourceTC> salesOrderSourceTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.SalesOrderSourceType);
                //获取[来源类型]为主动销售、手工创建、在线销售
                paramSourceTypeList = salesOrderSourceTypeList.Where(x => x.Text == SalesOrderSourceTypeEnum.Name.ZDXS || x.Text == SalesOrderSourceTypeEnum.Name.SGCJ || x.Text == SalesOrderSourceTypeEnum.Name.ZXXS).ToList();
                //仅查询[审核状态]为[已审核]的销售订单
                var paramApprovetatusList = _approvalStatusDs.Where(x => x.Text == ApprovalStatusEnum.Name.YSH).ToList();

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
                if (frmSalesOrderQuery.SelectedGridList.Count == 1)
                {
                    txtWhere_ARB_SrcBillNo.Text = frmSalesOrderQuery.SelectedGridList[0].SO_No;
                }
            }

        }
        /// <summary>
        /// 来源单号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_ARB_SrcBillNo_KeyDown(object sender, KeyEventArgs e)
        {
            //执行查询
            //QueryAction();
        }

        /// <summary>
        /// 审核状态ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_ARB_ApprovalStatusName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 有效CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckWhere_ARB_IsValid_CheckedChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
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
        /// [修改时间-终了]ValueChanged事件
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

        #region 单头相关事件

        /// <summary>
        /// [来源类型]ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbARB_SourceTypeName_ValueChanged(object sender, EventArgs e)
        {
            if (cbARB_SourceTypeName.Value == null)
            {
                return;
            }

            txtARB_SrcBillNo.Clear();

            if (cbARB_SourceTypeName.Text == AccountReceivableBillSourceTypeEnum.Name.XSYS
                || cbARB_SourceTypeName.Text == AccountReceivableBillSourceTypeEnum.Name.QTYS)
            {
                #region 来源类型为[销售应收]、[其他应收（赔偿）]的场合

                if (cbARB_SourceTypeName.Text == AccountReceivableBillSourceTypeEnum.Name.XSYS)
                {
                    //来源类型为[销售应收]的场合，显示【历史应收单】Tab
                    tabControlDetail.Tabs["HistoryARList"].Visible = true;
                }
                else
                {
                    //来源类型为[其他应收（赔偿）]的场合，隐藏【历史应收单】Tab
                    tabControlDetail.Tabs["HistoryARList"].Visible = false;
                }
                #endregion
            }
            else if (cbARB_SourceTypeName.Text == AccountReceivableBillSourceTypeEnum.Name.SGCJ)
            {
                #region 来源类型为[手工创建]的场合

                //隐藏【历史应收单】Tab
                tabControlDetail.Tabs["HistoryARList"].Visible = false;

                #endregion
            }
        }

        /// <summary>
        /// [付款对象类型]改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbARB_PayObjectTypeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curClientType = mcbARB_PayObjectName.SelectedTextExtra;
            if (curClientType != mcbARB_PayObjectTypeName.SelectedText)
            {
                _clientList.Clear();
                if (string.IsNullOrEmpty(mcbARB_PayObjectTypeName.SelectedText))
                {
                    _bll.CopyModelList(_tempAllClientList, _clientList);
                }
                else
                {
                    foreach (var loopTempClient in _tempAllClientList)
                    {
                        if (loopTempClient.ClientType == mcbARB_PayObjectTypeName.SelectedText)
                        {
                            _clientList.Add(loopTempClient);
                        }
                    }
                }
                mcbARB_PayObjectName.DataSource = _clientList;
                mcbARB_PayObjectName.Clear();
            }
            else if (string.IsNullOrEmpty(mcbARB_PayObjectTypeName.SelectedText))
            {
                _bll.CopyModelList(_tempAllClientList, _clientList);
                mcbARB_PayObjectName.DataSource = _clientList;
                mcbARB_PayObjectName.Clear();
            }
        }

        /// <summary>
        /// [付款对象]SelectedIndexChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbARB_PayObjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //根据选择的对象选中对象类型
            if (!string.IsNullOrEmpty(mcbARB_PayObjectName.SelectedTextExtra))
            {
                mcbARB_PayObjectTypeName.SelectedText = mcbARB_PayObjectName.SelectedTextExtra;
            }
            else if (!string.IsNullOrEmpty(mcbARB_PayObjectName.SelectedText))
            {
                mcbARB_PayObjectTypeName.SelectedText = "其他";
            }
        }

        /// <summary>
        /// [应收金额]KeyUp事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numARB_AccountReceivableAmount_KeyUp(object sender, KeyEventArgs e)
        {
            decimal receivedAmount =
                 Convert.ToDecimal(numARB_ReceivedAmount.Value ?? 0);
            decimal accountReceivableAmount =
                Convert.ToDecimal(numARB_AccountReceivableAmount.Value ?? 0);
            numARB_UnReceiveAmount.Text = Convert.ToString(accountReceivableAmount - receivedAmount);
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

            //控制详情是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();

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

            //2.将【详情】Tab内控件的值赋值给基类的HeadDS
            SetCardCtrlsToDetailDS();
            //3.执行保存（含服务端检查）
            bool saveDetailResult = _bll.SaveDetailDS(HeadDS);
            if (!saveDetailResult)
            {
                //保存失败
                MessageBoxs.Show(Trans.FM, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //4.将HeadDS数据赋值给【详情】Tab内的对应控件
            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //控制详情是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();
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
            var argsHead = HeadDS.ToTBModelForSaveAndDelete<MDLFM_AccountReceivableBill>();

            //2.执行删除
            bool deleteResult = _bll.Delete(argsHead);
            if (!deleteResult)
            {
                //删除失败
                MessageBoxs.Show(Trans.FM, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //删除成功
            MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.DELETE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //3.清空【详情】画面数据
            InitializeDetailTabControls();
            //刷新列表
            RefreshList(true);

            //4.将HeadDS数据赋值给【详情】Tab内的对应控件
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
            ConditionDS = new AccountReceivableBillManagerQCModel()
            {
                //查询用SqlId
                SqlId = SQLID.FM_AccountReceivableBillManager_SQL_01,
                //应收单号
                WHERE_ARB_No = txtWhere_ARB_No.Text.Trim(),
                //来源类型
                WHERE_ARB_SourceTypeName = cbWhere_ARB_SourceTypeName.Text.Trim(),
                //来源单号
                WHERE_ARB_SrcBillNo = txtWhere_ARB_SrcBillNo.Text.Trim(),
                //审核状态
                WHERE_ARB_ApprovalStatusName = cbWhere_ARB_ApprovalStatusName.Text.Trim(),
                //有效
                WHERE_ARB_IsValid = ckWhere_ARB_IsValid.Checked,
                ////付款对象类型
                WHERE_ARB_PayObjectTypeName = mcbWhere_ARB_PayObjectName.SelectedTextExtra,
                //付款对象名称
                WHERE_ARB_PayObjectName = mcbWhere_ARB_PayObjectName.SelectedText,
                //汽修商户编码
                WHERE_AutoFactoryCode = txtWhere_AutoFactoryCode.Text.Trim(),
                //组织
                WHERE_ARB_Org_ID = LoginInfoDAX.OrgID,

            };
            //业务状态
            if (!string.IsNullOrEmpty(mcbWhere_ARB_BusinessStatusName.SelectedText))
            {
                string businessStatusName = string.Empty;
                businessStatusName = SysConst.Semicolon_DBC;
                businessStatusName = businessStatusName + mcbWhere_ARB_BusinessStatusName.SelectedText.Trim();
                ConditionDS.WHERE_ARB_BusinessStatusName = businessStatusName;
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
            paramGridName = SystemTableEnums.Name.FM_AccountReceivableBill;
            base.ExportAction(gdGrid, paramGridName);
        }

        /// <summary>
        /// 导出全部
        /// </summary>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.FM_AccountReceivableBill;
            List<AccountReceivableBillManagerUIModel> resultAllList = new List<AccountReceivableBillManagerUIModel>();
            _bll.QueryForList(SQLID.FM_AccountReceivableBillManager_SQL_01, new AccountReceivableBillManagerQCModel()
            {
                PageIndex = 1,
                PageSize = null,
                //应收单号
                WHERE_ARB_No = txtWhere_ARB_No.Text.Trim(),
                //来源类型
                WHERE_ARB_SourceTypeName = cbWhere_ARB_SourceTypeName.Text.Trim(),
                //来源单号
                WHERE_ARB_SrcBillNo = txtWhere_ARB_SrcBillNo.Text.Trim(),
                //业务状态
                WHERE_ARB_BusinessStatusName = mcbWhere_ARB_BusinessStatusName.SelectedText.Trim(),
                //审核状态
                WHERE_ARB_ApprovalStatusName = cbWhere_ARB_ApprovalStatusName.Text.Trim(),
                //有效
                WHERE_ARB_IsValid = ckWhere_ARB_IsValid.Checked,
                //付款对象类型
                WHERE_ARB_PayObjectTypeName = mcbWhere_ARB_PayObjectName.SelectedTextExtra,
                //付款对象名称
                WHERE_ARB_PayObjectName = mcbWhere_ARB_PayObjectName.SelectedText,
                //汽修商户编码
                WHERE_AutoFactoryCode = txtWhere_AutoFactoryCode.Text.Trim(),
                //组织
                WHERE_ARB_Org_ID = LoginInfoDAX.OrgID,
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

            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //控制详情是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();
        }

        /// <summary>
        /// 反审核
        /// </summary>
        public override void UnApproveAction()
        {
            //1.前端检查-审核
            if (!ClientCheckForUnApprove())
            {
                return;
            }

            //确认反审核操作
            DialogResult isConfirmUnApprove = MessageBoxs.Show(Trans.FM, this.ToString(),
                MsgHelp.GetMsg(MsgCode.W_0018),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isConfirmUnApprove != DialogResult.OK)
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

            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //控制详情是否可编辑
            SetDetailControl();
            //设置动作按钮状态
            SetActionEnableByStatus();
        }

        /// <summary>
        /// 对账
        /// </summary>
        public override void ReconciliationAction()
        {
            //待对账的应收单列表
            List<AccountReceivableBillManagerUIModel> accountReceivableBillToReconciliation = new List<AccountReceivableBillManagerUIModel>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 详情对账

                if (string.IsNullOrEmpty(HeadDS.ARB_ID)
                    || string.IsNullOrEmpty(HeadDS.ARB_No))
                {
                    //应收单信息为空，不能对账
                    MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.W_0016, new object[] { SystemTableEnums.Name.FM_AccountReceivableBill, SystemActionEnum.Name.RECONCILIATION }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                accountReceivableBillToReconciliation.Add(HeadDS);
                #endregion
            }
            else
            {
                #region 列表对账

                gdGrid.UpdateData();
                var checkedAccountReceivableBillList = HeadGridDS.Where(x => x.IsChecked == true).ToList();
                if (checkedAccountReceivableBillList.Count == 0)
                {
                    //请选择执行中的应收单对账！
                    MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { AccountReceivableBillStatusEnum.Name.ZXZ + MsgParam.OF + SystemTableEnums.Name.FM_AccountReceivableBill + SystemActionEnum.Name.RECONCILIATION }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //[业务状态]为{执行中}的场合，[对账]可用，否则不可用
                var notZXZBill =
                    checkedAccountReceivableBillList.Where(
                        x => x.ARB_BusinessStatusName != AccountReceivableBillStatusEnum.Name.ZXZ).ToList();
                if (notZXZBill.Count > 0)
                {
                    //请选择执行中的应收单对账！
                    MessageBoxs.Show(Trans.FM, ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { AccountReceivableBillStatusEnum.Name.ZXZ + MsgParam.OF + SystemTableEnums.Name.FM_AccountReceivableBill + SystemActionEnum.Name.RECONCILIATION }), MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                accountReceivableBillToReconciliation.AddRange(checkedAccountReceivableBillList);
                #endregion
            }

            //对账时间
            if (dtARB_ReconciliationTime.Value == null)
            {
                dtARB_ReconciliationTime.Value = BLLCom.GetCurStdDatetime();
            }
            SetCardCtrlsToDetailDS();

            bool saveReconciliation = _bll.ReconciliationDetailDS(accountReceivableBillToReconciliation);
            if (!saveReconciliation)
            {
                //对账失败
                MessageBoxs.Show(Trans.FM, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //对账成功
            MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.RECONCILIATION }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected && accountReceivableBillToReconciliation.Count == 1)
            {
                _bll.CopyModel(accountReceivableBillToReconciliation[0], HeadDS);
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
            if (HeadDS == null || string.IsNullOrWhiteSpace(HeadDS.ARB_No))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, SystemTableEnums.Name.FM_AccountReceivableBill), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (HeadDS.ARB_ApprovalStatusName != ApprovalStatusEnum.Name.YSH)
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, SystemTableEnums.Name.FM_AccountReceivableBill + cbARB_ApprovalStatusName.Text, SystemActionEnum.Name.PRINT), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                AccountReceivableBillUIModelToPrint AccountReceivableBillToPrint = new AccountReceivableBillUIModelToPrint();
                _bll.CopyModel(HeadDS, AccountReceivableBillToPrint);
                Dictionary<string, object> argsViewParams = new Dictionary<string, object>
                    {

                    {FMViewParamKey.AccountReceivableBill.ToString(), AccountReceivableBillToPrint},

                    };

                FrmViewAndPrintAccountReceivableBill frmViewAndPrintAccountReceivableBill = new FrmViewAndPrintAccountReceivableBill
                {
                    ViewParameters = argsViewParams
                };
                frmViewAndPrintAccountReceivableBill.ViewParameters = argsViewParams;
                frmViewAndPrintAccountReceivableBill.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), ex.Message.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        #region 导航事件

        /// <summary>
        /// 转结算
        /// </summary>
        public override void ToSettlementNavigate()
        {
            base.ToSettlementNavigate();

            //选中的待收款的[应收单]列表
            List<AccountReceivableBillManagerUIModel> selectedAccountReceivableBillList = new List<AccountReceivableBillManagerUIModel>();

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 当前选中Tab为【详情】
                if (HeadDS == null || string.IsNullOrEmpty(HeadDS.ARB_No))
                {
                    //请选择一个执行中或已对账的应收单转收款
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + AccountReceivableBillStatusEnum.Name.ZXZ + MsgParam.OR + AccountReceivableBillStatusEnum.Name.YDZ + MsgParam.OF + SystemTableEnums.Name.FM_AccountReceivableBill + SystemNavigateEnum.Name.TORECEIPTBILL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (HeadDS.ARB_BusinessStatusName != AccountReceivableBillStatusEnum.Name.ZXZ
                    && HeadDS.ARB_BusinessStatusName != AccountReceivableBillStatusEnum.Name.YDZ)
                {
                    //请选择一个执行中或已对账的应收单转收款
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ONE + AccountReceivableBillStatusEnum.Name.ZXZ + MsgParam.OR + AccountReceivableBillStatusEnum.Name.YDZ + MsgParam.OF + SystemTableEnums.Name.FM_AccountReceivableBill + SystemNavigateEnum.Name.TORECEIPTBILL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (HeadDS.ARB_ApprovalStatusName != ApprovalStatusEnum.Name.YSH)
                {
                    //请选择已审核的应收单转收款
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { ApprovalStatusEnum.Name.YSH + MsgParam.AND + AccountReceivableBillStatusEnum.Name.ZXZ + MsgParam.OF + SystemTableEnums.Name.FM_AccountReceivableBill + SystemNavigateEnum.Name.TORECEIPTBILL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                selectedAccountReceivableBillList.Add(HeadDS);

                #endregion
            }
            else
            {
                #region 当前选中Tab为【列表】

                var checkedGrid = HeadGridDS.Where(x => x.IsChecked == true).ToList();

                if (checkedGrid.Count == 0)
                {
                    //请至少勾选一条执行中或已对账的应收单信息进行转收款
                    MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[] { AccountReceivableBillStatusEnum.Name.ZXZ + MsgParam.OR + AccountReceivableBillStatusEnum.Name.YDZ + MsgParam.OF + SystemTableEnums.Name.FM_AccountReceivableBill, SystemNavigateEnum.Name.TORECEIPTBILL }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var notCheckedZXZOrYDZ = checkedGrid.Where(x => x.ARB_BusinessStatusName != AccountReceivableBillStatusEnum.Name.ZXZ && x.ARB_BusinessStatusName != AccountReceivableBillStatusEnum.Name.YDZ).ToList();
                if (notCheckedZXZOrYDZ.Count > 0)
                {
                    //请选择执行中或已对账的应收单转收款
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { AccountReceivableBillStatusEnum.Name.ZXZ + MsgParam.OR + AccountReceivableBillStatusEnum.Name.YDZ + MsgParam.OF + SystemTableEnums.Name.FM_AccountReceivableBill + SystemNavigateEnum.Name.TORECEIPTBILL }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var loopCannotToReceipt in notCheckedZXZOrYDZ)
                    {
                        loopCannotToReceipt.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }
                var notCheckedYSH = checkedGrid.Where(x => x.ARB_ApprovalStatusName != ApprovalStatusEnum.Name.YSH).ToList();
                if (notCheckedYSH.Count > 0)
                {
                    //请选择已审核的应收单转收款
                    MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { ApprovalStatusEnum.Name.YSH + MsgParam.OF + SystemTableEnums.Name.FM_AccountReceivableBill + SystemNavigateEnum.Name.TORECEIPTBILL }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var loopCannotToReceipt in notCheckedYSH)
                    {
                        loopCannotToReceipt.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }
                var firstCheckedItem = checkedGrid.FirstOrDefault(x => x.ARB_ApprovalStatusName == ApprovalStatusEnum.Name.YSH
                                                                && (x.ARB_BusinessStatusName == AccountPayableBillStatusEnum.Name.ZXZ || x.ARB_BusinessStatusName == AccountPayableBillStatusEnum.Name.YDZ));
                if (firstCheckedItem == null)
                {
                    //请至少勾选一条已审核并且执行中或已对账的应付单信息进行转结算
                    MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0017, new object[]
                        { ApprovalStatusEnum.Name.YSH + MsgParam.AND + AccountPayableBillStatusEnum.Name.ZXZ+MsgParam.OR+AccountPayableBillStatusEnum.Name.YDZ + MsgParam.OF + SystemTableEnums.Name.FM_AccountPayableBill, SystemNavigateEnum.Name.TOSETTLEMENT }),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var differFirstCheckedItem = checkedGrid.Where(x => x.ARB_PayObjectName != firstCheckedItem.ARB_PayObjectName
                || x.ARB_PayObjectID != firstCheckedItem.ARB_PayObjectID).ToList();
                if (differFirstCheckedItem.Count > 0)
                {
                    //请选择相同来源类型，相同客户的应付单转结算
                    MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.SAME_ORGANIZATIONANDCUSTOMER + SystemNavigateEnum.Name.TOSETTLEMENT }), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    foreach (var loopCheckedItem in differFirstCheckedItem)
                    {
                        loopCheckedItem.IsChecked = false;
                    }
                    gdGrid.DataSource = HeadGridDS;
                    gdGrid.DataBind();
                    return;
                }

                _bll.CopyModelList(checkedGrid, selectedAccountReceivableBillList);

                #endregion
            }

            //未付金额
            decimal UnpaidAmount = 0;
            //未收金额
            decimal UnReceiveAmount = 0;
            //循环遍历list，计算出未付金额，未收金额
            foreach (var loopAccountPayableBill in selectedAccountReceivableBillList)
            {
                if (loopAccountPayableBill.ARB_BillDirectName == BillDirectionEnum.Name.MINUS)
                {
                    UnpaidAmount = UnpaidAmount - (loopAccountPayableBill.ARB_UnReceiveAmount ?? 0);
                }
                else if (loopAccountPayableBill.ARB_BillDirectName == BillDirectionEnum.Name.PLUS)
                {
                    UnReceiveAmount = UnReceiveAmount + (loopAccountPayableBill.ARB_UnReceiveAmount ?? 0);
                }
            }

            #region 判断未付金额，未收金额二者是谁大，未付金额大转付款，未收金额大转收款单

            if (UnpaidAmount > UnReceiveAmount)
            {
                #region 转付款

                //待确认付款的应付单列表
                List<PayablePayConfirmUIModel> accountPayableBillToPayList = new List<PayablePayConfirmUIModel>();
                foreach (var loopAccountPayableBill in selectedAccountReceivableBillList)
                {
                    PayablePayConfirmUIModel accountPayableBillToPay = new PayablePayConfirmUIModel
                    {
                        IsBusinessSourceAccountPayableBill = false,
                        BusinessID = loopAccountPayableBill.ARB_ID,
                        BusinessNo = loopAccountPayableBill.ARB_No,
                        BusinessOrgID = loopAccountPayableBill.ARB_Org_ID,
                        BusinessOrgName = loopAccountPayableBill.ARB_Org_Name,
                        BusinessSourceTypeName = loopAccountPayableBill.ARB_PayObjectTypeName,
                        BusinessSourceTypeCode = loopAccountPayableBill.ARB_SourceTypeCode,
                        ReceiveObjectTypeCode = loopAccountPayableBill.ARB_PayObjectTypeCode,
                        ReceiveObjectTypeName = loopAccountPayableBill.ARB_PayObjectTypeName,
                        ReceiveObjectID = loopAccountPayableBill.ARB_PayObjectID,
                        ReceiveObjectName = loopAccountPayableBill.ARB_PayObjectName,
                        Wal_No = loopAccountPayableBill.Wal_No,
                        Wal_AvailableBalance = loopAccountPayableBill.Wal_AvailableBalance,
                        //应收单相关
                        APB_ID = loopAccountPayableBill.ARB_ID,
                        APB_No = loopAccountPayableBill.ARB_No,
                        APB_BillDirectCode = loopAccountPayableBill.ARB_BillDirectCode,
                        APB_BillDirectName = loopAccountPayableBill.ARB_BillDirectName,
                        APB_SourceTypeCode = loopAccountPayableBill.ARB_SourceTypeCode,
                        APB_SourceTypeName = loopAccountPayableBill.ARB_SourceTypeName,
                        APB_SourceBillNo = loopAccountPayableBill.ARB_SrcBillNo,
                        APB_Org_ID = loopAccountPayableBill.ARB_Org_ID,
                        APB_Org_Name = loopAccountPayableBill.ARB_Org_Name,
                        APB_ReceiveObjectTypeCode = loopAccountPayableBill.ARB_PayObjectTypeCode,
                        APB_ReceiveObjectTypeName = loopAccountPayableBill.ARB_PayObjectTypeName,
                        APB_ReceiveObjectID = loopAccountPayableBill.ARB_PayObjectID,
                        APB_ReceiveObjectName = loopAccountPayableBill.ARB_PayObjectName,
                        APB_AccountPayableAmount = loopAccountPayableBill.ARB_AccountReceivableAmount,
                        APB_PaidAmount = loopAccountPayableBill.ARB_ReceivedAmount,
                        APB_UnpaidAmount = loopAccountPayableBill.ARB_UnReceiveAmount,
                        APB_BusinessStatusCode = loopAccountPayableBill.ARB_BusinessStatusCode,
                        APB_BusinessStatusName = loopAccountPayableBill.ARB_BusinessStatusName,
                        APB_ApprovalStatusCode = loopAccountPayableBill.ARB_ApprovalStatusCode,
                        APB_ApprovalStatusName = loopAccountPayableBill.ARB_ApprovalStatusName,
                        APB_CreatedBy = loopAccountPayableBill.ARB_CreatedBy,
                        APB_CreatedTime = loopAccountPayableBill.ARB_CreatedTime,
                        APB_VersionNo = loopAccountPayableBill.ARB_VersionNo,
                    };

                    //if (loopAccountPayableBill.ARB_BillDirectName == BillDirectionEnum.Name.MINUS)
                    //{
                    //accountPayableBillToPay.PayableTotalAmount = loopAccountPayableBill.ARB_AccountReceivableAmount;
                    //accountPayableBillToPay.PayTotalAmount = loopAccountPayableBill.ARB_ReceivedAmount;
                    //accountPayableBillToPay.UnPayTotalAmount = loopAccountPayableBill.ARB_UnReceiveAmount;
                    //}
                    //else
                    //{
                    accountPayableBillToPay.PayableTotalAmount = -loopAccountPayableBill.ARB_AccountReceivableAmount;
                    accountPayableBillToPay.PayTotalAmount = -loopAccountPayableBill.ARB_ReceivedAmount;
                    accountPayableBillToPay.UnPayTotalAmount = -loopAccountPayableBill.ARB_UnReceiveAmount;
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
                foreach (var loopAccountPayableBill in selectedAccountReceivableBillList)
                {
                    ReceiveableCollectionConfirmUIModel accountPayableToReceipt = new ReceiveableCollectionConfirmUIModel
                    {
                        IsBusinessSourceAccountPayableBill = false,
                        BusinessID = loopAccountPayableBill.ARB_ID,
                        BusinessNo = loopAccountPayableBill.ARB_No,
                        BusinessOrgID = loopAccountPayableBill.ARB_Org_ID,
                        BusinessOrgName = loopAccountPayableBill.ARB_Org_Name,
                        BusinessSourceTypeName = loopAccountPayableBill.ARB_SourceTypeName,
                        BusinessSourceTypeCode = loopAccountPayableBill.ARB_SourceTypeCode,
                        PayObjectTypeCode = loopAccountPayableBill.ARB_PayObjectTypeCode,
                        PayObjectTypeName = loopAccountPayableBill.ARB_PayObjectTypeName,
                        PayObjectName = loopAccountPayableBill.ARB_PayObjectName,
                        PayObjectID = loopAccountPayableBill.ARB_PayObjectID,
                        Wal_No = loopAccountPayableBill.Wal_No,
                        Wal_AvailableBalance = loopAccountPayableBill.Wal_AvailableBalance,

                        //应收单相关
                        ARB_ID = loopAccountPayableBill.ARB_ID,
                        ARB_No = loopAccountPayableBill.ARB_No,
                        ARB_BillDirectCode = loopAccountPayableBill.ARB_BillDirectCode,
                        ARB_BillDirectName = loopAccountPayableBill.ARB_BillDirectName,
                        ARB_SourceTypeCode = loopAccountPayableBill.ARB_SourceTypeCode,
                        ARB_SourceTypeName = loopAccountPayableBill.ARB_SourceTypeName,
                        ARB_SrcBillNo = loopAccountPayableBill.ARB_SrcBillNo,
                        ARB_Org_ID = loopAccountPayableBill.ARB_Org_ID,
                        ARB_Org_Name = loopAccountPayableBill.ARB_Org_Name,
                        ARB_PayObjectTypeCode = loopAccountPayableBill.ARB_PayObjectTypeCode,
                        ARB_PayObjectTypeName = loopAccountPayableBill.ARB_PayObjectTypeName,
                        ARB_PayObjectName = loopAccountPayableBill.ARB_PayObjectName,
                        ARB_PayObjectID = loopAccountPayableBill.ARB_PayObjectID,
                        ARB_AccountReceivableAmount = loopAccountPayableBill.ARB_AccountReceivableAmount,
                        ARB_ReceivedAmount = loopAccountPayableBill.ARB_ReceivedAmount,
                        ARB_UnReceiveAmount = loopAccountPayableBill.ARB_UnReceiveAmount,
                        ARB_BusinessStatusCode = loopAccountPayableBill.ARB_BusinessStatusCode,
                        ARB_BusinessStatusName = loopAccountPayableBill.ARB_BusinessStatusName,
                        ARB_ApprovalStatusCode = loopAccountPayableBill.ARB_ApprovalStatusCode,
                        ARB_ApprovalStatusName = loopAccountPayableBill.ARB_ApprovalStatusName,
                        ARB_CreatedBy = loopAccountPayableBill.ARB_CreatedBy,
                        ARB_CreatedTime = loopAccountPayableBill.ARB_CreatedTime,
                        ARB_VersionNo = loopAccountPayableBill.ARB_VersionNo,
                    };
                    //if (loopAccountPayableBill.ARB_BillDirectName == BillDirectionEnum.Name.MINUS)
                    //{
                    //    accountPayableToReceipt.ReceivableTotalAmount = -loopAccountPayableBill.ARB_AccountReceivableAmount;
                    //    accountPayableToReceipt.ReceiveTotalAmount = -loopAccountPayableBill.ARB_ReceivedAmount;
                    //    accountPayableToReceipt.UnReceiveTotalAmount = -loopAccountPayableBill.ARB_UnReceiveAmount;
                    //}
                    //else
                    //{
                    accountPayableToReceipt.ReceivableTotalAmount = loopAccountPayableBill.ARB_AccountReceivableAmount;
                    accountPayableToReceipt.ReceiveTotalAmount = loopAccountPayableBill.ARB_ReceivedAmount;
                    accountPayableToReceipt.UnReceiveTotalAmount = loopAccountPayableBill.ARB_UnReceiveAmount;
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

        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //应收单号
            txtARB_No.Clear();
            //单据方向
            cbARB_BillDirectName.Items.Clear();
            //来源类型
            cbARB_SourceTypeName.Items.Clear();
            //来源单号
            txtARB_SrcBillNo.Clear();
            //付款对象类型名称
            mcbARB_PayObjectTypeName.Clear();
            //付款对象
            mcbARB_PayObjectName.Clear();
            //应收金额
            numARB_AccountReceivableAmount.Value = null;
            //已收金额
            numARB_ReceivedAmount.Value = null;
            //未收金额
            numARB_UnReceiveAmount.Value = null;
            //业务状态
            cbARB_BusinessStatusName.Items.Clear();
            //审核状态
            cbARB_ApprovalStatusName.Items.Clear();
            //对账时间
            dtARB_ReconciliationTime.Value = null;
            //备注
            txtARB_Remark.Clear();
            //有效
            ckARB_IsValid.Checked = true;
            ckARB_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtARB_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtARB_CreatedTime.Value = DateTime.Now;
            //修改人
            txtARB_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtARB_UpdatedTime.Value = DateTime.Now;
            //应收单ID
            txtARB_ID.Clear();
            //组织ID
            txtARB_Org_ID.Clear();
            //版本号
            txtARB_VersionNo.Clear();
            //付款对象组织
            //txtSOD_StockInOrgName.Clear();
            //钱包账号
            txtWal_No.Clear();
            //钱包余额
            txtWal_AvailableBalance.Clear();
            //给 应收单号 设置焦点
            lblARB_No.Focus();

            #endregion

            #region 初始化下拉框
            //来源类型
            _accountReceivableBillSourceTypeDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.AccountReceivableBillSourceType);
            var tempSourceTypeDs = _accountReceivableBillSourceTypeDs.Where(x => x.Text == AccountReceivableBillSourceTypeEnum.Name.SGCJ).ToList();
            cbARB_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbARB_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbARB_SourceTypeName.DataSource = tempSourceTypeDs;
            cbARB_SourceTypeName.DataBind();
            //默认来源类型为[手工创建]
            cbARB_SourceTypeName.Text = AccountReceivableBillSourceTypeEnum.Name.SGCJ;
            cbARB_SourceTypeName.Value = AccountReceivableBillSourceTypeEnum.Code.SGCJ;

            //单据方向
            _billDirectStatusDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.BillDirection);
            cbARB_BillDirectName.DisplayMember = SysConst.EN_TEXT;
            cbARB_BillDirectName.ValueMember = SysConst.EN_Code;
            cbARB_BillDirectName.DataSource = _billDirectStatusDs;
            cbARB_BillDirectName.DataBind();
            //默认单据方向为[正向]
            cbARB_BillDirectName.Text = BillDirectionEnum.Name.PLUS;
            cbARB_BillDirectName.Value = BillDirectionEnum.Code.PLUS;

            //业务状态
            _accountReceivableBillStatusDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.AccountReceivableBillStatus);
            cbARB_BusinessStatusName.DisplayMember = SysConst.EN_TEXT;
            cbARB_BusinessStatusName.ValueMember = SysConst.EN_Code;
            cbARB_BusinessStatusName.DataSource = _accountReceivableBillStatusDs;
            cbARB_BusinessStatusName.DataBind();
            //默认业务状态为[已生成]
            cbARB_BusinessStatusName.Text = AccountReceivableBillStatusEnum.Name.YSC;
            cbARB_BusinessStatusName.Value = AccountReceivableBillStatusEnum.Code.YSC;

            //审核状态
            _approvalStatusDs = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.ApprovalStatus);
            cbARB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbARB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbARB_ApprovalStatusName.DataSource = _approvalStatusDs;
            cbARB_ApprovalStatusName.DataBind();
            //默认审核状态为[待审核]
            cbARB_ApprovalStatusName.Text = ApprovalStatusEnum.Name.DSH;
            cbARB_ApprovalStatusName.Value = ApprovalStatusEnum.Code.DSH;

            //付款对象
            _bll.CopyModelList(_tempAllClientList, _clientList);
            mcbARB_PayObjectName.ExtraDisplayMember = "ClientType";
            mcbARB_PayObjectName.DisplayMember = "ClientName";
            mcbARB_PayObjectName.ValueMember = "ClientID";
            mcbARB_PayObjectName.DataSource = _clientList;

            //付款对象类型
            _payObjectTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.AmountTransObjectType);
            mcbARB_PayObjectTypeName.DisplayMember = SysConst.EN_TEXT;
            mcbARB_PayObjectTypeName.ValueMember = SysConst.EN_Code;
            mcbARB_PayObjectTypeName.DataSource = _payObjectTypeList;

            //默认客户类型为汽修商户
            mcbARB_PayObjectTypeName.SelectedValue = AmountTransObjectTypeEnum.Code.PLATFORMAUTOFACTORY;

            #endregion

            #region 初始化详情tab中的grid

            //初始化历史应收单
            _historyARBillGridDS = new List<AccountReceivableBillManagerUIModel>();
            gdHistoryARBill.DataSource = _historyARBillGridDS;
            gdHistoryARBill.DataBind();
            //初始化收付款单
            _receiptAndPayBillGridDS = new List<ReceiptAndPayUIModel>();
            gdReceiptAndPayBill.DataSource = _receiptAndPayBillGridDS;
            gdReceiptAndPayBill.DataBind();

            #endregion

            //默认组织为当前组织
            txtARB_Org_ID.Text = LoginInfoDAX.OrgID;
            txtARB_Org_Name.Text = LoginInfoDAX.OrgShortName;
        }
        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //应收单号
            txtWhere_ARB_No.Clear();
            //来源类型
            cbWhere_ARB_SourceTypeName.Items.Clear();
            //来源单号
            txtWhere_ARB_SrcBillNo.Clear();
            //业务状态
            mcbWhere_ARB_BusinessStatusName.Clear();
            //审核状态
            cbWhere_ARB_ApprovalStatusName.Items.Clear();
            //付款对象类型
            //cbWhere_ARB_PayObjectTypeName.Items.Clear();
            //付款对象
            mcbWhere_ARB_PayObjectName.Clear();
            //汽修商户编码
            txtWhere_AutoFactoryCode.Clear();
            //创建时间-开始
            dtWhere_CreatedTimeStart.Value = null;
            //创建时间-终了
            dtWhere_CreatedTimeEnd.Value = null;
            //修改时间-开始
            dtWhere_UpdatedTimeStart.Value = null;
            //修改时间-终了
            dtWhere_UpdatedTimeEnd.Value = null;
            //有效
            ckWhere_ARB_IsValid.Checked = true;
            ckWhere_ARB_IsValid.CheckState = CheckState.Checked;
            //给 应收单号 设置焦点
            lblWhere_ARB_No.Focus();
            #endregion

            #region Grid初始化

            //清空Grid
            HeadGridDS = new BindingList<AccountReceivableBillManagerUIModel>();
            gdGrid.DataSource = HeadGridDS;
            gdGrid.DataBind();

            #endregion

            #endregion

            #region 初始化下拉框
            //来源类型
            cbWhere_ARB_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_ARB_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbWhere_ARB_SourceTypeName.DataSource = _accountReceivableBillSourceTypeDs;
            cbWhere_ARB_SourceTypeName.DataBind();

            //业务状态
            mcbWhere_ARB_BusinessStatusName.DisplayMember = SysConst.EN_TEXT;
            mcbWhere_ARB_BusinessStatusName.ValueMember = SysConst.EN_Code;
            mcbWhere_ARB_BusinessStatusName.DataSource = _accountReceivableBillStatusDs;
            mcbWhere_ARB_BusinessStatusName.SelectedValue = AccountReceivableBillStatusEnum.Code.YSC + SysConst.Semicolon_DBC + AccountReceivableBillStatusEnum.Code.ZXZ + SysConst.Semicolon_DBC + AccountReceivableBillStatusEnum.Code.YDZ;

            //审核状态
            cbWhere_ARB_ApprovalStatusName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_ARB_ApprovalStatusName.ValueMember = SysConst.EN_Code;
            cbWhere_ARB_ApprovalStatusName.DataSource = _approvalStatusDs;
            cbWhere_ARB_ApprovalStatusName.DataBind();

            //条件付款对象
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
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.FM_AccountReceivableBill.Code.ARB_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.FM_AccountReceivableBill.Code.ARB_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[HeadDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            HeadDS = HeadGridDS.FirstOrDefault(x => x.ARB_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.FM_AccountReceivableBill.Code.ARB_ID].Value);
            if (HeadDS == null || string.IsNullOrEmpty(HeadDS.ARB_ID))
            {
                return;
            }

            //设置来源类型数据源
            cbARB_SourceTypeName.DataSource = _accountReceivableBillSourceTypeDs;
            cbARB_SourceTypeName.DataBind();

            if (txtARB_ID.Text != HeadDS.ARB_ID
                || (txtARB_ID.Text == HeadDS.ARB_ID && txtARB_VersionNo.Text != HeadDS.ARB_VersionNo?.ToString()))
            {
                if (txtARB_ID.Text == HeadDS.ARB_ID && txtARB_VersionNo.Text != HeadDS.ARB_VersionNo?.ToString())
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

            //控制详情是否可编辑
            SetDetailControl();

            //查询历史应收单Grid数据并绑定
            QueryHistoryARBill();
            //查询收付款单Grid数据并绑定
            QueryReceiptAndPayBill();

            //设置动作按钮状态
            SetActionEnableByStatus();
        }

        /// <summary>
        /// 将HeadDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //应收单号
            txtARB_No.Text = HeadDS.ARB_No;
            //单据方向
            cbARB_BillDirectName.Text = HeadDS.ARB_BillDirectName ?? "";
            cbARB_BillDirectName.Value = HeadDS.ARB_BillDirectCode;
            //来源类型
            cbARB_SourceTypeName.Text = HeadDS.ARB_SourceTypeName ?? "";
            cbARB_SourceTypeName.Value = HeadDS.ARB_SourceTypeCode;
            //来源单号
            txtARB_SrcBillNo.Text = HeadDS.ARB_SrcBillNo;
            //付款对象类型名称
            mcbARB_PayObjectTypeName.SelectedValue = HeadDS.ARB_PayObjectTypeCode;
            //付款对象名称
            if (!string.IsNullOrEmpty(HeadDS.ARB_PayObjectID))
            {
                mcbARB_PayObjectName.SelectedValue = HeadDS.ARB_PayObjectID;
            }
            else
            {
                mcbARB_PayObjectName.SelectedTextExtra = HeadDS.ARB_PayObjectTypeName;
                mcbARB_PayObjectName.SelectedText = HeadDS.ARB_PayObjectName;
            }

            //应收金额
            numARB_AccountReceivableAmount.Value = HeadDS.ARB_AccountReceivableAmount;
            //已收金额
            numARB_ReceivedAmount.Value = HeadDS.ARB_ReceivedAmount;
            //未收金额
            numARB_UnReceiveAmount.Value = HeadDS.ARB_UnReceiveAmount;
            //业务状态
            cbARB_BusinessStatusName.Text = HeadDS.ARB_BusinessStatusName ?? "";
            cbARB_BusinessStatusName.Value = HeadDS.ARB_BusinessStatusCode;
            //审核状态
            cbARB_ApprovalStatusName.Text = HeadDS.ARB_ApprovalStatusName ?? "";
            cbARB_ApprovalStatusName.Value = HeadDS.ARB_ApprovalStatusCode;
            //对账时间
            dtARB_ReconciliationTime.Value = HeadDS.ARB_ReconciliationTime;
            //备注
            txtARB_Remark.Text = HeadDS.ARB_Remark;
            //有效
            if (HeadDS.ARB_IsValid != null)
            {
                ckARB_IsValid.Checked = HeadDS.ARB_IsValid.Value;
            }
            //创建人
            txtARB_CreatedBy.Text = HeadDS.ARB_CreatedBy;
            //创建时间
            dtARB_CreatedTime.Value = HeadDS.ARB_CreatedTime;
            //修改人
            txtARB_UpdatedBy.Text = HeadDS.ARB_UpdatedBy;
            //修改时间
            dtARB_UpdatedTime.Value = HeadDS.ARB_UpdatedTime;
            //应收单ID
            txtARB_ID.Text = HeadDS.ARB_ID;
            //组织ID
            txtARB_Org_ID.Text = HeadDS.ARB_Org_ID;
            //版本号
            txtARB_VersionNo.Value = HeadDS.ARB_VersionNo;
            //付款对象组织
            //txtSOD_StockInOrgName.Text = HeadDS.SOD_StockInOrgName;
            //钱包账号
            txtWal_No.Text = HeadDS.Wal_No;
            //钱包余额
            txtWal_AvailableBalance.Text = Convert.ToString(HeadDS.Wal_AvailableBalance);
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
            if (string.IsNullOrEmpty(cbARB_SourceTypeName.Text))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.SOURCE_TYPE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //来源类型为[销售应收]、[其他应收（赔偿）]的场合，验证来源单号
            if ((cbARB_SourceTypeName.Text == AccountReceivableBillSourceTypeEnum.Name.XSYS
                || cbARB_SourceTypeName.Text == AccountReceivableBillSourceTypeEnum.Name.QTYS)
                && string.IsNullOrEmpty(txtARB_SrcBillNo.Text))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { SystemTableColumnEnums.FM_AccountReceivableBill.Name.ARB_SrcBillNo }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //验证单据方向
            if (string.IsNullOrEmpty(cbARB_BillDirectName.Text))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.BILL_DIRECTION }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //验证付款对象
            if (string.IsNullOrEmpty(mcbARB_PayObjectName.SelectedText))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.FM_AccountReceivableBill.Name.ARB_PayObjectName }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                numARB_AccountReceivableAmount.Focus();
                return false;
            }
            //验证应收金额
            if (string.IsNullOrEmpty(numARB_AccountReceivableAmount.Text.Trim()))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { SystemTableColumnEnums.FM_AccountReceivableBill.Name.ARB_AccountReceivableAmount }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                numARB_AccountReceivableAmount.Focus();
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
            //确认删除操作
            DialogResult dialogResult = MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0012), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.OK)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 前端检查-审核
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForApprove()
        {
            #region 验证

            //应收单未保存,不能审核
            if (string.IsNullOrEmpty(txtARB_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.FM_AccountReceivableBill + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            MDLFM_AccountReceivableBill resultAccountReceivableBill = new MDLFM_AccountReceivableBill();
            _bll.QueryForObject<MDLFM_AccountReceivableBill, MDLFM_AccountReceivableBill>(new MDLFM_AccountReceivableBill()
            {
                WHERE_ARB_IsValid = true,
                WHERE_ARB_ID = txtARB_ID.Text.Trim()
            }, resultAccountReceivableBill);
            //应收单不存在,不能审核
            if (string.IsNullOrEmpty(resultAccountReceivableBill.ARB_ID))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.FM_AccountReceivableBill + MsgParam.NOTEXIST, SystemActionEnum.Name.APPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //确认审核操作
            DialogResult isConfirmApprove = MessageBoxs.Show(Trans.FM, this.ToString(),
                MsgHelp.GetMsg(MsgCode.W_0014),
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (isConfirmApprove != DialogResult.OK)
            {
                return false;
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 前端检查-反审核
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForUnApprove()
        {
            #region 前段检查-不涉及数据库

            //应收单未保存,不能反审核
            if (string.IsNullOrEmpty(txtARB_ID.Text.Trim()))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.FM_AccountReceivableBill + MsgParam.NOTYET + SystemActionEnum.Name.SAVE, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //应收单不为手工创建，不能反审核
            if (cbARB_SourceTypeName.Text.Trim() != AccountReceivableBillSourceTypeEnum.Name.SGCJ)
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.FM_AccountReceivableBill +MsgParam.BE+cbARB_SourceTypeName.Text, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            #endregion

            #region 前段检查-涉及数据库

            MDLFM_AccountReceivableBill resultAccountReceivableBill = new MDLFM_AccountReceivableBill();
            _bll.QueryForObject<MDLFM_AccountReceivableBill, MDLFM_AccountReceivableBill>(new MDLFM_AccountReceivableBill()
            {
                WHERE_ARB_IsValid = true,
                WHERE_ARB_ID = txtARB_ID.Text.Trim()
            }, resultAccountReceivableBill);
            //应收单不存在,不能反审核
            if (string.IsNullOrEmpty(resultAccountReceivableBill.ARB_ID))
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
                {
                    SystemTableEnums.Name.FM_AccountReceivableBill + MsgParam.NOTEXIST, SystemActionEnum.Name.UNAPPROVE
                }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //已存在对应的收款单，不能反审核
            List<MDLFM_ReceiptBillDetail> receiptBillList = new List<MDLFM_ReceiptBillDetail>();
            _bll.QueryForList<MDLFM_ReceiptBillDetail>(new MDLFM_ReceiptBillDetail()
            {
                WHERE_RBD_SrcBillNo = txtARB_No.Text.Trim(),
            }, receiptBillList);
            if (receiptBillList.Count > 0)
            {
                MessageBoxs.Show(Trans.FM, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[]
               {
                   MsgParam.ALREADYEXIST+MsgParam.CORRESPONDENCE+SystemTableEnums.Name.FM_ReceiptBill,SystemActionEnum.Name.UNAPPROVE
               }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的HeadDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            HeadDS = new AccountReceivableBillManagerUIModel()
            {
                //应收单号
                ARB_No = txtARB_No.Text.Trim(),
                //单据方向名称
                ARB_BillDirectName = cbARB_BillDirectName.Text.Trim(),
                //单据方向编码
                ARB_BillDirectCode = cbARB_BillDirectName.Value?.ToString() ?? "",
                //来源类型名称
                ARB_SourceTypeName = cbARB_SourceTypeName.Text.Trim(),
                //来源类型编码
                ARB_SourceTypeCode = cbARB_SourceTypeName.Value?.ToString() ?? "",
                //来源单号
                ARB_SrcBillNo = txtARB_SrcBillNo.Text.Trim(),
                //组织名称
                ARB_Org_Name = txtARB_Org_Name.Text.Trim(),
                //付款对象类型名称
                ARB_PayObjectTypeName = mcbARB_PayObjectTypeName.SelectedText,
                //付款对象类型编码
                ARB_PayObjectTypeCode = mcbARB_PayObjectTypeName.SelectedValue,
                //付款对象ID
                ARB_PayObjectID = mcbARB_PayObjectName.SelectedValue,
                //付款对象名称
                ARB_PayObjectName = mcbARB_PayObjectName.SelectedText,
                //应收金额
                ARB_AccountReceivableAmount = Convert.ToDecimal(numARB_AccountReceivableAmount.Value ?? 0),
                //已收金额
                ARB_ReceivedAmount = Convert.ToDecimal(numARB_ReceivedAmount.Value ?? 0),
                //未收金额
                ARB_UnReceiveAmount = Convert.ToDecimal(numARB_UnReceiveAmount.Value ?? 0),
                //业务状态名称
                ARB_BusinessStatusName = cbARB_BusinessStatusName.Text.Trim(),
                //业务状态编码
                ARB_BusinessStatusCode = cbARB_BusinessStatusName.Value?.ToString() ?? "",
                //审核状态名称
                ARB_ApprovalStatusName = cbARB_ApprovalStatusName.Text.Trim(),
                //审核状态编码
                ARB_ApprovalStatusCode = cbARB_ApprovalStatusName.Value?.ToString() ?? "",
                //对账时间
                ARB_ReconciliationTime = (DateTime?)dtARB_ReconciliationTime.Value,
                //备注
                ARB_Remark = txtARB_Remark.Text.Trim(),
                //有效
                ARB_IsValid = ckARB_IsValid.Checked,
                //创建人
                ARB_CreatedBy = txtARB_CreatedBy.Text.Trim(),
                //创建时间
                ARB_CreatedTime = (DateTime?)dtARB_CreatedTime.Value ?? DateTime.Now,
                //修改人
                ARB_UpdatedBy = txtARB_UpdatedBy.Text.Trim(),
                //修改时间
                ARB_UpdatedTime = (DateTime?)dtARB_UpdatedTime.Value ?? DateTime.Now,
                //应收单ID
                ARB_ID = txtARB_ID.Text.Trim(),
                //组织ID
                ARB_Org_ID = txtARB_Org_ID.Text.Trim(),
                //版本号
                ARB_VersionNo = Convert.ToInt64(txtARB_VersionNo.Text.Trim() == "" ? "1" : txtARB_VersionNo.Text.Trim()),
                //付款对象组织
                //SOD_StockInOrgName = txtSOD_StockInOrgName.Text.Trim(),
                //钱包账号
                Wal_No = txtWal_No.Text.Trim(),
                //钱包余额
                Wal_AvailableBalance = Convert.ToDecimal(txtWal_AvailableBalance.Text == "" ? "0" : txtWal_AvailableBalance.Text.Trim()),
            };
        }

        /// <summary>
        /// 设置详情页面控件的是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            if (cbARB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                #region 应收单.[审核状态]为[已审核] 的场合，详情不可编辑

                //单头
                cbARB_SourceTypeName.Enabled = false;
                txtARB_SrcBillNo.Enabled = false;
                txtARB_Remark.Enabled = false;
                mcbARB_PayObjectTypeName.Enabled = false;
                mcbARB_PayObjectName.Enabled = false;
                numARB_AccountReceivableAmount.Enabled = false;

                //对账时间
                if (cbARB_BusinessStatusName.Text == AccountReceivableBillStatusEnum.Name.ZXZ)
                {
                    //[业务状态]为{执行中}的场合，[对账时间]可编辑
                    dtARB_ReconciliationTime.Enabled = true;
                    dtARB_ReconciliationTime.Value = BLLCom.GetCurStdDatetime();
                }
                else
                {
                    //[业务状态]不是{执行中}的场合，[对账时间]不可编辑
                    dtARB_ReconciliationTime.Enabled = false;
                }
                #endregion

                if (cbARB_SourceTypeName.Text == AccountReceivableBillSourceTypeEnum.Name.SGCJ
                    && cbARB_BusinessStatusName.Text != AccountReceivableBillStatusEnum.Name.YWC)
                {
                    //来源类型为[手工创建] 且 [业务状态]不等于{已完成}的场合，不显示来源单号
                    lblARB_SrcBillNo.Visible = false;
                    txtARB_SrcBillNo.Visible = false;
                }
                else
                {
                    //来源类型为[销售应收]、[其他应收（赔偿）]的场合，显示来源单号
                    lblARB_SrcBillNo.Visible = true;
                    txtARB_SrcBillNo.Visible = true;
                }
            }
            else
            {
                #region 应收单未保存 或 应收单.[审核状态]为[待审核]的场合，详情可编辑

                //单头
                cbARB_SourceTypeName.Enabled = true;
                txtARB_SrcBillNo.Enabled = true;
                mcbARB_PayObjectTypeName.Enabled = true;
                mcbARB_PayObjectName.Enabled = true;
                txtARB_Remark.Enabled = true;
                numARB_AccountReceivableAmount.Enabled = true;

                //[对账时间]不可编辑
                dtARB_ReconciliationTime.Enabled = false;
                dtARB_ReconciliationTime.Value = null;

                if (cbARB_SourceTypeName.Text == AccountReceivableBillSourceTypeEnum.Name.XSYS
                    || cbARB_SourceTypeName.Text == AccountReceivableBillSourceTypeEnum.Name.QTYS)
                {
                    #region 来源类型为[销售应收]、[其他应收（赔偿）]的场合
                    //显示来源单号
                    lblARB_SrcBillNo.Visible = true;
                    txtARB_SrcBillNo.Visible = true;

                    #endregion
                }
                else if (cbARB_SourceTypeName.Text == AccountReceivableBillSourceTypeEnum.Name.SGCJ)
                {
                    #region 来源类型为[手工创建]的场合
                    //不显示来源单号
                    lblARB_SrcBillNo.Visible = false;
                    txtARB_SrcBillNo.Visible = false;

                    #endregion
                }

                #endregion
            }
        }

        /// <summary>
        /// 设置动作按钮状态
        /// </summary>
        private void SetActionEnableByStatus()
        {
            if (cbARB_ApprovalStatusName.Text == ApprovalStatusEnum.Name.YSH)
            {
                //[审核状态]为[已审核]的场合，[保存]、[删除]、[审核]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, false);
                SetActionEnable(SystemActionEnum.Code.DELETE, false);
                SetActionEnable(SystemActionEnum.Code.APPROVE, false);
                SetActionEnable(SystemActionEnum.Code.PRINT, true);

                if (cbARB_BusinessStatusName.Text == AccountReceivableBillStatusEnum.Name.ZXZ)
                {
                    //[业务状态]为{执行中}的场合，[对账]可用
                    SetActionEnable(SystemActionEnum.Code.RECONCILIATION, true);
                }
                else
                {
                    //[业务状态]不是{执行中}的场合，[对账]不可用
                    SetActionEnable(SystemActionEnum.Code.RECONCILIATION, false);
                }

                if (cbARB_BusinessStatusName.Text == AccountReceivableBillStatusEnum.Name.YDZ
                    || cbARB_BusinessStatusName.Text == AccountReceivableBillStatusEnum.Name.YWC)
                {
                    //[业务状态]为{已对账}或{已完成}的场合，[反审核]不可用
                    SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
                }
                else
                {
                    //[业务状态]不是{已对账}、{已完成}的场合，[反审核]可用
                    SetActionEnable(SystemActionEnum.Code.UNAPPROVE, true);
                }
                if (cbARB_BusinessStatusName.Text == AccountReceivableBillStatusEnum.Name.ZXZ
                    || cbARB_BusinessStatusName.Text == AccountReceivableBillStatusEnum.Name.YDZ)
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
                //[审核状态]为[待审核]的场合，[反审核]、[打印]、[对账]不可用
                //新增的场合，[删除]、[审核]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, true);
                SetActionEnable(SystemActionEnum.Code.DELETE, !string.IsNullOrEmpty(txtARB_ID.Text));
                SetActionEnable(SystemActionEnum.Code.APPROVE, !string.IsNullOrEmpty(txtARB_ID.Text));
                SetActionEnable(SystemActionEnum.Code.UNAPPROVE, false);
                SetActionEnable(SystemActionEnum.Code.PRINT, false);
                SetActionEnable(SystemActionEnum.Code.RECONCILIATION, false);

                //导航按钮[转结算]不可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.TOSETTLEMENT, true, false);
            }
        }

        /// <summary>
        /// 查询历史应收单列表
        /// </summary>
        private void QueryHistoryARBill()
        {
            List<AccountReceivableBillManagerUIModel> resultAccountReceivableBillList = new List<AccountReceivableBillManagerUIModel>();
            //查询应收单来源单据的所有应收单
            _bll.QueryForList(SQLID.FM_AccountReceivableBillManager_SQL_01, new AccountReceivableBillManagerQCModel()
            {
                WHERE_ARB_SrcBillNo = txtARB_SrcBillNo.Text.Trim(),
                WHERE_ARB_IsValid = true,
            }, resultAccountReceivableBillList);

            //历史应收单列表不包括当前应收单
            _historyARBillGridDS = resultAccountReceivableBillList.Where(x => x.ARB_No != txtARB_No.Text.Trim()).ToList();
            //历史应收单Grid
            gdHistoryARBill.DataSource = _historyARBillGridDS;
            gdHistoryARBill.DataBind();
            //设置历史应收单Grid自适应列宽（根据单元格内容）
            gdHistoryARBill.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }

        /// <summary>
        /// 查询收付款单列表
        /// </summary>
        private void QueryReceiptAndPayBill()
        {
            //查询应收单对应的收付款单
            _bll.QueryForList(SQLID.FM_AccountReceivableBillManager_SQL_02, new ReceiptAndPayQCModel
            {
                Where_BillSourceNo = string.IsNullOrEmpty(txtARB_SrcBillNo.Text.Trim()) ? txtARB_No.Text.Trim() : txtARB_SrcBillNo.Text.Trim(),
                Where_TradeObjectID = mcbARB_PayObjectName.SelectedValue,
            }, _receiptAndPayBillGridDS);

            //收付款单Grid
            gdReceiptAndPayBill.DataSource = _receiptAndPayBillGridDS;
            gdReceiptAndPayBill.DataBind();
            //设置收付款单Grid自适应列宽（根据单元格内容）
            gdReceiptAndPayBill.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }

        /// <summary>
        /// 设置历史应收单Grid单元格样式
        /// </summary>
        private void SetHistoryARBillStyle()
        {
            #region 设置Grid数据颜色
            gdHistoryARBill.DisplayLayout.Override.CellAppearance.BackColor = CustomEnums.CellBackColor.NoEditableColor;

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
                    var curHead = HeadGridDS.FirstOrDefault(x => x.ARB_ID == HeadDS.ARB_ID);
                    if (curHead != null)
                    {
                        HeadGridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = HeadGridDS.FirstOrDefault(x => x.ARB_ID == HeadDS.ARB_ID);
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
