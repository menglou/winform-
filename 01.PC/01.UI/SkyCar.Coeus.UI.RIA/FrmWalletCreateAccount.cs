using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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
using SkyCar.Coeus.UIModel.Common.APModel;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Coeus.UIModel.RIA.QCModel;
using SkyCar.Coeus.UIModel.RIA.UIModel;
using SkyCar.Common.Utility;

namespace SkyCar.Coeus.UI.RIA
{
    /// <summary>
    /// 钱包开户
    /// </summary>
    public partial class FrmWalletCreateAccount : BaseFormCardList<WalletCreateAccountUIModel, WalletCreateAccountQCModel, MDLEWM_Wallet>
    {
        #region 全局变量

        /// <summary>
        /// 钱包开户BLL
        /// </summary>
        private WalletCreateAccountBLL _bll = new WalletCreateAccountBLL();

        /// <summary>
        /// 页面参数字典
        /// </summary>
        private Dictionary<string, object> _viewParameters;

        #region 下拉框数据源

        /// <summary>
        /// 所有人类别
        /// </summary>
        List<ComComboBoxDataSourceTC> _ownerTypeList = new List<ComComboBoxDataSourceTC>();
        /// <summary>
        /// 汽修商户
        /// </summary>
        List<MDLPIS_AutoFactoryCustomer> _autoFactoryList = new List<MDLPIS_AutoFactoryCustomer>();
        /// <summary>
        /// 用户数据源
        /// </summary>
        List<MDLSM_User> _autoUserList = new List<MDLSM_User>();
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
        /// FrmWalletCreateAccount构造方法
        /// </summary>
        public FrmWalletCreateAccount()
        {
            InitializeComponent();
        }
        /// <summary>
        /// FrmWalletCreateAccount构造方法
        /// </summary>
        public FrmWalletCreateAccount(Dictionary<string, object> paramWindowParameters)
        {
            InitializeComponent();

            _viewParameters = paramWindowParameters;
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmWalletCreateAccount_Load(object sender, EventArgs e)
        {
            #region 固定
            //基类.工具栏（动作，导航）
            base.ToolBarActionAndNavigate = toolBarActionAndNavigate;
            //工具栏（动作）单击事件
            this.toolBarActionAndNavigate.ToolClick += new ToolClickEventHandler(base.toolBarActionAndNavigate_ToolClick);

            _tempAllClientList = BLLCom.GetAllCustomerList(LoginInfoDAX.OrgID);
            _clientList = new List<ComClientUIModel>();
            foreach (var loopClient in _tempAllClientList)
            {
                if (loopClient.ClientType == AmountTransObjectTypeEnum.Name.REGULARCUSTOMER)
                {
                    loopClient.ClientType = CustomerTypeEnum.Name.PTKH;
                    _clientList.Add(loopClient);
                }
                else if (loopClient.ClientType == AmountTransObjectTypeEnum.Name.GENERALAUTOFACTORY)
                {
                    loopClient.ClientType = CustomerTypeEnum.Name.YBQXSH;
                    _clientList.Add(loopClient);
                }
                else if (loopClient.ClientType == AmountTransObjectTypeEnum.Name.PLATFORMAUTOFACTORY)
                {
                    loopClient.ClientType = CustomerTypeEnum.Name.PTNQXSH;
                    _clientList.Add(loopClient);
                }
            }
            mcbClientName.ExtraDisplayMember = "ClientType";
            mcbClientName.DisplayMember = "ClientName";
            mcbClientName.ValueMember = "ClientID";
            mcbClientName.DataSource = _clientList;

            //初始化【详情】Tab内控件
            InitializeDetailTabControls();
            //根据选中的Tab，设置动作按钮[是否可用]（在系统权限的基础上进行控制）
            base.SetActionEnableBySelectedTab(SysConst.EN_LIST);
            #endregion

            //[保存]可用
            SetActionEnable(SystemActionEnum.Code.SAVE, true);
            //导航按钮[充值]不可用
            SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.DEPOSITMONEY, true, false);

            #region 处理参数

            if (_viewParameters == null)
            {
                return;
            }

            #region 客户信息

            if (_viewParameters.ContainsKey(ComViewParamKey.CustomerInfo.ToString()))
            {
                CustomerQueryUIModel resultWalletInfo = _viewParameters[ComViewParamKey.CustomerInfo.ToString()] as CustomerQueryUIModel;

                if (resultWalletInfo != null)
                {
                    //组织
                    txtWal_Org_ID.Text = LoginInfoDAX.OrgID;
                    txtWal_Org_Name.Text = LoginInfoDAX.OrgShortName;
                    //所有人类别
                    mcbWal_OwnerTypeName.SelectedText = resultWalletInfo.CustomerType;
                    //开户人
                    mcbClientName.SelectedValue = resultWalletInfo.CustomerID;
                    //汽修商户
                    mcbAutoFactoryName.SelectedValue = resultWalletInfo.AutoFactoryCode;
                    //汽修商户组织编码
                    txtWal_AutoFactoryOrgCode.Text = resultWalletInfo.AutoFactoryOrgCode;
                }
            }
            #endregion

            #endregion

            SetCardCtrlsToDetailDS();
            //将最新的值Copy到初始UIModel
            AcceptUIModelChanges();
        }

