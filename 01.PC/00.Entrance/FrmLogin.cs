using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Infragistics.Win;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Common;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Common.Utility;
using SkyCar.Framework.WindowUI;

namespace SkyCar.Coeus.Ult.Entrance
{
    /// <summary>
    /// 考伊斯(Coeus)智力之神。
    /// </summary>
    public partial class FrmLogin : BaseForm
    {
        #region 变量
        /// <summary>
        /// 登录信息DAX
        /// </summary>
        private LoginInfoDAX _loginDAX = new LoginInfoDAX();
        /// <summary>
        /// 窗体是否激活
        /// </summary>
        private bool _isActivated = false;
        /// <summary>
        /// 消息【固定】
        /// </summary>
        StringBuilder _message = new StringBuilder();
        /// <summary>
        /// 焦点控件【固定】
        /// </summary>
        Control _focusControl = null;
        /// <summary>
        /// 业务逻辑对象【固定】
        /// </summary>
        LoginBLL _bll = new LoginBLL();
        private WebUtils _webUtils = new WebUtils();
        /// <summary>
        /// 版权描述
        /// </summary>
        public string CopyRightDesc = string.Empty;
        /// <summary>
        /// 记住登陆用户列表
        /// </summary>
        private ObservableCollection<LoginUIModel> RembLoginUserList;
        /// <summary>
        /// 具有相同名字的用户
        /// </summary>
        public ObservableCollection<MDLSM_User> SameUserNameList = new ObservableCollection<MDLSM_User>();
        /// <summary>
        /// 组织列表数据源
        /// </summary>
        private List<MDLSM_Organization> _organizationList = new List<MDLSM_Organization>();

        /// <summary>
        /// 鼠标移动位置变量
        /// </summary>
        Point mouseOff;
        /// <summary>
        /// 标签(是否为左键)
        /// </summary>
        bool leftFlag;

        #endregion

