using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.PIS;
using SkyCar.Coeus.DAL;
using System.Reflection;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.Common.APModel;

namespace SkyCar.Coeus.BLL.PIS
{
    /// <summary>
    /// 入库管理BLL
    /// </summary>
    public class StockInBillManagerBLL : BLLBase
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
        /// 入库管理BLL
        /// </summary>
        public StockInBillManagerBLL() : base(Trans.PIS)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramHead">UIModel</param>
        /// <param name="paramDetailList">入库单明细列表</param>
        /// <param name="paramAutoPartsPictureList">配件图片UIModel列表</param>
        /// <returns></returns>
        public bool SaveDetailDS(StockInBillManagerUIModel paramHead, SkyCarBindingList<StockInBillDetailManagerUIModel, MDLPIS_StockInDetail> paramDetailList, List<AutoPartsPictureUIModel> paramAutoPartsPictureList)
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

            #region 单头
            //将UIModel转为TBModel
            var argsHead = paramHead.ToTBModelForSaveAndDelete<MDLPIS_StockInBill>();
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(paramHead.SIB_ID))
            {
                argsHead.SIB_ID = Guid.NewGuid().ToString();
                //单号
                argsHead.SIB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.SIB);
                argsHead.SIB_CreatedBy = LoginInfoDAX.UserName;
                argsHead.SIB_CreatedTime = BLLCom.GetCurStdDatetime();
            }
            argsHead.SIB_UpdatedBy = LoginInfoDAX.UserName;
            argsHead.SIB_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region 明细

            //添加的明细
            if (paramDetailList != null && paramDetailList.InsertList != null && paramDetailList.InsertList.Count > 0)
            {
                foreach (var loopStockInBillDetail in paramDetailList.InsertList)
                {
                    loopStockInBillDetail.SID_SIB_ID = argsHead.SIB_ID ?? argsHead.WHERE_SIB_ID;
                    loopStockInBillDetail.SID_SIB_No = argsHead.SIB_No;
                    loopStockInBillDetail.SID_CreatedBy = LoginInfoDAX.UserName;
                    loopStockInBillDetail.SID_CreatedTime = BLLCom.GetCurStdDatetime();
                    loopStockInBillDetail.SID_UpdatedBy = LoginInfoDAX.UserName;
                    loopStockInBillDetail.SID_UpdatedTime = BLLCom.GetCurStdDatetime();
                }
            }

            #endregion

            #region 配件图片

            //待保存的配件图片列表
            List<MDLPIS_InventoryPicture> savePictureList = new List<MDLPIS_InventoryPicture>();

            foreach (var loopPicture in paramAutoPartsPictureList)
            {
                if (string.IsNullOrEmpty(loopPicture.INVP_Barcode)
                    || string.IsNullOrEmpty(loopPicture.INVP_PictureName)
                    || string.IsNullOrEmpty(loopPicture.SourceFilePath))
                {
                    continue;
                }
                #region 将图片保存到本地以及上传文件服务器

                string fileNetUrl = string.Empty;
                bool savePictureResult = BLLCom.SaveFileByFileName(loopPicture.SourceFilePath, loopPicture.INVP_PictureName, ref fileNetUrl);
                if (!savePictureResult)
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + MsgParam.AUTOPARTS_PICTURE });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 保存入库明细图片数据

                //截取上传图片返回值中的文件名称
                int fileNameStartIndex = fileNetUrl.IndexOf("FileName=", StringComparison.Ordinal) + 1;
                int fileNameEndIndex = fileNameStartIndex + "FileName=".Length;
                int length = fileNetUrl.Length;
                //文件名称
                string tempFileName = fileNetUrl.Substring(fileNameEndIndex - 1, length - (fileNameEndIndex - 1));

                MDLPIS_InventoryPicture newAutoPartsPicture = new MDLPIS_InventoryPicture();

                _bll.CopyModel(loopPicture, newAutoPartsPicture);
                newAutoPartsPicture.INVP_PictureName = tempFileName;
                newAutoPartsPicture.INVP_IsValid = true;
                newAutoPartsPicture.INVP_CreatedBy = LoginInfoDAX.UserName;
                newAutoPartsPicture.INVP_CreatedTime = BLLCom.GetCurStdDatetime();
                newAutoPartsPicture.INVP_UpdatedBy = LoginInfoDAX.UserName;
                newAutoPartsPicture.INVP_UpdatedTime = BLLCom.GetCurStdDatetime();

                newAutoPartsPicture.WHERE_INVP_ID = newAutoPartsPicture.INVP_ID;
                newAutoPartsPicture.WHERE_INVP_VersionNo = newAutoPartsPicture.INVP_VersionNo;

                savePictureList.Add(newAutoPartsPicture);

                #endregion
            }

            #endregion

            #endregion

            #region 用事务保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存单头

                //执行保存
                if (!_bll.Save(argsHead, argsHead.SIB_ID))
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_StockInBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存明细

                //执行保存
                bool saveDetailResult = _bll.UnitySave(paramDetailList);
                if (!saveDetailResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_StockInDetail });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存配件图片

                foreach (var loopPicture in savePictureList)
                {
                    //执行保存
                    bool saveInvPictureResult = _bll.Save(loopPicture);
                    if (!saveInvPictureResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);

                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopPicture.INVP_PictureName, ref outMsg);

                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + MsgParam.AUTOPARTS_PICTURE });
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

                foreach (var loopPicture in savePictureList)
                {
                    //保存失败，删除本地以及文件服务器上的图片
                    var outMsg = string.Empty;
                    BLLCom.DeleteFileByFileName(loopPicture.INVP_PictureName, ref outMsg);
                }

                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
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
                        loopInsertDetail.SID_VersionNo = 1;
                    }
                }

                foreach (var loopUpdateDetail in paramDetailList.UpdateList)
                {
                    //更新时版本号加1
                    loopUpdateDetail.SID_VersionNo = loopUpdateDetail.SID_VersionNo + 1;
                }
            }

            #region 更新配件图片版本号

            foreach (var loopPicture in paramAutoPartsPictureList)
            {
                if (string.IsNullOrEmpty(loopPicture.SourceFilePath))
                {
                    continue;
                }
                //保存成功，删除临时保存的图片
                if (File.Exists(loopPicture.SourceFilePath))
                {
                    File.Delete(loopPicture.SourceFilePath);
                }
                //本次保存的图片
                var thisSavePicture = savePictureList.FirstOrDefault(x => x.INVP_PictureName == loopPicture.INVP_PictureName);
                if (thisSavePicture != null)
                {
                    _bll.CopyModel(thisSavePicture, loopPicture);
                }
                //设置版本号
                if (loopPicture.INVP_VersionNo == null)
                {
                    loopPicture.INVP_VersionNo = 1;
                }
                else
                {
                    loopPicture.INVP_VersionNo += 1;
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="paramHead">UIModel</param>
        /// <param name="paramDetailList">入库单明细列表</param>
        public bool DeleteDetailDS(MDLPIS_StockInBill paramHead,
            List<MDLPIS_StockInDetail> paramDetailList)
        {
            if (paramHead == null || paramDetailList.Count == 0)
            {
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0006, new object[] { SystemTableEnums.Name.PIS_StockInBill, SystemActionEnum.Name.DELETE });
                return false;
            }

            #region 带事务的删除

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 删除入库单和明细

                bool deleteResult = _bll.UnityDelete<MDLPIS_StockInBill, MDLPIS_StockInDetail>(paramHead, paramDetailList);
                if (!deleteResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE, SystemTableEnums.Name.PIS_StockInBill });
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
            #endregion

            return true;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="paramHead">UIModel</param>
        /// <param name="paramDetailList">入库单明细列表</param>
        /// <param name="paramAutoPartsPictureList">配件图片UIModel列表</param>
        /// <returns></returns>
        public bool ApproveDetailDS(StockInBillManagerUIModel paramHead, SkyCarBindingList<StockInBillDetailManagerUIModel, MDLPIS_StockInDetail> paramDetailList, List<AutoPartsPictureUIModel> paramAutoPartsPictureList)
        {
            var funcName = "ApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.SIB_ID)
                || string.IsNullOrEmpty(paramHead.SIB_No))
            {
                //没有获取到入库单，审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_StockInBill, SystemActionEnum.Name.APPROVE });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }
            #endregion

            #region 准备数据

            #region 变量定义

            //待更新的入库单
            MDLPIS_StockInBill updateStockInBill = new MDLPIS_StockInBill();
            //待新增的[应付单]
            MDLFM_AccountPayableBill newAccountPayableBill = new MDLFM_AccountPayableBill();
            //待新增的[应付单明细]列表
            List<MDLFM_AccountPayableBillDetail> newAccountPayableBillDetailList = new List<MDLFM_AccountPayableBillDetail>();
            //待保存的[库存]列表
            List<MDLPIS_Inventory> saveInventoryList = new List<MDLPIS_Inventory>();
            //待新增的[库存异动日志]列表
            List<MDLPIS_InventoryTransLog> newInventoryTransLogList = new List<MDLPIS_InventoryTransLog>();

            #endregion

            //计算入库金额
            decimal totalStockInAmount = CalculateStockInAmount(paramHead, paramDetailList);

            #region 更新入库单

            CopyModel(paramHead, updateStockInBill);

            //将入库单审核状态更新为[已审核]，单据状态更新为[已完成]
            updateStockInBill.WHERE_SIB_ID = updateStockInBill.SIB_ID;
            updateStockInBill.WHERE_SIB_VersionNo = updateStockInBill.SIB_VersionNo;
            updateStockInBill.SIB_VersionNo++;
            updateStockInBill.SIB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            updateStockInBill.SIB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            updateStockInBill.SIB_StatusCode = StockInBillStatusEnum.Code.YWC;
            updateStockInBill.SIB_StatusName = StockInBillStatusEnum.Name.YWC;
            updateStockInBill.SIB_UpdatedBy = LoginInfoDAX.UserName;
            updateStockInBill.SIB_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            #region 创建[应付单]的场合

            if (paramHead.SIB_SourceTypeName == StockInBillSourceTypeEnum.Name.SGCJ)
            {
                //应付单
                newAccountPayableBill.APB_ID = Guid.NewGuid().ToString();
                newAccountPayableBill.APB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.APB);
                //单据方向为[正向]
                newAccountPayableBill.APB_BillDirectCode = BillDirectionEnum.Code.PLUS;
                newAccountPayableBill.APB_BillDirectName = BillDirectionEnum.Name.PLUS;
                //来源类型为[收货应付]
                newAccountPayableBill.APB_SourceTypeCode = AccountPayableBillSourceTypeEnum.Code.SHYF;
                newAccountPayableBill.APB_SourceTypeName = AccountPayableBillSourceTypeEnum.Name.SHYF;
                //来源单号为[入库单号]
                newAccountPayableBill.APB_SourceBillNo = paramHead.SIB_No;
                newAccountPayableBill.APB_Org_ID = LoginInfoDAX.OrgID;
                newAccountPayableBill.APB_Org_Name = LoginInfoDAX.OrgShortName;
                //收款对象类型
                newAccountPayableBill.APB_ReceiveObjectTypeCode = AmountTransObjectTypeEnum.Code.AUTOPARTSSUPPLIER;
                newAccountPayableBill.APB_ReceiveObjectTypeName = AmountTransObjectTypeEnum.Name.AUTOPARTSSUPPLIER;
                newAccountPayableBill.APB_ReceiveObjectID = paramHead.SUPP_ID;
                newAccountPayableBill.APB_ReceiveObjectName = paramHead.SUPP_Name;
                //应付合计金额为[入库总金额]
                newAccountPayableBill.APB_AccountPayableAmount = totalStockInAmount;
                newAccountPayableBill.APB_PaidAmount = 0;
                newAccountPayableBill.APB_UnpaidAmount = totalStockInAmount;
                //应付单状态为[执行中]
                newAccountPayableBill.APB_BusinessStatusCode = AccountPayableBillStatusEnum.Code.ZXZ;
                newAccountPayableBill.APB_BusinessStatusName = AccountPayableBillStatusEnum.Name.ZXZ;
                //审核状态为[已审核]
                newAccountPayableBill.APB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
                newAccountPayableBill.APB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
                newAccountPayableBill.APB_IsValid = true;
                newAccountPayableBill.APB_CreatedBy = LoginInfoDAX.UserName;
                newAccountPayableBill.APB_CreatedTime = BLLCom.GetCurStdDatetime();
                newAccountPayableBill.APB_UpdatedBy = LoginInfoDAX.UserName;
                newAccountPayableBill.APB_UpdatedTime = BLLCom.GetCurStdDatetime();

            }
            #endregion

            #region 遍历[入库单明细]列表，创建[应付单明细]，创建或更新[库存]，创建[库存异动日志]

            foreach (var loopStockInBillDetail in paramDetailList)
            {
                if (string.IsNullOrEmpty(loopStockInBillDetail.SID_Barcode))
                {
                    continue;
                }

                #region 入库单明细

                if (string.IsNullOrEmpty(loopStockInBillDetail.SID_ID))
                {
                    loopStockInBillDetail.SID_ID = Guid.NewGuid().ToString();
                }
                //生成批次号
                loopStockInBillDetail.SID_BatchNo = BLLCom.GetBatchNo(new MDLPIS_Inventory
                {
                    WHERE_INV_Org_ID = updateStockInBill.SIB_Org_ID,
                    WHERE_INV_Barcode = loopStockInBillDetail.SID_Barcode,
                    WHERE_INV_WH_ID = loopStockInBillDetail.SID_WH_ID
                });
                //验证条码
                if (string.IsNullOrEmpty(loopStockInBillDetail.SID_Barcode))
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0016, new object[] { MsgParam.BARCODE, SystemActionEnum.Name.APPROVE });
                    return false;
                }
                //验证批次号
                if (string.IsNullOrEmpty(loopStockInBillDetail.SID_BatchNo))
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0016, new object[] { MsgParam.BATCHNO, SystemActionEnum.Name.APPROVE });
                    return false;
                }
                loopStockInBillDetail.WHERE_SID_ID = loopStockInBillDetail.SID_ID;
                loopStockInBillDetail.WHERE_SID_VersionNo = loopStockInBillDetail.SID_VersionNo;

                #endregion

                #region 应付单明细

                if (paramHead.SIB_SourceTypeName == StockInBillSourceTypeEnum.Name.SGCJ)
                {
                    MDLFM_AccountPayableBillDetail newAccountPayableBillDetail = new MDLFM_AccountPayableBillDetail
                    {
                        APBD_ID = Guid.NewGuid().ToString(),
                        APBD_APB_ID = newAccountPayableBill.APB_ID,
                        APBD_IsMinusDetail = false,
                        APBD_SourceBillNo = paramHead.SIB_No,
                        APBD_SourceBillDetailID = loopStockInBillDetail.SID_ID,
                        APBD_Org_ID = newAccountPayableBill.APB_Org_ID,
                        APBD_Org_Name = newAccountPayableBill.APB_Org_Name,
                        APBD_AccountPayableAmount = loopStockInBillDetail.SID_Amount,
                        APBD_PaidAmount = 0,
                        APBD_UnpaidAmount = loopStockInBillDetail.SID_Amount,
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
                }
                #endregion

                #region 库存和库存异动日志
                //在[入库单明细]列表中第一次出现的配件[库存]信息
                MDLPIS_Inventory inventoryExists = null;

                foreach (var loopInventory in saveInventoryList)
                {
                    if (loopInventory.INV_Barcode == loopStockInBillDetail.SID_Barcode
                        && loopInventory.INV_BatchNo == loopStockInBillDetail.SID_BatchNo)
                    {
                        inventoryExists = loopInventory;
                        break;
                    }
                }
                if (inventoryExists != null)
                {
                    //[入库单明细]列表中已遍历过该配件，累加数量
                    inventoryExists.INV_Qty += loopStockInBillDetail.SID_Qty;
                    inventoryExists.INV_UpdatedBy = LoginInfoDAX.UserName;
                    inventoryExists.INV_UpdatedTime = BLLCom.GetCurStdDatetime();

                    //生成[库存异动日志]
                    newInventoryTransLogList.Add(GenerateInventoryTransLogOfApprove(updateStockInBill, loopStockInBillDetail, inventoryExists));
                }
                else
                {
                    //[入库单明细]列表中第一次出现该配件
                    //查询该配件是否在[库存]中存在
                    MDLPIS_Inventory resultInventory = new MDLPIS_Inventory();
                    _bll.QueryForObject<MDLPIS_Inventory, MDLPIS_Inventory>(new MDLPIS_Inventory
                    {
                        WHERE_INV_Org_ID = updateStockInBill.SIB_Org_ID,
                        WHERE_INV_Barcode = loopStockInBillDetail.SID_Barcode,
                        WHERE_INV_BatchNo = loopStockInBillDetail.SID_BatchNo,
                        WHERE_INV_WH_ID = loopStockInBillDetail.SID_WH_ID,
                        WHERE_INV_IsValid = true
                    }, resultInventory);

                    //[库存]中不存在该配件
                    if (string.IsNullOrEmpty(resultInventory.INV_ID))
                    {
                        //新增一个该配件的库存信息
                        MDLPIS_Inventory inventoryToInsert = new MDLPIS_Inventory
                        {
                            INV_ID = Guid.NewGuid().ToString(),
                            INV_Org_ID = updateStockInBill.SIB_Org_ID,
                            INV_SUPP_ID = loopStockInBillDetail.SID_SUPP_ID,
                            INV_WH_ID = loopStockInBillDetail.SID_WH_ID,
                            INV_WHB_ID = loopStockInBillDetail.SID_WHB_ID,
                            INV_ThirdNo = loopStockInBillDetail.SID_ThirdNo,
                            INV_OEMNo = loopStockInBillDetail.SID_OEMNo,
                            INV_Barcode = loopStockInBillDetail.SID_Barcode,
                            INV_BatchNo = loopStockInBillDetail.SID_BatchNo,
                            INV_Name = loopStockInBillDetail.SID_Name,
                            INV_Specification = loopStockInBillDetail.SID_Specification,
                            INV_Qty = loopStockInBillDetail.SID_Qty,
                            INV_PurchaseUnitPrice = loopStockInBillDetail.SID_UnitCostPrice,
                            INV_IsValid = true,
                            INV_CreatedBy = LoginInfoDAX.UserName,
                            INV_CreatedTime = BLLCom.GetCurStdDatetime(),
                            INV_UpdatedBy = LoginInfoDAX.UserName,
                            INV_UpdatedTime = BLLCom.GetCurStdDatetime()
                        };
                        saveInventoryList.Add(inventoryToInsert);

                        //生成[库存异动日志]
                        newInventoryTransLogList.Add(GenerateInventoryTransLogOfApprove(updateStockInBill, loopStockInBillDetail, inventoryToInsert));
                    }
                    //[库存]中存在该配件
                    else
                    {
                        //更新[库存]中该配件的数量
                        resultInventory.INV_Qty += loopStockInBillDetail.SID_Qty;
                        resultInventory.INV_UpdatedBy = LoginInfoDAX.UserName;
                        resultInventory.INV_UpdatedTime = BLLCom.GetCurStdDatetime();
                        resultInventory.WHERE_INV_ID = resultInventory.INV_ID;
                        resultInventory.WHERE_INV_VersionNo = resultInventory.INV_VersionNo;
                        saveInventoryList.Add(resultInventory);

                        //生成[库存异动日志]
                        newInventoryTransLogList.Add(GenerateInventoryTransLogOfApprove(updateStockInBill, loopStockInBillDetail, resultInventory));
                    }
                }
                #endregion
            }
            #endregion

            #region 配件图片

            //待保存的配件图片列表
            List<MDLPIS_InventoryPicture> savePictureList = new List<MDLPIS_InventoryPicture>();
            foreach (var loopPicture in paramAutoPartsPictureList)
            {
                if (string.IsNullOrEmpty(loopPicture.INVP_Barcode)
                    || string.IsNullOrEmpty(loopPicture.INVP_PictureName)
                    || string.IsNullOrEmpty(loopPicture.SourceFilePath))
                {
                    continue;
                }

                #region 将图片保存到本地以及上传文件服务器

                string fileNetUrl = string.Empty;
                bool savePictureResult = BLLCom.SaveFileByFileName(loopPicture.SourceFilePath, loopPicture.INVP_PictureName, ref fileNetUrl);
                if (!savePictureResult)
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + MsgParam.AUTOPARTS_PICTURE });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 保存配件图片数据

                //截取上传图片返回值中的文件名称
                int fileNameStartIndex = fileNetUrl.IndexOf("FileName=", StringComparison.Ordinal) + 1;
                int fileNameEndIndex = fileNameStartIndex + "FileName=".Length;
                int length = fileNetUrl.Length;
                //文件名称
                string tempFileName = fileNetUrl.Substring(fileNameEndIndex - 1, length - (fileNameEndIndex - 1));

                MDLPIS_InventoryPicture newStockInPicture = new MDLPIS_InventoryPicture();
                _bll.CopyModel(loopPicture, newStockInPicture);
                newStockInPicture.INVP_PictureName = tempFileName;
                newStockInPicture.INVP_IsValid = true;
                newStockInPicture.INVP_CreatedBy = LoginInfoDAX.UserName;
                newStockInPicture.INVP_CreatedTime = BLLCom.GetCurStdDatetime();
                newStockInPicture.INVP_UpdatedBy = LoginInfoDAX.UserName;
                newStockInPicture.INVP_UpdatedTime = BLLCom.GetCurStdDatetime();

                newStockInPicture.WHERE_INVP_ID = newStockInPicture.INVP_ID;
                newStockInPicture.WHERE_INVP_VersionNo = newStockInPicture.INVP_VersionNo;

                savePictureList.Add(newStockInPicture);

                #endregion
            }
            #endregion

            #endregion

            #region 带事务的新增和保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 更新[入库单]
                bool updateStockInBillResult = _bll.Save(updateStockInBill);
                if (!updateStockInBillResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    foreach (var loopInventoryPicture in savePictureList)
                    {
                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopInventoryPicture.INVP_PictureName, ref outMsg);
                    }

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.PIS_StockInBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 保存[入库单明细]

                //执行保存
                if (!_bll.UnitySave(paramDetailList))
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    foreach (var loopInventoryPicture in savePictureList)
                    {
                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopInventoryPicture.INVP_PictureName, ref outMsg);
                    }

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_StockInDetail });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 新增[应付单]

                if (!string.IsNullOrEmpty(newAccountPayableBill.APB_ID))
                {
                    bool insertAccountPayableBillResult = _bll.Insert(newAccountPayableBill);
                    if (!insertAccountPayableBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);

                        foreach (var loopInventoryPicture in savePictureList)
                        {
                            //保存失败，删除本地以及文件服务器上的图片
                            var outMsg = string.Empty;
                            BLLCom.DeleteFileByFileName(loopInventoryPicture.INVP_PictureName, ref outMsg);
                        }

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

                        foreach (var loopInventoryPicture in savePictureList)
                        {
                            //保存失败，删除本地以及文件服务器上的图片
                            var outMsg = string.Empty;
                            BLLCom.DeleteFileByFileName(loopInventoryPicture.INVP_PictureName, ref outMsg);
                        }

                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.FM_AccountPayableBillDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 保存[库存]

                foreach (var loopInventory in saveInventoryList)
                {
                    bool saveInventoryResult = _bll.Save(loopInventory, loopInventory.INV_ID);
                    if (!saveInventoryResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);

                        foreach (var loopInventoryPicture in savePictureList)
                        {
                            //保存失败，删除本地以及文件服务器上的图片
                            var outMsg = string.Empty;
                            BLLCom.DeleteFileByFileName(loopInventoryPicture.INVP_PictureName, ref outMsg);
                        }

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

                        foreach (var loopInventoryPicture in savePictureList)
                        {
                            //保存失败，删除本地以及文件服务器上的图片
                            var outMsg = string.Empty;
                            BLLCom.DeleteFileByFileName(loopInventoryPicture.INVP_PictureName, ref outMsg);
                        }

                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.PIS_InventoryTransLog });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 保存配件图片

                foreach (var loopPicture in savePictureList)
                {
                    //执行保存
                    bool saveInvPictureResult = _bll.Save(loopPicture);
                    if (!saveInvPictureResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);

                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopPicture.INVP_PictureName, ref outMsg);

                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + MsgParam.AUTOPARTS_PICTURE });
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

                foreach (var loopInventoryPicture in savePictureList)
                {
                    //保存失败，删除本地以及文件服务器上的图片
                    var outMsg = string.Empty;
                    BLLCom.DeleteFileByFileName(loopInventoryPicture.INVP_PictureName, ref outMsg);
                }

                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.APPROVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }
            #endregion

            //将最新数据回写给DetailDS
            CopyModel(updateStockInBill, paramHead);

            //更新明细版本号
            if (paramDetailList.InsertList != null)
            {
                foreach (var loopInsertDetail in paramDetailList.InsertList)
                {
                    //新增时版本号为1
                    loopInsertDetail.SID_VersionNo = 1;
                    loopInsertDetail.PrintCount = 1;
                }
            }
            foreach (var loopUpdateDetail in paramDetailList.UpdateList)
            {
                //更新时版本号加1
                loopUpdateDetail.SID_VersionNo = loopUpdateDetail.SID_VersionNo + 1;
                loopUpdateDetail.PrintCount = 1;
            }

            #region 更新配件图片版本号
            foreach (var loopPicture in paramAutoPartsPictureList)
            {
                if (string.IsNullOrEmpty(loopPicture.SourceFilePath))
                {
                    continue;
                }
                //保存成功，删除临时保存的图片
                if (File.Exists(loopPicture.SourceFilePath))
                {
                    File.Delete(loopPicture.SourceFilePath);
                }
                //本次保存的图片
                var thisSavePicture = savePictureList.FirstOrDefault(x => x.INVP_PictureName == loopPicture.INVP_PictureName);
                if (thisSavePicture != null)
                {
                    _bll.CopyModel(thisSavePicture, loopPicture);
                }
                //设置版本号
                if (loopPicture.INVP_VersionNo == null)
                {
                    loopPicture.INVP_VersionNo = 1;
                }
                else
                {
                    loopPicture.INVP_VersionNo += 1;
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="paramHead">UIModel</param>
        /// <param name="paramDetailList">入库单明细列表</param>
        /// <returns></returns>
        public bool UnApproveDetailDS(StockInBillManagerUIModel paramHead, SkyCarBindingList<StockInBillDetailManagerUIModel, MDLPIS_StockInDetail> paramDetailList)
        {
            var funcName = "UnApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.SIB_ID)
                || string.IsNullOrEmpty(paramHead.SIB_No))
            {
                //没有获取到入库单，反审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_StockInBill, SystemActionEnum.Name.UNAPPROVE });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }
            #endregion

            #region 准备数据

            #region 变量定义

            //待更新的[入库单]
            var updateStockInBill = paramHead.ToTBModelForSaveAndDelete<MDLPIS_StockInBill>();
            //待删除的[应付单]
            MDLFM_AccountPayableBill deleteAccountPayableBill = new MDLFM_AccountPayableBill();
            //待删除的[应付单明细]列表
            List<MDLFM_AccountPayableBillDetail> deleteAccountPayableBillDetailList = new List<MDLFM_AccountPayableBillDetail>();
            //待新增的[库存异动日志]列表
            List<MDLPIS_InventoryTransLog> newInventoryTransLogList = new List<MDLPIS_InventoryTransLog>();
            //待更新的[库存]列表
            List<MDLPIS_Inventory> updateInventoryList = new List<MDLPIS_Inventory>();
            #endregion

            #region 更新[入库单]
            //将入库单[审核状态]更新为[待审核]，[单据状态]更新为[已生成]
            updateStockInBill.SIB_VersionNo++;
            updateStockInBill.SIB_ApprovalStatusCode = ApprovalStatusEnum.Code.DSH;
            updateStockInBill.SIB_ApprovalStatusName = ApprovalStatusEnum.Name.DSH;
            updateStockInBill.SIB_StatusCode = StockInBillStatusEnum.Code.YSC;
            updateStockInBill.SIB_StatusName = StockInBillStatusEnum.Name.YSC;
            updateStockInBill.SIB_UpdatedBy = LoginInfoDAX.UserName;
            updateStockInBill.SIB_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            //来源类型为[手工创建]的入库单，删除[应付单]和[应付单明细]
            if (paramHead.SIB_SourceTypeName == StockInBillSourceTypeEnum.Name.SGCJ)
            {
                #region 获取待删除的[应付单]

                _bll.QueryForObject<MDLFM_AccountPayableBill, MDLFM_AccountPayableBill>(new MDLFM_AccountPayableBill
                {
                    WHERE_APB_SourceBillNo = updateStockInBill.SIB_No,
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
                        WHERE_APBD_SourceBillNo = updateStockInBill.SIB_No,
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
            }

            #region 获取待更新的[库存]列表，生成[库存异动日志]

            foreach (var loopDetail in paramDetailList)
            {
                //待更新的[库存]
                MDLPIS_Inventory resultInventory = new MDLPIS_Inventory();

                _bll.QueryForObject<MDLPIS_Inventory, MDLPIS_Inventory>(new MDLPIS_Inventory
                {
                    WHERE_INV_Org_ID = updateStockInBill.SIB_Org_ID,
                    WHERE_INV_WH_ID = loopDetail.SID_WH_ID,
                    WHERE_INV_Barcode = loopDetail.SID_Barcode,
                    WHERE_INV_BatchNo = loopDetail.SID_BatchNo,
                    WHERE_INV_IsValid = true
                }, resultInventory);

                if (!string.IsNullOrEmpty(resultInventory.INV_ID)
                    && !string.IsNullOrEmpty(resultInventory.INV_Barcode)
                    && !string.IsNullOrEmpty(resultInventory.INV_BatchNo))
                {
                    //[反审核]时，恢复[审核]时入库的配件数量
                    resultInventory.INV_Qty -= loopDetail.SID_Qty;
                    //明细中的配件被使用过的场合，不能反审核
                    if (resultInventory.INV_Qty < 0)
                    {
                        //配件：resultInventory.INV_Name（条形码：resultInventory.INV_Barcode） 的库存不足，反审核失败
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0030, new object[]
                        {
                            resultInventory.INV_Name, resultInventory.INV_Barcode, MsgParam.SHORTAGE,
                            SystemActionEnum.Name.UNAPPROVE
                        });
                        return false;
                    }
                    resultInventory.INV_UpdatedBy = LoginInfoDAX.UserName;
                    resultInventory.INV_UpdatedTime = BLLCom.GetCurStdDatetime();
                    resultInventory.WHERE_INV_ID = resultInventory.INV_ID;
                    resultInventory.WHERE_INV_VersionNo = resultInventory.INV_VersionNo;
                }
                else
                {
                    //异常数据
                    //TODO
                }
                updateInventoryList.Add(resultInventory);

                //生成[库存异动日志]
                newInventoryTransLogList.Add(GenerateInventoryTransLogOfUnApprove(updateStockInBill, loopDetail, resultInventory));
            }

            #endregion

            #endregion

            #region 带事务的保存和删除

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 更新[入库单]
                bool updateStockOutBillResult = _bll.Save(updateStockInBill);
                if (!updateStockOutBillResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_StockOutBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
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
            CopyModel(updateStockInBill, paramHead);

            return true;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(StockInBillManagerUIModel paramModel)
        {
            return true;
        }

        /// <summary>
        /// 【审核】生成库存异动日志
        /// </summary>
        /// <param name="paramStockInBill">入库单</param>
        /// <param name="paramStockInBillDetail">入库单明细</param>
        /// <param name="paramInventory">库存</param>
        /// <returns></returns>
        private MDLPIS_InventoryTransLog GenerateInventoryTransLogOfApprove(MDLPIS_StockInBill paramStockInBill, StockInBillDetailManagerUIModel paramStockInBillDetail, MDLPIS_Inventory paramInventory)
        {
            MDLPIS_InventoryTransLog newInventoryTransLog = new MDLPIS_InventoryTransLog
            {
                ITL_Org_ID = string.IsNullOrEmpty(paramStockInBill.SIB_Org_ID) ? LoginInfoDAX.OrgID : paramStockInBill.SIB_Org_ID,
                ITL_WH_ID = paramInventory.INV_WH_ID,
                ITL_WHB_ID = paramStockInBillDetail.SID_WHB_ID,
                //业务单号为[入库单]的单号
                ITL_BusinessNo = paramStockInBill.SIB_No,
                ITL_Barcode = paramInventory.INV_Barcode,
                ITL_BatchNo = paramInventory.INV_BatchNo,
                ITL_Name = paramStockInBillDetail.SID_Name,
                ITL_Specification = paramStockInBillDetail.SID_Specification,
                ITL_UnitCostPrice = paramInventory.INV_PurchaseUnitPrice,
                ITL_UnitSalePrice = null,
                //入库，数量为正
                ITL_Qty = paramStockInBillDetail.SID_Qty,
                ITL_AfterTransQty = paramInventory.INV_Qty,
                ITL_IsValid = true,
                ITL_CreatedBy = LoginInfoDAX.UserName,
                ITL_UpdatedBy = LoginInfoDAX.UserName,
                ITL_Source = paramStockInBillDetail.SUPP_Name,
                ITL_Destination = paramStockInBillDetail.WH_Name,
            };

            //异动类型
            switch (paramStockInBill.SIB_SourceTypeName)
            {
                case StockInBillSourceTypeEnum.Name.SGCJ:
                    newInventoryTransLog.ITL_TransType = InventoryTransTypeEnum.Name.ZJRK;
                    break;
                case StockInBillSourceTypeEnum.Name.CGRK:
                    newInventoryTransLog.ITL_TransType = InventoryTransTypeEnum.Name.CGRK;
                    break;
                case StockInBillSourceTypeEnum.Name.SSTH:
                    newInventoryTransLog.ITL_TransType = InventoryTransTypeEnum.Name.XSTH;
                    break;
            }

            return newInventoryTransLog;
        }

        /// <summary>
        /// 【反审核】生成库存异动日志
        /// </summary>
        /// <param name="paramStockInBill">入库单</param>
        /// <param name="paramStockInBillDetail">入库单明细</param>
        /// <param name="paramInventory">库存</param>
        /// <returns></returns>
        private MDLPIS_InventoryTransLog GenerateInventoryTransLogOfUnApprove(MDLPIS_StockInBill paramStockInBill, StockInBillDetailManagerUIModel paramStockInBillDetail, MDLPIS_Inventory paramInventory)
        {
            MDLPIS_InventoryTransLog newInventoryTransLog = new MDLPIS_InventoryTransLog
            {
                ITL_Org_ID = string.IsNullOrEmpty(paramStockInBill.SIB_Org_ID) ? LoginInfoDAX.OrgID : paramStockInBill.SIB_Org_ID,
                ITL_WH_ID = paramInventory.INV_WH_ID,
                ITL_WHB_ID = paramStockInBillDetail.SID_WHB_ID,
                //业务单号为[入库单]的单号
                ITL_BusinessNo = paramStockInBill.SIB_No,
                ITL_Barcode = paramInventory.INV_Barcode,
                ITL_BatchNo = paramInventory.INV_BatchNo,
                ITL_Name = paramStockInBillDetail.SID_Name,
                ITL_Specification = paramStockInBillDetail.SID_Specification,
                ITL_UnitCostPrice = paramInventory.INV_PurchaseUnitPrice,
                ITL_UnitSalePrice = null,
                //入库 反审核，实际为出库，数量为负
                ITL_Qty = -paramStockInBillDetail.SID_Qty,
                ITL_AfterTransQty = paramInventory.INV_Qty,
                ITL_IsValid = true,
                ITL_CreatedBy = LoginInfoDAX.UserName,
                ITL_UpdatedBy = LoginInfoDAX.UserName
            };

            //异动类型
            switch (paramStockInBill.SIB_SourceTypeName)
            {
                case StockInBillSourceTypeEnum.Name.SGCJ:
                    newInventoryTransLog.ITL_TransType = InventoryTransTypeEnum.Name.ZJRK;
                    break;
                case StockInBillSourceTypeEnum.Name.CGRK:
                    newInventoryTransLog.ITL_TransType = InventoryTransTypeEnum.Name.CGRK;
                    break;
                case StockInBillSourceTypeEnum.Name.SSTH:
                    newInventoryTransLog.ITL_TransType = InventoryTransTypeEnum.Name.XSTH;
                    break;
            }
            newInventoryTransLog.ITL_Source = paramStockInBillDetail.SUPP_Name;
            newInventoryTransLog.ITL_Destination = paramStockInBillDetail.WH_Name;
            return newInventoryTransLog;
        }

        /// <summary>
        /// 计算入库总金额
        /// </summary>
        /// <param name="paramStockInBill">入库单</param>
        /// <param name="paramStockInBillDetailList">入库单明细列表</param>
        /// <returns></returns>
        private decimal CalculateStockInAmount(StockInBillManagerUIModel paramStockInBill, SkyCarBindingList<StockInBillDetailManagerUIModel, MDLPIS_StockInDetail> paramStockInBillDetailList)
        {
            if (paramStockInBill == null || string.IsNullOrEmpty(paramStockInBill.SIB_ID))
            {
                return 0;
            }
            if (paramStockInBillDetailList == null || paramStockInBillDetailList.Count == 0)
            {
                return 0;
            }

            #region 计算入库总金额
            decimal totalStockInAmount = 0;
            foreach (var loopStockInBillDetail in paramStockInBillDetailList)
            {
                if (string.IsNullOrEmpty(loopStockInBillDetail.SID_Barcode))
                {
                    continue;
                }
                totalStockInAmount += (loopStockInBillDetail.SID_Amount ?? 0);
            }
            #endregion

            return totalStockInAmount;
        }
        #endregion
    }
}
