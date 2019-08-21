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
    /// 销售订单管理BLL
    /// </summary>
    public class SalesOrderManagerBLL : BLLBase
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
        /// 销售订单管理BLL
        /// </summary>
        public SalesOrderManagerBLL() : base(Trans.SD)
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
        public bool SaveDetailDs(SalesOrderManagerUIModel paramHead, SkyCarBindingList<SalesOrderDetailUIModel, MDLSD_SalesOrderDetail> paramDetailList)
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
        /// <param name="paramIsHasInventory">是否启用进销存模块</param>
        /// <returns></returns>
        public bool ApproveDetailDS(SalesOrderManagerUIModel paramHead, SkyCarBindingList<SalesOrderDetailUIModel, MDLSD_SalesOrderDetail> paramDetailList, List<SalesStockOutDetailUIModel> paramStockOutDetailList, bool paramIsHasInventory)
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

            //手工创建、在线销售相关单据
            //待新增的销售[应收单]
            MDLFM_AccountReceivableBill newSalesAccountReceivableBill = new MDLFM_AccountReceivableBill();
            //待新增的销售[应收单明细]列表
            List<MDLFM_AccountReceivableBillDetail> newSalesAccountReceivableBillDetailList = new List<MDLFM_AccountReceivableBillDetail>();
            //待新增的销售[出库单]
            MDLPIS_StockOutBill newSalesStockOutBill = new MDLPIS_StockOutBill();
            //待新增的销售[出库单明细]列表
            List<MDLPIS_StockOutBillDetail> newSalesStockOutBillDetailList = new List<MDLPIS_StockOutBillDetail>();
            //待更新的销售出库[库存]列表
            List<MDLPIS_Inventory> updateStockOutInventoryList = new List<MDLPIS_Inventory>();
            //待新增的销售出库[库存异动日志]列表
            List<MDLPIS_InventoryTransLog> newStockOutInventoryTransLogList = new List<MDLPIS_InventoryTransLog>();

            #endregion

            #region 更新销售订单

            //更新[销售订单]审核状态为[已审核]，单据状态根据[来源类型]决定
            updateSalesOrder.SO_VersionNo++;
            updateSalesOrder.SO_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            updateSalesOrder.SO_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            updateSalesOrder.SO_UpdatedBy = LoginInfoDAX.UserName;
            updateSalesOrder.SO_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region [销售订单].[来源类型]为{手工创建}、{在线销售}的场合

            //不是销售退货的场合，更新[销售订单].[单据状态]为[待发货]
            updateSalesOrder.SO_StatusCode = SalesOrderStatusEnum.Code.DFH;
            updateSalesOrder.SO_StatusName = SalesOrderStatusEnum.Name.DFH;

            #region 创建[出库单]

            newSalesStockOutBill.SOB_ID = Guid.NewGuid().ToString();
            newSalesStockOutBill.SOB_Org_ID = LoginInfoDAX.OrgID;
            newSalesStockOutBill.SOB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.SOB);
            newSalesStockOutBill.SOB_SourceTypeCode = StockOutBillSourceTypeEnum.Code.XSCK;
            newSalesStockOutBill.SOB_SourceTypeName = StockOutBillSourceTypeEnum.Name.XSCK;
            newSalesStockOutBill.SOB_SourceNo = paramHead.SO_No;
            newSalesStockOutBill.SOB_StatusCode = StockOutBillStatusEnum.Code.YWC;
            newSalesStockOutBill.SOB_StatusName = StockOutBillStatusEnum.Name.YWC;
            newSalesStockOutBill.SOB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            newSalesStockOutBill.SOB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            newSalesStockOutBill.SOB_IsValid = true;
            newSalesStockOutBill.SOB_CreatedBy = LoginInfoDAX.UserName;
            newSalesStockOutBill.SOB_CreatedTime = BLLCom.GetCurStdDatetime();
            newSalesStockOutBill.SOB_UpdatedBy = LoginInfoDAX.UserName;
            newSalesStockOutBill.SOB_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            #region 创建[应收单]

            //新增[来源类型]为[销售应收]，[业务状态]为[已生成]，[审核状态]为[已审核]的[应收单]，[单据方向]根据[销售订单].[来源类型]区分
            newSalesAccountReceivableBill.ARB_ID = Guid.NewGuid().ToString();
            newSalesAccountReceivableBill.ARB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.ARB);
            newSalesAccountReceivableBill.ARB_SourceTypeCode = AccountReceivableBillSourceTypeEnum.Code.XSYS;
            newSalesAccountReceivableBill.ARB_SourceTypeName = AccountReceivableBillSourceTypeEnum.Name.XSYS;
            newSalesAccountReceivableBill.ARB_SrcBillNo = paramHead.SO_No;
            newSalesAccountReceivableBill.ARB_Org_ID = LoginInfoDAX.OrgID;
            newSalesAccountReceivableBill.ARB_Org_Name = LoginInfoDAX.OrgShortName;
            if (paramHead.SO_CustomerTypeName == CustomerTypeEnum.Name.PTNQXSH)
            {
                newSalesAccountReceivableBill.ARB_PayObjectTypeName = AmountTransObjectTypeEnum.Name.PLATFORMAUTOFACTORY;
                newSalesAccountReceivableBill.ARB_PayObjectTypeCode = AmountTransObjectTypeEnum.Code.PLATFORMAUTOFACTORY;
            }
            else if (paramHead.SO_CustomerTypeName == CustomerTypeEnum.Name.YBQXSH)
            {
                newSalesAccountReceivableBill.ARB_PayObjectTypeName = AmountTransObjectTypeEnum.Name.GENERALAUTOFACTORY;
                newSalesAccountReceivableBill.ARB_PayObjectTypeCode = AmountTransObjectTypeEnum.Code.GENERALAUTOFACTORY;
            }
            else
            {
                newSalesAccountReceivableBill.ARB_PayObjectTypeName = AmountTransObjectTypeEnum.Name.REGULARCUSTOMER;
                newSalesAccountReceivableBill.ARB_PayObjectTypeCode = AmountTransObjectTypeEnum.Code.REGULARCUSTOMER;
            }
            newSalesAccountReceivableBill.ARB_PayObjectName = paramHead.SO_CustomerName;
            newSalesAccountReceivableBill.ARB_PayObjectID = paramHead.SO_CustomerID;
            newSalesAccountReceivableBill.ARB_ReceivedAmount = 0;
            newSalesAccountReceivableBill.ARB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.YSC;
            newSalesAccountReceivableBill.ARB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.YSC;
            newSalesAccountReceivableBill.ARB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            newSalesAccountReceivableBill.ARB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            newSalesAccountReceivableBill.ARB_IsValid = true;
            newSalesAccountReceivableBill.ARB_CreatedBy = LoginInfoDAX.UserName;
            newSalesAccountReceivableBill.ARB_CreatedTime = BLLCom.GetCurStdDatetime();
            newSalesAccountReceivableBill.ARB_UpdatedBy = LoginInfoDAX.UserName;
            newSalesAccountReceivableBill.ARB_UpdatedTime = BLLCom.GetCurStdDatetime();

            if (updateSalesOrder.SO_SourceTypeName == SalesOrderSourceTypeEnum.Name.SGCJ
                || updateSalesOrder.SO_SourceTypeName == SalesOrderSourceTypeEnum.Name.ZDXS
                || updateSalesOrder.SO_SourceTypeName == SalesOrderSourceTypeEnum.Name.ZXXS
                || updateSalesOrder.SO_SourceTypeName == SalesOrderSourceTypeEnum.Name.XSYC)
            {
                //销售订单的[来源类型]为[手工创建]、[主动销售]、[在线销售]、[销售预测]的场合，应收单的[单据方向]为[正向]，金额为正
                newSalesAccountReceivableBill.ARB_BillDirectCode = BillDirectionEnum.Code.PLUS;
                newSalesAccountReceivableBill.ARB_BillDirectName = BillDirectionEnum.Name.PLUS;
                newSalesAccountReceivableBill.ARB_AccountReceivableAmount = paramHead.SO_TotalAmount;
                newSalesAccountReceivableBill.ARB_UnReceiveAmount = paramHead.SO_TotalAmount;
            }
            else
            {
                //销售订单的[来源类型]为[手工创建退货]、[在线销售退货]、[主动销售退货]的场合，应收单的[单据方向]为[负向]，金额为负
                newSalesAccountReceivableBill.ARB_BillDirectCode = BillDirectionEnum.Code.MINUS;
                newSalesAccountReceivableBill.ARB_BillDirectName = BillDirectionEnum.Name.MINUS;
                newSalesAccountReceivableBill.ARB_AccountReceivableAmount = -paramHead.SO_TotalAmount;
                newSalesAccountReceivableBill.ARB_UnReceiveAmount = -paramHead.SO_TotalAmount;
            }
            #endregion

            #region 遍历[销售订单明细]列表，更新[销售订单明细]，创建[应收单明细]

            foreach (var loopSalesOrderDetail in paramDetailList)
            {
                if (string.IsNullOrEmpty(loopSalesOrderDetail.SOD_Barcode))
                {
                    continue;
                }

                #region 更新[销售订单明细]

                //[销售订单明细]审核状态 和 单据状态与单头一致
                loopSalesOrderDetail.WHERE_SOD_ID = loopSalesOrderDetail.SOD_ID;
                loopSalesOrderDetail.WHERE_SOD_VersionNo = loopSalesOrderDetail.SOD_VersionNo;
                loopSalesOrderDetail.SOD_SO_ID = updateSalesOrder.SO_ID ?? updateSalesOrder.WHERE_SO_ID;
                loopSalesOrderDetail.SOD_VersionNo++;
                loopSalesOrderDetail.SOD_ApprovalStatusCode = updateSalesOrder.SO_ApprovalStatusCode;
                loopSalesOrderDetail.SOD_ApprovalStatusName = updateSalesOrder.SO_ApprovalStatusName;
                loopSalesOrderDetail.SOD_StatusCode = updateSalesOrder.SO_StatusCode;
                loopSalesOrderDetail.SOD_StatusName = updateSalesOrder.SO_StatusName;
                loopSalesOrderDetail.SOD_UpdatedBy = LoginInfoDAX.UserName;
                loopSalesOrderDetail.SOD_UpdatedTime = BLLCom.GetCurStdDatetime();
                #endregion

                if (paramIsHasInventory)
                {
                    #region [有进销存]的场合，遍历[出库明细]列表，创建[出库单明细]，更新[库存]，创建[库存异动日志]

                    foreach (var loopStockOutDetail in paramStockOutDetailList)
                    {
                        if (loopStockOutDetail.INV_Barcode != loopSalesOrderDetail.SOD_Barcode)
                        {
                            continue;
                        }

                        #region 创建[出库单明细]

                        MDLPIS_StockOutBillDetail newStockOutBillDetail = new MDLPIS_StockOutBillDetail
                        {
                            SOBD_ID = Guid.NewGuid().ToString(),
                            SOBD_SOB_ID = newSalesStockOutBill.SOB_ID,
                            SOBD_SOB_No = newSalesStockOutBill.SOB_No,
                            SOBD_SourceDetailID = loopSalesOrderDetail.SOD_ID,
                            SOBD_Barcode = loopSalesOrderDetail.SOD_Barcode,
                            //配件批次号
                            SOBD_BatchNo = loopStockOutDetail.INV_BatchNo,
                            SOBD_ThirdNo = loopStockOutDetail.INV_ThirdNo,
                            SOBD_OEMNo = loopStockOutDetail.INV_OEMNo,
                            SOBD_Name = loopSalesOrderDetail.SOD_Name,
                            SOBD_Specification = loopStockOutDetail.INV_Specification,
                            SOBD_UOM = loopSalesOrderDetail.SOD_UOM,
                            SOBD_WH_ID = loopStockOutDetail.INV_WH_ID,
                            SOBD_WHB_ID = loopStockOutDetail.INV_WHB_ID,
                            //出库成本
                            SOBD_UnitCostPrice = loopStockOutDetail.INV_PurchaseUnitPrice,
                            //销售单价
                            SOBD_UnitSalePrice = loopSalesOrderDetail.SOD_UnitPrice,
                            //出库数量
                            SOBD_Qty = loopStockOutDetail.StockOutQty,
                            SOBD_IsValid = true,
                            SOBD_CreatedBy = LoginInfoDAX.UserName,
                            SOBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                            SOBD_UpdatedBy = LoginInfoDAX.UserName,
                            SOBD_UpdatedTime = BLLCom.GetCurStdDatetime(),
                        };
                        //出库金额 = 出库成本 * 数量
                        newStockOutBillDetail.SOBD_Amount = Math.Round((loopStockOutDetail.StockOutQty ?? 0) * (newStockOutBillDetail.SOBD_UnitCostPrice ?? 0), 2);
                        newSalesStockOutBillDetailList.Add(newStockOutBillDetail);

                        #endregion

                        #region 生成/更新[库存]，生成[库存异动日志]

                        //[出库明细]列表中第一次出现该配件
                        //查询该配件是否在[库存]中存在
                        MDLPIS_Inventory resultInventory = new MDLPIS_Inventory();
                        QueryForObject<MDLPIS_Inventory, MDLPIS_Inventory>(new MDLPIS_Inventory
                        {
                            WHERE_INV_Org_ID = newSalesStockOutBill.SOB_Org_ID,
                            WHERE_INV_Barcode = loopStockOutDetail.INV_Barcode,
                            WHERE_INV_BatchNo = loopStockOutDetail.INV_BatchNo,
                            WHERE_INV_WH_ID = loopStockOutDetail.INV_WH_ID,
                            WHERE_INV_IsValid = true
                        }, resultInventory);

                        //[库存]中不存在该配件
                        if (string.IsNullOrEmpty(resultInventory.INV_ID))
                        {
                            //配件：loopStockOutDetail.INV_Name(条形码：loopStockOutDetail.INV_Barcode)的库存不存在，审核失败
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                            {
                                    loopStockOutDetail.INV_Name, loopStockOutDetail.INV_Barcode, MsgParam.NOTEXIST,
                                    SystemActionEnum.Name.APPROVE
                            });
                            LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                            return false;
                        }

                        if (resultInventory.INV_Qty < loopStockOutDetail.StockOutQty)
                        {
                            //配件：loopStockOutDetail.INV_Name(条形码：loopStockOutDetail.INV_Barcode)的库存不足，审核失败
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                            {
                                    loopStockOutDetail.INV_Name, loopStockOutDetail.INV_Barcode, MsgParam.SHORTAGE,
                                    SystemActionEnum.Name.APPROVE
                            });
                            LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                            return false;
                        }
                        //[库存]中存在该配件
                        //更新[库存]中该配件的数量
                        resultInventory.INV_Qty -= loopStockOutDetail.StockOutQty;
                        resultInventory.INV_UpdatedBy = LoginInfoDAX.UserName;
                        resultInventory.INV_UpdatedTime = BLLCom.GetCurStdDatetime();
                        resultInventory.WHERE_INV_ID = resultInventory.INV_ID;
                        resultInventory.WHERE_INV_VersionNo = resultInventory.INV_VersionNo;
                        updateStockOutInventoryList.Add(resultInventory);

                        //生成[库存异动日志]
                        newStockOutInventoryTransLogList.Add(GenerateStockOutInventoryTransLog(newSalesStockOutBill, loopSalesOrderDetail, loopStockOutDetail, resultInventory, paramHead));

                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region [无进销存]的场合，根据[销售明细]列表创建[出库单明细]
                    //创建[出库单明细]
                    MDLPIS_StockOutBillDetail newStockOutBillDetail = new MDLPIS_StockOutBillDetail
                    {
                        SOBD_ID = Guid.NewGuid().ToString(),
                        SOBD_SOB_ID = newSalesStockOutBill.SOB_ID,
                        SOBD_SOB_No = newSalesStockOutBill.SOB_No,
                        SOBD_SourceDetailID = loopSalesOrderDetail.SOD_ID,
                        SOBD_Barcode = loopSalesOrderDetail.SOD_Barcode,
                        SOBD_ThirdNo = loopSalesOrderDetail.INV_ThirdNo,
                        SOBD_OEMNo = loopSalesOrderDetail.INV_OEMNo,
                        SOBD_Name = loopSalesOrderDetail.SOD_Name,
                        SOBD_Specification = loopSalesOrderDetail.SOD_Specification,
                        SOBD_UOM = loopSalesOrderDetail.SOD_UOM,
                        //销售单价
                        SOBD_UnitSalePrice = loopSalesOrderDetail.SOD_UnitPrice,
                        //出库数量
                        SOBD_Qty = loopSalesOrderDetail.SOD_Qty,
                        SOBD_IsValid = true,
                        SOBD_CreatedBy = LoginInfoDAX.UserName,
                        SOBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                        SOBD_UpdatedBy = LoginInfoDAX.UserName,
                        SOBD_UpdatedTime = BLLCom.GetCurStdDatetime(),
                    };
                    //出库金额 = 出库成本 * 数量
                    newStockOutBillDetail.SOBD_Amount = Math.Round((loopSalesOrderDetail.SOD_Qty ?? 0) * (newStockOutBillDetail.SOBD_UnitCostPrice ?? 0), 2);
                    newSalesStockOutBillDetailList.Add(newStockOutBillDetail);
                    #endregion
                }

                #region 创建[应收单明细]

                //新增[来源类型]为[销售应收]，[业务状态]为[已生成]，金额为负，[审核状态]为[已审核]的[应收单明细]，[是否负向明细]根据[销售订单].[来源类型]区分
                MDLFM_AccountReceivableBillDetail newAccountReceivableBillDetail = new MDLFM_AccountReceivableBillDetail
                {
                    ARBD_ID = Guid.NewGuid().ToString(),
                    ARBD_ARB_ID = newSalesAccountReceivableBill.ARB_ID,
                    ARBD_IsMinusDetail = newSalesAccountReceivableBill.ARB_BillDirectName != BillDirectionEnum.Name.PLUS,
                    ARBD_SrcBillNo = paramHead.SO_No,
                    ARBD_SrcBillDetailID = loopSalesOrderDetail.SOD_ID,
                    ARBD_Org_ID = newSalesAccountReceivableBill.ARB_Org_ID,
                    ARBD_Org_Name = newSalesAccountReceivableBill.ARB_Org_Name,
                    ARBD_ReceivedAmount = 0,
                    ARBD_BusinessStatusCode = newSalesAccountReceivableBill.ARB_BusinessStatusCode,
                    ARBD_BusinessStatusName = newSalesAccountReceivableBill.ARB_BusinessStatusName,
                    ARBD_ApprovalStatusCode = newSalesAccountReceivableBill.ARB_ApprovalStatusCode,
                    ARBD_ApprovalStatusName = newSalesAccountReceivableBill.ARB_ApprovalStatusName,
                    ARBD_IsValid = true,
                    ARBD_CreatedBy = LoginInfoDAX.UserName,
                    ARBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                    ARBD_UpdatedBy = LoginInfoDAX.UserName,
                    ARBD_UpdatedTime = BLLCom.GetCurStdDatetime()
                };
                if (newSalesAccountReceivableBill.ARB_BillDirectName == BillDirectionEnum.Name.PLUS)
                {
                    //正向，金额为正
                    newAccountReceivableBillDetail.ARBD_AccountReceivableAmount = loopSalesOrderDetail.SOD_TotalAmount;
                    newAccountReceivableBillDetail.ARBD_UnReceiveAmount = loopSalesOrderDetail.SOD_TotalAmount;
                }
                else
                {
                    //负向，金额为负
                    newAccountReceivableBillDetail.ARBD_AccountReceivableAmount = -loopSalesOrderDetail.SOD_TotalAmount;
                    newAccountReceivableBillDetail.ARBD_UnReceiveAmount = -loopSalesOrderDetail.SOD_TotalAmount;
                }
                newSalesAccountReceivableBillDetailList.Add(newAccountReceivableBillDetail);

                #endregion

            }
            #endregion

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

                if (!string.IsNullOrEmpty(newSalesAccountReceivableBill.ARB_ID))
                {
                    //正常销售时产生的正向的[销售应收]
                    bool insertAccountReceivableBillResult = _bll.Insert(newSalesAccountReceivableBill);
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

                if (newSalesAccountReceivableBillDetailList.Count > 0)
                {
                    //正常销售时产生的正向的[销售应收]
                    bool insertAccountReceivableBillDetailResult = _bll.InsertByList<MDLFM_AccountReceivableBillDetail, MDLFM_AccountReceivableBillDetail>(newSalesAccountReceivableBillDetailList);
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
                foreach (var loopInventory in updateStockOutInventoryList)
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

                if (newStockOutInventoryTransLogList.Count > 0)
                {
                    bool insertInventoryTransLogResult = _bll.InsertByList<MDLPIS_InventoryTransLog, MDLPIS_InventoryTransLog>(newStockOutInventoryTransLogList);
                    if (!insertInventoryTransLogResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.PIS_InventoryTransLog });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 新增[出库单]

                if (!string.IsNullOrEmpty(newSalesStockOutBill.SOB_ID))
                {
                    bool insertStockOutBillResult = _bll.Insert(newSalesStockOutBill);
                    if (!insertStockOutBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.PIS_StockOutBill });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 新增[出库单明细]

                if (newSalesStockOutBillDetailList.Count > 0)
                {
                    bool insertStockOutBillDetailResult = _bll.InsertByList<MDLPIS_StockOutBillDetail, MDLPIS_StockOutBillDetail>(newSalesStockOutBillDetailList);
                    if (!insertStockOutBillDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.PIS_StockOutBillDetail });
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

        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="paramHead">销售订单</param>
        /// <param name="paramDetailList">销售订单明细列表</param>
        /// <param name="paramIsHasInventory">是否启用进销存模块</param>
        /// <param name="paramSalesStockOutDetailList">销售出库单</param>
        /// <returns></returns>
        public bool UnApproveDetailDS(SalesOrderManagerUIModel paramHead, SkyCarBindingList<SalesOrderDetailUIModel, MDLSD_SalesOrderDetail> paramDetailList, bool paramIsHasInventory, List<SalesStockOutDetailUIModel> paramSalesStockOutDetailList)
        {
            var funcName = "UnApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证

            //验证销售订单
            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.SO_ID)
                || string.IsNullOrEmpty(paramHead.SO_No))
            {
                //没有获取到销售订单，反审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.SD_SalesOrder, SystemActionEnum.Name.UNAPPROVE });
                LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);
                return false;
            }
            #endregion

            #region 准备数据

            #region 变量定义

            //待更新的[销售订单]
            MDLSD_SalesOrder updateSalesOrder = paramHead.ToTBModelForSaveAndDelete<MDLSD_SalesOrder>();
            //待更新的[销售订单明细]列表
            List<MDLSD_SalesOrderDetail> updateSalesOrderDetailList = new List<MDLSD_SalesOrderDetail>();

            //手工创建、在线销售相关
            //待删除的销售出库时产生的[应收单]
            MDLFM_AccountReceivableBill deleteSalesAccountReceivableBill = new MDLFM_AccountReceivableBill();
            //待删除的销售出库时产生的[应收单明细]列表
            List<MDLFM_AccountReceivableBillDetail> deleteSalesAccountReceivableDetailList = new List<MDLFM_AccountReceivableBillDetail>();
            //待删除的销售出库时产生的[出库单]
            MDLPIS_StockOutBill deleteSalesStockOutBill = new MDLPIS_StockOutBill();
            //待删除的销售出库时产生的[出库单明细]列表
            List<MDLPIS_StockOutBillDetail> deleteSalesStockOutDetailList = new List<MDLPIS_StockOutBillDetail>();
            //待新增的销售出库时产生的[库存异动日志]列表（与审核时数量相反）
            List<MDLPIS_InventoryTransLog> newInventoryTransLogList = new List<MDLPIS_InventoryTransLog>();
            //待更新的销售出库时更新的[库存]列表
            List<MDLPIS_Inventory> updateInventoryList = new List<MDLPIS_Inventory>();

            #endregion

            #region 更新[销售订单]
            //将销售订单[审核状态]更新为[待审核]，[单据状态]更新为[已生成]
            updateSalesOrder.SO_VersionNo++;
            updateSalesOrder.SO_ApprovalStatusCode = ApprovalStatusEnum.Code.DSH;
            updateSalesOrder.SO_ApprovalStatusName = ApprovalStatusEnum.Name.DSH;
            updateSalesOrder.SO_UpdatedBy = LoginInfoDAX.UserName;
            updateSalesOrder.SO_UpdatedTime = BLLCom.GetCurStdDatetime();
            updateSalesOrder.SO_StatusCode = SalesOrderStatusEnum.Code.YSC;
            updateSalesOrder.SO_StatusName = SalesOrderStatusEnum.Name.YSC;

            #endregion

            #region 更新[销售订单明细]

            _bll.CopyModelList(paramDetailList, updateSalesOrderDetailList);

            foreach (var loopSalesOrderDetail in updateSalesOrderDetailList)
            {
                if (string.IsNullOrEmpty(loopSalesOrderDetail.SOD_ID))
                {
                    continue;
                }

                //更新[销售订单明细]审核状态 和 单据状态与单头一致
                loopSalesOrderDetail.WHERE_SOD_ID = loopSalesOrderDetail.SOD_ID;
                loopSalesOrderDetail.WHERE_SOD_VersionNo = loopSalesOrderDetail.SOD_VersionNo;
                loopSalesOrderDetail.SOD_VersionNo++;
                loopSalesOrderDetail.SOD_ApprovalStatusCode = updateSalesOrder.SO_ApprovalStatusCode;
                loopSalesOrderDetail.SOD_ApprovalStatusName = updateSalesOrder.SO_ApprovalStatusName;
                loopSalesOrderDetail.SOD_StatusCode = updateSalesOrder.SO_StatusCode;
                loopSalesOrderDetail.SOD_StatusName = updateSalesOrder.SO_StatusName;
                loopSalesOrderDetail.SOD_UpdatedBy = LoginInfoDAX.UserName;
                loopSalesOrderDetail.SOD_UpdatedTime = BLLCom.GetCurStdDatetime();
            }
            #endregion

            #region [销售订单].[来源类型]为{手工创建}、{在线销售}的场合

            #region 获取待删除的[应收单]

            _bll.QueryForObject<MDLFM_AccountReceivableBill, MDLFM_AccountReceivableBill>(new MDLFM_AccountReceivableBill
            {
                WHERE_ARB_SrcBillNo = updateSalesOrder.SO_No,
                WHERE_ARB_IsValid = true
            }, deleteSalesAccountReceivableBill);
            deleteSalesAccountReceivableBill.WHERE_ARB_ID = deleteSalesAccountReceivableBill.ARB_ID;
            #endregion

            #region 获取待删除的[应收单明细]列表

            if (!string.IsNullOrEmpty(deleteSalesAccountReceivableBill.ARB_ID))
            {
                _bll.QueryForList<MDLFM_AccountReceivableBillDetail, MDLFM_AccountReceivableBillDetail>(new MDLFM_AccountReceivableBillDetail
                {
                    WHERE_ARBD_ARB_ID = deleteSalesAccountReceivableBill.ARB_ID,
                    WHERE_ARBD_SrcBillNo = updateSalesOrder.SO_No,
                    WHERE_ARBD_IsValid = true
                }, deleteSalesAccountReceivableDetailList);
            }
            foreach (var loopAccountReceivableBillDetail in deleteSalesAccountReceivableDetailList)
            {
                if (string.IsNullOrEmpty(loopAccountReceivableBillDetail.ARBD_ID))
                {
                    continue;
                }
                loopAccountReceivableBillDetail.WHERE_ARBD_ID = loopAccountReceivableBillDetail.ARBD_ID;
            }
            #endregion

            #region 获取待删除的[出库单]

            _bll.QueryForObject<MDLPIS_StockOutBill, MDLPIS_StockOutBill>(new MDLPIS_StockOutBill
            {
                WHERE_SOB_SourceTypeName = StockOutBillSourceTypeEnum.Name.XSCK,
                WHERE_SOB_SourceNo = updateSalesOrder.SO_No,
                WHERE_SOB_IsValid = true
            }, deleteSalesStockOutBill);
            deleteSalesStockOutBill.WHERE_SOB_ID = deleteSalesStockOutBill.SOB_ID;

            #endregion

            #region 获取待删除的[出库单明细]列表

            if (!string.IsNullOrEmpty(deleteSalesStockOutBill.SOB_ID))
            {
                _bll.QueryForList<MDLPIS_StockOutBillDetail, MDLPIS_StockOutBillDetail>(new MDLPIS_StockOutBillDetail
                {
                    WHERE_SOBD_SOB_ID = deleteSalesStockOutBill.SOB_ID,
                    WHERE_SOBD_SOB_No = deleteSalesStockOutBill.SOB_No,
                    WHERE_SOBD_IsValid = true
                }, deleteSalesStockOutDetailList);
            }
            foreach (var loopStockOutBillDetail in deleteSalesStockOutDetailList)
            {
                if (string.IsNullOrEmpty(loopStockOutBillDetail.SOBD_ID))
                {
                    continue;
                }
                loopStockOutBillDetail.WHERE_SOBD_ID = loopStockOutBillDetail.SOBD_ID;

                #region 有进销存的场合，新增[库存异动日志]列表，更新[库存]

                if (paramIsHasInventory)
                {
                    MDLPIS_Inventory resultInventory = new MDLPIS_Inventory();

                    _bll.QueryForObject<MDLPIS_Inventory, MDLPIS_Inventory>(new MDLPIS_Inventory
                    {
                        WHERE_INV_Org_ID = deleteSalesStockOutBill.SOB_Org_ID,
                        WHERE_INV_Barcode = loopStockOutBillDetail.SOBD_Barcode,
                        WHERE_INV_BatchNo = loopStockOutBillDetail.SOBD_BatchNo,
                        WHERE_INV_WH_ID = loopStockOutBillDetail.SOBD_WH_ID,
                        WHERE_INV_IsValid = true,
                    }, resultInventory);
                    if (!string.IsNullOrEmpty(resultInventory.INV_ID)
                        && !string.IsNullOrEmpty(resultInventory.INV_Barcode)
                        && !string.IsNullOrEmpty(resultInventory.INV_BatchNo))
                    {
                        //[反审核]时，恢复[审核]时出库的配件数量
                        resultInventory.INV_Qty += loopStockOutBillDetail.SOBD_Qty;
                        resultInventory.INV_UpdatedBy = LoginInfoDAX.UserName;
                        resultInventory.INV_UpdatedTime = BLLCom.GetCurStdDatetime();
                        resultInventory.WHERE_INV_ID = resultInventory.INV_ID;
                        resultInventory.WHERE_INV_VersionNo = resultInventory.INV_VersionNo;
                    }
                    updateInventoryList.Add(resultInventory);

                    //生成[库存异动日志]
                    newInventoryTransLogList.Add(GenerateStockOutInventoryTransLogOfUnApprove(deleteSalesStockOutBill, loopStockOutBillDetail, resultInventory, paramHead, paramSalesStockOutDetailList));
                }
                #endregion
            }
            #endregion

            #endregion

            #endregion

            #region 带事务的保存和删除

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 更新[销售订单]

                bool updateSalesOrderResult = _bll.Save(updateSalesOrder);
                if (!updateSalesOrderResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.SD_SalesOrder });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 更新[销售订单明细]

                foreach (var loopSalesOrderDetail in updateSalesOrderDetailList)
                {
                    bool updateSalesOrderDetailResult = _bll.Save(loopSalesOrderDetail);
                    if (!updateSalesOrderDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_SalesOrderDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 删除[应收单]
                if (!string.IsNullOrEmpty(deleteSalesAccountReceivableBill.ARB_ID))
                {
                    var deleteAccountReceivableBillResult = _bll.Delete<MDLFM_AccountReceivableBill>(deleteSalesAccountReceivableBill);
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

                if (deleteSalesAccountReceivableDetailList.Count > 0)
                {
                    var deleteAccountReceivableDetailResult = _bll.DeleteByList<MDLFM_AccountReceivableBillDetail, MDLFM_AccountReceivableBillDetail>(deleteSalesAccountReceivableDetailList);
                    if (!deleteAccountReceivableDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.FM_AccountReceivableBillDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #endregion

                #region 删除[出库单]

                if (!string.IsNullOrEmpty(deleteSalesStockOutBill.SOB_ID))
                {
                    var deleteStockOutBillResult = _bll.Delete<MDLPIS_StockOutBill>(deleteSalesStockOutBill);
                    if (!deleteStockOutBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.PIS_StockOutBill });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #endregion

                #region 删除[出库单明细]列表

                if (deleteSalesStockOutDetailList.Count > 0)
                {
                    var deleteStockOutBillDetailResult = _bll.DeleteByList<MDLPIS_StockOutBillDetail, MDLPIS_StockOutBillDetail>(deleteSalesStockOutDetailList);
                    if (!deleteStockOutBillDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.PIS_StockOutBillDetail });
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
            _bll.CopyModel(updateSalesOrder, paramHead);
            _bll.CopyModelList(updateSalesOrderDetailList, paramDetailList);

            return true;
        }

        /// <summary>
        /// 核实
        /// </summary>
        /// <param name="paramHead">单头UIModel</param>
        /// <param name="paramDetailList">销售订单明细列表</param>
        /// <param name="paramSalesOrderReturnDetailList">退货的销售明细列表</param>
        /// <param name="paramReturnStockInDetailList">退货入库明细列表</param>
        /// <param name="paramSalesOrderLoseDetailList">丢失的销售明细列表</param>
        /// <returns></returns>
        public bool VerifyDetailDS(SalesOrderManagerUIModel paramHead, SkyCarBindingList<SalesOrderDetailUIModel, MDLSD_SalesOrderDetail> paramDetailList, List<SalesReturnDetailUIModel> paramSalesOrderReturnDetailList, List<ReturnStockInDetailUIModel> paramReturnStockInDetailList, List<SalesOrderDetailUIModel> paramSalesOrderLoseDetailList)
        {
            var funcName = "VerifyDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.SO_ID)
                || string.IsNullOrEmpty(paramHead.SO_No))
            {
                //没有获取到销售订单，核实失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.SD_SalesOrder, SystemActionEnum.Name.VERIFY });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            #endregion

            #region 准备数据

            #region 变量定义

            //待更新的当前核实的[销售订单]
            MDLSD_SalesOrder updateCurSalesOrder = new MDLSD_SalesOrder();

            //1. 退货相关
            //待新增的来源类型为退货的[销售订单]
            MDLSD_SalesOrder newRejectSalesOrder = new MDLSD_SalesOrder();
            //待新增的退货的[销售订单明细]列表
            List<MDLSD_SalesOrderDetail> newRejectSalesOrderDetailList = new List<MDLSD_SalesOrderDetail>();
            //待新增的退货时的[应收单]
            MDLFM_AccountReceivableBill newRejectAccountReceivableBill = new MDLFM_AccountReceivableBill();
            //待新增的退货时的[应收单明细]列表
            List<MDLFM_AccountReceivableBillDetail> newRejectReceivableBillDetailList = new List<MDLFM_AccountReceivableBillDetail>();
            //待新增的退货入库[入库单]
            MDLPIS_StockInBill newReturnStockInBill = new MDLPIS_StockInBill();
            //待新增的退货入库[入库单明细]列表
            List<MDLPIS_StockInDetail> newReturnStockInDetailList = new List<MDLPIS_StockInDetail>();
            //待更新的[库存]列表
            List<MDLPIS_Inventory> updateInventoryList = new List<MDLPIS_Inventory>();
            //待新增的[库存异动日志]列表
            List<MDLPIS_InventoryTransLog> newInventoryTransLogList = new List<MDLPIS_InventoryTransLog>();

            //2. 丢失相关
            //待更新的原[应收单]
            MDLFM_AccountReceivableBill updateAccountReceivableBill = new MDLFM_AccountReceivableBill();
            //待新增的赔偿时的[应收单]
            MDLFM_AccountReceivableBill newLoseAccountReceivableBill = new MDLFM_AccountReceivableBill();
            //待新增的赔偿时的[应收单明细]列表
            List<MDLFM_AccountReceivableBillDetail> newLoseReceivableBillDetailList = new List<MDLFM_AccountReceivableBillDetail>();

            #endregion

            #region 更新当前核实的[销售订单].[单据状态]
            updateCurSalesOrder = paramHead.ToTBModelForSaveAndDelete<MDLSD_SalesOrder>();
            updateCurSalesOrder.SO_ID = updateCurSalesOrder.WHERE_SO_ID;
            if (paramDetailList.All(x => x.SOD_RejectQty == x.SOD_Qty))
            {
                //所有配件都拒收，则更新[销售订单].[单据状态]为{已关闭}
                updateCurSalesOrder.SO_StatusName = SalesOrderStatusEnum.Name.YGB;
                updateCurSalesOrder.SO_StatusCode = SalesOrderStatusEnum.Code.YGB;
            }
            else
            {
                //存在配件不是全部拒收，则更新[销售订单].[单据状态]为{交易成功}
                updateCurSalesOrder.SO_StatusName = SalesOrderStatusEnum.Name.JYCG;
                updateCurSalesOrder.SO_StatusCode = SalesOrderStatusEnum.Code.JYCG;
            }
            updateCurSalesOrder.SO_UpdatedBy = LoginInfoDAX.UserName;
            updateCurSalesOrder.SO_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            #region 更新当前核实的[销售订单明细]

            //更新原应收单的应收金额
            decimal originalNewReceiveableAmount = 0;

            foreach (var loopDetail in paramDetailList)
            {
                loopDetail.SOD_StatusName = updateCurSalesOrder.SO_StatusName;
                loopDetail.SOD_StatusCode = updateCurSalesOrder.SO_StatusCode;
                loopDetail.SOD_UpdatedBy = LoginInfoDAX.UserName;
                loopDetail.SOD_UpdatedTime = BLLCom.GetCurStdDatetime();

                //原应收单.[应收金额] = （签收数量+拒收数量） * 单价
                originalNewReceiveableAmount += Math.Round(((loopDetail.SOD_SignQty ?? 0) + (loopDetail.SOD_RejectQty ?? 0)) * (loopDetail.SOD_UnitPrice ?? 0), 2);
            }
            #endregion

            #region 获取对应的[物流单]

            MDLSD_LogisticsBill resultLogisticsBill = new MDLSD_LogisticsBill();
            _bll.QueryForObject<MDLSD_LogisticsBill, MDLSD_LogisticsBill>(new MDLSD_LogisticsBill
            {
                WHERE_LB_SourceNo = updateCurSalesOrder.SO_No,
                WHERE_LB_IsValid = true
            }, resultLogisticsBill);

            #endregion

            #region 获取对应的原应收单

            _bll.QueryForObject<MDLFM_AccountReceivableBill, MDLFM_AccountReceivableBill>(new MDLFM_AccountReceivableBill
            {
                WHERE_ARB_SourceTypeName = AccountReceivableBillSourceTypeEnum.Name.XSYS,
                WHERE_ARB_SrcBillNo = updateCurSalesOrder.SO_No,
                WHERE_ARB_IsValid = true,
            }, updateAccountReceivableBill);
            //更新[应收单].[单据状态]为{执行中}
            updateAccountReceivableBill.ARB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.ZXZ;
            updateAccountReceivableBill.ARB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.ZXZ;
            updateAccountReceivableBill.ARB_UpdatedBy = LoginInfoDAX.UserName;
            updateAccountReceivableBill.ARB_UpdatedTime = BLLCom.GetCurStdDatetime();
            updateAccountReceivableBill.WHERE_ARB_ID = updateAccountReceivableBill.ARB_ID;
            updateAccountReceivableBill.WHERE_ARB_VersionNo = updateAccountReceivableBill.ARB_VersionNo;

            #endregion

            if (paramSalesOrderReturnDetailList.Count > 0)
            {
                #region 存在退货配件的场合

                #region 创建退货的[销售订单]

                newRejectSalesOrder = paramHead.ToTBModelForSaveAndDelete<MDLSD_SalesOrder>();

                newRejectSalesOrder.SO_ID = Guid.NewGuid().ToString();
                newRejectSalesOrder.SO_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.SO);
                //根据当前销售订单.[来源类型]设置销售退货.[来源类型]
                switch (updateCurSalesOrder.SO_SourceTypeName)
                {
                    case SalesOrderSourceTypeEnum.Name.SGCJ:
                        //当前销售订单.[来源类型]为{手工创建}的场合，销售退货.[来源类型]为{手工创建退货}
                        newRejectSalesOrder.SO_SourceTypeName = SalesOrderSourceTypeEnum.Name.SGCJTH;
                        newRejectSalesOrder.SO_SourceTypeCode = SalesOrderSourceTypeEnum.Code.SGCJTH;
                        break;
                    case SalesOrderSourceTypeEnum.Name.ZXXS:
                        //当前销售订单.[来源类型]为{在线销售}的场合，销售退货.[来源类型]为{在线销售退货}
                        newRejectSalesOrder.SO_SourceTypeName = SalesOrderSourceTypeEnum.Name.ZXXSTH;
                        newRejectSalesOrder.SO_SourceTypeCode = SalesOrderSourceTypeEnum.Code.ZXXSTH;
                        break;
                }
                newRejectSalesOrder.SO_SourceNo = paramHead.SO_No;
                //单据状态为{交易完成}
                newRejectSalesOrder.SO_StatusName = SalesOrderStatusEnum.Name.JYCG;
                newRejectSalesOrder.SO_StatusCode = SalesOrderStatusEnum.Code.JYCG;
                //审核状态为{已审核}
                newRejectSalesOrder.SO_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
                newRejectSalesOrder.SO_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
                //业务员
                newRejectSalesOrder.SO_SalesByID = paramHead.SO_SalesByID;
                newRejectSalesOrder.SO_SalesByName = paramHead.SO_SalesByName;
                newRejectSalesOrder.SO_IsValid = true;
                newRejectSalesOrder.SO_CreatedBy = LoginInfoDAX.UserName;
                newRejectSalesOrder.SO_CreatedTime = BLLCom.GetCurStdDatetime();
                newRejectSalesOrder.SO_UpdatedBy = LoginInfoDAX.UserName;
                newRejectSalesOrder.SO_UpdatedTime = BLLCom.GetCurStdDatetime();

                #endregion

                #region 创建负向的[应收单]

                //新增[来源类型]为[销售应收]，[单据方向]为[负向]，金额为负，[业务状态]为{执行中}，[审核状态]为{已审核}的[应收单]
                newRejectAccountReceivableBill.ARB_ID = Guid.NewGuid().ToString();
                newRejectAccountReceivableBill.ARB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.ARB);
                newRejectAccountReceivableBill.ARB_BillDirectCode = BillDirectionEnum.Code.MINUS;
                newRejectAccountReceivableBill.ARB_BillDirectName = BillDirectionEnum.Name.MINUS;
                newRejectAccountReceivableBill.ARB_SourceTypeCode = AccountReceivableBillSourceTypeEnum.Code.XSYS;
                newRejectAccountReceivableBill.ARB_SourceTypeName = AccountReceivableBillSourceTypeEnum.Name.XSYS;
                newRejectAccountReceivableBill.ARB_SrcBillNo = newRejectSalesOrder.SO_No;
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

                #region 创建退货入库的[入库单]

                //新增[来源类型]为[销售退货]，[业务状态]为{已完成}，[审核状态]为{已审核}的[入库单]
                newReturnStockInBill.SIB_ID = Guid.NewGuid().ToString();
                newReturnStockInBill.SIB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.SIB);
                newReturnStockInBill.SIB_SourceTypeCode = StockInBillSourceTypeEnum.Code.SSTH;
                newReturnStockInBill.SIB_SourceTypeName = StockInBillSourceTypeEnum.Name.SSTH;
                newReturnStockInBill.SIB_SourceNo = newRejectSalesOrder.SO_No;
                newReturnStockInBill.SIB_Org_ID = newRejectSalesOrder.SO_Org_ID;
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

                //退货产生的应收单.[应收金额]
                decimal returnReceivableAmount = 0;

                foreach (var loopSalesOrderReturnDetail in paramSalesOrderReturnDetailList)
                {
                    #region 退货的[销售订单明细]

                    MDLSD_SalesOrderDetail newReturnSalesOrderDetail = new MDLSD_SalesOrderDetail();
                    _bll.CopyModel(loopSalesOrderReturnDetail, newReturnSalesOrderDetail);
                    newReturnSalesOrderDetail.SOD_ID = Guid.NewGuid().ToString();
                    newReturnSalesOrderDetail.SOD_SO_ID = newRejectSalesOrder.SO_ID;

                    newReturnSalesOrderDetail.SOD_Qty = loopSalesOrderReturnDetail.SOD_RejectQty;
                    newReturnSalesOrderDetail.SOD_SignQty = null;
                    newReturnSalesOrderDetail.SOD_RejectQty = null;
                    newReturnSalesOrderDetail.SOD_LoseQty = null;
                    newReturnSalesOrderDetail.SOD_TotalAmount = Math.Round((newReturnSalesOrderDetail.SOD_Qty ?? 0) * (newReturnSalesOrderDetail.SOD_UnitPrice ?? 0), 2);

                    newReturnSalesOrderDetail.SOD_StatusName = newRejectSalesOrder.SO_StatusName;
                    newReturnSalesOrderDetail.SOD_StatusCode = newRejectSalesOrder.SO_StatusCode;
                    newReturnSalesOrderDetail.SOD_ApprovalStatusCode = newRejectSalesOrder.SO_ApprovalStatusCode;
                    newReturnSalesOrderDetail.SOD_ApprovalStatusName = newRejectSalesOrder.SO_ApprovalStatusName;

                    newReturnSalesOrderDetail.SOD_IsValid = true;
                    newReturnSalesOrderDetail.SOD_CreatedBy = LoginInfoDAX.UserName;
                    newReturnSalesOrderDetail.SOD_CreatedTime = BLLCom.GetCurStdDatetime();
                    newReturnSalesOrderDetail.SOD_UpdatedBy = LoginInfoDAX.UserName;
                    newReturnSalesOrderDetail.SOD_UpdatedTime = BLLCom.GetCurStdDatetime();

                    newRejectSalesOrderDetailList.Add(newReturnSalesOrderDetail);

                    #endregion

                    #region 退货的[应收单明细]
                    //新增[来源类型]为[销售应收]，[是否负向明细]为[true]，金额为负的[应收单明细]
                    MDLFM_AccountReceivableBillDetail newAccountReceivableBillDetail = new MDLFM_AccountReceivableBillDetail
                    {
                        ARBD_ID = Guid.NewGuid().ToString(),
                        ARBD_ARB_ID = newRejectAccountReceivableBill.ARB_ID,
                        ARBD_IsMinusDetail = true,
                        ARBD_SrcBillNo = newRejectSalesOrder.SO_No,
                        ARBD_SrcBillDetailID = loopSalesOrderReturnDetail.SOD_ID,
                        ARBD_Org_ID = newRejectAccountReceivableBill.ARB_Org_ID,
                        ARBD_Org_Name = newRejectAccountReceivableBill.ARB_Org_Name,
                        //应收金额 = 拒收数量 * 单价
                        ARBD_AccountReceivableAmount = -Math.Round((loopSalesOrderReturnDetail.SOD_RejectQty ?? 0) * (loopSalesOrderReturnDetail.SOD_UnitPrice ?? 0), 2),
                        ARBD_ReceivedAmount = 0,
                        ARBD_UnReceiveAmount = -Math.Round((loopSalesOrderReturnDetail.SOD_RejectQty ?? 0) * (loopSalesOrderReturnDetail.SOD_UnitPrice ?? 0), 2),
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
                //退货的[销售订单].[总金额]：正数
                newRejectSalesOrder.SO_TotalAmount = returnReceivableAmount;
                //应收金额 = 拒收数量 * 单价：负数
                newRejectAccountReceivableBill.ARB_AccountReceivableAmount = returnReceivableAmount;
                newRejectAccountReceivableBill.ARB_ReceivedAmount = 0;
                newRejectAccountReceivableBill.ARB_UnReceiveAmount = returnReceivableAmount;

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
                        newRejectSalesOrderDetailList.FirstOrDefault(x => x.SOD_Barcode == newStockInDetail.SID_Barcode);
                    if (curInDetailOfSalesDetail != null)
                    {
                        //[入库单明细].[来源单据明细ID] 为 [销售退货单明细].[ID]
                        newStockInDetail.SID_SourceDetailID = curInDetailOfSalesDetail.SOD_ID;
                    }

                    newReturnStockInDetailList.Add(newStockInDetail);
                    #endregion

                    //核实销售订单时，存在销售退货的场合，更新[库存]，创建[库存异动日志]
                    //在[入库单明细]列表中第一次出现的配件[库存]信息
                    MDLPIS_Inventory inventoryExists = null;

                    foreach (var loopInventory in updateInventoryList)
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
                        newInventoryTransLogList.Add(GenerateStockInInventoryTransLog(newReturnStockInBill, loopReturnStockInDetail, inventoryExists, paramHead));
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
                            updateInventoryList.Add(inventoryToInsert);

                            //生成[库存异动日志]
                            newInventoryTransLogList.Add(GenerateStockInInventoryTransLog(newReturnStockInBill, loopReturnStockInDetail, inventoryToInsert, paramHead));
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
                            updateInventoryList.Add(resultInventory);

                            //生成[库存异动日志]
                            newInventoryTransLogList.Add(GenerateStockInInventoryTransLog(newReturnStockInBill, loopReturnStockInDetail, resultInventory, paramHead));
                        }
                    }
                }
                #endregion
            }

            if (paramSalesOrderLoseDetailList.Count > 0)
            {
                #region 存在丢失配件的场合

                //计算其他应收（赔偿）时的[应收单].[应收金额]
                decimal pcReceiveTotalAmount = 0;
                foreach (var loopSalesOrderLoseDetail in paramSalesOrderLoseDetailList)
                {
                    #region 其他应收（赔偿）的[应收单明细]
                    //新增[来源类型]为[其他应收（赔偿）]，[是否负向明细]为[false]，金额为正的[应收单明细]
                    MDLFM_AccountReceivableBillDetail newAccountReceivableBillDetail = new MDLFM_AccountReceivableBillDetail
                    {
                        ARBD_ID = Guid.NewGuid().ToString(),
                        ARBD_ARB_ID = newLoseAccountReceivableBill.ARB_ID,
                        ARBD_IsMinusDetail = false,
                        ARBD_SrcBillNo = updateCurSalesOrder.SO_No,
                        ARBD_SrcBillDetailID = loopSalesOrderLoseDetail.SOD_ID,
                        ARBD_Org_ID = newLoseAccountReceivableBill.ARB_Org_ID,
                        ARBD_Org_Name = newLoseAccountReceivableBill.ARB_Org_Name,
                        //应收金额 = 丢失数量 * 单价
                        ARBD_AccountReceivableAmount = Math.Round((loopSalesOrderLoseDetail.SOD_LoseQty ?? 0) * (loopSalesOrderLoseDetail.SOD_UnitPrice ?? 0), 2),
                        ARBD_ReceivedAmount = 0,
                        ARBD_UnReceiveAmount = Math.Round((loopSalesOrderLoseDetail.SOD_LoseQty ?? 0) * (loopSalesOrderLoseDetail.SOD_UnitPrice ?? 0), 2),
                        ARBD_BusinessStatusCode = newLoseAccountReceivableBill.ARB_BusinessStatusCode,
                        ARBD_BusinessStatusName = newLoseAccountReceivableBill.ARB_BusinessStatusName,
                        ARBD_ApprovalStatusCode = newLoseAccountReceivableBill.ARB_ApprovalStatusCode,
                        ARBD_ApprovalStatusName = newLoseAccountReceivableBill.ARB_ApprovalStatusName,
                        ARBD_IsValid = true,
                        ARBD_CreatedBy = LoginInfoDAX.UserName,
                        ARBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                        ARBD_UpdatedBy = LoginInfoDAX.UserName,
                        ARBD_UpdatedTime = BLLCom.GetCurStdDatetime()
                    };
                    pcReceiveTotalAmount += (newAccountReceivableBillDetail.ARBD_AccountReceivableAmount ?? 0);
                    newLoseReceivableBillDetailList.Add(newAccountReceivableBillDetail);
                    #endregion
                }

                #region 创建其他应收（赔偿）的[应收单]

                //新增[来源类型]为[其他应收（赔偿）]，[单据方向]为[正向]，金额为正，[业务状态]为{执行中}，[审核状态]为{已审核}的应收单
                newLoseAccountReceivableBill.ARB_ID = Guid.NewGuid().ToString();
                newLoseAccountReceivableBill.ARB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.ARB);
                newLoseAccountReceivableBill.ARB_BillDirectCode = BillDirectionEnum.Code.PLUS;
                newLoseAccountReceivableBill.ARB_BillDirectName = BillDirectionEnum.Name.PLUS;
                newLoseAccountReceivableBill.ARB_SourceTypeCode = AccountReceivableBillSourceTypeEnum.Code.QTYS;
                newLoseAccountReceivableBill.ARB_SourceTypeName = AccountReceivableBillSourceTypeEnum.Name.QTYS;
                newLoseAccountReceivableBill.ARB_SrcBillNo = updateCurSalesOrder.SO_No;
                newLoseAccountReceivableBill.ARB_Org_ID = LoginInfoDAX.OrgID;
                newLoseAccountReceivableBill.ARB_Org_Name = LoginInfoDAX.OrgShortName;
                newLoseAccountReceivableBill.ARB_PayObjectTypeName = AmountTransObjectTypeEnum.Name.DELIVERYMAN;
                newLoseAccountReceivableBill.ARB_PayObjectTypeCode = AmountTransObjectTypeEnum.Code.DELIVERYMAN;
                newLoseAccountReceivableBill.ARB_PayObjectName = resultLogisticsBill.LB_DeliveryBy;
                newLoseAccountReceivableBill.ARB_PayObjectID = resultLogisticsBill.LB_DeliveryByID;
                newLoseAccountReceivableBill.ARB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.ZXZ;
                newLoseAccountReceivableBill.ARB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.ZXZ;
                newLoseAccountReceivableBill.ARB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
                newLoseAccountReceivableBill.ARB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
                newLoseAccountReceivableBill.ARB_IsValid = true;
                newLoseAccountReceivableBill.ARB_CreatedBy = LoginInfoDAX.UserName;
                newLoseAccountReceivableBill.ARB_CreatedTime = BLLCom.GetCurStdDatetime();
                newLoseAccountReceivableBill.ARB_UpdatedBy = LoginInfoDAX.UserName;
                newLoseAccountReceivableBill.ARB_UpdatedTime = BLLCom.GetCurStdDatetime();
                //其他应收（赔偿）时的[应收单].[应收金额]
                newLoseAccountReceivableBill.ARB_AccountReceivableAmount = pcReceiveTotalAmount;
                newLoseAccountReceivableBill.ARB_ReceivedAmount = 0;
                newLoseAccountReceivableBill.ARB_UnReceiveAmount = pcReceiveTotalAmount;
                #endregion

                #region 更新原[应收单]金额

                //回写原[应收单].[应收金额]为 （签收数量+拒收数量） * 单价
                updateAccountReceivableBill.ARB_AccountReceivableAmount = originalNewReceiveableAmount;
                //回写原[应收单].[未收金额]为 原来的未收金额 - 丢失部分对应的金额
                updateAccountReceivableBill.ARB_UnReceiveAmount = (updateAccountReceivableBill.ARB_AccountReceivableAmount ?? 0) - (updateAccountReceivableBill.ARB_ReceivedAmount ?? 0);

                #endregion

                #endregion
            }
            #endregion

            #region 带事务的保存
            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 更新当前核实的[销售订单]

                bool saveCurSalesOrderResult = _bll.Save(updateCurSalesOrder);
                if (!saveCurSalesOrderResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_SalesOrder });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 保存当前核实的[销售订单明细]

                //执行保存
                bool saveCurDetailResult = _bll.UnitySave(paramDetailList);
                if (!saveCurDetailResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_SalesOrderDetail });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 更新对应的[物流单]和[物流单明细]列表

                if (!string.IsNullOrEmpty(resultLogisticsBill.LB_ID))
                {
                    var argsLogisticsBill = new MDLSD_LogisticsBill
                    {
                        WHERE_LB_ID = resultLogisticsBill.LB_ID,
                    };
                    if (updateCurSalesOrder.SO_StatusName == SalesOrderStatusEnum.Name.JYCG)
                    {
                        //[销售订单].[单据状态]更新为{交易成功}的场合，更新[物流单].[单据状态]为{已完成}
                        argsLogisticsBill.WHERE_LB_StatusName = LogisticsBillStatusEnum.Name.YWC;
                        argsLogisticsBill.WHERE_LB_StatusCode = LogisticsBillStatusEnum.Code.YWC;
                    }
                    else if (updateCurSalesOrder.SO_StatusName == SalesOrderStatusEnum.Name.YGB)
                    {
                        //[销售订单].[单据状态]更新为{已关闭}的场合，更新[物流单].[单据状态]为{已关闭}
                        argsLogisticsBill.WHERE_LB_StatusName = LogisticsBillStatusEnum.Name.YGB;
                        argsLogisticsBill.WHERE_LB_StatusCode = LogisticsBillStatusEnum.Code.YGB;
                    }
                    bool updateLogisticDetailResult = _bll.Update(SQLID.SD_SalesOrder_SQL05, argsLogisticsBill);
                    if (!updateLogisticDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_LogisticsBillDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 新增退货的[销售订单]

                if (!string.IsNullOrEmpty(newRejectSalesOrder.SO_ID))
                {
                    bool insertReturnSalesOrderResult = _bll.Insert(newRejectSalesOrder);
                    if (!insertReturnSalesOrderResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_SalesOrder });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 新增退货的[销售订单明细]

                if (newRejectSalesOrderDetailList.Count > 0)
                {
                    bool insertReturnSalesOrderDetailResult = _bll.InsertByList<MDLSD_SalesOrderDetail, MDLSD_SalesOrderDetail>(newRejectSalesOrderDetailList);
                    if (!insertReturnSalesOrderDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_SalesOrderDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 新增[应收单]

                //待新增的[应收单]列表
                List<MDLFM_AccountReceivableBill> newAccountReceivableBillList = new List<MDLFM_AccountReceivableBill>();
                if (!string.IsNullOrEmpty(newRejectAccountReceivableBill.ARB_ID))
                {
                    //退货时产生的应收单
                    newAccountReceivableBillList.Add(newRejectAccountReceivableBill);
                }
                if (!string.IsNullOrEmpty(newLoseAccountReceivableBill.ARB_ID))
                {
                    //赔偿时产生的应收单
                    newAccountReceivableBillList.Add(newLoseAccountReceivableBill);
                }
                if (newAccountReceivableBillList.Count > 0)
                {
                    bool insertAccountReceivableBillResult = _bll.InsertByList<MDLFM_AccountReceivableBill, MDLFM_AccountReceivableBill>(newAccountReceivableBillList);
                    if (!insertAccountReceivableBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.FM_AccountReceivableBill });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 更新[应收单]

                if (!string.IsNullOrEmpty(updateAccountReceivableBill.ARB_ID))
                {
                    bool updateAccountReceivableBillResult = _bll.Update(updateAccountReceivableBill);
                    if (!updateAccountReceivableBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.FM_AccountReceivableBill });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 新增[应收单明细]

                //待新增的[应收单明细]列表
                List<MDLFM_AccountReceivableBillDetail> newAccountReceivableBillDetailList = new List<MDLFM_AccountReceivableBillDetail>();
                //退货时产生的应收单明细
                newAccountReceivableBillDetailList.AddRange(newRejectReceivableBillDetailList);
                //赔偿时产生的应收单明细
                newAccountReceivableBillDetailList.AddRange(newLoseReceivableBillDetailList);
                if (newAccountReceivableBillDetailList.Count > 0)
                {
                    bool insertAccountReceivableBillDetailResult = _bll.InsertByList<MDLFM_AccountReceivableBillDetail, MDLFM_AccountReceivableBillDetail>(newAccountReceivableBillDetailList);
                    if (!insertAccountReceivableBillDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.FM_AccountReceivableBillDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 保存[入库单]

                if (!string.IsNullOrEmpty(newReturnStockInBill.SIB_ID))
                {
                    bool insertStockInBillResult = _bll.Insert(newReturnStockInBill);
                    if (!insertStockInBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_StockInBill });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 保存[入库单明细]

                if (newReturnStockInDetailList.Count > 0)
                {
                    bool insertStockInDetailResult = _bll.InsertByList<MDLPIS_StockInDetail, MDLPIS_StockInDetail>(newReturnStockInDetailList);
                    if (!insertStockInDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_StockInDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 保存[库存]

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
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_InventoryTransLog });
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
            CopyModel(updateCurSalesOrder, paramHead);

            //更新明细版本号
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

            return true;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(SalesOrderManagerUIModel paramModel)
        {
            return true;
        }

        /// <summary>
        /// 【审核-非退货】生成销售出库的库存异动日志
        /// </summary>
        /// <param name="paramStockOutBill">出库单</param>
        /// <param name="paramDetailList">销售订单明细</param>
        /// <param name="paramSalesStockOutDetail">出库明细</param>
        /// <param name="paramInventory">库存</param>
        /// <param name="paramSalesOrder">销售单</param>
        /// <returns></returns>
        private MDLPIS_InventoryTransLog GenerateStockOutInventoryTransLog(MDLPIS_StockOutBill paramStockOutBill, SalesOrderDetailUIModel paramDetailList, SalesStockOutDetailUIModel paramSalesStockOutDetail, MDLPIS_Inventory paramInventory, SalesOrderManagerUIModel paramSalesOrder)
        {
            MDLPIS_InventoryTransLog newInventoryTransLog = new MDLPIS_InventoryTransLog
            {
                ITL_Org_ID = string.IsNullOrEmpty(paramStockOutBill.SOB_Org_ID) ? LoginInfoDAX.OrgID : paramStockOutBill.SOB_Org_ID,
                ITL_WH_ID = paramSalesStockOutDetail.INV_WH_ID,
                ITL_WHB_ID = paramSalesStockOutDetail.INV_WHB_ID,
                //业务单号为[出库单]的单号
                ITL_BusinessNo = paramStockOutBill.SOB_No,
                ITL_Barcode = paramSalesStockOutDetail.INV_Barcode,
                ITL_BatchNo = paramSalesStockOutDetail.INV_BatchNo,
                ITL_Name = paramSalesStockOutDetail.INV_Name,
                ITL_Specification = paramSalesStockOutDetail.INV_Specification,
                //成本价
                ITL_UnitCostPrice = paramSalesStockOutDetail.INV_PurchaseUnitPrice,
                //销价
                ITL_UnitSalePrice = paramDetailList.SOD_UnitPrice,
                //出库，数量为负
                ITL_Qty = -paramSalesStockOutDetail.StockOutQty,
                ITL_AfterTransQty = paramInventory.INV_Qty,
                ITL_IsValid = true,
                ITL_CreatedBy = LoginInfoDAX.UserName,
                ITL_UpdatedBy = LoginInfoDAX.UserName,
                //异动类型为{销售出库}
                ITL_TransType = InventoryTransTypeEnum.Name.XSCK,
                ITL_Source = paramSalesStockOutDetail.WH_Name,
                ITL_Destination = paramSalesOrder.AROrgName,

            };
            return newInventoryTransLog;
        }

        /// <summary>
        /// 【反审核】生成销售出库的库存异动日志
        /// </summary>
        /// <param name="paramStockOutBill">出库单</param>
        /// <param name="paramSalesStockOutDetail">出库明细</param>
        /// <param name="paramInventory">库存</param>
        /// <param name="paramSalesOrderManager">销售单</param>
        /// <param name="paramSalesStockOutDetailList">销售出库单</param>
        /// <returns></returns>
        private MDLPIS_InventoryTransLog GenerateStockOutInventoryTransLogOfUnApprove(MDLPIS_StockOutBill paramStockOutBill, MDLPIS_StockOutBillDetail paramSalesStockOutDetail, MDLPIS_Inventory paramInventory, SalesOrderManagerUIModel paramSalesOrderManager, List<SalesStockOutDetailUIModel> paramSalesStockOutDetailList)
        {
            MDLPIS_InventoryTransLog newInventoryTransLog = new MDLPIS_InventoryTransLog
            {
                ITL_Org_ID = string.IsNullOrEmpty(paramStockOutBill.SOB_Org_ID)
                        ? LoginInfoDAX.OrgID
                        : paramStockOutBill.SOB_Org_ID,
                ITL_WH_ID = paramInventory.INV_WH_ID,
                ITL_WHB_ID = paramInventory.INV_WHB_ID,
                //业务单号为[出库单]的单号
                ITL_BusinessNo = paramStockOutBill.SOB_No,
                ITL_Barcode = paramInventory.INV_Barcode,
                ITL_BatchNo = paramInventory.INV_BatchNo,
                ITL_Name = paramInventory.INV_Name,
                ITL_Specification = paramInventory.INV_Specification,
                ITL_UnitCostPrice = paramInventory.INV_PurchaseUnitPrice,
                ITL_UnitSalePrice = paramSalesStockOutDetail.SOBD_UnitSalePrice,
                //销售出库 反审核时 实际是入库，数量为正
                ITL_Qty = paramSalesStockOutDetail.SOBD_Qty,
                ITL_AfterTransQty = paramInventory.INV_Qty,
                ITL_IsValid = true,
                ITL_CreatedBy = LoginInfoDAX.UserName,
                ITL_CreatedTime = BLLCom.GetCurStdDatetime(),
                ITL_UpdatedBy = LoginInfoDAX.UserName,
                ITL_UpdatedTime = BLLCom.GetCurStdDatetime(),
                //异动类型为{销售出库}
                ITL_TransType = InventoryTransTypeEnum.Name.XSCK,
                ITL_Destination = paramSalesOrderManager.AROrgName,
            };
            if (paramSalesStockOutDetailList.Count > 0)
            {
                foreach (var loopSalesStockOutDetail in paramSalesStockOutDetailList)
                {
                    if (loopSalesStockOutDetail.INV_Barcode == paramSalesStockOutDetail.SOBD_Barcode && loopSalesStockOutDetail.INV_BatchNo == paramSalesStockOutDetail.SOBD_BatchNo)
                    {
                        newInventoryTransLog.ITL_Destination = loopSalesStockOutDetail.WH_Name;
                        break;
                    }
                }
            }

            return newInventoryTransLog;
        }

        /// <summary>
        /// 【审核/核实-退货】生成销售退货入库的库存异动日志
        /// </summary>
        /// <param name="paramStockInBill">入库单</param>
        /// <param name="paramReturnStockInDetail">退货入库明细</param>
        /// <param name="paramInventory">库存</param>
        /// <param name="paramSalesOrderManager">销售单</param>
        /// <returns></returns>
        private MDLPIS_InventoryTransLog GenerateStockInInventoryTransLog(MDLPIS_StockInBill paramStockInBill, ReturnStockInDetailUIModel paramReturnStockInDetail, MDLPIS_Inventory paramInventory, SalesOrderManagerUIModel paramSalesOrderManager)
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
                ITL_Source = paramSalesOrderManager.AROrgName,
                ITL_Destination = paramReturnStockInDetail.WH_Name,
            };
            return newInventoryTransLog;
        }

        #endregion
    }
}