        #region 构造方法
        public FrmLogin()
        {
            InitializeComponent();
            this.txtUserName.GotFocus += TxtUserName_GotFocus;
            //if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"pic\login.png"))
            //{
            //    //初始化调用不规则窗体生成代码
            //    BitmapRegion.CreateControlRegion(this, new Bitmap(@"pic\login.png"));
            //}
            this.lblVersionNo.Text = SysConst.VersionNo;

            if (!Directory.Exists(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + LocalConfigFileConst.FileFolderName.LocalConfig + "\\" + LocalConfigFileConst.FileFolderName.LoginInfo))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + LocalConfigFileConst.FileFolderName.LocalConfig + "\\" + LocalConfigFileConst.FileFolderName.LoginInfo);
            }
            if (!Directory.Exists(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + LocalConfigFileConst.FileFolderName.LocalConfig + "\\" + LocalConfigFileConst.FileFolderName.SysConData))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + LocalConfigFileConst.FileFolderName.LocalConfig + "\\" + LocalConfigFileConst.FileFolderName.SysConData);
            }
            if (!Directory.Exists(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + LocalConfigFileConst.FileFolderName.LocalFileBase + "\\" + LocalConfigFileConst.FileFolderName.RecordFile))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + LocalConfigFileConst.FileFolderName.LocalFileBase + "\\" + LocalConfigFileConst.FileFolderName.RecordFile);
            }

            #region 获取版权说明
            //初始化系统
            ThreadPool.QueueUserWorkItem(new WaitCallback(InitCopyRightDesc));
            #endregion
        }

        private void TxtUserName_GotFocus(object sender, EventArgs e)
        {
            this.txtUserName.Select(this.txtUserName.Text.Length, 0);
        }

        public void InitCopyRightDesc(object paramObj)
        {
            MDLSM_Parameter resultParameter = new MDLSM_Parameter();
            _bll.QueryForObject<MDLSM_Parameter, MDLSM_Parameter>(new MDLSM_Parameter
            {
                WHERE_Para_Code1 = CacheDAX.CacheKey.S0003.ToString().Replace("S", "")
            }, resultParameter);
            CopyRightDesc = string.IsNullOrEmpty(resultParameter.Para_Value1) ? "Copyright@2014-2017 无锡云车物联网科技有限公司版权所有  客服热线：0510-68566886" : resultParameter.Para_Value1;
            SystemConfigInfo.CopyRightDesc = CopyRightDesc;
            if (this.lblCopyRight.InvokeRequired)
            {
                CopyRightDelegate md = new CopyRightDelegate(this.InitCopyRightDesc);
                this.Invoke(md, new object[] { CopyRightDesc });
            }
            else

            {
                this.lblCopyRight.Text = CopyRightDesc;
            }
        }

        private delegate void CopyRightDelegate(string paramCopyRightDesc);

        #endregion

        #region 系统事件
        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                //用户工号
                txtEMPNO.Clear();
                //用户密码
                txtUserPwd.Clear();
                //焦点
                this.txtUserName.Focus();

                SystemConfigInfo.ConnectServer = true;
                //加载记住的用户列表
                RembLoginUserList = new ObservableCollection<LoginUIModel>();

                //获取记住的用户名
                StringBuilder tempRembLoginUserName = new StringBuilder(255);
                SystemFunction.GetPrivateProfileString(LocalConfigFileConst.SelectionName.ClientInfo,
                    LocalConfigFileConst.KeyName.RembLoginUserNames, string.Empty, tempRembLoginUserName, 255,
                    LocalConfigFileConst.ConfigFilePath.LocalLoginInfo);

                //获取最后一次登录的用户名
                StringBuilder latestLoginUserName = new StringBuilder(255);
                SystemFunction.GetPrivateProfileString(LocalConfigFileConst.SelectionName.ClientInfo,
                    LocalConfigFileConst.KeyName.LastestUserName, string.Empty, latestLoginUserName, 255,
                    LocalConfigFileConst.ConfigFilePath.LocalLoginInfo);

                if (tempRembLoginUserName.ToString().Length > 0)
                {
                    string[] rembUserNameList = tempRembLoginUserName.ToString().Split(SysConst.SPLIT_CHAR);
                    foreach (string userName in rembUserNameList)
                    {
                        LoginUIModel rembUser = new LoginUIModel
                        {
                            User_Name = userName,
                            UserName_ShortSpellCode = ChineseSpellCode.GetShortSpellCode(userName),
                            UserName_FullSpellCode = ChineseSpellCode.GetFullSpellCode(userName)
                        };
                        RembLoginUserList.Add(rembUser);
                    }
                    //默认为最后一次登录的用户名
                    txtUserName.Text = latestLoginUserName.ToString();
                    txtUserName_Leave(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发现异常:" + ex.Message);
                return;
            }

            //初始化配置信息
            ThreadPool.QueueUserWorkItem(new WaitCallback(InitConfigInfo));

            //检查新版本
            ThreadPool.QueueUserWorkItem(new WaitCallback(GetLastVersionNo));
        }

        /// <summary>
        /// 获取允许升级的最新版本号
        /// </summary>
        private void GetLastVersionNo(object paramObj)
        {
            try
            {
                string localConfigFilePath = LocalConfigFileConst.ConfigFilePath.SoftVersionNo.Replace(SysConst.PH_SOFTVERSIONNO, null);
                if (File.Exists(localConfigFilePath))
                {
                    //本地版本号
                    StringBuilder tempSoftVersionNo = new StringBuilder(255);
                    SystemFunction.GetPrivateProfileString(LocalConfigFileConst.SelectionName.SoftInfo,
                        LocalConfigFileConst.KeyName.SoftVersionNo, string.Empty, tempSoftVersionNo, 255,
                        localConfigFilePath);
                    string argsPostData = string.Format(ApiParameter.BF0006, ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE], SysConst.ProductCode);
                    string strApiData = _webUtils.DoPost(ApiUrl.BF0006Url, argsPostData);
                    //获取平台版本号
                    var jsonResult = (JObject)JsonConvert.DeserializeObject(strApiData);
                    if (jsonResult != null)
                    {
                        #region 判断JSON的元素
                        if (jsonResult[SysConst.EN_RESULTCODE] == null)
                        {
                            MessageBox.Show("接口返回的ResultCode为空");
                            return;
                        }
                        if (jsonResult["ML_LastVersionNo"] == null)
                        {
                            MessageBox.Show("接口返回的ML_LastVersionNo为空");
                            return;
                        }
                        #endregion

                        if (jsonResult[SysConst.EN_RESULTCODE].ToString().Equals(SysConst.EN_I0001))
                        {
                            var argsOpenVersionNo = jsonResult["ML_LastVersionNo"];
                            if ((!string.IsNullOrEmpty(argsOpenVersionNo.ToString()) &&
                                 (!string.IsNullOrEmpty(localConfigFilePath))))
                            {
                                string[] plateformVersionChars = argsOpenVersionNo.ToString().Split('.');
                                string[] localVersionChars = tempSoftVersionNo.ToString().Split('.');
                                for (int i = 0; i < plateformVersionChars.Count(); i++)
                                {
                                    int plateformNum;
                                    int localNum = 0;
                                    int.TryParse(plateformVersionChars[i], out plateformNum);
                                    if (i <= localVersionChars.Count() - 1)
                                    {
                                        int.TryParse(localVersionChars[i], out localNum);
                                    }
                                    if (plateformNum > localNum)
                                    {
                                        DialogResult foundNewVersion = MessageBox.Show("发现新版本，是否更新？", "提示",
                                            MessageBoxButtons.OKCancel,
                                            MessageBoxIcon.Information);
                                        if (foundNewVersion == DialogResult.OK)
                                        {
                                            string currentDirectory =
                                                Process.GetCurrentProcess()
                                                    .MainModule.FileName.Replace(
                                                        Process.GetCurrentProcess().MainModule.ModuleName, string.Empty);
                                            //是否关闭主程序
                                            bool needKillProcess = false;
                                            Process[] processList = Process.GetProcesses();

                                            #region 调试用

                                            //输出各项进程
                                            //string tempProcessList = String.Empty;
                                            //foreach (var loopProcess in processList)
                                            //{
                                            //    tempProcessList += loopProcess.ProcessName + "\n";
                                            //}

                                            //FileStream fs = new FileStream("E:\\fileProcessList.txt", FileMode.Create);
                                            ////获得字节数组
                                            //byte[] data = System.Text.Encoding.Default.GetBytes(tempProcessList);
                                            ////开始写入
                                            //fs.Write(data, 0, data.Length);
                                            ////清空缓冲区、关闭流
                                            //fs.Flush();
                                            //fs.Close();

                                            #endregion

                                            if (processList.Any(
                                                    process => process.ProcessName.ToUpper().Equals(SysConst.EntranceFileName)
                                                               &&
                                                               process.MainModule.FileName.Replace(
                                                                   process.MainModule.ModuleName, string.Empty)
                                                                   .Equals(currentDirectory)))
                                            {
                                                DialogResult isKillProcess = MessageBox.Show("系统正在运行，关闭后升级吗?", "提示",
                                                    MessageBoxButtons.OKCancel,
                                                    MessageBoxIcon.Information);
                                                needKillProcess = isKillProcess == DialogResult.OK ? true : false;
                                            }
                                            if (needKillProcess)
                                            {
                                                Process.Start(
                                                    Environment.CurrentDirectory + "\\SkyCar.Coeus.UpdateClient.exe",
                                                    argsOpenVersionNo.ToString());
                                                foreach (Process process in processList)
                                                {
                                                    if (process.ProcessName.ToUpper().Equals(SysConst.EntranceFileName)
                                                        &&
                                                        process.MainModule.FileName.Replace(process.MainModule.ModuleName,
                                                            string.Empty).Equals(currentDirectory))
                                                    {
                                                        process.Kill();
                                                        process.Close();
                                                    }
                                                }
                                            }

                                            if (!needKillProcess)
                                            {
                                                Process.Start(
                                                    Environment.CurrentDirectory + "\\SkyCar.Coeus.UpdateClient.exe",
                                                    argsOpenVersionNo.ToString());
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("发现错误ResultCode：", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("获取Coeus最新版本号失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发现异常:" + ex.Message);
                return;
            }
        }

        /// <summary>
        /// 初始化配置信息
        /// </summary>
        private void InitConfigInfo(object paramObj)
        {
            try
            {
                LoginInfoDAX.MCTCode = ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE];
                LoginInfoDAX.MCTName = ConfigurationManager.AppSettings[AppSettingKey.MCT_NAME];
                LoginInfoDAX.SPCode = SysConst.ProductCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show("发现异常:" + ex.Message);
                return;
            }
        }

        private void txtUserPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UserLogin();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            UserLogin();
        }
        private void lblClose_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        #endregion

        #region 自定义方法

        #region 登录事件新版本
        private void UserLogin()
        {
            string userName = this.txtUserName.Text.Trim();
            string empNo = this.txtEMPNO.Text.Trim();
            string orgId = comboxOrg.SelectedItem == null ? string.Empty : ((MDLSM_Organization)comboxOrg.SelectedItem).Org_ID;
            string passWord = this.txtUserPwd.Text.Trim();

            try
            {
                #region 验证信息
                if (string.IsNullOrEmpty(userName) || "请输入姓名".Equals(userName))
                {
                    MessageBox.Show("请输入用户姓名", "登录验证", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUserName.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(empNo) || "请输入工号".Equals(empNo))
                {
                    MessageBox.Show("请选择工号", "登录验证", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtEMPNO.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(orgId))
                {
                    MessageBox.Show("请选择组织", "登录验证", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    comboxOrg.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(passWord) || "请输入密码".Equals(passWord))
                {
                    MessageBox.Show("请输入密码", "登录验证", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUserPwd.Focus();
                    txtUserPwd.Text = string.Empty;
                    return;
                }
                if (userName != SysConst.SUPER_ADMIN)
                {
                    //if (!CheckUserLicense())
                    //{
                    //    return;
                    //}
                }
                #endregion

                LoginUIModel argsLoginUiModel = new LoginUIModel
                {
                    User_Name = userName,
                    User_EMPNO = empNo,
                    UO_Org_ID = orgId
                };
                if (txtUserName.Text.Trim() != SysConst.SUPER_ADMIN)
                {
                    argsLoginUiModel.User_Password = CryptoHelp.EncodeToMD5(passWord);
                }
                else
                {
                    argsLoginUiModel.User_ID = SysConst.SUPER_ADMIN;

                    #region 验证登陆"SuperAdmin"
                    var requestDataBf0005 = string.Format(ApiParameter.BF0005, LoginInfoDAX.MCTCode, SysConst.ProductCode, CryptoHelp.EncodeToMD5(txtUserPwd.Text.Trim()));
                    var postResut = _webUtils.DoPost(ApiUrl.BF0005Url, requestDataBf0005);
                    var jsonResult = (JObject)JsonConvert.DeserializeObject(postResut);
                    if (jsonResult == null)
                    {
                        MessageBoxs.Show(Trans.COM, this.ToString(), "jsonResult为空", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (jsonResult[SysConst.EN_RESULTCODE] == null)
                    {
                        MessageBoxs.Show(Trans.COM, this.ToString(), "jsonResult[ResultCode]为空", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (!jsonResult[SysConst.EN_RESULTCODE].ToString().Equals(SysConst.EN_I0001))
                    {
                        string strErrorMessage = jsonResult["ResultMsg"] == null ? "" : jsonResult["ResultMsg"].ToString();
                        MessageBoxs.Show(Trans.COM, this.ToString(), strErrorMessage, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    #endregion
                }

                #region 获取配置信息
                SystemConfigInfoUIModel systemConfigInfo = _bll.GetSystemConfigInfo(argsLoginUiModel);
                if (systemConfigInfo != null)
                {
                    if (systemConfigInfo.User_Name != SysConst.SUPER_ADMIN)
                    {
                        MDLSM_UserOrg resultUserOrg = new MDLSM_UserOrg();
                        _bll.QueryForObject<MDLSM_UserOrg, MDLSM_UserOrg>(
                            new MDLSM_UserOrg
                            {
                                UO_User_ID = systemConfigInfo.User_ID,
                                UO_Org_ID = systemConfigInfo.Org_ID,
                                UO_IsValid = true
                            }, resultUserOrg);
                        if (string.IsNullOrEmpty(resultUserOrg.UO_ID))
                        {
                            MessageBoxs.Show(Trans.COM, this.ToString(), "用户和组织不匹配,请确认商户编码并同步组织", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    else
                    {
                        MDLSM_Organization resultOrganization = new MDLSM_Organization();
                        _bll.QueryForObject<MDLSM_Organization, MDLSM_Organization>(
                            new MDLSM_Organization { Org_ID = systemConfigInfo.Org_ID, Org_IsValid = true },
                            resultOrganization);
                        if (string.IsNullOrEmpty(resultOrganization.Org_ID))
                        {
                            MessageBoxs.Show(Trans.COM, this.ToString(), "用户和组织不匹配,请确认商户编码并同步组织", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    LoginInfoDAX.IsSuperAdmin = systemConfigInfo.User_IsSuperAdmin;
                    LoginInfoDAX.UserID = systemConfigInfo.User_ID;
                    LoginInfoDAX.UserName = systemConfigInfo.User_Name;
                    LoginInfoDAX.Password = systemConfigInfo.User_Password;
                    LoginInfoDAX.UserPassword = CryptoHelp.EncodeToMD5(txtUserPwd.Text.Trim());
                    LoginInfoDAX.UserEMPNO = systemConfigInfo.User_EMPNO;
                    LoginInfoDAX.User_PrintTitlePrefix = systemConfigInfo.User_PrintTitlePrefix;
                    LoginInfoDAX.OrgID = systemConfigInfo.Org_ID;
                    LoginInfoDAX.OrgCode = systemConfigInfo.Org_Code;
                    LoginInfoDAX.OrgFullName = systemConfigInfo.Org_FullName;
                    LoginInfoDAX.OrgShortName = systemConfigInfo.Org_ShortName;
                    LoginInfoDAX.OrgAddress = systemConfigInfo.Org_Address;
                    LoginInfoDAX.OrgTEL = systemConfigInfo.Org_TEL;
                    LoginInfoDAX.OrgPhoneNo = systemConfigInfo.Org_PhoneNo;
                    LoginInfoDAX.OrgList = _organizationList.ToList();
                    SystemConfigInfo.SystemName = systemConfigInfo.SM_SystemName;
                    SystemConfigInfo.SystemCode = systemConfigInfo.SM_SystemCode;
                    SystemConfigInfo.Org_WechatAdress = systemConfigInfo.Org_WechatAdress;
                    SystemConfigInfo.OrgPlatformCode = systemConfigInfo.Org_PlatformCode;
                    PCConfigInfo.PhisicalMemorySize = LocalSystemHelper.GetPhisicalMemorySize();
                }
                else
                {
                    MessageBoxs.Show(Trans.COM, this.ToString(), "用户名、密码或组织错误，请重新输入", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                #endregion

                #region 对组织进行验证
                if (!string.IsNullOrEmpty(LoginInfoDAX.OrgCode))
                {
                    //根据商户编码和产品编码进行授权验证（商户，组织，产品）
                    var requestDataBf0007 = string.Format(ApiParameter.BF0007, LoginInfoDAX.MCTCode, SysConst.ProductCode, LoginInfoDAX.OrgCode);
                    var resutlBf0007 = _webUtils.DoPost(ApiUrl.BF0007Url, requestDataBf0007);
                    var jsonResult1 = (JObject)JsonConvert.DeserializeObject(resutlBf0007);
                    if (jsonResult1 == null)
                    {
                        MessageBoxs.Show(Trans.COM, this.ToString(), "jsonResult1为空", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (jsonResult1[SysConst.EN_RESULTCODE] == null)
                    {
                        MessageBoxs.Show(Trans.COM, this.ToString(), "jsonResult1[ResultCode]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (!jsonResult1[SysConst.EN_RESULTCODE].ToString().Equals(SysConst.EN_I0001))
                    {
                        string strErrorMessage = jsonResult1["ResultMsg"] == null ? "" : jsonResult1["ResultMsg"].ToString();
                        MessageBox.Show(strErrorMessage, "组织验证", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    //更新API配置信息
                    ThreadPool.QueueUserWorkItem(UpdateAppConfigInfo, jsonResult1);
                }
                else
                {
                    MessageBoxs.Show(Trans.COM, this.ToString(), "登录组织不存在", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                #endregion

                //var synResult = BLLCom.SynchronizeSupMerchantAuthorityInfo();
                //if (!synResult)
                //{
                //    MessageBoxs.Show(Trans.COM, this.ToString(), "同步失败！" + BLLCom.ErrMsg, MessageBoxButtons.OK,
                //        MessageBoxIcon.Warning);
                //}
                
                this.RealHide();

                ThreadPool.QueueUserWorkItem(new WaitCallback(SaveUserLoginInfo));

                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadSystemConfigInfo));

                ThreadPool.QueueUserWorkItem(new WaitCallback(ShowMainWindow));
            }
            catch (Exception ex)
            {
                MessageBox.Show("登录失败，" + ex.Message, "审核失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        /// <summary>
        /// 保存用户登录信息
        /// </summary>
        private void SaveUserLoginInfo(object paramObj)
        {
            try
            {
                ISynchronizeInvoke synchronizer = this;
                if (synchronizer.InvokeRequired)
                {
                    MethodInvoker tempInvoker = new MethodInvoker(this.SaveLogInfo);
                    synchronizer.Invoke(tempInvoker, null);
                }
                else
                {
                    SaveLogInfo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发现异常:" + ex.Message);
                return;
            }
        }

        void SaveLogInfo()
        {
            StringBuilder tempRembLoginUserName = new StringBuilder(255);
            SystemFunction.GetPrivateProfileString(LocalConfigFileConst.SelectionName.ClientInfo,
                LocalConfigFileConst.KeyName.RembLoginUserNames, string.Empty, tempRembLoginUserName, 255,
                LocalConfigFileConst.ConfigFilePath.LocalLoginInfo);

            if (this.checkBoxRememberAccount.Checked)
            {
                //保存当前登录用户名称
                SystemFunction.WritePrivateProfileString(LocalConfigFileConst.SelectionName.ClientInfo,
                    LocalConfigFileConst.KeyName.LastLoginUserName, this.txtUserName.Text.Trim(),
                    LocalConfigFileConst.ConfigFilePath.UserLoginInfo.Replace(SysConst.PH_USERNAME, txtUserName.Text.Trim()));
                //保存当前登录组织
                SystemFunction.WritePrivateProfileString(LocalConfigFileConst.SelectionName.ClientInfo,
                    LocalConfigFileConst.KeyName.LastRembLoginOrgID, this.comboxOrg.Text ?? string.Empty,
                    LocalConfigFileConst.ConfigFilePath.UserLoginInfo.Replace(SysConst.PH_USERNAME, txtUserName.Text.Trim()));

                if (string.IsNullOrEmpty(tempRembLoginUserName.ToString()))
                {
                    //保存所有登录用户名称
                    SystemFunction.WritePrivateProfileString(LocalConfigFileConst.SelectionName.ClientInfo,
                        LocalConfigFileConst.KeyName.RembLoginUserNames, txtUserName.Text.Trim(),
                        LocalConfigFileConst.ConfigFilePath.LocalLoginInfo);
                }
                else if (!tempRembLoginUserName.ToString().Contains(this.txtUserName.Text.Trim()))
                {
                    //保存所有登录组织
                    SystemFunction.WritePrivateProfileString(LocalConfigFileConst.SelectionName.ClientInfo,
                        LocalConfigFileConst.KeyName.RembLoginUserNames,
                        tempRembLoginUserName.Append(SysConst.SPLIT_CHAR + this.txtUserName.Text.Trim()).ToString(),
                        LocalConfigFileConst.ConfigFilePath.LocalLoginInfo);
                }
            }
            else
            {
                SystemFunction.WritePrivateProfileString(LocalConfigFileConst.SelectionName.ClientInfo,
                    LocalConfigFileConst.KeyName.LastLoginUserName, string.Empty,
                    LocalConfigFileConst.ConfigFilePath.UserLoginInfo.Replace(SysConst.PH_USERNAME,
                        txtUserName.Text.Trim()));

                SystemFunction.WritePrivateProfileString(LocalConfigFileConst.SelectionName.ClientInfo,
                    LocalConfigFileConst.KeyName.LastRembLoginOrgID, string.Empty,
                    LocalConfigFileConst.ConfigFilePath.UserLoginInfo.Replace(SysConst.PH_USERNAME,
                        txtUserName.Text.Trim()));

                if (tempRembLoginUserName.ToString().Contains(txtUserName.Text.Trim()) &&
                    !string.IsNullOrEmpty(txtUserName.Text.Trim()))
                {
                    var rembUserNames = tempRembLoginUserName.ToString()
                        .Replace(txtUserName.Text.Trim(), string.Empty)
                        .Replace((SysConst.SPLIT_CHAR + SysConst.SPLIT_CHAR).ToString(),
                            SysConst.SPLIT_CHAR.ToString());
                    if (rembUserNames.StartsWith(SysConst.SPLIT_CHAR.ToString()))
                    {
                        rembUserNames = rembUserNames.Substring(1);
                    }
                    if (rembUserNames.EndsWith(SysConst.SPLIT_CHAR.ToString()))
                    {
                        rembUserNames = rembUserNames.Substring(0, rembUserNames.Length - 1);
                    }

                    SystemFunction.WritePrivateProfileString(LocalConfigFileConst.SelectionName.ClientInfo,
                        LocalConfigFileConst.KeyName.RembLoginUserNames, rembUserNames,
                        LocalConfigFileConst.ConfigFilePath.LocalLoginInfo);
                }
            }

            //保存最后一次登录的用户名
            SystemFunction.WritePrivateProfileString(LocalConfigFileConst.SelectionName.ClientInfo,
                LocalConfigFileConst.KeyName.LastestUserName, txtUserName.Text.Trim(),
                LocalConfigFileConst.ConfigFilePath.LocalLoginInfo);

            SystemFunction.WritePrivateProfileString(LocalConfigFileConst.SelectionName.ClientInfo,
                LocalConfigFileConst.KeyName.LastLoginPassword, this.checkBoxRememberPwd.Checked ? this.txtUserPwd.Text : string.Empty,
                LocalConfigFileConst.ConfigFilePath.UserLoginInfo.Replace(SysConst.PH_USERNAME, txtUserName.Text.Trim()));
        }

        private void LoadSystemConfigInfo(object paramObj)
        {
            try
            {
                ISynchronizeInvoke synchronizer = this;
                if (synchronizer.InvokeRequired)
                {
                    MethodInvoker tempInvoker = new MethodInvoker(this.LoadSystemConfigInfo);
                    synchronizer.Invoke(tempInvoker, null);
                }
                else
                {
                    LoadSystemConfigInfo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发现异常:" + ex.Message);
                return;
            }
        }

        private void LoadSystemConfigInfo()
        {
            //初始化数据库配置
            //DBManager.DBInit(DBCONFIG.Coeus);
            #region 初始化表的更新时间

            //LoginUIModel argsLoginUiModel = new LoginUIModel
            //{
            //    User_ID = LoginConfigInfo.UserID,
            //    UO_Org_ID = LoginConfigInfo.OrgID
            //};
            //SystemConfigInfo.DBTimeStampList = SystemDAX.GetTablesTimeStamp(argsLoginUiModel);
            if (!File.Exists(LocalConfigFileConst.ConfigFilePath.TimeStampInfo))
            {
                XmlFileUtility.SerializeToXmlFile(LocalConfigFileConst.ConfigFilePath.TimeStampInfo, typeof(List<DBTableTimeStampUIModel>), SystemConfigInfo.DBTimeStampList);
            }
            LocalConfigInfo.DBTimeStampList = XmlFileUtility.DeserializeXmlFileToObj(LocalConfigFileConst.ConfigFilePath.TimeStampInfo, typeof(List<DBTableTimeStampUIModel>)) as List<DBTableTimeStampUIModel>;

            #endregion

            _loginDAX.InitSystemConfigData();

            //将数据表的更新时间回写到本地文件
            XmlFileUtility.SerializeToXmlFile(LocalConfigFileConst.ConfigFilePath.TimeStampInfo, typeof(List<DBTableTimeStampUIModel>), LocalConfigInfo.DBTimeStampList);
        }

        private void ShowMainWindow(object paramObj)
        {
            try
            {
                ISynchronizeInvoke synchronizer = this;
                if (synchronizer.InvokeRequired)
                {
                    MethodInvoker tempInvoker = new MethodInvoker(this.ShowMainWindow);
                    synchronizer.Invoke(tempInvoker, null);
                }
                else
                {
                    ShowMainWindow();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发现异常:" + ex.Message);
                return;
            }
        }

        private void ShowMainWindow()
        {
            FrmMainView frmMain = new FrmMainView();
            frmMain.Show();
        }

        /// <summary>
        /// 更新AppConfig
        /// </summary>
        private static void UpdateAppConfigInfo(object paramObj)
        {
            JObject paramJObject = paramObj as JObject;
            if (paramJObject != null)
            {
                if (!paramJObject["APIURL"].ToString().Equals(ConfigurationManager.AppSettings[AppSettingKey.API_URL])
                        || !paramJObject["FileServiceAPIURL"].ToString().Equals(ConfigurationManager.AppSettings[AppSettingKey.FILE_SERVICE_API_URL]))
                {
                    //读取程序集的配置文件
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    //接口API地址
                    string mctName = paramJObject["APIURL"] == null ? "" : paramJObject["APIURL"].ToString();
                    //文件服务地址
                    string fileServiceApiurl = paramJObject["FileServiceAPIURL"] == null ? "" : paramJObject["FileServiceAPIURL"].ToString();
                    //获取appSettings节点
                    AppSettingsSection appSettings = (AppSettingsSection)config.GetSection("appSettings");
                    //设置接口API地址
                    appSettings.Settings["OpenPlatformUrl"].Value = mctName;
                    //文件服务地址
                    appSettings.Settings["FileServiceAPIURL"].Value = fileServiceApiurl;
                    //保存配置文件
                    config.Save();

                    //刷新appSettings
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }
        }

        #endregion

        MDLSM_ClientUseLicense argsLicense = new MDLSM_ClientUseLicense();
        //根据本机网卡Mac地址检查是否有许可证  
        private bool CheckUserLicense()
        {
            //记录有效的网卡顺序
            int netCardIndex = 0;
            NetworkInterface[] fNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in fNetworkInterfaces)
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet
                    || adapter.NetworkInterfaceType.ToString().StartsWith(SysConst.WLAN_NAME))
                {
                    netCardIndex++;
                    if (netCardIndex == 1)
                    {
                        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        {
                            argsLicense.CUL_NetworkCardType1 = NetworkCardTypeEnum.Name.LOCAL;
                        }
                        else if (adapter.NetworkInterfaceType.ToString().Contains(SysConst.WLAN_NAME))
                        {
                            argsLicense.CUL_NetworkCardType1 = NetworkCardTypeEnum.Name.WIRELESSLAN;
                        }
                        argsLicense.CUL_MACAdress1 = adapter.GetPhysicalAddress().ToString();
                    }
                    if (netCardIndex == 2)
                    {
                        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        {
                            argsLicense.CUL_NetworkCardType2 = NetworkCardTypeEnum.Name.LOCAL;
                        }
                        else if (adapter.NetworkInterfaceType.ToString().Contains(SysConst.WLAN_NAME))
                        {
                            argsLicense.CUL_NetworkCardType2 = NetworkCardTypeEnum.Name.WIRELESSLAN;
                        }
                        argsLicense.CUL_MACAdress2 = adapter.GetPhysicalAddress().ToString();
                    }
                    if (netCardIndex == 3)
                    {
                        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                        {
                            argsLicense.CUL_NetworkCardType3 = NetworkCardTypeEnum.Name.LOCAL;
                        }
                        else if (adapter.NetworkInterfaceType.ToString().Contains(SysConst.WLAN_NAME))
                        {
                            argsLicense.CUL_NetworkCardType3 = NetworkCardTypeEnum.Name.WIRELESSLAN;
                        }
                        argsLicense.CUL_MACAdress3 = adapter.GetPhysicalAddress().ToString();
                    }
                }
            }

            var resultLicence = _bll.GetClientUseLicenses(argsLicense);
            if (resultLicence.Count == 0)
            {
                FrmClientUseLicense licenseWindow = new FrmClientUseLicense();
                licenseWindow.ShowDialog();
                return false;
            }
            //if (resultLicence[0].CUL_InvalidDate < BLLCom.GetCurStdDatetime())
            //{
            //    CupidMessageBox.Show("原有许可证已失效，请重新申请", "审核失败", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    ClientUseLicenseWindow licenseWindow = new ClientUseLicenseWindow();
            //    licenseWindow.ShowDialog();
            //    return false;
            //}
            if (resultLicence[0].CUL_ApproveStatus == ApproveStatusEnum.Name.APPROVEFAILED)
            {
                MessageBox.Show("许可证审核失败，请重新申请", "审核失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FrmClientUseLicense licenseWindow = new FrmClientUseLicense(resultLicence[0].CUL_ID);
                licenseWindow.ShowDialog();
                return false;
            }
            if (resultLicence[0].CUL_ApproveStatus == ApproveStatusEnum.Name.TOAPPROVE)
            {
                MessageBox.Show("许可证已在审核中，可以联系管理员加速审核", "正在审核中", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }
        #endregion

        #region 用户名失去焦点事件
        private void txtUserPwd_Enter(object sender, EventArgs e)
        {
            if (txtUserPwd.Text.Trim() == "请输入密码")
            {
                txtUserPwd.TextChanged -= new EventHandler(txtUserPwd_TextChanged);
                txtUserPwd.Clear();
                txtUserPwd.TextChanged += new EventHandler(txtUserPwd_TextChanged);
                txtUserPwd.ForeColor = Color.Black;
                txtUserPwd.PasswordChar = '*';
            }
        }

        private void txtUserPwd_Leave(object sender, EventArgs e)
        {
            if (txtUserPwd.Text.Trim() == "")
            {
                txtUserPwd.Text = "请输入密码";
                txtUserPwd.ForeColor = Color.Gray;
                txtUserPwd.PasswordChar = '\0';
            }
        }
        private void txtUserName_Enter(object sender, EventArgs e)
        {
            if (txtUserName.Text.Trim() == "请输入姓名")
            {
                txtUserName.Clear();
                txtUserName.ForeColor = Color.Black;
            }
        }
        private void txtUserName_Leave(object sender, EventArgs e)
        {
            UserNameLeaveFunction();
            if (comboxOrg.Items.Count > 0)
            {
                txtUserPwd.Focus();
            }
            else
            {
                txtUserName.Focus();
                //txtUserName.SelectAll();
            }

            if (txtUserName.Text.Trim() == "")
            {
                txtUserName.Text = "请输入姓名";
                txtUserName.ForeColor = Color.Gray;
            }
        }
        private void txtEMPNO_TextChanged(object sender, EventArgs e)
        {
            if (txtEMPNO.Text.Trim() == "")
            {
                txtEMPNO.TextChanged -= new EventHandler(txtEMPNO_TextChanged);
                txtEMPNO.Text = "请输入工号";
                txtEMPNO.TextChanged += new EventHandler(txtEMPNO_TextChanged);
                txtEMPNO.ForeColor = Color.Gray;
            }
        }

        private void txtUserPwd_TextChanged(object sender, EventArgs e)
        {
            if (txtUserPwd.Text.Trim() == "")
            {
                txtUserPwd.TextChanged -= new EventHandler(txtUserPwd_TextChanged);
                txtUserPwd.Text = "请输入密码";
                txtUserPwd.SelectAll();
                txtUserPwd.TextChanged += new EventHandler(txtUserPwd_TextChanged);
                txtUserPwd.ForeColor = Color.Gray;
                txtUserPwd.PasswordChar = '\0';
            }
            else
            {
                txtUserPwd.ForeColor = Color.Black;
                txtUserPwd.PasswordChar = '*';
            }
        }
        private void UserNameLeaveFunction()
        {
            string userName = this.txtUserName.Text.Trim();
            if (SameUserNameList != null)
            {
                SameUserNameList.Clear();
            }
            else
            {
                SameUserNameList = new ObservableCollection<MDLSM_User>();
            }
            LoginUIModel argsLoginUIModel = new LoginUIModel();
            argsLoginUIModel.User_Name = userName;
            if (argsLoginUIModel.User_Name == null)
            {
                argsLoginUIModel.User_Name = string.Empty;
            }
            List<MDLSM_User> userList = _bll.GetUserList(argsLoginUIModel);
            foreach (var tbUser in userList)
            {
                SameUserNameList.Add(tbUser);
            }

            //检查是否记住用户名
            StringBuilder tempLastLoginUserName = new StringBuilder(255);
            StringBuilder tempLastLoginPassword = new StringBuilder(255);
            StringBuilder tempLastLoginOrgID = new StringBuilder(255);
            string localConfigFilePath = LocalConfigFileConst.ConfigFilePath.UserLoginInfo.Replace(SysConst.PH_USERNAME, txtUserName.Text);

            SystemFunction.GetPrivateProfileString(LocalConfigFileConst.SelectionName.ClientInfo, LocalConfigFileConst.KeyName.LastLoginUserName, string.Empty, tempLastLoginUserName, 255, localConfigFilePath);
            SystemFunction.GetPrivateProfileString(LocalConfigFileConst.SelectionName.ClientInfo, LocalConfigFileConst.KeyName.LastLoginPassword, string.Empty, tempLastLoginPassword, 255, localConfigFilePath);
            SystemFunction.GetPrivateProfileString(LocalConfigFileConst.SelectionName.ClientInfo, LocalConfigFileConst.KeyName.LastRembLoginOrgID, string.Empty, tempLastLoginOrgID, 255, localConfigFilePath);

            if (tempLastLoginUserName.ToString().Length > 0)
            {
                txtUserName.Text = tempLastLoginUserName.ToString();
                this.checkBoxRememberAccount.Checked = true;
                UserNameTextChangesFunction();
            }
            else
            {
                this.checkBoxRememberAccount.Checked = false;
                if (SameUserNameList.Count > 0)
                {
                    txtEMPNO.Text = SameUserNameList[0].User_EMPNO;
                }
                else
                {
                    txtEMPNO.Text = string.Empty;
                }
                EMPNoLeaveFunction();
            }
            if (this.checkBoxRememberAccount.Checked && tempLastLoginOrgID.ToString().Length > 0)
            {
                comboxOrg.Text = tempLastLoginOrgID.ToString();
            }
            if (tempLastLoginPassword.ToString().Length > 0)
            {
                txtUserPwd.ForeColor = Color.Black;
                txtUserPwd.PasswordChar = '*';
                txtUserPwd.Text = tempLastLoginPassword.ToString();
                this.checkBoxRememberPwd.Checked = true;
            }
            else
            {
                txtUserPwd.ForeColor = Color.Gray;
                txtUserPwd.PasswordChar = '\0';
                txtUserPwd.Text = "请输入密码";
                this.checkBoxRememberPwd.Checked = false;
            }
        }

        private string lastUserName = string.Empty;
        /// <summary>
        /// 用户TextChanged事件
        /// </summary>
        private void UserNameTextChangesFunction()
        {
            string userName = this.txtUserName.Text.Trim();
            string empNo = this.txtEMPNO.Text.Trim();
            if (userName == lastUserName && !string.IsNullOrEmpty(empNo) && !"请输入工号".Equals(empNo))
            {
                return;
            }
            this.txtEMPNO.ForeColor = Color.Black;
            lastUserName = userName;
            if (SameUserNameList != null)
            {
                SameUserNameList.Clear();
            }
            else
            {
                SameUserNameList = new ObservableCollection<MDLSM_User>();
            }

            List<MDLSM_User> userList = new List<MDLSM_User>();
            LoginUIModel argsLoginUIModel = new LoginUIModel();
            if (txtUserName.Text.Trim() == SysConst.SUPER_ADMIN)
            {
                txtUserName.Text = SysConst.SUPER_ADMIN;
                txtEMPNO.Text = SysConst.SUPER_ADMIN;
                EMPNoLeaveFunction();
                //ExecuteUserEMPNOChangedCommand(null);
                return;
            }
            txtUserName.Tag = userName;
            argsLoginUIModel.User_Name = userName;
            userList = _bll.GetUserList(argsLoginUIModel);
            foreach (var tbUser in userList)
            {
                SameUserNameList.Add(tbUser);
            }
            if (SameUserNameList.Count > 0)
            {
                txtEMPNO.Text = SameUserNameList[0].User_EMPNO;
                EMPNoLeaveFunction();
                //ExecuteUserEMPNOChangedCommand(null);
            }
        }

        #endregion

        private void txtEMPNO_Enter(object sender, EventArgs e)
        {
            if (txtEMPNO.Text.Trim() == "请输入工号")
            {
                txtEMPNO.TextChanged -= new EventHandler(txtEMPNO_TextChanged);
                txtEMPNO.Clear();
                txtEMPNO.TextChanged += new EventHandler(txtEMPNO_TextChanged);
                txtEMPNO.ForeColor = Color.Black;
            }
        }

        #region 工号失去焦点事件
        private string lastUserEmpNo = string.Empty;
        private void txtEMPNO_Leave(object sender, EventArgs e)
        {
            EMPNoLeaveFunction();
            if (txtEMPNO.Text.Trim() == "")
            {
                txtEMPNO.Text = "请输入工号";
                txtEMPNO.ForeColor = Color.Gray;
            }
        }

        private void EMPNoLeaveFunction()
        {
            try
            {
                string userName = this.txtUserName.Text.Trim();
                string empNo = this.txtEMPNO.Text.Trim();
                if (empNo == lastUserEmpNo && userName == lastUserName)
                {
                    return;
                }
                lastUserEmpNo = empNo;
                if (_organizationList != null)
                {
                    _organizationList.Clear();
                }
                else
                {
                    _organizationList = new List<MDLSM_Organization>();
                }

                LoginUIModel argsLoginUiModel = new LoginUIModel
                {
                    User_ID = null,
                    User_Name = userName,
                    User_EMPNO = empNo
                };
                var orgList = _bll.GetOrgListByUser(argsLoginUiModel);
                foreach (var tbOrg in orgList)
                {
                    _organizationList.Add(tbOrg);
                }
                if (_organizationList.Count > 0)
                {
                    comboxOrg.ValueMember = SystemTableColumnEnums.SM_Organization.Code.Org_ID;
                    comboxOrg.DisplayMember = SystemTableColumnEnums.SM_Organization.Code.Org_ShortName;
                    comboxOrg.DataSource = _organizationList;

                    ValueListItem listItem = new ValueListItem();
                    listItem.DisplayText = _organizationList[0].Org_ShortName;
                    listItem.DataValue = _organizationList[0].Org_ID;
                    comboxOrg.SelectedItem = listItem;
                    comboxOrg.Text = _organizationList[0].Org_ShortName;
                    //comboxOrg.SelectedText = OrganizationList[0].Org_ShortName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("出现异常：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        #endregion

        #region 同步组织信息

        private void btnSyncOrg_Click(object sender, EventArgs e)
        {
            //同步组织信息
            var result = BLLCom.SynchronizeOrgInfo();

            if (result)
            {
                if (_organizationList != null)
                {
                    _organizationList.Clear();
                }
                else
                {
                    _organizationList = new List<MDLSM_Organization>();
                }

                IList<MDLSM_Organization> orgList = new List<MDLSM_Organization>();
                if (txtUserName.Text.Trim() == SysConst.SUPER_ADMIN)
                {
                    MDLSM_Organization argsOrgQuery = new MDLSM_Organization();
                    argsOrgQuery.WHERE_Org_IsValid = true;
                    _bll.QueryForList(argsOrgQuery, orgList);
                }
                else
                {
                    LoginUIModel argsLoginUIModel = new LoginUIModel();
                    argsLoginUIModel.User_EMPNO = this.txtEMPNO.Text.Trim();
                    if (argsLoginUIModel.User_EMPNO == null)
                    {
                        argsLoginUIModel.User_EMPNO = string.Empty;
                    }
                    orgList = _bll.GetOrgListByUser(argsLoginUIModel);
                }
                foreach (var tbOrg in orgList)
                {
                    _organizationList.Add(tbOrg);
                }

                comboxOrg.ValueMember = "Org_ID";
                comboxOrg.DisplayMember = "Org_ShortName";
                comboxOrg.DataSource = orgList;

                MessageBoxs.Show(Trans.COM, this.ToString(), "同步组织信息成功！", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBoxs.Show(Trans.COM, this.ToString(), "同步组织信息失败！", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 清除缓存
        private void btnClearCache_Click(object sender, EventArgs e)
        {
            string cacheFolderPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" +
                                   LocalConfigFileConst.FileFolderName.LocalConfig + "\\" +
                                   LocalConfigFileConst.FileFolderName.SysConData;
            if (Directory.Exists(cacheFolderPath))
            {
                DirectoryInfo cacheFolder = new DirectoryInfo(cacheFolderPath);
                foreach (FileInfo fileInfo in cacheFolder.GetFiles())
                {
                    if (!fileInfo.Name.Contains("SoftConfig.ini"))
                    {
                        File.Delete(fileInfo.FullName);
                    }
                }
            }
        }
        #endregion

        private void checkBoxRememberAccount_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxRememberPwd_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //得到变量的值
                mouseOff = new Point(-e.X, -e.Y);
                //点击左键按下时标注为true;
                leftFlag = true;
            }
        }

        private void lblTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                //设置移动后的位置
                var mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);
                Location = mouseSet;
            }
        }

        private void lblTitle_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                //释放鼠标后标注为false;
                leftFlag = false;
            }
        }

        private void FrmLogin_Activated(object sender, EventArgs e)
        {
            if (!_isActivated)
            {
                _isActivated = true;
                this.Invalidate(true);
            }
        }

        private void FrmLogin_Deactivate(object sender, EventArgs e)
        {
            if (_isActivated)
            {
                _isActivated = false;
                this.Invalidate(true);
            }
        }

    }
}
