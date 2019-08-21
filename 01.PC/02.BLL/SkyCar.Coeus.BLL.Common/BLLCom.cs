using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.UIModel.Common.QCModel;
using SkyCar.Common.Utility;
using System.Drawing;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.Common.UIModel;
using SkyCar.Coeus.UIModel.SD.QCModel;
using System.Data.SqlClient;
using System.Data;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.UIModel.Common.APModel;

namespace SkyCar.Coeus.BLL.Common
{
    public class BLLCom
    {
        #region 私有静态变量
        /// <summary>
        /// 基本业务BLL
        /// </summary>
        private static BLLBase _bllBase = new BLLBase(Trans.COM);
        /// <summary>
        /// 网络工具类
        /// </summary>
        private static WebUtils _webUtils = new WebUtils();

        #endregion

        #region 公共属性
        /// <summary>
        /// 错误信息
        /// </summary>
        public static string ErrMsg = string.Empty;
        #endregion

        /// <summary>
        /// 获取基本信息（组织信息，个人客户信息，单位客户信息等等）
        /// </summary>
        /// <typeparam name="D">结果Model类型</typeparam>
        /// <param name="paramTBModel">查询条件TBModel，基本信息对应的TBModel(其类型即系统缓存的Key)</param>
        /// <param name="paramIsGetInfoFromDBDirectly">是否直接从数据库中获取信息
        /// <para>paramIsGetInfoFromDBDirectly=true，直接从数据库中取数据。</para>
        /// <para>paramIsGetInfoFromDBDirectly=false，先从缓存取数据，未取到的场合，再从数据库中取数据。</para>
        /// </param>
        /// <returns>结果ModelList</returns>
        public static List<D> GetBaseInfor<D>(object paramTBModel, bool paramIsGetInfoFromDBDirectly)
        {
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "", "", null);
            //结果List
            List<D> resultList = new List<D>();

            //非直接从数据库中获取信息
            if (!paramIsGetInfoFromDBDirectly)
            {
                //根据TBModel类型从缓存获取信息
                object obj = CacheDAX.Get(paramTBModel.GetType().FullName);
                if (obj != null)
                {
                    resultList = obj as List<D>;
                    return resultList;
                }
            }

