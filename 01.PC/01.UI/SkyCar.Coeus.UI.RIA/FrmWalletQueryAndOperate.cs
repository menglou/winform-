using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.RIA;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.Common.APModel;
using SkyCar.Coeus.UIModel.RIA.QCModel;
using SkyCar.Coeus.UIModel.RIA.UIModel;

namespace SkyCar.Coeus.UI.RIA
{
    /// <summary>
    /// 钱包查询及操作
    /// </summary>
    public partial class FrmWalletQueryAndOperate : BaseFormCardList<WalletQueryAndOperateUIModel, WalletQueryAndOperateQCModel, MDLEWM_Wallet>
    {
        #region 全局变量

        /// <summary>
        /// 钱包查询及操作BLL
        /// </summary>
        private WalletQueryAndOperateBLL _bll = new WalletQueryAndOperateBLL();

        #region 下拉框数据源

        /// <summary>
        /// 组织
        /// </summary>
        List<MDLSM_Organization> _orgList = new List<MDLSM_Organization>();
        /// <summary>
        /// 所有人类别
        /// </summary>
        List<ComComboBoxDataSourceTC> _ownerTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 钱包状态
        /// </summary>
        List<ComComboBoxDataSourceTC> _walletStatusList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 来源类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _walletSourceTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 汽修商户
        /// </summary>
        List<MDLPIS_AutoFactoryCustomer> _autoFactoryList = new List<MDLPIS_AutoFactoryCustomer>();
        /// <summary>
        /// 用户数据源
        /// </summary>
        List<MDLSM_User> _autoUserList = new List<MDLSM_User>();
        #endregion

        /// <summary>
        /// 界面属性值发生变化时不予检查的属性列表
        /// </summary>
        List<string> _skipPropertyList = new List<string>();

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmWalletQueryAndOperate构造方法
        /// </summary>
        public FrmWalletQueryAndOperate()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmWalletQueryAndOperate_Load(object sender, EventArgs e)
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

            //初始化【详情】Tab内控件
            InitializeDetailTabControls();
            //初始化【列表】Tab内控件
            InitializeListTabControls();
            //根据选中的Tab，设置动作按钮[是否可用]（在系统权限的基础上进行控制）
            base.SetActionEnableBySelectedTab(SysConst.EN_LIST);
            #endregion

            //导航按钮[充值]可用
            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.DEPOSITMONEY, true, true);

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            AcceptUIModelChanges();

            #region 界面发生变化时不予检查的属性值

