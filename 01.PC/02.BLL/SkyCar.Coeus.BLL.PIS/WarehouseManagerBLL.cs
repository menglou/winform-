using System;
using System.Collections.Generic;
using System.Linq;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.PIS;
using SkyCar.Coeus.DAL;
using System.Reflection;
using SkyCar.Coeus.Common.ExtendClass;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.BLL.PIS
{
    /// <summary>
    /// 仓库管理BLL
    /// </summary>
    public class WarehouseManagerBLL : BLLBase
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
        /// 仓库管理BLL
        /// </summary>
        public WarehouseManagerBLL() : base(Trans.PIS)
        {

        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramHead">UIModel</param>
        /// <returns></returns>
        public bool SaveDetailDS(WarehouseManagerUIModel paramHead, SkyCarBindingList<WarehouseBinManagerUIModel, MDLPIS_WarehouseBin> paramWarehouseBinList)
        {
            //服务端检查
            if (!ServerCheck(paramHead))
            {
                return false;
            }

            #region 准备数据

            #region 仓库
            //将UIModel转为TBModel
            var argsWarehouse = paramHead.ToTBModelForSaveAndDelete<MDLPIS_Warehouse>();
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(paramHead.WH_ID))
            {
                argsWarehouse.WH_ID = Guid.NewGuid().ToString();
                argsWarehouse.WH_CreatedBy = LoginInfoDAX.UserName;
                argsWarehouse.WH_CreatedTime = BLLCom.GetCurStdDatetime();
            }
            argsWarehouse.WH_UpdatedBy = LoginInfoDAX.UserName;
            argsWarehouse.WH_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region 仓位

            //添加的仓位
            if (paramWarehouseBinList != null && paramWarehouseBinList.InsertList != null &&
                paramWarehouseBinList.InsertList.Count > 0)
            {
                foreach (var loopDetailItem in paramWarehouseBinList.InsertList)
                {
                    loopDetailItem.WHB_WH_ID = argsWarehouse.WH_ID ?? argsWarehouse.WHERE_WH_ID;
                    loopDetailItem.WHB_CreatedBy = LoginInfoDAX.UserName;
                    loopDetailItem.WHB_CreatedTime = BLLCom.GetCurStdDatetime();
                    loopDetailItem.WHB_UpdatedBy = LoginInfoDAX.UserName;
                    loopDetailItem.WHB_UpdatedTime = BLLCom.GetCurStdDatetime();
                }
            }
            #endregion
            
            #endregion

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存仓库

                var saveWarehouseResult = _bll.Save(argsWarehouse, argsWarehouse.WH_ID);
                if (!saveWarehouseResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_Warehouse });
                    return false;
                }
                #endregion

                #region 保存仓位

                var saveWarehouseBinResult = _bll.UnitySave(paramWarehouseBinList);
                if (!saveWarehouseBinResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_WarehouseBin });
                    return false;
                }
                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                //保存[仓库]信息发生异常
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }
            //将最新数据回写给DetailDS
            CopyModel(argsWarehouse, paramHead);
            #endregion

            #region 刷新缓存

            //刷新仓库缓存
            var resultWarehouseList = CacheDAX.Get(CacheDAX.ConfigDataKey.Warehouse) as List<MDLPIS_Warehouse>;
            List<MDLPIS_Warehouse> newWarehouseList = new List<MDLPIS_Warehouse>();
            if (resultWarehouseList != null)
            {
                newWarehouseList = resultWarehouseList;
                if (resultWarehouseList.All(x => x.WH_ID != argsWarehouse.WH_ID && x.WH_Name != argsWarehouse.WH_Name))
                {
                    newWarehouseList.Add(argsWarehouse);
                    CacheDAX.Add(CacheDAX.ConfigDataKey.Warehouse, newWarehouseList, true);
                }
            }
            else
            {
                newWarehouseList.Add(argsWarehouse);
                CacheDAX.Add(CacheDAX.ConfigDataKey.Warehouse, newWarehouseList, true);
            }

            //刷新仓位缓存
            if (paramWarehouseBinList == null)
            {
                return true;
            }
            var resultWarehouseBinList = CacheDAX.Get(CacheDAX.ConfigDataKey.WarehouseBin) as List<MDLPIS_WarehouseBin>;
            List<MDLPIS_WarehouseBin> newWarehouseBinList = new List<MDLPIS_WarehouseBin>();
            if (resultWarehouseBinList != null)
            {
                newWarehouseBinList = resultWarehouseBinList;
                if (paramWarehouseBinList.InsertList != null && paramWarehouseBinList.InsertList.Count > 0)
                {
                    foreach (var loopInsertBin in paramWarehouseBinList.InsertList)
                    {
                        if (resultWarehouseBinList.All(x => x.WHB_ID != loopInsertBin.WHB_ID && x.WHB_Name != loopInsertBin.WHB_Name))
                        {
                            var tempWarehouseBin = new MDLPIS_WarehouseBin();
                            _bll.CopyModel(loopInsertBin, tempWarehouseBin);
                            newWarehouseBinList.Add(tempWarehouseBin);
                            CacheDAX.Add(CacheDAX.ConfigDataKey.WarehouseBin, newWarehouseBinList, true);
                        }
                    }
                }
                if (paramWarehouseBinList.DeleteList != null && paramWarehouseBinList.DeleteList.Count > 0)
                {
                    //待移除的仓位
                    List<MDLPIS_WarehouseBin> removeWarehouseBinList = new List<MDLPIS_WarehouseBin>();
                    foreach (var loopWarehouseBin in newWarehouseBinList)
                    {
                        var deleteWarehouseBin = paramWarehouseBinList.DeleteList.FirstOrDefault(x => x.WHB_ID == loopWarehouseBin.WHB_ID);
                        if (deleteWarehouseBin != null)
                        {
                            removeWarehouseBinList.Add(loopWarehouseBin);
                        }
                    }
                    foreach (var loopWarehouseBin in removeWarehouseBinList)
                    {
                        newWarehouseBinList.Remove(loopWarehouseBin);
                    }
                }
            }
            else
            {
                var tempWarehouseBinList = new List<MDLPIS_WarehouseBin>();
                _bll.CopyModelList(paramWarehouseBinList.InsertList, tempWarehouseBinList);
                newWarehouseBinList.AddRange(tempWarehouseBinList);
                CacheDAX.Add(CacheDAX.ConfigDataKey.WarehouseBin, newWarehouseBinList, true);
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 删除仓库
        /// </summary>
        /// <param name="paramWarehouseList">待删除的仓库列表</param>
        /// <param name="paramWarehouseBinList">待删除的仓位列表</param>
        /// <returns></returns>
        public bool DeleteWarehouseAndBin(List<MDLPIS_Warehouse> paramWarehouseList, List<MDLPIS_WarehouseBin> paramWarehouseBinList)
        {
            if (paramWarehouseList == null || paramWarehouseList.Count == 0)
            {
                //请选择要删除的仓库
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0006, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.PIS_Warehouse });
                return false;
            }

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 删除仓位
                var deleteWarehouseBinResult = _bll.DeleteByList<MDLPIS_WarehouseBin, MDLPIS_WarehouseBin>(paramWarehouseBinList);
                if (!deleteWarehouseBinResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    //删除[仓位]信息失败
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.PIS_WarehouseBin });
                    return false;
                }
                #endregion

                #region 删除仓库
                var deleteWarehouseResult = _bll.DeleteByList<MDLPIS_Warehouse, MDLPIS_Warehouse>(paramWarehouseList);
                if (!deleteWarehouseResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    //删除[仓库]信息失败
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0000, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.PIS_Warehouse });
                    return false;
                }
                #endregion

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

            #region 刷新缓存

            //刷新仓库缓存
            var resultWarehouseList = CacheDAX.Get(CacheDAX.ConfigDataKey.Warehouse) as List<MDLPIS_Warehouse>;
            if (resultWarehouseList != null)
            {
                var newWarehouseList = resultWarehouseList;
                //待移除的仓库
                List<MDLPIS_Warehouse> removeWarehouseList = new List<MDLPIS_Warehouse>();
                foreach (var loopWarehouse in newWarehouseList)
                {
                    var deleteWarehouse = paramWarehouseList.FirstOrDefault(x => x.WH_ID == loopWarehouse.WH_ID);
                    if (deleteWarehouse != null)
                    {
                        removeWarehouseList.Add(loopWarehouse);
                    }
                }
                foreach (var loopWarehouse in removeWarehouseList)
                {
                    newWarehouseList.Remove(loopWarehouse);
                }
                CacheDAX.Remove(CacheDAX.ConfigDataKey.Warehouse);
                CacheDAX.Add(CacheDAX.ConfigDataKey.Warehouse, newWarehouseList, true);
            }

            //刷新仓位缓存
            var resultWarehouseBinList = CacheDAX.Get(CacheDAX.ConfigDataKey.WarehouseBin) as List<MDLPIS_WarehouseBin>;
            if (resultWarehouseBinList != null)
            {
                var newWarehouseBinList = resultWarehouseBinList;
                //待移除的仓位
                List<MDLPIS_WarehouseBin> removeWarehouseBinList = new List<MDLPIS_WarehouseBin>();
                foreach (var loopWarehouseBin in newWarehouseBinList)
                {
                    var deleteWarehouseBin = paramWarehouseBinList.FirstOrDefault(x => x.WHB_ID == loopWarehouseBin.WHB_ID);
                    if (deleteWarehouseBin != null)
                    {
                        removeWarehouseBinList.Add(loopWarehouseBin);
                    }
                }
                foreach (var loopWarehouseBin in removeWarehouseBinList)
                {
                    newWarehouseBinList.Remove(loopWarehouseBin);
                }
                CacheDAX.Remove(CacheDAX.ConfigDataKey.WarehouseBin);
                CacheDAX.Add(CacheDAX.ConfigDataKey.WarehouseBin, newWarehouseBinList, true);
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
        private bool ServerCheck(WarehouseManagerUIModel paramModel)
        {

            return true;
        }

        #endregion
    }
}
