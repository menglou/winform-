using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.ComModel;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.SD;
using SkyCar.Coeus.UIModel.SD.APModel;
using SkyCar.Coeus.UIModel.SD.QCModel;
using SkyCar.Coeus.UIModel.SD.UIModel;
using SkyCar.Common.Utility;

namespace SkyCar.Coeus.BLL.SD
{
    /// <summary>
    /// 主动销售管理BLL
    /// </summary>
    public class ProactiveSalesManagerBLL : BLLBase
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
        /// 主动销售管理BLL
        /// </summary>
        public ProactiveSalesManagerBLL() : base(Trans.SD)
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
        public bool SaveDetailDS(ProactiveSalesManagerUIModel paramHead, SkyCarBindingList<SalesOrderDetailUIModel, MDLSD_SalesOrderDetail> paramDetailList)
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

            //待更新的[销售预测订单]
            MDLSD_SalesForecastOrder updateSalesForecastOrder = new MDLSD_SalesForecastOrder();

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

            #region 预测销售订单处理

            //[来源类型]为[销售预测]的场合，更新[销售预测订单]的[单据状态]为[已转销售]
            if (argsHead.SO_SourceTypeCode == SalesOrderSourceTypeEnum.Code.XSYC
                && !string.IsNullOrEmpty(paramHead.SO_SourceNo))
            {
                _bll.QueryForObject<MDLSD_SalesForecastOrder, MDLSD_SalesForecastOrder>(new MDLSD_SalesForecastOrder
                {
                    WHERE_SFO_IsValid = true,
                    WHERE_SFO_No = paramHead.SO_SourceNo
                }, updateSalesForecastOrder);
                if (!string.IsNullOrEmpty(updateSalesForecastOrder.SFO_ID))
                {
                    updateSalesForecastOrder.WHERE_SFO_ID = updateSalesForecastOrder.SFO_ID;
                    updateSalesForecastOrder.WHERE_SFO_VersionNo = updateSalesForecastOrder.SFO_VersionNo;
                    updateSalesForecastOrder.SFO_VersionNo++;
                    updateSalesForecastOrder.SFO_StatusCode = SalesForecastOrderStatusEnum.Code.YZXS;
                    updateSalesForecastOrder.SFO_StatusName = SalesForecastOrderStatusEnum.Name.YZXS;
                    updateSalesForecastOrder.SFO_UpdatedBy = LoginInfoDAX.UserName;
                    updateSalesForecastOrder.SFO_UpdatedTime = BLLCom.GetCurStdDatetime();
                }
            }

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

                #region 更新[销售预测订单]

                if (!string.IsNullOrEmpty(updateSalesForecastOrder.SFO_ID))
                {
                    bool updateSalesForecastOrderResult = _bll.Update(updateSalesForecastOrder);
                    if (!updateSalesForecastOrderResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.SD_SalesForecastOrder });
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
            CopyModel(argsHead, paramHead);

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
        /// <param name="paramVenusDbConfig">汽修商数据库连接</param>
        /// <param name="paramDetailList">销售订单明细列表</param>
        /// <param name="paramStockOutDetailList">出库明细列表</param>
        /// <param name="paramIsHasInventory">是否启用进销存模块</param>
        /// <returns></returns>
        public bool ApproveDetailDS(ProactiveSalesManagerUIModel paramHead, string paramVenusDbConfig,
            SkyCarBindingList<SalesOrderDetailUIModel, MDLSD_SalesOrderDetail> paramDetailList, List<SalesStockOutDetailUIModel> paramStockOutDetailList, bool paramIsHasInventory)
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
            //验证汽修商的数据库配置信息
            if (string.IsNullOrEmpty(paramVenusDbConfig))
            {
                //没有获取到汽修商的数据库配置信息，审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { MsgParam.AUTOFACTORY + MsgParam.OF + MsgParam.DATABASE + MsgParam.CONFIHURATION + MsgParam.INFORMATION, SystemActionEnum.Name.APPROVE });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }
            #endregion

            #region 准备数据

            #region 变量定义

            //待更新的[销售订单]
            MDLSD_SalesOrder updateSalesOrder = paramHead.ToTBModelForSaveAndDelete<MDLSD_SalesOrder>();

            //待新增的[应收单]
            MDLFM_AccountReceivableBill newAccountReceivableBill = new MDLFM_AccountReceivableBill();
            //待新增的[应收单明细]列表
            List<MDLFM_AccountReceivableBillDetail> newAccountReceivableBillDetailList = new List<MDLFM_AccountReceivableBillDetail>();

            //待新增的[出库单]
            MDLPIS_StockOutBill newStockOutBill = new MDLPIS_StockOutBill();
            //待新增的[出库单明细]列表
            List<MDLPIS_StockOutBillDetail> newStockOutBillDetailList = new List<MDLPIS_StockOutBillDetail>();

            //待保存的[库存]列表
            List<MDLPIS_Inventory> updateInventoryList = new List<MDLPIS_Inventory>();
            //待新增的[库存异动日志]列表
            List<MDLPIS_InventoryTransLog> newInventoryTransLogList = new List<MDLPIS_InventoryTransLog>();

            #endregion

            #region 更新[销售订单]

            //更新[销售订单]的审核状态为[已审核]，单据状态为[待发货]
            updateSalesOrder.SO_VersionNo++;
            updateSalesOrder.SO_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            updateSalesOrder.SO_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            updateSalesOrder.SO_StatusCode = SalesOrderStatusEnum.Code.DFH;
            updateSalesOrder.SO_StatusName = SalesOrderStatusEnum.Name.DFH;
            updateSalesOrder.SO_UpdatedBy = LoginInfoDAX.UserName;
            updateSalesOrder.SO_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region 创建[应收单]

            //新增[来源类型]为[销售应收]，[单据方向]为[正向]，金额为正，[业务状态]为[已生成]，[审核状态]为[已审核]的[应收单]
            newAccountReceivableBill.ARB_ID = Guid.NewGuid().ToString();
            newAccountReceivableBill.ARB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.ARB);
            newAccountReceivableBill.ARB_BillDirectCode = BillDirectionEnum.Code.PLUS;
            newAccountReceivableBill.ARB_BillDirectName = BillDirectionEnum.Name.PLUS;
            newAccountReceivableBill.ARB_SourceTypeCode = AccountReceivableBillSourceTypeEnum.Code.XSYS;
            newAccountReceivableBill.ARB_SourceTypeName = AccountReceivableBillSourceTypeEnum.Name.XSYS;
            newAccountReceivableBill.ARB_SrcBillNo = paramHead.SO_No;
            newAccountReceivableBill.ARB_Org_ID = LoginInfoDAX.OrgID;
            newAccountReceivableBill.ARB_Org_Name = LoginInfoDAX.OrgShortName;
            if (paramHead.SO_CustomerTypeName == CustomerTypeEnum.Name.PTNQXSH)
            {
                newAccountReceivableBill.ARB_PayObjectTypeName = AmountTransObjectTypeEnum.Name.PLATFORMAUTOFACTORY;
                newAccountReceivableBill.ARB_PayObjectTypeCode = AmountTransObjectTypeEnum.Code.PLATFORMAUTOFACTORY;
            }
            else if (paramHead.SO_CustomerTypeName == CustomerTypeEnum.Name.YBQXSH)
            {
                newAccountReceivableBill.ARB_PayObjectTypeName = AmountTransObjectTypeEnum.Name.GENERALAUTOFACTORY;
                newAccountReceivableBill.ARB_PayObjectTypeCode = AmountTransObjectTypeEnum.Code.GENERALAUTOFACTORY;
            }
            else
            {
                newAccountReceivableBill.ARB_PayObjectTypeName = AmountTransObjectTypeEnum.Name.REGULARCUSTOMER;
                newAccountReceivableBill.ARB_PayObjectTypeCode = AmountTransObjectTypeEnum.Code.REGULARCUSTOMER;
            }
            newAccountReceivableBill.ARB_PayObjectName = paramHead.SO_CustomerName;
            newAccountReceivableBill.ARB_PayObjectID = paramHead.SO_CustomerID;
            newAccountReceivableBill.ARB_AccountReceivableAmount = paramHead.SO_TotalAmount;
            newAccountReceivableBill.ARB_ReceivedAmount = 0;
            newAccountReceivableBill.ARB_UnReceiveAmount = paramHead.SO_TotalAmount;
            newAccountReceivableBill.ARB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.YSC;
            newAccountReceivableBill.ARB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.YSC;
            newAccountReceivableBill.ARB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            newAccountReceivableBill.ARB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            newAccountReceivableBill.ARB_IsValid = true;
            newAccountReceivableBill.ARB_CreatedBy = LoginInfoDAX.UserName;
            newAccountReceivableBill.ARB_CreatedTime = BLLCom.GetCurStdDatetime();
            newAccountReceivableBill.ARB_UpdatedBy = LoginInfoDAX.UserName;
            newAccountReceivableBill.ARB_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            #region 创建[出库单]

