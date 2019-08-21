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
using SkyCar.Coeus.UIModel.BS.UIModel;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Common.Utility;

namespace SkyCar.Coeus.BLL.BS
{
    /// <summary>
    /// 车型配件匹配管理BLL
    /// </summary>
    public class VehicleModelMatchAutoPartsManagerBLL : BLLBase
    {
        #region 构造方法

        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.BS);

        /// <summary>
        /// 车型配件匹配管理BLL
        /// </summary>
        public VehicleModelMatchAutoPartsManagerBLL() : base(Trans.BS)
        {

        }
        #endregion

        #region  公共方法

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="paramHead">待保存的车辆信息</param>
        /// <param name="paramVehicleOemPartsList">待保存的原厂件信息列表</param>
        /// <param name="paramVehicleBrandPartsList">待保存的品牌件信息列表</param>
        /// <param name="paramIsJoinVinInfo">是否合并当前车架号的信息</param>
        /// <returns></returns>
        public bool SaveDetailDS(VehicleModelMatchAutoPartsManagerUIModel paramHead, SkyCarBindingList<VehicleOemPartsInfoUIModel, MDLBS_VehicleOemPartsInfo> paramVehicleOemPartsList, SkyCarBindingList<VehicleThirdPartsInfoUIModel, MDLBS_VehicleThirdPartsInfo> paramVehicleBrandPartsList, bool paramIsJoinVinInfo)
        {
            var funcName = "SaveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 准备数据

            //待同步的[车辆信息列表]
            List<VehicleModelMatchAutoPartsManagerUIModel> syncVehicleInfoList = new List<VehicleModelMatchAutoPartsManagerUIModel>();
            //待同步的[车辆原厂件信息列表]
            List<VehicleOemPartsInfoUIModel> syncVehicleOemPartsInfoList = new List<VehicleOemPartsInfoUIModel>();
            //待同步的[车辆品牌件信息列表]
            List<VehicleThirdPartsInfoUIModel> syncVehicleBrandPartsInfoList = new List<VehicleThirdPartsInfoUIModel>();

            #region 车辆信息

            //待保存的[车辆信息]
            MDLBS_VehicleInfo saveVehicleInfo = new MDLBS_VehicleInfo();

            if (paramIsJoinVinInfo)
            {
                #region 合并当前车架号对应信息的场合

                //查询当前车架号对应的车辆信息
                _bll.QueryForObject<MDLBS_VehicleInfo, MDLBS_VehicleInfo>(new MDLBS_VehicleInfo
                {
                    WHERE_VC_VIN = paramHead.VC_VIN,
                    WHERE_VC_IsValid = true,
                }, saveVehicleInfo);
                saveVehicleInfo.VC_UpdatedBy = LoginInfoDAX.UserName;
                saveVehicleInfo.VC_UpdatedTime = BLLCom.GetCurStdDatetime();
                saveVehicleInfo.WHERE_VC_ID = saveVehicleInfo.VC_ID;
                saveVehicleInfo.WHERE_VC_VersionNo = saveVehicleInfo.VC_VersionNo;

                //查询当前车架号对应的所有[原厂件信息]
                List<MDLBS_VehicleOemPartsInfo> resultVehicleOemPartsInfoList = new List<MDLBS_VehicleOemPartsInfo>();
                _bll.QueryForList<MDLBS_VehicleOemPartsInfo, MDLBS_VehicleOemPartsInfo>(new MDLBS_VehicleOemPartsInfo
                {
                    WHERE_VOPI_VC_VIN = saveVehicleInfo.VC_VIN,
                    WHERE_VOPI_IsValid = true,
                }, resultVehicleOemPartsInfoList);

                //查询当前车架号对应的所有[品牌件信息]
                List<MDLBS_VehicleThirdPartsInfo> resultVehicleBrandPartsInfoList = new List<MDLBS_VehicleThirdPartsInfo>();
                _bll.QueryForList<MDLBS_VehicleThirdPartsInfo, MDLBS_VehicleThirdPartsInfo>(new MDLBS_VehicleThirdPartsInfo
                {
                    WHERE_VTPI_VC_VIN = saveVehicleInfo.VC_VIN,
                    WHERE_VTPI_IsValid = true,
                }, resultVehicleBrandPartsInfoList);

                //移除重复的[原厂件信息]
                foreach (var loopOemParts in resultVehicleOemPartsInfoList)
                {
                    var curOemParts = paramVehicleOemPartsList.FirstOrDefault(
                        x => x.VOPI_VC_VIN == loopOemParts.VOPI_VC_VIN && x.VOPI_OEMNo == loopOemParts.VOPI_OEMNo);
                    if (curOemParts != null)
                    {
                        paramVehicleOemPartsList.Remove(curOemParts);
                    }
                }

                //移除重复的[品牌件信息]
                foreach (var loopBrandParts in resultVehicleBrandPartsInfoList)
                {
                    var curBrandParts = paramVehicleBrandPartsList.FirstOrDefault(x =>
                            x.VTPI_VC_VIN == loopBrandParts.VTPI_VC_VIN &&
                            x.VTPI_ThirdNo == loopBrandParts.VTPI_ThirdNo);
                    if (curBrandParts != null)
                    {
                        paramVehicleBrandPartsList.Remove(curBrandParts);
                    }
                }
                #endregion
            }
            else
            {
                #region 不合并当前车架号对应信息的场合

                //将UIModel转为TBModel
                saveVehicleInfo = paramHead.ToTBModelForSaveAndDelete<MDLBS_VehicleInfo>();
                //判断主键是否被赋值
                if (string.IsNullOrEmpty(paramHead.VC_ID))
                {
                    saveVehicleInfo.VC_ID = Guid.NewGuid().ToString();
                    saveVehicleInfo.VC_CreatedBy = LoginInfoDAX.UserName;
                    saveVehicleInfo.VC_CreatedTime = BLLCom.GetCurStdDatetime();
                }
                saveVehicleInfo.VC_UpdatedBy = LoginInfoDAX.UserName;
                saveVehicleInfo.VC_UpdatedTime = BLLCom.GetCurStdDatetime();
                #endregion
            }

            VehicleModelMatchAutoPartsManagerUIModel syncehicleInfo = new VehicleModelMatchAutoPartsManagerUIModel();
            _bll.CopyModel(saveVehicleInfo, syncehicleInfo);
            syncehicleInfo.VC_OperateType = "Save";
            syncVehicleInfoList.Add(syncehicleInfo);

            #endregion

            #region 原厂件信息

            //添加的明细
            foreach (var loopDetailItem in paramVehicleOemPartsList.InsertList)
            {
                loopDetailItem.VOPI_VC_VIN = saveVehicleInfo.VC_VIN;
                loopDetailItem.VOPI_CreatedBy = LoginInfoDAX.UserName;
                loopDetailItem.VOPI_CreatedTime = BLLCom.GetCurStdDatetime();
                loopDetailItem.VOPI_UpdatedBy = LoginInfoDAX.UserName;
                loopDetailItem.VOPI_UpdatedTime = BLLCom.GetCurStdDatetime();

                loopDetailItem.VOPI_OperateType = "Save";
            }

            //更新的明细
            foreach (var loopDetailItem in paramVehicleOemPartsList.UpdateList)
            {
                loopDetailItem.VOPI_VC_VIN = saveVehicleInfo.VC_VIN;
                loopDetailItem.VOPI_UpdatedBy = LoginInfoDAX.UserName;
                loopDetailItem.VOPI_UpdatedTime = BLLCom.GetCurStdDatetime();

                loopDetailItem.VOPI_OperateType = "Save";
            }

            //删除的明细
            foreach (var loopDetailItem in paramVehicleOemPartsList.DeleteList)
            {
                loopDetailItem.VOPI_OperateType = "Delete";
            }
            syncVehicleOemPartsInfoList.AddRange(paramVehicleOemPartsList.InsertList);
            syncVehicleOemPartsInfoList.AddRange(paramVehicleOemPartsList.UpdateList);
            syncVehicleOemPartsInfoList.AddRange(paramVehicleOemPartsList.DeleteList);
            #endregion

            #region 品牌件信息

            //添加的明细
            foreach (var loopDetailItem in paramVehicleBrandPartsList.InsertList)
            {
                loopDetailItem.VTPI_VC_VIN = saveVehicleInfo.VC_VIN;
                loopDetailItem.VTPI_CreatedBy = LoginInfoDAX.UserName;
                loopDetailItem.VTPI_CreatedTime = BLLCom.GetCurStdDatetime();
                loopDetailItem.VTPI_UpdatedBy = LoginInfoDAX.UserName;
                loopDetailItem.VTPI_UpdatedTime = BLLCom.GetCurStdDatetime();

                loopDetailItem.VTPI_OperateType = "Save";
            }

            //更新的明细
            foreach (var loopDetailItem in paramVehicleBrandPartsList.UpdateList)
            {
                loopDetailItem.VTPI_VC_VIN = saveVehicleInfo.VC_VIN;
                loopDetailItem.VTPI_UpdatedBy = LoginInfoDAX.UserName;
                loopDetailItem.VTPI_UpdatedTime = BLLCom.GetCurStdDatetime();

                loopDetailItem.VTPI_OperateType = "Save";
            }

            //删除的明细
            foreach (var loopDetailItem in paramVehicleBrandPartsList.DeleteList)
            {
                loopDetailItem.VTPI_OperateType = "Delete";
            }
            syncVehicleBrandPartsInfoList.AddRange(paramVehicleBrandPartsList.InsertList);
            syncVehicleBrandPartsInfoList.AddRange(paramVehicleBrandPartsList.UpdateList);
            syncVehicleBrandPartsInfoList.AddRange(paramVehicleBrandPartsList.DeleteList);
            #endregion

            #endregion

            #region 同步到平台

            if (!SynchronizeVehicleRelateInfo(syncVehicleInfoList, syncVehicleOemPartsInfoList, syncVehicleBrandPartsInfoList))
            {
                //同步到平台失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, "同步平台失败" });
                return false;
            }
            #endregion

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存[车辆信息]

                var saveVehicleInfoResult = _bll.Save(saveVehicleInfo, saveVehicleInfo.VC_ID);
                if (!saveVehicleInfoResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    //保存本地失败的场合，还原平台上保存的信息
                    RestoreSavePlatformInfo(saveVehicleInfo, paramVehicleOemPartsList, paramVehicleBrandPartsList);

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.BS_VehicleInfo });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存[车辆原厂件信息]

