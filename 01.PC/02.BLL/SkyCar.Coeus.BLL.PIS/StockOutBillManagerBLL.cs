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
    /// 出库管理BLL
    /// </summary>
    public class StockOutBillManagerBLL : BLLBase
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
        public StockOutBillManagerBLL() : base(Trans.PIS)
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
        public bool SaveDetailDS(StockOutBillManagerUIModel paramHead, SkyCarBindingList<StockOutBillManagerDetailUIModel, MDLPIS_StockOutBillDetail> paramDetailList)
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
        public bool ApproveDetailDS(StockOutBillManagerUIModel paramHead, SkyCarBindingList<StockOutBillManagerDetailUIModel, MDLPIS_StockOutBillDetail> paramDetailList)
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

            //待新增的[应收单]
            MDLFM_AccountReceivableBill newAccountReceivableBill = new MDLFM_AccountReceivableBill();
            //待新增的[应收单明细]列表
            List<MDLFM_AccountReceivableBillDetail> newAccountReceivableBillDetailList = new List<MDLFM_AccountReceivableBillDetail>();

            //待更新的[库存]列表
            List<MDLPIS_Inventory> updateInventoryList = new List<MDLPIS_Inventory>();
            //待新增的[库存异动日志]列表
            List<MDLPIS_InventoryTransLog> newInventoryTransLogList = new List<MDLPIS_InventoryTransLog>();

            #endregion

            //计算出库金额
            decimal totalStockOutAmount = CalculateStockOutAmount(updateStockOutBill, paramDetailList);

            #region 创建[应收单]的场合

            if (updateStockOutBill.SOB_SourceTypeName == StockOutBillSourceTypeEnum.Name.SGCJ)
            {
                //应收单
                //TODO 
                newAccountReceivableBill.ARB_ID = Guid.NewGuid().ToString();
                newAccountReceivableBill.ARB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.APB);
                newAccountReceivableBill.ARB_BillDirectCode = BillDirectionEnum.Code.MINUS;
                newAccountReceivableBill.ARB_BillDirectName = BillDirectionEnum.Name.MINUS;
                //newAccountReceivableBill.ARB_SourceTypeCode = AccountReceivableBillSourceTypeEnum.Code.;
                //newAccountReceivableBill.ARB_SourceTypeName = AccountReceivableBillSourceTypeEnum.Name.CKYF;
                newAccountReceivableBill.ARB_SrcBillNo = updateStockOutBill.SOB_No;
                newAccountReceivableBill.ARB_Org_ID = LoginInfoDAX.OrgID;
                newAccountReceivableBill.ARB_Org_Name = LoginInfoDAX.OrgShortName;
                //newAccountReceivableBill.ARB_PayObjectTypeCode = AmountTransObjectTypeEnum.Code;
                //newAccountReceivableBill.ARB_PayObjectTypeName = AmountTransObjectTypeEnum.Name.AUTOPARTSSUPPLIER;
                newAccountReceivableBill.ARB_PayObjectID = paramHead.SOB_SUPP_ID;
                newAccountReceivableBill.ARB_PayObjectName = paramHead.SOB_SUPP_Name;
                newAccountReceivableBill.ARB_AccountReceivableAmount = totalStockOutAmount;
                newAccountReceivableBill.ARB_ReceivedAmount = 0;
                newAccountReceivableBill.ARB_UnReceiveAmount = totalStockOutAmount;
                newAccountReceivableBill.ARB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.ZXZ;
                newAccountReceivableBill.ARB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.ZXZ;
                newAccountReceivableBill.ARB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
                newAccountReceivableBill.ARB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
                newAccountReceivableBill.ARB_IsValid = true;
                newAccountReceivableBill.ARB_CreatedBy = LoginInfoDAX.UserName;
                newAccountReceivableBill.ARB_CreatedTime = BLLCom.GetCurStdDatetime();
                newAccountReceivableBill.ARB_UpdatedBy = LoginInfoDAX.UserName;
                newAccountReceivableBill.ARB_UpdatedTime = BLLCom.GetCurStdDatetime();

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

            #region 遍历[出库单明细]列表，创建[应收单明细]和[应收单明细]，创建或更新[库存]，创建[库存异动日志]

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

                    #region 应收单明细

                    MDLFM_AccountReceivableBillDetail newAccountReceivableBillDetail = new MDLFM_AccountReceivableBillDetail
                    {
                        ARBD_ID = Guid.NewGuid().ToString(),
                        ARBD_ARB_ID = newAccountReceivableBill.ARB_ID,
                        ARBD_IsMinusDetail = false,
                        ARBD_SrcBillNo = updateStockOutBill.SOB_No,
                        ARBD_SrcBillDetailID = loopStockOutBillDetail.SOBD_ID,
                        ARBD_Org_ID = newAccountReceivableBill.ARB_Org_ID,
                        ARBD_Org_Name = newAccountReceivableBill.ARB_Org_Name,
                        ARBD_AccountReceivableAmount = loopStockOutBillDetail.SOBD_Amount,
                        ARBD_ReceivedAmount = 0,
                        ARBD_UnReceiveAmount = loopStockOutBillDetail.SOBD_Amount,
                        ARBD_BusinessStatusCode = newAccountReceivableBill.ARB_BusinessStatusCode,
                        ARBD_BusinessStatusName = newAccountReceivableBill.ARB_BusinessStatusName,
                        ARBD_ApprovalStatusCode = newAccountReceivableBill.ARB_ApprovalStatusCode,
                        ARBD_ApprovalStatusName = newAccountReceivableBill.ARB_ApprovalStatusName,
                        ARBD_IsValid = true,
                        ARBD_CreatedBy = LoginInfoDAX.UserName,
                        ARBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                        ARBD_UpdatedBy = LoginInfoDAX.UserName,
                        ARBD_UpdatedTime = BLLCom.GetCurStdDatetime()
                    };
                    newAccountReceivableBillDetailList.Add(newAccountReceivableBillDetail);

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

                #region 新增[应收单]

                if (!string.IsNullOrEmpty(newAccountReceivableBill.ARB_ID))
                {
                    bool insertAccountReceivableBillResult = _bll.Insert(newAccountReceivableBill);
                    if (!insertAccountReceivableBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.FM_AccountReceivableBill });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 新增[应收单明细]

                if (newAccountReceivableBillDetailList.Count > 0)
                {
                    bool insertAccountReceivableBillDetailResult = _bll.InsertByList<MDLFM_AccountReceivableBillDetail, MDLFM_AccountReceivableBillDetail>(newAccountReceivableBillDetailList);
                    if (!insertAccountReceivableBillDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.FM_AccountReceivableBillDetail });
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

        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="paramHead">UIModel</param>
        /// <param name="paramDetailList">出库单明细列表</param>
        /// <returns></returns>
        public bool UnApproveDetailDS(StockOutBillManagerUIModel paramHead, SkyCarBindingList<StockOutBillManagerDetailUIModel, MDLPIS_StockOutBillDetail> paramDetailList)
        {
            var funcName = "UnApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.SOB_ID)
                || string.IsNullOrEmpty(paramHead.SOB_No))
            {
                //没有获取到出库单，反审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_StockOutBill, SystemActionEnum.Name.UNAPPROVE });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }
            #endregion

            #region 准备数据

            #region 变量定义

            //待更新的[出库单]
            var updateStockOutBill = paramHead.ToTBModelForSaveAndDelete<MDLPIS_StockOutBill>();

            //待删除的[应收单]
            MDLFM_AccountReceivableBill deleteAccountReceivableBill = new MDLFM_AccountReceivableBill();
            //待删除的[应收单明细]列表
            List<MDLFM_AccountReceivableBillDetail> deleteAccountReceivableBillDetailList = new List<MDLFM_AccountReceivableBillDetail>();

            //待新增的[库存异动日志]列表
            List<MDLPIS_InventoryTransLog> newInventoryTransLogList = new List<MDLPIS_InventoryTransLog>();
            //待更新的[库存]列表
            List<MDLPIS_Inventory> updateInventoryList = new List<MDLPIS_Inventory>();
            #endregion

            #region 更新[出库单]
            //将出库单[审核状态]更新为[待审核]，[单据状态]更新为[已生成]
            updateStockOutBill.SOB_VersionNo++;
            updateStockOutBill.SOB_ApprovalStatusCode = ApprovalStatusEnum.Code.DSH;
            updateStockOutBill.SOB_ApprovalStatusName = ApprovalStatusEnum.Name.DSH;
            updateStockOutBill.SOB_UpdatedBy = LoginInfoDAX.UserName;
            updateStockOutBill.SOB_UpdatedTime = BLLCom.GetCurStdDatetime();
            //updateStockOutBill.SOB_StatusCode = StockOutBillStatusEnum.Code.YSC;
            //updateStockOutBill.SOB_StatusName = StockOutBillStatusEnum.Name.YSC;

            #endregion

            #region 获取待删除的[应收单]

            _bll.QueryForObject<MDLFM_AccountReceivableBill, MDLFM_AccountReceivableBill>(new MDLFM_AccountReceivableBill
            {
                WHERE_ARB_SrcBillNo = updateStockOutBill.SOB_No,
                WHERE_ARB_IsValid = true
            }, deleteAccountReceivableBill);
            deleteAccountReceivableBill.WHERE_ARB_ID = deleteAccountReceivableBill.ARB_ID;
            #endregion

            #region 获取待删除的[应收单明细]列表

            if (!string.IsNullOrEmpty(deleteAccountReceivableBill.ARB_ID))
            {
                _bll.QueryForList<MDLFM_AccountReceivableBillDetail, MDLFM_AccountReceivableBillDetail>(new MDLFM_AccountReceivableBillDetail
                {
                    WHERE_ARBD_ARB_ID = deleteAccountReceivableBill.ARB_ID,
                    WHERE_ARBD_SrcBillNo = updateStockOutBill.SOB_No,
                    WHERE_ARBD_IsValid = true
                }, deleteAccountReceivableBillDetailList);
            }
            foreach (var loopAccountReceivableBillDetail in deleteAccountReceivableBillDetailList)
            {
                if (string.IsNullOrEmpty(loopAccountReceivableBillDetail.ARBD_ID))
                {
                    continue;
                }
                loopAccountReceivableBillDetail.WHERE_ARBD_ID = loopAccountReceivableBillDetail.ARBD_ID;
            }
            #endregion

            #region 获取待更新的[库存]列表，生成[库存异动日志]

            foreach (var loopDetail in paramDetailList)
            {
                //待更新的[库存]
                MDLPIS_Inventory updateInventory = new MDLPIS_Inventory();

                _bll.QueryForObject<MDLPIS_Inventory, MDLPIS_Inventory>(new MDLPIS_Inventory
                {
                    WHERE_INV_Org_ID = updateStockOutBill.SOB_Org_ID,
                    WHERE_INV_WH_ID = loopDetail.SOBD_WH_ID,
                    WHERE_INV_WHB_ID = loopDetail.SOBD_WHB_ID,
                    WHERE_INV_Barcode = loopDetail.SOBD_Barcode,
                    WHERE_INV_BatchNo = loopDetail.SOBD_BatchNo,
                    WHERE_INV_IsValid = true
                }, updateInventory);

                if (!string.IsNullOrEmpty(updateInventory.INV_ID)
                    && !string.IsNullOrEmpty(updateInventory.INV_Barcode)
                    && !string.IsNullOrEmpty(updateInventory.INV_BatchNo))
                {
                    //[反审核]时，恢复[审核]时出库的配件数量
                    updateInventory.INV_Qty += loopDetail.SOBD_Qty;
                    updateInventory.INV_UpdatedBy = LoginInfoDAX.UserName;
                    updateInventory.INV_UpdatedTime = BLLCom.GetCurStdDatetime();
                    updateInventory.WHERE_INV_ID = updateInventory.INV_ID;
                    updateInventory.WHERE_INV_VersionNo = updateInventory.INV_VersionNo;
                }

                updateInventoryList.Add(updateInventory);

                //生成[库存异动日志]
                newInventoryTransLogList.Add(GenerateInventoryTransLogOfUnApprove(updateStockOutBill, loopDetail, updateInventory));
            }

            #endregion

            #endregion

            #region 带事务的保存和删除

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 更新[出库单]

                bool updateStockOutBillResult = _bll.Save(updateStockOutBill);
                if (!updateStockOutBillResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_StockOutBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 删除[应收单]

                if (!string.IsNullOrEmpty(deleteAccountReceivableBill.ARB_ID))
                {
                    var deleteAccountReceivableBillResult = _bll.Delete<MDLFM_AccountReceivableBill>(deleteAccountReceivableBill);
                    if (!deleteAccountReceivableBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.FM_AccountReceivableBill });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #endregion

                #region 删除[应收单明细]

                if (deleteAccountReceivableBillDetailList.Count > 0)
                {
                    var deleteAccountReceivableBillDetailResult = _bll.DeleteByList<MDLFM_AccountReceivableBillDetail, MDLFM_AccountReceivableBillDetail>(deleteAccountReceivableBillDetailList);
                    if (!deleteAccountReceivableBillDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.FM_AccountReceivableBillDetail });
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
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.UNAPPROVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ex.Message, "", null);
                return false;
            }

            #endregion

            //将最新数据回写给DetailDS
            CopyModel(updateStockOutBill, paramHead);

            return true;
        }

        /// <summary>
        /// 获取出库单对应[应收单]
        /// </summary>
        /// <param name="paramStockOutBillNo"></param>
        /// <returns></returns>
        public MDLFM_AccountReceivableBill GetAccountReceivableBillByNo(string paramStockOutBillNo)
        {
            MDLFM_AccountReceivableBill resultAccountReceivableBill = new MDLFM_AccountReceivableBill();

            _bll.QueryForObject<MDLFM_AccountReceivableBill, MDLFM_AccountReceivableBill>(new MDLFM_AccountReceivableBill
            {
                WHERE_ARB_SrcBillNo = paramStockOutBillNo,
                WHERE_ARB_IsValid = true
            }, resultAccountReceivableBill);
            if (!string.IsNullOrEmpty(resultAccountReceivableBill.ARB_ID))
            {
                return resultAccountReceivableBill;
            }
            return new MDLFM_AccountReceivableBill();
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(StockOutBillManagerUIModel paramModel, SkyCarBindingList<StockOutBillManagerDetailUIModel, MDLPIS_StockOutBillDetail> paramDetailList)
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
        /// <param name="paramStockOutBillDetailList">出库单明细</param>
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
            newInventoryTransLog.ITL_Source = paramStockOutBillDetail.WH_Name;
            newInventoryTransLog.ITL_Destination = paramStockOutBillDetail.SUPP_Name;
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
            newInventoryTransLog.ITL_Source = paramStockOutBillDetail.WH_Name;
            newInventoryTransLog.ITL_Destination = paramStockOutBillDetail.SUPP_Name;
           
            return newInventoryTransLog;
        }

        #endregion
    }
}
