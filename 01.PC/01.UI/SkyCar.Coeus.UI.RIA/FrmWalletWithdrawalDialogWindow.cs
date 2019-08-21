using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.BLL.RIA;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.UIModel.Common.APModel;
using SkyCar.Coeus.UIModel.RIA.UIModel;
using SkyCar.Common.Utility;
using SkyCar.Framework.WindowUI;

namespace SkyCar.Coeus.UI.RIA
{
    /// <summary>
    /// 钱包提现DialogWindow
    /// </summary>
    public partial class FrmWalletWithdrawalDialogWindow : BaseForm
    {
        #region 全局变量

        /// <summary>
        /// 钱包提现BLL
        /// </summary>
        private WalletWithdrawalBLL _bll = new WalletWithdrawalBLL();

        /// <summary>
        /// 当前提现信息
        /// </summary>
        WalletWithdrawalUIModel _withdrawalInfo = new WalletWithdrawalUIModel();

        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters;

        #endregion

        #region 公共属性

        #endregion

        #region 系统事件

        /// <summary>
        /// FrmWalletWithdrawalDialogWindow构造方法
        /// </summary>
        public FrmWalletWithdrawalDialogWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// FrmWalletWithdrawalDialogWindow构造方法
        /// </summary>
        public FrmWalletWithdrawalDialogWindow(Dictionary<string, object> paramWindowParameters)
        {
            InitializeComponent();

            _viewParameters = paramWindowParameters;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmWalletWithdrawalDialogWindow_Load(object sender, EventArgs e)
        {
            #region 固定

            //初始化【详情】Tab内控件
            InitializeDetailTabControls();
            #endregion

            #region 处理参数

            if (_viewParameters == null)
            {
                return;
            }

            #region 钱包相关信息

            if (_viewParameters.ContainsKey(ComViewParamKey.DestModel.ToString()))
            {
                _withdrawalInfo = _viewParameters[ComViewParamKey.DestModel.ToString()] as WalletWithdrawalUIModel;

                if (_withdrawalInfo != null)
                {
                    //开户人
                    txtWal_CustomerName.Text = _withdrawalInfo.Wal_CustomerName;
                    //开户组织
                    txtWal_Org_Name.Text = _withdrawalInfo.Wal_Org_Name;
                    //钱包账号
                    txtWal_No.Text = _withdrawalInfo.Wal_No;
                    //可用余额
                    txtWal_AvailableBalance.Text = (_withdrawalInfo.Wal_AvailableBalance ?? 0).ToString();
                }
            }
            #endregion

            #endregion
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!ClientCheckForWithdrawal())
            {
                return;
            }
            SetCardCtrlsToDetailDS();
            bool saveResult = _bll.SaveDetailDS(_withdrawalInfo);
            if (!saveResult)
            {
                //提现失败
                MessageBoxs.Show(Trans.RIA, ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //提现成功
            MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.WITHDRAWCASH }), MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 自定义方法

        /// <summary>
        /// 初始化【详情】Tab内控件
        /// </summary>
        private void InitializeDetailTabControls()
        {
            //开户人姓名
            txtWal_CustomerName.Clear();
            //开户组织
            txtWal_Org_Name.Clear();
            //钱包账号
            txtWal_No.Clear();
            //本次提现金额
            numThisWithdrawalAmount.Value = null;
            //可用余额
            txtWal_AvailableBalance.Clear();
            //备注
            txtWal_Remark.Clear();
        }

        /// <summary>
        /// 前端检查-提现
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForWithdrawal()
        {
            //验证密码
            if (string.IsNullOrEmpty(txtWal_TradingPassword.Text.Trim()))
            {
                MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.E_0001, new object[] { MsgParam.PASSWORD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (_withdrawalInfo.Wal_TradingPassword != CryptoHelp.EncodeToMD5(txtWal_TradingPassword.Text.Trim()))
            {
                //密码错误
                MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.E_0011, new object[] { MsgParam.PASSWORD }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //验证钱包
            if (string.IsNullOrEmpty(txtWal_No.Text))
            {
                //没有获取到钱包，提现失败
                MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { MsgParam.WALLET, SystemActionEnum.Name.WITHDRAWCASH }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            WalletInfoUIModel curWallet = BLLCom.GetWalletByWalletNo(txtWal_No.Text);
            if (string.IsNullOrEmpty(curWallet.Wal_ID)
                || string.IsNullOrEmpty(curWallet.Wal_No))
            {
                //没有获取到钱包，提现失败
                MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.W_0024, new object[] { MsgParam.WALLET, SystemActionEnum.Name.WITHDRAWCASH }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //验证本次提现金额
            if (numThisWithdrawalAmount.Value == null
                || Convert.ToDecimal(numThisWithdrawalAmount.Value) == 0)
            {
                //本次提现金额不能为零！
                MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { MsgParam.WITHDRAWALAMOUNT_CANNOTZERO }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (curWallet.Wal_AvailableBalance < Convert.ToDecimal(numThisWithdrawalAmount.Value))
            {
                //提现金额大于钱包可用余额，不能提现！
                MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.E_0017, new object[] { "提现金额大于钱包可用余额", SystemActionEnum.Name.WITHDRAWCASH }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //确定要提现吗？
            DialogResult dialogResult = MessageBoxs.Show(Trans.RIA, ToString(), MsgHelp.GetMsg(MsgCode.I_0003, new object[] { SystemActionEnum.Name.WITHDRAWCASH }), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
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
            //本次提现金额
            _withdrawalInfo.ThisWithdrawalAmount = Convert.ToDecimal(numThisWithdrawalAmount.Value?.ToString() == "" ? "0" : numThisWithdrawalAmount.Value);
            //备注
            _withdrawalInfo.Wal_Remark = txtWal_Remark.Text.Trim();
            //钱包可用余额
            _withdrawalInfo.Wal_AvailableBalance = _withdrawalInfo.Wal_AvailableBalance - (_withdrawalInfo.ThisWithdrawalAmount ?? 0);
        }

        #endregion

    }
}