            //新增[来源类型]为{销售出库}，[单据状态]为{已完成}，[审核状态]为{已审核}的[出库单]
            newStockOutBill.SOB_ID = Guid.NewGuid().ToString();
            newStockOutBill.SOB_Org_ID = LoginInfoDAX.OrgID;
            newStockOutBill.SOB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.SOB);
            newStockOutBill.SOB_SourceTypeCode = StockOutBillSourceTypeEnum.Code.XSCK;
            newStockOutBill.SOB_SourceTypeName = StockOutBillSourceTypeEnum.Name.XSCK;
            newStockOutBill.SOB_SourceNo = paramHead.SO_No;
            newStockOutBill.SOB_StatusCode = StockOutBillStatusEnum.Code.YWC;
            newStockOutBill.SOB_StatusName = StockOutBillStatusEnum.Name.YWC;
            newStockOutBill.SOB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            newStockOutBill.SOB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            newStockOutBill.SOB_IsValid = true;
            newStockOutBill.SOB_CreatedBy = LoginInfoDAX.UserName;
            newStockOutBill.SOB_CreatedTime = BLLCom.GetCurStdDatetime();
            newStockOutBill.SOB_UpdatedBy = LoginInfoDAX.UserName;
            newStockOutBill.SOB_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            #region 遍历[销售订单明细]列表，更新[销售订单明细]，创建[应收单明细]

            foreach (var loopSalesOrderDetail in paramDetailList)
            {
                if (string.IsNullOrEmpty(loopSalesOrderDetail.SOD_Barcode))
                {
                    continue;
                }

                #region 更新[销售订单明细]

                //更新[销售订单明细]审核状态为[已审核]，单据状态为[待发货]
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
                //生成新的批次号（汽修）
                if (paramHead.IsPrintBarcode && string.IsNullOrEmpty(loopSalesOrderDetail.SOD_BatchNoNew))
                {
                    //单头勾选[打印条码]的场合，生成[配件批次号（汽修）]
                    loopSalesOrderDetail.SOD_BatchNoNew = BLLCom.GetBatchNoFromVenusDb(paramVenusDbConfig, new MDLAPM_Inventory
                    {
                        INV_Org_ID = loopSalesOrderDetail.SOD_StockInOrgID,
                        INV_Barcode = loopSalesOrderDetail.SOD_Barcode
                    });
                    //获取批次号失败
                    if (string.IsNullOrEmpty(loopSalesOrderDetail.SOD_BatchNoNew))
                    {
                        ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { MsgParam.BARCODE + MsgParam.BE + loopSalesOrderDetail.SOD_Barcode + MsgParam.OF + MsgParam.BATCHNO, SystemActionEnum.Name.APPROVE });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 创建[应收单明细]

                //新增[来源类型]为[销售应收]，[是否负向明细]为[false]，金额为正，[业务状态]和[审核状态]与单头一致的[应收单明细]
                MDLFM_AccountReceivableBillDetail newAccountReceivableBillDetail = new MDLFM_AccountReceivableBillDetail
                {
                    ARBD_ID = Guid.NewGuid().ToString(),
                    ARBD_ARB_ID = newAccountReceivableBill.ARB_ID,
                    ARBD_IsMinusDetail = false,
                    ARBD_SrcBillNo = paramHead.SO_No,
                    ARBD_SrcBillDetailID = loopSalesOrderDetail.SOD_ID,
                    ARBD_Org_ID = newAccountReceivableBill.ARB_Org_ID,
                    ARBD_Org_Name = newAccountReceivableBill.ARB_Org_Name,
                    ARBD_AccountReceivableAmount = loopSalesOrderDetail.SOD_TotalAmount,
                    ARBD_ReceivedAmount = 0,
                    ARBD_UnReceiveAmount = loopSalesOrderDetail.SOD_TotalAmount,
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
                            SOBD_SOB_ID = newStockOutBill.SOB_ID,
                            SOBD_SOB_No = newStockOutBill.SOB_No,
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
                        newStockOutBillDetailList.Add(newStockOutBillDetail);

                        #endregion

                        #region 更新[库存]，生成[库存异动日志]

                        //[出库明细]列表中第一次出现该配件
                        //查询该配件是否在[库存]中存在
                        MDLPIS_Inventory resultInventory = new MDLPIS_Inventory();
                        QueryForObject<MDLPIS_Inventory, MDLPIS_Inventory>(new MDLPIS_Inventory
                        {
                            WHERE_INV_Org_ID = newStockOutBill.SOB_Org_ID,
                            WHERE_INV_Barcode = loopStockOutDetail.INV_Barcode,
                            WHERE_INV_BatchNo = loopStockOutDetail.INV_BatchNo,
                            WHERE_INV_WH_ID = loopStockOutDetail.INV_WH_ID,
                            WHERE_INV_IsValid = true,
                        }, resultInventory);

                        //[库存]中不存在该配件
                        if (string.IsNullOrEmpty(resultInventory.INV_ID))
                        {
                            //配件：loopStockOutDetail.INV_Name(条形码：loopStockOutDetail.INV_Barcode)的库存不存在，审核失败
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                            { loopStockOutDetail.INV_Name, loopStockOutDetail.INV_Barcode, MsgParam.NOTEXIST, SystemActionEnum.Name.APPROVE });
                            LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                            return false;
                        }

                        if (resultInventory.INV_Qty < loopStockOutDetail.StockOutQty)
                        {
                            //配件：loopStockOutDetail.INV_Name(条形码：loopStockOutDetail.INV_Barcode)的库存不足，审核失败
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                            { loopStockOutDetail.INV_Name, loopStockOutDetail.INV_Barcode, MsgParam.SHORTAGE, SystemActionEnum.Name.APPROVE });
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
                        updateInventoryList.Add(resultInventory);

                        //生成[库存异动日志]
                        newInventoryTransLogList.Add(GenerateStockOutInventoryTransLogOfApprove(newStockOutBill, loopSalesOrderDetail, loopStockOutDetail, resultInventory, paramHead));

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
                        SOBD_SOB_ID = newStockOutBill.SOB_ID,
                        SOBD_SOB_No = newStockOutBill.SOB_No,
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
                    newStockOutBillDetailList.Add(newStockOutBillDetail);
                    #endregion
                }
            }

            #endregion

            #region Venus相关

            //单据的[来源类型]是“主动销售”或者单据的[来源类型]是“销售预测”且对应[销售预测订单].[来源类型]不是“汽修商采购”的场合，
            //需向对应汽修商的Venus系统中新建对应的“配件预定订单”，同时需要检查配件档案及相关信息是存在，
            //若不存在则需要新建对应档案信息（配件名称，配件档案，车辆品牌车系，配件级别，配件类别）

            //是否需要写入Venus的数据
            bool isWriteVenusData = false;
            if (updateSalesOrder.SO_SourceTypeName == SalesOrderSourceTypeEnum.Name.ZDXS)
            {
                //单据的[来源类型]是“主动销售” 的场合
                isWriteVenusData = true;
            }
            else if (updateSalesOrder.SO_SourceTypeName == SalesOrderSourceTypeEnum.Name.XSYC &&
                 !string.IsNullOrEmpty(updateSalesOrder.SO_SourceNo))
            {
                //单据的[来源类型]是“销售预测”且对应[销售预测订单].[来源类型]不是“汽修商采购”的场合
                MDLSD_SalesForecastOrder resultSalesForecastOrder = new MDLSD_SalesForecastOrder();
                _bll.QueryForObject<MDLSD_SalesForecastOrder, MDLSD_SalesForecastOrder>(new MDLSD_SalesForecastOrder
                {
                    WHERE_SFO_IsValid = true,
                    WHERE_SFO_No = updateSalesOrder.SO_SourceNo
                }, resultSalesForecastOrder);
                if (!string.IsNullOrEmpty(resultSalesForecastOrder.SFO_ID))
                {
                    if (resultSalesForecastOrder.SFO_SourceTypeCode == SalesForecastOrderSourceTypeEnum.Code.XSMB
                        ||
                        resultSalesForecastOrder.SFO_SourceTypeCode == SalesForecastOrderSourceTypeEnum.Code.QXAQKC)
                    {
                        isWriteVenusData = true;
                    }
                }
            }

            //待新增的配件采购订单列表
            List<MDLAPM_PurchaseOrder> insertPurchaseOrderList = new List<MDLAPM_PurchaseOrder>();
            //待新增的供应商列表
            List<MDLAPM_Supplier> insertSupplierList = new List<MDLAPM_Supplier>();
            //待新增的配件名称列表
            List<MDLAPM_AutoPartsName> insertAutoPartsNameList = new List<MDLAPM_AutoPartsName>();
            //待新增的配件档案列表
            List<MDLAPM_AutoPartsArchive> insertAutoPartsArchiveList = new List<MDLAPM_AutoPartsArchive>();
            //待新增的配件采购订单明细列表
            List<MDLAPM_PurchaseOrderDetail> insertPurchaseOrderDetailList = new List<MDLAPM_PurchaseOrderDetail>();
            //待新增的车辆品牌车系列表
            List<MDLSCON_VehicleBrandInspireSumma> insertVehicleBrandInspireSummaList = new List<MDLSCON_VehicleBrandInspireSumma>();
            //待新增的码表数据列表（类型为[配件级别]）
            List<MDLSCON_CodeTable> insertCodeTableList = new List<MDLSCON_CodeTable>();
            //带新增的配件类别
            List<MDLAPM_AutoPartsType> insertAutoPartsTypeList = new List<MDLAPM_AutoPartsType>();

            if (isWriteVenusData)
            {
                #region Venus相关表处理

                //获取当前商户下的有效组织
                List<MDLSM_Organization> resultEffectiveOrgList = new List<MDLSM_Organization>();
                bool isGetEffectiveOrg = BLLCom.QueryAutoFactoryCustomerOrgList(paramVenusDbConfig,
                    resultEffectiveOrgList);
                if (!isGetEffectiveOrg || resultEffectiveOrgList.Count == 0)
                {
                    //没有获取到汽修商的有效组织信息
                    ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { MsgParam.AUTOFACTORY + MsgParam.OF + MsgParam.VALID + MsgParam.ORGNIZATION + MsgParam.INFORMATION, SystemActionEnum.Name.APPROVE });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                //组织不同，生成多条采购单
                var resultVenusOrgList =
                    paramDetailList.Where(x => !string.IsNullOrEmpty(x.SOD_StockInOrgID))
                        .Select(x => new { x.SOD_StockInOrgID, x.SOD_StockInOrgCode })
                        .Distinct()
                        .ToList();
                if (resultVenusOrgList.Count > 0)
                {
                    foreach (var loopVenusOrg in resultVenusOrgList)
                    {
                        //判断组织是否是真的有效
                        bool orgIsEffective = resultEffectiveOrgList.Any(p => p.Org_ID == loopVenusOrg.SOD_StockInOrgID);
                        if (!orgIsEffective)
                        {
                            continue;
                        }

                        #region Venus-配件采购订单

                        //获取采购订单号
                        string purchaseOrderNo;
                        if (!string.IsNullOrEmpty(updateSalesOrder.SO_SourceNo))
                        {
                            //[销售订单]有[来源单号]的场合，使用[销售订单]的[来源单号]作为Venus中[配件采购订单]的单号
                            purchaseOrderNo = updateSalesOrder.SO_SourceNo;
                        }
                        else
                        {
                            //[销售订单]无[来源单号]的场合，Venus中[配件采购订单]的单号通过接口生成，并回写[销售订单]的[来源单号]
                            purchaseOrderNo = BLLCom.GetVenusDocumentNo(paramVenusDbConfig, loopVenusOrg.SOD_StockInOrgCode, SysConst.APM_PurchaseOrder);
                            updateSalesOrder.SO_SourceNo = purchaseOrderNo;
                        }
                        if (string.IsNullOrEmpty(purchaseOrderNo))
                        {
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, "获取采购订单编号失败，请联系云车工作人员，重新创建当前商户！" });
                            LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                            return false;
                        }
                        decimal? purchaseOrderAmcount = 0;
                        foreach (var loopSalesOrderDetail in paramDetailList)
                        {
                            if (string.IsNullOrEmpty(loopSalesOrderDetail.SOD_StockInOrgID)
                                || loopSalesOrderDetail.SOD_StockInOrgID != loopVenusOrg.SOD_StockInOrgID)
                            {
                                continue;
                            }
                            purchaseOrderAmcount += Math.Round((loopSalesOrderDetail.SOD_Qty ?? 0) *
                                                    (loopSalesOrderDetail.SOD_UnitPrice ?? 0), 2);

                        }
                        var insertPurchaseOrder = new MDLAPM_PurchaseOrder
                        {
                            PO_ID = Guid.NewGuid().ToString(),
                            PO_No = purchaseOrderNo,
                            PO_Status = "已预定",
                            PO_SourceType = "供应商",
                            PO_Org_ID = loopVenusOrg.SOD_StockInOrgID,
                            PO_Amount = purchaseOrderAmcount,
                            PO_IsValid = true,
                            PO_CreatedBy = string.IsNullOrEmpty(paramHead.Org_ShortName) ? LoginInfoDAX.OrgShortName : paramHead.Org_ShortName,
                            PO_UpdatedBy = string.IsNullOrEmpty(paramHead.Org_ShortName) ? LoginInfoDAX.OrgShortName : paramHead.Org_ShortName,
                            PO_VersionNo = 1,

                        };
                        insertPurchaseOrderList.Add(insertPurchaseOrder);

                        #endregion

                        #region Venus-供应商
                        
                        //查询供应商在Venus系统中是否存在
                        var resultVenusSupplier = new SupplierVenusUIModel();
                        DBManager.QueryForObject(paramVenusDbConfig, SQLID.SD_ProactiveSales_SQL04, new SupplierVenusUIModel
                        {
                            SUPP_Code = LoginInfoDAX.OrgCode,
                            SUPP_MerchantCode = LoginInfoDAX.MCTCode,
                        }, resultVenusSupplier);

                        string supplierId = string.Empty;
                        if (string.IsNullOrEmpty(resultVenusSupplier.SUPP_ID))
                        {
                            //获取当前登录组织信息
                            var curLoginOrgInfo = new MDLSM_Organization();
                            _bll.QueryForObject<MDLSM_Organization, MDLSM_Organization>(new MDLSM_Organization
                            {
                                WHERE_Org_Code = LoginInfoDAX.OrgCode,
                            }, curLoginOrgInfo);

                            if (curLoginOrgInfo != null && !string.IsNullOrEmpty(curLoginOrgInfo.Org_ID))
                            {
                                #region Venus-新增供应商

                                var insertSupplier = new MDLAPM_Supplier
                                {
                                    SUPP_ID = Guid.NewGuid().ToString(),
                                    SUPP_MerchantCode = LoginInfoDAX.MCTCode,
                                    SUPP_MerchantName = LoginInfoDAX.MCTName,
                                    //因供应商管理平台无此字段，所以参照Venus的[MerchantSourceTypeEnum]暂用固定字符（Platform-平台）
                                    SUPP_SourceTypeCode = "Platform",
                                    SUPP_SourceTypeName = "平台",
                                    //供应商名称：当前登录组织全称
                                    SUPP_Name = LoginInfoDAX.OrgFullName,
                                    SUPP_Code = LoginInfoDAX.OrgCode,
                                    SUPP_Contacter = curLoginOrgInfo.Org_Contacter,
                                    SUPP_Tel = curLoginOrgInfo.Org_TEL,
                                    SUPP_Phone = curLoginOrgInfo.Org_PhoneNo,
                                    SUPP_MainAutoParts = curLoginOrgInfo.Org_MainProducts,
                                    SUPP_Addreess = curLoginOrgInfo.Org_Addr,
                                    SUPP_IsValid = true
                                };
                                insertSupplier.SUPP_CreatedBy = insertSupplier.SUPP_UpdatedBy = string.IsNullOrEmpty(paramHead.Org_ShortName) ? LoginInfoDAX.OrgShortName : paramHead.Org_ShortName;
                                insertSupplier.SUPP_CreatedTime = insertSupplier.SUPP_UpdatedTime = BLLCom.GetCurStdDatetime();
                                insertSupplier.SUPP_VersionNo = 1;

                                insertSupplierList.Add(insertSupplier);
                                supplierId = insertSupplier.SUPP_ID;

                                #endregion
                            }
                        }
                        else
                        {
                            supplierId = resultVenusSupplier.SUPP_ID;
                        }
                        
                        #endregion

                        #region Venus-采购订单明细，供应商，配件名称，配件档案，车辆品牌车系，配件级别，配件类别

                        foreach (var loopSalesOrderDetail in paramDetailList)
                        {
                            if (string.IsNullOrEmpty(loopSalesOrderDetail.SOD_StockInOrgID)
                                || loopSalesOrderDetail.SOD_StockInOrgID != loopVenusOrg.SOD_StockInOrgID)
                            {
                                continue;
                            }

                            #region Venus配件类别
                            
                            if (!string.IsNullOrEmpty(loopSalesOrderDetail.APT_Name))
                            {
                                bool repeat = false;
                                foreach (var loopAutoPartsType in insertAutoPartsTypeList)
                                {
                                    if (loopAutoPartsType.APT_Name == loopSalesOrderDetail.APT_Name)
                                    {
                                        repeat = true;
                                        break;
                                    }
                                }
                                if (!repeat)
                                {
                                    //新增Venus配件类别信息
                                    var insertVenusAutoPartsType = new MDLAPM_AutoPartsType
                                    {
                                        APT_Name = loopSalesOrderDetail.APT_Name,
                                        APT_IsValid = true,
                                        APT_CreatedBy = string.IsNullOrEmpty(paramHead.Org_ShortName) ? LoginInfoDAX.OrgShortName : paramHead.Org_ShortName,
                                        APT_UpdatedBy = string.IsNullOrEmpty(paramHead.Org_ShortName) ? LoginInfoDAX.OrgShortName : paramHead.Org_ShortName,
                                        APT_CreatedTime = BLLCom.GetCurStdDatetime(),
                                        APT_UpdatedTime = BLLCom.GetCurStdDatetime(),
                                        APT_VersionNo = 1
                                    };
                                    insertAutoPartsTypeList.Add(insertVenusAutoPartsType);
                                }
                            }
                            #endregion

                            #region Venus-配件名称
                            
                            if (!string.IsNullOrEmpty(loopSalesOrderDetail.SOD_Name))
                            {
                                bool repeat = false;
                                foreach (var loopAutoPartsName in insertAutoPartsNameList)
                                {
                                    if (loopAutoPartsName.APN_Name == loopSalesOrderDetail.SOD_Name)
                                    {
                                        repeat = true;
                                        break;
                                    }
                                }
                                if (!repeat)
                                {
                                    #region 查询配件名称信息（在Venus系统中新增配件名称档案用）

                                    var resultAutoPartsNameList = new List<AutoPartsNameUIModel>();
                                    _bll.QueryForList<AutoPartsNameUIModel>(SQLID.SD_ProactiveSales_SQL22, new MDLBS_AutoPartsName
                                    {
                                        WHERE_APN_Name = loopSalesOrderDetail.SOD_Name,
                                        WHERE_APN_IsValid = true
                                    }, resultAutoPartsNameList);

                                    #endregion

                                    if (resultAutoPartsNameList.Count == 1)
                                    {
                                        #region Venus-新增配件名称

                                        var insertAutoPartsName = new MDLAPM_AutoPartsName
                                        {
                                            APN_Name = loopSalesOrderDetail.SOD_Name,
                                            APN_Alias = resultAutoPartsNameList[0].APN_Alias ?? string.Empty,
                                            APN_NameSpellCode = resultAutoPartsNameList[0].APN_NameSpellCode ?? string.Empty,
                                            APN_AliasSpellCode = resultAutoPartsNameList[0].APN_AliasSpellCode ?? string.Empty,
                                            APN_APT_ID = resultAutoPartsNameList[0].APN_APT_ID ?? string.Empty,
                                            APN_APT_Name = resultAutoPartsNameList[0].APT_Name ?? string.Empty,
                                            APN_SlackDays = resultAutoPartsNameList[0].APN_SlackDays == null
                                                    ? 0
                                                    : Convert.ToInt32(resultAutoPartsNameList[0].APN_SlackDays.ToString()),
                                            APN_UOM = resultAutoPartsNameList[0].APN_UOM ?? string.Empty,
                                            APN_FixUOM = resultAutoPartsNameList[0].APN_FixUOM != null && Convert.ToBoolean(resultAutoPartsNameList[0].APN_FixUOM.ToString()),
                                            APN_IsSuitableForApp = resultAutoPartsNameList[0].APN_IsSuitableForApp != null && Convert.ToBoolean(resultAutoPartsNameList[0].APN_IsSuitableForApp.ToString()),
                                            APN_IsValid = true,
                                            APN_CreatedBy = string.IsNullOrEmpty(paramHead.Org_ShortName) ? LoginInfoDAX.OrgShortName : paramHead.Org_ShortName,
                                            APN_UpdatedBy = string.IsNullOrEmpty(paramHead.Org_ShortName) ? LoginInfoDAX.OrgShortName : paramHead.Org_ShortName,
                                            APN_CreatedTime = BLLCom.GetCurStdDatetime(),
                                            APN_UpdatedTime = BLLCom.GetCurStdDatetime(),
                                            APN_VersionNo = 1,
                                        };
                                        insertAutoPartsNameList.Add(insertAutoPartsName);
                                        #endregion
                                    }
                                }
                            }
                            #endregion

                            #region Venus-配件档案
                            if (!string.IsNullOrEmpty(loopSalesOrderDetail.SOD_Barcode))
                            {
                                #region Venus-新增配件档案

                                bool repeat = false;
                                foreach (var loopAutoPartsArchive in insertAutoPartsArchiveList)
                                {
                                    if (loopAutoPartsArchive.APA_Barcode == loopSalesOrderDetail.SOD_Barcode)
                                    {
                                        repeat = true;
                                        break;
                                    }
                                }
                                if (!repeat)
                                {
                                    var insertAutoPartsArchive = new MDLAPM_AutoPartsArchive
                                    {
                                        APA_Barcode = loopSalesOrderDetail.SOD_Barcode,
                                        APA_OEMNo = loopSalesOrderDetail.INV_OEMNo,
                                        APA_ThirdNo = loopSalesOrderDetail.INV_ThirdNo,
                                        APA_Name = loopSalesOrderDetail.SOD_Name,
                                        APA_Brand = loopSalesOrderDetail.APA_Brand,
                                        APA_Specification = loopSalesOrderDetail.SOD_Specification,
                                        APA_Level = loopSalesOrderDetail.APA_Level,
                                        APA_UOM = loopSalesOrderDetail.SOD_UOM,
                                        APA_VehicleBrand = loopSalesOrderDetail.APA_VehicleBrand,
                                        APA_VehicleInspire = loopSalesOrderDetail.APA_VehicleInspire,
                                        APA_VehicleCapacity = loopSalesOrderDetail.APA_VehicleCapacity,
                                        APA_VehicleYearModel = loopSalesOrderDetail.APA_VehicleYearModel,
                                        APA_VehicleGearboxType = loopSalesOrderDetail.APA_VehicleGearboxTypeName,
                                        APA_SUPP_ID = supplierId,
                                        APA_IsValid = true,
                                        APA_CreatedBy = string.IsNullOrEmpty(paramHead.Org_ShortName) ? LoginInfoDAX.OrgShortName : paramHead.Org_ShortName,
                                        APA_UpdatedBy = string.IsNullOrEmpty(paramHead.Org_ShortName) ? LoginInfoDAX.OrgShortName : paramHead.Org_ShortName,
                                        APA_CreatedTime = BLLCom.GetCurStdDatetime(),
                                        APA_UpdatedTime = BLLCom.GetCurStdDatetime(),
                                        APA_VersionNo = 1
                                    };
                                    insertAutoPartsArchiveList.Add(insertAutoPartsArchive);
                                }
                                #endregion
                            }
                            #endregion

                            #region Venus-采购订单明细

                            var insertPurchaseOrderDetail = new MDLAPM_PurchaseOrderDetail
                            {
                                POD_AutoPartsBarcode = loopSalesOrderDetail.SOD_Barcode,
                                POD_AutoPartsBatchNo = loopSalesOrderDetail.SOD_BatchNoNew,
                                POD_ID = Guid.NewGuid().ToString(),
                                POD_PO_No = purchaseOrderNo,
                                POD_OEMCode = loopSalesOrderDetail.INV_OEMNo,
                                POD_ThirdCode = loopSalesOrderDetail.INV_ThirdNo,
                                POD_AutoPartsName = loopSalesOrderDetail.SOD_Name,
                                POD_AutoPartsBrand = loopSalesOrderDetail.APA_Brand,
                                POD_AutoPartsSpec = loopSalesOrderDetail.SOD_Specification,
                                POD_AutoPartsLevel = loopSalesOrderDetail.APA_Level,
                                POD_UOM = loopSalesOrderDetail.SOD_UOM,
                                POD_VehicleBrand = loopSalesOrderDetail.APA_VehicleBrand,
                                POD_VehicleInspire = loopSalesOrderDetail.APA_VehicleInspire,
                                POD_VehicleCapacity = loopSalesOrderDetail.APA_VehicleCapacity,
                                POD_VehicleYearModel = loopSalesOrderDetail.APA_VehicleYearModel,
                                POD_VehicleGearboxType = loopSalesOrderDetail.APA_VehicleGearboxTypeName,
                                //汽配商户
                                POD_SUPP_MerchantCode = LoginInfoDAX.MCTCode,
                                POD_SUPP_MerchantName = LoginInfoDAX.MCTName,
                                //供应商
                                POD_SUPP_ID = supplierId,
                                //仓库
                                POD_WH_ID = loopSalesOrderDetail.SOD_StockInWarehouseID,
                                //仓位
                                POD_WHB_ID = loopSalesOrderDetail.SOD_StockInBinID,
                                POD_OrderQuantity = loopSalesOrderDetail.SOD_Qty ?? 0,
                                POD_UnitPrice = loopSalesOrderDetail.SOD_UnitPrice ?? 0,
                                POD_Status = "已预定",
                                POD_IsValid = true,
                                POD_CreatedBy = string.IsNullOrEmpty(paramHead.Org_ShortName) ? LoginInfoDAX.OrgShortName : paramHead.Org_ShortName,
                                POD_UpdatedBy = string.IsNullOrEmpty(paramHead.Org_ShortName) ? LoginInfoDAX.OrgShortName : paramHead.Org_ShortName,
                                POD_CreatedTime = BLLCom.GetCurStdDatetime(),
                                POD_UpdatedTime = BLLCom.GetCurStdDatetime(),
                                POD_VersionNo = 1
                            };
                            insertPurchaseOrderDetailList.Add(insertPurchaseOrderDetail);
                            #endregion

                            #region Venus车辆品牌车系
                            if (!string.IsNullOrEmpty(loopSalesOrderDetail.APA_VehicleBrand)
                                && !string.IsNullOrEmpty(loopSalesOrderDetail.APA_VehicleInspire))
                            {
                                //查询Coeus中的车辆品牌车系信息
                                List<MDLBS_VehicleBrandInspireSumma> resultVehicleBrandInspireList =
                                    new List<MDLBS_VehicleBrandInspireSumma>();
                                _bll.QueryForList(SQLID.SD_ProactiveSales_SQL21, new MDLBS_VehicleBrandInspireSumma
                                {
                                    WHERE_VBIS_Brand = loopSalesOrderDetail.APA_VehicleBrand,
                                    WHERE_VBIS_Inspire = loopSalesOrderDetail.APA_VehicleInspire,
                                }, resultVehicleBrandInspireList);

                                //组合的车辆品牌车系字符串
                                string vehicleBrandInspireStr = string.Empty;
                                if (resultVehicleBrandInspireList.Count > 0)
                                {
                                    foreach (var loopVehicleBrandInspire in resultVehicleBrandInspireList)
                                    {
                                        vehicleBrandInspireStr += loopVehicleBrandInspire.VBIS_Brand + ";" +
                                                                  loopVehicleBrandInspire.VBIS_Inspire + "|";
                                    }
                                }

                                //查询车辆品牌车系信息在Venus中是否存在，获取不存在的车辆品牌车系
                                List<MDLSCON_VehicleBrandInspireSumma> resultVehicleBrandInspireToInsertList = new List<MDLSCON_VehicleBrandInspireSumma>();
                                DBManager.QueryForList<MDLSCON_VehicleBrandInspireSumma>(paramVenusDbConfig, SQLID.SD_ProactiveSales_SQL14, vehicleBrandInspireStr, resultVehicleBrandInspireToInsertList);

                                if (resultVehicleBrandInspireToInsertList.Count > 0)
                                {
                                    foreach (var loopVehicleBrandInspire in resultVehicleBrandInspireToInsertList)
                                    {
                                        //新增Venus车辆品牌车系信息
                                        var insertVenusVehicleBrandInspire = new MDLSCON_VehicleBrandInspireSumma
                                        {
                                            VBIS_Brand = loopVehicleBrandInspire.VBIS_Brand,
                                            VBIS_Inspire = loopVehicleBrandInspire.VBIS_Inspire,
                                            VBIS_BrandSpellCode = ChineseSpellCode.GetShortSpellCode(loopVehicleBrandInspire.VBIS_Brand),
                                            VBIS_InspireSpellCode = ChineseSpellCode.GetShortSpellCode(loopVehicleBrandInspire.VBIS_Inspire),
                                            VBIS_IsValid = true,
                                            VBIS_CreatedBy = string.IsNullOrEmpty(paramHead.Org_ShortName) ? LoginInfoDAX.OrgShortName : paramHead.Org_ShortName,
                                            VBIS_UpdatedBy = string.IsNullOrEmpty(paramHead.Org_ShortName) ? LoginInfoDAX.OrgShortName : paramHead.Org_ShortName,
                                            VBIS_CreatedTime = BLLCom.GetCurStdDatetime(),
                                            VBIS_UpdatedTime = BLLCom.GetCurStdDatetime(),
                                            VBIS_VersionNo = 1
                                        };
                                        insertVehicleBrandInspireSummaList.Add(insertVenusVehicleBrandInspire);
                                    }
                                }
                            }
                            
                            #endregion

                            #region Venus码表（类型为[配件级别]）
                            if (!string.IsNullOrEmpty(loopSalesOrderDetail.APA_Level))
                            {
                                bool repeat = false;
                                foreach (var loopCodeTable in insertCodeTableList)
                                {
                                    if (loopCodeTable.CT_Type == CodeTypeEnum.Code.AUTOPARTSLEVEL
                                        && loopCodeTable.CT_Value == loopSalesOrderDetail.APA_Level)
                                    {
                                        repeat = true;
                                        break;
                                    }
                                }
                                if (!repeat)
                                {
                                    var insertVenusCodeTable = new MDLSCON_CodeTable
                                    {
                                        CT_Type = CodeTypeEnum.Code.AUTOPARTSLEVEL,
                                        CT_Name = loopSalesOrderDetail.APA_Level,
                                        CT_Value = loopSalesOrderDetail.APA_Level,
                                        CT_Desc = loopSalesOrderDetail.APA_Level,
                                        CT_IsValid = true,
                                        CT_CreatedBy = string.IsNullOrEmpty(paramHead.Org_ShortName) ? LoginInfoDAX.OrgShortName : paramHead.Org_ShortName,
                                        CT_UpdatedBy = string.IsNullOrEmpty(paramHead.Org_ShortName) ? LoginInfoDAX.OrgShortName : paramHead.Org_ShortName,
                                        CT_CreatedTime = BLLCom.GetCurStdDatetime(),
                                        CT_UpdatedTime = BLLCom.GetCurStdDatetime(),
                                        CT_VersionNo = 1
                                    };
                                    insertCodeTableList.Add(insertVenusCodeTable);
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
                #endregion
            }
            #endregion

            #endregion

            #region 带事务的新增和更新

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);
                DBManager.BeginTransaction(paramVenusDbConfig);

                #region 更新[销售订单]
                bool updateSalesOrderResult = _bll.Save(updateSalesOrder);
                if (!updateSalesOrderResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    DBManager.RollBackTransaction(paramVenusDbConfig);
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
                    DBManager.RollBackTransaction(paramVenusDbConfig);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_SalesOrderDetail });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 新增[应收单]

                if (!string.IsNullOrEmpty(newAccountReceivableBill.ARB_ID))
                {
                    bool insertAccountReceivableBillResult = _bll.Insert(newAccountReceivableBill);
                    if (!insertAccountReceivableBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        DBManager.RollBackTransaction(paramVenusDbConfig);
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
                        DBManager.RollBackTransaction(paramVenusDbConfig);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.FM_AccountReceivableBillDetail });
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
                        DBManager.RollBackTransaction(paramVenusDbConfig);
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
                        DBManager.RollBackTransaction(paramVenusDbConfig);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.PIS_InventoryTransLog });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 新增[出库单]

                if (!string.IsNullOrEmpty(newStockOutBill.SOB_ID))
                {
                    bool insertStockOutBillResult = _bll.Insert(newStockOutBill);
                    if (!insertStockOutBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        DBManager.RollBackTransaction(paramVenusDbConfig);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.PIS_StockOutBill });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 新增[出库单明细]

                if (newStockOutBillDetailList.Count > 0)
                {
                    bool insertStockOutBillDetailResult = _bll.InsertByList<MDLPIS_StockOutBillDetail, MDLPIS_StockOutBillDetail>(newStockOutBillDetailList);
                    if (!insertStockOutBillDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        DBManager.RollBackTransaction(paramVenusDbConfig);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.PIS_StockOutBillDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region Venus相关数据表的保存

                if (isWriteVenusData)
                {
                    //传入的待新增的[采购订单]DataTable
                    SyncVenusDataWhenProactiveSalesDataSet.PurchaseOrderDataTable purchaseOrderDataTable = new SyncVenusDataWhenProactiveSalesDataSet.PurchaseOrderDataTable();
                    //传入的待新增的[采购订单明细]DataTable
                    SyncVenusDataWhenProactiveSalesDataSet.PurchaseOrderDetailDataTable purchaseOrderDetailDataTable = new SyncVenusDataWhenProactiveSalesDataSet.PurchaseOrderDetailDataTable();
                    //传入的待新增的[供应商]DataTable
                    SyncVenusDataWhenProactiveSalesDataSet.SupplierDataTable supplierDataTable = new SyncVenusDataWhenProactiveSalesDataSet.SupplierDataTable();
                    //传入的待新增的[配件名称]DataTable
                    SyncVenusDataWhenProactiveSalesDataSet.AutoPartsNameDataTable autoPartsNameDataTable = new SyncVenusDataWhenProactiveSalesDataSet.AutoPartsNameDataTable();
                    //传入的待新增的[配件档案]DataTable
                    SyncVenusDataWhenProactiveSalesDataSet.AutoPartsArchiveDataTable autoPartsArchiveDataTable = new SyncVenusDataWhenProactiveSalesDataSet.AutoPartsArchiveDataTable();
                    //传入的待新增的[配件类别]DataTable
                    SyncVenusDataWhenProactiveSalesDataSet.AutoPartsTypeDataTable autoPartsTypeDataTable = new SyncVenusDataWhenProactiveSalesDataSet.AutoPartsTypeDataTable();
                    //传入的待新增的[车辆品牌车系大全]DataTable
                    SyncVenusDataWhenProactiveSalesDataSet.VehicleBrandInspireSummaDataTable vehicleBrandInspireSummaDataTable = new SyncVenusDataWhenProactiveSalesDataSet.VehicleBrandInspireSummaDataTable();
                    //传入的带新增的[码表]DataTable
                    SyncVenusDataWhenProactiveSalesDataSet.CodeTableDataTable codeTableDataTable = new SyncVenusDataWhenProactiveSalesDataSet.CodeTableDataTable();

                    #region Venus-配件采购订单
                    foreach (var loopPurchaseOrder in insertPurchaseOrderList)
                    {
                        if (string.IsNullOrEmpty(loopPurchaseOrder.PO_No))
                        {
                            continue;
                        }
                        SyncVenusDataWhenProactiveSalesDataSet.PurchaseOrderRow newPurchaseOrderRow = purchaseOrderDataTable.NewPurchaseOrderRow();
                        newPurchaseOrderRow.PO_No = loopPurchaseOrder.PO_No;
                        newPurchaseOrderRow.PO_Status = loopPurchaseOrder.PO_Status;
                        newPurchaseOrderRow.PO_SourceType = loopPurchaseOrder.PO_SourceType;
                        newPurchaseOrderRow.PO_Org_ID = loopPurchaseOrder.PO_Org_ID;
                        newPurchaseOrderRow.PO_Amount = (loopPurchaseOrder.PO_Amount ?? 0);
                        newPurchaseOrderRow.PO_IsValid = loopPurchaseOrder.PO_IsValid.Value;
                        newPurchaseOrderRow.PO_CreatedBy = loopPurchaseOrder.PO_CreatedBy;
                        newPurchaseOrderRow.PO_UpdatedBy = loopPurchaseOrder.PO_UpdatedBy;
                        newPurchaseOrderRow.PO_VersionNo = loopPurchaseOrder.PO_VersionNo.Value;

                        purchaseOrderDataTable.AddPurchaseOrderRow(newPurchaseOrderRow);
                    }
                    #endregion

                    #region Venus-配件采购订单明细
                    foreach (var loopPurchaseOrderDetail in insertPurchaseOrderDetailList)
                    {
                        if (string.IsNullOrEmpty(loopPurchaseOrderDetail.POD_AutoPartsName))
                        {
                            continue;
                        }
                        SyncVenusDataWhenProactiveSalesDataSet.PurchaseOrderDetailRow newPurchaseOrderDetailRow =
                            purchaseOrderDetailDataTable.NewPurchaseOrderDetailRow();
                        newPurchaseOrderDetailRow.POD_PO_No = loopPurchaseOrderDetail.POD_PO_No;
                        newPurchaseOrderDetailRow.POD_AutoPartsBarcode = loopPurchaseOrderDetail.POD_AutoPartsBarcode;
                        newPurchaseOrderDetailRow.POD_AutoPartsBatchNo = loopPurchaseOrderDetail.POD_AutoPartsBatchNo;
                        newPurchaseOrderDetailRow.POD_ThirdCode = loopPurchaseOrderDetail.POD_ThirdCode;
                        newPurchaseOrderDetailRow.POD_OEMCode = loopPurchaseOrderDetail.POD_OEMCode;
                        newPurchaseOrderDetailRow.POD_AutoPartsName = loopPurchaseOrderDetail.POD_AutoPartsName;
                        newPurchaseOrderDetailRow.POD_AutoPartsBrand = loopPurchaseOrderDetail.POD_AutoPartsBrand;
                        newPurchaseOrderDetailRow.POD_AutoPartsSpec = loopPurchaseOrderDetail.POD_AutoPartsSpec;
                        newPurchaseOrderDetailRow.POD_AutoPartsLevel = loopPurchaseOrderDetail.POD_AutoPartsLevel;
                        newPurchaseOrderDetailRow.POD_UOM = loopPurchaseOrderDetail.POD_UOM;
                        newPurchaseOrderDetailRow.POD_VehicleBrand = loopPurchaseOrderDetail.POD_VehicleBrand;
                        newPurchaseOrderDetailRow.POD_VehicleInspire = loopPurchaseOrderDetail.POD_VehicleInspire;
                        newPurchaseOrderDetailRow.POD_VehicleCapacity = loopPurchaseOrderDetail.POD_VehicleCapacity;
                        newPurchaseOrderDetailRow.POD_VehicleYearModel = loopPurchaseOrderDetail.POD_VehicleYearModel;
                        newPurchaseOrderDetailRow.POD_VehicleGearboxType = loopPurchaseOrderDetail.POD_VehicleGearboxType;
                        newPurchaseOrderDetailRow.POD_SUPP_ID = loopPurchaseOrderDetail.POD_SUPP_ID;
                        newPurchaseOrderDetailRow.POD_SUPP_MerchantCode = loopPurchaseOrderDetail.POD_SUPP_MerchantCode;
                        newPurchaseOrderDetailRow.POD_SUPP_MerchantName = loopPurchaseOrderDetail.POD_SUPP_MerchantName;
                        newPurchaseOrderDetailRow.POD_OrderQuantity = (loopPurchaseOrderDetail.POD_OrderQuantity ?? 0);
                        newPurchaseOrderDetailRow.POD_UnitPrice = (loopPurchaseOrderDetail.POD_UnitPrice ?? 0);
                        newPurchaseOrderDetailRow.POD_Status = loopPurchaseOrderDetail.POD_Status;
                        newPurchaseOrderDetailRow.POD_IsValid = loopPurchaseOrderDetail.POD_IsValid.Value;
                        newPurchaseOrderDetailRow.POD_CreatedBy = loopPurchaseOrderDetail.POD_CreatedBy;
                        newPurchaseOrderDetailRow.POD_UpdatedBy = loopPurchaseOrderDetail.POD_UpdatedBy;
                        newPurchaseOrderDetailRow.POD_VersionNo = (loopPurchaseOrderDetail.POD_VersionNo ?? 1);

                        purchaseOrderDetailDataTable.AddPurchaseOrderDetailRow(newPurchaseOrderDetailRow);
                    }
                    #endregion

                    #region Venus-供应商
                    foreach (var loopSupplier in insertSupplierList)
                    {
                        if (string.IsNullOrEmpty(loopSupplier.SUPP_Name))
                        {
                            continue;
                        }
                        SyncVenusDataWhenProactiveSalesDataSet.SupplierRow newSupplierRow = supplierDataTable.NewSupplierRow();
                        newSupplierRow.SUPP_ID = loopSupplier.SUPP_ID;
                        newSupplierRow.SUPP_MerchantCode = loopSupplier.SUPP_MerchantCode;
                        newSupplierRow.SUPP_MerchantName = loopSupplier.SUPP_MerchantName;
                        newSupplierRow.SUPP_SourceTypeCode = loopSupplier.SUPP_SourceTypeCode;
                        newSupplierRow.SUPP_SourceTypeName = loopSupplier.SUPP_SourceTypeName;
                        newSupplierRow.SUPP_Name = loopSupplier.SUPP_Name;
                        newSupplierRow.SUPP_Code = loopSupplier.SUPP_Code;
                        newSupplierRow.SUPP_Contacter = loopSupplier.SUPP_Contacter;
                        newSupplierRow.SUPP_Tel = loopSupplier.SUPP_Tel;
                        newSupplierRow.SUPP_Phone = loopSupplier.SUPP_Phone;
                        newSupplierRow.SUPP_MainAutoParts = loopSupplier.SUPP_MainAutoParts;
                        newSupplierRow.SUPP_Addreess = loopSupplier.SUPP_Addreess;
                        newSupplierRow.SUPP_IsValid = loopSupplier.SUPP_IsValid.Value;
                        newSupplierRow.SUPP_CreatedBy = loopSupplier.SUPP_CreatedBy;
                        newSupplierRow.SUPP_UpdatedBy = loopSupplier.SUPP_UpdatedBy;
                        newSupplierRow.SUPP_VersionNo = loopSupplier.SUPP_VersionNo.Value;

                        supplierDataTable.AddSupplierRow(newSupplierRow);
                    }
                    #endregion

                    #region Venus-配件名称
                    foreach (var loopAutoPartsName in insertAutoPartsNameList)
                    {
                        if (string.IsNullOrEmpty(loopAutoPartsName.APN_Name))
                        {
                            continue;
                        }
                        SyncVenusDataWhenProactiveSalesDataSet.AutoPartsNameRow newAutoPartsNameRow = autoPartsNameDataTable.NewAutoPartsNameRow();
                        newAutoPartsNameRow.APN_Name = loopAutoPartsName.APN_Name;
                        newAutoPartsNameRow.APN_Alias = loopAutoPartsName.APN_Alias;
                        newAutoPartsNameRow.APN_NameSpellCode = loopAutoPartsName.APN_NameSpellCode;
                        newAutoPartsNameRow.APN_AliasSpellCode = loopAutoPartsName.APN_AliasSpellCode;
                        newAutoPartsNameRow.APN_APT_ID = loopAutoPartsName.APN_APT_ID;
                        newAutoPartsNameRow.APN_APT_Name = loopAutoPartsName.APN_APT_Name;
                        newAutoPartsNameRow.APN_SlackDays = (loopAutoPartsName.APN_SlackDays ?? 0);
                        newAutoPartsNameRow.APN_UOM = loopAutoPartsName.APN_UOM;
                        newAutoPartsNameRow.APN_FixUOM = loopAutoPartsName.APN_FixUOM.Value;
                        newAutoPartsNameRow.APN_IsSuitableForApp = loopAutoPartsName.APN_IsSuitableForApp.Value;
                        newAutoPartsNameRow.APN_IsValid = loopAutoPartsName.APN_IsValid.Value;
                        newAutoPartsNameRow.APN_CreatedBy = loopAutoPartsName.APN_CreatedBy;
                        newAutoPartsNameRow.APN_UpdatedBy = loopAutoPartsName.APN_UpdatedBy;
                        newAutoPartsNameRow.APN_VersionNo = (loopAutoPartsName.APN_VersionNo ?? 0);

                        autoPartsNameDataTable.AddAutoPartsNameRow(newAutoPartsNameRow);
                    }
                    #endregion

                    #region Venus-配件档案
                    foreach (var loopAutoPartsArchive in insertAutoPartsArchiveList)
                    {
                        if (string.IsNullOrEmpty(loopAutoPartsArchive.APA_Barcode))
                        {
                            continue;
                        }
                        SyncVenusDataWhenProactiveSalesDataSet.AutoPartsArchiveRow newAutoPartsArchiveRow =
                            autoPartsArchiveDataTable.NewAutoPartsArchiveRow();
                        newAutoPartsArchiveRow.APA_Barcode = loopAutoPartsArchive.APA_Barcode;
                        newAutoPartsArchiveRow.APA_OEMNo = loopAutoPartsArchive.APA_OEMNo;
                        newAutoPartsArchiveRow.APA_ThirdNo = loopAutoPartsArchive.APA_ThirdNo;
                        newAutoPartsArchiveRow.APA_Name = loopAutoPartsArchive.APA_Name;
                        newAutoPartsArchiveRow.APA_Brand = loopAutoPartsArchive.APA_Brand;
                        newAutoPartsArchiveRow.APA_Specification = loopAutoPartsArchive.APA_Specification;
                        newAutoPartsArchiveRow.APA_UOM = loopAutoPartsArchive.APA_UOM;
                        newAutoPartsArchiveRow.APA_Level = loopAutoPartsArchive.APA_Level;
                        newAutoPartsArchiveRow.APA_VehicleBrand = loopAutoPartsArchive.APA_VehicleBrand;
                        newAutoPartsArchiveRow.APA_VehicleInspire = loopAutoPartsArchive.APA_VehicleInspire;
                        newAutoPartsArchiveRow.APA_VehicleCapacity = loopAutoPartsArchive.APA_VehicleCapacity;
                        newAutoPartsArchiveRow.APA_VehicleYearModel = loopAutoPartsArchive.APA_VehicleYearModel;
                        newAutoPartsArchiveRow.APA_VehicleGearboxType = loopAutoPartsArchive.APA_VehicleGearboxType;
                        newAutoPartsArchiveRow.APA_SUPP_ID = loopAutoPartsArchive.APA_SUPP_ID;
                        newAutoPartsArchiveRow.APA_IsValid = loopAutoPartsArchive.APA_IsValid.Value;
                        newAutoPartsArchiveRow.APA_CreatedBy = loopAutoPartsArchive.APA_CreatedBy;
                        newAutoPartsArchiveRow.APA_UpdatedBy = loopAutoPartsArchive.APA_UpdatedBy;
                        newAutoPartsArchiveRow.APA_VersionNo = loopAutoPartsArchive.APA_VersionNo.Value;

                        autoPartsArchiveDataTable.AddAutoPartsArchiveRow(newAutoPartsArchiveRow);
                    }
                    #endregion

                    #region Venus-配件类别
                    foreach (var loopAutoPartsType in insertAutoPartsTypeList)
                    {
                        if (string.IsNullOrEmpty(loopAutoPartsType.APT_Name))
                        {
                            continue;
                        }
                        SyncVenusDataWhenProactiveSalesDataSet.AutoPartsTypeRow newAutoPartsTypeRow =
                            autoPartsTypeDataTable.NewAutoPartsTypeRow();
                        newAutoPartsTypeRow.APT_Name = loopAutoPartsType.APT_Name;
                        newAutoPartsTypeRow.APT_CreatedBy = loopAutoPartsType.APT_CreatedBy;
                        newAutoPartsTypeRow.APT_UpdatedBy = loopAutoPartsType.APT_UpdatedBy;
                        newAutoPartsTypeRow.APT_VersionNo = (loopAutoPartsType.APT_VersionNo ?? 1);

                        autoPartsTypeDataTable.AddAutoPartsTypeRow(newAutoPartsTypeRow);
                    }
                    #endregion

                    #region Venus-车辆品牌车系大全
                    foreach (var loopVehicleBrandInspireSumma in insertVehicleBrandInspireSummaList)
                    {
                        if (string.IsNullOrEmpty(loopVehicleBrandInspireSumma.VBIS_Brand))
                        {
                            continue;
                        }
                        SyncVenusDataWhenProactiveSalesDataSet.VehicleBrandInspireSummaRow newVehicleBrandInspireSummaRow =
                            vehicleBrandInspireSummaDataTable.NewVehicleBrandInspireSummaRow();
                        newVehicleBrandInspireSummaRow.VBIS_Org_Name = loopVehicleBrandInspireSumma.VBIS_Org_Name;
                        newVehicleBrandInspireSummaRow.VBIS_Brand = loopVehicleBrandInspireSumma.VBIS_Brand;
                        newVehicleBrandInspireSummaRow.VBIS_Inspire = loopVehicleBrandInspireSumma.VBIS_Inspire;
                        newVehicleBrandInspireSummaRow.VBIS_Model = loopVehicleBrandInspireSumma.VBIS_Model;
                        newVehicleBrandInspireSummaRow.VBIS_ManHourPriceDiff = (loopVehicleBrandInspireSumma.VBIS_ManHourPriceDiff ?? 0);
                        newVehicleBrandInspireSummaRow.VBIS_ManHourPriceRatio = (loopVehicleBrandInspireSumma.VBIS_ManHourPriceRatio ?? 0);
                        newVehicleBrandInspireSummaRow.VBIS_BrandSpellCode = loopVehicleBrandInspireSumma.VBIS_BrandSpellCode;
                        newVehicleBrandInspireSummaRow.VBIS_InspireSpellCode = loopVehicleBrandInspireSumma.VBIS_InspireSpellCode;
                        newVehicleBrandInspireSummaRow.VBIS_IsValid = loopVehicleBrandInspireSumma.VBIS_IsValid.Value;
                        newVehicleBrandInspireSummaRow.VBIS_CreatedBy = loopVehicleBrandInspireSumma.VBIS_CreatedBy;
                        newVehicleBrandInspireSummaRow.VBIS_UpdatedBy = loopVehicleBrandInspireSumma.VBIS_UpdatedBy;
                        newVehicleBrandInspireSummaRow.VBIS_VersionNo = (loopVehicleBrandInspireSumma.VBIS_VersionNo ?? 0);

                        vehicleBrandInspireSummaDataTable.AddVehicleBrandInspireSummaRow(newVehicleBrandInspireSummaRow);
                    }
                    #endregion

                    #region Venus-码表（类型为[配件级别]）
                    foreach (var loopCodeTable in insertCodeTableList)
                    {
                        if (string.IsNullOrEmpty(loopCodeTable.CT_Type))
                        {
                            continue;
                        }
                        SyncVenusDataWhenProactiveSalesDataSet.CodeTableRow newCodeTableRow =
                            codeTableDataTable.NewCodeTableRow();
                        newCodeTableRow.CT_Type = loopCodeTable.CT_Type;
                        newCodeTableRow.CT_Name = loopCodeTable.CT_Name;
                        newCodeTableRow.CT_Value = loopCodeTable.CT_Value;
                        newCodeTableRow.CT_Desc = loopCodeTable.CT_Desc;
                        newCodeTableRow.CT_IsValid = loopCodeTable.CT_IsValid.Value;
                        newCodeTableRow.CT_CreatedBy = loopCodeTable.CT_CreatedBy;
                        newCodeTableRow.CT_UpdatedBy = loopCodeTable.CT_UpdatedBy;
                        newCodeTableRow.CT_VersionNo = (loopCodeTable.CT_VersionNo ?? 1);

                        codeTableDataTable.AddCodeTableRow(newCodeTableRow);
                    }
                    #endregion

                    //创建SqlConnection数据库连接对象
                    SqlConnection sqlCon = new SqlConnection
                    {
                        ConnectionString = DBManager.GetConnectionString(paramVenusDbConfig)
                    };
                    //打开数据库连接
                    sqlCon.Open();
                    //创建并初始化SqlCommand对象
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlCon;
                    
                    cmd.CommandText = "P_CoeusSD_SyncProactiveSalesInfo";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PurchaseOrderList", SqlDbType.Structured);
                    cmd.Parameters[0].Value = purchaseOrderDataTable;
                    cmd.Parameters.Add("@PurchaseOrderDetailList", SqlDbType.Structured);
                    cmd.Parameters[1].Value = purchaseOrderDetailDataTable;
                    cmd.Parameters.Add("@SupplierList", SqlDbType.Structured);
                    cmd.Parameters[2].Value = supplierDataTable;
                    cmd.Parameters.Add("@AutoPartsNameList", SqlDbType.Structured);
                    cmd.Parameters[3].Value = autoPartsNameDataTable;
                    cmd.Parameters.Add("@AutoPartsArchiveList", SqlDbType.Structured);
                    cmd.Parameters[4].Value = autoPartsArchiveDataTable;
                    cmd.Parameters.Add("@AutoPartsTypeList", SqlDbType.Structured);
                    cmd.Parameters[5].Value = autoPartsTypeDataTable;
                    cmd.Parameters.Add("@VehicleBrandInspireSummaList", SqlDbType.Structured);
                    cmd.Parameters[6].Value = vehicleBrandInspireSummaDataTable;
                    cmd.Parameters.Add("@CodeTableList", SqlDbType.Structured);
                    cmd.Parameters[7].Value = codeTableDataTable;
                    cmd.ExecuteNonQuery();
                }
                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
                DBManager.CommitTransaction(paramVenusDbConfig);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                DBManager.RollBackTransaction(paramVenusDbConfig);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.APPROVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            #endregion

            //将最新数据回写给DetailDS
            CopyModel(updateSalesOrder, paramHead);

            return true;
        }

        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="paramHead">销售订单</param>
        /// <param name="paramVenusDbConfig">汽修商数据库连接</param>
        /// <param name="paramDetailList">销售订单明细列表</param>
        /// <param name="paramIsHasInventory">是否启用进销存模块</param>
        /// <param name="paramStockOutDetailList">出库明细列表</param>
        /// <returns></returns>
        public bool UnApproveDetailDS(ProactiveSalesManagerUIModel paramHead, string paramVenusDbConfig, SkyCarBindingList<SalesOrderDetailUIModel, MDLSD_SalesOrderDetail> paramDetailList, bool paramIsHasInventory, List<SalesStockOutDetailUIModel> paramStockOutDetailList)
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
            //验证汽修商的数据库配置信息
            if (string.IsNullOrEmpty(paramVenusDbConfig))
            {
                //没有获取到汽修商的数据库配置信息，反审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { MsgParam.AUTOFACTORY + MsgParam.OF + MsgParam.DATABASE + MsgParam.CONFIHURATION + MsgParam.INFORMATION, SystemActionEnum.Name.UNAPPROVE });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }
            #endregion

            #region 准备数据

            #region 变量定义

            //待更新的[销售订单]
            MDLSD_SalesOrder updateSalesOrder = paramHead.ToTBModelForSaveAndDelete<MDLSD_SalesOrder>();
            //待更新的[销售订单明细]列表
            List<MDLSD_SalesOrderDetail> updateSalesOrderDetailList = new List<MDLSD_SalesOrderDetail>();

            //待删除的[应收单]
            MDLFM_AccountReceivableBill deleteAccountReceivableBill = new MDLFM_AccountReceivableBill();
            //待删除的[应收单明细]列表
            List<MDLFM_AccountReceivableBillDetail> deleteAccountReceivableBillDetailList = new List<MDLFM_AccountReceivableBillDetail>();

            //待删除的[出库单]
            MDLPIS_StockOutBill deleteStockOutBill = new MDLPIS_StockOutBill();
            //待删除的[出库单明细]列表
            List<MDLPIS_StockOutBillDetail> deleteStockOutBillDetailList = new List<MDLPIS_StockOutBillDetail>();

            //待新增的[库存异动日志]列表
            List<MDLPIS_InventoryTransLog> newInventoryTransLogList = new List<MDLPIS_InventoryTransLog>();
            //待更新的[库存]列表
            List<MDLPIS_Inventory> updateInventoryList = new List<MDLPIS_Inventory>();

            //待删除的Venus[配件采购订单]列表
            List<MDLAPM_PurchaseOrder> deleteVenusPurchaseOrderList = new List<MDLAPM_PurchaseOrder>();
            //待删除的Venus[配件采购订单明细]列表
            List<MDLAPM_PurchaseOrderDetail> deleteVenusPurchaseOrderDetailList = new List<MDLAPM_PurchaseOrderDetail>();
            //待删除的Venus[系统编号]列表
            List<MDLSM_SystemNo> deleteVenusSystemNoList = new List<MDLSM_SystemNo>();

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

            CopyModelList(paramDetailList, updateSalesOrderDetailList);

            foreach (var loopSalesOrderDetail in updateSalesOrderDetailList)
            {
                if (string.IsNullOrEmpty(loopSalesOrderDetail.SOD_ID))
                {
                    continue;
                }

                // 更新[销售订单明细]审核状态为[待审核]，单据状态为[已生成]，清空批次号(供汽修商使用)
                loopSalesOrderDetail.WHERE_SOD_ID = loopSalesOrderDetail.SOD_ID;
                loopSalesOrderDetail.WHERE_SOD_VersionNo = loopSalesOrderDetail.SOD_VersionNo;
                loopSalesOrderDetail.SOD_VersionNo++;
                loopSalesOrderDetail.SOD_ApprovalStatusCode = updateSalesOrder.SO_ApprovalStatusCode;
                loopSalesOrderDetail.SOD_ApprovalStatusName = updateSalesOrder.SO_ApprovalStatusName;
                loopSalesOrderDetail.SOD_StatusCode = updateSalesOrder.SO_StatusCode;
                loopSalesOrderDetail.SOD_StatusName = updateSalesOrder.SO_StatusName;
                loopSalesOrderDetail.SOD_BatchNoNew = "";
                loopSalesOrderDetail.SOD_UpdatedBy = LoginInfoDAX.UserName;
                loopSalesOrderDetail.SOD_UpdatedTime = BLLCom.GetCurStdDatetime();
            }
            #endregion

            #region 获取待删除的[应收单]

            _bll.QueryForObject<MDLFM_AccountReceivableBill, MDLFM_AccountReceivableBill>(new MDLFM_AccountReceivableBill
            {
                WHERE_ARB_SrcBillNo = updateSalesOrder.SO_No,
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
                    WHERE_ARBD_SrcBillNo = updateSalesOrder.SO_No,
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

            #region 获取待删除的[出库单]

            _bll.QueryForObject<MDLPIS_StockOutBill, MDLPIS_StockOutBill>(new MDLPIS_StockOutBill
            {
                WHERE_SOB_SourceTypeName = StockOutBillSourceTypeEnum.Name.XSCK,
                WHERE_SOB_SourceNo = updateSalesOrder.SO_No,
                WHERE_SOB_IsValid = true
            }, deleteStockOutBill);
            deleteStockOutBill.WHERE_SOB_ID = deleteStockOutBill.SOB_ID;

            #endregion

            #region 获取待删除的[出库单明细]列表

            if (!string.IsNullOrEmpty(deleteStockOutBill.SOB_ID))
            {
                _bll.QueryForList<MDLPIS_StockOutBillDetail, MDLPIS_StockOutBillDetail>(new MDLPIS_StockOutBillDetail
                {
                    WHERE_SOBD_SOB_ID = deleteStockOutBill.SOB_ID,
                    WHERE_SOBD_SOB_No = deleteStockOutBill.SOB_No,
                    WHERE_SOBD_IsValid = true
                }, deleteStockOutBillDetailList);
            }
            foreach (var loopStockOutBillDetail in deleteStockOutBillDetailList)
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
                        WHERE_INV_Org_ID = deleteStockOutBill.SOB_Org_ID,
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
                    newInventoryTransLogList.Add(GenerateStockOutInventoryTransLogOfUnApprove(deleteStockOutBill, loopStockOutBillDetail, resultInventory, paramHead, paramStockOutDetailList));
                }
                #endregion
            }
            #endregion

            #region Venus相关

            //单据的[来源类型]是“主动销售”或者单据的[来源类型]是“销售预测”且对应[销售预测订单].[来源类型]不是“汽修商采购”的场合，
            //需删除汽修商的Venus系统中对应的“配件预定订单”

            //是否需要删除Venus的数据
            bool isDeleteVenusData = false;

            if (updateSalesOrder.SO_SourceTypeName == SalesOrderSourceTypeEnum.Name.ZDXS)
            {
                //单据的[来源类型]是“主动销售” 的场合
                isDeleteVenusData = true;
            }
            else if (!string.IsNullOrEmpty(updateSalesOrder.SO_SourceNo))
            {
                //单据的[来源类型]是“销售预测”且对应[销售预测订单].[来源类型]不是“汽修商采购”的场合
                MDLSD_SalesForecastOrder resultSalesForecastOrder = new MDLSD_SalesForecastOrder();
                _bll.QueryForObject<MDLSD_SalesForecastOrder, MDLSD_SalesForecastOrder>(new MDLSD_SalesForecastOrder
                {
                    WHERE_SFO_IsValid = true,
                    WHERE_SFO_No = updateSalesOrder.SO_SourceNo
                }, resultSalesForecastOrder);
                if (!string.IsNullOrEmpty(resultSalesForecastOrder.SFO_ID))
                {
                    if (resultSalesForecastOrder.SFO_SourceTypeCode == SalesForecastOrderSourceTypeEnum.Code.XSMB
                        ||
                        resultSalesForecastOrder.SFO_SourceTypeCode == SalesForecastOrderSourceTypeEnum.Code.QXAQKC)
                    {
                        isDeleteVenusData = true;
                    }
                }
            }

            //需要删除Venus数据的场合，删除[配件采购订单]，删除[配件采购订单明细]列表
            if (isDeleteVenusData)
            {
                var orgIdList = paramDetailList.Select(x => x.SOD_StockInOrgID).Distinct().ToList();
                foreach (var loopOrgId in orgIdList)
                {
                    if (string.IsNullOrEmpty(loopOrgId) || string.IsNullOrEmpty(updateSalesOrder.SO_SourceNo))
                    {
                        continue;
                    }
                    #region 获取待删除的[配件采购订单]

                    MDLAPM_PurchaseOrder deletePurchaseOrder = new MDLAPM_PurchaseOrder();
                    DBManager.QueryForObject(paramVenusDbConfig, SQLID.SD_ProactiveSales_SQL09, new MDLAPM_PurchaseOrder
                    {
                        PO_SourceType = "供应商",
                        PO_Org_ID = loopOrgId,
                        PO_No = updateSalesOrder.SO_SourceNo
                    }, deletePurchaseOrder);
                    deleteVenusPurchaseOrderList.Add(deletePurchaseOrder);

                    #endregion

                    #region 获取待删除的[系统编号]

                    MDLSM_SystemNo deleteSystemNo = new MDLSM_SystemNo();
                    DBManager.QueryForObject(paramVenusDbConfig, SQLID.SD_ProactiveSales_SQL17, new MDLSM_SystemNo
                    {
                        WHERE_SN_Value = updateSalesOrder.SO_SourceNo
                    }, deleteSystemNo);
                    deleteSystemNo.WHERE_SN_Value = deleteSystemNo.SN_Value;
                    deleteVenusSystemNoList.Add(deleteSystemNo);

                    #endregion
                }
                foreach (var loopVenusPurchaseOrder in deleteVenusPurchaseOrderList)
                {
                    if (string.IsNullOrEmpty(loopVenusPurchaseOrder.PO_No))
                    {
                        continue;
                    }
                    #region 获取待删除的[配件采购订单明细]

                    List<MDLAPM_PurchaseOrderDetail> resultPurchaseOrderDetailList = new List<MDLAPM_PurchaseOrderDetail>();
                    DBManager.QueryForList<MDLAPM_PurchaseOrderDetail>(paramVenusDbConfig, SQLID.SD_ProactiveSales_SQL10, new MDLAPM_PurchaseOrderDetail
                    {
                        POD_PO_No = loopVenusPurchaseOrder.PO_No
                    }, resultPurchaseOrderDetailList);
                    deleteVenusPurchaseOrderDetailList.AddRange(resultPurchaseOrderDetailList);

                    #endregion
                }
                //清除[销售订单].[来源单号]
                updateSalesOrder.SO_SourceNo = string.Empty;
            }
            #endregion

            #endregion

            #region 带事务的保存和删除

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);
                DBManager.BeginTransaction(paramVenusDbConfig);

                #region 更新[销售订单]

                bool updateSalesOrderResult = _bll.Update(SQLID.SD_ProactiveSales_SQL19, updateSalesOrder);
                if (!updateSalesOrderResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    DBManager.RollBackTransaction(paramVenusDbConfig);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.SD_SalesOrder });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 更新[销售订单明细]

                foreach (var loopSalesOrderDetail in updateSalesOrderDetailList)
                {
                    bool updateSalesOrderDetailResult = _bll.Update(SQLID.SD_ProactiveSales_SQL20, loopSalesOrderDetail);
                    if (!updateSalesOrderDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        DBManager.RollBackTransaction(paramVenusDbConfig);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_SalesOrderDetail });
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
                        DBManager.RollBackTransaction(paramVenusDbConfig);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.FM_AccountReceivableBill });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #endregion

                #region 删除[应收单明细]列表

                if (deleteAccountReceivableBillDetailList.Count > 0)
                {
                    var deleteAccountReceivableBillDetailResult = _bll.DeleteByList<MDLFM_AccountReceivableBillDetail, MDLFM_AccountReceivableBillDetail>(deleteAccountReceivableBillDetailList);
                    if (!deleteAccountReceivableBillDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        DBManager.RollBackTransaction(paramVenusDbConfig);
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
                        DBManager.RollBackTransaction(paramVenusDbConfig);
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
                        DBManager.RollBackTransaction(paramVenusDbConfig);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.PIS_InventoryTransLog });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 删除[出库单]

                if (!string.IsNullOrEmpty(deleteStockOutBill.SOB_ID))
                {
                    var deleteStockOutBillResult = _bll.Delete<MDLPIS_StockOutBill>(deleteStockOutBill);
                    if (!deleteStockOutBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        DBManager.RollBackTransaction(paramVenusDbConfig);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.PIS_StockOutBill });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #endregion

                #region 删除[出库单明细]列表

                if (deleteStockOutBillDetailList.Count > 0)
                {
                    var deleteStockOutBillDetailResult = _bll.DeleteByList<MDLPIS_StockOutBillDetail, MDLPIS_StockOutBillDetail>(deleteStockOutBillDetailList);
                    if (!deleteStockOutBillDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        DBManager.RollBackTransaction(paramVenusDbConfig);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + SystemTableEnums.Name.PIS_StockOutBillDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #endregion

                #region 删除Venus的[配件采购订单]列表

                foreach (var loopDeleteVenusPurchaseOrder in deleteVenusPurchaseOrderList)
                {
                    var deleteVenusPurchaseOrderResult = DBManager.Delete(paramVenusDbConfig, SQLID.SD_ProactiveSales_SQL11, loopDeleteVenusPurchaseOrder);
                    if (deleteVenusPurchaseOrderResult == 0)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        DBManager.RollBackTransaction(paramVenusDbConfig);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + MsgParam.AUTOFACTORY + MsgParam.OF + SystemTableEnums.Name.PIS_PurchaseOrder });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 删除Venus的[配件采购订单明细]列表

                foreach (var loopDeleteVenusPurchaseOrderDetail in deleteVenusPurchaseOrderDetailList)
                {
                    var deleteVenusPurchaseOrderDetailResult = DBManager.Delete(paramVenusDbConfig, SQLID.SD_ProactiveSales_SQL12, loopDeleteVenusPurchaseOrderDetail);
                    if (deleteVenusPurchaseOrderDetailResult == 0)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        DBManager.RollBackTransaction(paramVenusDbConfig);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + MsgParam.AUTOFACTORY + MsgParam.OF + SystemTableEnums.Name.PIS_PurchaseOrderDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }

                #region 删除Venus的[系统编号]列表

                foreach (var loopDeleteVenusSystemNo in deleteVenusSystemNoList)
                {
                    var deleteVenusSystemNoResult = DBManager.Delete(paramVenusDbConfig, SQLID.SD_ProactiveSales_SQL18, loopDeleteVenusSystemNo);
                    if (deleteVenusSystemNoResult == 0)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        DBManager.RollBackTransaction(paramVenusDbConfig);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE + MsgParam.AUTOFACTORY + MsgParam.OF + SystemTableEnums.Name.SM_SystemNo });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
                DBManager.CommitTransaction(paramVenusDbConfig);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                DBManager.RollBackTransaction(paramVenusDbConfig);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.UNAPPROVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ex.Message, "", null);
                return false;
            }

            #endregion

            //将最新数据回写给DetailDS
            CopyModel(updateSalesOrder, paramHead);
            if (paramHead.SO_VersionNo == null)
            {
                paramHead.SO_VersionNo = 1;
            }
            else
            {
                paramHead.SO_VersionNo += 1;
            }

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
        public bool VerifyDetailDS(ProactiveSalesManagerUIModel paramHead, SkyCarBindingList<SalesOrderDetailUIModel, MDLSD_SalesOrderDetail> paramDetailList, List<SalesReturnDetailUIModel> paramSalesOrderReturnDetailList, List<ReturnStockInDetailUIModel> paramReturnStockInDetailList, List<SalesOrderDetailUIModel> paramSalesOrderLoseDetailList)
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
            //待更新的[应收单]
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
                //来源类型为{主动销售退货}
                newRejectSalesOrder.SO_SourceTypeName = SalesOrderSourceTypeEnum.Name.ZDXSTH;
                newRejectSalesOrder.SO_SourceTypeCode = SalesOrderSourceTypeEnum.Code.ZDXSTH;
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
                        ARBD_AccountReceivableAmount = newReturnSalesOrderDetail.SOD_TotalAmount,
                        ARBD_ReceivedAmount = 0,
                        ARBD_UnReceiveAmount = -newReturnSalesOrderDetail.SOD_TotalAmount,
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
                    newStockInDetail.SID_SourceDetailID = loopReturnStockInDetail.SID_SourceDetailID;
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
                        newInventoryTransLogList.Add(GenerateStockInInventoryTransLogOfVerify(newReturnStockInBill, loopReturnStockInDetail, inventoryExists, paramHead));
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
                            newInventoryTransLogList.Add(GenerateStockInInventoryTransLogOfVerify(newReturnStockInBill, loopReturnStockInDetail, inventoryToInsert, paramHead));
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
                            newInventoryTransLogList.Add(GenerateStockInInventoryTransLogOfVerify(newReturnStockInBill, loopReturnStockInDetail, resultInventory, paramHead));
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
                newLoseAccountReceivableBill.ARB_PayObjectTypeCode = AmountTransObjectTypeEnum.Name.DELIVERYMAN;
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

                #region 更新对应的[物流单明细]列表

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

            #region 核实汽修配件预定单(非关键逻辑，不需要事务)
            if (updateCurSalesOrder.SO_SourceTypeName == SalesOrderSourceTypeEnum.Name.ZDXS &&
                !string.IsNullOrEmpty(updateCurSalesOrder.SO_CustomerID) &&
                !string.IsNullOrEmpty(updateCurSalesOrder.SO_SourceNo))
            {

                MDLPIS_AutoFactoryCustomer resultAutoFactoryCustomer = new MDLPIS_AutoFactoryCustomer();
                _bll.QueryForObject<MDLPIS_AutoFactoryCustomer, MDLPIS_AutoFactoryCustomer>(new MDLPIS_AutoFactoryCustomer
                {
                    WHERE_AFC_ID = updateCurSalesOrder.SO_CustomerID
                }, resultAutoFactoryCustomer);
                if (!string.IsNullOrEmpty(resultAutoFactoryCustomer.AFC_ID))
                {
                    string argsUrl = ApiUrl.BF0035Url;
                    string argsPostData = string.Format(ApiParameter.BF0035, LoginInfoDAX.MCTCode, SysConst.ProductCode, resultAutoFactoryCustomer.AFC_Code, updateCurSalesOrder.SO_SourceNo);
                    string strApiData = APIDataHelper.GetAPIData(argsUrl, argsPostData);

                    var jsonResult = (JObject)JsonConvert.DeserializeObject(strApiData);
                    if (jsonResult != null)
                    {
                        if (jsonResult["ResultCode"] != null && jsonResult["ResultCode"].ToString().Equals("I0001"))
                        {

                        }
                    }
                }
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
        private bool ServerCheck(ProactiveSalesManagerUIModel paramModel)
        {
            return true;
        }

        /// <summary>
        /// 【审核】生成销售出库的库存异动日志
        /// </summary>
        /// <param name="paramStockOutBill">出库单</param>
        /// <param name="paramDetailList">销售订单明细</param>
        /// <param name="paramSalesStockOutDetail">出库明细</param>
        /// <param name="paramInventory">库存</param>
        /// <param name="paramProactiveSalesManager">销售单</param>
        /// <returns></returns>
        private MDLPIS_InventoryTransLog GenerateStockOutInventoryTransLogOfApprove(MDLPIS_StockOutBill paramStockOutBill, SalesOrderDetailUIModel paramDetailList, SalesStockOutDetailUIModel paramSalesStockOutDetail, MDLPIS_Inventory paramInventory, ProactiveSalesManagerUIModel paramProactiveSalesManager)
        {
            MDLPIS_InventoryTransLog newInventoryTransLog = new MDLPIS_InventoryTransLog
            {
                ITL_Org_ID =
                    string.IsNullOrEmpty(paramStockOutBill.SOB_Org_ID)
                        ? LoginInfoDAX.OrgID
                        : paramStockOutBill.SOB_Org_ID,
                ITL_WH_ID = paramSalesStockOutDetail.INV_WH_ID,
                ITL_WHB_ID = paramSalesStockOutDetail.INV_WHB_ID,
                //业务单号为[出库单]的单号
                ITL_BusinessNo = paramStockOutBill.SOB_No,
                ITL_Barcode = paramSalesStockOutDetail.INV_Barcode,
                ITL_BatchNo = paramSalesStockOutDetail.INV_BatchNo,
                ITL_Name = paramSalesStockOutDetail.INV_Name,
                ITL_Specification = paramSalesStockOutDetail.INV_Specification,
                ITL_UnitCostPrice = paramSalesStockOutDetail.INV_PurchaseUnitPrice,
                ITL_UnitSalePrice = paramDetailList.SOD_UnitPrice,
                //销售出库，数量为负
                ITL_Qty = -paramSalesStockOutDetail.StockOutQty,
                ITL_AfterTransQty = paramInventory.INV_Qty,
                ITL_IsValid = true,
                ITL_CreatedBy = LoginInfoDAX.UserName,
                ITL_CreatedTime = BLLCom.GetCurStdDatetime(),
                ITL_UpdatedBy = LoginInfoDAX.UserName,
                ITL_UpdatedTime = BLLCom.GetCurStdDatetime(),
                //异动类型为{销售出库}
                ITL_TransType = InventoryTransTypeEnum.Name.XSCK,
                ITL_Source = paramSalesStockOutDetail.WH_Name,
                ITL_Destination = paramProactiveSalesManager.AROrgName
            };
            return newInventoryTransLog;
        }

        /// <summary>
        /// 【反审核】生成销售出库的库存异动日志
        /// </summary>
        /// <param name="paramStockOutBill">出库单</param>
        /// <param name="paramSalesStockOutDetail">出库明细</param>
        /// <param name="paramInventory">库存</param>
        /// <param name="paramProactiveSalesManager">主动销售单</param>
        /// <param name="paramStockOutDetailList">销售出库单</param>
        /// <returns></returns>
        private MDLPIS_InventoryTransLog GenerateStockOutInventoryTransLogOfUnApprove(MDLPIS_StockOutBill paramStockOutBill, MDLPIS_StockOutBillDetail paramSalesStockOutDetail, MDLPIS_Inventory paramInventory, ProactiveSalesManagerUIModel paramProactiveSalesManager, List<SalesStockOutDetailUIModel> paramStockOutDetailList)
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
                ITL_Destination = paramProactiveSalesManager.AROrgName

            };
            if (paramStockOutDetailList.Count > 0)
            {
                foreach (var loopStockOutDetail in paramStockOutDetailList)
                {
                    if (loopStockOutDetail.INV_Barcode == paramSalesStockOutDetail.SOBD_Barcode && loopStockOutDetail.INV_BatchNo == paramSalesStockOutDetail.SOBD_BatchNo)
                    {
                        newInventoryTransLog.ITL_Source = loopStockOutDetail.WH_Name;
                        break;
                    }
                }
            }
            return newInventoryTransLog;
        }

        /// <summary>
        /// 【核实】生成销售退货的库存异动日志
        /// </summary>
        /// <param name="paramStockInBill">入库单</param>
        /// <param name="paramReturnStockInDetail">退货入库明细</param>
        /// <param name="paramInventory">库存</param> 
        /// <param name="paramProactiveSalesManager">主动销售单</param>
        /// <returns></returns>
        private MDLPIS_InventoryTransLog GenerateStockInInventoryTransLogOfVerify(MDLPIS_StockInBill paramStockInBill, ReturnStockInDetailUIModel paramReturnStockInDetail, MDLPIS_Inventory paramInventory, ProactiveSalesManagerUIModel paramProactiveSalesManager)
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
                //退货入库，数量为正
                ITL_Qty = paramReturnStockInDetail.SID_Qty,
                ITL_AfterTransQty = paramInventory.INV_Qty,
                ITL_IsValid = true,
                ITL_CreatedBy = LoginInfoDAX.UserName,
                ITL_UpdatedBy = LoginInfoDAX.UserName,
                //异动类型为{销售退货}
                ITL_TransType = InventoryTransTypeEnum.Name.XSTH,
                ITL_Source = paramReturnStockInDetail.WH_Name,
                ITL_Destination = paramProactiveSalesManager.AROrgName,
            };
            return newInventoryTransLog;
        }

        #endregion
    }
}