            _skipPropertyList.Add("CustomerPhoneNo");
            _skipPropertyList.Add("AutoFactoryName");
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
            if (tabControlFull.Tabs[SysConst.EN_LIST].Selected)
            {
                #region 选中【列表】Tab的场合

                //[充值]按钮可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.DEPOSITMONEY, true, true);

                #endregion
            }
            else
            {
                #region 选中【详情】Tab的场合

                if (string.IsNullOrEmpty(txtWal_ID.Text)
                    || string.IsNullOrEmpty(txtWal_No.Text))
                {
                    //未保存的场合，[充值]按钮不可用
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.DEPOSITMONEY, true, false);
                }
                else
                {
                    //已保存的场合，[充值]按钮可用
                    SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.DEPOSITMONEY, true, true);
                }
                #endregion
            }
        }
        #endregion

        #region 查询条件相关事件

        /// <summary>
        /// 钱包账号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_Wal_No_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //   QueryAction();
            //}
        }
        /// <summary>
        /// 开户人KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_Wal_CustomerName_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 开户时间ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_Wal_CreatedTimeStart_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 列表查询条件dtWhere_Wal_CreatedTimeEnd_ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_Wal_CreatedTimeEnd_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtWhere_Wal_CreatedTimeEnd.Value != null &&
                this.dtWhere_Wal_CreatedTimeEnd.DateTime.Hour == 0 &&
                this.dtWhere_Wal_CreatedTimeEnd.DateTime.Minute == 0 &&
                this.dtWhere_Wal_CreatedTimeEnd.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_Wal_CreatedTimeEnd.DateTime.Year, this.dtWhere_Wal_CreatedTimeEnd.DateTime.Month, this.dtWhere_Wal_CreatedTimeEnd.DateTime.Day, 23, 59, 59);
                this.dtWhere_Wal_CreatedTimeEnd.DateTime = newDateTime;
            }
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
            base.NewUIModel = DetailDS;
            if (ViewHasChanged(_skipPropertyList))
            {
                //信息尚未保存，确定进行当前操作？
                DialogResult dialogResult = MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.W_0001), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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
            if (!_bll.SaveDetailDS(DetailDS))
            {
                MessageBoxs.Show(Trans.RIA, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //刷新列表
            RefreshList();

            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();
        }
        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //1.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
            //2.设置查询条件（翻页相关属性不用设置）
            ConditionDS = new WalletQueryAndOperateQCModel()
            {
                //SqlId
                SqlId = SQLID.RIA_WalletQueryAndOperate_SQL01,
                //钱包账号
                WHERE_Wal_No = txtWhere_Wal_No.Text.Trim(),
                //所有人类别
                WHERE_Wal_OwnerTypeName = cbWhere_Wal_OwnerTypeName.Text.Trim(),
                //开户人
                WHERE_Wal_CustomerName = txtWhere_Wal_CustomerName.Text.Trim(),
                //汽修商户名称
                WHERE_AutoFactoryName = mcbWhere_AutoFactoryName.SelectedText,
                //推荐员工
                WHERE_Wal_RecommendEmployee = mcbWhere_Wal_RecommendEmployee.SelectedText,
                //手机号
                WHERE_CustomerPhoneNo = txtWhere_CustomerPhoneNo.Text.Trim(),
                //组织
                WHERE_Wal_Org_ID = cbWhere_Wal_Org_ID.Value?.ToString(),
            };
            if (dtWhere_Wal_CreatedTimeStart.Value != null)
            {
                //开户时间-开始
                ConditionDS._CreatedTimeStart = dtWhere_Wal_CreatedTimeStart.DateTime;
            }
            if (dtWhere_Wal_CreatedTimeEnd.Value != null)
            {
                //开户时间-终了
                ConditionDS._CreatedTimeEnd = dtWhere_Wal_CreatedTimeEnd.DateTime;
            }
            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = GridDS;
            gdGrid.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
            //6.设置【列表】Tab为选中状态
            tabControlFull.Tabs[SysConst.EN_LIST].Selected = true;
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
            base.NewUIModel = DetailDS;
            if (ViewHasChanged(_skipPropertyList))
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
            paramGridName = SystemTableEnums.Name.EWM_Wallet;
            base.ExportAction(gdGrid, paramGridName);
        }
        /// <summary>
        /// 导出全部
        /// </summary>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = SystemTableEnums.Name.EWM_Wallet;
            List<WalletQueryAndOperateUIModel> resultAllList = new List<WalletQueryAndOperateUIModel>();
            _bll.QueryForList(SQLID.RIA_WalletQueryAndOperate_SQL01, new WalletQueryAndOperateQCModel()
            {
                PageIndex = 1,
                PageSize = null,
                //钱包账号
                WHERE_Wal_No = txtWhere_Wal_No.Text.Trim(),
                //所有人类别
                WHERE_Wal_OwnerTypeName = cbWhere_Wal_OwnerTypeName.Text.Trim(),
                //开户人
                WHERE_Wal_CustomerName = txtWhere_Wal_CustomerName.Text.Trim(),
                //汽修商组织名称
                WHERE_AutoFactoryName = mcbWhere_AutoFactoryName.SelectedText,
                //推荐员工
                WHERE_Wal_RecommendEmployee = mcbWhere_Wal_RecommendEmployee.SelectedText,
                //手机号
                WHERE_CustomerPhoneNo = txtWhere_CustomerPhoneNo.Text.Trim(),
                //组织
                WHERE_Wal_Org_ID = cbWhere_Wal_Org_ID.Value?.ToString(),
            }, resultAllList);
            UltraGrid allGrid = gdGrid;
            allGrid.DataSource = resultAllList;
            allGrid.DataBind();

            base.ExportAllAction(allGrid, paramGridName);

            gdGrid.DataSource = GridDS;
            gdGrid.DataBind();
        }

        /// <summary>
        /// 提现
        /// </summary>
        public override void WithdrawCashAction()
        {
            //当前钱包
            MDLEWM_Wallet curWallet = new MDLEWM_Wallet();
            string walletNo = string.Empty;

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 详情提现

                if (string.IsNullOrEmpty(DetailDS.Wal_ID)
                    || string.IsNullOrEmpty(DetailDS.Wal_No))
                {
                    //没有获取到钱包，提现失败
                    MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.EWM_Wallet, SystemActionEnum.Name.WITHDRAWCASH }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                #endregion

                walletNo = DetailDS.Wal_No;
                _bll.CopyModel(DetailDS, curWallet);
            }
            else
            {
                #region 列表提现

                gdGrid.UpdateData();

                var checkedWallet = GridDS.Where(x => x.IsChecked == true).ToList();
                if (checkedWallet.Count != 1)
                {
                    //请勾选一个钱包提现！
                    MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0000, new object[] { "请勾选一个钱包提现！" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (string.IsNullOrEmpty(checkedWallet[0].Wal_ID)
                    || string.IsNullOrEmpty(checkedWallet[0].Wal_No))
                {
                    //没有获取到钱包，提现失败
                    MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.EWM_Wallet, SystemActionEnum.Name.WITHDRAWCASH }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                walletNo = checkedWallet[0].Wal_No;
                _bll.CopyModel(checkedWallet[0], curWallet);

                #endregion
            }

            string argsResultMessage = string.Empty;
            string argsNewWalletNo = string.Empty;
            bool validateResult = BLLCom.ValidateWallet(WalTransTypeEnum.Name.TX, 0, string.Empty,
                walletNo, string.Empty, string.Empty, string.Empty, ref argsResultMessage, ref argsNewWalletNo, ref curWallet);
            if (!validateResult)
            {
                MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0000, new object[] { argsResultMessage }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            WalletWithdrawalUIModel withdrawalWallet = new WalletWithdrawalUIModel();
            _bll.CopyModel(curWallet, withdrawalWallet);

            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //操作参数Key
                {ComViewParamKey.EnumKeyOperation.ToString(), Operation.Show},
                //新打开界面的Model
                { ComViewParamKey.DestModel.ToString(), withdrawalWallet}
            };

            FrmWalletWithdrawalDialogWindow frmWalletWithdrawalDialogWindow = new FrmWalletWithdrawalDialogWindow(paramViewParameters)
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = frmWalletWithdrawalDialogWindow.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                _bll.CopyModel(withdrawalWallet, DetailDS);
                //刷新列表
                RefreshList();
            }
            else
            {
                var curHead = GridDS.FirstOrDefault(x => x.Wal_ID == withdrawalWallet.Wal_ID);
                if (curHead != null)
                {
                    _bll.CopyModel(withdrawalWallet, curHead);
                }
            }

            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
        }

        /// <summary>
        /// 销户
        /// </summary>
        public override void CloseAccountAction()
        {
            //当前钱包
            WalletQueryAndOperateUIModel curWallet = new WalletQueryAndOperateUIModel();

            #region 验证以及准备数据

            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                #region 详情销户

                if (string.IsNullOrEmpty(DetailDS.Wal_ID)
                    || string.IsNullOrEmpty(DetailDS.Wal_No))
                {
                    //没有获取到钱包，销户失败
                    MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.EWM_Wallet, SystemActionEnum.Name.CLOSEACCOUNT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                #endregion

                curWallet = DetailDS;
            }
            else
            {
                #region 列表销户

                gdGrid.UpdateData();

                var checkedWallet = GridDS.Where(x => x.IsChecked == true).ToList();
                if (checkedWallet.Count != 1)
                {
                    //请勾选一个钱包销户！
                    MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0000, new object[] { "请勾选一个钱包销户！" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (string.IsNullOrEmpty(checkedWallet[0].Wal_ID)
                    || string.IsNullOrEmpty(checkedWallet[0].Wal_No))
                {
                    //没有获取到钱包，销户失败
                    MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.EWM_Wallet, SystemActionEnum.Name.CLOSEACCOUNT }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                curWallet = checkedWallet[0];

                #endregion
            }

            //判断钱包有无应收款
            var noSettleBill = BLLCom.GetNoSettleBillByCustomerID(curWallet.Wal_CustomerID);
            if (noSettleBill.Count > 0)
            {
                MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0000, new object[] { "钱包的开户人还有未结算的单据，请先收款" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (curWallet.Wal_AvailableBalance > 0)
            {
                MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0000, new object[] { "该钱包余额大于零,请提现后销户" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //提示信息
            string warningMessage = "您是否需要销户？\n";
            warningMessage += "钱包账号：" + curWallet.Wal_No + "\n";
            warningMessage += "开户组织：" + curWallet.Wal_CreatedByOrgName + "\n";
            warningMessage += "开户人：" + curWallet.Wal_CustomerName + "\n";
            warningMessage += "可用余额：" + curWallet.Wal_AvailableBalance + "\n";
            //弹提示框
            DialogResult dialogResult = MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0000, warningMessage), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            #endregion

            #region 保存数据

            bool closeAccountResult = _bll.CloseAccountDetailDS(curWallet);
            if (!closeAccountResult)
            {
                //销户失败
                MessageBoxs.Show(Trans.RIA, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //销户成功
            MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.CLOSEACCOUNT }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            #endregion

            //刷新列表
            RefreshList();

            //4.将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();
        }

        #region 导航按钮

        /// <summary>
        /// 初始化导航按钮
        /// </summary>
        public override void InitializeNavigate()
        {
            base.InitializeNavigate();
            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.DEPOSITMONEY, true, false);
        }

        /// <summary>
        /// 充值
        /// </summary>
        public override void WalletDepositMoney()
        {
            //待充值的钱包
            WalletInfoUIModel walletToDeposit = new WalletInfoUIModel();
            if (tabControlFull.Tabs[SysConst.EN_DETAIL].Selected)
            {
                //选中【详情】Tab的场合
                _bll.CopyModel(DetailDS, walletToDeposit);
            }
            else
            {
                //选中【列表】Tab的场合
                gdGrid.UpdateData();

                var selectedWalletList = GridDS.Where(x => x.IsChecked == true).ToList();
                if (selectedWalletList.Count == 0 || selectedWalletList.Count > 1)
                {
                    //请选择一个钱包进行充值
                    MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.W_0006, new object[] { MsgParam.ONE + SystemTableEnums.Name.EWM_Wallet, SystemNavigateEnum.Name.DEPOSITMONEY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    for (int i = 0; i < selectedWalletList.Count; i++)
                    {
                        if (i != 0)
                        {
                            selectedWalletList[i].IsChecked = false;
                        }
                        gdGrid.DataSource = GridDS;
                        gdGrid.DataBind();
                    }
                    return;
                }
                _bll.CopyModel(selectedWalletList[0], walletToDeposit);
            }

            if (string.IsNullOrEmpty(walletToDeposit.Wal_ID)
                || string.IsNullOrEmpty(walletToDeposit.Wal_No))
            {
                //没有获取到钱包，充值失败
                MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { MsgParam.WALLET, SystemActionEnum.Name.RECHARGE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
            {
                //钱包相关信息
                {RIAViewParamKey.WalletInfo.ToString(), walletToDeposit},
            };

            //跳转到[钱包充值]
            SystemFunction.ShowViewFromView(MsgParam.WALLET_DEPOSITMONEY, ViewClassFullNameConst.RIA_FrmWalletDepositMoney, true, PageDisplayMode.TabPage, paramViewParameters, null);
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
            //钱包账号
            txtWal_No.Clear();
            //来源类型
            cbWal_SourceTypeName.Items.Clear();
            //来源账号
            txtWal_SourceNo.Clear();
            //所有人类别
            cbWal_OwnerTypeName.Items.Clear();
            //开户人
            txtWal_CustomerName.Clear();
            //汽修商户
            mcbAutoFactoryName.Clear();
            //可用余额
            txtWal_AvailableBalance.Clear();
            //冻结余额
            txtWal_FreezingBalance.Clear();
            //推荐员工
            mcbWal_RecommendEmployee.Clear();
            //生效时间
            dtWal_EffectiveTime.Value = null;
            //失效时间
            dtWal_IneffectiveTime.Value = null;
            //开户组织
            txtWal_CreatedByOrgName.Clear();
            //状态
            cbWal_StatusName.Items.Clear();
            //备注
            txtWal_Remark.Clear();
            //有效
            ckWal_IsValid.Checked = true;
            ckWal_IsValid.CheckState = CheckState.Checked;
            //创建人
            txtWal_CreatedBy.Text = LoginInfoDAX.UserName;
            //创建时间
            dtWal_CreatedTime.Value = DateTime.Now;
            //修改人
            txtWal_UpdatedBy.Text = LoginInfoDAX.UserName;
            //修改时间
            dtWal_UpdatedTime.Value = DateTime.Now;
            //钱包ID
            txtWal_ID.Clear();
            //组织ID
            txtWal_Org_ID.Clear();
            //组织名称
            txtWal_Org_Name.Clear();
            //开户人ID
            txtWal_CustomerID.Clear();
            //汽修商户组织编码
            txtWal_AutoFactoryOrgCode.Clear();
            //交易密码
            txtWal_TradingPassword.Clear();
            //充值基数
            txtWal_DepositBaseAmount.Clear();
            //开户组织ID
            txtWal_CreatedByOrgID.Clear();
            //版本号
            txtWal_VersionNo.Clear();
            //给 钱包账号 设置焦点
            lblWal_No.Focus();
            #endregion

            #region 初始化下拉框

            //所有人类别
            _ownerTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.CustomerType);
            cbWal_OwnerTypeName.DisplayMember = SysConst.EN_TEXT;
            cbWal_OwnerTypeName.ValueMember = SysConst.EN_Code;
            cbWal_OwnerTypeName.DataSource = _ownerTypeList;
            cbWal_OwnerTypeName.DataBind();

            //钱包状态
            _walletStatusList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.WalletStatus);
            cbWal_StatusName.DisplayMember = SysConst.EN_TEXT;
            cbWal_StatusName.ValueMember = SysConst.EN_Code;
            cbWal_StatusName.DataSource = _walletStatusList;
            cbWal_StatusName.DataBind();

            //来源类型
            _walletSourceTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.WalletSourceType);
            cbWal_SourceTypeName.DisplayMember = SysConst.EN_TEXT;
            cbWal_SourceTypeName.ValueMember = SysConst.EN_Code;
            cbWal_SourceTypeName.DataSource = _walletSourceTypeList;
            cbWal_SourceTypeName.DataBind();

            //汽修商户名称
            _autoFactoryList = CacheDAX.Get(CacheDAX.ConfigDataKey.ARMerchant) as List<MDLPIS_AutoFactoryCustomer>;
            if (_autoFactoryList != null)
            {
                var autoFactoryCustomerList = _autoFactoryList.Where(x => x.AFC_IsPlatform == true).ToList();
                mcbAutoFactoryName.DisplayMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Name;
                mcbAutoFactoryName.ValueMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Code;
                mcbAutoFactoryName.DataSource = autoFactoryCustomerList;
            }

            //推荐员工
            _autoUserList = CacheDAX.Get(CacheDAX.ConfigDataKey.SystemUser) as List<MDLSM_User>;
            mcbWal_RecommendEmployee.DisplayMember = SystemTableColumnEnums.SM_User.Code.User_Name;
            mcbWal_RecommendEmployee.ValueMember = SystemTableColumnEnums.SM_User.Code.User_ID;
            mcbWal_RecommendEmployee.DataSource = _autoUserList;
            #endregion
        }

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 查询条件初始化
            //组织
            cbWhere_Wal_Org_ID.Items.Clear();
            //钱包账号
            txtWhere_Wal_No.Clear();
            //所有人类别
            cbWhere_Wal_OwnerTypeName.Value = null;
            //开户人
            txtWhere_Wal_CustomerName.Clear();
            //汽修商户
            mcbWhere_AutoFactoryName.Clear();
            //推荐员工
            mcbWhere_Wal_RecommendEmployee.Clear();
            //开户时间-开始
            dtWhere_Wal_CreatedTimeStart.Value = null;
            //开户时间-终了
            dtWhere_Wal_CreatedTimeEnd.Value = null;
            //手机号
            txtWhere_CustomerPhoneNo.Clear();
            //给 钱包账号 设置焦点
            lblWhere_Wal_No.Focus();
            #endregion

            #region Grid初始化

            //清空Grid
            GridDS = new BindingList<WalletQueryAndOperateUIModel>();
            gdGrid.DataSource = GridDS;
            gdGrid.DataBind();

            #endregion

            #region 初始化下拉框
            //组织
            _orgList = LoginInfoDAX.OrgList;
            cbWhere_Wal_Org_ID.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            cbWhere_Wal_Org_ID.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            cbWhere_Wal_Org_ID.DataSource = _orgList;
            cbWhere_Wal_Org_ID.DataBind();

            //所有人类别
            cbWhere_Wal_OwnerTypeName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_Wal_OwnerTypeName.ValueMember = SysConst.EN_Code;
            cbWhere_Wal_OwnerTypeName.DataSource = _ownerTypeList;
            cbWhere_Wal_OwnerTypeName.DataBind();

            //汽修商户名称
            if (_autoFactoryList != null)
            {
                var autoFactoryCustomerList = _autoFactoryList.Where(x => x.AFC_IsPlatform == true).ToList();
                mcbWhere_AutoFactoryName.DisplayMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Name;
                mcbWhere_AutoFactoryName.ValueMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Code;
                mcbWhere_AutoFactoryName.DataSource = autoFactoryCustomerList;
            }

            //推荐员工
            mcbWhere_Wal_RecommendEmployee.DisplayMember = SystemTableColumnEnums.SM_User.Code.User_Name;
            mcbWhere_Wal_RecommendEmployee.ValueMember = SystemTableColumnEnums.SM_User.Code.User_ID;
            mcbWhere_Wal_RecommendEmployee.DataSource = _autoUserList;

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
            base.NewUIModel = DetailDS;

            var activeRowIndex = gdGrid.ActiveRow.Index;
            //判断Grid内[唯一标识]是否为空
            if (gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.EWM_Wallet.Code.Wal_ID].Value == null ||
                string.IsNullOrEmpty(gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.EWM_Wallet.Code.Wal_ID].Value.ToString()))
            {
                return;
            }
            //将选中的Grid行对应数据Model赋值给[DetailDS]
            //********************************************************************************
            //**********************************【重要说明】**********************************
            //*****此处和上面的条件判断必须用GridDS内能唯一标识一条记录的字段作为过滤条件*****
            //********************************************************************************
            DetailDS = GridDS.FirstOrDefault(x => x.Wal_ID == gdGrid.Rows[activeRowIndex].Cells[SystemTableColumnEnums.EWM_Wallet.Code.Wal_ID].Value);
            if (DetailDS == null || string.IsNullOrEmpty(DetailDS.Wal_ID))
            {
                return;
            }

            if (txtWal_ID.Text != DetailDS.Wal_ID
                || (txtWal_ID.Text == DetailDS.Wal_ID && txtWal_VersionNo.Text != DetailDS.Wal_VersionNo?.ToString()))
            {
                if (txtWal_ID.Text == DetailDS.Wal_ID && txtWal_VersionNo.Text != DetailDS.Wal_VersionNo?.ToString())
                {
                    //数据版本已过期，将加载最新详情。
                    MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.DataHasOverdue }), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
                else if (ViewHasChanged(_skipPropertyList))
                {
                    //将放弃之前的修改，是否继续？
                    DialogResult dialogResult = MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.I_0000, new object[] { MsgParam.ConfirmGiveUpEdit }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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
            //设置详情页面控件以及导航按钮是否可编辑
            SetDetailControl();
        }

        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //钱包账号
            txtWal_No.Text = DetailDS.Wal_No;
            //来源类型
            cbWal_SourceTypeName.Text = DetailDS.Wal_SourceTypeName;
            //钱包来源类型编码
            cbWal_SourceTypeName.Value = DetailDS.Wal_SourceTypeCode;
            //来源账号
            txtWal_SourceNo.Text = DetailDS.Wal_SourceNo;
            //所有人类别
            cbWal_OwnerTypeName.Text = DetailDS.Wal_OwnerTypeName;
            //钱包所有人类别编码
            cbWal_OwnerTypeName.Value = DetailDS.Wal_OwnerTypeCode;
            //汽修商户
            mcbAutoFactoryName.SelectedValue = DetailDS.Wal_AutoFactoryCode;
            //开户人
            txtWal_CustomerName.Text = DetailDS.Wal_CustomerName;
            //可用余额
            txtWal_AvailableBalance.Value = DetailDS.Wal_AvailableBalance;
            //冻结余额
            txtWal_FreezingBalance.Value = DetailDS.Wal_FreezingBalance;
            //推荐员工
            mcbWal_RecommendEmployee.SelectedValue = DetailDS.Wal_RecommendEmployeeID;
            //生效时间
            dtWal_EffectiveTime.Value = DetailDS.Wal_EffectiveTime;
            //失效时间
            dtWal_IneffectiveTime.Value = DetailDS.Wal_IneffectiveTime;
            //开户组织
            txtWal_CreatedByOrgName.Text = DetailDS.Wal_CreatedByOrgName;
            //状态
            cbWal_StatusName.Text = DetailDS.Wal_StatusName;
            //钱包状态编码
            cbWal_StatusName.Value = DetailDS.Wal_StatusCode;
            //备注
            txtWal_Remark.Text = DetailDS.Wal_Remark;
            //有效
            if (DetailDS.Wal_IsValid != null)
            {
                ckWal_IsValid.Checked = DetailDS.Wal_IsValid.Value;
            }
            //创建人
            txtWal_CreatedBy.Text = DetailDS.Wal_CreatedBy;
            //创建时间
            dtWal_CreatedTime.Value = DetailDS.Wal_CreatedTime;
            //修改人
            txtWal_UpdatedBy.Text = DetailDS.Wal_UpdatedBy;
            //修改时间
            dtWal_UpdatedTime.Value = DetailDS.Wal_UpdatedTime;
            //钱包ID
            txtWal_ID.Text = DetailDS.Wal_ID;
            //组织ID
            txtWal_Org_ID.Text = DetailDS.Wal_Org_ID;
            //组织名称
            txtWal_Org_Name.Text = DetailDS.Wal_Org_Name;
            //开户人ID
            txtWal_CustomerID.Text = DetailDS.Wal_CustomerID;
            //汽修商户组织编码
            txtWal_AutoFactoryOrgCode.Text = DetailDS.Wal_AutoFactoryOrgCode;
            //交易密码
            txtWal_TradingPassword.Text = DetailDS.Wal_TradingPassword;
            //充值基数
            txtWal_DepositBaseAmount.Value = DetailDS.Wal_DepositBaseAmount;
            //开户组织ID
            txtWal_CreatedByOrgID.Text = DetailDS.Wal_CreatedByOrgID;
            //版本号
            txtWal_VersionNo.Value = DetailDS.Wal_VersionNo;
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
            return true;
        }

        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            DetailDS = new WalletQueryAndOperateUIModel()
            {
                //钱包账号
                Wal_No = txtWal_No.Text.Trim(),
                //来源类型
                Wal_SourceTypeName = cbWal_SourceTypeName.Text.Trim(),
                //钱包来源类型编码
                Wal_SourceTypeCode = cbWal_SourceTypeName.Value?.ToString(),
                //来源账号
                Wal_SourceNo = txtWal_SourceNo.Text.Trim(),
                //所有人类别
                Wal_OwnerTypeName = cbWal_OwnerTypeName.Text.Trim(),
                //钱包所有人类别编码
                Wal_OwnerTypeCode = cbWal_OwnerTypeName.Value?.ToString(),
                //汽修商户
                Wal_AutoFactoryCode = mcbAutoFactoryName.SelectedValue,
                AutoFactoryName = mcbAutoFactoryName.SelectedText,
                //开户人
                Wal_CustomerName = txtWal_CustomerName.Text.Trim(),
                //可用余额
                Wal_AvailableBalance = Convert.ToDecimal(txtWal_AvailableBalance.Text.Trim() == "" ? "0" : txtWal_AvailableBalance.Text.Trim()),
                //冻结余额
                Wal_FreezingBalance = Convert.ToDecimal(txtWal_FreezingBalance.Text.Trim() == "" ? "0" : txtWal_FreezingBalance.Text.Trim()),
                //推荐员工
                Wal_RecommendEmployee = mcbWal_RecommendEmployee.SelectedText,
                //推荐员工ID
                Wal_RecommendEmployeeID = mcbWal_RecommendEmployee.SelectedValue,
                //生效时间
                Wal_EffectiveTime = (DateTime?)dtWal_EffectiveTime.Value,
                //失效时间
                Wal_IneffectiveTime = (DateTime?)dtWal_IneffectiveTime.Value,
                //开户组织
                Wal_CreatedByOrgName = txtWal_CreatedByOrgName.Text.Trim(),
                //状态
                Wal_StatusName = cbWal_StatusName.Text.Trim(),
                //钱包状态编码
                Wal_StatusCode = cbWal_StatusName.Value?.ToString() ?? "",
                //备注
                Wal_Remark = txtWal_Remark.Text.Trim(),
                //有效
                Wal_IsValid = ckWal_IsValid.Checked,
                //创建人
                Wal_CreatedBy = txtWal_CreatedBy.Text.Trim(),
                //创建时间
                Wal_CreatedTime = (DateTime?)dtWal_CreatedTime.Value ?? DateTime.Now,
                //修改人
                Wal_UpdatedBy = txtWal_UpdatedBy.Text.Trim(),
                //修改时间
                Wal_UpdatedTime = (DateTime?)dtWal_UpdatedTime.Value ?? DateTime.Now,
                //钱包ID
                Wal_ID = txtWal_ID.Text.Trim(),
                //组织ID
                Wal_Org_ID = txtWal_Org_ID.Text.Trim(),
                //组织名称
                Wal_Org_Name = txtWal_Org_Name.Text.Trim(),
                //开户人ID
                Wal_CustomerID = txtWal_CustomerID.Text.Trim(),
                //汽修商户组织编码
                Wal_AutoFactoryOrgCode = txtWal_AutoFactoryOrgCode.Text.Trim(),
                //交易密码
                Wal_TradingPassword = txtWal_TradingPassword.Text.Trim(),
                //充值基数
                Wal_DepositBaseAmount = Convert.ToDecimal(txtWal_DepositBaseAmount.Text.Trim() == "" ? "0" : txtWal_DepositBaseAmount.Text.Trim()),
                //开户组织ID
                Wal_CreatedByOrgID = txtWal_CreatedByOrgID.Text.Trim(),
                //版本号
                Wal_VersionNo = Convert.ToInt64(txtWal_VersionNo.Text.Trim() == "" ? "1" : txtWal_VersionNo.Text.Trim()),
            };
        }

        /// <summary>
        /// 设置详情页面控件以及导航按钮是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            if (string.IsNullOrEmpty(txtWal_ID.Text)
                || string.IsNullOrEmpty(txtWal_No.Text))
            {
                //未保存的场合，[充值]按钮不可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.DEPOSITMONEY, true, false);
            }
            else
            {
                //已保存的场合，[充值]按钮可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.DEPOSITMONEY, true, true);
            }

            if (cbWal_OwnerTypeName.Text == CustomerTypeEnum.Name.PTNQXSH)
            {
                //所有人类别为{平台内汽修商}的场合，显示[汽修商户]
                lblAutoFactoryName.Visible = true;
                mcbAutoFactoryName.Visible = true;
            }
            else
            {
                //所有人类别不是{平台内汽修商}的场合，隐藏[汽修商户]
                lblAutoFactoryName.Visible = false;
                mcbAutoFactoryName.Visible = false;
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
                    var removeList = GridDS.Where(x => x.IsChecked == true).ToList();
                    foreach (var loopRemove in removeList)
                    {
                        GridDS.Remove(loopRemove);
                    }
                }
                else
                {
                    var curHead = GridDS.FirstOrDefault(x => x.Wal_ID == DetailDS.Wal_ID);
                    if (curHead != null)
                    {
                        GridDS.Remove(curHead);
                    }
                }
            }
            else
            {
                var curHead = GridDS.FirstOrDefault(x => x.Wal_ID == DetailDS.Wal_ID);
                if (curHead != null)
                {
                    _bll.CopyModel(DetailDS, curHead);
                }
                else
                {
                    GridDS.Insert(0, DetailDS);
                }
            }

            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
        }

        #endregion

    }
}
