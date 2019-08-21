using System;
using System.Collections.Generic;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.PIS;

namespace SkyCar.Coeus.BLL.PIS
{
    /// <summary>
    /// 采购退货出库管理BLL
    /// </summary>
    public class PurchaseReturnManagerBLL : BLLBase
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
        /// 出库管理BLL
        /// </summary>
        public PurchaseReturnManagerBLL() : base(Trans.PIS)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramHead">单头UIModel</param>
        /// <param name="paramDetailList">明细UIModel列表</param>
        /// <returns></returns>
        public bool SaveDetailDS(PurchaseReturnManagerUIModel paramHead, SkyCarBindingList<StockOutBillManagerDetailUIModel, MDLPIS_StockOutBillDetail> paramDetailList)
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
            var argsHead = paramHead.ToTBModelForSaveAndDelete<MDLPIS_StockOutBill>();
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(paramHead.SOB_ID))
            {
                argsHead.SOB_ID = Guid.NewGuid().ToString();
                //单号
                argsHead.SOB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.SOB);
                argsHead.SOB_CreatedBy = LoginInfoDAX.UserName;
                argsHead.SOB_CreatedTime = BLLCom.GetCurStdDatetime();
            }
            argsHead.SOB_UpdatedBy = LoginInfoDAX.UserName;
            argsHead.SOB_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region 明细

            //添加的明细
            if (paramDetailList != null && paramDetailList.InsertList != null &&
                paramDetailList.InsertList.Count > 0)
            {
                foreach (var loopDetailItem in paramDetailList.InsertList)
                {
                    loopDetailItem.SOBD_SOB_ID = argsHead.SOB_ID ?? argsHead.WHERE_SOB_ID;
                    loopDetailItem.SOBD_SOB_No = argsHead.SOB_No;
                    loopDetailItem.SOBD_CreatedBy = LoginInfoDAX.UserName;
                    loopDetailItem.SOBD_CreatedTime = BLLCom.GetCurStdDatetime();
                    loopDetailItem.SOBD_UpdatedBy = LoginInfoDAX.UserName;
                    loopDetailItem.SOBD_UpdatedTime = BLLCom.GetCurStdDatetime();
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
                var saveStockOutBillResult = _bll.Save(argsHead, argsHead.SOB_ID);
                if (!saveStockOutBillResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_StockOutBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存明细

                //执行保存
                var saveStockOutDetailResult = _bll.UnitySave(paramDetailList);
                if (!saveStockOutDetailResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_StockOutBillDetail });
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
                        loopInsertDetail.SOBD_VersionNo = 1;
                    }
                }

                foreach (var loopUpdateDetail in paramDetailList.UpdateList)
                {
                    //更新时版本号加1
                    loopUpdateDetail.SOBD_VersionNo = loopUpdateDetail.SOBD_VersionNo + 1;
                }
            }

            return true;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="paramHead">UIModel</param>
        /// <param name="paramDetailList">出库单明细列表</param>
        /// <returns></returns>
        public bool ApproveDetailDS(PurchaseReturnManagerUIModel paramHead, SkyCarBindingList<StockOutBillManagerDetailUIModel, MDLPIS_StockOutBillDetail> paramDetailList)
        {
            var funcName = "ApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.SOB_ID)
                || string.IsNullOrEmpty(paramHead.SOB_No))
            {
                //没有获取到出库单，审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_StockOutBill, SystemActionEnum.Name.APPROVE });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            #endregion

            #region 准备数据

            #region 变量定义

            //待更新的出库单
            MDLPIS_StockOutBill updateStockOutBill = paramHead.ToTBModelForSaveAndDelete<MDLPIS_StockOutBill>();

            //待新增的[应付单]
            MDLFM_AccountPayableBill newAccountPayableBill = new MDLFM_AccountPayableBill();
            //待新增的[应付单明细]列表
            List<MDLFM_AccountPayableBillDetail> newAccountPayableBillDetailList = new List<MDLFM_AccountPayableBillDetail>();

            //待更新的[库存]列表
            List<MDLPIS_Inventory> updateInventoryList = new List<MDLPIS_Inventory>();
            //待新增的[库存异动日志]列表
            List<MDLPIS_InventoryTransLog> newInventoryTransLogList = new List<MDLPIS_InventoryTransLog>();

            #endregion

            //计算出库金额
            decimal totalStockOutAmount = CalculateStockOutAmount(updateStockOutBill, paramDetailList);

            #region 创建[应付单]的场合

            if (updateStockOutBill.SOB_SourceTypeName == StockOutBillSourceTypeEnum.Name.THCK)
            {
                //应付单
                newAccountPayableBill.APB_ID = Guid.NewGuid().ToString();
                newAccountPayableBill.APB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.APB);
                newAccountPayableBill.APB_BillDirectCode = BillDirectionEnum.Code.MINUS;
                newAccountPayableBill.APB_BillDirectName = BillDirectionEnum.Name.MINUS;
                newAccountPayableBill.APB_SourceTypeCode = AccountPayableBillSourceTypeEnum.Code.CKYF;
                newAccountPayableBill.APB_SourceTypeName = AccountPayableBillSourceTypeEnum.Name.CKYF;
                newAccountPayableBill.APB_SourceBillNo = updateStockOutBill.SOB_No;
                newAccountPayableBill.APB_Org_ID = LoginInfoDAX.OrgID;
                newAccountPayableBill.APB_Org_Name = LoginInfoDAX.OrgShortName;
                newAccountPayableBill.APB_ReceiveObjectTypeCode = AmountTransObjectTypeEnum.Code.AUTOPARTSSUPPLIER;
                newAccountPayableBill.APB_ReceiveObjectTypeName = AmountTransObjectTypeEnum.Name.AUTOPARTSSUPPLIER;
                newAccountPayableBill.APB_ReceiveObjectID = paramHead.SOB_SUPP_ID;
                newAccountPayableBill.APB_ReceiveObjectName = paramHead.SOB_SUPP_Name;
                newAccountPayableBill.APB_AccountPayableAmount = -totalStockOutAmount;
                newAccountPayableBill.APB_PaidAmount = 0;
                newAccountPayableBill.APB_UnpaidAmount = -totalStockOutAmount;
                newAccountPayableBill.APB_BusinessStatusCode = AccountPayableBillStatusEnum.Code.ZXZ;
                newAccountPayableBill.APB_BusinessStatusName = AccountPayableBillStatusEnum.Name.ZXZ;
                newAccountPayableBill.APB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
                newAccountPayableBill.APB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
                newAccountPayableBill.APB_IsValid = true;
                newAccountPayableBill.APB_CreatedBy = LoginInfoDAX.UserName;
                newAccountPayableBill.APB_CreatedTime = BLLCom.GetCurStdDatetime();
                newAccountPayableBill.APB_UpdatedBy = LoginInfoDAX.UserName;
                newAccountPayableBill.APB_UpdatedTime = BLLCom.GetCurStdDatetime();

            }
            #endregion

            #region 更新[出库单]

            //将出库单审核状态更新为[已审核]，单据状态更新为[已完成]
            updateStockOutBill.SOB_VersionNo++;
            updateStockOutBill.SOB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            updateStockOutBill.SOB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            updateStockOutBill.SOB_UpdatedBy = LoginInfoDAX.UserName;
            updateStockOutBill.SOB_UpdatedTime = BLLCom.GetCurStdDatetime();
            //updateStockOutBill.SOB_StatusCode = StockOutBillStatusEnum.Code.YWC;
            //updateStockOutBill.SOB_StatusName = StockOutBillStatusEnum.Name.YWC;
            #endregion

            #region 遍历[出库单明细]列表，创建[应付单明细]和[应收单明细]，创建或更新[库存]，创建[库存异动日志]

            foreach (var loopStockOutBillDetail in paramDetailList)
            {
                if (string.IsNullOrEmpty(loopStockOutBillDetail.SOBD_Barcode)
                    || string.IsNullOrEmpty(loopStockOutBillDetail.SOBD_BatchNo))
                {
                    continue;
                }

                if (updateStockOutBill.SOB_SourceTypeName == StockOutBillSourceTypeEnum.Name.THCK)
                {
                    //出库明细
                    loopStockOutBillDetail.WHERE_SOBD_ID = loopStockOutBillDetail.SOBD_ID;
                    loopStockOutBillDetail.WHERE_SOBD_VersionNo = loopStockOutBillDetail.SOBD_VersionNo;

                    #region 应付单明细

                    MDLFM_AccountPayableBillDetail newAccountPayableBillDetail = new MDLFM_AccountPayableBillDetail
                    {
                        APBD_ID = Guid.NewGuid().ToString(),
                        APBD_APB_ID = newAccountPayableBill.APB_ID,
                        APBD_IsMinusDetail = false,
                        APBD_SourceBillNo = updateStockOutBill.SOB_No,
                        APBD_SourceBillDetailID = loopStockOutBillDetail.SOBD_ID,
                        APBD_Org_ID = newAccountPayableBill.APB_Org_ID,
                        APBD_Org_Name = newAccountPayableBill.APB_Org_Name,
                        APBD_AccountPayableAmount = -loopStockOutBillDetail.SOBD_Amount,
                        APBD_PaidAmount = 0,
                        APBD_UnpaidAmount = -loopStockOutBillDetail.SOBD_Amount,
                        APBD_BusinessStatusCode = newAccountPayableBill.APB_BusinessStatusCode,
                        APBD_BusinessStatusName = newAccountPayableBill.APB_BusinessStatusName,
                        APBD_ApprovalStatusCode = newAccountPayableBill.APB_ApprovalStatusCode,
                        APBD_ApprovalStatusName = newAccountPayableBill.APB_ApprovalStatusName,
                        APBD_IsValid = true,
                        APBD_CreatedBy = LoginInfoDAX.UserName,
                        APBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                        APBD_UpdatedBy = LoginInfoDAX.UserName,
                        APBD_UpdatedTime = BLLCom.GetCurStdDatetime()
                    };
                    newAccountPayableBillDetailList.Add(newAccountPayableBillDetail);

                    #endregion

                    #region 库存和库存异动日志
                    //在[出库单明细]列表中第一次出现的配件[库存]信息
                    MDLPIS_Inventory inventoryExists = null;

                    foreach (var loopInventory in updateInventoryList)
                    {
                        if (loopInventory.INV_Barcode == loopStockOutBillDetail.SOBD_Barcode
                            && loopInventory.INV_BatchNo == loopStockOutBillDetail.SOBD_BatchNo)
                        {
                            inventoryExists = loopInventory;
                            break;
                        }
                    }
                    if (inventoryExists != null)
                    {
                        if (inventoryExists.INV_Qty < loopStockOutBillDetail.SOBD_Qty)
                        {
                            //配件：loopStockOutBillDetail.SOBD_Name（条形码：loopStockOutBillDetail.SOBD_Barcode）的库存不足，审核失败
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                            {
                                loopStockOutBillDetail.SOBD_Name, loopStockOutBillDetail.SOBD_Barcode,
                                MsgParam.SHORTAGE, SystemActionEnum.Name.APPROVE
                            });
                            LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                            return false;
                        }
                        //[出库单明细]列表中已遍历过该配件，累减数量
                        inventoryExists.INV_Qty -= loopStockOutBillDetail.SOBD_Qty;

                        //生成[库存异动日志]
                        newInventoryTransLogList.Add(GenerateInventoryTransLogOfApprove(updateStockOutBill, loopStockOutBillDetail, inventoryExists));
                    }
                    else
                    {
                        //[出库单明细]列表中第一次出现该配件
                        //查询该配件是否在[库存]中存在
                        MDLPIS_Inventory resultInventory = new MDLPIS_Inventory();
                        _bll.QueryForObject<MDLPIS_Inventory, MDLPIS_Inventory>(new MDLPIS_Inventory
                        {
                            WHERE_INV_Org_ID = updateStockOutBill.SOB_Org_ID,
                            WHERE_INV_Barcode = loopStockOutBillDetail.SOBD_Barcode,
                            WHERE_INV_BatchNo = loopStockOutBillDetail.SOBD_BatchNo,
                            WHERE_INV_WH_ID = loopStockOutBillDetail.SOBD_WH_ID,
                            WHERE_INV_IsValid = true
                        }, resultInventory);

                        //[库存]中不存在该配件
                        if (string.IsNullOrEmpty(resultInventory.INV_ID))
                        {
                            //配件：loopStockOutBillDetail.SOBD_Name（条形码：loopStockOutBillDetail.SOBD_Barcode）的库存不存在，审核失败
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                            {
                                loopStockOutBillDetail.SOBD_Name, loopStockOutBillDetail.SOBD_Barcode,
                                MsgParam.NOTEXIST, SystemActionEnum.Name.APPROVE
                            });
                            LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                            return false;
                        }

                        if (resultInventory.INV_Qty < loopStockOutBillDetail.SOBD_Qty)
                        {
                            //配件：loopStockOutBillDetail.SOBD_Name（条形码：loopStockOutBillDetail.SOBD_Barcode）的库存不足，审核失败
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[] { loopStockOutBillDetail.SOBD_Name, loopStockOutBillDetail.SOBD_Barcode, MsgParam.SHORTAGE, SystemActionEnum.Name.APPROVE });
                            LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                            return false;
                        }
                        //[库存]中存在该配件
                        //更新[库存]中该配件的数量
                        resultInventory.INV_Qty -= loopStockOutBillDetail.SOBD_Qty;
                        resultInventory.INV_UpdatedBy = LoginInfoDAX.UserName;
                        resultInventory.INV_UpdatedTime = BLLCom.GetCurStdDatetime();
                        resultInventory.WHERE_INV_ID = resultInventory.INV_ID;
                        resultInventory.WHERE_INV_VersionNo = resultInventory.INV_VersionNo;
                        updateInventoryList.Add(resultInventory);

                        //生成[库存异动日志]
                        newInventoryTransLogList.Add(GenerateInventoryTransLogOfApprove(updateStockOutBill, loopStockOutBillDetail, resultInventory));
                    }
                    #endregion
                }

            }
            #endregion

            #endregion

            #region 带事务的新增和保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 新增[应付单]

                if (!string.IsNullOrEmpty(newAccountPayableBill.APB_ID))
                {
                    bool insertAccountPayableBillResult = _bll.Insert(newAccountPayableBill);
                    if (!insertAccountPayableBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.FM_AccountPayableBill });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 新增[应付单明细]

                if (newAccountPayableBillDetailList.Count > 0)
                {
                    bool insertAccountPayableBillDetailResult = _bll.InsertByList<MDLFM_AccountPayableBillDetail, MDLFM_AccountPayableBillDetail>(newAccountPayableBillDetailList);
                    if (!insertAccountPayableBillDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.FM_AccountPayableBillDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 保存[出库单]

                bool updateStockOutBillResult = _bll.Save(updateStockOutBill);
                if (!updateStockOutBillResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.PIS_StockOutBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存[出库单明细]

                var updateDetailList = new List<MDLPIS_StockOutBillDetail>();
                //将当前DetailGridDS转换为指定类型的TBModelList
                paramDetailList.ToTBModelListForUpdateAndDelete<MDLPIS_StockOutBillDetail>(updateDetailList);
                foreach (var loopStockoutBillDetail in updateDetailList)
                {
                    bool saveStockOutBillDetailResult = _bll.Save(loopStockoutBillDetail);
                    if (!saveStockOutBillDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_StockOutBillDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 更新[库存]

                foreach (var loopInventory in updateInventoryList)
                {
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

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.APPROVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }
            #endregion

            //将最新数据回写给DetailDS
            CopyModel(updateStockOutBill, paramHead);

            //更新明细版本号
            foreach (var loopInsertDetail in paramDetailList.InsertList)
            {
                //新增时版本号为1
                loopInsertDetail.SOBD_VersionNo = 1;
            }
            foreach (var loopUpdateDetail in paramDetailList.UpdateList)
            {
                //更新时版本号加1
                loopUpdateDetail.SOBD_VersionNo = loopUpdateDetail.SOBD_VersionNo + 1;
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
        private bool ServerCheck(PurchaseReturnManagerUIModel paramModel, SkyCarBindingList<StockOutBillManagerDetailUIModel, MDLPIS_StockOutBillDetail> paramDetailList)
        {
            return true;
        }

        /// <summary>
        /// 计算入库总金额
        /// </summary>
        /// <param name="paramStockOutBill">入库单</param>
        /// <param name="paramStockOutBillDetailList">入库单明细列表</param>
        /// <returns></returns>
        private decimal CalculateStockOutAmount(MDLPIS_StockOutBill paramStockOutBill, SkyCarBindingList<StockOutBillManagerDetailUIModel, MDLPIS_StockOutBillDetail> paramStockOutBillDetailList)
        {
            if (paramStockOutBill == null || string.IsNullOrEmpty(paramStockOutBill.WHERE_SOB_ID))
            {
                return 0;
            }
            if (paramStockOutBillDetailList == null || paramStockOutBillDetailList.Count == 0)
            {
                return 0;
            }
            #region 计算入库总金额
            decimal totalStockOutAmount = 0;
            foreach (var loopStockOutBillDetail in paramStockOutBillDetailList)
            {
                if (string.IsNullOrEmpty(loopStockOutBillDetail.SOBD_Barcode)
                    || string.IsNullOrEmpty(loopStockOutBillDetail.SOBD_BatchNo))
                {
                    continue;
                }
                totalStockOutAmount += (loopStockOutBillDetail.SOBD_Amount ?? 0);
            }
            #endregion

            return totalStockOutAmount;
        }

        /// <summary>
        /// 【审核】生成库存异动日志
        /// </summary>
        /// <param name="paramStockOutBill">出库单</param>
        /// <param name="paramStockOutBillDetail">出库单明细</param>
        /// <param name="paramInventory">库存</param>
        /// <returns></returns>
        private MDLPIS_InventoryTransLog GenerateInventoryTransLogOfApprove(MDLPIS_StockOutBill paramStockOutBill, StockOutBillManagerDetailUIModel paramStockOutBillDetail, MDLPIS_Inventory paramInventory)
        {
            MDLPIS_InventoryTransLog newInventoryTransLog = new MDLPIS_InventoryTransLog
            {
                ITL_Org_ID = string.IsNullOrEmpty(paramStockOutBill.SOB_Org_ID) ? LoginInfoDAX.OrgID : paramStockOutBill.SOB_Org_ID,
                ITL_WH_ID = paramInventory.INV_WH_ID,
                ITL_WHB_ID = paramStockOutBillDetail.SOBD_WHB_ID,
                //业务单号为[出库单]的单号
                ITL_BusinessNo = paramStockOutBill.SOB_No,
                ITL_Barcode = paramInventory.INV_Barcode,
                ITL_BatchNo = paramInventory.INV_BatchNo,
                ITL_Name = paramStockOutBillDetail.SOBD_Name,
                ITL_Specification = paramStockOutBillDetail.SOBD_Specification,
                ITL_UnitCostPrice = paramInventory.INV_PurchaseUnitPrice,
                ITL_UnitSalePrice = paramStockOutBillDetail.SOBD_UnitSalePrice,
                //出库，数量为负
                ITL_Qty = -paramStockOutBillDetail.SOBD_Qty,
                ITL_AfterTransQty = paramInventory.INV_Qty,
                ITL_IsValid = true,
                ITL_CreatedBy = LoginInfoDAX.UserName,
                ITL_UpdatedBy = LoginInfoDAX.UserName
            };

            //异动类型
            switch (paramStockOutBill.SOB_SourceTypeName)
            {
                case StockOutBillSourceTypeEnum.Name.THCK:
                    //退货出库
                    newInventoryTransLog.ITL_TransType = InventoryTransTypeEnum.Name.THCK;
                    break;
                case StockOutBillSourceTypeEnum.Name.XSCK:
                    //销售出库
                    newInventoryTransLog.ITL_TransType = InventoryTransTypeEnum.Name.XSCK;
                    break;
            }

            newInventoryTransLog.ITL_Destination = paramStockOutBillDetail.SUPP_Name;
            newInventoryTransLog.ITL_Source = paramStockOutBillDetail.WH_Name;

            return newInventoryTransLog;
        }

        /// <summary>
        /// 【反审核】生成库存异动日志
        /// </summary>
        /// <param name="paramStockOutBill">出库单</param>
        /// <param name="paramStockOutBillDetail">出库单明细</param>
        /// <param name="paramInventory">库存</param>
        /// <returns></returns>
        private MDLPIS_InventoryTransLog GenerateInventoryTransLogOfUnApprove(MDLPIS_StockOutBill paramStockOutBill, StockOutBillManagerDetailUIModel paramStockOutBillDetail, MDLPIS_Inventory paramInventory)
        {
            MDLPIS_InventoryTransLog newInventoryTransLog = new MDLPIS_InventoryTransLog
            {
                ITL_Org_ID = string.IsNullOrEmpty(paramStockOutBill.SOB_Org_ID) ? LoginInfoDAX.OrgID : paramStockOutBill.SOB_Org_ID,
                ITL_WH_ID = paramInventory.INV_WH_ID,
                ITL_WHB_ID = paramStockOutBillDetail.SOBD_WHB_ID,
                //业务单号为[出库单]的单号
                ITL_BusinessNo = paramStockOutBill.SOB_No,
                ITL_Barcode = paramInventory.INV_Barcode,
                ITL_BatchNo = paramInventory.INV_BatchNo,
                ITL_Name = paramStockOutBillDetail.SOBD_Name,
                ITL_Specification = paramStockOutBillDetail.SOBD_Specification,
                ITL_UnitCostPrice = paramInventory.INV_PurchaseUnitPrice,
                ITL_UnitSalePrice = paramStockOutBillDetail.SOBD_UnitSalePrice,
                //出库 反审核，实际为入库，数量为正
                ITL_Qty = paramStockOutBillDetail.SOBD_Qty,
                ITL_AfterTransQty = paramInventory.INV_Qty,
                ITL_IsValid = true,
                ITL_CreatedBy = LoginInfoDAX.UserName,
                ITL_UpdatedBy = LoginInfoDAX.UserName
            };

            //异动类型
            switch (paramStockOutBill.SOB_SourceTypeName)
            {
                case StockOutBillSourceTypeEnum.Name.THCK:
                    //退货出库
                    newInventoryTransLog.ITL_TransType = InventoryTransTypeEnum.Name.THCK;
                    break;
                case StockOutBillSourceTypeEnum.Name.XSCK:
                    //销售出库
                    newInventoryTransLog.ITL_TransType = InventoryTransTypeEnum.Name.XSCK;
                    break;
            }
            return newInventoryTransLog;
        }

        #endregion
    }
}
