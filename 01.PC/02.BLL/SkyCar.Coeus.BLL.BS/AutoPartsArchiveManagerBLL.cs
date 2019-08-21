using System;
using System.Collections.Generic;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.BS;
using SkyCar.Coeus.BLL.COM;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using SkyCar.Coeus.Common.Const;
using SkyCar.Common.Utility;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SkyCar.Coeus.DAL;
using System.Reflection;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.Common.APModel;

namespace SkyCar.Coeus.BLL.BS
{
    /// <summary>
    /// 配件档案BLL
    /// </summary>
    public class AutoPartsArchiveManagerBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.BS);
        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 配件档案BLL
        /// </summary>
        public AutoPartsArchiveManagerBLL() : base(Trans.BS)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <param name="paramAutoPartsPriceDetailList">配件价格明细列表</param>
        /// <param name="paramAutoPartsPictureList">配件图片列表</param>
        /// <returns></returns>
        public bool SaveDetailDS(AutoPartsArchiveManagerUIModel paramModel, SkyCarBindingList<AutoPartsPriceTypeUIModel, MDLBS_AutoPartsPriceType> paramAutoPartsPriceDetailList, List<AutoPartsPictureUIModel> paramAutoPartsPictureList)
        {
            var funcName = "SaveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);
            //服务端检查
            if (!ServerCheck(paramModel))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            #region 准备数据

            #region 配件档案

            //将UIModel转为TBModel
            var argsAutoPartsArchive = CopyModel<MDLBS_AutoPartsArchive>(paramModel);

            if (string.IsNullOrEmpty(argsAutoPartsArchive.APA_ID) && string.IsNullOrEmpty(argsAutoPartsArchive.APA_Barcode))
            {
                #region 新增
                string argsPostData = string.Format(ApiParameter.BF0017,
                        ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE],
                        SysConst.Merchant,
                        argsAutoPartsArchive.APA_Name,
                        argsAutoPartsArchive.APA_Brand,
                        argsAutoPartsArchive.APA_Specification,
                        argsAutoPartsArchive.APA_UOM,
                        argsAutoPartsArchive.APA_Level,
                        argsAutoPartsArchive.APA_VehicleBrand,
                        argsAutoPartsArchive.APA_VehicleInspire,
                        argsAutoPartsArchive.APA_VehicleCapacity,
                        argsAutoPartsArchive.APA_VehicleYearModel,
                        argsAutoPartsArchive.APA_VehicleGearboxTypeName,
                        argsAutoPartsArchive.APA_OEMNo,
                        argsAutoPartsArchive.APA_ThirdNo,
                        argsAutoPartsArchive.APA_Barcode,
                        argsAutoPartsArchive.APA_IsValid);
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

                if (apiResult[SysConst.EN_RESULTCODE] != null && apiResult[SysConst.EN_RESULTCODE].ToString().Equals(SysConst.EN_I0001))
                {
                    var barCode = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Barcode] == null ? null : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Barcode].ToString();
                    if (string.IsNullOrEmpty(barCode))
                    {
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0020, new object[] { MsgParam.BARCODE, SystemActionEnum.Name.SAVE });
                        return false;
                    }
                    argsAutoPartsArchive.APA_Name = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Name] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Name].ToString();
                    argsAutoPartsArchive.APA_OEMNo = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_OEMNo] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_OEMNo].ToString();
                    argsAutoPartsArchive.APA_ThirdNo = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_ThirdNo] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_ThirdNo].ToString();
                    argsAutoPartsArchive.APA_Brand = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Brand] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Brand].ToString();
                    argsAutoPartsArchive.APA_Specification = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Specification] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Specification].ToString();
                    argsAutoPartsArchive.APA_UOM = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_UOM] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_UOM].ToString();
                    argsAutoPartsArchive.APA_Level = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Level] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_Level].ToString();
                    argsAutoPartsArchive.APA_VehicleBrand = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_VehicleBrand] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_VehicleBrand].ToString();
                    argsAutoPartsArchive.APA_VehicleInspire = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_VehicleInspire] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_VehicleInspire].ToString();
                    argsAutoPartsArchive.APA_VehicleCapacity = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_VehicleCapacity] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_VehicleCapacity].ToString();
                    argsAutoPartsArchive.APA_VehicleYearModel = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_VehicleYearModel] == null
                        ? null
                        : jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_VehicleYearModel].ToString();
                    argsAutoPartsArchive.APA_IsValid = jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_IsValid] == null || Convert.ToBoolean(jsonResult[SystemTableColumnEnums.BS_AutoPartsArchive.Code.APA_IsValid]);
                    argsAutoPartsArchive.APA_ID = null;
                    argsAutoPartsArchive.APA_Barcode = barCode;
                    argsAutoPartsArchive.APA_CreatedBy = LoginInfoDAX.UserName;

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
                            argsAutoPartsArchive.APA_VehicleGearboxTypeName = resultVehicleGearboxTypeName.Trim().Substring(0, resultVehicleGearboxTypeName.Trim().Length - 1);
                        }
                        if (resultVehicleGearboxTypeCode.Trim().Length > 0)
                        {
                            argsAutoPartsArchive.APA_VehicleGearboxTypeCode = resultVehicleGearboxTypeCode.Trim().Substring(0, resultVehicleGearboxTypeCode.Trim().Length - 1);
                        }
                    }
                }
                else
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0020, new object[] { SystemTableEnums.Name.BS_AutoPartsArchive, SystemActionEnum.Name.SAVE });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                //生成新ID
                argsAutoPartsArchive.APA_ID = Guid.NewGuid().ToString();
                argsAutoPartsArchive.APA_UpdatedBy = LoginInfoDAX.UserName;
                argsAutoPartsArchive.APA_CreatedBy = LoginInfoDAX.UserName;
                argsAutoPartsArchive.APA_CreatedTime = BLLCom.GetCurStdDatetime();
                argsAutoPartsArchive.APA_UpdatedBy = LoginInfoDAX.UserName;
                argsAutoPartsArchive.APA_UpdatedTime = BLLCom.GetCurStdDatetime();
                argsAutoPartsArchive.APA_Org_ID = paramModel.APA_Org_ID;
                #endregion
            }
            else
            {
                #region 更新
                //主键被赋值，则需要更新，更新需要设定更新条件
                argsAutoPartsArchive.WHERE_APA_ID = argsAutoPartsArchive.APA_ID;
                argsAutoPartsArchive.WHERE_APA_VersionNo = argsAutoPartsArchive.APA_VersionNo;
                argsAutoPartsArchive.APA_VersionNo++;
                argsAutoPartsArchive.APA_UpdatedBy = LoginInfoDAX.UserName;
                argsAutoPartsArchive.APA_UpdatedTime = BLLCom.GetCurStdDatetime();
                #endregion
            }
            #endregion

            #region 配件价格类别

            //同步到平台的数据
            List<AutoPartsPriceTypeUIModel> syncAutoPartsPriceTypeList = new List<AutoPartsPriceTypeUIModel>();

            foreach (var loopNewAutoPartsPrice in paramAutoPartsPriceDetailList.InsertList)
            {
                loopNewAutoPartsPrice.APPT_Barcode = argsAutoPartsArchive.APA_Barcode;
                loopNewAutoPartsPrice.APPT_OperateType = "Save";

                syncAutoPartsPriceTypeList.Add(loopNewAutoPartsPrice);
            }

            //待更新的配件价格类别的ID组合字符串(用于本地保存失败时，还原平台上的数据)
            string updateIdStr = string.Empty;
            foreach (var loopUpdateAutoPartsPrice in paramAutoPartsPriceDetailList.UpdateList)
            {
                updateIdStr += loopUpdateAutoPartsPrice.APPT_ID + SysConst.Semicolon_DBC;
                loopUpdateAutoPartsPrice.APPT_OperateType = "Save";

                syncAutoPartsPriceTypeList.Add(loopUpdateAutoPartsPrice);
            }
            foreach (var loopDeleteAutoPartsPrice in paramAutoPartsPriceDetailList.DeleteList)
            {
                loopDeleteAutoPartsPrice.APPT_OperateType = "Delete";

                syncAutoPartsPriceTypeList.Add(loopDeleteAutoPartsPrice);
            }
            #endregion

            #region 配件图片

            //待保存的配件图片列表
            List<MDLPIS_InventoryPicture> savePictureList = new List<MDLPIS_InventoryPicture>();

            foreach (var loopPicture in paramAutoPartsPictureList)
            {
                if (string.IsNullOrEmpty(loopPicture.INVP_PictureName)
                    || string.IsNullOrEmpty(loopPicture.SourceFilePath))
                {
                    continue;
                }
                #region 将图片保存到本地以及上传文件服务器

                string fileNetUrl = string.Empty;
                bool savePictureResult = BLLCom.SaveFileByFileName(loopPicture.SourceFilePath, loopPicture.INVP_PictureName, ref fileNetUrl);
                if (!savePictureResult)
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + MsgParam.AUTOPARTS_PICTURE });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 保存配件图片数据

                //截取上传图片返回值中的文件名称
                int fileNameStartIndex = fileNetUrl.IndexOf("FileName=", StringComparison.Ordinal) + 1;
                int fileNameEndIndex = fileNameStartIndex + "FileName=".Length;
                int length = fileNetUrl.Length;
                //文件名称
                string tempFileName = fileNetUrl.Substring(fileNameEndIndex - 1, length - (fileNameEndIndex - 1));

                MDLPIS_InventoryPicture newAutoPartsPicture = new MDLPIS_InventoryPicture();

                _bll.CopyModel(loopPicture, newAutoPartsPicture);
                newAutoPartsPicture.INVP_Barcode = argsAutoPartsArchive.APA_Barcode;
                newAutoPartsPicture.INVP_PictureName = tempFileName;
                newAutoPartsPicture.INVP_IsValid = true;
                newAutoPartsPicture.INVP_CreatedBy = LoginInfoDAX.UserName;
                newAutoPartsPicture.INVP_CreatedTime = BLLCom.GetCurStdDatetime();
                newAutoPartsPicture.INVP_UpdatedBy = LoginInfoDAX.UserName;
                newAutoPartsPicture.INVP_UpdatedTime = BLLCom.GetCurStdDatetime();

                newAutoPartsPicture.WHERE_INVP_ID = newAutoPartsPicture.INVP_ID;
                newAutoPartsPicture.WHERE_INVP_VersionNo = newAutoPartsPicture.INVP_VersionNo;

                savePictureList.Add(newAutoPartsPicture);

                #endregion
            }

            #endregion

            #endregion

            #region 同步[配件价格类别明细]到平台

            if (!SynchronizeAutoPartsPriceType(syncAutoPartsPriceTypeList))
            {
                //同步到平台失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.BS_AutoPartsArchive, "同步平台失败" });
                return false;
            }
            #endregion

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存[配件档案]

                //判断主键是否被赋值
                if (string.IsNullOrEmpty(argsAutoPartsArchive.APA_ID) && string.IsNullOrEmpty(argsAutoPartsArchive.APA_Barcode))
                {
                    #region 新增
                    //主键未被赋值，则执行新增
                    bool insertAutoPartsArchiveResult = _bll.Insert(argsAutoPartsArchive);
                    if (!insertAutoPartsArchiveResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.BS_AutoPartsArchive });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                    #endregion
                }
                else
                {
                    #region 更新

                    bool updateAutoPartsArchiveResult = _bll.Save(argsAutoPartsArchive);
                    if (!updateAutoPartsArchiveResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.BS_AutoPartsArchive });
                        return false;
                    }
                    #endregion
                }
                #endregion

                #region 保存[配件价格类别]

                bool saveAutoPartsPriceResult = _bll.UnitySave(paramAutoPartsPriceDetailList);
                if (!saveAutoPartsPriceResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.BS_AutoPartsArchive });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 保存配件图片

                foreach (var loopPicture in savePictureList)
                {
                    //执行保存
                    bool saveInvPictureResult = _bll.Save(loopPicture);
                    if (!saveInvPictureResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);

                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopPicture.INVP_PictureName, ref outMsg);

                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + MsgParam.AUTOPARTS_PICTURE });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);

                foreach (var loopPicture in savePictureList)
                {
                    //保存失败，删除本地以及文件服务器上的图片
                    var outMsg = string.Empty;
                    BLLCom.DeleteFileByFileName(loopPicture.INVP_PictureName, ref outMsg);
                }

                #region 保存本地失败，还原同步到平台上已新增、已更新、已删除的配件价格类别信息

                List<AutoPartsPriceTypeUIModel> restoreSyncAutoPartsPriceTypeList = new List<AutoPartsPriceTypeUIModel>();
                foreach (var loopInsert in paramAutoPartsPriceDetailList.InsertList)
                {
                    loopInsert.APPT_OperateType = "Delete";
                }
                restoreSyncAutoPartsPriceTypeList.AddRange(paramAutoPartsPriceDetailList.InsertList);
                foreach (var loopDelete in paramAutoPartsPriceDetailList.DeleteList)
                {
                    loopDelete.APPT_OperateType = "Save";
                }
                restoreSyncAutoPartsPriceTypeList.AddRange(paramAutoPartsPriceDetailList.DeleteList);
                //查询待更新数据原保存数据内容
                List<MDLBS_AutoPartsPriceType> updateAutoPartsPriceTypeList = new List<MDLBS_AutoPartsPriceType>();
                _bll.QueryForList(SQLID.BS_AutoPartsArchiveManager_SQL05, new MDLBS_AutoPartsPriceType
                {
                    WHERE_APPT_ID = updateIdStr,
                }, updateAutoPartsPriceTypeList);
                foreach (var loopUpdate in paramAutoPartsPriceDetailList.UpdateList)
                {
                    var curUpdateAutoPartsPriceType =
                        updateAutoPartsPriceTypeList.FirstOrDefault(x => x.APPT_ID == loopUpdate.APPT_ID);
                    if (curUpdateAutoPartsPriceType != null && !string.IsNullOrEmpty(curUpdateAutoPartsPriceType.APPT_ID))
                    {
                        _bll.CopyModel(curUpdateAutoPartsPriceType, loopUpdate);
                    }
                    loopUpdate.APPT_OperateType = "Save";
                }
                restoreSyncAutoPartsPriceTypeList.AddRange(paramAutoPartsPriceDetailList.UpdateList);
                SynchronizeAutoPartsPriceType(restoreSyncAutoPartsPriceTypeList);

                #endregion

                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ex.Message, "", null);
                return false;
            }

            #endregion

            //将最新数据回写给DetailDS
            CopyModel(argsAutoPartsArchive, paramModel);

            //更新明细版本号
            if (paramAutoPartsPriceDetailList != null)
            {
                if (paramAutoPartsPriceDetailList.InsertList != null)
                {
                    foreach (var loopInsertDetail in paramAutoPartsPriceDetailList.InsertList)
                    {
                        //新增时版本号为1
                        loopInsertDetail.APPT_VersionNo = 1;
                    }
                }

                foreach (var loopUpdateDetail in paramAutoPartsPriceDetailList.UpdateList)
                {
                    //更新时版本号加1
                    loopUpdateDetail.APPT_VersionNo = loopUpdateDetail.APPT_VersionNo + 1;
                }
            }

            #region 更新配件图片版本号

            foreach (var loopPicture in paramAutoPartsPictureList)
            {
                if (string.IsNullOrEmpty(loopPicture.SourceFilePath))
                {
                    continue;
                }
                //保存成功，删除临时保存的图片
                if (File.Exists(loopPicture.SourceFilePath))
                {
                    File.Delete(loopPicture.SourceFilePath);
                }
                //本次保存的图片
                var thisSavePicture = savePictureList.FirstOrDefault(x => x.INVP_PictureName == loopPicture.INVP_PictureName);
                if (thisSavePicture != null)
                {
                    _bll.CopyModel(thisSavePicture, loopPicture);
                }
                //设置版本号
                if (loopPicture.INVP_VersionNo == null)
                {
                    loopPicture.INVP_VersionNo = 1;
                }
                else
                {
                    loopPicture.INVP_VersionNo += 1;
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 导入配件档案
        /// </summary>
        /// <param name="paramImportAutoPartsArchiveList">待导入的配件档案列表</param>
        /// <returns></returns>
        public bool ImportAutoPartsArchive(List<ImportAutoPartsArchiveUIModel> paramImportAutoPartsArchiveList)
        {
            var funcName = "ImportAutoPartsArchive";
            LogHelper.WriteBussLogStart(Trans.BS, LoginInfoDAX.UserName, funcName, "", "", null);

            if (paramImportAutoPartsArchiveList == null || paramImportAutoPartsArchiveList.Count == 0)
            {
                //待导入的数据为空
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "待导入的数据为空" });
                return true;
            }

            try
            {
                #region 通过平台接口导入并返回配件档案列表

                var jsonAutoPartsPriceTypeList = (JValue)JsonConvert.SerializeObject(paramImportAutoPartsArchiveList);
                string argsPostData = string.Format(ApiParameter.BF0046,
                    ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE],
                    jsonAutoPartsPriceTypeList);
                string strApiData = APIDataHelper.GetAPIData(ApiUrl.BF0046Url, argsPostData);
                //本地调试：
                //string strApiData = APIDataHelper.GetAPIData("http://localhost:61860//API/BF0046", argsPostData);
                var jsonResult = (JObject)JsonConvert.DeserializeObject(strApiData);
                if (jsonResult == null)
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.IMPORT + SystemTableEnums.Name.BS_AutoPartsArchive, "同步平台失败" });
                    return false;
                }
                if (jsonResult[SysConst.EN_RESULTCODE].ToString() != SysConst.EN_I0001)
                {
                    var strErrorMessage = jsonResult == null ? "" : jsonResult[SysConst.EN_RESULTCODE].ToString();
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.IMPORT + SystemTableEnums.Name.BS_AutoPartsArchive, strErrorMessage });
                    return false;
                }
                if (jsonResult["AutoPartsArchiveList"] == null)
                {
                    //没有获取到平台返回的配件档案，导入配件档案失败
                    ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { "平台返回的配件档案", SystemActionEnum.Name.IMPORT + SystemTableEnums.Name.BS_AutoPartsArchive });
                    return false;
                }
                var autoPartsArchiveJsonResult = jsonResult["AutoPartsArchiveList"];
                var jsonAutoPartsArchiveList = (JArray)JsonConvert.DeserializeObject(autoPartsArchiveJsonResult.ToString());
                //返回的配件档案列表
                var resultAutoPartsArchiveList = JsonConvert.DeserializeObject<List<ImportAutoPartsArchiveUIModel>>(jsonAutoPartsArchiveList.ToString());

                if (resultAutoPartsArchiveList == null || resultAutoPartsArchiveList.Count == 0)
                {
                    //没有获取到平台返回的配件档案，导入配件档案失败
                    ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { "平台返回的配件档案", SystemActionEnum.Name.IMPORT + SystemTableEnums.Name.BS_AutoPartsArchive });
                    return false;
                }
                #endregion

                #region 维护本地[配件档案]、[配件名称]、[车辆品牌车系大全]

                AutoPartsArchiveDataSet.AutoPartsArchiveDataTable importAutoPartsArchiveDataTable = new AutoPartsArchiveDataSet.AutoPartsArchiveDataTable();

                foreach (var loopAutoPartsArchive in resultAutoPartsArchiveList)
                {
                    AutoPartsArchiveDataSet.AutoPartsArchiveRow newAutoPartsPriceTypeRow = importAutoPartsArchiveDataTable.NewAutoPartsArchiveRow();
                    newAutoPartsPriceTypeRow.APA_Org_ID = LoginInfoDAX.OrgID;
                    newAutoPartsPriceTypeRow.APA_Barcode = loopAutoPartsArchive.APA_Barcode;
                    newAutoPartsPriceTypeRow.APA_OEMNo = loopAutoPartsArchive.APA_OEMNo;
                    newAutoPartsPriceTypeRow.APA_ThirdNo = loopAutoPartsArchive.APA_ThirdNo;
                    newAutoPartsPriceTypeRow.APA_Name = loopAutoPartsArchive.APA_Name;
                    newAutoPartsPriceTypeRow.APA_Brand = loopAutoPartsArchive.APA_Brand;
                    newAutoPartsPriceTypeRow.APA_Specification = loopAutoPartsArchive.APA_Specification;
                    newAutoPartsPriceTypeRow.APA_UOM = loopAutoPartsArchive.APA_UOM;
                    newAutoPartsPriceTypeRow.APA_Level = loopAutoPartsArchive.APA_Level;
                    newAutoPartsPriceTypeRow.APA_VehicleBrand = loopAutoPartsArchive.APA_VehicleBrand;
                    newAutoPartsPriceTypeRow.APA_VehicleInspire = loopAutoPartsArchive.APA_VehicleInspire;
                    newAutoPartsPriceTypeRow.APA_VehicleCapacity = loopAutoPartsArchive.APA_VehicleCapacity;
                    newAutoPartsPriceTypeRow.APA_VehicleYearModel = loopAutoPartsArchive.APA_VehicleYearModel;
                    newAutoPartsPriceTypeRow.APA_VehicleGearboxTypeName = loopAutoPartsArchive.APA_VehicleGearboxType;
                    newAutoPartsPriceTypeRow.APA_CreatedBy = LoginInfoDAX.UserName;
                    newAutoPartsPriceTypeRow.APA_UpdatedBy = LoginInfoDAX.UserName;

                    //转化变速类型
                    string vehicleGearboxTypeName = loopAutoPartsArchive.APA_VehicleGearboxType;

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
                        newAutoPartsPriceTypeRow.APA_VehicleGearboxTypeName = resultVehicleGearboxTypeName.Trim().Substring(0, resultVehicleGearboxTypeName.Trim().Length - 1);
                    }
                    if (resultVehicleGearboxTypeCode.Trim().Length > 0)
                    {
                        newAutoPartsPriceTypeRow.APA_VehicleGearboxTypeCode = resultVehicleGearboxTypeCode.Trim().Substring(0, resultVehicleGearboxTypeCode.Trim().Length - 1);
                    }

                    //配件名称拼音简写
                    newAutoPartsPriceTypeRow.APN_NameSpellCode = ChineseSpellCode.GetShortSpellCode(newAutoPartsPriceTypeRow.APA_Name);
                    importAutoPartsArchiveDataTable.AddAutoPartsArchiveRow(newAutoPartsPriceTypeRow);
                }

                SqlConnection sqlCon = new SqlConnection
                {
                    ConnectionString = DBManager.GetConnectionString(DBCONFIG.Coeus)
                };

                sqlCon.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlCon;
                cmd.CommandText = "P_BS_ImportAutoPartsArchiveInfo";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ImportAutoPartsArchiveList", SqlDbType.Structured);
                cmd.Parameters[0].Value = importAutoPartsArchiveDataTable;
                cmd.Parameters.Add("@ResultCode", SqlDbType.VarChar, 50);
                cmd.Parameters[1].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                sqlCon.Close();

                string resultCode = cmd.Parameters[1].Value?.ToString();
                if (resultCode == "导入失败")
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.IMPORT + SystemTableEnums.Name.BS_AutoPartsArchive });
                    return false;
                }
                #endregion
            }
            catch (Exception ex)
            {
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.IMPORT + SystemTableEnums.Name.BS_AutoPartsArchive, ex.Message });
                return false;
            }
            return true;
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(AutoPartsArchiveManagerUIModel paramModel)
        {
            //新增时检查是否存在相同的配件档案
            if (string.IsNullOrEmpty(paramModel.APA_ID))
            {
                MDLBS_AutoPartsArchive argsAutoPartsArchive = new MDLBS_AutoPartsArchive();
                CopyModel(paramModel, argsAutoPartsArchive);
                string autoPartsBarCode;
                if (AutoPartsComFunction.AutoPartsArchiveExist(argsAutoPartsArchive, out autoPartsBarCode))
                {
                    paramModel.APA_Barcode = autoPartsBarCode;
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0006, new object[] { SystemTableEnums.Name.BS_AutoPartsArchive });
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 同步配件价格类别明细到平台
        /// </summary>
        /// <returns></returns>
        private bool SynchronizeAutoPartsPriceType(List<AutoPartsPriceTypeUIModel> paramAutoPartsPriceTypeList)
        {
            var funcName = "SynchronizeAutoPartsPriceType";
            LogHelper.WriteBussLogStart(Trans.BS, LoginInfoDAX.UserName, funcName, "", "", null);

            if (paramAutoPartsPriceTypeList.Count > 0)
            {
                var jsonAutoPartsPriceTypeList = (JValue)JsonConvert.SerializeObject(paramAutoPartsPriceTypeList);
                string argsPostData = string.Format(ApiParameter.BF0044,
                        ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE],
                        LoginInfoDAX.OrgCode,
                        jsonAutoPartsPriceTypeList);
                string strApiData = APIDataHelper.GetAPIData(ApiUrl.BF0044Url, argsPostData);
                //本地调试：
                //string strApiData = APIDataHelper.GetAPIData("http://localhost:61860//API/BF0044", argsPostData);
                var jsonResult = (JObject)JsonConvert.DeserializeObject(strApiData);
                if (jsonResult == null || jsonResult[SysConst.EN_RESULTCODE].ToString() != SysConst.EN_I0001)
                {
                    var strErrorMessage = jsonResult == null ? "" : jsonResult[SysConst.EN_RESULTCODE].ToString();
                    LogHelper.WriteBussLogEndOK(Trans.BS, LoginInfoDAX.UserName, funcName, strErrorMessage, "", null);
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
