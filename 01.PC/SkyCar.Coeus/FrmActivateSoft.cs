using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UI.Common;
using SkyCar.Common.Utility;


namespace SkyCar.Coeus.Ult.Entrance
{
    public partial class FrmActivateSoft : Form
    {

        /// <summary>
        /// 业务逻辑对象【固定】
        /// </summary>
        LoginBLL bll = new LoginBLL();

        public FrmActivateSoft()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtActivateSoftCode.Text.Trim()))
            {
                MessageBox.Show("激活码不可以为空");
                return;
            }
            ValActivateSoftCode(this.txtActivateSoftCode.Text.Trim());
        }

        /// <summary>
        /// 验证激活码
        /// </summary>
        /// <param name="activateCode"></param>
        /// <returns></returns>
        private void ValActivateSoftCode(string activateCode)
        {

            #region 激活
            
            string argsPostData = string.Format(ApiParameter.BF0001,
                HttpUtility.UrlEncode(activateCode), SysConst.ProductCode);
            string strApiData = APIDataHelper.GetAPIData(ApiUrl.BF0001Url, argsPostData);
            var jsonResult = (JObject) JsonConvert.DeserializeObject(strApiData);
            if (jsonResult["ResultCode"].ToString().Equals("I0001"))
            {
                #region 更新appSettings

                //更新appSettings
                ChangeAppConfiguration(jsonResult,activateCode);
                //刷新appSettings
                ConfigurationManager.RefreshSection("appSettings");

                #endregion

                try
                {
                    var boolTemp = false;
                    DBManager.DBInit(DBCONFIG.Coeus);
                    DBManager.BeginTransaction(DBCONFIG.Coeus);

                    #region 同步组织

                    var argsListOrganization = jsonResult["ListOrganization"];
                    if (argsListOrganization != null )
                    {
                        var plateformOrgList =JsonConvert.DeserializeObject<List<MDLSM_Organization>>(argsListOrganization.ToString());
                        var localOrgList = new List<MDLSM_Organization>();
                        bll.QuerryForList<MDLSM_Organization>(new MDLSM_Organization(), localOrgList);

                        #region

                        if (localOrgList.Count > 0)
                        {
                            foreach (var model in localOrgList)
                            {
                                model.Org_IsValid = false;
                                model.WHERE_Org_ID = model.Org_ID;
                                boolTemp = bll.Save<MDLSM_Organization>(model);
                                if (!boolTemp)
                                {
                                    MessageBoxs.Show(Trans.COM, this.ToString(), "商户激活失败！\r\n更新组织状态失败！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                                    return;
                                }
                            }
                        }

                        foreach (var org in plateformOrgList)
                        {
                            var templocalOrgList = localOrgList.Where(p => p.Org_Code == org.Org_Code).ToList();
                            if (templocalOrgList.Count > 0)
                            {
                                var argsOrganizationorg = templocalOrgList[0];
                                argsOrganizationorg.WHERE_Org_ID = templocalOrgList[0].Org_ID;

                                argsOrganizationorg.Org_Code = org.Org_Code;
                                argsOrganizationorg.Org_FullName = org.Org_FullName;
                                argsOrganizationorg.Org_ShortName = org.Org_ShortName;
                                argsOrganizationorg.Org_Contacter = org.Org_Contacter;
                                argsOrganizationorg.Org_TEL = org.Org_TEL;
                                argsOrganizationorg.Org_PhoneNo = org.Org_PhoneNo;
                                argsOrganizationorg.Org_Prov_Code = org.Org_Prov_Code;
                                argsOrganizationorg.Org_City_Code = org.Org_City_Code;
                                argsOrganizationorg.Org_Dist_Code = org.Org_Dist_Code;
                                argsOrganizationorg.Org_Addr = org.Org_Addr;
                                argsOrganizationorg.Org_Remark = org.Org_Remark;
                                argsOrganizationorg.Org_PlatformCode = jsonResult["MCT_Code"].ToString();
                                argsOrganizationorg.Org_IsValid = true;
                                boolTemp = bll.Save<MDLSM_Organization>(argsOrganizationorg);
                                if (!boolTemp)
                                {
                                    MessageBoxs.Show(Trans.COM, this.ToString(), "商户激活失败！\r\n更新组织内容失败！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                                    return;
                                }
                            }
                            else
                            {
                                var argsorg = new MDLSM_Organization
                                {
                                    Org_Code = org.Org_Code,
                                    Org_FullName = org.Org_FullName,
                                    Org_ShortName = org.Org_ShortName,
                                    Org_Contacter = org.Org_Contacter,
                                    Org_TEL = org.Org_TEL,
                                    Org_PhoneNo = org.Org_PhoneNo,
                                    Org_Prov_Code = org.Org_Prov_Code,
                                    Org_City_Code = org.Org_City_Code,
                                    Org_Dist_Code = org.Org_Dist_Code,
                                    Org_Addr = org.Org_Addr,
                                    Org_Remark = org.Org_Remark,
                                    Org_PlatformCode = jsonResult["MCT_Code"].ToString(),
                                    Org_IsValid = true
                                };
                                boolTemp = bll.Save<MDLSM_Organization>(argsorg);
                                if (!boolTemp)
                                {
                                    MessageBoxs.Show(Trans.COM, this.ToString(), "商户激活失败！\r\n新增组织失败！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                                    return;
                                }
                            }
                        }

                        #endregion
                    }

                    #endregion

                    #region 更新参数表

                    var argsParameterQuery = new MDLSM_Parameter
                    {
                        Para_Code1 = "1001",
                        Para_IsValid = true
                    };
                    var parameterResult = new MDLSM_Parameter();
                    bll.QuerryForObject<MDLSM_Parameter, MDLSM_Parameter>(argsParameterQuery, parameterResult);
                    parameterResult.Para_Code1 = "1001";
                    parameterResult.Para_Name1 = "商户编码";
                    parameterResult.Para_Value1 = jsonResult["MCT_Code"] == null
                        ? ""
                        : jsonResult["MCT_Code"].ToString();
                    ;
                    boolTemp = bll.Save<MDLSM_Parameter>(parameterResult);
                    if (!boolTemp)
                    {
                        MessageBoxs.Show(Trans.COM, this.ToString(), "商户激活失败！\r\n更新系统参数（商户编码）失败！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        return;
                    }
                    #endregion
                    DBManager.CommitTransaction(DBCONFIG.Coeus);
                }
                catch (Exception ex)
                {
                    MessageBoxs.Show(Trans.COM, this.ToString(), "商户激活失败！\r\n"+ex.Message, "消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    return;
                }
            }
            else
            {
                var strErrorMessage = jsonResult["ResultMsg"] == null ? "" : jsonResult["ResultMsg"].ToString();
                MessageBoxs.Show(Trans.COM, this.ToString(), "商户激活失败！\r\n"+strErrorMessage, "消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            #endregion

            MessageBoxs.Show(Trans.COM, this.ToString(), "恭喜！您已成功激活本系统！\r\n", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var loginWindow = new FrmLogin();
            loginWindow.Show();
            this.Hide();
            
        }


        /// <summary>
        /// 更新AppConfig
        /// </summary>
        /// <param name="paramJObject"></param>
        private static void ChangeAppConfiguration(JObject paramJObject,string paramActiveCode)
        {
            //读取程序集的配置文件
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //商户编码
            string mctCode = paramJObject["MCT_Code"] == null ? "" : paramJObject["MCT_Code"].ToString();
            //商户名称
            string mctName = paramJObject["MCT_Name"] == null ? "" : paramJObject["MCT_Name"].ToString();
            //数据连接字符串
            string systemConnString = paramJObject["MerchantServerConnectString"] == null
                ? ""
                : paramJObject["MerchantServerConnectString"].ToString();
            if (!string.IsNullOrEmpty(systemConnString))
            {
                string[] connstring = systemConnString.Split(';');
                if (!string.IsNullOrEmpty(connstring[0]) && !string.IsNullOrEmpty(connstring[1]) && !string.IsNullOrEmpty(connstring[2]) && !string.IsNullOrEmpty(connstring[3]))
                {
                    systemConnString = connstring[0].Substring(connstring[0].IndexOf("=", StringComparison.Ordinal) + 1) + ";"
                                       + connstring[1].Substring(connstring[1].IndexOf("=", StringComparison.Ordinal) + 1) + ";"
                                       + connstring[2].Substring(connstring[2].IndexOf("=", StringComparison.Ordinal) + 1) + ";"
                                       + connstring[3].Substring(connstring[3].IndexOf("=", StringComparison.Ordinal) + 1);
                }
            }
            //获取appSettings节点
            AppSettingsSection appSettings = (AppSettingsSection)config.GetSection("appSettings");

            //设置商户编码
            appSettings.Settings["MCT_Code"].Value = mctCode;
            //设置商户名称
            appSettings.Settings["MCT_Name"].Value = mctName;
            //设置商户激活码
            appSettings.Settings["MCT_ActivationCode"].Value = paramActiveCode;
            //设置数据连接字符串
            appSettings.Settings["SystemConnString"].Value = CryptoHelp.Encode(systemConnString);

            //保存配置文件
            config.Save();
        }

    }
}
