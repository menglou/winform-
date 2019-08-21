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
using SkyCar.Coeus.UIModel.SD;
using SkyCar.Coeus.UIModel.SD.APModel;

namespace SkyCar.Coeus.BLL.SD
{
    /// <summary>
    /// 销售退货管理BLL
    /// </summary>
    public class SalesReturnOrderManagerBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.SD);
        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 销售退货管理BLL
        /// </summary>
        public SalesReturnOrderManagerBLL() : base(Trans.SD)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramHead">UIModel</param>
        /// <param name="paramDetailList">销售订单明细列表</param>
        /// <returns></returns>
        public bool SaveDetailDs(SalesReturnOrderManagerUIModel paramHead, SkyCarBindingList<SalesOrderDetailUIModel, MDLSD_SalesOrderDetail> paramDetailList)
        {
            var funcName = "SaveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);
            //服务端检查
            if (!ServerCheck(paramHead))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            #region 准备数据

            //待更新的[配件档案]列表
            List<MDLBS_AutoPartsArchive> updateAutoPartsArchiveList = new List<MDLBS_AutoPartsArchive>();

            #region 单头
            //将UIModel转为TBModel
            var argsHead = paramHead.ToTBModelForSaveAndDelete<MDLSD_SalesOrder>();
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(paramHead.SO_ID))
            {
                argsHead.SO_ID = Guid.NewGuid().ToString();
                //单号
                argsHead.SO_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.SO);
                argsHead.SO_CreatedBy = LoginInfoDAX.UserName;
                argsHead.SO_CreatedTime = BLLCom.GetCurStdDatetime();
            }
            argsHead.SO_UpdatedBy = LoginInfoDAX.UserName;
            argsHead.SO_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region 明细

            //添加的明细
            if (paramDetailList != null && paramDetailList.InsertList != null &&
                paramDetailList.InsertList.Count > 0)
            {
                foreach (var loopSalesOrderDetail in paramDetailList.InsertList)
                {
                    //赋值销售订单ID
                    loopSalesOrderDetail.SOD_SO_ID = argsHead.SO_ID ?? argsHead.WHERE_SO_ID;

                    //如果[计价基准可改]为true
                    if (loopSalesOrderDetail.SOD_SalePriceRateIsChangeable ?? false)
                    {
                        //回写配件档案[条码和名称唯一]
                        //待更新的配件档案
                        MDLBS_AutoPartsArchive updateAutoPartsArchive = new MDLBS_AutoPartsArchive();
                        _bll.QueryForObject<MDLBS_AutoPartsArchive, MDLBS_AutoPartsArchive>(new MDLBS_AutoPartsArchive
                        {
                            WHERE_APA_IsValid = true,
                            WHERE_APA_Barcode = loopSalesOrderDetail.SOD_Barcode,
                            WHERE_APA_Name = loopSalesOrderDetail.SOD_Name
                        }, updateAutoPartsArchive);
                        if (!string.IsNullOrEmpty(updateAutoPartsArchive.APA_ID))
                        {
                            updateAutoPartsArchive.WHERE_APA_ID = updateAutoPartsArchive.APA_ID;
                            updateAutoPartsArchive.WHERE_APA_VersionNo = updateAutoPartsArchive.APA_VersionNo;
                            updateAutoPartsArchive.APA_VersionNo++;
                            updateAutoPartsArchive.APA_SalePriceRate = loopSalesOrderDetail.SOD_SalePriceRate;
                            updateAutoPartsArchive.APA_UpdatedBy = LoginInfoDAX.UserName;
                            updateAutoPartsArchive.APA_UpdatedTime = BLLCom.GetCurStdDatetime();

                            updateAutoPartsArchiveList.Add(updateAutoPartsArchive);
                        }
                    }
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
                if (!_bll.Save(argsHead, argsHead.SO_ID))
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_SalesOrder });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存明细

                if (paramDetailList != null)
                {
                    //执行保存
                    bool saveDetailResult = _bll.UnitySave(paramDetailList);
                    if (!saveDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_SalesOrderDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #endregion

                #region 更新[配件档案]列表

                foreach (var loopAutoPartsArchive in updateAutoPartsArchiveList)
                {
                    bool updateAutoPartsArchiveResult = _bll.Update(loopAutoPartsArchive);
                    if (!updateAutoPartsArchiveResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.BS_AutoPartsArchive });
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
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ex.Message, "", null);
                return false;
            }

            #endregion

            //将最新数据回写给DetailDS
            _bll.CopyModel(argsHead, paramHead);

            //更新明细版本号
            if (paramDetailList != null)
            {
                if (paramDetailList.InsertList != null)
                {
                    foreach (var loopInsertDetail in paramDetailList.InsertList)
                    {
                        //新增时版本号为1
                        loopInsertDetail.SOD_VersionNo = 1;
                    }
                }

                foreach (var loopUpdateDetail in paramDetailList.UpdateList)
                {
                    //更新时版本号加1
                    loopUpdateDetail.SOD_VersionNo = loopUpdateDetail.SOD_VersionNo + 1;
                }
            }

            return true;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="paramHead">销售订单</param>
        /// <param name="paramDetailList">销售订单明细列表</param>
        /// <param name="paramStockOutDetailList">出库明细列表</param>
        /// <param name="paramReturnStockInDetailList">退货入库明细列表</param>
        /// <param name="paramIsHasInventory">是否启用进销存模块</param>
        /// <returns></returns>
        public bool ApproveDetailDS(SalesReturnOrderManagerUIModel paramHead, SkyCarBindingList<SalesOrderDetailUIModel, MDLSD_SalesOrderDetail> paramDetailList, List<SalesStockOutDetailUIModel> paramStockOutDetailList, List<ReturnStockInDetailUIModel> paramReturnStockInDetailList, bool paramIsHasInventory)
        {
            var funcName = "ApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证
            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.SO_ID)
                || string.IsNullOrEmpty(paramHead.SO_No))
            {
                //没有获取到销售订单，审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.SD_SalesOrder, SystemActionEnum.Name.APPROVE });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }
            #endregion

            #region 准备数据

            #region 变量定义

            //待更新的[销售订单]
            MDLSD_SalesOrder updateSalesOrder = paramHead.ToTBModelForSaveAndDelete<MDLSD_SalesOrder>();

            //待新增的退货时的[应收单]
            MDLFM_AccountReceivableBill newRejectAccountReceivableBill = new MDLFM_AccountReceivableBill();
            //待新增的退货时的[应收单明细]列表
            List<MDLFM_AccountReceivableBillDetail> newRejectReceivableBillDetailList = new List<MDLFM_AccountReceivableBillDetail>();
            //待新增的退货入库[入库单]
            MDLPIS_StockInBill newReturnStockInBill = new MDLPIS_StockInBill();
            //待新增的退货入库[入库单明细]列表
            List<MDLPIS_StockInDetail> newReturnStockInDetailList = new List<MDLPIS_StockInDetail>();
            //待更新的[库存]列表
            List<MDLPIS_Inventory> updateStockInInventoryList = new List<MDLPIS_Inventory>();
            //待新增的[库存异动日志]列表
            List<MDLPIS_InventoryTransLog> newStockInInventoryTransLogList = new List<MDLPIS_InventoryTransLog>();

            #endregion

            #region 更新销售订单

            //更新[销售订单]审核状态为[已审核]，单据状态根据[来源类型]决定
            updateSalesOrder.SO_VersionNo++;
            updateSalesOrder.SO_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            updateSalesOrder.SO_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            updateSalesOrder.SO_UpdatedBy = LoginInfoDAX.UserName;
            updateSalesOrder.SO_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region [销售订单].[来源类型]为{手工创建退货}、{在线销售退货}、{主动销售退货}的场合

            //销售退货的场合，更新[销售订单].[单据状态]为[交易成功]
            updateSalesOrder.SO_StatusCode = SalesOrderStatusEnum.Code.JYCG;
            updateSalesOrder.SO_StatusName = SalesOrderStatusEnum.Name.JYCG;

            #region 创建退货入库的[入库单]

            //新增[来源类型]为[销售退货]，[业务状态]为{已完成}，[审核状态]为{已审核}的[入库单]
            newReturnStockInBill.SIB_ID = Guid.NewGuid().ToString();
            newReturnStockInBill.SIB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.SIB);
            newReturnStockInBill.SIB_SourceTypeCode = StockInBillSourceTypeEnum.Code.SSTH;
            newReturnStockInBill.SIB_SourceTypeName = StockInBillSourceTypeEnum.Name.SSTH;
            newReturnStockInBill.SIB_SourceNo = updateSalesOrder.SO_No;
            newReturnStockInBill.SIB_Org_ID = updateSalesOrder.SO_Org_ID;
            newReturnStockInBill.SIB_StatusName = StockInBillStatusEnum.Name.YWC;
            newReturnStockInBill.SIB_StatusCode = StockInBillStatusEnum.Code.YWC;
            newReturnStockInBill.SIB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            newReturnStockInBill.SIB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            newReturnStockInBill.SIB_IsValid = true;
            newReturnStockInBill.SIB_CreatedBy = LoginInfoDAX.UserName;
            newReturnStockInBill.SIB_CreatedTime = BLLCom.GetCurStdDatetime();
            newReturnStockInBill.SIB_UpdatedBy = LoginInfoDAX.UserName;
            newReturnStockInBill.SIB_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            #region 创建负向的[应收单]

            //新增[来源类型]为[销售应收]，[单据方向]为[负向]，金额为负，[业务状态]为{执行中}，[审核状态]为{已审核}的[应收单]
            newRejectAccountReceivableBill.ARB_ID = Guid.NewGuid().ToString();
            newRejectAccountReceivableBill.ARB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.ARB);
            newRejectAccountReceivableBill.ARB_BillDirectCode = BillDirectionEnum.Code.MINUS;
            newRejectAccountReceivableBill.ARB_BillDirectName = BillDirectionEnum.Name.MINUS;
            newRejectAccountReceivableBill.ARB_SourceTypeCode = AccountReceivableBillSourceTypeEnum.Code.XSYS;
            newRejectAccountReceivableBill.ARB_SourceTypeName = AccountReceivableBillSourceTypeEnum.Name.XSYS;
            newRejectAccountReceivableBill.ARB_SrcBillNo = updateSalesOrder.SO_No;
            newRejectAccountReceivableBill.ARB_Org_ID = LoginInfoDAX.OrgID;
            newRejectAccountReceivableBill.ARB_Org_Name = LoginInfoDAX.OrgShortName;
            if (paramHead.SO_CustomerTypeName == CustomerTypeEnum.Name.PTNQXSH)
            {
                newRejectAccountReceivableBill.ARB_PayObjectTypeName = AmountTransObjectTypeEnum.Name.PLATFORMAUTOFACTORY;
                newRejectAccountReceivableBill.ARB_PayObjectTypeCode = AmountTransObjectTypeEnum.Code.PLATFORMAUTOFACTORY;
            }
            else if (paramHead.SO_CustomerTypeName == CustomerTypeEnum.Name.YBQXSH)
            {
                newRejectAccountReceivableBill.ARB_PayObjectTypeName = AmountTransObjectTypeEnum.Name.GENERALAUTOFACTORY;
                newRejectAccountReceivableBill.ARB_PayObjectTypeCode = AmountTransObjectTypeEnum.Code.GENERALAUTOFACTORY;
            }
            else
            {
                newRejectAccountReceivableBill.ARB_PayObjectTypeName = AmountTransObjectTypeEnum.Name.REGULARCUSTOMER;
                newRejectAccountReceivableBill.ARB_PayObjectTypeCode = AmountTransObjectTypeEnum.Code.REGULARCUSTOMER;
            }
            newRejectAccountReceivableBill.ARB_PayObjectName = paramHead.SO_CustomerName;
            newRejectAccountReceivableBill.ARB_PayObjectID = paramHead.SO_CustomerID;
            newRejectAccountReceivableBill.ARB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.ZXZ;
            newRejectAccountReceivableBill.ARB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.ZXZ;
            newRejectAccountReceivableBill.ARB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            newRejectAccountReceivableBill.ARB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            newRejectAccountReceivableBill.ARB_IsValid = true;
            newRejectAccountReceivableBill.ARB_CreatedBy = LoginInfoDAX.UserName;
            newRejectAccountReceivableBill.ARB_CreatedTime = BLLCom.GetCurStdDatetime();
            newRejectAccountReceivableBill.ARB_UpdatedBy = LoginInfoDAX.UserName;
            newRejectAccountReceivableBill.ARB_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            //退货产生的应收单.[应收金额]
            decimal returnReceivableAmount = 0;

            foreach (var loopSalesOrderReturnDetail in paramDetailList)
            {
                #region 退货的[销售订单明细]

                //[销售订单明细]审核状态 和 单据状态与单头一致
                loopSalesOrderReturnDetail.WHERE_SOD_ID = loopSalesOrderReturnDetail.SOD_ID;
                loopSalesOrderReturnDetail.WHERE_SOD_VersionNo = loopSalesOrderReturnDetail.SOD_VersionNo;
                loopSalesOrderReturnDetail.SOD_VersionNo++;
                loopSalesOrderReturnDetail.SOD_ApprovalStatusCode = updateSalesOrder.SO_ApprovalStatusCode;
                loopSalesOrderReturnDetail.SOD_ApprovalStatusName = updateSalesOrder.SO_ApprovalStatusName;
                loopSalesOrderReturnDetail.SOD_StatusCode = updateSalesOrder.SO_StatusCode;
                loopSalesOrderReturnDetail.SOD_StatusName = updateSalesOrder.SO_StatusName;
                loopSalesOrderReturnDetail.SOD_UpdatedBy = LoginInfoDAX.UserName;
                loopSalesOrderReturnDetail.SOD_UpdatedTime = BLLCom.GetCurStdDatetime();
                #endregion

                #region 退货的[应收单明细]
                //新增[来源类型]为[销售应收]，[是否负向明细]为[true]，金额为负的[应收单明细]
                MDLFM_AccountReceivableBillDetail newAccountReceivableBillDetail = new MDLFM_AccountReceivableBillDetail
                {
                    ARBD_ID = Guid.NewGuid().ToString(),
                    ARBD_ARB_ID = newRejectAccountReceivableBill.ARB_ID,
                    ARBD_IsMinusDetail = true,
                    ARBD_SrcBillNo = updateSalesOrder.SO_No,
                    ARBD_SrcBillDetailID = loopSalesOrderReturnDetail.SOD_ID,
                    ARBD_Org_ID = newRejectAccountReceivableBill.ARB_Org_ID,
                    ARBD_Org_Name = newRejectAccountReceivableBill.ARB_Org_Name,
                    //应收金额 = 销售数量（退货数量） * 单价
                    ARBD_AccountReceivableAmount = -Math.Round((loopSalesOrderReturnDetail.SOD_Qty ?? 0) * (loopSalesOrderReturnDetail.SOD_UnitPrice ?? 0), 2),
                    ARBD_ReceivedAmount = 0,
                    ARBD_UnReceiveAmount = -Math.Round((loopSalesOrderReturnDetail.SOD_Qty ?? 0) * (loopSalesOrderReturnDetail.SOD_UnitPrice ?? 0), 2),
                    ARBD_BusinessStatusCode = newRejectAccountReceivableBill.ARB_BusinessStatusCode,
                    ARBD_BusinessStatusName = newRejectAccountReceivableBill.ARB_BusinessStatusName,
                    ARBD_ApprovalStatusCode = newRejectAccountReceivableBill.ARB_ApprovalStatusCode,
                    ARBD_ApprovalStatusName = newRejectAccountReceivableBill.ARB_ApprovalStatusName,
                    ARBD_IsValid = true,
                    ARBD_CreatedBy = LoginInfoDAX.UserName,
                    ARBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                    ARBD_UpdatedBy = LoginInfoDAX.UserName,
                    ARBD_UpdatedTime = BLLCom.GetCurStdDatetime()
                };
                returnReceivableAmount += (newAccountReceivableBillDetail.ARBD_AccountReceivableAmount ?? 0);
                newRejectReceivableBillDetailList.Add(newAccountReceivableBillDetail);
               
                #endregion
            }
            //[应收单].[应收金额]
            newRejectAccountReceivableBill.ARB_AccountReceivableAmount = returnReceivableAmount;
            newRejectAccountReceivableBill.ARB_ReceivedAmount = 0;
            newRejectAccountReceivableBill.ARB_UnReceiveAmount = returnReceivableAmount;
            //[销售单].[总金额]
            updateSalesOrder.SO_TotalAmount = returnReceivableAmount;

            foreach (var loopReturnStockInDetail in paramReturnStockInDetailList)
            {
                #region 退货入库的[入库单明细]

                MDLPIS_StockInDetail newStockInDetail = new MDLPIS_StockInDetail();
                _bll.CopyModel(loopReturnStockInDetail, newStockInDetail);

                newStockInDetail.SID_ID = Guid.NewGuid().ToString();
                newStockInDetail.SID_SIB_ID = newReturnStockInBill.SIB_ID;
                newStockInDetail.SID_SIB_No = newReturnStockInBill.SIB_No;
                newStockInDetail.SID_IsValid = true;
                newStockInDetail.SID_CreatedBy = LoginInfoDAX.UserName;
                newStockInDetail.SID_CreatedTime = BLLCom.GetCurStdDatetime();
                newStockInDetail.SID_UpdatedBy = LoginInfoDAX.UserName;
                newStockInDetail.SID_UpdatedTime = BLLCom.GetCurStdDatetime();

                var curInDetailOfSalesDetail =
                    paramDetailList.FirstOrDefault(x => x.SOD_Barcode == newStockInDetail.SID_Barcode);
                if (curInDetailOfSalesDetail != null)
                {
                    //[入库单明细].[来源单据明细ID] 为 [销售退货单明细].[ID]
                    newStockInDetail.SID_SourceDetailID = curInDetailOfSalesDetail.SOD_ID;
                }

                newReturnStockInDetailList.Add(newStockInDetail);
                #endregion

                if (paramIsHasInventory)
                {
                    #region 有进销存 并且 审核销售退货订单的场合，创建相关单据为已审核的场合，更新[库存]，创建[库存异动日志]

                    //在[入库单明细]列表中第一次出现的配件[库存]信息
                    MDLPIS_Inventory inventoryExists = null;

                    foreach (var loopInventory in updateStockInInventoryList)
                    {
                        if (loopInventory.INV_Barcode == loopReturnStockInDetail.SID_Barcode
                            && loopInventory.INV_BatchNo == loopReturnStockInDetail.SID_BatchNo)
                        {
                            inventoryExists = loopInventory;
                            break;
                        }
                    }
                    if (inventoryExists != null)
                    {
                        //[入库单明细]列表中已遍历过该配件，累加数量
                        inventoryExists.INV_Qty += loopReturnStockInDetail.SID_Qty;
                        inventoryExists.INV_UpdatedBy = LoginInfoDAX.UserName;
                        inventoryExists.INV_UpdatedTime = BLLCom.GetCurStdDatetime();
                        //生成[库存异动日志]
                        newStockInInventoryTransLogList.Add(GenerateStockInInventoryTransLog(newReturnStockInBill, loopReturnStockInDetail, inventoryExists, paramHead));
                    }
                    else
                    {
                        //[入库单明细]列表中第一次出现该配件
                        //查询该配件是否在[库存]中存在
                        MDLPIS_Inventory resultInventory = new MDLPIS_Inventory();
                        _bll.QueryForObject<MDLPIS_Inventory, MDLPIS_Inventory>(new MDLPIS_Inventory
                        {
                            WHERE_INV_Org_ID = newReturnStockInBill.SIB_Org_ID,
                            WHERE_INV_Barcode = loopReturnStockInDetail.SID_Barcode,
                            WHERE_INV_BatchNo = loopReturnStockInDetail.SID_BatchNo,
                            WHERE_INV_WH_ID = loopReturnStockInDetail.SID_WH_ID,
                            WHERE_INV_IsValid = true
                        }, resultInventory);

                        //[库存]中不存在该配件
                        if (string.IsNullOrEmpty(resultInventory.INV_ID))
                        {
                            //新增一个该配件的库存信息
                            MDLPIS_Inventory inventoryToInsert = new MDLPIS_Inventory
                            {
                                INV_Org_ID = newReturnStockInBill.SIB_Org_ID,
                                INV_SUPP_ID = loopReturnStockInDetail.SID_SUPP_ID,
                                INV_WH_ID = loopReturnStockInDetail.SID_WH_ID,
                                INV_WHB_ID = loopReturnStockInDetail.SID_WHB_ID,
                                INV_ThirdNo = loopReturnStockInDetail.SID_ThirdNo,
                                INV_OEMNo = loopReturnStockInDetail.SID_OEMNo,
                                INV_Barcode = loopReturnStockInDetail.SID_Barcode,
                                INV_BatchNo = loopReturnStockInDetail.SID_BatchNo,
                                INV_Name = loopReturnStockInDetail.SID_Name,
                                INV_Specification = loopReturnStockInDetail.SID_Specification,
                                INV_Qty = loopReturnStockInDetail.SID_Qty,
                                INV_PurchaseUnitPrice = loopReturnStockInDetail.SID_UnitCostPrice,
                                INV_IsValid = true,
                                INV_CreatedBy = LoginInfoDAX.UserName,
                                INV_CreatedTime = BLLCom.GetCurStdDatetime(),
                                INV_UpdatedBy = LoginInfoDAX.UserName,
                                INV_UpdatedTime = BLLCom.GetCurStdDatetime()
                            };
                            updateStockInInventoryList.Add(inventoryToInsert);

                            //生成[库存异动日志]
                            newStockInInventoryTransLogList.Add(GenerateStockInInventoryTransLog(newReturnStockInBill, loopReturnStockInDetail, inventoryToInsert, paramHead));
                        }
                        //[库存]中存在该配件
                        else
                        {
                            //更新[库存]中该配件的数量
                            resultInventory.INV_Qty += loopReturnStockInDetail.SID_Qty;
                            resultInventory.INV_UpdatedBy = LoginInfoDAX.UserName;
                            resultInventory.INV_UpdatedTime = BLLCom.GetCurStdDatetime();
                            resultInventory.WHERE_INV_ID = resultInventory.INV_ID;
                            resultInventory.WHERE_INV_VersionNo = resultInventory.INV_VersionNo;
                            updateStockInInventoryList.Add(resultInventory);

                            //生成[库存异动日志]
                            newStockInInventoryTransLogList.Add(GenerateStockInInventoryTransLog(newReturnStockInBill, loopReturnStockInDetail, resultInventory, paramHead));
                        }
                    }

                    #endregion
                }
            }
            #endregion

            #endregion

            #region 带事务的新增和更新

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 更新[销售订单]

                bool saveSalesOrder = _bll.Save(updateSalesOrder);
                if (!saveSalesOrder)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.SD_SalesOrder });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 保存[销售订单明细]

                bool saveSalesOrderDetailResult = _bll.UnitySave(paramDetailList);
                if (!saveSalesOrderDetailResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_SalesOrderDetail });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 新增[应收单]
                if (!string.IsNullOrEmpty(newRejectAccountReceivableBill.ARB_ID))
                {
                    //销售退货时产生的负向的[销售应收]
                    bool insertAccountReceivableBillResult = _bll.Insert(newRejectAccountReceivableBill);
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

                //销售退货时产生的负向的[销售应收]
                if (newRejectReceivableBillDetailList.Count > 0)
                {
                    bool insertAccountReceivableBillDetailResult = _bll.InsertByList<MDLFM_AccountReceivableBillDetail, MDLFM_AccountReceivableBillDetail>(newRejectReceivableBillDetailList);
                    if (!insertAccountReceivableBillDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.FM_AccountReceivableBillDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 保存[库存]
                foreach (var loopInventory in updateStockInInventoryList)
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
                if (newStockInInventoryTransLogList.Count > 0)
                {
                    bool insertInventoryTransLogResult = _bll.InsertByList<MDLPIS_InventoryTransLog, MDLPIS_InventoryTransLog>(newStockInInventoryTransLogList);
                    if (!insertInventoryTransLogResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.PIS_InventoryTransLog });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 销售退货入库时生成[入库单]

                if (!string.IsNullOrEmpty(newReturnStockInBill.SIB_ID))
                {
                    bool insertStockInBillResult = _bll.Insert(newReturnStockInBill);
                    if (!insertStockInBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.PIS_StockInBill });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 销售退货入库时生成[入库单明细]

                if (newReturnStockInDetailList.Count > 0)
                {
                    bool insertStockInDetailResult = _bll.InsertByList<MDLPIS_StockInDetail, MDLPIS_StockInDetail>(newReturnStockInDetailList);
                    if (!insertStockInDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.PIS_StockInDetail });
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
            _bll.CopyModel(updateSalesOrder, paramHead);

            return true;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(SalesReturnOrderManagerUIModel paramModel)
        {
            return true;
        }

        /// <summary>
        /// 【审核-退货】生成销售退货入库的库存异动日志
        /// </summary>
        /// <param name="paramStockInBill">入库单</param>
        /// <param name="paramReturnStockInDetail">退货入库明细</param>
        /// <param name="paramInventory">库存</param>
        /// <param name="paramSalesReturnOrderManager">销售退货</param>
        /// <returns></returns>
        private MDLPIS_InventoryTransLog GenerateStockInInventoryTransLog(MDLPIS_StockInBill paramStockInBill, ReturnStockInDetailUIModel paramReturnStockInDetail, MDLPIS_Inventory paramInventory, SalesReturnOrderManagerUIModel paramSalesReturnOrderManager)
        {
            MDLPIS_InventoryTransLog newInventoryTransLog = new MDLPIS_InventoryTransLog
            {
                ITL_Org_ID = string.IsNullOrEmpty(paramStockInBill.SIB_Org_ID) ? LoginInfoDAX.OrgID : paramStockInBill.SIB_Org_ID,
                ITL_WH_ID = paramInventory.INV_WH_ID,
                ITL_WHB_ID = paramReturnStockInDetail.SID_WHB_ID,
                //业务单号为[入库单]的单号
                ITL_BusinessNo = paramStockInBill.SIB_No,
                ITL_Barcode = paramInventory.INV_Barcode,
                ITL_BatchNo = paramInventory.INV_BatchNo,
                ITL_Name = paramReturnStockInDetail.SID_Name,
                ITL_Specification = paramReturnStockInDetail.SID_Specification,
                ITL_UnitCostPrice = paramInventory.INV_PurchaseUnitPrice,
                ITL_UnitSalePrice = null,
                //入库，数量为正
                ITL_Qty = paramReturnStockInDetail.SID_Qty,
                ITL_AfterTransQty = paramInventory.INV_Qty,
                ITL_IsValid = true,
                ITL_CreatedBy = LoginInfoDAX.UserName,
                ITL_UpdatedBy = LoginInfoDAX.UserName,
                //异动类型为{销售退货}
                ITL_TransType = InventoryTransTypeEnum.Name.XSTH,
                ITL_Source = paramSalesReturnOrderManager.AROrgName,
                ITL_Destination = paramReturnStockInDetail.WH_Name,
            };
            return newInventoryTransLog;
        }

        #endregion
    }
}
