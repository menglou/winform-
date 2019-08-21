using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Common.Utility;

namespace SkyCar.Coeus.BLL
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
        private static WebUtils _webUtils =new WebUtils();

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
            _bllBase.QuerryForList<D>(paramTBModel, resultList);
            //将结果信息添加到缓存，已经存在则覆盖原来的信息
            CacheDAX.Add(paramTBModel.GetType().FullName, resultList, true);

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "", "", null);
            return resultList;
        }

        #region 初始化
        /// <summary>
        /// 初始化系统
        /// </summary>
        public static void InitializeSystem()
        {
            //初始化数据库连接
            DBManager.DBInit(DBCONFIG.Coeus);

            #region 初始化系统参数缓存
            var resultParameter = new List<MDLSM_Parameter>();
            var argsParameter = new MDLSM_Parameter {Para_IsValid = true};
            _bllBase.QuerryForList<MDLSM_Parameter>(argsParameter, resultParameter);
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
        }
        #endregion

        /// <summary>
        /// 初始化菜单到缓存
        /// </summary>
        public static void InitializeMenuDetailToCache()
        {
            var menuDetailList = new List<MDLSM_MenuDetail>();
            var argsMenuDetailQuery = new MDLSM_MenuDetail {WHERE_MenuD_IsValid = true};
            //查询菜单组
            _bllBase.QuerryForList<MDLSM_MenuDetail>(argsMenuDetailQuery, menuDetailList);

            foreach (var md in menuDetailList)
            {
                CacheDAX.Add(md.MenuD_ClassFullName, md, true);
            }
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
            _bllBase.QuerryForList<MDLSM_Message>(new MDLSM_Message(), msgList);
            //初始化系统消息
            MsgHelp.InitializeMsg(msgList);

            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, System.Reflection.MethodBase.GetCurrentMethod().ToString(), "", "", null);
        }


        public static List<MDLSM_Menu> GetSystemMenu()
        {
            List<MDLSM_Menu> resultList = new List<MDLSM_Menu>();

            return resultList;
        }

    
        
        /// <summary>
        /// 获取单据编号（通过接口）
        /// </summary>
        /// <param name="paramDocumentType">PO：采购订单，SO：销售订单，OPO：在线支付订单</param>
        /// <returns></returns>
        public static string GetDocumentNoByApi(string paramDocumentType)
        {
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, "GetDocumentNoByApi", "", "", null);
            //根据商户编码，商户激活码和产品编码查询授权信息
            var argsPostData = string.Format(ApiParameter.BF0018, ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE], "N", "1", paramDocumentType);
            var strApiData = _webUtils.DoPost(ApiUrl.BF0018Url, argsPostData);
            var jsonResult = (JObject)JsonConvert.DeserializeObject(strApiData);

            if (jsonResult != null && jsonResult["ResultCode"].ToString().Equals("I0001"))
            {
                var poCodeList = JsonConvert.DeserializeObject<List<string>>(jsonResult["DocumentNoList"].ToString());
                LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, "GetDocumentNoByApi",
                    string.Format("调用BF0018获取单据编号成功！消息：{0}", jsonResult["MerchantList"]), "", null);
                return poCodeList[0];
            }
            else
            {
                LogHelper.WriteBussLogEndNG(Trans.COM, LoginInfoDAX.UserName, "GetDocumentNoByApi",
                    jsonResult != null
                        ? string.Format("调用BF0018获取单据编号失败！错误消息：{0}", jsonResult["ResultMsg"])
                        : "调用BF0018获取单据编号失败！错误消息：无", "", null);
                return string.Empty;
            }
        }
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

            if (jsonResult != null && jsonResult["ResultCode"].ToString().Equals("I0001"))
            {
                var argsSettingKeyMctCode1 = jsonResult["ListOrganization"];
                var platformOrgJson = (JArray)JsonConvert.DeserializeObject(argsSettingKeyMctCode1.ToString());

                var localOrgList = new List<MDLSM_Organization>();
                _bllBase.QuerryForList(new MDLSM_Organization { WHERE_Org_IsValid = true }, localOrgList);
                var invalidOrgList = new List<MDLSM_Organization>();

                _bllBase.CopyModelList(localOrgList, invalidOrgList);
                //从无效列表中移除在平台上有效的组织
                foreach (var json in platformOrgJson)
                {
                    var validOrgList = invalidOrgList.Where(p => p.Org_Code == json["Org_Code"].ToString()).ToList();
                    if (validOrgList.Count > 0)
                    {
                        invalidOrgList.Remove(validOrgList[0]);
                    }
                }

                try
                {
                    DBManager.BeginTransaction(DBCONFIG.Coeus);
                    if (invalidOrgList.Count > 0)
                    {
                        foreach (var model in invalidOrgList)
                        {
                            model.WHERE_Org_ID = model.Org_ID;
                            model.Org_IsValid = false;
                            bool save = _bllBase.Save<MDLSM_Organization>(model);
                            if (!save)
                            {
                                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                                LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "同步组织失败！", "", null);
                                return false;
                            }
                        }
                    }
                    var orgList = new List<MDLSM_Organization>();
                    foreach (var json in platformOrgJson)
                    {
                        var argsorg = new MDLSM_Organization();
                        var sameOrgList = localOrgList.Where(p => p.Org_Code == json["Org_Code"].ToString()).ToList();
                        if (sameOrgList.Count > 0)
                        {
                            argsorg = sameOrgList[0];
                        }
                        argsorg.WHERE_Org_ID = argsorg.Org_ID;
                        argsorg.Org_Code = json["Org_Code"].ToString();
                        argsorg.Org_FullName = json["Org_FullName"].ToString();
                        argsorg.Org_ShortName = json["Org_ShortName"].ToString();
                        argsorg.Org_Contacter = json["Org_Contacter"].ToString();
                        argsorg.Org_TEL = json["Org_TEL"].ToString();
                        argsorg.Org_PhoneNo = json["Org_PhoneNo"].ToString();
                        argsorg.Org_Prov_Code = json["Org_Prov_Code"].ToString();
                        argsorg.Org_City_Code = json["Org_City_Code"].ToString();
                        argsorg.Org_Dist_Code = json["Org_Dist_Code"].ToString();
                        argsorg.Org_Addr = json["Org_Addr"].ToString();
                        argsorg.Org_Longitude = json["Org_Longitude"].ToString();
                        argsorg.Org_Latitude = json["Org_Latitude"].ToString();
                        argsorg.Org_MarkerTitle = json["Org_MarkerTitle"].ToString();
                        argsorg.Org_MarkerContent = json["Org_MarkerContent"].ToString();
                        argsorg.Org_Remark = json["Org_Remark"].ToString();
                        //Org_MCT_ID 作为MCT_Code来存放
                        argsorg.Org_MCT_ID = ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE] ?? string.Empty;
                        argsorg.Org_CreatedBy = argsorg.Org_UpdatedBy = "系统同步";
                        argsorg.Org_IsValid = true;
                        bool save = _bllBase.Save<MDLSM_Organization>(argsorg);
                        if (!save)
                        {
                            DBManager.RollBackTransaction(DBCONFIG.Coeus);
                            LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, "同步组织失败！", "", null);
                            return false;
                        }
                        orgList.Add(argsorg);
                    }
                    DBManager.CommitTransaction(DBCONFIG.Coeus);
                    LoginInfoDAX.OrgList = new List<MDLSM_Organization>(orgList);
                }
                catch (Exception ex)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    LogHelper.WriteErrorLog(Trans.COM, "ExecuteSyncCommand", ex.Message, null, ex);
                    LogHelper.WriteBussLogEndOK(Trans.COM, LoginInfoDAX.UserName, funcName, ex.Message, "", null);
                    return false;
                }
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
        /// 从平台同步授权商户信息到本地数据库
        /// </summary>
        public static bool SynchronizeSupMerchantAuthorityInfo()
        {
            var funcName = "SynchronizeSupMerchantAuthorityInfo";
            LogHelper.WriteBussLogStart(Trans.COM, LoginInfoDAX.UserName, funcName, "", "", null);

            var argsMerchantPostData = string.Format(ApiParameter.BF0012,
                ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE],
                HttpUtility.UrlEncode(ConfigurationManager.AppSettings[AppSettingKey.ACTIVATION_CODE]),
                SysConst.ProductCode);
            var strMerchantApiData = APIDataHelper.GetAPIData(ApiUrl.BF0012Url, argsMerchantPostData);
            var jsonMerchantResult = (JObject)JsonConvert.DeserializeObject(strMerchantApiData);

            if (jsonMerchantResult != null && jsonMerchantResult["ResultCode"].ToString().Equals("I0001"))
            {
                var argsSettingKeyMctCode1 = jsonMerchantResult["AROrgSupMerchantAuthorityList"];
                var platformMctAuthJson = (JArray)JsonConvert.DeserializeObject(argsSettingKeyMctCode1.ToString());

                var localAuthorityList = new List<MDLSM_AROrgSupMerchantAuthority>();
                _bllBase.QuerryForList(new MDLSM_AROrgSupMerchantAuthority(), localAuthorityList);
                var invalidAuthorityList = new List<MDLSM_AROrgSupMerchantAuthority>();

                #region
                _bllBase.CopyModelList(localAuthorityList, invalidAuthorityList);
                //从无效列表中移除在平台上有效的授权
                foreach (var json in platformMctAuthJson)
                {
                    var validOrgList =
                        invalidAuthorityList.Where(
                            p =>
                                p.ASAH_ARMerchant_Code == json["ASAH_ARMerchant_Code"].ToString() &&
                                p.ASAH_AROrg_Code == json["ASAH_AROrg_Code"].ToString()).ToList();
                    if (validOrgList.Count > 0)
                    {
                        invalidAuthorityList.Remove(validOrgList[0]);
                    }
                }
                try
                {
                    DBManager.BeginTransaction(DBCONFIG.Coeus);
                    if (invalidAuthorityList.Count > 0)
                    {
                        foreach (var model in invalidAuthorityList)
                        {
                            model.ASAH_IsValid = false;
                            model.WHERE_ASAH_ID = model.ASAH_ID;
                            bool save = _bllBase.Save<MDLSM_AROrgSupMerchantAuthority>(model);
                            if (!save)
                            {
                                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                                LogHelper.WriteBussLogEndOK(Trans.SM, LoginInfoDAX.UserName, funcName, "同步汽配汽修商户授权失败！", "", null);
                            }
                        }
                    }
                    // 保存授权信息
                    foreach (var json in platformMctAuthJson)
                    {
                        var argsAuthority = new MDLSM_AROrgSupMerchantAuthority();
                        var sameAuthorityList =
                            localAuthorityList.Where(
                                p =>
                                    p.ASAH_ARMerchant_Code == json["ASAH_ARMerchant_Code"].ToString() &&
                                    p.ASAH_AROrg_Code == json["ASAH_AROrg_Code"].ToString()).ToList();
                        if (sameAuthorityList.Count > 0)
                        {
                            argsAuthority = sameAuthorityList[0];
                        }
                        argsAuthority.WHERE_ASAH_ID = argsAuthority.ASAH_ID;
                        argsAuthority.ASAH_ID = json["ASAH_ID"].ToString();
                        argsAuthority.ASAH_ARMerchant_Code = json["ASAH_ARMerchant_Code"].ToString();
                        argsAuthority.ASAH_ARMerchant_Name = json["ASAH_ARMerchant_Name"].ToString();
                        argsAuthority.ASAH_AROrg_Code = json["ASAH_AROrg_Code"].ToString();
                        argsAuthority.ASAH_AROrg_Name = json["ASAH_AROrg_Name"].ToString();
                        argsAuthority.ASAH_Remark = json["ASAH_Remark"].ToString();
                        argsAuthority.ASAH_IsValid = true;
                        argsAuthority.ASAH_CreatedBy = json["ASAH_CreatedBy"].ToString();
                        argsAuthority.ASAH_UpdatedBy = json["ASAH_UpdatedBy"].ToString();

                        bool save = _bllBase.Save<MDLSM_AROrgSupMerchantAuthority>(argsAuthority);
                        if (!save)
                        {
                            DBManager.RollBackTransaction(DBCONFIG.Coeus);
                            LogHelper.WriteBussLogEndOK(Trans.SM, LoginInfoDAX.UserName, funcName, "同步汽配汽修商户授权失败！", "", null);
                        }
                    }
                    DBManager.CommitTransaction(DBCONFIG.Coeus);
                }
                catch (Exception ex)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    LogHelper.WriteErrorLog(Trans.SM, funcName, ex.Message, null, ex);
                    LogHelper.WriteBussLogEndNG(Trans.SM, LoginInfoDAX.UserName, funcName, ex.Message, "", null);
                    return false;
                }
                #endregion
            }
            else
            {
                ErrMsg = jsonMerchantResult == null ? "" : jsonMerchantResult["ResultMsg"].ToString();
                LogHelper.WriteBussLogEndOK(Trans.SM, LoginInfoDAX.UserName, funcName, ErrMsg, "", null);
                return false;
            }
            LogHelper.WriteBussLogEndOK(Trans.SM, LoginInfoDAX.UserName, funcName, "", "", null);
            return true;
        }

        ///// <summary>
        ///// 从VenusDB获取配件批次号
        ///// </summary>
        ///// <param name="paramDBConfig"></param>
        ///// <param name="paramInventoryModel">配件库存信息</param>
        ///// <returns>批次号</returns>
        //public static string GetBatchNoFromVenusDB(string paramDBConfig, MDLAPM_Inventory paramInventoryModel)
        //{
        //    if (string.IsNullOrEmpty(paramInventoryModel.INV_Org_ID)
        //        || string.IsNullOrEmpty(paramInventoryModel.INV_Barcode))
        //    {
        //        MessageBox.Show("入库组织和条码不能为空！");
        //        return string.Empty;
        //    }
        //    string dateString = SystemDAX.GetCurStdDatetime().ToString("yyMMdd");
        //    string batchNo;
        //    object result = DBManager.QueryForObject(paramDBConfig, SQLID.COMM_SQL08, paramInventoryModel);
        //    if (result != null)
        //    {
        //        var index = Convert.ToInt32(result);
        //        if (index >= 99)
        //        {
        //            MessageBox.Show("当天批次号已经达到最大数99！");
        //            return string.Empty;
        //        }
        //        batchNo = dateString + (index + 1).ToString("00");
        //    }
        //    else
        //    {
        //        batchNo = dateString + "00";
        //    }
        //    return batchNo;
        //}
    }
}
