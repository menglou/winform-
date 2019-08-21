using System;
using System.Collections.Generic;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.SM;
using SkyCar.Coeus.DAL;
using Newtonsoft.Json.Linq;
using SkyCar.Coeus.Common.Const;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using SkyCar.Coeus.Common.Message;
using SkyCar.Common.Utility;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.UIModel.SM.UIModel;

namespace SkyCar.Coeus.BLL.SM
{
    /// <summary>
    /// 已授权汽修商户授权查询BLL
    /// </summary>
    public class MerchantAuthorityQueryBLL : BLLBase
    {
        #region 私有变量
        /// <summary>
        /// 基本业务BLL
        /// </summary>
        private static BLLBase _bllBase = new BLLBase(Trans.COM);
        #endregion

        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.SM);

        /// <summary>
        /// 错误信息
        /// </summary>
        public static string ErrMsg = string.Empty;
        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 已授权汽修商户授权查询BLL
        /// </summary>
        public MerchantAuthorityQueryBLL() : base(Trans.SM)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        public bool SaveDetailDS(MerchantAuthorityQueryUIModel paramModel)
        {
            //服务端检查
            if (!ServerCheck(paramModel))
            {
                return false;
            }

            #region 事务，多数据表操作

            //将UIModel转为TBModel
            var argsAROrgSupMerchantAuthority = CopyModel<MDLSM_AROrgSupMerchantAuthority>(paramModel);

            #region 新增
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(argsAROrgSupMerchantAuthority.ASAH_ID))
            {
                //生成新ID
                argsAROrgSupMerchantAuthority.ASAH_ID = Guid.NewGuid().ToString();
                argsAROrgSupMerchantAuthority.ASAH_CreatedBy = LoginInfoDAX.UserName;
                argsAROrgSupMerchantAuthority.ASAH_CreatedTime = BLLCom.GetCurStdDatetime();
                argsAROrgSupMerchantAuthority.ASAH_UpdatedBy = LoginInfoDAX.UserName;
                argsAROrgSupMerchantAuthority.ASAH_UpdatedTime = BLLCom.GetCurStdDatetime();
                //主键未被赋值，则执行新增
                if (!_bll.Insert(argsAROrgSupMerchantAuthority))
                {
                    //新增[汽修商户授权]信息失败
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.ADD + SystemTableEnums.Name.SM_AROrgSupMerchantAuthority });
                    return false;
                }
            }
            #endregion

            #region 更新
            else
            {
                //主键被赋值，则需要更新，更新需要设定更新条件
                argsAROrgSupMerchantAuthority.WHERE_ASAH_ID = argsAROrgSupMerchantAuthority.ASAH_ID;
                argsAROrgSupMerchantAuthority.WHERE_ASAH_VersionNo = argsAROrgSupMerchantAuthority.ASAH_VersionNo;
                argsAROrgSupMerchantAuthority.ASAH_VersionNo++;

                argsAROrgSupMerchantAuthority.ASAH_UpdatedBy = LoginInfoDAX.UserName;
                argsAROrgSupMerchantAuthority.ASAH_UpdatedTime = BLLCom.GetCurStdDatetime();
                if (!_bll.Update(argsAROrgSupMerchantAuthority))
                {
                    //更新[汽修商户授权]信息失败
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.SM_AROrgSupMerchantAuthority });
                    return false;
                }
            }
            #endregion

            //将最新数据回写给DetailDS
            CopyModel(argsAROrgSupMerchantAuthority, paramModel);

            #endregion

            return true;
        }
        /// <summary>
        /// 从平台同步授权商户信息到本地数据库
        /// </summary>
        public static bool SynchronizeSupMerchantAuthorityInfo()
        {
            var funcName = "SynchronizeSupMerchantAuthorityInfo";
            LogHelper.WriteBussLogStart(Trans.SM, LoginInfoDAX.UserName, funcName, "", "", null);

            var argsMerchantPostData = string.Format(ApiParameter.BF0012,
                ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE],
                HttpUtility.UrlEncode(ConfigurationManager.AppSettings[AppSettingKey.ACTIVATION_CODE]),
                SysConst.ProductCode);
            var strMerchantApiData = APIDataHelper.GetAPIData(ApiUrl.BF0012Url, argsMerchantPostData);
            var jsonMerchantResult = (JObject)JsonConvert.DeserializeObject(strMerchantApiData);

            if (jsonMerchantResult != null && jsonMerchantResult[SysConst.EN_RESULTCODE].ToString().Equals(SysConst.EN_I0001))
            {
                //获取平台[汽配汽修商户授权]列表
                var platformAROrgAuthorityList = jsonMerchantResult["AROrgSupMerchantAuthorityList"];
                var platformMctAuthJson = (JArray)JsonConvert.DeserializeObject(platformAROrgAuthorityList.ToString());
                //转存到临时List,便于后面使用
                List<MDLSM_AROrgSupMerchantAuthority> tempPlatformAROrgAuthorityList = JsonConvert.DeserializeObject<List<MDLSM_AROrgSupMerchantAuthority>>(platformMctAuthJson.ToString());

                #region 传入到[汽配汽修商户授权]列表中
                AROrgSupMerchantAuthorityDataSet.AROrgSupMerchantAuthorityDataTable supMerchantAuthorityDataTable = new AROrgSupMerchantAuthorityDataSet.AROrgSupMerchantAuthorityDataTable();
                foreach (var loopAROrgAuthority in tempPlatformAROrgAuthorityList)
                {
                    if (string.IsNullOrEmpty(loopAROrgAuthority.ASAH_ARMerchant_Code))
                    {
                        continue;
                    }
                    AROrgSupMerchantAuthorityDataSet.AROrgSupMerchantAuthorityRow newAROrgAuthorityRow = supMerchantAuthorityDataTable.NewAROrgSupMerchantAuthorityRow();
                    newAROrgAuthorityRow.ASAH_ID = loopAROrgAuthority.ASAH_ID;
                    newAROrgAuthorityRow.ASAH_ARMerchant_Code = loopAROrgAuthority.ASAH_ARMerchant_Code;
                    newAROrgAuthorityRow.ASAH_ARMerchant_Name = loopAROrgAuthority.ASAH_ARMerchant_Name;
                    newAROrgAuthorityRow.ASAH_AROrg_Code = loopAROrgAuthority.ASAH_AROrg_Code;
                    newAROrgAuthorityRow.ASAH_AROrg_Name = loopAROrgAuthority.ASAH_AROrg_Name;
                    newAROrgAuthorityRow.ASAH_AROrg_Contacter = loopAROrgAuthority.ASAH_AROrg_Contacter;
                    newAROrgAuthorityRow.ASAH_AROrg_Phone = loopAROrgAuthority.ASAH_AROrg_Phone;
                    newAROrgAuthorityRow.ASAH_Remark = loopAROrgAuthority.ASAH_Remark;
                    if (loopAROrgAuthority.ASAH_IsValid != null)
                    {
                        newAROrgAuthorityRow.ASAH_IsValid = loopAROrgAuthority.ASAH_IsValid.Value;
                    }
                    newAROrgAuthorityRow.ASAH_CreatedBy = string.IsNullOrEmpty(loopAROrgAuthority.ASAH_CreatedBy)
                        ? LoginInfoDAX.UserName : loopAROrgAuthority.ASAH_CreatedBy;
                    newAROrgAuthorityRow.ASAH_CreatedTime = loopAROrgAuthority.ASAH_CreatedTime == null
                        ? BLLCom.GetCurStdDatetime()
                        : loopAROrgAuthority.ASAH_CreatedTime.Value;
                    newAROrgAuthorityRow.ASAH_UpdatedBy = LoginInfoDAX.UserName;
                    newAROrgAuthorityRow.ASAH_UpdatedTime = BLLCom.GetCurStdDatetime();
                    if (loopAROrgAuthority.ASAH_VersionNo != null)
                    {
                        newAROrgAuthorityRow.ASAH_VersionNo = loopAROrgAuthority.ASAH_VersionNo.Value;
                    }

                    supMerchantAuthorityDataTable.AddAROrgSupMerchantAuthorityRow(newAROrgAuthorityRow);
                }
                #endregion
                
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
                        cmdSupMerchant.CommandText = "P_SM_BatchSyncAROrgSupMerchantAuthorityInfo";
                        cmdSupMerchant.CommandType = CommandType.StoredProcedure;
                        cmdSupMerchant.Parameters.Add("@AROrgSupMerchantAuthorityList", SqlDbType.Structured);
                        cmdSupMerchant.Parameters[0].Value = supMerchantAuthorityDataTable;
                        
                        sqlCon.Open();
                        cmdSupMerchant.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteBussLogEndOK(Trans.SM, LoginInfoDAX.UserName, funcName,
                            MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SYNCHRONIZE + SystemTableEnums.Name.SM_AROrgSupMerchantAuthority }), "", null);

                    return false;
                }
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
        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(MerchantAuthorityQueryUIModel paramModel)
        {
            return true;
        }



        #endregion
    }
}