            //检查参数是否为TBModel
            if (!paramTBModel.GetType().FullName.Contains(SysConst.EN_TBMODEL))
            {
                LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "", "", null);
                return resultList;
            }
            //根据TBModel对象，查询信息
            _bllBase.QueryForList<D>(paramTBModel, resultList);
            //将结果信息添加到缓存，已经存在则覆盖原来的信息
            CacheDAX.Add(paramTBModel.GetType().FullName, resultList, true);

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "", "", null);
            return resultList;
        }

        #region 初始化
        /// <summary>
        /// 初始化系统
        /// </summary>
        public static void InitializeSystem(object paramObj)
        {
            #region 初始化系统参数缓存
            var resultParameter = new List<MDLSM_Parameter>();
            var argsParameter = new MDLSM_Parameter { Para_IsValid = true };
            _bllBase.QueryForList<MDLSM_Parameter>(argsParameter, resultParameter);
            foreach (var loopPara in resultParameter)
            {
                CacheDAX.Add(loopPara.Para_Code1 + SysConst.ULINE + loopPara.Para_Code2, loopPara.Para_Value2, true);
            }
            #endregion

            //初始化系统消息
            //InitializeSystemMessage();

            //初始化系统枚举
            EnumDAX.InitializeEnum();

            //初始化菜单到缓存
            InitializeMenuDetailToCache();

            //初始化用户系统权限到缓存TODO
        }

        #endregion

        /// <summary>
        /// 初始化菜单到缓存
        /// </summary>
        public static void InitializeMenuDetailToCache()
        {
            var menuDetailList = new List<MDLSM_MenuDetail>();
            var argsMenuDetailQuery = new MDLSM_MenuDetail { WHERE_MenuD_IsValid = true };
            //查询菜单组
            _bllBase.QueryForList<MDLSM_MenuDetail>(argsMenuDetailQuery, menuDetailList);

            foreach (var md in menuDetailList)
            {
                CacheDAX.Add(md.MenuD_ClassFullName, md, true);
            }
        }

        /// <summary>
        /// 初始化汽修商户信息到缓存
        /// </summary>
        public static List<MDLPIS_AutoFactoryCustomer> InitializeARMerchantToCache()
        {
            var resultAutoFactoryCustomerList = new List<MDLPIS_AutoFactoryCustomer>();
            //查询汽修商户
            if (LoginInfoDAX.UserID == SysConst.SUPER_ADMIN)
            {
                _bllBase.QueryForList(new MDLPIS_AutoFactoryCustomer { WHERE_AFC_IsValid = true }, resultAutoFactoryCustomerList);
            }
            else
            {
                _bllBase.QueryForList(SQLID.COMM_SQL47, new MDLSM_AROrgSupOrgAuthority
                {
                    WHERE_ASOAH_SupOrg_ID = LoginInfoDAX.OrgID,
                }, resultAutoFactoryCustomerList);
            }
            resultAutoFactoryCustomerList = resultAutoFactoryCustomerList.GroupBy(p => new { p.AFC_Code, p.AFC_Name })
                    .Select(g => g.First()).OrderBy(x => x.AFC_Name).Distinct().ToList();
            CacheDAX.Add(CacheDAX.ConfigDataKey.ARMerchant, resultAutoFactoryCustomerList, true);
            return resultAutoFactoryCustomerList;
        }

        /// <summary>
        /// 初始化汽修商户数据库配置信息
        /// </summary>
        public static void InitializeARMerchantDBConfigInfo()
        {
            #region 获取数据库链接

            //获取当前汽配商户的汽修商户服务器信息
            var resultMerchantServerList = GetMerchantServerList();
            DBCONFIG.Configs = new Dictionary<string, string>();
            foreach (var loopServerConfigInfo in resultMerchantServerList)
            {
                DBCONFIG.Configs.Add(loopServerConfigInfo.AR_MCT_Code, loopServerConfigInfo.MS_ConnectString);
                //检查数据库连接是否正常
                if (!DBManager.CheckConnectin(loopServerConfigInfo.AR_MCT_Code))
                {
                    continue;
                }
            }
            #endregion
        }

        /// <summary>
        /// 获取商户数据库配置Key
        /// </summary>
        /// <param name="paramMerchantCode"></param>
        /// <returns></returns>
        public static string GetMerchantDbConfigKey(string paramMerchantCode)
        {
            if (string.IsNullOrEmpty(paramMerchantCode))
            {
                return string.Empty;
            }

            if (DBCONFIG.Configs == null)
            {
                DBCONFIG.Configs = new Dictionary<string, string>();
            }
            //如果不在已缓存列表
            if (!DBCONFIG.Configs.ContainsKey(paramMerchantCode))
            {
                //根据汽修商户编码获取汽修商户服务器信息
                var resultMerchantServer = BLLCom.GetMerchantServerInfoBySupplier(paramMerchantCode);
                if (resultMerchantServer == null || string.IsNullOrEmpty(resultMerchantServer.MS_DataBase))
                {
                    return string.Empty;
                }

                DBCONFIG.Configs.Add(paramMerchantCode, resultMerchantServer.MS_ConnectString);
                //检查数据库连接是否正常
                if (!DBManager.CheckConnectin(paramMerchantCode))
                {
                    return string.Empty;
                }
            }
            return paramMerchantCode;
        }

        /// <summary>
        /// 初始化系统消息
        /// </summary>
        private static void InitializeSystemMessage()
        {
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "", "", null);

            //定义枚举对象列表
            List<MDLSM_Message> msgList = new List<MDLSM_Message>();
            //查询系统所有枚举数据
            _bllBase.QueryForList<MDLSM_Message>(new MDLSM_Message(), msgList);
            //初始化系统消息
            MsgHelp.InitializeMsg(msgList);

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "", "", null);
        }


        public static List<MDLSM_Menu> GetSystemMenu()
        {
            List<MDLSM_Menu> resultList = new List<MDLSM_Menu>();

            return resultList;
        }

        #region 获取单据编号

        /// <summary>
        /// 获取Coeus单据编号
        /// </summary>
        /// <param name="paramDocumentType">单据类型，请使用：DocumentTypeEnums.Code.XX</param>
        /// <returns>单号,如果返回NULL,表示获取单号失败</returns>
        public static string GetCoeusDocumentNo(string paramDocumentType)
        {
            string newSystemNo = string.Empty;
            var resultSystemNo = _bllBase.QueryForObject(SQLID.COMM_SQL38, new RecordCodeQCModel
            {
                OrgCode = LoginInfoDAX.OrgCode,
                TableAbridge = paramDocumentType,
            });
            if (resultSystemNo != null)
            {
                newSystemNo = resultSystemNo.ToString();
            }

            bool saveSystomNoResult = _bllBase.Save<MDLSM_SystemNo>(new MDLSM_SystemNo()
            {
                SN_Value = newSystemNo,
                SN_Status = "有效",
                SN_CreatedBy = LoginInfoDAX.UserName,
                SN_UpdatedBy = LoginInfoDAX.UserName
            });
            if (!saveSystomNoResult)
            {
                return null;
            }
            return newSystemNo;
        }

        /// <summary>
        /// 获取Venus中单据编号
        /// </summary>
        /// <param name="paramVenusDbConfig"></param>
        /// <param name="paramVenusOrgCode"></param>
        /// <param name="paramVenusDocumentType"></param>
        /// <returns></returns>
        public static string GetVenusDocumentNo(string paramVenusDbConfig, string paramVenusOrgCode, string paramVenusDocumentType)
        {
            //根据Venus中单据编号的生成规则生成编号
            string resultDocumentNo = DBManager.QueryForObject<string>(paramVenusDbConfig, SQLID.COMM_SQL36, new RecordCodeQCModel
            {
                OrgCode = paramVenusOrgCode,
                TableAbridge = paramVenusDocumentType
            });
            if (string.IsNullOrEmpty(resultDocumentNo))
            {
                return null;
            }
            //新增Venus系统编号数据
            MDLSM_SystemNo insertSystemNo = new MDLSM_SystemNo()
            {
                SN_ID = Guid.NewGuid().ToString(),
                SN_Value = resultDocumentNo,
                SN_Status = "有效",
                SN_IsValid = true,
                SN_CreatedBy = LoginInfoDAX.MCTName,
                SN_UpdatedBy = LoginInfoDAX.MCTName,
                SN_VersionNo = 1
            };
            var createSql = new CreateSQL();
            var argsComCrud = new ComCRUDModel { StrSQL = createSql.CreateSQLForInsert(insertSystemNo) };
            var insertSystemNoResult = DBManager.Insert(paramVenusDbConfig, SQLID.COMM_SQL01, argsComCrud);
            if (insertSystemNoResult != null)
            {
                return null;
            }
            return resultDocumentNo;
        }

        #endregion

        /// <summary>
        /// 从平台同步组织信息到本地数据库
        /// </summary>
        /// <returns></returns>
        public static bool SynchronizeOrgInfo()
        {
            var funcName = "SynchronizeOrgInfo";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);

            //根据商户编码和产品编码来查询组织信息
            string argsPostData = string.Format(ApiParameter.BF0002,
                ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE], SysConst.ProductCode);
            string strApiData = APIDataHelper.GetAPIData(ApiUrl.BF0002Url, argsPostData);
            var jsonResult = (JObject)JsonConvert.DeserializeObject(strApiData);

            if (jsonResult != null && jsonResult[SysConst.EN_RESULTCODE].ToString().Equals(SysConst.EN_I0001))
            {
                var argsSettingKeyMctCode1 = jsonResult["ListOrganization"];
                var platformOrgJson = (JArray)JsonConvert.DeserializeObject(argsSettingKeyMctCode1.ToString());

                List<MDLSM_Organization> tempPlatformOrganizationList = JsonConvert.DeserializeObject<List<MDLSM_Organization>>(platformOrgJson.ToString());
                var localOrgList = new List<MDLSM_Organization>();
                _bllBase.QueryForList(new MDLSM_Organization { WHERE_Org_IsValid = true }, localOrgList);
                var invalidOrgList = new List<MDLSM_Organization>();

                _bllBase.CopyModelList(localOrgList, invalidOrgList);

                foreach (var loopPlatFromOrganizetion in tempPlatformOrganizationList)
                {
                    //平台上数据
                    loopPlatFromOrganizetion.WHERE_Org_ID = loopPlatFromOrganizetion.Org_ID;
                    loopPlatFromOrganizetion.Org_MCT_ID = ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE] ?? string.Empty;
                    loopPlatFromOrganizetion.Org_CreatedBy = loopPlatFromOrganizetion.Org_UpdatedBy = "系统同步";

                    loopPlatFromOrganizetion.WHERE_Org_VersionNo = loopPlatFromOrganizetion.Org_VersionNo;
                    loopPlatFromOrganizetion.Org_IsValid = true;
                }
                #region 传入到[组织]列表中
                AROrganizationDataSet.AROrganizationDataTable organizationDataTable = new AROrganizationDataSet.AROrganizationDataTable();
                foreach (var loopPlatformOrganization in tempPlatformOrganizationList)
                {
                    if (string.IsNullOrEmpty(loopPlatformOrganization.Org_Code))
                    {
                        continue;
                    }
                    AROrganizationDataSet.AROrganizationRow newArOrganizationRow =
                        organizationDataTable.NewAROrganizationRow();
                    newArOrganizationRow.Org_ID = loopPlatformOrganization.Org_ID;
                    newArOrganizationRow.Org_MCT_ID = loopPlatformOrganization.Org_MCT_ID;
                    newArOrganizationRow.Org_Code = loopPlatformOrganization.Org_Code;
                    newArOrganizationRow.Org_PlatformCode = loopPlatformOrganization.Org_PlatformCode;
                    newArOrganizationRow.Org_FullName = loopPlatformOrganization.Org_FullName;
                    newArOrganizationRow.Org_ShortName = loopPlatformOrganization.Org_ShortName;
                    newArOrganizationRow.Org_Contacter = loopPlatformOrganization.Org_Contacter;
                    newArOrganizationRow.Org_TEL = loopPlatformOrganization.Org_TEL;
                    newArOrganizationRow.Org_PhoneNo = loopPlatformOrganization.Org_PhoneNo;
                    newArOrganizationRow.Org_Prov_Code = loopPlatformOrganization.Org_Prov_Code;
                    newArOrganizationRow.Org_City_Code = loopPlatformOrganization.Org_City_Code;
                    newArOrganizationRow.Org_Dist_Code = loopPlatformOrganization.Org_Dist_Code;
                    newArOrganizationRow.Org_Addr = loopPlatformOrganization.Org_Addr;
                    newArOrganizationRow.Org_Longitude = loopPlatformOrganization.Org_Longitude;
                    newArOrganizationRow.Org_Latitude = loopPlatformOrganization.Org_Latitude;
                    newArOrganizationRow.Org_MarkerTitle = loopPlatformOrganization.Org_MarkerTitle;
                    newArOrganizationRow.Org_MarkerContent = loopPlatformOrganization.Org_MarkerContent;
                    newArOrganizationRow.Org_MainBrands = loopPlatformOrganization.Org_MainBrands;

                    newArOrganizationRow.Org_MainProducts = loopPlatformOrganization.Org_MainProducts;
                    newArOrganizationRow.Org_Remark = loopPlatformOrganization.Org_Remark;
                    if (loopPlatformOrganization.Org_IsValid != null)
                    {
                        newArOrganizationRow.Org_IsValid = loopPlatformOrganization.Org_IsValid.Value;
                    }

                    newArOrganizationRow.Org_Longitude = loopPlatformOrganization.Org_Longitude;
                    newArOrganizationRow.Org_Latitude = loopPlatformOrganization.Org_Latitude;
                    newArOrganizationRow.Org_MarkerTitle = loopPlatformOrganization.Org_MarkerTitle;
                    newArOrganizationRow.Org_MarkerContent = loopPlatformOrganization.Org_MarkerContent;
                    newArOrganizationRow.Org_MainBrands = loopPlatformOrganization.Org_MainBrands;

                    newArOrganizationRow.Org_CreatedBy = string.IsNullOrEmpty(loopPlatformOrganization.Org_CreatedBy)
                        ? LoginInfoDAX.UserName : loopPlatformOrganization.Org_CreatedBy;
                    newArOrganizationRow.Org_CreatedTime = loopPlatformOrganization.Org_CreatedTime == null
                        ? BLLCom.GetCurStdDatetime()
                        : loopPlatformOrganization.Org_CreatedTime.Value;
                    newArOrganizationRow.Org_UpdatedBy = LoginInfoDAX.UserName;
                    newArOrganizationRow.Org_UpdatedTime = BLLCom.GetCurStdDatetime();
                    if (loopPlatformOrganization.Org_VersionNo != null)
                    {
                        newArOrganizationRow.Org_VersionNo = loopPlatformOrganization.Org_VersionNo.Value;
                    }
                    organizationDataTable.AddAROrganizationRow(newArOrganizationRow);
                }
                #endregion

                #region 链接数据库并执行[存储过程]
                try
                {

                    //打开数据库连接
                    using (SqlConnection sqlCon = new SqlConnection
                    {
                        ConnectionString = DBManager.GetConnectionString(DBCONFIG.Coeus)
                    })
                    {
                        //创建并初始化SqlCommand对象
                        SqlCommand cmdSupMerchant = new SqlCommand();
                        cmdSupMerchant.Connection = sqlCon;
                        cmdSupMerchant.CommandText = "P_COMM_BatchSyncAROrganizationInfo";
                        cmdSupMerchant.CommandType = CommandType.StoredProcedure;
                        cmdSupMerchant.Parameters.Add("@COMAROrganizationList", SqlDbType.Structured);
                        cmdSupMerchant.Parameters[0].Value = organizationDataTable;

                        sqlCon.Open();
                        cmdSupMerchant.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName,
                            MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SYNCHRONIZE + SystemTableEnums.Name.SM_Organization }), "", null);

                    return false;
                }
                finally
                {

                }
                #endregion

            }
            else
            {
                var strErrorMessage = jsonResult == null ? "" : jsonResult["ResultMsg"].ToString();
                LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, strErrorMessage, "", null);
            }

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            return true;
        }

        /// <summary>
        /// 查询省份 
        /// </summary>
        /// <param name="paramProvRegID">不为空 按大区来查省份</param>
        /// <returns></returns>
        public static List<EnumCodeTextModel> QueryProvList(string paramProvRegID)
        {
            if (string.IsNullOrEmpty(paramProvRegID))
            {
                paramProvRegID = null;
            }
            List<EnumCodeTextModel> resultProvList = new List<EnumCodeTextModel>();
            _bllBase.QueryForList<EnumCodeTextModel>(SQLID.COMM_SQL03, new MDLSM_ChineseProvince()
            {
                WHERE_Prov_Reg_ID = paramProvRegID
            }, resultProvList);
            return resultProvList;
        }

        /// <summary>
        /// 查询城市
        /// </summary>
        /// <param name="paramProvCode"></param>
        /// <returns></returns>
        public static List<EnumCodeTextModel> QueryCityList(string paramProvCode)
        {
            List<EnumCodeTextModel> resultProvList = new List<EnumCodeTextModel>();
            _bllBase.QueryForList<EnumCodeTextModel>(SQLID.COMM_SQL04, new MDLSM_ProvinceCity()
            {
                WHERE_City_Prov_Code = paramProvCode
            }, resultProvList);
            return resultProvList;
        }

        /// <summary>
        /// 查询区域
        /// </summary>
        /// <param name="paramCityCode"></param>
        /// <returns></returns>
        public static List<EnumCodeTextModel> QueryDistList(string paramCityCode)
        {
            List<EnumCodeTextModel> resultProvList = new List<EnumCodeTextModel>();
            _bllBase.QueryForList<EnumCodeTextModel>(SQLID.COMM_SQL05, new MDLSM_CityDistrict()
            {
                WHERE_Dist_City_Code = paramCityCode
            }, resultProvList);
            return resultProvList;
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <returns></returns>
        public static bool Validation(string paramRegularFormula, string paramValidationContent)
        {
            if (string.IsNullOrEmpty(paramValidationContent)
                || paramRegularFormula == null)
            {
                return false;
            }
            if (!Regex.IsMatch(paramValidationContent, paramRegularFormula))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramRecipientEmailAdress">收件人邮箱，多个以；分隔</param>
        /// <param name="paramCcEmailAdress">抄送人邮箱</param>
        /// <param name="paramEmailSubject">邮件主题</param>
        /// <param name="paramEmailBody">邮件内容</param>
        /// <param name="paramAttachFilePath">附件</param>
        public static void SendMail(string paramRecipientEmailAdress, string paramCcEmailAdress, string paramEmailSubject, string paramEmailBody, string paramAttachFilePath)
        {
            string addressOfSender = SystemConfigInfo.SkyCarDevEmailAdress;
            string pwdOfSender = SystemConfigInfo.SkyCarDevEmailPwd;
            if (addressOfSender.Length > 0)
            {
                MailAddress mailAdress = new MailAddress(addressOfSender, addressOfSender.Substring(0, addressOfSender.IndexOf("@", StringComparison.Ordinal)));
                MailMessage mailMessage = new MailMessage
                {
                    Subject = paramEmailSubject.Replace((char)13, (char)0).Replace((char)10, (char)0),
                    From = mailAdress
                };

                //设置邮件收件人  
                string[] recpEmailList = (paramRecipientEmailAdress + SysConst.Semicolon_DBC).Split(Convert.ToChar(SysConst.Semicolon_DBC));
                foreach (string name in recpEmailList)
                {
                    if (name != string.Empty)
                    {
                        string address;
                        string displayName;
                        if (name.IndexOf('<') > 0)
                        {
                            displayName = name.Substring(0, name.IndexOf('<'));
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        else
                        {
                            displayName = string.Empty;
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        mailMessage.To.Add(new MailAddress(address, displayName));
                    }
                }
                //设置邮件的抄送收件人  
                string[] mailNamesOfCc = (paramCcEmailAdress + SysConst.Semicolon_DBC).Split(Convert.ToChar(SysConst.Semicolon_DBC));
                foreach (string name in mailNamesOfCc)
                {
                    if (name != string.Empty)
                    {
                        string address;
                        string displayName;
                        if (name.IndexOf('<') > 0)
                        {
                            displayName = name.Substring(0, name.IndexOf('<'));
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        else
                        {
                            displayName = string.Empty;
                            address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                        }
                        mailMessage.CC.Add(new MailAddress(address, displayName));
                    }
                }

                //设置邮件的内容
                mailMessage.Body = paramEmailBody;
                //设置邮件的格式
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.IsBodyHtml = true;
                //设置邮件的发送级别
                mailMessage.Priority = MailPriority.Normal;

                //设置邮件的附件，将在客户端选择的附件先上传到服务器保存一个，然后加入到mail中  
                if (!string.IsNullOrEmpty(paramAttachFilePath))
                {
                    mailMessage.Attachments.Add(new Attachment(paramAttachFilePath));
                }
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                SmtpClient client = new SmtpClient
                {
                    Host = "smtp.ym.163.com",
                    Port = 25,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(addressOfSender, pwdOfSender),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                client.Send(mailMessage);
            }
            else
            {
                MessageBox.Show("参数表未配置云车研发中心邮箱(0000)");
                return;
            }
        }

        /// <summary>
        /// 验证输入的数据是不是正整数
        /// </summary>
        /// <param name="paramInputData"></param>
        /// <returns></returns>
        public static bool IsInt(string paramInputData)
        {
            var tmp = 0;
            return int.TryParse(paramInputData, out tmp);
        }

        /// <summary>
        /// 验证输入是否只为Decimal类型
        /// </summary>
        /// <param name="paramInputData"></param>
        /// <returns></returns>
        public static bool IsDecimal(string paramInputData)
        {
            var tmp = 0m;
            return decimal.TryParse(paramInputData, out tmp);
        }

        /// <summary>
        /// 获取服务器标准时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurStdDatetime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 根据汽配商户编码获取指定汽修商户服务器信息
        /// </summary>
        /// <param name="paramArMcTCode">汽修商户编码</param>
        /// <returns></returns>
        public static MDLSM_MerchantServer GetMerchantServerInfoBySupplier(string paramArMcTCode)
        {
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, "GetMerchantServerInfoBySupplier", "", "", null);

            var resultMerchantServer = new MDLSM_MerchantServer();
            try
            {
                var argsSupMcTCode = ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE];
                var argsMctActivationCode = HttpUtility.UrlEncode(ConfigurationManager.AppSettings["MctActivationCode"]);
                var argsUrl = ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "API/BF0013";
                var argsPostData = string.Format("SupMCT_Code={0}&MCT_ActivationCode={1}&ARMCT_Code={2}&SP_Code={3}", argsSupMcTCode,
                    argsMctActivationCode, paramArMcTCode, SysConst.ProductCode);
                var strApiData = APIDataHelper.GetAPIData(argsUrl, argsPostData);
                var jsonResult = (JObject)JsonConvert.DeserializeObject(strApiData);

                if (jsonResult != null && jsonResult[SysConst.EN_RESULTCODE].ToString().Equals(SysConst.EN_I0001))
                {
                    resultMerchantServer =
                        JsonConvert.DeserializeObject<MDLSM_MerchantServer>(jsonResult["MerchantServer"].ToString());
                }
                else
                {
                    var strErrorMessage = jsonResult == null ? "" : jsonResult["ResultMsg"].ToString();
                    LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, "GetMerchantServerInfoBySupplier", strErrorMessage,
                        "", null);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(Trans.COM, "GetMerchantServerInfoBySupplier", ex.Message, null, ex);
            }

            return resultMerchantServer;
        }

        /// <summary>
        /// 获取汽修商户服务器信息列表
        /// </summary>
        /// <param name="paramArMcTCode">汽修商户编码</param>
        /// <returns></returns>
        public static List<MerchantServerUIModel> GetMerchantServerList(string paramArMcTCode = "")
        {
            List<MerchantServerUIModel> resultMerchantServer = new List<MerchantServerUIModel>();
            try
            {
                var argsSupMcTCode = ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE];
                var argsMctActivationCode = HttpUtility.UrlEncode(ConfigurationManager.AppSettings["MctActivationCode"]);

                //本地调试
                //var argsUrl = "http://localhost:61860//API/BF0032";
                var argsUrl = ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "API/BF0032";
                var argsPostData = string.Format("SupMCT_Code={0}&MCT_ActivationCode={1}&ARMCT_Code={2}&SP_Code={3}", argsSupMcTCode,
                    argsMctActivationCode, paramArMcTCode, SysConst.ProductCode);
                var strApiData = APIDataHelper.GetAPIData(argsUrl, argsPostData);
                var jsonResult = (JObject)JsonConvert.DeserializeObject(strApiData);

                if (jsonResult != null && jsonResult[SysConst.EN_RESULTCODE].ToString().Equals(SysConst.EN_I0001))
                {
                    resultMerchantServer = JsonConvert.DeserializeObject<List<MerchantServerUIModel>>(jsonResult["MerchantServerList"].ToString());
                }
                else
                {
                    var strErrorMessage = jsonResult == null ? "" : jsonResult["ResultMsg"].ToString();
                    LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, "GetMerchantServerInfoBySupplier", strErrorMessage,
                        "", null);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(Trans.COM, "GetMerchantServerInfoBySupplier", ex.Message, null, ex);
            }

            return resultMerchantServer;
        }

        /// <summary>
        /// 根据汽配商户编码获取指定汽修商户服务器信息
        /// </summary>
        /// <param name="paramArMcTCode">汽修商户编码</param>
        /// <param name="paramSupMcTCode">汽配商户编码</param>
        /// <param name="paramMctActivationCode">汽配商户激活码</param>
        /// <returns></returns>
        public static MDLSM_MerchantServer GetMerchantServerInfoBySupplier(string paramArMcTCode, string paramSupMcTCode, string paramMctActivationCode)
        {
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, "GetMerchantServerInfoBySupplier", "", "", null);

            var resultMerchantServer = new MDLSM_MerchantServer();
            try
            {
                var argsUrl = ConfigurationManager.AppSettings[AppSettingKey.API_URL] + "API/BF0013";
                var argsPostData = string.Format("SupMCT_Code={0}&MCT_ActivationCode={1}&ARMCT_Code={2}&SP_Code={3}", paramSupMcTCode,
                    paramMctActivationCode, paramArMcTCode, SysConst.ProductCode);
                var strApiData = APIDataHelper.GetAPIData(argsUrl, argsPostData);
                var jsonResult = (JObject)JsonConvert.DeserializeObject(strApiData);

                if (jsonResult != null && jsonResult[SysConst.EN_RESULTCODE].ToString().Equals(SysConst.EN_I0001))
                {
                    resultMerchantServer =
                        JsonConvert.DeserializeObject<MDLSM_MerchantServer>(jsonResult["MerchantServer"].ToString());
                }
                else
                {
                    var strErrorMessage = jsonResult == null ? "" : jsonResult["ResultMsg"].ToString();
                    LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, "GetMerchantServerInfoBySupplier", strErrorMessage,
                        "", null);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(Trans.COM, "GetMerchantServerInfoBySupplier", ex.Message, null, ex);
            }

            return resultMerchantServer;
        }

        /// <summary>
        /// 根据指定的汽修商户数据库信息获取组织列表
        /// </summary>
        /// <param name="paramAutoFactoryCustomerDbConfig"></param>
        /// <param name="paramOrgList"></param>
        public static bool QueryAutoFactoryCustomerOrgList(string paramAutoFactoryCustomerDbConfig, IList<MDLSM_Organization> paramOrgList)
        {
            try
            {
                DBManager.QueryForList<MDLSM_Organization>(paramAutoFactoryCustomerDbConfig, SQLID.COMM_SQL21, new MDLSM_Organization(), paramOrgList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据指定的汽修商户数据库信息获取组织列表
        /// </summary>
        /// <param name="paramAutoFactoryCustomerDbConfig"></param>
        /// <param name="paramOrganization"></param>
        /// <param name="paramOrgList"></param>
        public static bool QueryAutoFactoryCustomerOrgList(string paramAutoFactoryCustomerDbConfig, MDLSM_Organization paramOrganization, IList<MDLSM_Organization> paramOrgList)
        {
            try
            {
                if (paramOrganization == null)
                {
                    paramOrganization = new MDLSM_Organization();
                }
                DBManager.QueryForList<MDLSM_Organization>(paramAutoFactoryCustomerDbConfig, SQLID.COMM_SQL21, paramOrganization, paramOrgList);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取显示的配件销售单价
        /// </summary>
        /// <param name="paramAutoPartBarcode">配件条形码</param>
        /// <param name="paramCustomerId">客户ID</param>
        /// <param name="paramAutoFactoryOrgId">平台内汽修商户组织ID</param>
        /// <returns></returns>
        public static decimal GetAutoPartUnitPrice(string paramAutoPartBarcode, string paramCustomerId, string paramAutoFactoryOrgId = "")
        {
            decimal unitPrice = 0;
            if (string.IsNullOrEmpty(paramAutoPartBarcode))
            {
                return unitPrice;
            }
            //不管是否考虑进销存，上次销价，[配件档案].[销价]，[配件档案].[销价系数]*采购价格（最新）
            //因为单价可以进行修改操作 不做任何限制
            if (!string.IsNullOrEmpty(paramCustomerId))
            {
                //获取上次销售价格【考虑平台内汽修商户ID 以及 平台内商户组织ID】
                List<MDLSD_SalesOrderDetail> resultSalesOrderDetailList = new List<MDLSD_SalesOrderDetail>();
                _bllBase.QueryForList(SQLID.SD_SalesOrder_SQL02, new QueryAutoPartUnitPriceQCModel
                {
                    WHERE_SOD_Barcode = paramAutoPartBarcode,
                    WHERE_SO_CustomerID = paramCustomerId,
                    WHERE_AutoFactoryOrgId = paramAutoFactoryOrgId,
                }, resultSalesOrderDetailList);
                if (resultSalesOrderDetailList.Count > 0)
                {
                    unitPrice = resultSalesOrderDetailList[0].SOD_UnitPrice ?? 0;
                }
            }
            if (unitPrice > 0)
            {
                return unitPrice;
            }
            //获取配件档案中的销售单价【不考虑组织】
            MDLBS_AutoPartsArchive resultAutoPartsArchive = new MDLBS_AutoPartsArchive();
            _bllBase.QueryForObject<MDLBS_AutoPartsArchive, MDLBS_AutoPartsArchive>(new MDLBS_AutoPartsArchive()
            {
                WHERE_APA_IsValid = true,
                WHERE_APA_Barcode = paramAutoPartBarcode,
            },
                resultAutoPartsArchive);
            if (!string.IsNullOrEmpty(resultAutoPartsArchive.APA_ID))
            {
                unitPrice = resultAutoPartsArchive.APA_SalePrice ?? 0;
            }
            if (unitPrice > 0)
            {
                return unitPrice;
            }

            //获取库存中的配件采购价格【考虑组织】
            MDLPIS_Inventory resultInventory = new MDLPIS_Inventory();
            _bllBase.QueryForObject<MDLPIS_Inventory, MDLPIS_Inventory>(new MDLPIS_Inventory()
            {
                WHERE_INV_IsValid = true,
                WHERE_INV_Barcode = paramAutoPartBarcode,
                WHERE_INV_Org_ID = LoginInfoDAX.OrgID
            }, resultInventory);
            if (!string.IsNullOrEmpty(resultInventory.INV_ID))
            {
                unitPrice = (resultInventory.INV_PurchaseUnitPrice ?? 0) * (resultAutoPartsArchive.APA_SalePriceRate ?? 0);
            }
            return unitPrice;
        }

        /// <summary>
        /// 获取Venus的枚举列表
        /// </summary>
        /// <param name="paramARMerchantCode">汽修商户编码</param>
        /// <param name="paramEnumType">枚举类型</param>
        /// <returns></returns>
        public static List<ComComboBoxDataSourceTC> GetVenusTypeEnumList(string paramARMerchantCode, string paramEnumType)
        {
            if (string.IsNullOrEmpty(paramEnumType) || string.IsNullOrEmpty(paramARMerchantCode))
            {
                return null;
            }
            List<ComComboBoxDataSourceTC> resulComboBoxDataSourceTcList = new List<ComComboBoxDataSourceTC>();
            try
            {
                DBManager.QueryForList<ComComboBoxDataSourceTC>(paramARMerchantCode, SQLID.COMM_SQL22, paramEnumType, resulComboBoxDataSourceTcList);
            }
            catch (Exception ex)
            {
                return null;
            }
            return resulComboBoxDataSourceTcList;
        }

        /// <summary>
        /// 从VenusDB获取配件批次号
        /// </summary>
        /// <param name="paramVenusDbConfig"></param>
        /// <param name="paramInventory">配件库存信息</param>
        /// <returns>批次号</returns>
        public static string GetBatchNoFromVenusDb(string paramVenusDbConfig, MDLAPM_Inventory paramInventory)
        {
            if (string.IsNullOrEmpty(paramInventory.INV_Org_ID))
            {
                MsgHelp.GetMsg(MsgCode.E_0001, new object[] { MsgParam.STOCKIN_ORG });
                return string.Empty;
            }
            if (string.IsNullOrEmpty(paramInventory.INV_Barcode))
            {
                MsgHelp.GetMsg(MsgCode.E_0001, new object[] { MsgParam.BARCODE });
                return string.Empty;
            }
            string dateString = GetCurStdDatetime().ToString("yyMMdd");
            string batchNo;
            object result = DBManager.QueryForObject(paramVenusDbConfig, SQLID.COMM_SQL23, paramInventory);
            if (result != null)
            {
                var index = Convert.ToInt32(result);
                if (index >= 99)
                {
                    MsgHelp.GetMsg(MsgCode.W_0015, new object[] { MsgParam.TODAY + MsgParam.BATCHNO, MsgParam.NINETY_NINE });
                    return string.Empty;
                }
                batchNo = dateString + (index + 1).ToString("00");
            }
            else
            {
                batchNo = dateString + "00";
            }
            return batchNo;
        }

        /// <summary>
        /// 获取配件批次号
        /// </summary>
        /// <param name="paramInventory">配件条码</param>
        /// <returns>批次号</returns>
        public static string GetBatchNo(MDLPIS_Inventory paramInventory)
        {
            if (string.IsNullOrEmpty(paramInventory.WHERE_INV_Org_ID))
            {
                MsgHelp.GetMsg(MsgCode.E_0001, new object[] { MsgParam.STOCKIN_ORG });
                return string.Empty;
            }
            if (string.IsNullOrEmpty(paramInventory.WHERE_INV_Barcode))
            {
                MsgHelp.GetMsg(MsgCode.E_0001, new object[] { MsgParam.BARCODE });
                return string.Empty;
            }
            string dateString = GetCurStdDatetime().ToString("yyMMdd");
            string batchNo;
            object resultBatchNoCount = _bllBase.QueryForObject(SQLID.COMM_SQL28, paramInventory);
            if (resultBatchNoCount != null)
            {
                var indexBatchNoCount = Convert.ToInt32(resultBatchNoCount);
                if (indexBatchNoCount >= 99)
                {
                    MsgHelp.GetMsg(MsgCode.W_0015, new object[] { MsgParam.TODAY + MsgParam.BATCHNO, MsgParam.NINETY_NINE });
                    return string.Empty;
                }
                batchNo = dateString + (indexBatchNoCount + 1).ToString("00");
            }
            else
            {
                batchNo = dateString + "00";
            }
            return batchNo;
        }

        #region 图片上传及查看

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="paramFilePath">本文件完整路径</param>
        /// <param name="paramFileName">文件名（可空，由系统定义GUID文件名）</param>
        /// <param name="paramOutMessageContent">错误消息</param>
        /// <returns>成功：图片网络路径（含Http）；失败：空串</returns>
        public static string UpLoadFile(string paramFilePath, string paramFileName, ref string paramOutMessageContent)
        {
            var funcName = "UpLoadFile";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);
            if (string.IsNullOrEmpty(paramFilePath))
            {
                paramOutMessageContent = "上传的文件路径为空";
                LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, funcName, paramOutMessageContent, "", null);
                return string.Empty;
            }

            #region 文件上传到云车文件服务

            string argsUrl = $"{ConfigurationManager.AppSettings[AppSettingKey.FILE_SERVICE_API_URL]}/api/file/do";
            string argsMctCode = ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE] ?? "";
            if (string.IsNullOrEmpty(argsUrl))
            {
                paramOutMessageContent = "文件上传失败,失败原因:获取文件服务地址失败";
                LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, funcName, paramOutMessageContent, "", null);
                return string.Empty;
            }

            string imageName = string.Empty;

            if (string.IsNullOrEmpty(paramFileName))
            {
                string fileType = paramFilePath.Substring(paramFilePath.LastIndexOf(".", StringComparison.Ordinal) + 1);
                if (fileType.ToLower() == "png")
                {
                    imageName = Guid.NewGuid() + ".png";
                }
                else if (fileType.ToLower() == "jpg")
                {
                    imageName = Guid.NewGuid() + ".jpg";
                }

                if (string.IsNullOrEmpty(imageName))
                {
                    paramOutMessageContent = "文件上传失败,失败原因:上传未知类型";
                    LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, funcName, paramOutMessageContent, "", null);
                    return string.Empty;
                }
            }
            else
            {
                imageName = paramFileName;
            }

            //表单参数						
            var txtPara = new Dictionary<string, string>()
            {
                //商户编码						
                {"MCTCode", argsMctCode},
                //文件操作（I:新增，U：更新，D：删除，Q：查询）						
                {"Action", "I"},
                //文件名						
                {"FileName", imageName},
            };
            //文件参数						
            var filePara = new Dictionary<string, FileItem>()
            {
                //Key:文件上传后的新文件名，Value:上传前本地的文件信息						
                {imageName,new FileItem(new FileInfo(paramFilePath))}
            };
            try
            {
                var util = new WebUtils();
                //执行带文件上传的HTTP POST请求。						
                var data = util.DoPost(argsUrl, txtPara, filePara);
                var jsonResult = (JObject)JsonConvert.DeserializeObject(data);
                if (jsonResult == null || !jsonResult[SysConst.EN_RESULTCODE].ToString().Equals(SysConst.EN_I0001))
                {
                    //文件上传失败	
                    if (jsonResult != null)
                    {
                        paramOutMessageContent = $"文件上传失败,失败原因:{jsonResult[SysConst.EN_RESULTMSG]}";
                    }

                    LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, funcName, paramOutMessageContent, "", null);
                    return string.Empty;
                }

                LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, $"jsonResult={jsonResult}", "", null);
                return jsonResult["AbsoluteUri"].ToString();
            }
            catch (Exception ex)
            {
                LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, funcName, ex.Message + ex.StackTrace, "", null);
                return string.Empty;
            }

            #endregion
        }

        /// <summary>
        /// 通过文件名获取BitmapImage对象
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="paramUseLocalProiority">优先使用本地文件</param>
        /// <returns>返回BitmapImage:null 表示获取失败，其他获取成功</returns>
        public static Bitmap GetBitmapImageByFileName(string fileName, bool paramUseLocalProiority = false)
        {
            string fileFullPath = GetFileByFileName(fileName, Environment.CurrentDirectory + @"\" + LocalConfigFileConst.FileFolderName.LocalFileBase + @"\", paramUseLocalProiority);
            if (string.IsNullOrEmpty(fileFullPath))
            {
                return null;
            }

            FileStream fileStream = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            //读取文件的 byte[] 
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            //把byte[]转换成Stream 
            Stream stream = new MemoryStream(bytes);

            return new Bitmap(stream);
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="paramFileName">文件名</param>
        /// <param name="paramDirectory">文件夹路径</param>
        /// <param name="paramUseLocalProiority">优先使用本地文件</param>
        /// <returns>文件全路径:空字符串表示文件获取失败，其他文件路径</returns>
        public static string GetFileByFileName(string paramFileName, string paramDirectory, bool paramUseLocalProiority = false)
        {
            try
            {
                string paramOutMessageContent = string.Empty;
                if (string.IsNullOrEmpty(paramFileName))
                {
                    paramOutMessageContent = "文件名为空";
                    return string.Empty;
                }
                string localFullFileName = paramDirectory + paramFileName;
                string argsMctCode = ConfigurationManager.AppSettings["MctCode"].ToString();

                string argsUrl = ConfigurationManager.AppSettings["FileServiceAPIURL"] + string.Format("API/File/Get?MCTCode={0}&FileName={1}", argsMctCode, paramFileName);

                if (string.IsNullOrEmpty(argsUrl))
                {
                    paramOutMessageContent = "下载文件失败,失败原因:获取文件服务地址失败";
                    return string.Empty;
                }
                if (paramUseLocalProiority)
                {
                    if (File.Exists(localFullFileName))
                    {
                        return localFullFileName;
                    }
                }
                DateTime? filesLastModifyTime = GetFilesLastModifyTime(paramFileName);
                if (filesLastModifyTime != null)
                {
                    bool needDownloadFile = false;
                    if (File.Exists(localFullFileName))
                    {
                        FileInfo fileInfo = new FileInfo(localFullFileName);
                        if (fileInfo.LastWriteTime < filesLastModifyTime)
                        {
                            needDownloadFile = true;
                        }
                    }
                    else
                    {
                        needDownloadFile = true;
                    }
                    if (needDownloadFile)
                    {
                        if (!Directory.Exists(paramDirectory))
                        {
                            Directory.CreateDirectory(paramDirectory);
                        }
                        var util = new WebUtils();
                        string errorInfo = string.Empty;
                        var downLoadResult = util.DownloadFileByWebClient(argsUrl, localFullFileName, out errorInfo);
                        if (downLoadResult)
                        {
                            return localFullFileName;
                        }
                    }
                    return localFullFileName;
                }
                else
                {
                    if (File.Exists(localFullFileName))
                    {

                        File.Delete(localFullFileName);
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取文件的最后更新时间
        /// </summary>
        /// <param name="paramFileName"></param>
        /// <returns></returns>
        public static DateTime? GetFilesLastModifyTime(string paramFileName)
        {
            try
            {
                string argsMctCode = ConfigurationManager.AppSettings["MctCode"];

                string queryUrl = ConfigurationManager.AppSettings["FileServiceAPIURL"] + "API/File/Get?";

                var util = new WebUtils();
                DateTime lastModidyDateTime = new DateTime();
                bool tryResult = false;
                //paramFileName = System.Web.HttpUtility.UrlEncode(paramFileName, Encoding.UTF8);

                var data = util.DoPost(queryUrl, string.Format("MCTCode={0}&Action={1}&FileName={2}", argsMctCode, "Q", paramFileName));
                //var data = util.DoPost(queryUrl + string.Format("MCTCode={0}&FileName={1}", argsMctCode, paramFileName), paramFileName);
                var jsonResult = (JObject)JsonConvert.DeserializeObject(data);
                if (jsonResult != null && jsonResult["ResultCode"].ToString().Equals("I0001") && jsonResult["LastWriteTime"] != null)
                {
                    tryResult = DateTime.TryParse(jsonResult["LastWriteTime"].ToString(), out lastModidyDateTime);
                }
                if (tryResult)
                {
                    return lastModidyDateTime;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }
        }

        #endregion

        #region 图片保存、导出、删除

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="paramSourceFilePath">源文件路径</param>
        /// <param name="paramFileName">保存的文件名称</param>
        /// <param name="paramFileFullUrl">文件服务器路径</param>
        /// <returns>true，保存成功；false，保存失败</returns>
        public static bool SaveFileByFileName(string paramSourceFilePath, string paramFileName, ref string paramFileFullUrl)
        {
            if (!string.IsNullOrEmpty(paramFileName))
            {
                string fileFullPath = Application.StartupPath + @"\" + LocalConfigFileConst.FileFolderName.LocalFileBase + @"\" + paramFileName;

                //保存到本地
                File.Copy(paramSourceFilePath, fileFullPath, true);

                //TODO 测试
                //MessageBox.Show(MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "程序运行路径：" + Application.StartupPath }), SysConst.CHS_MSG_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);

                //MessageBox.Show(MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "源文件路径：" + paramSourceFilePath }), SysConst.CHS_MSG_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);

                //MessageBox.Show(MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "目的文件路径：" + fileFullPath }), SysConst.CHS_MSG_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);

                //上传到文件服务器
                string outMessageContent = string.Empty;
                paramFileFullUrl = UpLoadFile(fileFullPath, paramFileName, ref outMessageContent);
                if (string.IsNullOrEmpty(paramFileFullUrl)
                    || !string.IsNullOrEmpty(outMessageContent))
                {
                    //上传服务器失败的场合，删除本地文件
                    if (File.Exists(fileFullPath))
                    {
                        File.Delete(fileFullPath);
                    }
                    return false;
                }
                return true;
            }
            return true;
        }

        /// <summary>
        /// 导出图片
        /// </summary>
        /// <param name="paramFileName">文件名</param>
        /// <param name="paramOutMessageContent">错误消息</param>
        public static bool ExportFileByFileName(string paramFileName, ref string paramOutMessageContent)
        {
            var funcName = "ExportFileByFileName";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);

            if (paramFileName == null)
            {
                paramOutMessageContent = MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "图片为空，请先上传图片" });
                LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, funcName, paramOutMessageContent, "", null);
                return false;
            }
            //获取文件服务地址
            string argsMctCode = ConfigurationManager.AppSettings["MctCode"];
            string argsUrl = ConfigurationManager.AppSettings["FileServiceAPIURL"] +
                             $"API/File/Get?MCTCode={argsMctCode}&FileName={paramFileName}";
            if (string.IsNullOrEmpty(argsUrl))
            {
                //导出失败，失败原因：获取文件服务地址失败
                paramOutMessageContent = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.EXPORT, "获取文件服务地址失败" });
                return false;
            }

            SaveFileDialog saveImageDialog = new SaveFileDialog
            {
                Title = "图片保存",
                //文件类型
                Filter = @"jpeg|*.jpg|bmp|*.bmp|gif|*.gif",
                //默认文件名
                FileName = paramFileName,
                //保存对话框是否记忆上次打开的目录
                RestoreDirectory = true,
            };
            if (saveImageDialog.ShowDialog() != DialogResult.OK)
            {
                paramOutMessageContent = string.Empty;
                return true;
            }
            string destFileName = saveImageDialog.FileName;
            if (string.IsNullOrEmpty(destFileName))
            {
                paramOutMessageContent = string.Empty;
                return true;
            }

            try
            {
                var util = new WebUtils();
                string errorInfo = string.Empty;
                //下载文件
                var downLoadResult = util.DownloadFileByWebClient(argsUrl, destFileName, out errorInfo);
                if (downLoadResult)
                {
                    //导出成功
                    LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, paramOutMessageContent, "",
                        null);
                    paramOutMessageContent = MsgHelp.GetMsg(MsgCode.I_0001, new object[] { SystemActionEnum.Name.EXPORT });
                    return true;
                }
                //导出失败，失败原因：下载文件失败
                paramOutMessageContent = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.EXPORT, "下载文件失败" });
                LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, funcName, paramOutMessageContent, "",
                    null);
                return false;
            }
            catch (Exception ex)
            {
                //导出失败，失败原因：ex.Message
                paramOutMessageContent = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.EXPORT, ex.Message });
                LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, funcName, paramOutMessageContent, "", null);
                return false;
            }
        }

        /// <summary>
        /// 导出多张图片
        /// </summary>
        /// <param name="paramFileNameLists">文件名列表</param>
        /// <param name="paramDefaultFilePath"></param>
        /// <param name="paramOutMessageContent">错误消息</param>
        public static bool ExportFileListByFileNameList(List<string> paramFileNameLists, ref string paramDefaultFilePath, ref string paramOutMessageContent)
        {
            var funcName = "ExportFileListByFileName";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);

            if (paramFileNameLists == null
                || paramFileNameLists.Count == 0)
            {
                paramOutMessageContent = MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "图片为空，请先上传图片" });
                LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, funcName, paramOutMessageContent, "", null);
                return false;
            }

            FolderBrowserDialog saveImageDialog = new FolderBrowserDialog()
            {
                ShowNewFolderButton = true,
            };
            if (!string.IsNullOrEmpty(paramDefaultFilePath))
            {
                //默认路径
                saveImageDialog.SelectedPath = paramDefaultFilePath;
            }
            if (saveImageDialog.ShowDialog() != DialogResult.OK)
            {
                return true;
            }
            if (string.IsNullOrEmpty(saveImageDialog.SelectedPath))
            {
                return true;
            }
            paramDefaultFilePath = saveImageDialog.SelectedPath;

            try
            {
                string argsMctCode = ConfigurationManager.AppSettings["MctCode"];
                foreach (var loopFileName in paramFileNameLists)
                {
                    //获取文件服务地址
                    string argsUrl = ConfigurationManager.AppSettings["FileServiceAPIURL"] +
                                     $"API/File/Get?MCTCode={argsMctCode}&FileName={loopFileName}";
                    if (string.IsNullOrEmpty(argsUrl))
                    {
                        //导出失败，失败原因：获取文件服务地址失败
                        paramOutMessageContent = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.EXPORT, "获取文件服务地址失败" });
                        return false;
                    }

                    var util = new WebUtils();
                    string errorInfo = string.Empty;
                    //选择的文件夹及文件名（默认原文件名）
                    var destFileName = saveImageDialog.SelectedPath + @"\" + loopFileName;
                    //下载文件
                    var downLoadResult = util.DownloadFileByWebClient(argsUrl, destFileName, out errorInfo);
                    if (!downLoadResult)
                    {
                        //导出失败，失败原因：下载文件失败
                        paramOutMessageContent = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.EXPORT, "下载文件失败" });
                        LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, funcName, paramOutMessageContent, "",
                            null);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                //导出失败，失败原因：ex.Message
                paramOutMessageContent = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.EXPORT, ex.Message });
                LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, funcName, paramOutMessageContent, "", null);
                return false;
            }
            //导出成功
            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, paramOutMessageContent, "",
                null);
            return true;
        }

        /// <summary>
        /// 通过文件名删除本地和文件服务器中的文件
        /// </summary>
        /// <param name="paramFileName">文件名</param>
        /// <param name="paramOutMessageContent">错误消息</param>
        /// <returns></returns>
        public static bool DeleteFileByFileName(string paramFileName, ref string paramOutMessageContent)
        {
            var funcName = "DeleteFileByFileName";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);

            if (paramFileName == null)
            {
                paramOutMessageContent = MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "图片为空，删除失败" });
                LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, funcName, paramOutMessageContent, "", null);
                return false;
            }
            try
            {
                //从本地删除
                string fileFullPath = Environment.CurrentDirectory + @"\" + LocalConfigFileConst.FileFolderName.LocalFileBase + @"\" + paramFileName;

                //从文件服务器上删除
                DateTime? filesLastModifyTime = GetFilesLastModifyTime(paramFileName);
                if (filesLastModifyTime != null)
                {
                    if (string.IsNullOrEmpty(paramFileName))
                    {
                        paramOutMessageContent = "文件名为空";
                        LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, funcName, paramOutMessageContent, "", null);
                        return false;
                    }

                    string argsUrl = $"{ConfigurationManager.AppSettings[AppSettingKey.FILE_SERVICE_API_URL]}/api/file/do";
                    string argsMctCode = ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE] ?? "";
                    if (string.IsNullOrEmpty(argsUrl))
                    {
                        paramOutMessageContent = "删除文件失败,失败原因:获取文件服务地址失败";
                        LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, funcName, paramOutMessageContent, "", null);
                        return false;
                    }

                    //表单参数						
                    var txtPara = new Dictionary<string, string>()
                    {
                        //商户编码						
                        {"MCTCode", argsMctCode},
                        //文件操作（I:新增，U：更新，D：删除，Q：查询）						
                        {"Action", "D"},
                        //文件名						
                        {"FileName", paramFileName},
                    };
                    //文件参数						
                    var filePara = new Dictionary<string, FileItem>()
                    {
                        //Key:文件上传后的新文件名，Value:上传前本地的文件信息						
                        {paramFileName, new FileItem(new FileInfo(fileFullPath))}
                    };
                    var util = new WebUtils();
                    //执行带文件上传的HTTP POST请求。						
                    var data = util.DoPost(argsUrl, txtPara, filePara);
                    var jsonResult = (JObject)JsonConvert.DeserializeObject(data);
                    if (jsonResult == null || !jsonResult[SysConst.EN_RESULTCODE].ToString().Equals(SysConst.EN_I0001))
                    {
                        //文件删除失败	
                        if (jsonResult != null)
                        {
                            paramOutMessageContent = $"删除文件失败,失败原因:{jsonResult[SysConst.EN_RESULTMSG]}";
                        }

                        LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, funcName, paramOutMessageContent, "", null);
                        return false;
                    }

                    //从本地删除
                    if (File.Exists(fileFullPath))
                    {
                        File.Delete(fileFullPath);
                    }

                    LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, $"jsonResult={jsonResult}", "", null);
                    return true;
                }
            }
            catch (Exception ex)
            {
                //删除失败，失败原因：ex.Message
                paramOutMessageContent = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.DELETE, ex.Message });
                LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, funcName, ex.Message + ex.StackTrace, "", null);
                return false;
            }
            return true;
        }

        #endregion

        #region 获取供应商列表

        /// <summary>
        /// 获取供应商列表
        /// </summary>
        /// <returns></returns>
        public static List<MDLPIS_Supplier> GetSupplierList()
        {
            List<MDLPIS_Supplier> resultSupplierList = new List<MDLPIS_Supplier>();
            _bllBase.QueryForList<MDLPIS_Supplier, MDLPIS_Supplier>(new MDLPIS_Supplier()
            {
                WHERE_SUPP_IsValid = true
            }, resultSupplierList);
            return resultSupplierList;
        }

        #endregion

        #region 获取仓库列表

        /// <summary>
        /// 获取仓库列表
        /// </summary>
        /// <param name="paramOrgId">组织ID</param>
        /// <returns></returns>
        public static List<MDLPIS_Warehouse> GetWarehouseList(string paramOrgId)
        {
            List<MDLPIS_Warehouse> resultWarehouseList = new List<MDLPIS_Warehouse>();
            //获取paramOrgId组织下的所有仓库
            _bllBase.QueryForList<MDLPIS_Warehouse, MDLPIS_Warehouse>(new MDLPIS_Warehouse()
            {
                WHERE_WH_Org_ID = paramOrgId,
                WHERE_WH_IsValid = true
            }, resultWarehouseList);
            return resultWarehouseList;
        }

        #endregion

        #region 获取仓位列表

        /// <summary>
        /// 获取仓位列表
        /// </summary>
        /// <param name="paramOrgId">组织ID</param>
        /// <param name="paramWarehouseId">仓库ID</param>
        /// <returns></returns>
        public static List<MDLPIS_WarehouseBin> GetWarehouseBinList(string paramOrgId, string paramWarehouseId = null)
        {
            List<MDLPIS_WarehouseBin> resultWarehouseBinList = new List<MDLPIS_WarehouseBin>();

            if (string.IsNullOrEmpty(paramWarehouseId))
            {
                //未指定仓库时，获取paramOrgId组织下所有的仓位
                _bllBase.QueryForList(SQLID.COMM_SQL33, new MDLPIS_Warehouse
                {
                    WHERE_WH_Org_ID = paramOrgId
                }, resultWarehouseBinList);
            }
            else
            {
                //指定仓库时，获取paramWarehouseId仓库下所有的仓位
                _bllBase.QueryForList<MDLPIS_WarehouseBin, MDLPIS_WarehouseBin>(new MDLPIS_WarehouseBin
                {
                    WHERE_WHB_WH_ID = paramWarehouseId,
                    WHERE_WHB_IsValid = true
                }, resultWarehouseBinList);
            }

            return resultWarehouseBinList;
        }

        #endregion

        #region 验证用户在对应组织下的权限

        /// <summary>
        /// 验证用户在对应组织下某个菜单下的动作权限
        /// </summary>
        /// <param name="paranOrgId">组织ID</param>
        /// <param name="paramActionKey">动作按钮Key</param>
        /// <param name="paramMenuId">菜单ID</param>
        /// <returns>true：有权限，false，无权限</returns>
        public static bool HasAuthorityInOrg(string paranOrgId, string paramActionKey, string paramMenuId)
        {
            //superadmin拥有所有权限
            if (LoginInfoDAX.UserID == SysConst.SUPER_ADMIN)
            {
                return true;
            }
            if (string.IsNullOrEmpty(paranOrgId))
            {
                return false;
            }
            AuthorityVerifyQCModel argsAuthorityVeify = new AuthorityVerifyQCModel
            {
                OrgId = paranOrgId,
                UserId = LoginInfoDAX.UserID,
                MenuId = paramMenuId,
                ActionKey = paramActionKey
            };
            //如果是当前组织，先查缓存。
            if (paranOrgId == LoginInfoDAX.OrgID)
            {
                var cacheObj = CacheDAX.Get(CacheDAX.ConfigDataKey.SystemUserAuthority);
                if (cacheObj != null)
                {
                    var actionList = new List<MenuGroupActionUIModel>(
                            ((List<MenuGroupActionUIModel>)cacheObj).Where(
                                x => x.MenuD_ID == argsAuthorityVeify.MenuId && x.Act_Key == argsAuthorityVeify.ActionKey).ToList());
                    if (actionList.Count > 0)
                    {
                        return true;
                    }
                }
            }

            var resultCount = (int)_bllBase.QueryForObject(SQLID.COMM_SQL34, argsAuthorityVeify);
            if (resultCount == 0)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region 获取汽修商/普通客户信息

        /// <summary>
        /// 获取汽修商/普通客户信息
        /// </summary>
        /// <param name="paramCustomerType">客户类型</param>
        /// <param name="paramCustomerId">客户ID</param>
        /// <param name="paramCustomerName">客户名称</param>
        /// <param name="paramAutoFactoryCode">汽修商户编码</param>
        /// <returns></returns>
        public static List<CustomerQueryUIModel> GetCustomerInfo(string paramCustomerType, string paramCustomerId = "", string paramCustomerName = "", string paramAutoFactoryCode = "")
        {
            List<CustomerQueryUIModel> resultCustomerList = new List<CustomerQueryUIModel>();
            _bllBase.QueryForList(SQLID.COMM_SQL11, new CustomerQueryQCModel
            {
                //客户类型
                WHERE_CustomerType = paramCustomerType,
                //客户ID
                WHERE_CustomerID = paramCustomerId,
                //客户名称
                WHERE_CustomerName = paramCustomerName,
                //汽修商户编码
                WHERE_AutoFactoryCode = paramAutoFactoryCode,
                //组织ID
                WHERE_OrgID = LoginInfoDAX.UserID == SysConst.SUPER_ADMIN ? null : LoginInfoDAX.OrgID,
            }, resultCustomerList);

            if (paramCustomerType == CustomerTypeEnum.Name.PTNQXSH)
            {
                //[客户类型]为{平台内汽修商}的场合，获取汽修商组织ID
                foreach (var loopCustomer in resultCustomerList)
                {
                    if (string.IsNullOrEmpty(loopCustomer.AutoFactoryCode))
                    {
                        continue;
                    }

                    //根据指定的汽修商户数据库信息获取Venus组织列表
                    List<MDLSM_Organization> tempVenusOrgList = new List<MDLSM_Organization>();
                    BLLCom.QueryAutoFactoryCustomerOrgList(loopCustomer.AutoFactoryCode, tempVenusOrgList);
                    foreach (var loopVenusOrg in tempVenusOrgList)
                    {
                        if (loopCustomer.AutoFactoryOrgCode == loopVenusOrg.Org_Code
                            && loopCustomer.CustomerName == loopVenusOrg.Org_ShortName)
                        {
                            loopCustomer.AutoFactoryOrgID = loopVenusOrg.Org_ID;
                        }
                    }
                }
            }

            return resultCustomerList;
        }

        #endregion

        #region 钱包相关

        /// <summary>
        /// 生成钱包异动
        /// </summary>
        /// <param name="paramWalletTrans">钱包异动</param>
        /// <returns></returns>
        public static MDLEWM_WalletTrans CreateWalletTrans(MDLEWM_WalletTrans paramWalletTrans)
        {
            DateTime curDateTime = BLLCom.GetCurStdDatetime();
            MDLEWM_WalletTrans walletTrans = new MDLEWM_WalletTrans
            {
                WalT_ID = Guid.NewGuid().ToString(),
                WalT_Org_ID = string.IsNullOrEmpty(paramWalletTrans.WalT_Org_ID) ? LoginInfoDAX.OrgID : paramWalletTrans.WalT_Org_ID,
                WalT_Org_Name = string.IsNullOrEmpty(paramWalletTrans.WalT_Org_Name) ? LoginInfoDAX.OrgShortName : paramWalletTrans.WalT_Org_Name,
                WalT_Wal_ID = paramWalletTrans.WalT_Wal_ID,
                WalT_Wal_No = paramWalletTrans.WalT_Wal_No,
                WalT_Time = curDateTime,
                WalT_TypeName = paramWalletTrans.WalT_TypeName,
                WalT_TypeCode = paramWalletTrans.WalT_TypeCode,
                WalT_Amount = paramWalletTrans.WalT_Amount,
                WalT_BillNo = paramWalletTrans.WalT_BillNo,
                WalT_ChannelName = paramWalletTrans.WalT_ChannelName,
                WalT_ChannelCode = paramWalletTrans.WalT_ChannelCode,
                WalT_RechargeTypeName = paramWalletTrans.WalT_RechargeTypeName,
                WalT_RechargeTypeCode = paramWalletTrans.WalT_RechargeTypeCode,
                WalT_Remark = paramWalletTrans.WalT_Remark,
                WalT_IsValid = true,
                WalT_CreatedBy = LoginInfoDAX.UserName,
                WalT_CreatedTime = curDateTime,
                WalT_UpdatedBy = LoginInfoDAX.UserName,
                WalT_UpdatedTime = curDateTime,
                WalT_VersionNo = 1
            };
            return walletTrans;
        }

        /// <summary>
        /// 根据所有人信息获取钱包列表
        /// </summary>
        /// <param name="paramOwnerType">所有人类别</param>
        /// <param name="paramCustomerId">客户ID</param>
        /// <param name="paramAutoFactoryCode">汽修商户编码</param>
        /// <param name="paramAutoFactoryOrgCode">汽修商组织编码</param>
        /// <returns>钱包Model</returns>
        public static List<WalletInfoUIModel> GetWalletListByOwnerInfo(string paramOwnerType, string paramCustomerId, string paramAutoFactoryCode = "", string paramAutoFactoryOrgCode = "")
        {
            if (string.IsNullOrEmpty(paramCustomerId))
            {
                //客户ID不能为空
                MessageBox.Show(MsgHelp.GetMsg(MsgCode.E_0001, new object[] { MsgParam.CUST_ID }), SysConst.CHS_MSG_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<WalletInfoUIModel>();
            }

            List<WalletInfoUIModel> resultWalletList = new List<WalletInfoUIModel>();
            _bllBase.QueryForList(SQLID.COMM_SQL44, new MDLEWM_Wallet
            {
                WHERE_Wal_OwnerTypeName = paramOwnerType,
                WHERE_Wal_CustomerID = paramCustomerId,
                WHERE_Wal_AutoFactoryCode = paramAutoFactoryCode,
                WHERE_Wal_AutoFactoryOrgCode = paramAutoFactoryOrgCode,
                WHERE_Wal_IsValid = true
            }, resultWalletList);

            return resultWalletList;
        }

        /// <summary>
        /// 根据钱包账号获取钱包信息
        /// </summary>
        /// <param name="paramWalletNo">钱包账号</param>
        /// <param name="paramWalletStatus">钱包状态</param>
        /// <param name="paramIsValid">是否有效</param>
        /// <returns></returns>
        public static WalletInfoUIModel GetWalletByWalletNo(string paramWalletNo, string paramWalletStatus = WalletStatusEnum.Name.ZC, bool? paramIsValid = true)
        {
            if (string.IsNullOrEmpty(paramWalletNo))
            {
                //钱包账号不能为空
                MessageBox.Show(MsgHelp.GetMsg(MsgCode.E_0001, new object[] { }), SysConst.CHS_MSG_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new WalletInfoUIModel();
            }

            WalletInfoUIModel resultWallet = new WalletInfoUIModel();
            resultWallet = _bllBase.QueryForObject<WalletInfoUIModel>(SQLID.COMM_SQL44, new MDLEWM_Wallet
            {
                WHERE_Wal_No = paramWalletNo,
                WHERE_Wal_StatusName = paramWalletStatus,
                WHERE_Wal_IsValid = paramIsValid,
            });

            return resultWallet;
        }

        /// <summary>
        /// 验证钱包状态
        /// </summary>
        /// <param name="paramWalletTransType">异动类型（枚举）</param>
        /// <param name="paramTransAmount">异动金额（消费时传入，不用时传0）</param>
        /// <param name="paramCustomerId">客户ID（开户时传入，不用时传空）</param>
        /// <param name="paramExistsWalletNo">已有钱包账号（除了开户，其他异动都需传入）</param>
        /// <param name="paramExistsWalletPwd">已有钱包账号的密文密码（消费时传入）</param>
        /// <param name="paramExistsWalletNewPwd">已有钱包账号的新密文密码（修改密码时传入）</param>
        /// <param name="paramNewWalletNo">新钱包账号（开户，补办时传入）</param>
        /// 
        /// <param name="outValidateResultMessage">返回验证结果</param>
        /// <param name="outNewValidWalletNo">返回补办后的钱包账号（退款时返回）</param>
        /// <param name="outResultWallet"></param>
        /// <returns>返回验证是否通过,有三种情况；
        /// 1.返回true,且outValidateResultMessage非空，弹出【是】【否】对话框，选【是】继续执行;
        /// 2.返回true,且outValidateResultMessage为空，继续执行;
        /// 3.返回false,且outValidateResultMessage非空，弹出【是】对话框，选【是】停止执行;
        /// 4.返回false,且outValidateResultMessage为空，停止执行;
        /// </returns>
        public static bool ValidateWallet(string paramWalletTransType, decimal paramTransAmount, string paramCustomerId, string paramExistsWalletNo, string paramExistsWalletPwd, string paramExistsWalletNewPwd, string paramNewWalletNo, ref string outValidateResultMessage, ref string outNewValidWalletNo, ref MDLEWM_Wallet outResultWallet)
        {
            //返回结果
            bool validateResult = true;
            //返回结果消息
            outValidateResultMessage = string.Empty;

            WalletInfoUIModel existsWallet = new WalletInfoUIModel();
            switch (paramWalletTransType)
            {
                #region 钱包异动类型-开户
                case WalTransTypeEnum.Name.KH:
                    List<WalletInfoUIModel> walletOfCurCustomer = GetWalletListByOwnerInfo(null, paramCustomerId);
                    if (walletOfCurCustomer.Count > 0)
                    {
                        outValidateResultMessage = "开户人已有" + walletOfCurCustomer.Count + "个钱包，不能" + WalTransTypeEnum.Name.KH;
                        validateResult = false;
                    }
                    else
                    {
                        //获取钱包(包括无效)
                        existsWallet = GetWalletByWalletNo(paramNewWalletNo, null, null);
                        if (!string.IsNullOrEmpty(existsWallet.Wal_No))
                        {
                            outValidateResultMessage = "新钱包账号：" + existsWallet.Wal_No + "已开户，当前状态为：" + existsWallet.Wal_StatusName + "，不能" + WalTransTypeEnum.Name.KH;
                            return false;
                        }
                    }
                    break;

                #endregion

                #region 钱包异动类型-充值
                case WalTransTypeEnum.Name.CZ:
                    if (string.IsNullOrEmpty(paramExistsWalletNo))
                    {
                        outValidateResultMessage = "没有获取到钱包账号";
                        validateResult = false;
                    }
                    else
                    {
                        existsWallet = GetWalletByWalletNo(paramExistsWalletNo, null);
                        if (existsWallet == null || string.IsNullOrEmpty(existsWallet.Wal_ID))
                        {
                            outValidateResultMessage = "钱包不存在，请先" + WalTransTypeEnum.Name.KH;
                            validateResult = false;
                        }
                        else
                        {
                            if (existsWallet.Wal_StatusName != WalletStatusEnum.Name.ZC)
                            {
                                outValidateResultMessage = "钱包状态为：" + existsWallet.Wal_StatusName + "，不能" + WalTransTypeEnum.Name.CZ;
                                validateResult = false;
                            }
                            else if (existsWallet.Wal_FreezingBalance > 0)
                            {
                                outValidateResultMessage = "钱包有冻结金额，确定充值吗？";
                                validateResult = true;
                            }
                            else
                            {
                                outValidateResultMessage = string.Empty;
                                validateResult = true;
                            }
                        }
                    }
                    break;
                #endregion

                #region 钱包异动类型-消费
                case WalTransTypeEnum.Name.XF:
                    if (string.IsNullOrEmpty(paramExistsWalletNo))
                    {
                        outValidateResultMessage = "没有获取到钱包账号";
                        validateResult = false;
                    }
                    else
                    {
                        existsWallet = GetWalletByWalletNo(paramExistsWalletNo, null);
                        if (existsWallet == null || string.IsNullOrEmpty(existsWallet.Wal_ID))
                        {
                            outValidateResultMessage = "钱包不存在，\n" +
                                                       "请先做钱包开户\n" +
                                                       "或\n" +
                                                       "点单据号去原始单据页改为非钱包结算";
                            validateResult = false;
                        }
                        else
                        {
                            if (existsWallet.Wal_StatusName != WalletStatusEnum.Name.ZC)
                            {
                                outValidateResultMessage = "钱包状态为：" + existsWallet.Wal_StatusName + "，不能" + WalTransTypeEnum.Name.XF;
                                validateResult = false;
                            }
                            else
                            {
                                if (paramExistsWalletPwd == string.Empty)
                                {
                                    //可用余额
                                    decimal availableBalance = 0;
                                    if (existsWallet.Wal_AvailableBalance != null)
                                    {
                                        availableBalance = (decimal)existsWallet.Wal_AvailableBalance;
                                    }
                                    //冻结金额
                                    decimal freezingBalance = 0;
                                    if (existsWallet.Wal_FreezingBalance != null)
                                    {
                                        freezingBalance = (decimal)existsWallet.Wal_FreezingBalance;
                                    }
                                    if (availableBalance < paramTransAmount)
                                    {
                                        if (freezingBalance > 0)
                                        {
                                            outValidateResultMessage = "钱包可用余额不足，现有冻结金额：" + freezingBalance.ToString(FormatConst.MONEY_FORMAT_01);
                                            validateResult = false;
                                        }
                                        else
                                        {
                                            outValidateResultMessage = "钱包可用余额不足";
                                            validateResult = false;
                                        }
                                    }
                                    else
                                    {
                                        outValidateResultMessage = string.Empty;
                                        validateResult = true;
                                    }
                                }
                                else
                                {
                                    outValidateResultMessage = "密码错误，不能消费";
                                    validateResult = false;
                                }
                            }
                        }
                    }
                    break;
                #endregion

                #region 钱包异动类型-提现
                case WalTransTypeEnum.Name.TX:
                    if (string.IsNullOrEmpty(paramExistsWalletNo))
                    {
                        outValidateResultMessage = "没有获取到钱包账号";
                        validateResult = false;
                    }
                    else
                    {
                        existsWallet = GetWalletByWalletNo(paramExistsWalletNo, null);
                        if (existsWallet == null)
                        {
                            outValidateResultMessage = "钱包不存在，不能" + WalTransTypeEnum.Name.TX;
                            validateResult = false;
                        }
                        else if (existsWallet.Wal_StatusName != WalletStatusEnum.Name.ZC)
                        {
                            outValidateResultMessage = "钱包状态为：" + existsWallet.Wal_StatusName + "，不能" + WalTransTypeEnum.Name.TX;
                            validateResult = false;
                        }
                        else if (existsWallet.Wal_AvailableBalance <= 0)
                        {
                            outValidateResultMessage = "钱包的可用余额小于等于0，不能" + WalTransTypeEnum.Name.TX;
                            validateResult = false;
                        }
                        else
                        {
                            outValidateResultMessage = string.Empty;
                            validateResult = true;
                        }
                    }
                    break;
                #endregion

                #region 钱包异动类型-销户
                case WalTransTypeEnum.Name.XH:
                    if (string.IsNullOrEmpty(paramExistsWalletNo))
                    {
                        outValidateResultMessage = "没有获取到钱包账号";
                        validateResult = false;
                    }
                    else
                    {
                        existsWallet = GetWalletByWalletNo(paramExistsWalletNo, null);
                        if (existsWallet == null)
                        {
                            outValidateResultMessage = "钱包不存在，不能" + WalTransTypeEnum.Name.XH;
                            validateResult = false;
                        }
                        else if (existsWallet.Wal_StatusName != WalletStatusEnum.Name.ZC)
                        {
                            outValidateResultMessage = "钱包状态为：" + existsWallet.Wal_StatusName + "，不能" + WalTransTypeEnum.Name.XH;
                            validateResult = false;
                        }
                        else
                        {
                            outValidateResultMessage = string.Empty;
                            validateResult = true;

                            var noSettleBill = GetNoSettleBillByCustomerID(existsWallet.Wal_CustomerID);
                            if (noSettleBill.Count > 0)
                            {
                                if (outValidateResultMessage.Length > 0)
                                {
                                    outValidateResultMessage = "钱包的开户人还有未结算的单据，请先收款";
                                }
                            }

                            decimal availableBalance = 0;
                            if (existsWallet.Wal_AvailableBalance != null)
                            {
                                availableBalance = (decimal)existsWallet.Wal_AvailableBalance;
                            }
                            if (availableBalance > 0)
                            {
                                outValidateResultMessage += (!string.IsNullOrEmpty(outValidateResultMessage) ? "\n" : string.Empty) + "钱包还有余额，请先提现所有余额";
                                validateResult = false;
                            }
                            else
                            {
                                outValidateResultMessage = string.Empty;
                                validateResult = true;
                            }
                        }
                    }
                    break;
                #endregion

                default:
                    outValidateResultMessage = "不支持的异动类型";
                    outNewValidWalletNo = string.Empty;
                    validateResult = false;
                    break;
            }
            if (validateResult)
            {
                _bllBase.CopyModel(existsWallet, outResultWallet);
            }
            return validateResult;
        }

        #endregion

        #region 获取供应商+汽修组织+普通客户列表
        /// <summary>
        /// 查询所有客户
        /// </summary>
        /// <param name="paramSupplierFirstlyDisplay">供应商优先显示</param>
        /// <param name="paramOrgID"></param>
        /// <returns></returns>
        public static List<ComClientUIModel> GetAllCustomerList(bool paramSupplierFirstlyDisplay, string paramOrgID)
        {
            List<ComClientUIModel> resultCustomerList = new List<ComClientUIModel>();
            _bllBase.QueryForList(SQLID.COMM_SQL48, new ComClientUIModel
            {
                SupplierFirstlyDisplay = paramSupplierFirstlyDisplay,
                OrgID = paramOrgID
            }, resultCustomerList);
            return resultCustomerList;
        }

        /// <summary>
        /// 查询所有客户
        /// </summary>
        /// <param name="paramOrgID"></param>
        /// <returns></returns>
        public static List<ComClientUIModel> GetAllCustomerList(string paramOrgID)
        {
            List<ComClientUIModel> resultCustomerList = new List<ComClientUIModel>();
            _bllBase.QueryForList(SQLID.COMM_SQL50, new ComClientUIModel
            {
                OrgID = paramOrgID
            }, resultCustomerList);
            return resultCustomerList;
        }

        /// <summary>
        /// 查询所有物流人员
        /// </summary>
        /// <param name="paramOrgId">组织ID</param>
        /// <returns></returns>
        public static List<ComClientUIModel> GetAllLogisticsList(string paramOrgId)
        {
            List<ComClientUIModel> resultCustomerList = new List<ComClientUIModel>();
            _bllBase.QueryForList(SQLID.COMM_SQL51, new ComClientUIModel
            {
                OrgID = paramOrgId
            }, resultCustomerList);
            return resultCustomerList;
        }
        #endregion

        #region 获取客户的欠款
        /// <summary>
        /// 查询客户欠款
        /// </summary>
        /// <param name="paramReceiveOrgID">收款组织ID</param>
        /// <param name="paramClientID">客户ID</param>
        /// <returns></returns>
        public static Decimal GetClientArrears(string paramReceiveOrgID, string paramClientID)
        {
            MDLFM_AccountReceivableBill argsAccountReceivableBill = new MDLFM_AccountReceivableBill
            {
                WHERE_ARB_Org_ID = paramReceiveOrgID,
                WHERE_ARB_PayObjectID = paramClientID
            };

            Decimal amount = _bllBase.QueryForObject<Decimal>(SQLID.COMM_SQL52, argsAccountReceivableBill);
            return amount;
        }

        /// <summary>
        /// 根据客户ID获取未结算的应收单
        /// </summary>
        /// <param name="paramCustomerID">客户ID</param>
        /// <returns></returns>
        public static List<MDLFM_AccountReceivableBill> GetNoSettleBillByCustomerID(string paramCustomerID)
        {
            List<MDLFM_AccountReceivableBill> noSettleBillList = new List<MDLFM_AccountReceivableBill>();
            _bllBase.QueryForList(SQLID.COMM_SQL54, new MDLFM_AccountReceivableBill
            {
                WHERE_ARB_PayObjectID = paramCustomerID,
            }, noSettleBillList);

            return noSettleBillList;
        }

        #endregion

        #region 全/半角转换

        /// <summary>
        /// 半角转全角
        /// </summary>
        /// <param name="paramInput">传入的任意字符串</param>
        /// <returns>全角字符串</returns>
        /// 全角空格为12288，半角空格为32
        /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        public static String ToSBC(String paramInput)
        {
            char[] dbcChar = paramInput.ToCharArray();
            for (int i = 0; i < dbcChar.Length; i++)
            {
                if (dbcChar[i] == 32)
                {
                    dbcChar[i] = (char)12288;
                    continue;
                }
                if (dbcChar[i] < 127)
                {
                    dbcChar[i] = (char)(dbcChar[i] + 65248);
                }
            }
            return new String(dbcChar);
        }

        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="paramInput">传入的任意字符串</param>
        /// <returns>半角字符串</returns>
        /// 全角空格为12288，半角空格为32
        /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        public static String ToDBC(String paramInput)
        {
            char[] sbcChar = paramInput.ToCharArray();
            for (int i = 0; i < sbcChar.Length; i++)
            {
                if (sbcChar[i] == 12288)
                {
                    sbcChar[i] = (char)32;
                    continue;
                }
                if (sbcChar[i] > 65280 && sbcChar[i] < 65375)
                {
                    sbcChar[i] = (char)(sbcChar[i] - 65248);
                }
            }
            return new String(sbcChar);
        }
        #endregion
    }
}
