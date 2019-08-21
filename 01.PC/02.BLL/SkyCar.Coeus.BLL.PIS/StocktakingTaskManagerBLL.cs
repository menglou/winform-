using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.PIS;
using System.Windows.Forms;
using SkyCar.Coeus.Common.Const;

namespace SkyCar.Coeus.BLL.PIS
{
    /// <summary>
    /// 盘点管理BLL
    /// </summary>
    public class StocktakingTaskManagerBLL : BLLBase
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
        /// 盘点管理BLL
        /// </summary>
        public StocktakingTaskManagerBLL() : base(Trans.PIS)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramHead">单头UIModel</param>
        /// <param name="paramDetailList">明细UIModel列表</param>
        /// <param name="paramStatusName">需要更新为的盘点状态名称</param>
        /// <param name="paramStatusCode">需要更新为的盘点状态编码</param>
        /// <returns></returns>
        public bool SaveDetailDS(StocktakingTaskManagerUIModel paramHead, SkyCarBindingList<StocktakingTaskManagerDetailUIModel, MDLPIS_StocktakingTaskDetail> paramDetailList, string paramStatusName, string paramStatusCode)
        {
            var funcName = "SaveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);
            //服务端检查
            if (!ServerCheck(paramHead, paramDetailList))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            #region 准备数据

            #region 单头

            //将UIModel转为TBModel
            var argsHead = paramHead.ToTBModelForSaveAndDelete<MDLPIS_StocktakingTask>();
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(paramHead.ST_ID))
            {
                argsHead.ST_ID = Guid.NewGuid().ToString();
                //单号 
                argsHead.ST_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.ST);
                argsHead.ST_CreatedBy = LoginInfoDAX.UserName;
                argsHead.ST_CreatedTime = BLLCom.GetCurStdDatetime();
            }
            argsHead.ST_UpdatedBy = LoginInfoDAX.UserName;
            argsHead.ST_UpdatedTime = BLLCom.GetCurStdDatetime();

            if (!string.IsNullOrEmpty(paramStatusName) && !string.IsNullOrEmpty(paramStatusCode))
            {
                //更新[盘点状态]为传入的盘点状态参数
                argsHead.ST_StatusName = paramStatusName;
                argsHead.ST_StatusCode = paramStatusCode;

                if (paramStatusName == StocktakingBillStatusEnum.Name.YJQX)
                {
                    //取消盘点任务的场合，记录[结束时间]
                    argsHead.ST_EndTime = BLLCom.GetCurStdDatetime();
                }
            }
            #endregion

            #region 明细

            //添加的明细
            if (paramDetailList != null && paramDetailList.InsertList != null &&
                paramDetailList.InsertList.Count > 0)
            {
                foreach (var loopDetailItem in paramDetailList.InsertList)
                {
                    loopDetailItem.STD_TB_ID = argsHead.ST_ID ?? argsHead.WHERE_ST_ID;
                    loopDetailItem.STD_TB_No = argsHead.ST_No;
                    loopDetailItem.STD_CreatedBy = LoginInfoDAX.UserName;
                    loopDetailItem.STD_CreatedTime = BLLCom.GetCurStdDatetime();
                    loopDetailItem.STD_UpdatedBy = LoginInfoDAX.UserName;
                    loopDetailItem.STD_UpdatedTime = BLLCom.GetCurStdDatetime();
                }
            }
            #endregion

            #endregion

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存单头

                //执行保存
                bool saveStocktakingTaskResult = _bll.Save(argsHead, argsHead.ST_ID);
                if (!saveStocktakingTaskResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_StocktakingTask });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存明细

                //执行保存
                bool saveStocktakingTaskDetailResult = _bll.UnitySave(paramDetailList);
                if (!saveStocktakingTaskDetailResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_StocktakingTaskDetail });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ex.Message, "", null);
                return false;
            }

            #endregion

            //将最新数据回写给DetailDS
            CopyModel(argsHead, paramHead);

            //更新明细版本号
            if (paramDetailList != null)
            {
                if (paramDetailList.InsertList != null)
                {
                    foreach (var loopInsertDetail in paramDetailList.InsertList)
                    {
                        //新增时版本号为1
                        loopInsertDetail.STD_VersionNo = 1;
                    }
                }

                foreach (var loopUpdateDetail in paramDetailList.UpdateList)
                {
                    //更新时版本号加1
                    loopUpdateDetail.STD_VersionNo = loopUpdateDetail.STD_VersionNo + 1;
                }
            }

            return true;
        }
        
        /// <summary>
        /// 根据损益表校正库存
        /// </summary>
        /// <param name="paramHead">单头UIModel</param>
        /// <param name="paramDetailList">明细UIModel列表</param>
        /// <param name="paramUpdateInventoryList">待更新的库存列表</param>
        /// <returns></returns>
        public bool CorrectInventoryByProfitAndLoss(StocktakingTaskManagerUIModel paramHead, SkyCarBindingList<StocktakingTaskManagerDetailUIModel, MDLPIS_StocktakingTaskDetail> paramDetailList, List<MDLPIS_Inventory> paramUpdateInventoryList)
        {
            var funcName = "CorrectInventoryByProfitAndLoss";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 准备数据

            //待新增的[库存异动日志]列表
            List<MDLPIS_InventoryTransLog> newInventoryTransLogList = new List<MDLPIS_InventoryTransLog>();

            #region 更新[盘点任务]

            //将UIModel转为TBModel
            var argsHead = paramHead.ToTBModelForSaveAndDelete<MDLPIS_StocktakingTask>();

            //更新[盘点状态]为[校正完成]，记录[结束时间]
            argsHead.ST_StatusName = StocktakingBillStatusEnum.Name.JZWC;
            argsHead.ST_StatusCode = StocktakingBillStatusEnum.Code.JZWC;
            argsHead.ST_EndTime = BLLCom.GetCurStdDatetime();

            //根据[实际库存量]与[应有库存量]得出[盘点结果]
            if (argsHead.ST_ActualQty > argsHead.ST_DueQty)
            {
                //[实际库存量]>[应有库存量]:盘盈
                argsHead.ST_CheckResultName = StocktakingBillCheckResultEnum.Name.PY;
                argsHead.ST_CheckResultCode = StocktakingBillCheckResultEnum.Code.PY;
            }
            else if (argsHead.ST_ActualQty < argsHead.ST_DueQty)
            {
                //[实际库存量]<[应有库存量]:盘亏
                argsHead.ST_CheckResultName = StocktakingBillCheckResultEnum.Name.PK;
                argsHead.ST_CheckResultCode = StocktakingBillCheckResultEnum.Code.PK;
            }
            else if (argsHead.ST_ActualQty == argsHead.ST_DueQty)
            {
                //[实际库存量]=[应有库存量]:账实相符
                argsHead.ST_CheckResultName = StocktakingBillCheckResultEnum.Name.ZSXF;
                argsHead.ST_CheckResultCode = StocktakingBillCheckResultEnum.Code.ZSXF;
            }
            #endregion

            #region 遍历[盘点明细]列表，生成[库存异动日志]

            foreach (var loopStockTaskDetail in paramDetailList)
            {
                //[应有量]等于[实际量]的配件不进行校正
                if (loopStockTaskDetail.STD_DueQty == loopStockTaskDetail.STD_ActualQty)
                {
                    continue;
                }
                MDLPIS_InventoryTransLog newInventoryTransLog = new MDLPIS_InventoryTransLog();
                newInventoryTransLog = GenerateInventoryTransLog(argsHead, loopStockTaskDetail);
                newInventoryTransLogList.Add(newInventoryTransLog);
            }

            #endregion

            #endregion

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存库存

                foreach (var loopInventory in paramUpdateInventoryList)
                {
                    loopInventory.INV_UpdatedBy = LoginInfoDAX.UserName;
                    loopInventory.INV_UpdatedTime = BLLCom.GetCurStdDatetime();
                    bool saveInventoryResult = _bll.Save(loopInventory);
                    if (!saveInventoryResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_Inventory });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #endregion

                #region 新增[库存异动日志]

                if (newInventoryTransLogList.Count > 0)
                {
                    bool insertInventoryTransLogResult = _bll.InsertByList<MDLPIS_InventoryTransLog, MDLPIS_InventoryTransLog>(newInventoryTransLogList);
                    if (!insertInventoryTransLogResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.PIS_InventoryTransLog });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #endregion

                #region 保存[盘点任务]

                bool saveStocktakingTaskResult = _bll.Save(argsHead);
                if (!saveStocktakingTaskResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_StocktakingTask });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存[盘点任务明细]

                //bool saveStocktakingTaskDetailResult = _bll.UnitySave(paramDetailList);
                //if (!saveStocktakingTaskDetailResult)
                //{
                //    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                //    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_StocktakingTaskDetail });
                //    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                //    return false;
                //}

                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ex.Message, "", null);
                return false;
            }

            #endregion

            //将最新数据回写给DetailDS
            CopyModel(argsHead, paramHead);

            return true;
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(StocktakingTaskManagerUIModel paramModel, SkyCarBindingList<StocktakingTaskManagerDetailUIModel, MDLPIS_StocktakingTaskDetail> paramDetailList)
        {
            return true;
        }

        /// <summary>
        /// 生成库存异动日志
        /// </summary>
        /// <param name="paramHead">盘库任务</param>
        /// <param name="paramDetail">盘库任务明细</param>
        /// <returns></returns>
        private MDLPIS_InventoryTransLog GenerateInventoryTransLog(MDLPIS_StocktakingTask paramHead, StocktakingTaskManagerDetailUIModel paramDetail)
        {
            MDLPIS_InventoryTransLog newInventoryTransLog = new MDLPIS_InventoryTransLog
            {
                ITL_Org_ID = string.IsNullOrEmpty(paramHead.ST_Org_ID) ? LoginInfoDAX.OrgID : paramHead.ST_Org_ID,
                ITL_WH_ID = paramDetail.STD_WH_ID,
                ITL_WHB_ID = paramDetail.STD_WHB_ID,
                //业务单号为[盘点单]的单号
                ITL_BusinessNo = paramHead.ST_No,
                ITL_Barcode = paramDetail.STD_Barcode,
                ITL_BatchNo = paramDetail.STD_BatchNo,
                ITL_Name = paramDetail.STD_Name,
                ITL_Specification = paramDetail.STD_Specification,
                ITL_UnitCostPrice = paramDetail.INV_PurchaseUnitPrice,
                ITL_UnitSalePrice = null,
                ITL_Qty = (paramDetail.STD_ActualQty ?? 0) - (paramDetail.STD_DueQty ?? 0),
                ITL_TransType = InventoryTransTypeEnum.Name.PDTZ,
                ITL_Source = "其他位置",
                ITL_Destination = paramDetail.WH_Name,
                ITL_IsValid = true,
                ITL_CreatedBy = LoginInfoDAX.UserName,
                ITL_UpdatedBy = LoginInfoDAX.UserName
            };
            newInventoryTransLog.ITL_AfterTransQty = paramDetail.INV_Qty + newInventoryTransLog.ITL_Qty;

            return newInventoryTransLog;
        }

        #endregion
    }
}
