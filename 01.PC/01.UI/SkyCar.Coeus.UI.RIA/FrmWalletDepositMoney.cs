using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
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
using SkyCar.Coeus.UIModel.Common.APModel;
using SkyCar.Coeus.UIModel.RIA.QCModel;
using SkyCar.Coeus.UIModel.RIA.UIModel;

namespace SkyCar.Coeus.UI.RIA
{
    /// <summary>
    /// 钱包充值
    /// </summary>
    public partial class FrmWalletDepositMoney : BaseFormCardList<WalletDepositMoneyUIModel, WalletDepositMoneyQCModel, MDLEWM_Wallet>
    {
        #region 全局变量

        /// <summary>
        /// 钱包充值BLL
        /// </summary>
        private WalletDepositMoneyBLL _bll = new WalletDepositMoneyBLL();

        /// <summary>
        /// 充值明细列表
        /// </summary>
        private List<WalletDepositMoneyUIModel> _depositDetailList = new List<WalletDepositMoneyUIModel>();

        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters;

        /// <summary>
        /// 钱包原可用余额
        /// </summary>
        private decimal _oldAvailableBalance = 0;

        /// <summary>
        /// 钱包账号
        /// </summary>
        private string _latestWalletNo = string.Empty;

        #region 下拉框数据源

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
        /// FrmWalletDepositMoney构造方法
        /// </summary>
        public FrmWalletDepositMoney()
        {
            InitializeComponent();
        }
        /// <summary>
        /// FrmWalletDepositMoney构造方法
        /// </summary>
        public FrmWalletDepositMoney(Dictionary<string, object> paramWindowParameters)
        {
            InitializeComponent();

            _viewParameters = paramWindowParameters;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmWalletDepositMoney_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（动作，导航）
            base.ToolBarActionAndNavigate = toolBarActionAndNavigate;
            //工具栏（动作）单击事件
            this.toolBarActionAndNavigate.ToolClick += new ToolClickEventHandler(base.toolBarActionAndNavigate_ToolClick);
            //初始化【详情】Tab内控件
            InitializeDetailTabControls();
            //根据选中的Tab，设置动作按钮[是否可用]（在系统权限的基础上进行控制）
            base.SetActionEnableBySelectedTab(SysConst.EN_LIST);
            #endregion

            //[保存]可用
            SetActionEnable(SystemActionEnum.Code.SAVE, true);

            #region 处理参数

            if (_viewParameters == null)
            {
                return;
            }

            #region 钱包相关信息

            if (_viewParameters.ContainsKey(RIAViewParamKey.WalletInfo.ToString()))
            {
                WalletInfoUIModel resultWalletInfo = _viewParameters[RIAViewParamKey.WalletInfo.ToString()] as WalletInfoUIModel;

                if (resultWalletInfo != null)
                {
                    //钱包ID
                    txtWal_ID.Text = resultWalletInfo.Wal_ID;
                    //钱包账号
                    txtWal_No.Text = resultWalletInfo.Wal_No;
                    //开户人
                    txtWal_CustomerID.Text = resultWalletInfo.Wal_CustomerID;
                    txtWal_CustomerName.Text = resultWalletInfo.Wal_CustomerName;
                    //汽修商户
                    mcbAutoFactoryName.SelectedValue = resultWalletInfo.Wal_AutoFactoryCode;
                    //汽修商组织编码
                    txtWal_AutoFactoryOrgCode.Text = resultWalletInfo.Wal_AutoFactoryOrgCode;
                    //可用余额
                    txtWal_AvailableBalance.Text = (resultWalletInfo.Wal_AvailableBalance ?? 0).ToString();
                    _oldAvailableBalance = resultWalletInfo.Wal_AvailableBalance ?? 0;
                    //充值基数
                    txtWal_DepositBaseAmount.Text = (resultWalletInfo.Wal_DepositBaseAmount ?? 0).ToString();
                    //钱包版本号
                    txtWal_VersionNo.Text = resultWalletInfo.Wal_VersionNo?.ToString() ?? "1";
                }
            }
            #endregion

            #endregion
            
            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            AcceptUIModelChanges();
        }