        /// <summary>
        /// 开户人发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mcbClientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //所有人类别为{普通客户}、{一般汽修商}的场合，不显示[汽修商户]
            panAutoFactoryName.Visible = false;
            mcbAutoFactoryName.Visible = false;
            mcbAutoFactoryName.Clear();
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
                    this.mcbWal_OwnerTypeName.SelectedText = selectedClient.ClientType;
                    if (selectedClient.ClientType == CustomerTypeEnum.Name.PTNQXSH)
                    {
                        //所有人类别为{平台内汽修商}的场合，显示[汽修商户]
                        panAutoFactoryName.Visible = true;
                        mcbAutoFactoryName.Visible = true;
                        mcbAutoFactoryName.SelectedValue = selectedClient.MerchantCode;
                        txtWal_AutoFactoryOrgCode.Text = selectedClient.OrgCode;
                    }
                }
            }

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

            //设置详情页面控件的是否可编辑
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
            bool saveResult = _bll.SaveDetailDS(DetailDS);
            if (!saveResult)
            {
                //保存失败
                MessageBoxs.Show(Trans.RIA, this.ToString(), _bll.ResultMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //保存成功
            MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.SAVE }), MessageBoxButtons.OK, MessageBoxIcon.Information);

            //将DetailDS数据赋值给【详情】Tab内的对应控件
            SetDetailDSToCardCtrls();
            //将最新的值Copy到初始UIModel
            this.AcceptUIModelChanges();

            //设置详情页面控件的是否可编辑
            SetDetailControl();
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
            if (string.IsNullOrEmpty(txtWal_No.Text))
            {
                //钱包账号为空，充值失败
                MessageBoxs.Show(Trans.SD, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0016, new object[] { SystemTableColumnEnums.EWM_Wallet.Name.Wal_No, SystemActionEnum.Name.RECHARGE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //待充值的钱包
            WalletInfoUIModel walletToDeposit = BLLCom.GetWalletByWalletNo(txtWal_No.Text);
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
            //钱包ID
            txtWal_ID.Clear();
            //组织ID
            txtWal_Org_ID.Clear();
            //组织名称
            txtWal_Org_Name.Clear();
            //钱包账号
            txtWal_No.Clear();
            //钱包所有人类别
            mcbWal_OwnerTypeName.Clear();
            //汽修商户
            mcbAutoFactoryName.Clear();
            //开户人ID
            mcbClientName.Clear();
            //开户人姓名
            mcbClientName.Clear();
            //汽修商户组织编码
            txtWal_AutoFactoryOrgCode.Clear();
            //交易密码
            txtWal_TradingPassword.Clear();
            //确认密码
            txtConfirmTradingPassword.Clear();
            //推荐员工
            mcbWal_RecommendEmployee.Clear();
            //备注
            txtWal_Remark.Clear();
            //版本号
            txtWal_VersionNo.Clear();
            #endregion

            #region 初始化下拉框
            //所有人类别
            _ownerTypeList = EnumDAX.GetEnumForComboBoxWithCodeText(EnumKey.CustomerType);
            mcbWal_OwnerTypeName.DisplayMember = SysConst.EN_TEXT;
            mcbWal_OwnerTypeName.ValueMember = SysConst.EN_Code;
            mcbWal_OwnerTypeName.DataSource = _ownerTypeList;

            //汽修商户名称
            _autoFactoryList = CacheDAX.Get(CacheDAX.ConfigDataKey.ARMerchant) as List<MDLPIS_AutoFactoryCustomer>;
            if (_autoFactoryList != null)
            {
                var autoFactoryCustomerList = _autoFactoryList.Where(x => x.AFC_IsPlatform == true).ToList();
                mcbAutoFactoryName.DisplayMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Name;
                mcbAutoFactoryName.ValueMember = SystemTableColumnEnums.PIS_AutoFactoryCustomer.Code.AFC_Code;
                mcbAutoFactoryName.DataSource = autoFactoryCustomerList;
            }

            //用户名
            _autoUserList = CacheDAX.Get(CacheDAX.ConfigDataKey.SystemUser) as List<MDLSM_User>;
            mcbWal_RecommendEmployee.DisplayMember = SystemTableColumnEnums.SM_User.Code.User_Name;
            mcbWal_RecommendEmployee.ValueMember = SystemTableColumnEnums.SM_User.Code.User_ID;
            mcbWal_RecommendEmployee.DataSource = _autoUserList;

            #endregion

            //组织为{当前登录组织}
            txtWal_Org_ID.Text = LoginInfoDAX.OrgID;
            txtWal_Org_Name.Text = LoginInfoDAX.OrgShortName;
        }

        /// <summary>
        /// 前端检查-保存
        /// </summary>
        /// <returns></returns>
        private bool ClientCheckForSave()
        {
            //验证所有人类别
            if (string.IsNullOrEmpty(mcbWal_OwnerTypeName.SelectedText))
            {
                MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.OWNER_TYPE }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //所有人类别为{平台内汽修商}的场合，验证汽修商户和汽修商组织
            if (mcbWal_OwnerTypeName.SelectedText == CustomerTypeEnum.Name.PTNQXSH)
            {
                if (string.IsNullOrEmpty(mcbAutoFactoryName.SelectedValue)
                    || string.IsNullOrEmpty(mcbAutoFactoryName.SelectedText))
                {
                    MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.AUTOFACTORY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (string.IsNullOrEmpty(txtWal_AutoFactoryOrgCode.Text))
                {
                    MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.AUTOFACTORY_ORG }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            //验证开户人
            if (string.IsNullOrEmpty(mcbClientName.SelectedText))
            {
                MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0013, new object[] { MsgParam.ACCOUNT_BY }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            
            //验证交易密码
            if (string.IsNullOrEmpty(txtWal_TradingPassword.Text) && string.IsNullOrEmpty(txtConfirmTradingPassword.Text))
            {
                //获取系统配置中的电子钱包初始密码
                var resultParameters = CacheDAX.Get(CacheDAX.CacheKey.S1006) as List<MDLSM_Parameter>;
                if (resultParameters != null && resultParameters.Count > 0)
                {
                    txtConfirmTradingPassword.Text = txtWal_TradingPassword.Text = CryptoHelp.EncodeToMD5(resultParameters[0].Para_Value1);
                }
                else
                {
                    MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "1006(钱包初始密码)未在SM_Parameter表中配置" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            else
            {
                //交易密码和确认密码不一致
                if (txtWal_TradingPassword.Text != txtConfirmTradingPassword.Text)
                {
                    MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "交易密码和确认密码不一致" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (txtWal_TradingPassword.Text != null)
                {
                    //加密
                    txtWal_TradingPassword.Text = CryptoHelp.EncodeToMD5(txtWal_TradingPassword.Text);
                    txtConfirmTradingPassword.Text = txtWal_TradingPassword.Text;
                }
                else
                {
                    //获取系统配置中的电子钱包初始密码
                    var resultParameters = CacheDAX.Get(CacheDAX.CacheKey.S1006) as List<MDLSM_Parameter>;
                    if (resultParameters != null && resultParameters.Count > 0)
                    {
                        txtWal_TradingPassword.Text = CryptoHelp.EncodeToMD5(resultParameters[0].Para_Value1);
                        txtConfirmTradingPassword.Text = txtWal_TradingPassword.Text;
                    }
                    else
                    {
                        MessageBoxs.Show(Trans.RIA, this.ToString(), MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "1006(钱包初始密码)未在SM_Parameter表中配置" }), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }

            return true;
        }
       
        /// <summary>
        /// 将【详情】Tab内控件的值赋值给基类的DetailDS
        /// </summary>
        private void SetCardCtrlsToDetailDS()
        {
            DetailDS = new WalletCreateAccountUIModel()
            {
                //钱包ID
                Wal_ID = txtWal_ID.Text.Trim(),
                //组织ID
                Wal_Org_ID = txtWal_Org_ID.Text.Trim(),
                //组织名称
                Wal_Org_Name = txtWal_Org_Name.Text.Trim(),
                //钱包账号
                Wal_No = txtWal_No.Text.Trim(),
                //钱包来源类型编码
                Wal_SourceTypeCode = WalletSourceTypeEnum.Code.XZ,
                //钱包来源类型名称
                Wal_SourceTypeName = WalletSourceTypeEnum.Name.XZ,
                //钱包所有人类别编码
                Wal_OwnerTypeCode = mcbWal_OwnerTypeName.SelectedValue,
                //钱包所有人类别名称
                Wal_OwnerTypeName = mcbWal_OwnerTypeName.SelectedText,
                //开户人ID
                Wal_CustomerID = mcbClientName.SelectedValue,
                //开户人姓名
                Wal_CustomerName = mcbClientName.SelectedText,
                //汽修商户编码
                Wal_AutoFactoryCode = mcbAutoFactoryName.SelectedValue,
                //汽修商户名称
                AutoFactoryName = mcbAutoFactoryName.SelectedText,
                //汽修商户组织编码
                Wal_AutoFactoryOrgCode = txtWal_AutoFactoryOrgCode.Text.Trim(),
                //交易密码
                Wal_TradingPassword = txtWal_TradingPassword.Text.Trim(),
                //推荐员工ID
                Wal_RecommendEmployeeID = mcbWal_RecommendEmployee.SelectedValue,
                //推荐员工
                Wal_RecommendEmployee = mcbWal_RecommendEmployee.SelectedText,
                //开户组织ID
                Wal_CreatedByOrgID = txtWal_Org_ID.Text.Trim(),
                //开户组织名称
                Wal_CreatedByOrgName = txtWal_Org_Name.Text.Trim(),
                //生效时间
                Wal_EffectiveTime = BLLCom.GetCurStdDatetime(),
                //钱包状态编码
                Wal_StatusCode = WalletStatusEnum.Code.ZC,
                //钱包状态名称
                Wal_StatusName = WalletStatusEnum.Name.ZC,
                //备注
                Wal_Remark = txtWal_Remark.Text.Trim(),
                //有效
                Wal_IsValid = true,
                //创建人
                Wal_CreatedBy = LoginInfoDAX.UserName,
                //创建时间
                Wal_CreatedTime = BLLCom.GetCurStdDatetime(),
                //修改人
                Wal_UpdatedBy = LoginInfoDAX.UserName,
                //修改时间
                Wal_UpdatedTime = BLLCom.GetCurStdDatetime(),
                //版本号
                Wal_VersionNo = Convert.ToInt64(txtWal_VersionNo.Text.Trim() == "" ? "1" : txtWal_VersionNo.Text.Trim()),

                //汽修商户组织名称
                AutoFactoryOrgName = mcbClientName.SelectedText,
            };
        }

        /// <summary>
        /// 将DetailDS数据赋值给【详情】Tab内的对应控件
        /// </summary>
        private void SetDetailDSToCardCtrls()
        {
            //钱包ID
            txtWal_ID.Text = DetailDS.Wal_ID;
            //组织ID
            txtWal_Org_ID.Text = DetailDS.Wal_Org_ID;
            //组织名称
            txtWal_Org_Name.Text = DetailDS.Wal_Org_Name;
            //钱包账号
            txtWal_No.Text = DetailDS.Wal_No;
            //钱包所有人类别
            mcbWal_OwnerTypeName.SelectedValue = DetailDS.Wal_OwnerTypeCode;
            //开户人
            mcbClientName.SelectedValue = DetailDS.Wal_CustomerID;
            //汽修商户
            mcbAutoFactoryName.SelectedValue = DetailDS.Wal_AutoFactoryCode;
            //汽修商户组织编码
            txtWal_AutoFactoryOrgCode.Text = DetailDS.Wal_AutoFactoryOrgCode;
            //交易密码
            txtWal_TradingPassword.Text = DetailDS.Wal_TradingPassword;
            //确认密码
            //txtConfirmTradingPassword.Text = DetailDS.pa;
            //推荐员工
            mcbWal_RecommendEmployee.SelectedValue = DetailDS.Wal_RecommendEmployeeID;
            //备注
            txtWal_Remark.Text = DetailDS.Wal_Remark;
            //版本号
            txtWal_VersionNo.Text = DetailDS.Wal_VersionNo?.ToString() ?? "1";
        }
       
        /// <summary>
        /// 设置详情页面控件的是否可编辑
        /// </summary>
        private void SetDetailControl()
        {
            if (string.IsNullOrEmpty(txtWal_ID.Text))
            {
                //未保存的场合，[保存]可用
                SetActionEnable(SystemActionEnum.Code.SAVE, true);

                //详情可编辑
                ////所有人类别
                //cbWal_OwnerTypeName.Enabled = true;
                ////汽修商户
                //mcbAutoFactoryName.Enabled = true;
                //开户人姓名
                mcbClientName.Enabled = true;
                //交易密码
                txtWal_TradingPassword.Enabled = true;
                //确认密码
                txtConfirmTradingPassword.Enabled = true;
                //推荐员工
                mcbWal_RecommendEmployee.Enabled = true;
                //备注
                txtWal_Remark.Enabled = true;
                //导航按钮[充值]不可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.DEPOSITMONEY, true, false);
            }
            else
            {
                //已保存的场合，[保存]不可用
                SetActionEnable(SystemActionEnum.Code.SAVE, false);

                //详情不可编辑
                ////所有人类别
                //cbWal_OwnerTypeName.Enabled = false;
                ////汽修商户
                //mcbAutoFactoryName.Enabled = false;
                //开户人姓名
                mcbClientName.Enabled = false;
                //交易密码
                txtWal_TradingPassword.Enabled = false;
                //确认密码
                txtConfirmTradingPassword.Enabled = false;
                //推荐员工
                mcbWal_RecommendEmployee.Enabled = false;
                //备注
                txtWal_Remark.Enabled = false;

                //导航按钮[充值]可用
                SetNavigateVisiableAndEnable(SystemNavigateEnum.Code.DEPOSITMONEY, true, true);
            }
        }
        #endregion

    }
}
