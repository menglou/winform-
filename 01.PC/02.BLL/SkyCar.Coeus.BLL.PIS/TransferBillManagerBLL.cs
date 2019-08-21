using System;
using System.Collections.Generic;
using System.Linq;
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
    /// 调拨管理BLL
    /// </summary>
    public class TransferBillManagerBLL : BLLBase
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
        /// 调拨管理BLL
        /// </summary>
        public TransferBillManagerBLL() : base(Trans.PIS)
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
        public bool SaveDetailDS(TransferBillManagerUIModel paramHead, SkyCarBindingList<TransferBillManagerDetailUIModel, MDLPIS_TransferBillDetail> paramDetailList)
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
            var argsHead = paramHead.ToTBModelForSaveAndDelete<MDLPIS_TransferBill>();
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(paramHead.TB_ID))
            {
                argsHead.TB_ID = Guid.NewGuid().ToString();
                //单号 
                argsHead.TB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.TB);
                argsHead.TB_CreatedBy = LoginInfoDAX.UserName;
                argsHead.TB_CreatedTime = BLLCom.GetCurStdDatetime();
            }
            argsHead.TB_UpdatedBy = LoginInfoDAX.UserName;
            argsHead.TB_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            #region 明细

            //添加的明细
            if (paramDetailList != null && paramDetailList.InsertList != null &&
                paramDetailList.InsertList.Count > 0)
            {
                foreach (var loopDetailItem in paramDetailList.InsertList)
                {
                    loopDetailItem.TBD_TB_ID = argsHead.TB_ID ?? argsHead.WHERE_TB_ID;
                    loopDetailItem.TBD_TB_No = argsHead.TB_No;
                    loopDetailItem.TBD_CreatedBy = LoginInfoDAX.UserName;
                    loopDetailItem.TBD_CreatedTime = BLLCom.GetCurStdDatetime();
                    loopDetailItem.TBD_UpdatedBy = LoginInfoDAX.UserName;
                    loopDetailItem.TBD_UpdatedTime = BLLCom.GetCurStdDatetime();
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
                var saveTransferBillResult = _bll.Save(argsHead, argsHead.TB_ID);
                if (!saveTransferBillResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_TransferBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存明细

                var saveTransferDetailResult = _bll.UnitySave(paramDetailList);
                //执行保存
                if (!saveTransferDetailResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_TransferBillDetail });
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
                        loopInsertDetail.TBD_VersionNo = 1;
                    }
                }

                foreach (var loopUpdateDetail in paramDetailList.UpdateList)
                {
                    //更新时版本号加1
                    loopUpdateDetail.TBD_VersionNo = loopUpdateDetail.TBD_VersionNo + 1;
                }
            }

            return true;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="paramHead">单头UIModel</param>
        /// <param name="paramDetailList">明细UIModel列表</param>
        /// <returns></returns>
        public bool ApproveDetailDS(TransferBillManagerUIModel paramHead, SkyCarBindingList<TransferBillManagerDetailUIModel, MDLPIS_TransferBillDetail> paramDetailList)
        {
            var funcName = "ApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.TB_ID)
                || string.IsNullOrEmpty(paramHead.TB_No))
            {
                //没有获取到调拨单，审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_TransferBill, SystemActionEnum.Name.APPROVE });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            #endregion

            #region 准备数据

            #region 变量定义

            //待更新的调拨单
            MDLPIS_TransferBill updateTransferBill = paramHead.ToTBModelForSaveAndDelete<MDLPIS_TransferBill>();

            //待保存的[库存]列表
            List<MDLPIS_Inventory> saveInventoryList = new List<MDLPIS_Inventory>();
            //待新增的[库存异动日志]列表
            List<MDLPIS_InventoryTransLog> newInventoryTransLogList = new List<MDLPIS_InventoryTransLog>();

            //待新增的[应收单]（调出组织的）
            MDLFM_AccountReceivableBill newAccountReceivableBill = new MDLFM_AccountReceivableBill();
            //待新增的[应收单明细]列表
            List<MDLFM_AccountReceivableBillDetail> newAccountReceivableBillDetailList = new List<MDLFM_AccountReceivableBillDetail>();

            //待新增的[应付单]（调入组织的）
            MDLFM_AccountPayableBill newAccountPayableBill = new MDLFM_AccountPayableBill();
            //待新增的[应付单明细]列表
            List<MDLFM_AccountPayableBillDetail> newAccountPayableBillDetailList = new List<MDLFM_AccountPayableBillDetail>();

            #endregion

            #region 更新[调拨单]

            //将调拨单审核状态更新为[已审核]，单据状态更新为[已完成]
            updateTransferBill.TB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            updateTransferBill.TB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            //期望：当前单据审核后，状态改为“已完成”；后期如果完善物流，再增加中间的待发货、已发货、部分签收、已签收状态。
            //if (paramHead.TB_TransferTypeName == TransferTypeEnum.Name.ZZJDB)
            //{
            //    updateTransferBill.TB_StatusCode = TransfeStatusEnum.Code.DFH;
            //    updateTransferBill.TB_StatusName = TransfeStatusEnum.Name.DFH;
            //}
            //else
            //{
            updateTransferBill.TB_StatusCode = TransfeStatusEnum.Code.YWC;
            updateTransferBill.TB_StatusName = TransfeStatusEnum.Name.YWC;
            //}

            updateTransferBill.TB_UpdatedBy = LoginInfoDAX.UserName;
            updateTransferBill.TB_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region 创建调出组织的[应收单]

            newAccountReceivableBill.ARB_ID = Guid.NewGuid().ToString();
            newAccountReceivableBill.ARB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.ARB);
            //[单据方向]为{正向}
            newAccountReceivableBill.ARB_BillDirectCode = BillDirectionEnum.Code.PLUS;
            newAccountReceivableBill.ARB_BillDirectName = BillDirectionEnum.Name.PLUS;
            //[来源类型]为{销售应收}
            newAccountReceivableBill.ARB_SourceTypeCode = AccountReceivableBillSourceTypeEnum.Code.XSYS;
            newAccountReceivableBill.ARB_SourceTypeName = AccountReceivableBillSourceTypeEnum.Name.XSYS;
            //[来源单号]为[调拨单号]
            newAccountReceivableBill.ARB_SrcBillNo = updateTransferBill.TB_No;
            newAccountReceivableBill.ARB_Org_ID = LoginInfoDAX.OrgID;
            newAccountReceivableBill.ARB_Org_Name = LoginInfoDAX.OrgShortName;
            //[付款对象类型]为{组织}
            newAccountReceivableBill.ARB_PayObjectTypeName = AmountTransObjectTypeEnum.Name.ORGANIZATION;
            newAccountReceivableBill.ARB_PayObjectTypeCode = AmountTransObjectTypeEnum.Code.ORGANIZATION;
            //[付款对象]为{调入组织}
            newAccountReceivableBill.ARB_PayObjectID = updateTransferBill.TB_TransferInOrgId;
            newAccountReceivableBill.ARB_PayObjectName = updateTransferBill.TB_TransferInOrgName;
            //[应收金额]为{调拨总金额}
            newAccountReceivableBill.ARB_AccountReceivableAmount = paramHead.TotalAmount;
            newAccountReceivableBill.ARB_ReceivedAmount = 0;
            newAccountReceivableBill.ARB_UnReceiveAmount = paramHead.TotalAmount;
            //[业务状态]为{执行中}
            newAccountReceivableBill.ARB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.ZXZ;
            newAccountReceivableBill.ARB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.ZXZ;
            //[审核状态]为{已审核}
            newAccountReceivableBill.ARB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            newAccountReceivableBill.ARB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            newAccountReceivableBill.ARB_IsValid = true;
            newAccountReceivableBill.ARB_CreatedBy = LoginInfoDAX.UserName;
            newAccountReceivableBill.ARB_CreatedTime = BLLCom.GetCurStdDatetime();
            newAccountReceivableBill.ARB_UpdatedBy = LoginInfoDAX.UserName;
            newAccountReceivableBill.ARB_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            #region 创建调入组织的[应付单]

            newAccountPayableBill.APB_ID = Guid.NewGuid().ToString();
            newAccountPayableBill.APB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.APB);
            //[单据方向]为{正向}
            newAccountPayableBill.APB_BillDirectCode = BillDirectionEnum.Code.PLUS;
            newAccountPayableBill.APB_BillDirectName = BillDirectionEnum.Name.PLUS;
            //[来源类型]为{收货应付}
            newAccountPayableBill.APB_SourceTypeCode = AccountPayableBillSourceTypeEnum.Code.SHYF;
            newAccountPayableBill.APB_SourceTypeName = AccountPayableBillSourceTypeEnum.Name.SHYF;
            //[来源单号]为{调拨单号}
            newAccountPayableBill.APB_SourceBillNo = updateTransferBill.TB_No;
            newAccountPayableBill.APB_Org_ID = updateTransferBill.TB_TransferInOrgId;
            newAccountPayableBill.APB_Org_Name = updateTransferBill.TB_TransferInOrgName;
            //[收款对象类型]为{组织}
            newAccountPayableBill.APB_ReceiveObjectTypeCode = AmountTransObjectTypeEnum.Code.ORGANIZATION;
            newAccountPayableBill.APB_ReceiveObjectTypeName = AmountTransObjectTypeEnum.Name.ORGANIZATION;
            //[收款对象]为{调出组织}
            newAccountPayableBill.APB_ReceiveObjectID = updateTransferBill.TB_TransferOutOrgId;
            newAccountPayableBill.APB_ReceiveObjectName = updateTransferBill.TB_TransferOutOrgName;
            //[应付金额]为{调拨总金额}
            newAccountPayableBill.APB_AccountPayableAmount = paramHead.TotalAmount;
            newAccountPayableBill.APB_PaidAmount = 0;
            newAccountPayableBill.APB_UnpaidAmount = paramHead.TotalAmount;
            //[业务状态]为{执行中}
            newAccountPayableBill.APB_BusinessStatusCode = AccountPayableBillStatusEnum.Code.ZXZ;
            newAccountPayableBill.APB_BusinessStatusName = AccountPayableBillStatusEnum.Name.ZXZ;
            //[审核状态]为{已审核}
            newAccountPayableBill.APB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            newAccountPayableBill.APB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            newAccountPayableBill.APB_IsValid = true;
            newAccountPayableBill.APB_CreatedBy = LoginInfoDAX.UserName;
            newAccountPayableBill.APB_CreatedTime = BLLCom.GetCurStdDatetime();
            newAccountPayableBill.APB_UpdatedBy = LoginInfoDAX.UserName;
            newAccountPayableBill.APB_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            #region 遍历[调拨单明细]列表，创建或更新[库存]，创建[库存异动日志]，创建[应收单明细]，创建[应付单明细]

            foreach (var loopTransferBillDetail in paramDetailList)
            {
                if (string.IsNullOrEmpty(loopTransferBillDetail.TBD_Barcode)
                    || string.IsNullOrEmpty(loopTransferBillDetail.TBD_TransOutBatchNo)
                    || string.IsNullOrEmpty(loopTransferBillDetail.TBD_TransOutWhId))
                {
                    continue;
                }

                if (updateTransferBill.TB_TypeName == TransferBillTypeEnum.Name.YBS)
                {
                    #region 单据类型为[一步式调拨]的场合，[调入组织]和[调出组织]同时调整库存

                    #region 库存和库存异动日志
                    //在[调拨单明细]列表中第一次出现的[调出组织]的配件[库存]信息
                    MDLPIS_Inventory transOutInventoryExists = null;
                    foreach (var loopInventory in saveInventoryList)
                    {
                        if (loopInventory.INV_Barcode == loopTransferBillDetail.TBD_Barcode
                            && loopInventory.INV_BatchNo == loopTransferBillDetail.TBD_TransOutBatchNo
                            && loopInventory.INV_WH_ID == loopTransferBillDetail.TBD_TransOutWhId)
                        {
                            transOutInventoryExists = loopInventory;
                            break;
                        }
                    }

                    //在[调拨单明细]列表中第一次出现的[调入组织]的配件[库存]信息
                    MDLPIS_Inventory transInInventoryExists = null;
                    foreach (var loopInventory in saveInventoryList)
                    {
                        if (loopInventory.INV_Barcode == loopTransferBillDetail.TBD_Barcode
                            && loopInventory.INV_WH_ID == loopTransferBillDetail.TBD_TransInWhId)
                        {
                            transInInventoryExists = loopInventory;
                            break;
                        }
                    }

                    #region [调拨类型]为{组织间调拨}或{仓库转储}的场合

                    if (updateTransferBill.TB_TransferTypeName == TransferTypeEnum.Name.ZZJDB
                        || updateTransferBill.TB_TransferTypeName == TransferTypeEnum.Name.CKZC)
                    {
                        #region 调出组织的[库存]和[库存异动日志]

                        if (transOutInventoryExists != null)
                        {
                            if (transOutInventoryExists.INV_Qty < loopTransferBillDetail.TBD_Qty)
                            {
                                //配件：loopTransferBillDetail.TBD_Name（条形码：loopTransferBillDetail.TBD_Barcode）的库存不足，审核失败
                                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                                {
                                loopTransferBillDetail.TBD_Name, loopTransferBillDetail.TBD_Barcode, MsgParam.SHORTAGE,
                                SystemActionEnum.Name.APPROVE
                                });
                                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                                return false;
                            }
                            //[调拨单明细]列表中已遍历过该配件，累减数量
                            transOutInventoryExists.INV_Qty -= loopTransferBillDetail.TBD_Qty;

                            //生成[调出组织]的[库存异动日志]
                            newInventoryTransLogList.Add(GenerateInventoryTransLogOfApprove(updateTransferBill, loopTransferBillDetail, transOutInventoryExists, true));
                        }
                        else
                        {
                            //[调拨单明细]列表中第一次出现[调出组织]的该配件
                            //查询该配件是否在[库存]中存在
                            MDLPIS_Inventory resultInventory = new MDLPIS_Inventory();
                            _bll.QueryForObject<MDLPIS_Inventory, MDLPIS_Inventory>(new MDLPIS_Inventory
                            {
                                WHERE_INV_Org_ID = updateTransferBill.TB_TransferOutOrgId,
                                WHERE_INV_Barcode = loopTransferBillDetail.TBD_Barcode,
                                WHERE_INV_BatchNo = loopTransferBillDetail.TBD_TransOutBatchNo,
                                WHERE_INV_WH_ID = loopTransferBillDetail.TBD_TransOutWhId,
                                WHERE_INV_IsValid = true
                            }, resultInventory);

                            //[库存]中不存在该配件
                            if (string.IsNullOrEmpty(resultInventory.INV_ID))
                            {
                                //配件：loopTransferBillDetail.TBD_Name（条形码：loopTransferBillDetail.TBD_Barcode）的库存不存在，审核失败
                                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                                {
                                loopTransferBillDetail.TBD_Name, loopTransferBillDetail.TBD_Barcode, MsgParam.NOTEXIST,
                                SystemActionEnum.Name.APPROVE
                                });
                                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                                return false;
                            }

                            if (resultInventory.INV_Qty < loopTransferBillDetail.TBD_Qty)
                            {
                                //配件：loopTransferBillDetail.TBD_Name（条形码：loopTransferBillDetail.TBD_Barcode）的库存不足，审核失败
                                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                                {
                                loopTransferBillDetail.TBD_Name, loopTransferBillDetail.TBD_Barcode, MsgParam.SHORTAGE,
                                SystemActionEnum.Name.APPROVE
                                });
                                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                                return false;
                            }
                            //[库存]中存在该配件
                            //更新[库存]中该配件的数量
                            resultInventory.INV_Qty -= loopTransferBillDetail.TBD_Qty;
                            resultInventory.INV_UpdatedBy = LoginInfoDAX.UserName;
                            resultInventory.INV_UpdatedTime = BLLCom.GetCurStdDatetime();
                            resultInventory.WHERE_INV_ID = resultInventory.INV_ID;
                            resultInventory.WHERE_INV_VersionNo = resultInventory.INV_VersionNo;
                            saveInventoryList.Add(resultInventory);

                            //生成[调出组织]的[库存异动日志]
                            newInventoryTransLogList.Add(GenerateInventoryTransLogOfApprove(updateTransferBill, loopTransferBillDetail, resultInventory, true));
                        }
                        #endregion

                        #region 调入组织的[库存]和[库存异动日志]

                        if (transInInventoryExists != null)
                        {
                            transInInventoryExists.INV_Qty += loopTransferBillDetail.TBD_Qty;

                            //当前库存对应的[库存异动日志]
                            var curInventoryLog =
                                newInventoryTransLogList.FirstOrDefault(
                                    x =>
                                        x.ITL_Org_ID == transInInventoryExists.INV_Org_ID &&
                                        x.ITL_Barcode == transInInventoryExists.INV_Barcode &&
                                        x.ITL_BatchNo == transInInventoryExists.INV_BatchNo &&
                                        x.ITL_WH_ID == transInInventoryExists.INV_WH_ID);
                            if (curInventoryLog != null)
                            {
                                curInventoryLog.ITL_Qty = transInInventoryExists.INV_Qty;
                                curInventoryLog.ITL_AfterTransQty = transInInventoryExists.INV_Qty;
                            }

                            loopTransferBillDetail.TBD_TransInBatchNo = transInInventoryExists.INV_BatchNo;
                        }
                        else
                        {
                            //新增一个该配件的库存信息
                            MDLPIS_Inventory inventoryToInsert = new MDLPIS_Inventory
                            {
                                INV_Org_ID = updateTransferBill.TB_TransferInOrgId,
                                INV_SUPP_ID = loopTransferBillDetail.TBD_SUPP_ID,
                                INV_WH_ID = loopTransferBillDetail.TBD_TransInWhId,
                                INV_WHB_ID = loopTransferBillDetail.TBD_TransInBinId,
                                INV_ThirdNo = loopTransferBillDetail.TBD_ThirdNo,
                                INV_OEMNo = loopTransferBillDetail.TBD_OEMNo,
                                INV_Barcode = loopTransferBillDetail.TBD_Barcode,
                                INV_Name = loopTransferBillDetail.TBD_Name,
                                INV_Specification = loopTransferBillDetail.TBD_Specification,
                                INV_Qty = loopTransferBillDetail.TBD_Qty,
                                INV_PurchaseUnitPrice = loopTransferBillDetail.TBD_DestUnitPrice,
                                INV_IsValid = true,
                                INV_CreatedBy = LoginInfoDAX.UserName,
                                INV_CreatedTime = BLLCom.GetCurStdDatetime(),
                                INV_UpdatedBy = LoginInfoDAX.UserName,
                                INV_UpdatedTime = BLLCom.GetCurStdDatetime(),
                            };
                            //生成新的批次号
                            inventoryToInsert.INV_BatchNo = BLLCom.GetBatchNo(new MDLPIS_Inventory
                            {
                                WHERE_INV_Org_ID = updateTransferBill.TB_TransferInOrgId,
                                WHERE_INV_Barcode = loopTransferBillDetail.TBD_Barcode,
                            });
                            //验证批次号
                            if (string.IsNullOrEmpty(inventoryToInsert.INV_BatchNo))
                            {
                                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { MsgParam.BATCHNO, SystemActionEnum.Name.APPROVE, });
                                return false;
                            }
                            loopTransferBillDetail.TBD_TransInBatchNo = inventoryToInsert.INV_BatchNo;

                            saveInventoryList.Add(inventoryToInsert);

                            //生成[调入组织]的[库存异动日志]
                            newInventoryTransLogList.Add(GenerateInventoryTransLogOfApprove(updateTransferBill, loopTransferBillDetail, inventoryToInsert, false));
                        }

                        #endregion
                    }
                    else
                    {
                        #region [调拨类型]为{库位转储}的场合

                        if (transOutInventoryExists != null)
                        {
                            if (transOutInventoryExists.INV_Qty < loopTransferBillDetail.TBD_Qty)
                            {
                                //配件：loopTransferBillDetail.TBD_Name（条形码：loopTransferBillDetail.TBD_Barcode）的库存不足，审核失败
                                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                                {
                                loopTransferBillDetail.TBD_Name, loopTransferBillDetail.TBD_Barcode, MsgParam.SHORTAGE,
                                SystemActionEnum.Name.APPROVE
                                });
                                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                                return false;
                            }
                            //更新库存仓位
                            transOutInventoryExists.INV_WHB_ID = loopTransferBillDetail.TBD_TransInBinId;

                            //生成[调出组织]的[库存异动日志]
                            newInventoryTransLogList.Add(GenerateInventoryTransLogOfApprove(updateTransferBill, loopTransferBillDetail, transOutInventoryExists, true));
                            //生成[调入组织]的[库存异动日志]
                            newInventoryTransLogList.Add(GenerateInventoryTransLogOfApprove(updateTransferBill, loopTransferBillDetail, transOutInventoryExists, false));
                        }
                        else
                        {
                            //[调拨单明细]列表中第一次出现[调出组织]的该配件
                            //查询该配件是否在[库存]中存在
                            MDLPIS_Inventory resultInventory = new MDLPIS_Inventory();
                            _bll.QueryForObject<MDLPIS_Inventory, MDLPIS_Inventory>(new MDLPIS_Inventory
                            {
                                WHERE_INV_Org_ID = updateTransferBill.TB_TransferOutOrgId,
                                WHERE_INV_Barcode = loopTransferBillDetail.TBD_Barcode,
                                WHERE_INV_BatchNo = loopTransferBillDetail.TBD_TransOutBatchNo,
                                WHERE_INV_WH_ID = loopTransferBillDetail.TBD_TransOutWhId,
                                WHERE_INV_IsValid = true
                            }, resultInventory);

                            //[库存]中不存在该配件
                            if (string.IsNullOrEmpty(resultInventory.INV_ID))
                            {
                                //配件：loopTransferBillDetail.TBD_Name（条形码：loopTransferBillDetail.TBD_Barcode）的库存不存在，审核失败
                                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                                {
                                loopTransferBillDetail.TBD_Name, loopTransferBillDetail.TBD_Barcode, MsgParam.NOTEXIST,
                                SystemActionEnum.Name.APPROVE
                                });
                                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                                return false;
                            }

                            if (resultInventory.INV_Qty < loopTransferBillDetail.TBD_Qty)
                            {
                                //配件：loopTransferBillDetail.TBD_Name（条形码：loopTransferBillDetail.TBD_Barcode）的库存不足，审核失败
                                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                                {
                                loopTransferBillDetail.TBD_Name, loopTransferBillDetail.TBD_Barcode, MsgParam.SHORTAGE,
                                SystemActionEnum.Name.APPROVE
                                });
                                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                                return false;
                            }
                            //[库存]中存在该配件
                            loopTransferBillDetail.TBD_TransInBatchNo = loopTransferBillDetail.TBD_TransOutBatchNo;
                            //更新库存仓位
                            resultInventory.INV_WHB_ID = loopTransferBillDetail.TBD_TransInBinId;
                            resultInventory.INV_UpdatedBy = LoginInfoDAX.UserName;
                            resultInventory.INV_UpdatedTime = BLLCom.GetCurStdDatetime();
                            resultInventory.WHERE_INV_ID = resultInventory.INV_ID;
                            resultInventory.WHERE_INV_VersionNo = resultInventory.INV_VersionNo;
                            saveInventoryList.Add(resultInventory);

                            //生成[调出组织]的[库存异动日志]
                            newInventoryTransLogList.Add(GenerateInventoryTransLogOfApprove(updateTransferBill, loopTransferBillDetail, resultInventory, true));
                            //生成[调入组织]的[库存异动日志]
                            newInventoryTransLogList.Add(GenerateInventoryTransLogOfApprove(updateTransferBill, loopTransferBillDetail, resultInventory, false));
                        }
                        #endregion  
                    }

                    #endregion

                    #endregion

                    #endregion
                }

                #region 调拨单明细

                if (string.IsNullOrEmpty(loopTransferBillDetail.TBD_ID))
                {
                    loopTransferBillDetail.TBD_ID = Guid.NewGuid().ToString();
                }
                #endregion

                #region 创建[应收单明细]

                //新增[来源类型]为[销售应收]，[业务状态]为[已生成]，金额为负，[审核状态]为[已审核]的[应收单明细]，[是否负向明细]根据[销售订单].[来源类型]区分
                MDLFM_AccountReceivableBillDetail newAccountReceivableBillDetail = new MDLFM_AccountReceivableBillDetail
                {
                    ARBD_ID = Guid.NewGuid().ToString(),
                    ARBD_ARB_ID = newAccountReceivableBill.ARB_ID,
                    ARBD_IsMinusDetail = newAccountReceivableBill.ARB_BillDirectName != BillDirectionEnum.Name.PLUS,
                    ARBD_SrcBillNo = updateTransferBill.TB_No,
                    ARBD_SrcBillDetailID = loopTransferBillDetail.TBD_ID,
                    ARBD_Org_ID = newAccountReceivableBill.ARB_Org_ID,
                    ARBD_Org_Name = newAccountReceivableBill.ARB_Org_Name,
                    ARBD_AccountReceivableAmount = loopTransferBillDetail.DetailDestAmount,
                    ARBD_ReceivedAmount = 0,
                    ARBD_UnReceiveAmount = loopTransferBillDetail.DetailDestAmount,
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

                #region 应付单明细

                MDLFM_AccountPayableBillDetail newAccountPayableBillDetail = new MDLFM_AccountPayableBillDetail
                {
                    APBD_ID = Guid.NewGuid().ToString(),
                    APBD_APB_ID = newAccountPayableBill.APB_ID,
                    APBD_IsMinusDetail = false,
                    APBD_SourceBillNo = updateTransferBill.TB_No,
                    APBD_SourceBillDetailID = loopTransferBillDetail.TBD_ID,
                    APBD_Org_ID = newAccountPayableBill.APB_Org_ID,
                    APBD_Org_Name = newAccountPayableBill.APB_Org_Name,
                    APBD_AccountPayableAmount = loopTransferBillDetail.DetailDestAmount,
                    APBD_PaidAmount = 0,
                    APBD_UnpaidAmount = loopTransferBillDetail.DetailDestAmount,
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
            }
            #endregion

            #endregion

            #region 带事务的新增和保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存[调拨单]

                bool updateTransferBillResult = _bll.Save(updateTransferBill);
                if (!updateTransferBillResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.PIS_TransferBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存[调拨单明细]

                var saveTransferDetailResult = _bll.UnitySave(paramDetailList);
                if (!saveTransferDetailResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_TransferBillDetail });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存[库存]

                foreach (var loopInventory in saveInventoryList)
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
            CopyModel(updateTransferBill, paramHead);

            //更新明细版本号
            foreach (var loopInsertDetail in paramDetailList.InsertList)
            {
                //新增时版本号为1
                loopInsertDetail.TBD_VersionNo = 1;
            }
            foreach (var loopUpdateDetail in paramDetailList.UpdateList)
            {
                //更新时版本号加1
                loopUpdateDetail.TBD_VersionNo = loopUpdateDetail.TBD_VersionNo + 1;
            }

            return true;
        }

        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="paramHead">单头UIModel</param>
        /// <param name="paramDetailList">明细UIModel列表</param>
        /// <returns></returns>
        public bool UnApproveDetailDS(TransferBillManagerUIModel paramHead, SkyCarBindingList<TransferBillManagerDetailUIModel, MDLPIS_TransferBillDetail> paramDetailList)
        {
            var funcName = "UnApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.TB_ID)
                || string.IsNullOrEmpty(paramHead.TB_No))
            {
                //没有获取到调拨单，反审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_TransferBill, SystemActionEnum.Name.UNAPPROVE });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }
            #endregion

            #region 准备数据

            #region 变量定义

            //待更新的[调拨单]
            var updateTransferBill = paramHead.ToTBModelForSaveAndDelete<MDLPIS_TransferBill>();

            //待新增的[库存异动日志]列表
            List<MDLPIS_InventoryTransLog> newInventoryTransLogList = new List<MDLPIS_InventoryTransLog>();
            //待更新的[库存]列表
            List<MDLPIS_Inventory> updateInventoryList = new List<MDLPIS_Inventory>();

            //待删除的[应收单]
            MDLFM_AccountReceivableBill deleteAccountReceivableBill = new MDLFM_AccountReceivableBill();
            //待删除的[应收单明细]列表
            List<MDLFM_AccountReceivableBillDetail> deleteAccountReceivableDetailList = new List<MDLFM_AccountReceivableBillDetail>();

            //待删除的[应付单]
            MDLFM_AccountPayableBill deleteAccountPayableBill = new MDLFM_AccountPayableBill();
            //待删除的[应付单明细]列表
            List<MDLFM_AccountPayableBillDetail> deleteAccountPayableBillDetailList = new List<MDLFM_AccountPayableBillDetail>();
            #endregion

            #region 更新[调拨单]

            //将调拨单[审核状态]更新为[待审核]，[单据状态]更新为[已生成]
            updateTransferBill.TB_ApprovalStatusCode = ApprovalStatusEnum.Code.DSH;
            updateTransferBill.TB_ApprovalStatusName = ApprovalStatusEnum.Name.DSH;
            updateTransferBill.TB_StatusCode = StockInBillStatusEnum.Code.YSC;
            updateTransferBill.TB_StatusName = StockInBillStatusEnum.Name.YSC;
            updateTransferBill.TB_UpdatedBy = LoginInfoDAX.UserName;
            updateTransferBill.TB_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            if (updateTransferBill.TB_TypeName == TransferBillTypeEnum.Name.YBS)
            {
                #region 单据类型为[一步式调拨]的场合，[调入组织]和[调出组织]同时调整库存，获取待更新的[库存]列表，生成[库存异动日志]

                if (updateTransferBill.TB_TransferTypeName == TransferTypeEnum.Name.ZZJDB
                    || updateTransferBill.TB_TransferTypeName == TransferTypeEnum.Name.CKZC)
                {
                    #region [组织间调拨]、[仓库转储]的场合

                    foreach (var loopDetail in paramDetailList)
                    {
                        #region 调出库存和异动

                        //待更新的[调出组织]的[库存]
                        MDLPIS_Inventory updateTransOutInventory = new MDLPIS_Inventory();

                        _bll.QueryForObject<MDLPIS_Inventory, MDLPIS_Inventory>(new MDLPIS_Inventory
                        {
                            WHERE_INV_Org_ID = updateTransferBill.TB_TransferOutOrgId,
                            WHERE_INV_WH_ID = loopDetail.TBD_TransOutWhId,
                            WHERE_INV_Barcode = loopDetail.TBD_Barcode,
                            WHERE_INV_BatchNo = loopDetail.TBD_TransOutBatchNo,
                            WHERE_INV_IsValid = true
                        }, updateTransOutInventory);

                        if (!string.IsNullOrEmpty(updateTransOutInventory.INV_ID)
                            && !string.IsNullOrEmpty(updateTransOutInventory.INV_Barcode)
                            && !string.IsNullOrEmpty(updateTransOutInventory.INV_BatchNo))
                        {
                            //[反审核]时，恢复[审核]时[调出组织]中出库的配件数量
                            updateTransOutInventory.INV_Qty += loopDetail.TBD_Qty;
                            updateTransOutInventory.INV_UpdatedBy = LoginInfoDAX.UserName;
                            updateTransOutInventory.INV_UpdatedTime = BLLCom.GetCurStdDatetime();
                            updateTransOutInventory.WHERE_INV_ID = updateTransOutInventory.INV_ID;
                            updateTransOutInventory.WHERE_INV_VersionNo = updateTransOutInventory.INV_VersionNo;
                        }
                        else
                        {
                            //异常数据
                            //TODO
                        }

                        updateInventoryList.Add(updateTransOutInventory);

                        //生成出库的[库存异动日志]
                        newInventoryTransLogList.Add(GenerateInventoryTransLogOfUnApprove(updateTransferBill, loopDetail, updateTransOutInventory));

                        #endregion

                        #region 调入库存和异动

                        //在[调拨单明细]列表中第一次出现的[调入组织]的配件[库存]信息
                        MDLPIS_Inventory transInInventoryExists = null;
                        foreach (var loopInventory in updateInventoryList)
                        {
                            if (loopInventory.INV_Barcode == loopDetail.TBD_Barcode
                                && loopInventory.INV_BatchNo == loopDetail.TBD_TransInBatchNo
                                && loopInventory.INV_WH_ID == loopDetail.TBD_TransInWhId)
                            {
                                transInInventoryExists = loopInventory;
                                break;
                            }
                        }
                        if (transInInventoryExists != null)
                        {
                            transInInventoryExists.INV_Qty -= loopDetail.TBD_Qty;
                            //明细中的配件被使用过的场合，不能反审核
                            if (transInInventoryExists.INV_Qty < 0)
                            {
                                //配件：updateTransInInventory.INV_Name （条形码：updateTransInInventory.INV_Barcode）的库存不足，反审核失败
                                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                                {
                                transInInventoryExists.INV_Name, transInInventoryExists.INV_Barcode, MsgParam.SHORTAGE,
                                SystemActionEnum.Name.UNAPPROVE
                                });
                                return false;
                            }

                            //当前库存对应的[库存异动日志]
                            var curInventoryLog =
                                newInventoryTransLogList.FirstOrDefault(
                                    x =>
                                        x.ITL_Org_ID == transInInventoryExists.INV_Org_ID &&
                                        x.ITL_Barcode == transInInventoryExists.INV_Barcode &&
                                        x.ITL_BatchNo == transInInventoryExists.INV_BatchNo &&
                                        x.ITL_WH_ID == transInInventoryExists.INV_WH_ID);
                            if (curInventoryLog != null)
                            {
                                //入库 反审核 实际出库，异动数量为负
                                curInventoryLog.ITL_Qty -= loopDetail.TBD_Qty;
                                curInventoryLog.ITL_AfterTransQty = transInInventoryExists.INV_Qty;
                            }
                        }
                        else
                        {
                            //待更新的[调入组织]的[库存]
                            MDLPIS_Inventory updateTransInInventory = new MDLPIS_Inventory();

                            _bll.QueryForObject<MDLPIS_Inventory, MDLPIS_Inventory>(new MDLPIS_Inventory
                            {
                                WHERE_INV_Org_ID = updateTransferBill.TB_TransferInOrgId,
                                WHERE_INV_WH_ID = loopDetail.TBD_TransInWhId,
                                WHERE_INV_Barcode = loopDetail.TBD_Barcode,
                                WHERE_INV_BatchNo = loopDetail.TBD_TransInBatchNo,
                                WHERE_INV_IsValid = true
                            }, updateTransInInventory);

                            if (!string.IsNullOrEmpty(updateTransInInventory.INV_ID)
                                && !string.IsNullOrEmpty(updateTransInInventory.INV_Barcode)
                                && !string.IsNullOrEmpty(updateTransInInventory.INV_BatchNo))
                            {
                                //[反审核]时，恢复[审核]时[调入组织]中入库的配件数量
                                updateTransInInventory.INV_Qty -= loopDetail.TBD_Qty;
                                //明细中的配件被使用过的场合，不能反审核
                                if (updateTransInInventory.INV_Qty < 0)
                                {
                                    //配件：updateTransInInventory.INV_Name （条形码：updateTransInInventory.INV_Barcode）的库存不足，反审核失败
                                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                                    {
                                updateTransInInventory.INV_Name, updateTransInInventory.INV_Barcode, MsgParam.SHORTAGE,
                                SystemActionEnum.Name.UNAPPROVE
                                    });
                                    return false;
                                }
                                updateTransInInventory.INV_UpdatedBy = LoginInfoDAX.UserName;
                                updateTransInInventory.INV_UpdatedTime = BLLCom.GetCurStdDatetime();
                                updateTransInInventory.WHERE_INV_ID = updateTransInInventory.INV_ID;
                                updateTransInInventory.WHERE_INV_VersionNo = updateTransInInventory.INV_VersionNo;
                            }
                            else
                            {
                                //异常数据
                                //TODO
                            }

                            updateInventoryList.Add(updateTransInInventory);

                            //生成[库存异动日志]
                            newInventoryTransLogList.Add(GenerateInventoryTransLogOfUnApprove(updateTransferBill, loopDetail, updateTransInInventory, false));
                        }

                        #endregion
                    }

                    #endregion
                }
                else
                {
                    #region [仓位转储]的场合

                    foreach (var loopDetail in paramDetailList)
                    {
                        //待更新的[调入组织]的[库存]
                        MDLPIS_Inventory updateTransInInventory = new MDLPIS_Inventory();
                        _bll.QueryForObject<MDLPIS_Inventory, MDLPIS_Inventory>(new MDLPIS_Inventory
                        {
                            WHERE_INV_Org_ID = updateTransferBill.TB_TransferInOrgId,
                            WHERE_INV_WH_ID = loopDetail.TBD_TransInWhId,
                            WHERE_INV_WHB_ID = loopDetail.TBD_TransInBinId,
                            WHERE_INV_Barcode = loopDetail.TBD_Barcode,
                            WHERE_INV_BatchNo = loopDetail.TBD_TransInBatchNo,
                            WHERE_INV_IsValid = true
                        }, updateTransInInventory);

                        if (!string.IsNullOrEmpty(updateTransInInventory.INV_ID)
                            && !string.IsNullOrEmpty(updateTransInInventory.INV_Barcode)
                            && !string.IsNullOrEmpty(updateTransInInventory.INV_BatchNo))
                        {
                            //[反审核]时，恢复库位
                            updateTransInInventory.INV_WHB_ID = loopDetail.TBD_TransOutBinId;
                            //明细中的配件被使用过的场合，不能反审核
                            if (updateTransInInventory.INV_Qty <= 0)
                            {
                                //配件：updateTransInInventory.INV_Name （条形码：updateTransInInventory.INV_Barcode）的库存不足，反审核失败
                                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                                {
                                updateTransInInventory.INV_Name, updateTransInInventory.INV_Barcode, MsgParam.SHORTAGE,
                                SystemActionEnum.Name.UNAPPROVE
                                });
                                return false;
                            }
                            updateTransInInventory.INV_UpdatedBy = LoginInfoDAX.UserName;
                            updateTransInInventory.INV_UpdatedTime = BLLCom.GetCurStdDatetime();
                            updateTransInInventory.WHERE_INV_ID = updateTransInInventory.INV_ID;
                            updateTransInInventory.WHERE_INV_VersionNo = updateTransInInventory.INV_VersionNo;
                        }
                        else
                        {
                            //异常数据
                            //TODO
                        }

                        updateInventoryList.Add(updateTransInInventory);

                        //生成入库的[库存异动日志]
                        newInventoryTransLogList.Add(GenerateInventoryTransLogOfUnApprove(updateTransferBill, loopDetail, updateTransInInventory, false));

                        //待更新的[调出组织]的[库存]
                        MDLPIS_Inventory updateTransOutInventory = new MDLPIS_Inventory();
                        updateTransOutInventory = updateTransInInventory;
                        updateTransOutInventory.INV_WHB_ID = loopDetail.TBD_TransInBinId;
                        //生成出库的[库存异动日志]
                        newInventoryTransLogList.Add(GenerateInventoryTransLogOfUnApprove(updateTransferBill, loopDetail, updateTransOutInventory, true));
                    }
                    #endregion
                }

                #endregion
            }

            #region 获取待删除的[应收单]

            _bll.QueryForObject<MDLFM_AccountReceivableBill, MDLFM_AccountReceivableBill>(new MDLFM_AccountReceivableBill
            {
                WHERE_ARB_SrcBillNo = updateTransferBill.TB_No,
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
                    WHERE_ARBD_SrcBillNo = updateTransferBill.TB_No,
                    WHERE_ARBD_IsValid = true
                }, deleteAccountReceivableDetailList);
            }
            foreach (var loopAccountReceivableBillDetail in deleteAccountReceivableDetailList)
            {
                if (string.IsNullOrEmpty(loopAccountReceivableBillDetail.ARBD_ID))
                {
                    continue;
                }
                loopAccountReceivableBillDetail.WHERE_ARBD_ID = loopAccountReceivableBillDetail.ARBD_ID;
            }
            #endregion

            #region 获取待删除的[应付单]

            _bll.QueryForObject<MDLFM_AccountPayableBill, MDLFM_AccountPayableBill>(new MDLFM_AccountPayableBill
            {
                WHERE_APB_SourceBillNo = updateTransferBill.TB_No,
                WHERE_APB_IsValid = true
            }, deleteAccountPayableBill);
            deleteAccountPayableBill.WHERE_APB_ID = deleteAccountPayableBill.APB_ID;
            #endregion

            #region 获取待删除的[应付单明细]列表

            if (!string.IsNullOrEmpty(deleteAccountPayableBill.APB_ID))
            {
                _bll.QueryForList<MDLFM_AccountPayableBillDetail, MDLFM_AccountPayableBillDetail>(new MDLFM_AccountPayableBillDetail
                {
                    WHERE_APBD_APB_ID = deleteAccountPayableBill.APB_ID,
                    WHERE_APBD_SourceBillNo = updateTransferBill.TB_No,
                    WHERE_APBD_IsValid = true
                }, deleteAccountPayableBillDetailList);
            }
            foreach (var loopAccountPayableBillDetail in deleteAccountPayableBillDetailList)
            {
                if (string.IsNullOrEmpty(loopAccountPayableBillDetail.APBD_ID))
                {
                    continue;
                }
                loopAccountPayableBillDetail.WHERE_APBD_ID = loopAccountPayableBillDetail.APBD_ID;
            }
            #endregion
            #endregion

            #region 带事务的保存和删除

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 更新[调拨单]

                bool updateTransferBillResult = _bll.Save(updateTransferBill);
                if (!updateTransferBillResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_TransferBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 更新[调拨单明细]列表
                //清除[调入批次号]
                //前提：更新调拨单明细时同时更新调拨单
                //TODO 直接调用UpdateList方法
                bool updateTransferDetailResult = _bll.Update(SQLID.PIS_TransferBillManager_SQL_03, updateTransferBill.TB_ID);
                if (!updateTransferDetailResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_TransferBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
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

                #region 删除[应收单明细]列表

                if (deleteAccountReceivableDetailList.Count > 0)
                {
                    var deleteAccountReceivableDetailResult = _bll.DeleteByList<MDLFM_AccountReceivableBillDetail, MDLFM_AccountReceivableBillDetail>(deleteAccountReceivableDetailList);
                    if (!deleteAccountReceivableDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.FM_AccountReceivableBillDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #endregion

                #region 删除[应付单]

                if (!string.IsNullOrEmpty(deleteAccountPayableBill.APB_ID))
                {
                    var deleteAccountPayableBillResult = _bll.Delete<MDLFM_AccountPayableBill>(deleteAccountPayableBill);
                    if (!deleteAccountPayableBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.FM_AccountPayableBill });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #endregion

                #region 删除[应付单明细]

                if (deleteAccountPayableBillDetailList.Count > 0)
                {
                    var deleteAccountPayableBillDetailResult = _bll.DeleteByList<MDLFM_AccountPayableBillDetail, MDLFM_AccountPayableBillDetail>(deleteAccountPayableBillDetailList);
                    if (!deleteAccountPayableBillDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.FM_AccountPayableBillDetail });
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
            CopyModel(updateTransferBill, paramHead);

            //更新明细版本号
            if (paramDetailList != null)
            {
                if (paramDetailList.InsertList != null)
                {
                    foreach (var loopInsertDetail in paramDetailList.InsertList)
                    {
                        //新增时版本号为1
                        loopInsertDetail.TBD_VersionNo = 1;
                    }
                }

                foreach (var loopUpdateDetail in paramDetailList.UpdateList)
                {
                    //更新时版本号加1
                    loopUpdateDetail.TBD_VersionNo = loopUpdateDetail.TBD_VersionNo + 1;
                }
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
        private bool ServerCheck(TransferBillManagerUIModel paramModel, SkyCarBindingList<TransferBillManagerDetailUIModel, MDLPIS_TransferBillDetail> paramDetailList)
        {
            return true;
        }

        /// <summary>
        /// 【审核】生成库存异动日志
        /// </summary>
        /// <param name="paramTransferBill">调拨单</param>
        /// <param name="paramTransferBillDetail">调拨单明细</param>
        /// <param name="paramInventory">库存</param>
        /// <param name="paramIsStockout">是否是调拨出库的异动</param>
        /// <returns></returns>
        private MDLPIS_InventoryTransLog GenerateInventoryTransLogOfApprove(MDLPIS_TransferBill paramTransferBill, TransferBillManagerDetailUIModel paramTransferBillDetail, MDLPIS_Inventory paramInventory, bool paramIsStockout = true)
        {
            MDLPIS_InventoryTransLog newInventoryTransLog = new MDLPIS_InventoryTransLog
            {
                //业务单号为[调拨单]的单号
                ITL_BusinessNo = paramTransferBill.TB_No,
                ITL_Barcode = paramInventory.INV_Barcode,
                ITL_BatchNo = paramInventory.INV_BatchNo,
                ITL_Name = paramTransferBillDetail.TBD_Name,
                ITL_Specification = paramTransferBillDetail.TBD_Specification,
                ITL_UnitSalePrice = null,
                ITL_AfterTransQty = paramInventory.INV_Qty,
                ITL_IsValid = true,
                ITL_CreatedBy = LoginInfoDAX.UserName,
                ITL_UpdatedBy = LoginInfoDAX.UserName
            };
            if (paramIsStockout == true)
            {
                //调出组织的[库存异动日志]
                newInventoryTransLog.ITL_Org_ID = paramTransferBill.TB_TransferOutOrgId;
                newInventoryTransLog.ITL_WH_ID = paramTransferBillDetail.TBD_TransOutWhId;
                newInventoryTransLog.ITL_WHB_ID = paramTransferBillDetail.TBD_TransOutBinId;
                //异动数量为负数
                newInventoryTransLog.ITL_Qty = -paramTransferBillDetail.TBD_Qty;
                newInventoryTransLog.ITL_UnitCostPrice = paramTransferBillDetail.TBD_SourUnitPrice;
                newInventoryTransLog.ITL_TransType = InventoryTransTypeEnum.Name.DBCK;
                newInventoryTransLog.ITL_Source = paramTransferBillDetail.TransOutWhName;
                newInventoryTransLog.ITL_Destination = paramTransferBillDetail.TransInWhName;
            }
            else
            {
                //调入组织的[库存异动日志]
                newInventoryTransLog.ITL_Org_ID = paramTransferBill.TB_TransferInOrgId;
                newInventoryTransLog.ITL_WH_ID = paramTransferBillDetail.TBD_TransInWhId;
                //异动数量为正数
                newInventoryTransLog.ITL_Qty = paramTransferBillDetail.TBD_Qty;
                newInventoryTransLog.ITL_WHB_ID = paramTransferBillDetail.TBD_TransInBinId;
                newInventoryTransLog.ITL_UnitCostPrice = paramTransferBillDetail.TBD_DestUnitPrice;
                newInventoryTransLog.ITL_TransType = InventoryTransTypeEnum.Name.DBRK;
                newInventoryTransLog.ITL_Source = paramTransferBillDetail.TransOutWhName;
                newInventoryTransLog.ITL_Destination = paramTransferBillDetail.TransInWhName;
            }

            return newInventoryTransLog;
        }

        /// <summary>
        /// 【反审核】生成库存异动日志
        /// </summary>
        /// <param name="paramTransferBill">调拨单</param>
        /// <param name="paramTransferBillDetail">调拨单明细</param>
        /// <param name="paramInventory">库存</param>
        /// <param name="paramIsStockout">是否是调拨出库的异动</param>
        /// <returns></returns>
        private MDLPIS_InventoryTransLog GenerateInventoryTransLogOfUnApprove(MDLPIS_TransferBill paramTransferBill, TransferBillManagerDetailUIModel paramTransferBillDetail, MDLPIS_Inventory paramInventory, bool paramIsStockout = true)
        {
            MDLPIS_InventoryTransLog newInventoryTransLog = new MDLPIS_InventoryTransLog
            {
                //业务单号为[调拨单]的单号
                ITL_BusinessNo = paramTransferBill.TB_No,
                ITL_Barcode = paramInventory.INV_Barcode,
                ITL_BatchNo = paramInventory.INV_BatchNo,
                ITL_Name = paramTransferBillDetail.TBD_Name,
                ITL_Specification = paramTransferBillDetail.TBD_Specification,
                ITL_UnitSalePrice = null,
                ITL_AfterTransQty = paramInventory.INV_Qty,
                ITL_IsValid = true,
                ITL_CreatedBy = LoginInfoDAX.UserName,
                ITL_UpdatedBy = LoginInfoDAX.UserName
            };
            if (paramIsStockout == true)
            {
                //调出组织的[库存异动日志]
                newInventoryTransLog.ITL_Org_ID = paramTransferBill.TB_TransferOutOrgId;
                newInventoryTransLog.ITL_WH_ID = paramTransferBillDetail.TBD_TransOutWhId;
                newInventoryTransLog.ITL_WHB_ID = paramTransferBillDetail.TBD_TransOutBinId;
                //出库 反审核 实际是入库，异动数量为正数
                newInventoryTransLog.ITL_Qty = paramTransferBillDetail.TBD_Qty;
                newInventoryTransLog.ITL_UnitCostPrice = paramTransferBillDetail.TBD_SourUnitPrice;
                newInventoryTransLog.ITL_TransType = InventoryTransTypeEnum.Name.DBCK;
                newInventoryTransLog.ITL_Source = paramTransferBillDetail.TransOutWhName;
                newInventoryTransLog.ITL_Destination = paramTransferBillDetail.TransInWhName;
            }
            else
            {
                //调入组织的[库存异动日志]
                newInventoryTransLog.ITL_Org_ID = paramTransferBill.TB_TransferInOrgId;
                newInventoryTransLog.ITL_WH_ID = paramTransferBillDetail.TBD_TransInWhId;
                //入库 反审核 实际是出库，异动数量为正负数
                newInventoryTransLog.ITL_Qty = -paramTransferBillDetail.TBD_Qty;
                newInventoryTransLog.ITL_WHB_ID = paramTransferBillDetail.TBD_TransInBinId;
                newInventoryTransLog.ITL_UnitCostPrice = paramTransferBillDetail.TBD_DestUnitPrice;
                newInventoryTransLog.ITL_TransType = InventoryTransTypeEnum.Name.DBRK;
                newInventoryTransLog.ITL_Source = paramTransferBillDetail.TransOutWhName;
                newInventoryTransLog.ITL_Destination = paramTransferBillDetail.TransInWhName;
            }

            return newInventoryTransLog;
        }

        #endregion
    }
}
