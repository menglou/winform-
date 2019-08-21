using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.RIA;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Base;
using SkyCar.Coeus.UIModel.RIA.QCModel;
using SkyCar.Coeus.UIModel.RIA.UIModel;

namespace SkyCar.Coeus.UI.RIA
{
    /// <summary>
    /// 钱包金额流水
    /// </summary>
    public partial class FrmWalletTransLogQuery : BaseFormCardList<WalletTransLogQueryUIModel, WalletTransLogQueryQCModel, MDLEWM_WalletTrans>
    {
        #region 全局变量

        /// <summary>
        /// 钱包金额流水BLL
        /// </summary>
        private WalletTransLogQueryBLL _bll = new WalletTransLogQueryBLL();

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
        /// 异动类型
        /// </summary>
        List<ComComboBoxDataSourceTC> _walletTransTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 充值方式
        /// </summary>
        List<ComComboBoxDataSourceTC> _rechargeTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 汽修商户
        /// </summary>
        List<MDLPIS_AutoFactoryCustomer> _autoFactoryList = new List<MDLPIS_AutoFactoryCustomer>();
        #endregion

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmWalletTransLogQuery构造方法
        /// </summary>
        public FrmWalletTransLogQuery()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmWalletTransLogQuery_Load(object sender, EventArgs e)
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

            //初始化【列表】Tab内控件
            InitializeListTabControls();
            //根据选中的Tab，设置动作按钮[是否可用]（在系统权限的基础上进行控制）
            base.SetActionEnableBySelectedTab(SysConst.EN_LIST);
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

            PageSize = tmpPageSize;

            ExecuteQuery?.Invoke();
        }

