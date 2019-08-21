using System;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.BLL.COM;
using System.Configuration;
using SkyCar.Common.Utility;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;

namespace SkyCar.Coeus.BLL.Common
{
    /// <summary>
    /// 维护配件明细BLL
    /// </summary>
    public class MaintainAutoPartsDetailBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.COM);
        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 维护配件明细BLL
        /// </summary>
        public MaintainAutoPartsDetailBLL() : base(Trans.COM)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 同步配件档案
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <param name="paramSyncWithPlatform">是否同步平台</param>
        /// <returns></returns>
        public bool? SyncAutoPartsArchive(MaintainAutoPartsDetailUIModel paramModel, bool paramSyncWithPlatform)
        {
            try
            {
                //检查配件档案是否已存在
                MDLBS_AutoPartsArchive curAutoPartsArchive = new MDLBS_AutoPartsArchive();
                CopyModel(paramModel, curAutoPartsArchive);
                MDLBS_AutoPartsArchive resultAutoPartsArchive = AutoPartsComFunction.GetAutoPartsArchive(curAutoPartsArchive);

                if (!string.IsNullOrEmpty(resultAutoPartsArchive.APA_ID))
                {
                    paramModel.APA_Barcode = resultAutoPartsArchive.APA_Barcode;

                    MDLBS_AutoPartsArchive updateAutoPartsArchive = new MDLBS_AutoPartsArchive();

                    updateAutoPartsArchive.WHERE_APA_ID = resultAutoPartsArchive.APA_ID;
                    updateAutoPartsArchive.WHERE_APA_VersionNo = resultAutoPartsArchive.APA_VersionNo;

                    updateAutoPartsArchive.APA_VehicleModelCode = paramModel.APA_VehicleModelCode;
                    updateAutoPartsArchive.APA_ExchangeCode = paramModel.APA_ExchangeCode;
                    updateAutoPartsArchive.APA_VersionNo = resultAutoPartsArchive.APA_VersionNo + 1;

                    var updateArchiveResult = Save<MDLBS_AutoPartsArchive>(updateAutoPartsArchive);
                    if (!updateArchiveResult)
                    {
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.BS_AutoPartsArchive });
                        return false;
                    }

                    return null;
                }
                paramModel.APA_Barcode = null;

                //不同步平台
                if (!paramSyncWithPlatform)
                {
                    return null;
                }

                #region 配件档案已存在，从平台获取配件档案

                string argsPostData = string.Format(ApiParameter.BF0017,
                        ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE],
                        SysConst.Merchant,
                        paramModel.APA_Name,
                        paramModel.APA_Brand,
                        paramModel.APA_Specification,
                        paramModel.APA_UOM,
                        paramModel.APA_Level,
                        paramModel.APA_VehicleBrand,
                        paramModel.APA_VehicleInspire,
                        paramModel.APA_VehicleCapacity,
                        paramModel.APA_VehicleYearModel,
                        paramModel.APA_VehicleGearboxTypeName,
                        paramModel.APA_OEMNo,
                        paramModel.APA_ThirdNo,
                        paramModel.APA_Barcode,
                        true);
                string strApiData = APIDataHelper.GetAPIData(ApiUrl.BF0017Url, argsPostData);
                var apiResult = (JObject)JsonConvert.DeserializeObject(strApiData);
                if (apiResult == null)
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0019, SystemActionEnum.Name.SAVE);
                    return false;
                }
                if (apiResult["autoPartsArchive"] == null)
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0020, new object[] { SystemTableEnums.Name.BS_AutoPartsArchive, SystemActionEnum.Name.SAVE });
                    return false;
                }
                var jsonResult = (JObject)JsonConvert.DeserializeObject(apiResult["autoPartsArchive"].ToString());

                #endregion

                #region 新增配件档案
                //待新增的配件档案
                MDLBS_AutoPartsArchive insertAutoPartsArchive = new MDLBS_AutoPartsArchive();

                if (apiResult[SysConst.EN_RESULTCODE] != null && apiResult[SysConst.EN_RESULTCODE].ToString().Equals(SysConst.EN_I0001))
                {
                    var barCode = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Barcode] == null ? null : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Barcode].ToString();
                    if (string.IsNullOrEmpty(barCode))
                    {
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0020, new object[] { MsgParam.BARCODE, SystemActionEnum.Name.SAVE });
                        return false;
                    }
                    insertAutoPartsArchive.APA_Name = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Name] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Name].ToString();
                    insertAutoPartsArchive.APA_OEMNo = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_OEMNo] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_OEMNo].ToString();
                    insertAutoPartsArchive.APA_ThirdNo = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_ThirdNo] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_ThirdNo].ToString();
                    insertAutoPartsArchive.APA_Brand = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Brand] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Brand].ToString();
                    insertAutoPartsArchive.APA_Specification = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Specification] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Specification].ToString();
                    insertAutoPartsArchive.APA_UOM = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_UOM] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_UOM].ToString();
                    insertAutoPartsArchive.APA_Level = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Level] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Level].ToString();
                    insertAutoPartsArchive.APA_VehicleBrand = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_VehicleBrand] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_VehicleBrand].ToString();
                    insertAutoPartsArchive.APA_VehicleInspire = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_VehicleInspire] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_VehicleInspire].ToString();
                    insertAutoPartsArchive.APA_VehicleCapacity = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_VehicleCapacity] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_VehicleCapacity].ToString();
                    insertAutoPartsArchive.APA_VehicleYearModel = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_VehicleYearModel] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_VehicleYearModel].ToString();
                    insertAutoPartsArchive.APA_IsValid = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_IsValid] == null || Convert.ToBoolean(jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_IsValid]);
                    insertAutoPartsArchive.APA_ID = null;
                    insertAutoPartsArchive.APA_Barcode = barCode;
                    insertAutoPartsArchive.APA_SUPP_ID = paramModel.APA_SUPP_ID;
                    insertAutoPartsArchive.APA_WH_ID = paramModel.APA_WH_ID;
                    insertAutoPartsArchive.APA_WHB_ID = paramModel.APA_WHB_ID;
                    insertAutoPartsArchive.APA_Org_ID = LoginInfoDAX.OrgID;
                    insertAutoPartsArchive.APA_CreatedBy = LoginInfoDAX.UserName;
                    insertAutoPartsArchive.APA_CreatedTime = BLLCom.GetCurStdDatetime();
                    insertAutoPartsArchive.APA_UpdatedBy = LoginInfoDAX.UserName;
                    insertAutoPartsArchive.APA_UpdatedTime = BLLCom.GetCurStdDatetime();

                    //变速类型
                    if (jsonResult["APA_VehicleGearboxType"] != null)
                    {
                        //转化变速类型
                        string vehicleGearboxTypeName = jsonResult["APA_VehicleGearboxType"].ToString();

                        string[] vehicleGearboxTypeNameList = vehicleGearboxTypeName.Split(';');

                        string resultVehicleGearboxTypeCode = string.Empty;
                        string resultVehicleGearboxTypeName = string.Empty;
                        foreach (var loopVehicleGearboxTypeName in vehicleGearboxTypeNameList)
                        {
                            switch (loopVehicleGearboxTypeName.Trim())
                            {
                                case GearboxTypeEnum.Name.AT:
                                    resultVehicleGearboxTypeCode += GearboxTypeEnum.Code.AT + SysConst.Semicolon_DBC;
                                    resultVehicleGearboxTypeName += GearboxTypeEnum.Name.AT + SysConst.Semicolon_DBC;
                                    break;
                                case GearboxTypeEnum.Name.CVT:
                                    resultVehicleGearboxTypeCode += GearboxTypeEnum.Code.CVT + SysConst.Semicolon_DBC;
                                    resultVehicleGearboxTypeName += GearboxTypeEnum.Name.CVT + SysConst.Semicolon_DBC;
                                    break;
                                case GearboxTypeEnum.Name.MT:
                                    resultVehicleGearboxTypeCode += GearboxTypeEnum.Code.MT + SysConst.Semicolon_DBC;
                                    resultVehicleGearboxTypeName += GearboxTypeEnum.Name.MT + SysConst.Semicolon_DBC;
                                    break;
                                case GearboxTypeEnum.Name.MTAT:
                                    resultVehicleGearboxTypeCode += GearboxTypeEnum.Code.MTAT + SysConst.Semicolon_DBC;
                                    resultVehicleGearboxTypeName += GearboxTypeEnum.Name.MTAT + SysConst.Semicolon_DBC;
                                    break;
                                case GearboxTypeEnum.Name.AMT:
                                    resultVehicleGearboxTypeCode += GearboxTypeEnum.Code.AMT + SysConst.Semicolon_DBC;
                                    resultVehicleGearboxTypeName += GearboxTypeEnum.Name.AMT + SysConst.Semicolon_DBC;
                                    break;
                                case GearboxTypeEnum.Name.DSGDCT:
                                    resultVehicleGearboxTypeCode += GearboxTypeEnum.Code.DSGDCT + SysConst.Semicolon_DBC;
                                    resultVehicleGearboxTypeName += GearboxTypeEnum.Name.DSGDCT + SysConst.Semicolon_DBC;
                                    break;
                                case GearboxTypeEnum.Name.EVAT:
                                    resultVehicleGearboxTypeCode += GearboxTypeEnum.Code.EVAT + SysConst.Semicolon_DBC;
                                    resultVehicleGearboxTypeName += GearboxTypeEnum.Name.EVAT + SysConst.Semicolon_DBC;
                                    break;
                            }
                        }
                        //去掉最后一个半角分号
                        if (resultVehicleGearboxTypeName.Trim().Length > 0)
                        {
                            insertAutoPartsArchive.APA_VehicleGearboxTypeName = resultVehicleGearboxTypeName.Trim().Substring(0, resultVehicleGearboxTypeName.Trim().Length - 1);
                        }
                        if (resultVehicleGearboxTypeCode.Trim().Length > 0)
                        {
                            insertAutoPartsArchive.APA_VehicleGearboxTypeCode = resultVehicleGearboxTypeCode.Trim().Substring(0, resultVehicleGearboxTypeCode.Trim().Length - 1);
                        }
                    }

                    var insertArchiveResult = Save<MDLBS_AutoPartsArchive>(insertAutoPartsArchive);
                    if (!insertArchiveResult)
                    {
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.BS_AutoPartsArchive });
                        return false;
                    }
                    paramModel.APA_Barcode = barCode;
                }

                #endregion
            }
            catch (Exception ex)
            {
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.BS_AutoPartsArchive, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }
            return true;
        }

        #endregion

        #region 私有方法

        #endregion
    }
}
