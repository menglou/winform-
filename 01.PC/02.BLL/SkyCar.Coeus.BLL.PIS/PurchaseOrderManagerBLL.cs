using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.PIS;
using System.Reflection;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.UIModel.Common.APModel;

namespace SkyCar.Coeus.BLL.PIS
{
    /// <summary>
    /// 采购订单管理BLL
    /// </summary>
    public class PurchaseOrderManagerBLL : BLLBase
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
        /// 采购订单管理BLL
        /// </summary>
        public PurchaseOrderManagerBLL() : base(Trans.PIS)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramHead">单头UIModel</param>
        /// <param name="paramDetailList">明细UIModel列表</param>
        /// <param name="paramAutoPartsPictureList">配件图片UIModel列表</param>
        /// <returns></returns>
        public bool SaveDetailDS(PurchaseOrderManagerUIModel paramHead, SkyCarBindingList<PurchaseOrderManagerDetailUIModel, MDLPIS_PurchaseOrderDetail> paramDetailList, List<AutoPartsPictureUIModel> paramAutoPartsPictureList)
        {
            var funcName = "SaveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 准备数据

            #region 单头数据
            //将UIModel转为TBModel
            var argsHead = paramHead.ToTBModelForSaveAndDelete<MDLPIS_PurchaseOrder>();
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(paramHead.PO_ID))
            {
                argsHead.PO_ID = Guid.NewGuid().ToString();
                //采购单号
                argsHead.PO_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.PO);
                argsHead.PO_CreatedBy = LoginInfoDAX.UserName;
                argsHead.PO_CreatedTime = BLLCom.GetCurStdDatetime();
            }
            argsHead.PO_UpdatedBy = LoginInfoDAX.UserName;
            argsHead.PO_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region 明细数据
            if (paramDetailList != null && paramDetailList.InsertList != null && paramDetailList.InsertList.Count > 0)
            {
                foreach (var loopDetail in paramDetailList.InsertList)
                {
                    loopDetail.POD_PO_ID = argsHead.PO_ID ?? argsHead.WHERE_PO_ID;
                    loopDetail.POD_PO_No = argsHead.PO_No;
                    loopDetail.POD_CreatedBy = LoginInfoDAX.UserName;
                    loopDetail.POD_CreatedTime = BLLCom.GetCurStdDatetime();
                    loopDetail.POD_UpdatedBy = LoginInfoDAX.UserName;
                    loopDetail.POD_UpdatedTime = BLLCom.GetCurStdDatetime();
                }
            }
            #endregion

            #region 采购预测订单
            //待更新的采购预测订单
            MDLPIS_PurchaseForecastOrder updatePurchaseForecastOrder = new MDLPIS_PurchaseForecastOrder();
            if (argsHead.PO_SourceTypeName == PurchaseOrderSourceTypeEnum.Name.CGYC)
            {
                if (!string.IsNullOrEmpty(argsHead.PO_SourceNo))
                {
                    _bll.QueryForObject<MDLPIS_PurchaseForecastOrder, MDLPIS_PurchaseForecastOrder>(new MDLPIS_PurchaseForecastOrder
                    {
                        WHERE_PFO_No = argsHead.PO_SourceNo,
                        WHERE_PFO_IsValid = true,
                    }, updatePurchaseForecastOrder);
                    //更新[单据状态]为{已转采购}
                    updatePurchaseForecastOrder.PFO_StatusName = PurchaseForecastOrderStatusEnum.Name.YZCG;
                    updatePurchaseForecastOrder.PFO_StatusCode = PurchaseForecastOrderStatusEnum.Code.YZCG;
                    updatePurchaseForecastOrder.PFO_UpdatedBy = LoginInfoDAX.UserName;
                    updatePurchaseForecastOrder.PFO_UpdatedTime = BLLCom.GetCurStdDatetime();

                    updatePurchaseForecastOrder.WHERE_PFO_ID = updatePurchaseForecastOrder.PFO_ID;
                    updatePurchaseForecastOrder.WHERE_PFO_VersionNo = updatePurchaseForecastOrder.PFO_VersionNo;
                }
            }

            #endregion

            #region 配件图片

            //待保存的配件图片列表
            List<MDLPIS_InventoryPicture> savePictureList = new List<MDLPIS_InventoryPicture>();

            foreach (var loopPicture in paramAutoPartsPictureList)
            {
                if (string.IsNullOrEmpty(loopPicture.INVP_PictureName)
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

                #region 保存采购明细图片数据

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

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存单头

                //执行保存
                bool savePurchaseOrderResult = _bll.Save(argsHead, argsHead.PO_ID);
                if (!savePurchaseOrderResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    foreach (var loopPicture in savePictureList)
                    {
                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopPicture.INVP_PictureName, ref outMsg);
                    }

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_PurchaseOrder });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存明细

                //执行保存
                bool savePurchaseDetailResult = _bll.UnitySave(paramDetailList);
                if (!savePurchaseDetailResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    foreach (var loopPicture in savePictureList)
                    {
                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopPicture.INVP_PictureName, ref outMsg);
                    }

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_PurchaseOrderDetail });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 更新[采购预测订单]
                if (!string.IsNullOrEmpty(updatePurchaseForecastOrder.PFO_ID))
                {
                    bool updatePurchaseForecastOrderResult = _bll.Update(updatePurchaseForecastOrder);
                    if (!updatePurchaseForecastOrderResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);

                        foreach (var loopPicture in savePictureList)
                        {
                            //保存失败，删除本地以及文件服务器上的图片
                            var outMsg = string.Empty;
                            BLLCom.DeleteFileByFileName(loopPicture.INVP_PictureName, ref outMsg);
                        }

                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.PIS_PurchaseForecastOrder });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 保存[配件图片]

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
                        loopInsertDetail.POD_VersionNo = 1;
                    }
                }

                foreach (var loopUpdateDetail in paramDetailList.UpdateList)
                {
                    //更新时版本号加1
                    loopUpdateDetail.POD_VersionNo = loopUpdateDetail.POD_VersionNo + 1;
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
        /// <param name="paramDetailList">采购明细列表</param>
        public bool DeleteDetailDS(MDLPIS_PurchaseOrder paramHead,
            List<MDLPIS_PurchaseOrderDetail> paramDetailList)
        {
            if (paramHead == null || paramDetailList.Count == 0)
            {
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0006, new object[] { SystemTableEnums.Name.PIS_PurchaseOrder, SystemActionEnum.Name.DELETE });
                return false;
            }

            #region 带事务的删除

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 删除[采购单]和[采购明细]

                bool deleteResult = _bll.UnityDelete(paramHead, paramDetailList);
                if (!deleteResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE, SystemTableEnums.Name.PIS_PurchaseOrder });
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
        /// <param name="paramHead">单头UIModel</param>
        /// <param name="paramDetailList">明细UIModel列表</param>
        /// <param name="paramAutoPartsPictureList">配件图片UIModel列表</param>
        /// <returns></returns>
        public bool ApproveDetailDS(PurchaseOrderManagerUIModel paramHead, SkyCarBindingList<PurchaseOrderManagerDetailUIModel, MDLPIS_PurchaseOrderDetail> paramDetailList, List<AutoPartsPictureUIModel> paramAutoPartsPictureList)
        {
            var funcName = "ApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.PO_ID)
                || string.IsNullOrEmpty(paramHead.PO_No))
            {
                //没有获取到采购订单，审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_PurchaseOrder, SystemActionEnum.Name.APPROVE });
                return false;
            }
            #endregion

            #region 准备数据

            #region 更新采购订单
            //将UIModel转为TBModel
            var updatePurchaseOrder = paramHead.ToTBModelForSaveAndDelete<MDLPIS_PurchaseOrder>();
            //更新[审核状态]为{已下单}
            updatePurchaseOrder.PO_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            updatePurchaseOrder.PO_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            //更新[单据状态]为{已下单}
            updatePurchaseOrder.PO_StatusName = PurchaseOrderStatusEnum.Name.YXD;
            updatePurchaseOrder.PO_StatusCode = PurchaseOrderStatusEnum.Code.YXD;
            updatePurchaseOrder.PO_UpdatedBy = LoginInfoDAX.UserName;
            updatePurchaseOrder.PO_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            #region 更新采购订单明细

            foreach (var loopUpdatePurchaseOrder in paramDetailList)
            {
                //更新单据状态为{已下单}
                loopUpdatePurchaseOrder.POD_StatusName = PurchaseOrderDetailStatusEnum.Name.YXD;
                loopUpdatePurchaseOrder.POD_StatusCode = PurchaseOrderDetailStatusEnum.Code.YXD;
                loopUpdatePurchaseOrder.POD_UpdatedBy = LoginInfoDAX.UserName;
                loopUpdatePurchaseOrder.POD_UpdatedTime = BLLCom.GetCurStdDatetime();
            }
            #endregion

            #region 配件图片

            //待保存的配件图片列表
            List<MDLPIS_InventoryPicture> savePictureList = new List<MDLPIS_InventoryPicture>();

            foreach (var loopPicture in paramAutoPartsPictureList)
            {
                if (string.IsNullOrEmpty(loopPicture.INVP_PictureName)
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

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 更新[采购单]

                bool updatePurchaseOrderResult = _bll.Save(updatePurchaseOrder);
                if (!updatePurchaseOrderResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    foreach (var loopPicture in savePictureList)
                    {
                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopPicture.INVP_PictureName, ref outMsg);
                    }

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.APPROVE + SystemTableEnums.Name.PIS_PurchaseOrder });
                    return false;
                }

                #endregion

                #region 更新[采购单明细]

                bool updatePurchaseOrderDetailResult = _bll.UnitySave(paramDetailList);
                if (!updatePurchaseOrderDetailResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    foreach (var loopPicture in savePictureList)
                    {
                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopPicture.INVP_PictureName, ref outMsg);
                    }

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.APPROVE + SystemTableEnums.Name.PIS_PurchaseOrderDetail });
                    return false;
                }

                #endregion

                #region 保存[配件图片]

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

                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.APPROVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }

            #endregion

            //将最新数据回写给DetailDS
            CopyModel(updatePurchaseOrder, paramHead);
            //更新明细版本号
            if (paramDetailList.InsertList != null)
            {
                foreach (var loopInsertDetail in paramDetailList.InsertList)
                {
                    //新增时版本号为1
                    loopInsertDetail.POD_VersionNo = 1;
                }
            }

            foreach (var loopUpdateDetail in paramDetailList.UpdateList)
            {
                //更新时版本号加1
                loopUpdateDetail.POD_VersionNo = loopUpdateDetail.POD_VersionNo + 1;
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
        /// <param name="paramHead">单头UIModel</param>
        /// <param name="paramDetailList">明细UIModel列表</param>
        /// <returns></returns>
        public bool UnApproveDetailDS(PurchaseOrderManagerUIModel paramHead,
            SkyCarBindingList<PurchaseOrderManagerDetailUIModel, MDLPIS_PurchaseOrderDetail> paramDetailList)
        {
            var funcName = "UnApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.PO_ID)
                || string.IsNullOrEmpty(paramHead.PO_No))
            {
                //没有获取到采购订单，反审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_PurchaseOrder, SystemActionEnum.Name.UNAPPROVE });
                return false;
            }
            #endregion

            #region 准备数据

            #region 更新采购单
            //待更新的[采购单]
            var updatePurchaseOrder = paramHead.ToTBModelForSaveAndDelete<MDLPIS_PurchaseOrder>();
            updatePurchaseOrder.PO_VersionNo = paramHead.PO_VersionNo + 1;
            //更新[审核状态]为{待审核}
            updatePurchaseOrder.PO_ApprovalStatusName = ApprovalStatusEnum.Name.DSH;
            updatePurchaseOrder.PO_ApprovalStatusCode = ApprovalStatusEnum.Code.DSH;
            //更新[单据状态]为{已生成}
            updatePurchaseOrder.PO_StatusName = PurchaseOrderStatusEnum.Name.YSC;
            updatePurchaseOrder.PO_StatusCode = PurchaseOrderStatusEnum.Code.YSC;
            updatePurchaseOrder.PO_UpdatedBy = LoginInfoDAX.UserName;
            updatePurchaseOrder.PO_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            #region 更新采购单明细

            foreach (var loopDetail in paramDetailList)
            {
                //更新[单据状态]为{已生成}
                loopDetail.POD_StatusName = PurchaseOrderDetailStatusEnum.Name.YSC;
                loopDetail.POD_StatusCode = PurchaseOrderDetailStatusEnum.Code.YSC;
                loopDetail.POD_UpdatedBy = LoginInfoDAX.UserName;
                loopDetail.POD_UpdatedTime = BLLCom.GetCurStdDatetime();
                //todo 验证不给whereid和版本号赋值是否正确
            }
            #endregion

            #endregion

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 更新[采购单]

                bool updatePurchaseOrderResult = _bll.Save(updatePurchaseOrder);
                if (!updatePurchaseOrderResult)
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.UNAPPROVE + SystemTableEnums.Name.PIS_PurchaseOrder });
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    return false;
                }

                #endregion

                #region 更新[采购单明细]

                bool updatePurchaseOrderDetailResult = _bll.UnitySave(paramDetailList);
                if (!updatePurchaseOrderDetailResult)
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.UNAPPROVE + SystemTableEnums.Name.PIS_PurchaseOrderDetail });
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    return false;
                }

                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.APPROVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }

            #endregion

            //将最新数据回写给DetailDS
            CopyModel(updatePurchaseOrder, paramHead);
            //更新明细版本号
            foreach (var loopUpdateDetail in paramDetailList.UpdateList)
            {
                //更新时版本号加1
                loopUpdateDetail.POD_VersionNo = loopUpdateDetail.POD_VersionNo + 1;
            }

            return true;
        }

        /// <summary>
        /// 签收
        /// </summary>
        /// <param name="paramHead">单头UIModel</param>
        /// <param name="paramDetailList">明细UIModel列表</param>
        /// <param name="paramAutoPartsPictureList">配件图片UIModel列表</param>
        /// <returns></returns>
        public bool SignInDetailDS(PurchaseOrderManagerUIModel paramHead,
            SkyCarBindingList<PurchaseOrderManagerDetailUIModel, MDLPIS_PurchaseOrderDetail> paramDetailList, List<AutoPartsPictureUIModel> paramAutoPartsPictureList)
        {
            var funcName = "SignInDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.PO_ID)
                || string.IsNullOrEmpty(paramHead.PO_No))
            {
                //没有获取到采购订单，签收失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_PurchaseOrder, SystemActionEnum.Name.SIGNIN });
                return false;
            }
            #endregion

            #region 准备数据

            #region 变量定义

            //待更新[采购单]
            var updatePurchaseOrder = paramHead.ToTBModelForSaveAndDelete<MDLPIS_PurchaseOrder>();
            //待新增的[入库单]
            MDLPIS_StockInBill newStockInBill = new MDLPIS_StockInBill();
            //待新增的[入库单明细]列表
            List<MDLPIS_StockInDetail> newStockInDetailList = new List<MDLPIS_StockInDetail>();
            //待新增的[应付单]
            MDLFM_AccountPayableBill newAccountPayableBill = new MDLFM_AccountPayableBill();
            //待新增的[应付单明细]列表
            List<MDLFM_AccountPayableBillDetail> newAccountPayableBillDetailList = new List<MDLFM_AccountPayableBillDetail>();
            //待保存的[库存]列表
            List<MDLPIS_Inventory> saveInventoryList = new List<MDLPIS_Inventory>();
            //待新增的[库存异动日志]列表
            List<MDLPIS_InventoryTransLog> newInventoryTransLogList = new List<MDLPIS_InventoryTransLog>();

            #endregion

            #region 更新采购单
            updatePurchaseOrder.PO_VersionNo = paramHead.PO_VersionNo + 1;
            updatePurchaseOrder.PO_ReceivedTime = BLLCom.GetCurStdDatetime();
            updatePurchaseOrder.PO_UpdatedBy = LoginInfoDAX.UserName;
            updatePurchaseOrder.PO_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            #region 待新增的[入库单]
            newStockInBill.SIB_ID = Guid.NewGuid().ToString();
            newStockInBill.SIB_Org_ID = LoginInfoDAX.OrgID;
            //入库单号
            newStockInBill.SIB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.SIB);
            //[来源类型]为{采购入库}
            newStockInBill.SIB_SourceTypeCode = StockInBillSourceTypeEnum.Code.CGRK;
            newStockInBill.SIB_SourceTypeName = StockInBillSourceTypeEnum.Name.CGRK;
            newStockInBill.SIB_SourceNo = updatePurchaseOrder.PO_No;
            //单据状态为{已完成}
            newStockInBill.SIB_StatusCode = StockInBillStatusEnum.Code.YWC;
            newStockInBill.SIB_StatusName = StockInBillStatusEnum.Name.YWC;
            //审核状态为{已审核}
            newStockInBill.SIB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            newStockInBill.SIB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            newStockInBill.SIB_IsValid = true;
            newStockInBill.SIB_CreatedBy = LoginInfoDAX.UserName;
            newStockInBill.SIB_CreatedTime = BLLCom.GetCurStdDatetime();
            newStockInBill.SIB_UpdatedBy = LoginInfoDAX.UserName;
            newStockInBill.SIB_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region 待新增的[应付单]
            newAccountPayableBill.APB_ID = Guid.NewGuid().ToString();
            //应付单号
            newAccountPayableBill.APB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.APB);
            //[单据方向]为{正向}
            newAccountPayableBill.APB_BillDirectCode = BillDirectionEnum.Code.PLUS;
            newAccountPayableBill.APB_BillDirectName = BillDirectionEnum.Name.PLUS;
            //[来源类型]为{收货应付}
            newAccountPayableBill.APB_SourceTypeCode = AccountPayableBillSourceTypeEnum.Code.SHYF;
            newAccountPayableBill.APB_SourceTypeName = AccountPayableBillSourceTypeEnum.Name.SHYF;
            //[来源单号]为[入库单].[单号]
            newAccountPayableBill.APB_SourceBillNo = newStockInBill.SIB_No;
            newAccountPayableBill.APB_Org_ID = LoginInfoDAX.OrgID;
            newAccountPayableBill.APB_Org_Name = LoginInfoDAX.OrgShortName;
            //[收款对象类型]为{供应商}
            newAccountPayableBill.APB_ReceiveObjectTypeCode = AmountTransObjectTypeEnum.Code.AUTOPARTSSUPPLIER;
            newAccountPayableBill.APB_ReceiveObjectTypeName = AmountTransObjectTypeEnum.Name.AUTOPARTSSUPPLIER;
            newAccountPayableBill.APB_ReceiveObjectID = paramHead.PO_SUPP_ID;
            newAccountPayableBill.APB_ReceiveObjectName = paramHead.PO_SUPP_Name;
            //应付单单头的应付金额在[应付单明细]中计算
            newAccountPayableBill.APB_AccountPayableAmount = 0;
            newAccountPayableBill.APB_UnpaidAmount = 0;
            //[业务状态]为{执行中}
            newAccountPayableBill.APB_BusinessStatusCode = AccountPayableBillStatusEnum.Code.ZXZ;
            newAccountPayableBill.APB_BusinessStatusName = AccountPayableBillStatusEnum.Name.ZXZ;
            newAccountPayableBill.APB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            newAccountPayableBill.APB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            newAccountPayableBill.APB_IsValid = true;
            newAccountPayableBill.APB_CreatedBy = LoginInfoDAX.UserName;
            newAccountPayableBill.APB_CreatedTime = BLLCom.GetCurStdDatetime();
            newAccountPayableBill.APB_UpdatedBy = LoginInfoDAX.UserName;
            newAccountPayableBill.APB_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region 采购明细列表

            if (paramHead.PO_StatusName == PurchaseOrderStatusEnum.Name.QBQS)
            {
                #region [全部签收]的场合

                foreach (var loopDetail in paramDetailList)
                {
                    if (string.IsNullOrEmpty(loopDetail.POD_AutoPartsBarcode)
                        || loopDetail.ThisReceivedQty == null
                        || loopDetail.ThisReceivedQty == 0)
                    {
                        continue;
                    }

                    #region 待新增的[入库单明细]

                    MDLPIS_StockInDetail newStockInDetail = new MDLPIS_StockInDetail
                    {
                        SID_ID = Guid.NewGuid().ToString(),
                        SID_SIB_ID = newStockInBill.SIB_ID,
                        SID_SIB_No = newStockInBill.SIB_No,
                        SID_SourceDetailID = loopDetail.POD_ID,
                        SID_Barcode = loopDetail.POD_AutoPartsBarcode,
                        //生成批次号
                        SID_BatchNo = BLLCom.GetBatchNo(new MDLPIS_Inventory
                        {
                            WHERE_INV_Org_ID = paramHead.PO_Org_ID,
                            WHERE_INV_Barcode = loopDetail.POD_AutoPartsBarcode,
                            WHERE_INV_WH_ID = loopDetail.POD_WH_ID
                        }),
                        SID_ThirdNo = loopDetail.POD_ThirdCode,
                        SID_OEMNo = loopDetail.POD_OEMCode,
                        SID_Name = loopDetail.POD_AutoPartsName,
                        SID_Specification = loopDetail.POD_AutoPartsSpec,
                        SID_SUPP_ID = updatePurchaseOrder.PO_SUPP_ID,
                        SID_WH_ID = loopDetail.POD_WH_ID,
                        SID_WHB_ID = loopDetail.POD_WHB_ID,
                        SID_UOM = loopDetail.POD_UOM,
                        //入库单价
                        SID_UnitCostPrice = loopDetail.POD_UnitPrice,
                        //入库数量 = 本次签收数量
                        SID_Qty = Convert.ToDecimal(loopDetail.ThisReceivedQty ?? 0),
                        //入库金额 = 本次签收数量 * 订货单价
                        SID_Amount = Math.Round(
                            (Convert.ToDecimal(loopDetail.ThisReceivedQty ?? 0)) *
                            Convert.ToDecimal(loopDetail.POD_UnitPrice ?? 0), 2),
                        SID_IsValid = true,
                        SID_CreatedBy = LoginInfoDAX.UserName,
                        SID_CreatedTime = BLLCom.GetCurStdDatetime(),
                        SID_UpdatedBy = LoginInfoDAX.UserName,
                        SID_UpdatedTime = BLLCom.GetCurStdDatetime()
                    };
                    // 暂时不维护
                    //newStockInDetail.SID_IsSettled = false;
                    newStockInDetailList.Add(newStockInDetail);

                    #endregion

                    #region 待新增的[应付单明细]

                    MDLFM_AccountPayableBillDetail newAccountPayableBillDetail = new MDLFM_AccountPayableBillDetail
                    {
                        APBD_APB_ID = newAccountPayableBill.APB_ID,
                        APBD_IsMinusDetail = newAccountPayableBill.APB_BillDirectCode != BillDirectionEnum.Name.PLUS,
                        APBD_SourceBillNo = newAccountPayableBill.APB_SourceBillNo,
                        //[来源单据明细ID]为[入库单明细].[ID]
                        APBD_SourceBillDetailID = newStockInDetail.SID_ID,
                        APBD_Org_ID = newAccountPayableBill.APB_Org_ID,
                        APBD_Org_Name = newAccountPayableBill.APB_Org_Name,
                        //应付金额 = 本次签收数量 * 订货单价
                        APBD_AccountPayableAmount = Math.Round((loopDetail.ThisReceivedQty ?? 0) * (loopDetail.POD_UnitPrice ?? 0), 2),
                        //已付金额 = 0
                        APBD_PaidAmount = Convert.ToDecimal(0),
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
                    //未付金额 = 应付金额
                    newAccountPayableBillDetail.APBD_UnpaidAmount = newAccountPayableBillDetail.APBD_AccountPayableAmount;
                    newAccountPayableBillDetailList.Add(newAccountPayableBillDetail);

                    //应付单.[应付金额]
                    newAccountPayableBill.APB_AccountPayableAmount = newAccountPayableBill.APB_AccountPayableAmount
                        + newAccountPayableBillDetail.APBD_AccountPayableAmount;
                    //应付单.[未付金额]
                    newAccountPayableBill.APB_UnpaidAmount = newAccountPayableBill.APB_AccountPayableAmount;

                    #endregion

                    #region [采购明细]
                    //本次签收数量为全部订货数量
                    loopDetail.POD_ReceivedQty = loopDetail.POD_OrderQty;
                    loopDetail.ThisReceivedQty = 0;
                    //单据状态为{已签收}
                    loopDetail.POD_StatusName = PurchaseOrderDetailStatusEnum.Name.YQS;
                    loopDetail.POD_StatusCode = PurchaseOrderDetailStatusEnum.Code.YQS;
                    loopDetail.POD_ReceivedTime = BLLCom.GetCurStdDatetime();
                    loopDetail.POD_UpdatedBy = LoginInfoDAX.UserName;
                    loopDetail.POD_UpdatedTime = BLLCom.GetCurStdDatetime();
                    #endregion
                }

                #endregion
            }
            else if (paramHead.PO_StatusName == PurchaseOrderStatusEnum.Name.BFQS)
            {
                #region [部分签收]的场合
                foreach (var loopDetail in paramDetailList)
                {
                    if (string.IsNullOrEmpty(loopDetail.POD_AutoPartsBarcode)
                        || loopDetail.ThisReceivedQty == null
                        || loopDetail.ThisReceivedQty == 0)
                    {
                        continue;
                    }

                    #region 待新增的[入库单明细]

                    MDLPIS_StockInDetail newStockInDetail = new MDLPIS_StockInDetail
                    {
                        SID_ID = Guid.NewGuid().ToString(),
                        SID_SIB_ID = newStockInBill.SIB_ID,
                        SID_SIB_No = newStockInBill.SIB_No,
                        SID_SourceDetailID = loopDetail.POD_ID,
                        SID_Barcode = loopDetail.POD_AutoPartsBarcode,
                        //生成批次号
                        SID_BatchNo = BLLCom.GetBatchNo(new MDLPIS_Inventory
                        {
                            WHERE_INV_Org_ID = paramHead.PO_Org_ID,
                            WHERE_INV_Barcode = loopDetail.POD_AutoPartsBarcode,
                            WHERE_INV_WH_ID = loopDetail.POD_WH_ID
                        }),
                        SID_ThirdNo = loopDetail.POD_ThirdCode,
                        SID_OEMNo = loopDetail.POD_OEMCode,
                        SID_Name = loopDetail.POD_AutoPartsName,
                        SID_Specification = loopDetail.POD_AutoPartsSpec,
                        SID_SUPP_ID = updatePurchaseOrder.PO_SUPP_ID,
                        SID_WH_ID = loopDetail.POD_WH_ID,
                        SID_WHB_ID = loopDetail.POD_WHB_ID,
                        SID_UOM = loopDetail.POD_UOM,
                        //入库单价
                        SID_UnitCostPrice = loopDetail.POD_UnitPrice,
                        //入库数量 = 本次签收数量
                        SID_Qty = loopDetail.ThisReceivedQty,
                        //入库金额 = 本次签收数量 * 订货单价
                        SID_Amount = Math.Round(Convert.ToDecimal(loopDetail.ThisReceivedQty ?? 0) *
                                     Convert.ToDecimal(loopDetail.POD_UnitPrice ?? 0), 2),
                        SID_IsSettled = false,
                        SID_IsValid = true,
                        SID_CreatedBy = LoginInfoDAX.UserName,
                        SID_CreatedTime = BLLCom.GetCurStdDatetime(),
                        SID_UpdatedBy = LoginInfoDAX.UserName,
                        SID_UpdatedTime = BLLCom.GetCurStdDatetime()
                    };
                    newStockInDetailList.Add(newStockInDetail);

                    #endregion

                    #region 待新增的[应付单明细]

                    MDLFM_AccountPayableBillDetail newAccountPayableBillDetail = new MDLFM_AccountPayableBillDetail
                    {
                        APBD_APB_ID = newAccountPayableBill.APB_ID,
                        APBD_IsMinusDetail = newAccountPayableBill.APB_BillDirectCode != BillDirectionEnum.Name.PLUS,
                        APBD_SourceBillNo = newAccountPayableBill.APB_SourceBillNo,
                        //[来源单据明细ID]为[入库单明细].[ID]
                        APBD_SourceBillDetailID = newStockInDetail.SID_ID,
                        APBD_Org_ID = newAccountPayableBill.APB_Org_ID,
                        APBD_Org_Name = newAccountPayableBill.APB_Org_Name,
                        //应付金额 = 本次签收数量 * 订货单价
                        APBD_AccountPayableAmount = Math.Round((loopDetail.ThisReceivedQty ?? 0) * (loopDetail.POD_UnitPrice ?? 0), 2),
                        //已付金额 = 0
                        APBD_PaidAmount = Convert.ToDecimal(0),
                        APBD_BusinessStatusCode = newAccountPayableBill.APB_SourceTypeCode,
                        APBD_BusinessStatusName = newAccountPayableBill.APB_SourceTypeName,
                        APBD_ApprovalStatusCode = newAccountPayableBill.APB_ApprovalStatusCode,
                        APBD_ApprovalStatusName = newAccountPayableBill.APB_ApprovalStatusName,
                        APBD_IsValid = true,
                        APBD_CreatedBy = LoginInfoDAX.UserName,
                        APBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                        APBD_UpdatedBy = LoginInfoDAX.UserName,
                        APBD_UpdatedTime = BLLCom.GetCurStdDatetime()
                    };
                    //未付金额 = 应付金额
                    newAccountPayableBillDetail.APBD_UnpaidAmount = newAccountPayableBillDetail.APBD_AccountPayableAmount;
                    newAccountPayableBillDetailList.Add(newAccountPayableBillDetail);

                    //应付单.[应付金额]
                    newAccountPayableBill.APB_AccountPayableAmount = newAccountPayableBill.APB_AccountPayableAmount
                        + newAccountPayableBillDetail.APBD_AccountPayableAmount;
                    //应付单.[未付金额]
                    newAccountPayableBill.APB_UnpaidAmount = newAccountPayableBill.APB_AccountPayableAmount;

                    #endregion

                    #region 待更新的[采购明细]数据
                    if (loopDetail.POD_ReceivedQty == null)
                    {
                        loopDetail.POD_ReceivedQty = 0;
                    }
                    //签收数量 = 原签收数量 + 本次签收数量
                    loopDetail.POD_ReceivedQty = loopDetail.POD_ReceivedQty + loopDetail.ThisReceivedQty;
                    loopDetail.ThisReceivedQty = 0;
                    //单据状态为{已签收}
                    loopDetail.POD_StatusName = PurchaseOrderDetailStatusEnum.Name.YQS;
                    loopDetail.POD_StatusCode = PurchaseOrderDetailStatusEnum.Code.YQS;
                    loopDetail.POD_ReceivedTime = BLLCom.GetCurStdDatetime();
                    loopDetail.POD_UpdatedBy = LoginInfoDAX.UserName;
                    loopDetail.POD_UpdatedTime = BLLCom.GetCurStdDatetime();
                    #endregion

                }
                #endregion
            }

            #endregion

            #region 遍历[入库单明细]列表，创建或更新[库存]，创建[库存异动日志]

            foreach (var loopStockInBillDetail in newStockInDetailList)
            {
                if (string.IsNullOrEmpty(loopStockInBillDetail.SID_Barcode))
                {
                    continue;
                }

                #region 入库单明细

                //生成批次号
                loopStockInBillDetail.SID_BatchNo = BLLCom.GetBatchNo(new MDLPIS_Inventory
                {
                    WHERE_INV_Org_ID = LoginInfoDAX.OrgID,
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
                    //生成[库存异动日志]
                    newInventoryTransLogList.Add(GenerateInventoryTransLog(newStockInBill, loopStockInBillDetail, inventoryExists, paramHead, paramDetailList));
                }
                else
                {
                    //[入库单明细]列表中第一次出现该配件
                    //查询该配件是否在[库存]中存在
                    MDLPIS_Inventory resultInventory = new MDLPIS_Inventory();
                    _bll.QueryForObject<MDLPIS_Inventory, MDLPIS_Inventory>(new MDLPIS_Inventory
                    {
                        WHERE_INV_Org_ID = newStockInBill.SIB_Org_ID,
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
                            INV_Org_ID = newStockInBill.SIB_Org_ID,
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
                        newInventoryTransLogList.Add(GenerateInventoryTransLog(newStockInBill, loopStockInBillDetail, inventoryToInsert, paramHead, paramDetailList));
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
                        newInventoryTransLogList.Add(GenerateInventoryTransLog(newStockInBill, loopStockInBillDetail, resultInventory, paramHead, paramDetailList));
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
                if (string.IsNullOrEmpty(loopPicture.INVP_PictureName)
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

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 更新[采购单]

                bool updatePurchaseOrderResult = Save(updatePurchaseOrder);
                if (!updatePurchaseOrderResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    foreach (var loopInventoryPicture in savePictureList)
                    {
                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopInventoryPicture.INVP_PictureName, ref outMsg);
                    }

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.UNAPPROVE + SystemTableEnums.Name.PIS_PurchaseOrder });
                    return false;
                }

                #endregion

                #region 更新[采购单明细]

                bool updatePurchaseOrderDetailResult = _bll.UnitySave(paramDetailList);
                if (!updatePurchaseOrderDetailResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    foreach (var loopInventoryPicture in savePictureList)
                    {
                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopInventoryPicture.INVP_PictureName, ref outMsg);
                    }

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.PIS_PurchaseOrderDetail });
                    return false;
                }

                #endregion

                #region 待新增的[入库单]、[入库单明细]列表
                if (newStockInDetailList.Count > 0)
                {
                    #region 待新增的[入库单]

                    bool insertStockInBillResult = _bll.Save(newStockInBill, newStockInBill.SIB_ID);
                    if (!insertStockInBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);

                        foreach (var loopInventoryPicture in savePictureList)
                        {
                            //保存失败，删除本地以及文件服务器上的图片
                            var outMsg = string.Empty;
                            BLLCom.DeleteFileByFileName(loopInventoryPicture.INVP_PictureName, ref outMsg);
                        }

                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.PIS_StockInBill });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                    #endregion

                    #region 待新增的[入库单明细]列表
                    bool insertStockInDetailResult = _bll.InsertByList<MDLPIS_StockInDetail, MDLPIS_StockInDetail>(newStockInDetailList);
                    if (!insertStockInDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.PIS_StockInDetail });

                        foreach (var loopInventoryPicture in savePictureList)
                        {
                            //保存失败，删除本地以及文件服务器上的图片
                            var outMsg = string.Empty;
                            BLLCom.DeleteFileByFileName(loopInventoryPicture.INVP_PictureName, ref outMsg);
                        }

                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                    #endregion
                }

                #endregion

                #region 待新增的[应付单]、[应付单明细]列表
                if (newAccountPayableBillDetailList.Count > 0)
                {
                    #region 待新增的[应付单]

                    bool insertAccountPayableBillResult = _bll.Save(newAccountPayableBill, newAccountPayableBill.APB_ID);
                    if (!insertAccountPayableBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.FM_AccountPayableBill });

                        foreach (var loopInventoryPicture in savePictureList)
                        {
                            //保存失败，删除本地以及文件服务器上的图片
                            var outMsg = string.Empty;
                            BLLCom.DeleteFileByFileName(loopInventoryPicture.INVP_PictureName, ref outMsg);
                        }

                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                    #endregion

                    #region 待新增的[应付单明细]列表

                    bool insertAccountPayableDetailResult = _bll.InsertByList<MDLFM_AccountPayableBillDetail, MDLFM_AccountPayableBillDetail>(newAccountPayableBillDetailList);
                    if (!insertAccountPayableDetailResult)
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
                    #endregion
                }
                #endregion

                #region 保存[库存]

                foreach (var loopInventory in saveInventoryList)
                {
                    bool saveInventoryResult = _bll.Save(loopInventory);
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
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }

            #endregion

            //将最新数据回写给DetailDS
            CopyModel(updatePurchaseOrder, paramHead);
            //更新明细版本号
            foreach (var loopUpdateDetail in paramDetailList.UpdateList)
            {
                //更新时版本号加1
                loopUpdateDetail.POD_VersionNo = loopUpdateDetail.POD_VersionNo + 1;
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
                var thisSavePicture = savePictureList.FirstOrDefault(x => x.INVP_PictureName == loopPicture.INVP_PictureName
                && x.INVP_SourceTypeName == InventoryPictureSourceTypeEnum.Name.PURCHASEDETAIL);
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
        /// 核实
        /// </summary>
        /// <param name="paramHead">单头UIModel</param>
        /// <param name="paramDetailList">明细UIModel列表</param>
        /// <returns></returns>
        public bool VerifyDetailDS(PurchaseOrderManagerUIModel paramHead,
            SkyCarBindingList<PurchaseOrderManagerDetailUIModel, MDLPIS_PurchaseOrderDetail> paramDetailList)
        {
            #region 准备数据

            #region 待更新[采购单]
            //待更新[采购单]
            var updatePurchaseOrder = paramHead.ToTBModelForSaveAndDelete<MDLPIS_PurchaseOrder>();

            //更新[单据状态]为{已完成}
            updatePurchaseOrder.PO_StatusName = PurchaseOrderStatusEnum.Name.YWC;
            updatePurchaseOrder.PO_StatusCode = PurchaseOrderStatusEnum.Code.YWC;
            updatePurchaseOrder.PO_UpdatedBy = LoginInfoDAX.UserName;
            updatePurchaseOrder.PO_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region 待更新的[采购单明细]

            foreach (var loopDetail in paramDetailList)
            {
                //更新[单据状态]为{已完成}
                loopDetail.POD_StatusName = PurchaseOrderDetailStatusEnum.Name.YWC;
                loopDetail.POD_StatusCode = PurchaseOrderDetailStatusEnum.Code.YWC;
                loopDetail.POD_UpdatedBy = LoginInfoDAX.UserName;
                loopDetail.POD_UpdatedTime = BLLCom.GetCurStdDatetime();
            }

            #endregion

            #endregion

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 更新[采购单]

                bool updatePurchaseOrderResult = Save(updatePurchaseOrder);
                if (!updatePurchaseOrderResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.UNAPPROVE + SystemTableEnums.Name.PIS_PurchaseOrder });
                    return false;
                }

                #endregion

                #region 更新[采购单明细]

                bool updatePurchaseOrderDetailResult = _bll.UnitySave(paramDetailList);
                if (!updatePurchaseOrderDetailResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.PIS_PurchaseOrderDetail });
                    return false;
                }

                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.APPROVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }

            #endregion

            //将最新数据回写给DetailDS
            CopyModel(updatePurchaseOrder, paramHead);
            //更新明细版本号
            foreach (var loopUpdateDetail in paramDetailList.UpdateList)
            {
                //更新时版本号加1
                loopUpdateDetail.POD_VersionNo = loopUpdateDetail.POD_VersionNo + 1;
            }
            return true;
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 【签收】生成库存异动日志
        /// </summary>
        /// <param name="paramStockInBill">入库单</param>
        /// <param name="paramStockInBillDetail">入库单明细</param>
        /// <param name="paramInventory">库存</param>
        /// <param name="paramHead">采购单</param>
        /// <param name="paramPurchaseOrderManagerDetail">采购单明细</param>
        /// <returns></returns>
        private MDLPIS_InventoryTransLog GenerateInventoryTransLog(MDLPIS_StockInBill paramStockInBill, MDLPIS_StockInDetail paramStockInBillDetail, MDLPIS_Inventory paramInventory, PurchaseOrderManagerUIModel paramHead, SkyCarBindingList<PurchaseOrderManagerDetailUIModel, MDLPIS_PurchaseOrderDetail> paramPurchaseOrderManagerDetailList)
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
            newInventoryTransLog.ITL_Source = paramHead.PO_SUPP_Name;
            if (paramPurchaseOrderManagerDetailList.Count > 0)
            {
                foreach (var loopPurchaseOrderManagerDetail in paramPurchaseOrderManagerDetailList)
                {
                    if (loopPurchaseOrderManagerDetail.POD_WH_ID == paramStockInBillDetail.SID_WH_ID)
                    {
                        newInventoryTransLog.ITL_Destination = loopPurchaseOrderManagerDetail.WH_Name;
                        break;
                    }
                }
            }
            return newInventoryTransLog;
        }

        #endregion
    }
}