        /// <summary>
        /// 列表Grid_DoubleClickCell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            if (e.Cell.Column.Key == SystemTableColumnEnums.EWM_WalletTrans.Code.WalT_BillNo)
            {
                string billNo = e.Cell.Text;
                if (billNo.Length < 7)
                {
                    return;
                }
                string no = billNo.Substring(4, 2);
                if (no == "RB")
                {
                    Dictionary<string, object> paramViewParameters = new Dictionary<string, object>
                    {
                        //收款单
                        {ComViewParamKey.BillNo.ToString(), billNo},
                    };
                    //跳转到[收款单]
                    SystemFunction.ShowViewFromView(MsgParam.RECEIPTBILL_MANAGER, ViewClassFullNameConst.FM_FrmReceiptBillManager, true, PageDisplayMode.TabPage, paramViewParameters, null);
                }
                else if (no == "PB")
                {
                    Dictionary<string, object> parambillNoViewParameters = new Dictionary<string, object>
                    {
                        //付款单
                        {ComViewParamKey.BillNo.ToString(), billNo},
                    };
                    //跳转到[付款单]
                    SystemFunction.ShowViewFromView(MsgParam.PAYBILL_MANGER, ViewClassFullNameConst.FM_FrmPayBillManager, true, PageDisplayMode.TabPage, parambillNoViewParameters, null);
                }
            }
        }

        #region 查询条件相关事件

        /// <summary>
        /// 钱包账号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_WalT_Wal_No_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
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
        /// 异动类型ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_WalT_TypeName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 充值方式ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbWhere_WalT_RechargeTypeName_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 异动单号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWhere_WalT_BillNo_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    //执行查询
            //    QueryAction();
            //}
        }
        /// <summary>
        /// 异动时间ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_WalT_TimeStart_ValueChanged(object sender, EventArgs e)
        {
            //执行查询
            //QueryAction();
        }
        /// <summary>
        /// 列表查询条件dtWhere_WalT_TimeEnd_ValueChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtWhere_WalT_TimeEnd_ValueChanged(object sender, EventArgs e)
        {
            if (this.dtWhere_WalT_TimeEnd.Value != null &&
              this.dtWhere_WalT_TimeEnd.DateTime.Hour == 0 &&
              this.dtWhere_WalT_TimeEnd.DateTime.Minute == 0 &&
              this.dtWhere_WalT_TimeEnd.DateTime.Second == 0)
            {
                DateTime newDateTime = new DateTime(this.dtWhere_WalT_TimeEnd.DateTime.Year, this.dtWhere_WalT_TimeEnd.DateTime.Month, this.dtWhere_WalT_TimeEnd.DateTime.Day, 23, 59, 59);
                this.dtWhere_WalT_TimeEnd.DateTime = newDateTime;
            }
        }
        #endregion

        #endregion

        #region 重写基类方法

        /// <summary>
        /// 查询
        /// </summary>
        public override void QueryAction()
        {
            //2.设置查询条件（翻页相关属性不用设置）
            ConditionDS = new WalletTransLogQueryQCModel()
            {
                //SqlId
                SqlId = SQLID.RIA_WalletTransLogQuery_SQL01,
                //钱包账号
                WHERE_WalT_Wal_No = txtWhere_WalT_Wal_No.Text.Trim(),
                //所有人类别
                WHERE_Wal_OwnerTypeName = cbWhere_Wal_OwnerTypeName.Text.Trim(),
                //开户人
                WHERE_Wal_CustomerName = txtWhere_Wal_CustomerName.Text.Trim(),
                //异动类型
                WHERE_WalT_TypeName = cbWhere_WalT_TypeName.Text.Trim(),
                //充值方式
                WHERE_WalT_RechargeTypeName = cbWhere_WalT_RechargeTypeName.Text.Trim(),
                //异动单号
                WHERE_WalT_BillNo = txtWhere_WalT_BillNo.Text.Trim(),
                //开户人手机号
                WHERE_CustomerPhoneNo = txtWhere_CustomerPhoneNo.Text.Trim(),
                //汽修商户
                WHERE_AutoFactoryName = mcbWhere_AutoFactoryName.SelectedText,
                //受理组织
                WHERE_WalT_Org_ID = cbWhere_WalT_Org_ID.Value?.ToString(),
                //备注
                WHERE_WalT_Remark = txtWhere_WalT_Remark.Text.Trim(),
            };
            if (dtWhere_WalT_TimeStart.Value != null)
            {
                // 异动时间-开始
                ConditionDS._TimeStart = dtWhere_WalT_TimeStart.DateTime;
            }
            if (dtWhere_WalT_TimeEnd.Value != null)
            {
                // 异动时间-终了
                ConditionDS._TimeEnd = dtWhere_WalT_TimeEnd.DateTime;
            }

            //3.执行基类方法（注意：基类使用模糊查询）
            base.QueryAction();
            //4.Grid绑定数据源
            gdGrid.DataSource = base.GridDS;
            gdGrid.DataBind();
            //5.设置Grid自适应列宽（根据单元格内容）
            gdGrid.DisplayLayout.Bands[0].PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
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
        /// 导出当前页
        /// </summary>
        public override void ExportAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = "钱包金额流水";
            base.ExportAction(gdGrid, paramGridName);
        }
        /// <summary>
        /// 导出全部
        /// </summary>
        public override void ExportAllAction(UltraGrid paramGrid, string paramGridName = "")
        {
            paramGridName = "钱包金额流水";
            List<WalletTransLogQueryUIModel> resultAllList = new List<WalletTransLogQueryUIModel>();
            _bll.QueryForList(SQLID.RIA_WalletTransLogQuery_SQL01, new WalletTransLogQueryQCModel()
            {
                PageIndex = 1,
                PageSize = null,
                //钱包账号
                WHERE_WalT_Wal_No = txtWhere_WalT_Wal_No.Text.Trim(),
                //所有人类别
                WHERE_Wal_OwnerTypeName = cbWhere_Wal_OwnerTypeName.Text.Trim(),
                //开户人
                WHERE_Wal_CustomerName = txtWhere_Wal_CustomerName.Text.Trim(),
                //异动类型
                WHERE_WalT_TypeName = cbWhere_WalT_TypeName.Text.Trim(),
                //充值方式
                WHERE_WalT_RechargeTypeName = cbWhere_WalT_RechargeTypeName.Text.Trim(),
                //异动单号
                WHERE_WalT_BillNo = txtWhere_WalT_BillNo.Text.Trim(),
                //开户人手机号
                WHERE_CustomerPhoneNo = txtWhere_CustomerPhoneNo.Text.Trim(),
                //汽修商户
                WHERE_AutoFactoryName = mcbWhere_AutoFactoryName.SelectedText,
                //受理组织
                WHERE_WalT_Org_ID = cbWhere_WalT_Org_ID.Value?.ToString(),
                //备注
                WHERE_WalT_Remark = txtWhere_WalT_Remark.Text.Trim(),
            }, resultAllList);
            UltraGrid allGrid = gdGrid;
            allGrid.DataSource = resultAllList;
            allGrid.DataBind();

            base.ExportAllAction(allGrid, paramGridName);

            gdGrid.DataSource = GridDS;
            gdGrid.DataBind();
        }
        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【列表】Tab内控件
        /// </summary>
        private void InitializeListTabControls()
        {
            #region 工具生成

            #region 查询条件初始化
            //钱包账号
            txtWhere_WalT_Wal_No.Clear();
            //受理组织
            cbWhere_WalT_Org_ID.Items.Clear();
            //所有人类别
            cbWhere_Wal_OwnerTypeName.Items.Clear();
            //汽修商户
            mcbWhere_AutoFactoryName.Clear();
            //开户人
            txtWhere_Wal_CustomerName.Clear();
            //异动类型
            cbWhere_WalT_TypeName.Items.Clear();
            //充值方式
            cbWhere_WalT_RechargeTypeName.Items.Clear();
            //异动单号
            txtWhere_WalT_BillNo.Clear();
            //异动时间-开始
            dtWhere_WalT_TimeStart.Value = null;
            //异动时间-终了
            dtWhere_WalT_TimeEnd.Value = null;
            //开户人手机号
            txtWhere_CustomerPhoneNo.Clear();
            //备注
            txtWhere_WalT_Remark.Clear();
            //给 钱包账号 设置焦点
            lblWhere_WalT_Wal_No.Focus();
            #endregion

            #region Grid初始化

            //清空Grid
            GridDS = new BindingList<WalletTransLogQueryUIModel>();
            gdGrid.DataSource = GridDS;
            gdGrid.DataBind();

            #endregion

            #endregion

            #region 初始化下拉框
            //组织
            _orgList = LoginInfoDAX.OrgList;
            cbWhere_WalT_Org_ID.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
            cbWhere_WalT_Org_ID.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
            cbWhere_WalT_Org_ID.DataSource = _orgList;
            cbWhere_WalT_Org_ID.DataBind();

            //所有人类别
            _ownerTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.CustomerType);
            cbWhere_Wal_OwnerTypeName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_Wal_OwnerTypeName.ValueMember = SysConst.EN_Code;
            cbWhere_Wal_OwnerTypeName.DataSource = _ownerTypeList;
            cbWhere_Wal_OwnerTypeName.DataBind();

            //异动类型
            _walletTransTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.WalTransType);
            cbWhere_WalT_TypeName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_WalT_TypeName.ValueMember = SysConst.EN_Code;
            cbWhere_WalT_TypeName.DataSource = _walletTransTypeList;
            cbWhere_WalT_TypeName.DataBind();

            //汽修商户名称
            _autoFactoryList = CacheDAX.Get(CacheDAX.ConfigDataKey.ARMerchant) as List<MDLPIS_AutoFactoryCustomer>;
            if (_autoFactoryList != null)
            {
                var autoFactoryCustomerList = _autoFactoryList.Where(x => x.AFC_IsPlatform == true).ToList();
                mcbWhere_AutoFactoryName.DisplayMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Name;
                mcbWhere_AutoFactoryName.ValueMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Code;
                mcbWhere_AutoFactoryName.DataSource = autoFactoryCustomerList;
            }

            //充值方式
            _rechargeTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.TradeType);
            var tempRechargeTypeList = _rechargeTypeList.Where(x => x.Text != TradeTypeEnum.Name.WALLET
            && x.Text != TradeTypeEnum.Name.ONACCOUNT).ToList();
            cbWhere_WalT_RechargeTypeName.DisplayMember = SysConst.EN_TEXT;
            cbWhere_WalT_RechargeTypeName.ValueMember = SysConst.EN_Code;
            cbWhere_WalT_RechargeTypeName.DataSource = tempRechargeTypeList;
            cbWhere_WalT_RechargeTypeName.DataBind();
            #endregion
        }
        #endregion
        
    }
}
