using System;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.Common.Const;
using SkyCar.Common.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.UIModel.BS;

namespace SkyCar.Coeus.BLL.BS
{
    /// <summary>
    /// 品牌车系BLL
    /// </summary>
    public class VehicleBrandInspireSummaManagerBLL : BLLBase
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
        /// 品牌车系BLL
        /// </summary>
        public VehicleBrandInspireSummaManagerBLL() : base(Trans.SM)
        {

        }
        #endregion

        #region  公共方法

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="paramModel">待保存的车辆品牌车系</param>
        /// <returns></returns>
        public bool SaveDetailDS(VehicleBrandInspireSummaManagerUIModel paramModel)
        {
            //服务端检查
            if (!ServerCheck(paramModel))
            {
                return false;
            }

            //将UIModel转为TBModel
            var argsVehicleBrandInspireSumma = CopyModel<MDLBS_VehicleBrandInspireSumma>(paramModel);
            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                //判断主键是否被赋值
                if (string.IsNullOrEmpty(argsVehicleBrandInspireSumma.VBIS_ID))
                {
                    #region 新增

                    if (argsVehicleBrandInspireSumma.VBIS_IsValid == true)
                    {
                        #region 同步平台数据

                        //获取车辆品牌车系信息
                        string argsPostData = string.Format(ApiParameter.BF0020,
                            argsVehicleBrandInspireSumma.VBIS_Brand,
                            argsVehicleBrandInspireSumma.VBIS_Inspire,
                            ConfigurationManager.AppSettings[AppSettingKey.MCT_CODE]);

                        string strApiData = APIDataHelper.GetAPIData(ApiUrl.BF0020Url, argsPostData);
                        var jsonResult = (JObject)JsonConvert.DeserializeObject(strApiData);

                        if (jsonResult != null && jsonResult[SysConst.EN_RESULTCODE] != null && jsonResult[SysConst.EN_RESULTCODE].ToString().Equals(SysConst.EN_I0001))
                        {
                            paramModel.VBIS_Brand = jsonResult[SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Brand] == null ? "" : jsonResult[SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Brand].ToString();
                            paramModel.VBIS_Inspire = jsonResult[SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Inspire] == null ? "" : jsonResult[SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Inspire].ToString();
                            paramModel.VBIS_Model = jsonResult[SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Model] == null ? "" : jsonResult[SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_Model].ToString();
                            paramModel.VBIS_BrandSpellCode = jsonResult[SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_BrandSpellCode] == null ? "" : jsonResult[SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_BrandSpellCode].ToString();
                            paramModel.VBIS_InspireSpellCode = jsonResult[SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_InspireSpellCode] == null ? "" : jsonResult[SystemTableColumnEnums.BS_VehicleBrandInspireSumma.Code.VBIS_InspireSpellCode].ToString();
                            paramModel.VBIS_IsValid = true;
                            paramModel.VBIS_CreatedBy = LoginInfoDAX.UserName;
                            paramModel.VBIS_UpdatedBy = LoginInfoDAX.UserName;
                        }
                        else
                        {
                            var strErrorMessage = jsonResult == null ? "" : jsonResult["ResultMsg"].ToString();
                            LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                                strErrorMessage, "", null);
                        }

                        #endregion
                    }

                    //生成新ID
                    argsVehicleBrandInspireSumma.VBIS_ID = Guid.NewGuid().ToString();
                    argsVehicleBrandInspireSumma.VBIS_CreatedBy = LoginInfoDAX.UserName;
                    argsVehicleBrandInspireSumma.VBIS_CreatedTime = BLLCom.GetCurStdDatetime();
                    argsVehicleBrandInspireSumma.VBIS_UpdatedBy = LoginInfoDAX.UserName;
                    argsVehicleBrandInspireSumma.VBIS_UpdatedTime = BLLCom.GetCurStdDatetime();
                    //主键未被赋值，则执行新增
                    if (!_bll.Insert(argsVehicleBrandInspireSumma))
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.BS_VehicleBrandInspireSumma });
                        return false;
                    }
                    #endregion
                }
                else
                {
                    #region 更新
                    //主键被赋值，则需要更新，更新需要设定更新条件
                    argsVehicleBrandInspireSumma.WHERE_VBIS_ID = argsVehicleBrandInspireSumma.VBIS_ID;
                    argsVehicleBrandInspireSumma.WHERE_VBIS_VersionNo = argsVehicleBrandInspireSumma.VBIS_VersionNo;
                    argsVehicleBrandInspireSumma.VBIS_VersionNo++;
                    argsVehicleBrandInspireSumma.VBIS_UpdatedBy = LoginInfoDAX.UserName;
                    argsVehicleBrandInspireSumma.VBIS_UpdatedTime = BLLCom.GetCurStdDatetime();
                    if (!_bll.Update(argsVehicleBrandInspireSumma))
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.BS_VehicleBrandInspireSumma });
                        return false;
                    }
                    #endregion
                }

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteErrorLog(BussID, MethodBase.GetCurrentMethod().ToString(), ex.Message + SysConst.ENTER + ex.StackTrace, null, ex);
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }

            //将最新数据回写给DetailDS
            CopyModel(argsVehicleBrandInspireSumma, paramModel);

            //刷新车辆品牌缓存
            var resultVehicleBrandList = CacheDAX.Get(CacheDAX.ConfigDataKey.VehicleBrand) as List<MDLBS_VehicleBrandInspireSumma>;
            List<MDLBS_VehicleBrandInspireSumma> newVehicleBrandList = new List<MDLBS_VehicleBrandInspireSumma>();
            if (resultVehicleBrandList != null)
            {
                newVehicleBrandList = resultVehicleBrandList;
                if (resultVehicleBrandList.All(x => x.VBIS_Brand != argsVehicleBrandInspireSumma.VBIS_Brand))
                {
                    newVehicleBrandList.Insert(0, argsVehicleBrandInspireSumma);
                    newVehicleBrandList = newVehicleBrandList.GroupBy(p => new { p.VBIS_Brand, p.VBIS_BrandSpellCode })
                        .Select(g => g.First()).OrderBy(x => x.VBIS_Brand).ToList();
                    CacheDAX.Add(CacheDAX.ConfigDataKey.VehicleBrand, newVehicleBrandList, true);
                }
            }
            else
            {
                newVehicleBrandList.Add(argsVehicleBrandInspireSumma);
                CacheDAX.Add(CacheDAX.ConfigDataKey.VehicleBrand, newVehicleBrandList, true);
            }

            //刷新车辆车系缓存
            var resultVehicleInspireList = CacheDAX.Get(CacheDAX.ConfigDataKey.VehicleBrandInspire) as List<MDLBS_VehicleBrandInspireSumma>;
            List<MDLBS_VehicleBrandInspireSumma> newVehicleInspireList = new List<MDLBS_VehicleBrandInspireSumma>();
            if (resultVehicleInspireList != null)
            {
                newVehicleInspireList = resultVehicleInspireList;
                if (resultVehicleInspireList.All(x => x.VBIS_Brand != argsVehicleBrandInspireSumma.VBIS_Brand))
                {
                    newVehicleInspireList.Insert(0, argsVehicleBrandInspireSumma);
                    newVehicleInspireList = newVehicleInspireList.GroupBy(p => new { p.VBIS_Brand, p.VBIS_Inspire, p.VBIS_InspireSpellCode })
                        .Select(g => g.First()).OrderBy(x => x.VBIS_Brand).ThenBy(x => x.VBIS_Inspire).ToList();
                    CacheDAX.Add(CacheDAX.ConfigDataKey.VehicleBrandInspire, newVehicleInspireList, true);
                }
            }
            else
            {
                newVehicleInspireList.Add(argsVehicleBrandInspireSumma);
                CacheDAX.Add(CacheDAX.ConfigDataKey.VehicleBrandInspire, newVehicleInspireList, true);
            }

            return true;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="paramVehicleBrandInspireSummaList">待删除的车辆品牌车系列表</param>
        /// <returns></returns>
        public bool DeleteVehicleBrandInspire(List<MDLBS_VehicleBrandInspireSumma> paramVehicleBrandInspireSummaList)
        {
            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                var deleteVehicleBrandInspireResult = DeleteByList<MDLBS_VehicleBrandInspireSumma, MDLBS_VehicleBrandInspireSumma>(paramVehicleBrandInspireSummaList);
                if (!deleteVehicleBrandInspireResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE, SystemTableEnums.Name.BS_VehicleBrandInspireSumma });
                    return false;
                }
                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.DELETE, ex.Message });
                LogHelper.WriteErrorLog(BussID, MethodBase.GetCurrentMethod().ToString(), ex.Message + SysConst.ENTER + ex.StackTrace, null, ex);
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }

            //刷新车辆品牌缓存
            var resultVehicleBrandList = CacheDAX.Get(CacheDAX.ConfigDataKey.VehicleBrand) as List<MDLBS_VehicleBrandInspireSumma>;
            if (resultVehicleBrandList != null)
            {
                var newVehicleBrandList = resultVehicleBrandList;
                //待移除的车辆品牌
                List<MDLBS_VehicleBrandInspireSumma> removeVehicleBrandList = new List<MDLBS_VehicleBrandInspireSumma>();
                foreach (var loopVehicleBrand in newVehicleBrandList)
                {
                    var deleteVehicleBrand = paramVehicleBrandInspireSummaList.FirstOrDefault(x => x.VBIS_Brand == loopVehicleBrand.VBIS_Brand);
                    if (deleteVehicleBrand != null)
                    {
                        removeVehicleBrandList.Add(loopVehicleBrand);
                    }
                }
                foreach (var loopVehicleBrand in removeVehicleBrandList)
                {
                    newVehicleBrandList.Remove(loopVehicleBrand);
                }
                CacheDAX.Remove(CacheDAX.ConfigDataKey.VehicleBrand);
                CacheDAX.Add(CacheDAX.ConfigDataKey.VehicleBrand, newVehicleBrandList, true);
            }

            //刷新车辆车系缓存
            var resultVehicleInspireList = CacheDAX.Get(CacheDAX.ConfigDataKey.VehicleBrandInspire) as List<MDLBS_VehicleBrandInspireSumma>;
            if (resultVehicleInspireList != null)
            {
                var newVehicleInspireList = resultVehicleInspireList;
                //待移除的车辆品牌
                List<MDLBS_VehicleBrandInspireSumma> removeVehicleInspireList = new List<MDLBS_VehicleBrandInspireSumma>();
                foreach (var loopVehicleInspire in newVehicleInspireList)
                {
                    var deleteVehicleInspire = paramVehicleBrandInspireSummaList.FirstOrDefault(x => x.VBIS_Inspire == loopVehicleInspire.VBIS_Inspire);
                    if (deleteVehicleInspire != null)
                    {
                        removeVehicleInspireList.Add(loopVehicleInspire);
                    }
                }
                foreach (var loopVehicleInspire in removeVehicleInspireList)
                {
                    newVehicleInspireList.Remove(loopVehicleInspire);
                }
                CacheDAX.Remove(CacheDAX.ConfigDataKey.VehicleBrandInspire);
                CacheDAX.Add(CacheDAX.ConfigDataKey.VehicleBrandInspire, newVehicleInspireList, true);
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
        private bool ServerCheck(VehicleBrandInspireSummaManagerUIModel paramModel)
        {
            //验证车辆品牌和车系+车型描述是否已存在
            var resultSameVehicleBrandInspire = QueryForObject<Int32>(SQLID.BS_VehicleBrandInspireSummaManager_SQL01, new MDLBS_VehicleBrandInspireSumma()
            {
                WHERE_VBIS_ID = paramModel.VBIS_ID,
                WHERE_VBIS_Brand = paramModel.VBIS_Brand,
                WHERE_VBIS_Inspire = paramModel.VBIS_Inspire,
                WHERE_VBIS_ModelDesc = paramModel.VBIS_ModelDesc,
            });
            if (resultSameVehicleBrandInspire > 0)
            {
                //相同的品牌、车系、车型描述已存在，不能重复添加！
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0006, new object[] { MsgParam.SAME_BRAND_INSPIRE_MODELDESC });
                return false;
            }
            return true;
        }
        #endregion
    }
}