                var saveOemPartsResult = _bll.UnitySave(paramVehicleOemPartsList);
                if (!saveOemPartsResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    //保存本地失败的场合，还原平台上保存的信息
                    RestoreSavePlatformInfo(saveVehicleInfo, paramVehicleOemPartsList, paramVehicleBrandPartsList);

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.BS_VehicleOemPartsInfo });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存[车辆品牌件信息]

                var saveBrandPartsResult = _bll.UnitySave(paramVehicleBrandPartsList);
                if (!saveBrandPartsResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    //保存本地失败的场合，还原平台上保存的信息
                    RestoreSavePlatformInfo(saveVehicleInfo, paramVehicleOemPartsList, paramVehicleBrandPartsList);

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.BS_VehicleBrandPartsInfo });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);

                //保存本地失败的场合，还原平台上保存的信息
                RestoreSavePlatformInfo(saveVehicleInfo, paramVehicleOemPartsList, paramVehicleBrandPartsList);

                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ex.Message, "", null);
                return false;
            }

            #endregion

            //将最新数据回写给DetailDS
            CopyModel(saveVehicleInfo, paramHead);

            #region 更新[车辆原厂件信息]版本号
            foreach (var loopInsertDetail in paramVehicleOemPartsList.InsertList)
            {
                //新增时版本号为1
                loopInsertDetail.VOPI_VersionNo = 1;
            }
            foreach (var loopUpdateDetail in paramVehicleOemPartsList.UpdateList)
            {
                //更新时版本号加1
                loopUpdateDetail.VOPI_VersionNo = loopUpdateDetail.VOPI_VersionNo + 1;
            }
            #endregion

            #region 更新[车辆品牌件信息]版本号
            foreach (var loopInsertDetail in paramVehicleBrandPartsList.InsertList)
            {
                //新增时版本号为1
                loopInsertDetail.VTPI_VersionNo = 1;
            }
            foreach (var loopUpdateDetail in paramVehicleBrandPartsList.UpdateList)
            {
                //更新时版本号加1
                loopUpdateDetail.VTPI_VersionNo = loopUpdateDetail.VTPI_VersionNo + 1;
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="paramModelList">待删除的车辆信息列表</param>
        /// <param name="paramVehicleOemPartsList">待删除的原厂件信息列表</param>
        /// <param name="paramVehicleBrandPartsList">待删除的品牌件信息列表</param>
        /// <returns></returns>
        public bool DeleteDetailDS(List<VehicleModelMatchAutoPartsManagerUIModel> paramModelList, List<VehicleOemPartsInfoUIModel> paramVehicleOemPartsList, List<VehicleThirdPartsInfoUIModel> paramVehicleBrandPartsList)
        {
            if (paramModelList == null || paramModelList.Count == 0)
            {
                //请选择车辆信息进行删除
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0006, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.BS_VehicleInfo });
                return false;
            }

            #region 同步到平台

            if (!SynchronizeVehicleRelateInfo(paramModelList, paramVehicleOemPartsList, paramVehicleBrandPartsList))
            {
                //同步到平台失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, "同步平台失败" });
                return false;
            }
            #endregion

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 删除[车辆信息]
                var deleteVehicleInfoResult = _bll.DeleteByList<VehicleModelMatchAutoPartsManagerUIModel, MDLBS_VehicleInfo>(paramModelList);
                if (!deleteVehicleInfoResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    //删除本地失败的场合，还原平台上删除的信息
                    RestoreDeletePlatformInfo(paramModelList, paramVehicleOemPartsList, paramVehicleBrandPartsList);

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.BS_VehicleInfo });
                    return false;
                }
                #endregion

                #region 删除[车辆原厂件信息]

                if (paramVehicleOemPartsList.Count > 0)
                {
                    var deleteOemPartsInfoResult = _bll.DeleteByList<VehicleOemPartsInfoUIModel, MDLBS_VehicleOemPartsInfo>(paramVehicleOemPartsList);
                    if (!deleteOemPartsInfoResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);

                        //删除本地失败的场合，还原平台上删除的信息
                        RestoreDeletePlatformInfo(paramModelList, paramVehicleOemPartsList, paramVehicleBrandPartsList);

                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.BS_VehicleOemPartsInfo });
                        return false;
                    }
                }
                #endregion

                #region 删除[车辆品牌件信息]

                if (paramVehicleBrandPartsList.Count > 0)
                {
                    var deleteBrandPartsInfoResult = _bll.DeleteByList<VehicleThirdPartsInfoUIModel, MDLBS_VehicleThirdPartsInfo>(paramVehicleBrandPartsList);
                    if (!deleteBrandPartsInfoResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);

                        //删除本地失败的场合，还原平台上删除的信息
                        RestoreDeletePlatformInfo(paramModelList, paramVehicleOemPartsList, paramVehicleBrandPartsList);

                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.BS_VehicleBrandPartsInfo });
                        return false;
                    }
                }
                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);

                //删除本地失败的场合，还原平台上删除的信息
                RestoreDeletePlatformInfo(paramModelList, paramVehicleOemPartsList, paramVehicleBrandPartsList);

                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.DELETE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }
            return true;
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 同步车辆信息、车辆原厂件信息、车辆品牌件信息到平台
        /// </summary>
        /// <param name="paramVehicleInfoList">待同步的车辆信息列表</param>
        /// <param name="paramVehicleOemPartsInfoList">待同步的车辆原厂件信息</param>
        /// <param name="paramVehicleBrandPartsInfoList">待同步的车辆品牌件信息</param>
        /// <returns></returns>
        private bool SynchronizeVehicleRelateInfo(List<VehicleModelMatchAutoPartsManagerUIModel> paramVehicleInfoList, List<VehicleOemPartsInfoUIModel> paramVehicleOemPartsInfoList, List<VehicleThirdPartsInfoUIModel> paramVehicleBrandPartsInfoList)
        {
            var funcName = "SynchronizeShareInventory";
            LogHelper.WriteBussLogStart(Trans.BS, LoginInfoDAX.UserName, funcName, "", "", null);

            if (paramVehicleInfoList.Count > 0)
            {
                //车辆信息
                var jsonVehicleInfoList = (JValue)JsonConvert.SerializeObject(paramVehicleInfoList);
                //车辆信息
                var jsonVehicleOemPartsInfoList = (JValue)JsonConvert.SerializeObject(paramVehicleOemPartsInfoList);
                //车辆信息
                var jsonVehicleBrandPartsInfoList = (JValue)JsonConvert.SerializeObject(paramVehicleBrandPartsInfoList);
                string argsPostData = string.Format(ApiParameter.BF0043,
                    ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE],
                    jsonVehicleInfoList,
                    jsonVehicleOemPartsInfoList,
                    jsonVehicleBrandPartsInfoList);
                string strApiData = APIDataHelper.GetAPIData(ApiUrl.BF0043Url, argsPostData);
                //本地调试：
                //string strApiData = APIDataHelper.GetAPIData("http://localhost:61860//API/BF0043", argsPostData);
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

        /// <summary>
        /// 保存本地失败的场合，还原平台上保存的信息
        /// </summary>
        /// <param name="paramHead">待保存的车辆信息</param>
        /// <param name="paramVehicleOemPartsList">待保存的原厂件信息列表</param>
        /// <param name="paramVehicleBrandPartsList">待保存的品牌件信息列表</param>
        private void RestoreSavePlatformInfo(MDLBS_VehicleInfo paramHead, SkyCarBindingList<VehicleOemPartsInfoUIModel, MDLBS_VehicleOemPartsInfo> paramVehicleOemPartsList, SkyCarBindingList<VehicleThirdPartsInfoUIModel, MDLBS_VehicleThirdPartsInfo> paramVehicleBrandPartsList)
        {
            #region 1. 待还原平台数据的[车辆信息]

            var restoreSyncVehicleInfo = new VehicleModelMatchAutoPartsManagerUIModel();
            _bll.QueryForObject<MDLBS_VehicleInfo, VehicleModelMatchAutoPartsManagerUIModel>(new MDLBS_VehicleInfo
            {
                WHERE_VC_VIN = paramHead.VC_VIN,
                WHERE_VC_IsValid = true,
            }, restoreSyncVehicleInfo);
            if (string.IsNullOrEmpty(restoreSyncVehicleInfo.VC_ID))
            {
                _bll.CopyModel(paramHead, restoreSyncVehicleInfo);
                restoreSyncVehicleInfo.VC_OperateType = "Delete";
            }
            else
            {
                restoreSyncVehicleInfo.VC_OperateType = "Save";
            }
            List<VehicleModelMatchAutoPartsManagerUIModel> restoreVehicleInfoList = new List<VehicleModelMatchAutoPartsManagerUIModel>();
            restoreVehicleInfoList.Add(restoreSyncVehicleInfo);

            #endregion

            #region 2. 待还原平台数据的[车辆原厂件信息]列表

            List<VehicleOemPartsInfoUIModel> restoreVehicleOemPartsInfoList = new List<VehicleOemPartsInfoUIModel>();
            foreach (var loopInsert in paramVehicleOemPartsList.InsertList)
            {
                loopInsert.VOPI_OperateType = "Delete";
            }
            restoreVehicleOemPartsInfoList.AddRange(paramVehicleOemPartsList.InsertList);
            foreach (var loopDelete in paramVehicleOemPartsList.DeleteList)
            {
                loopDelete.VOPI_OperateType = "Save";
            }
            restoreVehicleOemPartsInfoList.AddRange(paramVehicleOemPartsList.DeleteList);
            //查询待更新数据的原保存内容
            List<MDLBS_VehicleOemPartsInfo> updateVehicleOemPartsInfoList = new List<MDLBS_VehicleOemPartsInfo>();
            string updateOemPartsIdStr = string.Empty;
            _bll.QueryForList(SQLID.BS_VehicleModelMatchAutoPartsManager_SQL05, new MDLBS_VehicleOemPartsInfo
            {
                WHERE_VOPI_ID = updateOemPartsIdStr,
            }, updateVehicleOemPartsInfoList);
            foreach (var loopUpdate in paramVehicleOemPartsList.UpdateList)
            {
                var curUpdateVehicleOemParts =
                    updateVehicleOemPartsInfoList.FirstOrDefault(x => x.VOPI_ID == loopUpdate.VOPI_ID);
                if (curUpdateVehicleOemParts != null && !string.IsNullOrEmpty(curUpdateVehicleOemParts.VOPI_ID))
                {
                    _bll.CopyModel(curUpdateVehicleOemParts, loopUpdate);
                }
                loopUpdate.VOPI_OperateType = "Save";
            }
            restoreVehicleOemPartsInfoList.AddRange(paramVehicleOemPartsList.UpdateList);

            #endregion

            #region 3. 待还原平台数据的[车辆品牌件信息]列表
            List<VehicleThirdPartsInfoUIModel> restoreVehicleBrandPartsInfoList = new List<VehicleThirdPartsInfoUIModel>();
            foreach (var loopInsert in paramVehicleBrandPartsList.InsertList)
            {
                loopInsert.VTPI_OperateType = "Delete";
            }
            restoreVehicleBrandPartsInfoList.AddRange(paramVehicleBrandPartsList.InsertList);
            foreach (var loopDelete in paramVehicleBrandPartsList.DeleteList)
            {
                loopDelete.VTPI_OperateType = "Save";
            }
            restoreVehicleBrandPartsInfoList.AddRange(paramVehicleBrandPartsList.DeleteList);
            //查询待更新数据的原保存内容
            List<MDLBS_VehicleThirdPartsInfo> updateVehicleBrandPartsInfoList = new List<MDLBS_VehicleThirdPartsInfo>();
            string updateBrandPartsIdStr = string.Empty;
            _bll.QueryForList(SQLID.BS_VehicleModelMatchAutoPartsManager_SQL06, new MDLBS_VehicleThirdPartsInfo
            {
                WHERE_VTPI_ID = updateBrandPartsIdStr,
            }, updateVehicleBrandPartsInfoList);
            foreach (var loopUpdate in paramVehicleBrandPartsList.UpdateList)
            {
                var curUpdateVehicleBrandParts =
                    updateVehicleBrandPartsInfoList.FirstOrDefault(x => x.VTPI_ID == loopUpdate.VTPI_ID);
                if (curUpdateVehicleBrandParts != null && !string.IsNullOrEmpty(curUpdateVehicleBrandParts.VTPI_ID))
                {
                    _bll.CopyModel(curUpdateVehicleBrandParts, loopUpdate);
                }
                loopUpdate.VTPI_OperateType = "Save";
            }
            restoreVehicleBrandPartsInfoList.AddRange(paramVehicleBrandPartsList.UpdateList);

            #endregion

            //同步到平台
            SynchronizeVehicleRelateInfo(restoreVehicleInfoList, restoreVehicleOemPartsInfoList, restoreVehicleBrandPartsInfoList);
        }

        /// <summary>
        /// 删除本地失败的场合，还原平台上删除的信息
        /// </summary>
        /// <param name="paramModelList"></param>
        /// <param name="paramVehicleOemPartsList"></param>
        /// <param name="paramVehicleBrandPartsList"></param>
        private void RestoreDeletePlatformInfo(List<VehicleModelMatchAutoPartsManagerUIModel> paramModelList, List<VehicleOemPartsInfoUIModel> paramVehicleOemPartsList, List<VehicleThirdPartsInfoUIModel> paramVehicleBrandPartsList)
        {
            #region 待还原平台数据的[车辆信息]

            List<VehicleModelMatchAutoPartsManagerUIModel> restoreVehicleInfoList = new List<VehicleModelMatchAutoPartsManagerUIModel>();
            foreach (var loopInsert in paramModelList)
            {
                loopInsert.VC_OperateType = "Save";
            }
            restoreVehicleInfoList.AddRange(paramModelList);

            #endregion

            #region 待还原平台数据的[车辆原厂件信息]

            List<VehicleOemPartsInfoUIModel> restoreVehicleOemPartsInfoList = new List<VehicleOemPartsInfoUIModel>();
            foreach (var loopInsert in paramVehicleOemPartsList)
            {
                loopInsert.VOPI_OperateType = "Save";
            }
            restoreVehicleOemPartsInfoList.AddRange(paramVehicleOemPartsList);

            #endregion

            #region 待还原平台数据的[车辆品牌件信息]

            List<VehicleThirdPartsInfoUIModel> restoreVehicleBrandPartsInfoList = new List<VehicleThirdPartsInfoUIModel>();
            foreach (var loopInsert in paramVehicleBrandPartsList)
            {
                loopInsert.VTPI_OperateType = "Save";
            }
            restoreVehicleBrandPartsInfoList.AddRange(paramVehicleBrandPartsList);

            #endregion

            //同步到平台
            SynchronizeVehicleRelateInfo(restoreVehicleInfoList, restoreVehicleOemPartsInfoList, restoreVehicleBrandPartsInfoList);
        }
        #endregion
    }
}