        /// <summary>
        /// 钱包账号失焦
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWal_No_MouseLeave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtWal_No.Text)
                || txtWal_No.Text == _latestWalletNo)
            {
                return;
            }

            //验证并获取钱包信息
            ValidateAndGetWalletInfo();

            _latestWalletNo = txtWal_No.Text;
        }
        /// <summary>
        /// 钱包账号KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWal_No_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //验证并获取钱包信息
                ValidateAndGetWalletInfo();
            }
        }

        /// <summary>
        /// 充值明细Grid的单元格改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gdGrid_CellChange(object sender, CellEventArgs e)
        {
            gdGrid.UpdateData();

            #region 计算本次充值金额、可用余额

            //明细中总金额之和
            decimal tempTotalDetailAmount = 0;

            foreach (var loopDeatil in _depositDetailList)
            {
                tempTotalDetailAmount += (loopDeatil.PayAmount ?? 0);
            }
            //本次充值金额
            txtThisDepositAmount.Text = tempTotalDetailAmount.ToString();

            #endregion
        }

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
            if (ViewHasChanged())
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

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
        }

        /// <summary>
        /// 充值
        /// </summary>
        public override void RechargeAction()
        {
            //1.前端检查-充值
            if (!ClientCheckForRecharge())
            {
                return;
            }
            //2.将【详情】Tab内控件的值赋值给基类的DetailDS
            SetCardCtrlsToDetailDS();
            //3.执行保存（含服务端检查）
            bool saveResult = _bll.SaveDetailDS(DetailDS, _depositDetailList);
            if (!saveResult)
            {
                //充值失败
                MessageBoxs.Show(Trans.RIA, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //充值成功
            MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.RECHARGE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //初始化详情
            InitializeDetailTabControls();

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();
        }

        /// <summary>
        /// 关闭画面时检查画面值是否发生变化
        /// </summary>
        /// <returns></returns>
        public override bool ViewHasChangedWhenClose()
        {
            SetCardCtrlsToDetailDS();
            base.NewUIModel = DetailDS;
            if (ViewHasChanged())
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            #region 工具生成
            //钱包ID
            txtWal_ID.Clear();
            //钱包账号
            txtWal_No.Clear();
            //开户人ID
            txtWal_CustomerID.Clear();
            //开户人姓名
            txtWal_CustomerName.Clear();
            //汽修商户
            mcbAutoFactoryName.Clear();
            //汽修商户组织编码
            txtWal_AutoFactoryOrgCode.Clear();
            //本次充值金额
            txtThisDepositAmount.Clear();
            //可用余额
            txtWal_AvailableBalance.Clear();
            _oldAvailableBalance = 0;
            //充值基数
            txtWal_DepositBaseAmount.Clear();
            //备注
            txtWal_Remark.Clear();
            //版本号
            txtWal_VersionNo.Clear();
            #endregion

            //清除最后账号
            _latestWalletNo = string.Empty;

            #region 初始化下拉框

            //汽修商户名称
            _autoFactoryList = CacheDAX.Get(CacheDAX.ConfigDataKey.ARMerchant) as List<MDLPIS_AutoFactoryCustomer>;
            if (_autoFactoryList != null)
            {
                var autoFactoryCustomerList = _autoFactoryList.Where(x => x.AFC_IsPlatform == true).ToList();
                mcbAutoFactoryName.DisplayMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Name;
                mcbAutoFactoryName.ValueMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Code;
                mcbAutoFactoryName.DataSource = autoFactoryCustomerList;
            }

            #endregion

            #region 加载充值明细列表

            _depositDetailList = new List<WalletDepositMoneyUIModel>();
            List<MDLSM_Enum> tempPaymentModeList = EnumDAX.GetEnum(EnumKey.TradeType);
            foreach (var loopPaymentMode in tempPaymentModeList)
            {
                if (loopPaymentMode.Enum_DisplayName != TradeTypeEnum.Name.WALLET
                    && loopPaymentMode.Enum_DisplayName != TradeTypeEnum.Name.ONACCOUNT)
                {
                    //支付方式：{现金}、{网银转账}、{POS}、{微信}、{支付宝}、{支票}、{其他}、
                    WalletDepositMoneyUIModel tempDepositMoney = new WalletDepositMoneyUIModel()
                    {
                        PaymentModeName = loopPaymentMode.Enum_DisplayName,
                        PaymentModeCode = loopPaymentMode.Enum_ValueCode,
                    };
                    _depositDetailList.Add(tempDepositMoney);
                }
            }
            gdGrid.DataSource = _depositDetailList;
            gdGrid.DataBind();

            #endregion
        }

        /// <summary>
        /// 前端检查-充值
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForRecharge()
        {
            //验证钱包
            if (string.IsNullOrEmpty(txtWal_No.Text))
            {
                //请输入有效的钱包
                MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.E_0002, new object[] { SystemTableColumnEnums.EWM_Wallet.Name.Wal_No }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            WalletInfoUIModel curWallet = BLLCom.GetWalletByWalletNo(txtWal_No.Text);
            if (string.IsNullOrEmpty(curWallet.Wal_ID)
                || string.IsNullOrEmpty(curWallet.Wal_No))
            {
                //没有获取到钱包，充值失败
                MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { MsgParam.WALLET, SystemActionEnum.Name.RECHARGE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //验证本次充值金额
            if (string.IsNullOrEmpty(txtThisDepositAmount.Text)
                || txtThisDepositAmount.Text == "0")
            {
                //本次充值金额不能为零！
                MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { MsgParam.DEPOSITAMOUNT_CANNOTZERO }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //TODO 验证充值基数
            //decimal thisDepositAmount = Convert.ToDecimal(txtThisDepositAmount.Text.Trim() == "" ? "0" : txtThisDepositAmount.Text.Trim());
            ////if (thisDepositAmount > 0 && !BLLCom.HasAuthorityInOrg(LoginInfoDAX.OrgID, SystemAction.IgnoreDepositBaseAmount))
            //if (thisDepositAmount > 0 && !BLLCom.HasAuthorityInOrg(LoginInfoDAX.OrgID, "IgnoreDepositBaseAmount",""))
            //{
            //    int depositBaseAmount = curWallet.Wal_DepositBaseAmount == null || curWallet.Wal_DepositBaseAmount == 0
            //        ? SystemConfigInfo.DepositTimesValue
            //        : Convert.ToInt32(curWallet.Wal_DepositBaseAmount);
            //    if (depositBaseAmount != 0 && base.DetailDS.Wal_Balance % depositBaseAmount != 0)
            //    {
            //        //ErrorMessage.Append(MsgHelp.GetMsg(MsgHelp.E_0000, new object[] { "本次充值金额必须是" + depositBaseAmount + "的整数倍" }));
            //        return false;
            //    }
            //}

            return true;
        }
        
        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            DetailDS = new WalletDepositMoneyUIModel()
            {
                //钱包ID
                Wal_ID = txtWal_ID.Text.Trim(),
                //钱包账号
                Wal_No = txtWal_No.Text.Trim(),
                //开户人ID
                Wal_CustomerID = txtWal_CustomerID.Text.Trim(),
                //开户人姓名
                Wal_CustomerName = txtWal_CustomerName.Text.Trim(),
                //汽修商户编码
                Wal_AutoFactoryCode = mcbAutoFactoryName.SelectedValue,
                //汽修商户名称
                AutoFactoryName = mcbAutoFactoryName.SelectedText,
                //汽修商户组织编码
                Wal_AutoFactoryOrgCode = txtWal_AutoFactoryOrgCode.Text.Trim(),
                //本次充值金额
                ThisDepositAmount = Convert.ToDecimal(txtThisDepositAmount.Text.Trim() == "" ? "0" : txtThisDepositAmount.Text.Trim()),
                //充值基数
                Wal_DepositBaseAmount = Convert.ToDecimal(txtWal_DepositBaseAmount.Text.Trim() == "" ? "0" : txtWal_DepositBaseAmount.Text.Trim()),
                //备注
                Wal_Remark = txtWal_Remark.Text.Trim(),
                //版本号
                Wal_VersionNo = Convert.ToInt64(txtWal_VersionNo.Text.Trim() == "" ? "1" : txtWal_VersionNo.Text.Trim()),
            };
            //钱包可用余额
            DetailDS.Wal_AvailableBalance = _oldAvailableBalance + (DetailDS.ThisDepositAmount ?? 0);
        }

        /// <summary>
        /// 验证并获取钱包信息
        /// </summary>
        private void ValidateAndGetWalletInfo()
        {
            //判断钱包是否正常
            MDLEWM_Wallet argsWalletTbModel = new MDLEWM_Wallet();
            string valicateMessage = string.Empty;
            string newWalletNo = string.Empty;
            bool valicateResult = BLLCom.ValidateWallet(WalTransTypeEnum.Name.CZ, Convert.ToDecimal(txtThisDepositAmount.Text == "" ? "0" : txtThisDepositAmount.Text), string.Empty, txtWal_No.Text, string.Empty, string.Empty, string.Empty, ref valicateMessage, ref newWalletNo, ref argsWalletTbModel);
            if (!valicateResult)
            {
                MessageBoxs.Show(Trans.RIA, ToString(),
                    MsgHelp.GetMsg(MsgCode.I_0000, new object[] { valicateMessage }), MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                //清空画面
                InitializeDetailTabControls();
            }
            else
            {
                if (valicateMessage != string.Empty)
                {
                    var dialogResult = MessageBoxs.Show(Trans.RIA, ToString(),
                        MsgHelp.GetMsg(MsgCode.I_0000, new object[] { valicateMessage }), MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.OK)
                    {
                        WalletInfoUIModel resultWallet = BLLCom.GetWalletByWalletNo(txtWal_No.Text);
                        if (!string.IsNullOrEmpty(resultWallet.Wal_ID)
                            && !string.IsNullOrEmpty(resultWallet.Wal_No))
                        {
                            //钱包ID
                            txtWal_ID.Text = resultWallet.Wal_ID;
                            //开户人
                            txtWal_CustomerID.Text = resultWallet.Wal_CustomerID;
                            txtWal_CustomerName.Text = resultWallet.Wal_CustomerName;
                            //汽修商户
                            mcbAutoFactoryName.SelectedValue = resultWallet.Wal_AutoFactoryCode;
                            //汽修商组织
                            txtWal_AutoFactoryOrgCode.Text = resultWallet.Wal_AutoFactoryOrgCode;
                            //可用余额
                            txtWal_AvailableBalance.Text = (resultWallet.Wal_AvailableBalance ?? 0).ToString();
                            _oldAvailableBalance = resultWallet.Wal_AvailableBalance ?? 0;
                            //充值基数
                            txtWal_DepositBaseAmount.Text = (resultWallet.Wal_DepositBaseAmount ?? 0).ToString();
                            //钱包版本号
                            txtWal_VersionNo.Text = resultWallet.Wal_VersionNo?.ToString() ?? "1";
                        }
                    }
                    else
                    {
                        //清空画面
                        InitializeDetailTabControls();
                        return;
                    }
                }
                else
                {
                    WalletInfoUIModel resultWallet = BLLCom.GetWalletByWalletNo(txtWal_No.Text);
                    if (!string.IsNullOrEmpty(resultWallet.Wal_ID)
                        && !string.IsNullOrEmpty(resultWallet.Wal_No))
                    {
                        //钱包ID
                        txtWal_ID.Text = resultWallet.Wal_ID;
                        //开户人
                        txtWal_CustomerID.Text = resultWallet.Wal_CustomerID;
                        txtWal_CustomerName.Text = resultWallet.Wal_CustomerName;
                        //汽修商户
                        mcbAutoFactoryName.SelectedValue = resultWallet.Wal_AutoFactoryCode;
                        //汽修商组织
                        txtWal_AutoFactoryOrgCode.Text = resultWallet.Wal_AutoFactoryOrgCode;
                        //可用余额
                        txtWal_AvailableBalance.Text = (resultWallet.Wal_AvailableBalance ?? 0).ToString();
                        _oldAvailableBalance = resultWallet.Wal_AvailableBalance ?? 0;
                        //充值基数
                        txtWal_DepositBaseAmount.Text = (resultWallet.Wal_DepositBaseAmount ?? 0).ToString();
                        //钱包版本号
                        txtWal_VersionNo.Text = resultWallet.Wal_VersionNo?.ToString() ?? "1";
                    }
                }
            }
        }

        #endregion

    }
}
