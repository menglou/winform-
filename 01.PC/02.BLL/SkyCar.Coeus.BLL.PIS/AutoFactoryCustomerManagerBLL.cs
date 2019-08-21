using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.PIS;
using SkyCar.Common.Utility;

namespace SkyCar.Coeus.BLL.PIS
{
    /// <summary>
    /// 汽修商管理BLL
    /// </summary>
    public class AutoFactoryCustomerManagerBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.PIS);

        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 汽修商管理BLL
        /// </summary>
        public AutoFactoryCustomerManagerBLL() : base(Trans.PIS)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <param name="paramArOrgSupOrgAuthorityList">汽配汽修组织授权列表</param>
        /// <returns></returns>
        public bool SaveDetailDS(AutoFactoryCustomerManagerUIModel paramModel, SkyCarBindingList<AROrgSupOrgAuthorityUIModel, MDLSM_AROrgSupOrgAuthority> paramArOrgSupOrgAuthorityList)
        {
            //服务端检查
            if (!ServerCheck(paramModel))
            {
                return false;
            }

            #region 保存数据

            //将UIModel转为TBModel
            var argsAutoFactoryCustomer = CopyModel<MDLPIS_AutoFactoryCustomer>(paramModel);

            #region 同步到平台

            if (argsAutoFactoryCustomer.AFC_IsPlatform == true)
            {
                //同步到平台的数据
                List<ARCustomerAutoPartsPriceTypeUIModel> syncArCustomerAutoPartsPriceTypeList = new List<ARCustomerAutoPartsPriceTypeUIModel>();
                ARCustomerAutoPartsPriceTypeUIModel curArCustomerAutoPartsPriceType = new ARCustomerAutoPartsPriceTypeUIModel
                {
                    SAAPPT_ARMerchantCode = argsAutoFactoryCustomer.AFC_Code,
                    SAAPPT_AROrgCode = argsAutoFactoryCustomer.AFC_AROrg_Code,
                    SAAPPT_AutoPartsPriceType = argsAutoFactoryCustomer.AFC_AutoPartsPriceType,
                    SAAPPT_OperateType = "Save"
                };
                syncArCustomerAutoPartsPriceTypeList.Add(curArCustomerAutoPartsPriceType);
                if (!SynchronizeARCustomerAutoPartsPriceType(syncArCustomerAutoPartsPriceTypeList))
                {
                    //同步到平台失败
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_AutoFactoryCustomer, "同步平台失败" });
                    return false;
                }
            }

            #endregion

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                //判断主键是否被赋值
                if (string.IsNullOrEmpty(argsAutoFactoryCustomer.AFC_ID))
                {
                    #region 新增
                    //生成新ID
                    argsAutoFactoryCustomer.AFC_ID = Guid.NewGuid().ToString();
                    argsAutoFactoryCustomer.AFC_CreatedBy = LoginInfoDAX.UserName;
                    argsAutoFactoryCustomer.AFC_CreatedTime = BLLCom.GetCurStdDatetime();
                    argsAutoFactoryCustomer.AFC_UpdatedBy = LoginInfoDAX.UserName;
                    argsAutoFactoryCustomer.AFC_UpdatedTime = BLLCom.GetCurStdDatetime();
                    //主键未被赋值，则执行新增
                    bool insertCustomerResult = _bll.Insert(argsAutoFactoryCustomer);
                    if (!insertCustomerResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.PIS_AutoFactoryCustomer });
                        return false;
                    }
                    #endregion
                }
                else
                {
                    #region 更新
                    //主键被赋值，则需要更新，更新需要设定更新条件
                    argsAutoFactoryCustomer.WHERE_AFC_ID = argsAutoFactoryCustomer.AFC_ID;
                    argsAutoFactoryCustomer.WHERE_AFC_VersionNo = argsAutoFactoryCustomer.AFC_VersionNo;
                    argsAutoFactoryCustomer.AFC_VersionNo++;
                    argsAutoFactoryCustomer.AFC_UpdatedBy = LoginInfoDAX.UserName;
                    argsAutoFactoryCustomer.AFC_UpdatedTime = BLLCom.GetCurStdDatetime();
                    bool updateCustomerResult = _bll.Update(argsAutoFactoryCustomer);
                    if (!updateCustomerResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.PIS_AutoFactoryCustomer });
                        return false;
                    }
                    #endregion
                }

                #region 保存[汽配汽修组织授权]

                if (argsAutoFactoryCustomer.AFC_IsPlatform == true)
                {
                    bool saveArOrgSupOrgAuthorityResult = _bll.UnitySave(paramArOrgSupOrgAuthorityList);
                    if (!saveArOrgSupOrgAuthorityResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SM_AROrgSupOrgAuthority });
                        return false;
                    }
                }
                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);

                if (argsAutoFactoryCustomer.AFC_IsPlatform == true)
                {
                    #region 保存本地失败，还原同步到平台上已新增、已更新、已删除的汽修商客户配件价格类别信息

                    List<ARCustomerAutoPartsPriceTypeUIModel> restoreSyncAutoPartsPriceTypeList = new List<ARCustomerAutoPartsPriceTypeUIModel>();
                    //查询待更新数据原保存数据内容
                    List<ARCustomerAutoPartsPriceTypeUIModel> updateAutoPartsPriceTypeList = new List<ARCustomerAutoPartsPriceTypeUIModel>();
                    _bll.QueryForList(SQLID.PIS_AutoFactoryCustomerManager_SQL04, new MDLPIS_AutoFactoryCustomer
                    {
                        WHERE_AFC_ID = argsAutoFactoryCustomer.AFC_ID,
                    }, updateAutoPartsPriceTypeList);

                    restoreSyncAutoPartsPriceTypeList.AddRange(updateAutoPartsPriceTypeList);
                    SynchronizeARCustomerAutoPartsPriceType(restoreSyncAutoPartsPriceTypeList);
                    #endregion
                }

                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }

            //将最新数据回写给DetailDS
            CopyModel(argsAutoFactoryCustomer, paramModel);

            #endregion

            return true;
        }

        /// <summary>
        /// 删除汽修商
        /// </summary>
        /// <param name="paramAutoFactoryCustomerList">待删除汽修商列表</param>
        /// <returns></returns>
        public bool DeleteAutoFactoryCustomer(List<MDLPIS_AutoFactoryCustomer> paramAutoFactoryCustomerList)
        {
            if (paramAutoFactoryCustomerList == null || paramAutoFactoryCustomerList.Count == 0)
            {
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0006, new object[] { SystemTableEnums.Name.PIS_AutoFactoryCustomer, SystemActionEnum.Name.DELETE });
                return false;
            }

            #region 带事务的删除

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                var deleteAutoFactoryCustomerResult = _bll.DeleteByList<MDLPIS_AutoFactoryCustomer, MDLPIS_AutoFactoryCustomer>(paramAutoFactoryCustomerList);
                if (!deleteAutoFactoryCustomerResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE, SystemTableEnums.Name.PIS_AutoFactoryCustomer });
                    return false;
                }
                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.DELETE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }
            #endregion

            return true;
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(AutoFactoryCustomerManagerUIModel paramModel)
        {
            //验证汽修商编码唯一性
            var sameAutoFactoryCustomer = _bll.QueryForObject<Int32>(SQLID.PIS_AutoFactoryCustomerManager_SQL01, new MDLPIS_AutoFactoryCustomer
            {
                WHERE_AFC_ID = paramModel.AFC_ID,
                WHERE_AFC_Code = paramModel.AFC_Code,
                WHERE_AFC_AROrg_Code = paramModel.AFC_AROrg_Code,
            });
            if (sameAutoFactoryCustomer > 0)
            {
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0006, new object[] { SystemTableColumnEnums.PIS_AutoFactoryCustomer.Name.AFC_Code });
                return false;
            }

            return true;
        }

        /// <summary>
        /// 同步汽修商客户配件价格类别明细到平台
        /// </summary>
        /// <returns></returns>
        private bool SynchronizeARCustomerAutoPartsPriceType(List<ARCustomerAutoPartsPriceTypeUIModel> paramArCustomerAutoPartsPriceTypeList)
        {
            var funcName = "SynchronizeARCustomerAutoPartsPriceType";
            LogHelper.WriteBussLogStart(Trans.PIS, LoginInfoDAX.UserName, funcName, "", "", null);

            if (paramArCustomerAutoPartsPriceTypeList.Count > 0)
            {
                var jsonArCustomerAutoPartsPriceTypeList = (JValue)JsonConvert.SerializeObject(paramArCustomerAutoPartsPriceTypeList);
                string argsPostData = string.Format(ApiParameter.BF0045,
                        ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE],
                        LoginInfoDAX.OrgCode,
                        jsonArCustomerAutoPartsPriceTypeList);
                string strApiData = APIDataHelper.GetAPIData(ApiUrl.BF0045Url, argsPostData);
                //本地调试：
                //string strApiData = APIDataHelper.GetAPIData("http://localhost:61860//API/BF0045", argsPostData);
                var jsonResult = (JObject)JsonConvert.DeserializeObject(strApiData);
                if (jsonResult == null || jsonResult[SysConst.EN_RESULTCODE].ToString() != SysConst.EN_I0001)
                {
                    var strErrorMessage = jsonResult == null ? "" : jsonResult[SysConst.EN_RESULTCODE].ToString();
                    LogHelper.WriteBussLogEndOK(Trans.PIS, LoginInfoDAX.UserName, funcName, strErrorMessage, "", null);
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
