using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.SD;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.DAL;
using System.Reflection;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.SD.APModel;


namespace SkyCar.Coeus.BLL.SD
{
    /// <summary>
    /// 物流单管理BLL
    /// </summary>
    public class LogisticsBillManagerBLL : BLLBase
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
        /// 物流单管理BLL
        /// </summary>
        public LogisticsBillManagerBLL() : base(Trans.SD)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramHead">UIModel</param>
        /// <param name="paramDetailList">明细List</param>
        /// <param name="paramPictureNameAndPath"></param>
        /// <returns></returns>
        public bool SaveDetailDS(LogisticsBillManagerUIModel paramHead, SkyCarBindingList<LogisticsBillDetailManagerUIModel, MDLSD_LogisticsBillDetail> paramDetailList,
            Dictionary<string, string> paramPictureNameAndPath)
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

            var argsHead = paramHead.ToTBModelForSaveAndDelete<MDLSD_LogisticsBill>();
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(paramHead.LB_ID))
            {
                argsHead.LB_ID = Guid.NewGuid().ToString();
                //物流单号
                argsHead.LB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.LB);
                argsHead.LB_CreatedBy = LoginInfoDAX.UserName;
                argsHead.LB_CreatedTime = BLLCom.GetCurStdDatetime();
            }
            argsHead.LB_UpdatedBy = LoginInfoDAX.UserName;
            argsHead.LB_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region 明细
            //添加的明细
            if (paramDetailList != null && paramDetailList.InsertList != null && paramDetailList.InsertList.Count > 0)
            {
                foreach (var loopLogisticsBillDetail in paramDetailList.InsertList)
                {
                    loopLogisticsBillDetail.LBD_LB_ID = argsHead.LB_ID ?? argsHead.WHERE_LB_ID;
                    loopLogisticsBillDetail.LBD_LB_No = argsHead.LB_No;
                }
            }
            #endregion

            #region 上传图片

            foreach (var loopPicture in paramPictureNameAndPath)
            {
                if (string.IsNullOrEmpty(loopPicture.Key)
                    || string.IsNullOrEmpty(loopPicture.Value))
                {
                    continue;
                }

                #region 将图片保存到本地以及上传文件服务器

                string fileNetUrl = string.Empty;
                bool savePictureResult = BLLCom.SaveFileByFileName(loopPicture.Value, loopPicture.Key, ref fileNetUrl);
                if (!savePictureResult)
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + MsgParam.IMAGE });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                //截取上传图片返回值中的文件名称
                int fileNameStartIndex = fileNetUrl.IndexOf("FileName=", StringComparison.Ordinal) + 1;
                int fileNameEndIndex = fileNameStartIndex + "FileName=".Length;
                int length = fileNetUrl.Length;
                //文件名称
                string tempFileName = fileNetUrl.Substring(fileNameEndIndex - 1, length - (fileNameEndIndex - 1));

                //给各个图片赋值
                if (loopPicture.Key == argsHead.LB_AcceptPicPath1)
                {
                    argsHead.LB_AcceptPicPath1 = tempFileName;
                }
                else if (loopPicture.Key == argsHead.LB_AcceptPicPath2)
                {
                    argsHead.LB_AcceptPicPath2 = tempFileName;
                }
                else if (loopPicture.Key == argsHead.LB_ReceivedPicPath1)
                {
                    argsHead.LB_ReceivedPicPath1 = tempFileName;
                }
                else if (loopPicture.Key == argsHead.LB_ReceivedPicPath2)
                {
                    argsHead.LB_ReceivedPicPath2 = tempFileName;
                }

            }
            #endregion

            #region 物流订单异动日志

            //新增状态为 当前物流单状态 的[物流单异动日志]
            MDLSD_LogisticsBillTrans newLogisticsBillTrans = new MDLSD_LogisticsBillTrans
            {
                LBT_ID = Guid.NewGuid().ToString(),
                LBT_Org_ID = argsHead.LB_Org_ID,
                LBT_Org_Name = argsHead.LB_Org_Name,
                LBT_LB_ID = argsHead.LB_ID ?? argsHead.WHERE_LB_ID,
                LBT_LB_NO = argsHead.LB_No,
                LBT_Time = BLLCom.GetCurStdDatetime(),
                LBT_Status = argsHead.LB_StatusName,
                LBT_IsValid = true,
                LBT_CreatedBy = LoginInfoDAX.UserName,
                LBT_CreatedTime = BLLCom.GetCurStdDatetime(),
                LBT_UpdatedBy = LoginInfoDAX.UserName,
                LBT_UpdatedTime = BLLCom.GetCurStdDatetime()
            };

            #endregion

            #endregion

            #region 事务处理
            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存[物流单]

                //执行保存
                if (!_bll.Save(argsHead, argsHead.LB_ID))
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    foreach (var loopPicture in paramPictureNameAndPath)
                    {
                        if (string.IsNullOrEmpty(loopPicture.Key)
                            || string.IsNullOrEmpty(loopPicture.Value))
                        {
                            continue;
                        }
                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopPicture.Key, ref outMsg);
                    }

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_LogisticsBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存[物流单明细]

                //执行保存
                if (!_bll.UnitySave(paramDetailList))
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    foreach (var loopPicture in paramPictureNameAndPath)
                    {
                        if (string.IsNullOrEmpty(loopPicture.Key)
                            || string.IsNullOrEmpty(loopPicture.Value))
                        {
                            continue;
                        }
                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopPicture.Key, ref outMsg);
                    }

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.SD_LogisticsBillDetail });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 新增[物流单日志]
                bool addLogisticsBillTransResult = _bll.Insert(newLogisticsBillTrans);
                if (!addLogisticsBillTransResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    foreach (var loopPicture in paramPictureNameAndPath)
                    {
                        if (string.IsNullOrEmpty(loopPicture.Key)
                            || string.IsNullOrEmpty(loopPicture.Value))
                        {
                            continue;
                        }
                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopPicture.Key, ref outMsg);
                    }

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.SD_LogisticsBillTrans });
                    return false;
                }
                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);

                foreach (var loopPicture in paramPictureNameAndPath)
                {
                    if (string.IsNullOrEmpty(loopPicture.Key)
                        || string.IsNullOrEmpty(loopPicture.Value))
                    {
                        continue;
                    }
                    //保存失败，删除本地以及文件服务器上的图片
                    var outMsg = string.Empty;
                    BLLCom.DeleteFileByFileName(loopPicture.Key, ref outMsg);
                }

                //保存[物流单管理]信息发生异常
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }
            #endregion

            //将最新数据回写给DetailDS argsLogisticsBill
            CopyModel(argsHead, paramHead);

            //更新明细版本号
            if (paramDetailList != null)
            {
                if (paramDetailList.InsertList != null)
                {
                    foreach (var loopInsertDetail in paramDetailList.InsertList)
                    {
                        //新增时版本号为1
                        loopInsertDetail.LBD_VersionNo = 1;
                    }
                }

                foreach (var loopUpdateDetail in paramDetailList.UpdateList)
                {
                    //更新时版本号加1
                    loopUpdateDetail.LBD_VersionNo = loopUpdateDetail.LBD_VersionNo + 1;
                }
            }

            foreach (var loopPicture in paramPictureNameAndPath)
            {
                if (string.IsNullOrEmpty(loopPicture.Key)
                    || string.IsNullOrEmpty(loopPicture.Value))
                {
                    continue;
                }
                //保存成功，删除临时保存的图片
                if (File.Exists(loopPicture.Value))
                {
                    File.Delete(loopPicture.Value);
                }
            }
            return true;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="paramHead">物流单</param>
        /// <param name="paramDetailList">物流单明细列表</param>
        /// <returns></returns>
        public bool ApproveDetailDS(LogisticsBillManagerUIModel paramHead, SkyCarBindingList<LogisticsBillDetailManagerUIModel, MDLSD_LogisticsBillDetail> paramDetailList,
            Dictionary<string, string> paramPictureNameAndPath)
        {
            var funcName = "ApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.LB_ID)
                || string.IsNullOrEmpty(paramHead.LB_No))
            {
                //没有获取到物流单，审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.SD_LogisticsBill, SystemActionEnum.Name.APPROVE });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            #region 准备数据

            #region 变量定义

            //待更新的当前[物流单]
            MDLSD_LogisticsBill updateCurLogisticsBill = new MDLSD_LogisticsBill();
            updateCurLogisticsBill = paramHead.ToTBModelForSaveAndDelete<MDLSD_LogisticsBill>();
            //待更新的当前[物流单明细]列表
            List<MDLSD_LogisticsBillDetail> updateCurLogisticsBillDetailList = new List<MDLSD_LogisticsBillDetail>();
            CopyModelList(paramDetailList, updateCurLogisticsBillDetailList);

            //待新增的当前[物流单异动日志]
            MDLSD_LogisticsBillTrans newLogisticsBillTrans = new MDLSD_LogisticsBillTrans();

            //待更新的当前物流单对应的[销售订单]
            MDLSD_SalesOrder updateCurSalesOrder = new MDLSD_SalesOrder();
            //待更新的当前物流单对应的[销售订单明细]列表
            List<MDLSD_SalesOrderDetail> updateCurSalesOrderDetailList = new List<MDLSD_SalesOrderDetail>();
            //待更新的[调拨单]
            MDLPIS_TransferBill transferBill = new MDLPIS_TransferBill();

            #endregion

            #region 更新[物流单]
            //更新[物流单].[单据状态]为{配送中}，[审核状态]为{已审核}
            updateCurLogisticsBill.LB_StatusCode = LogisticsBillStatusEnum.Code.PSZ;
            updateCurLogisticsBill.LB_StatusName = LogisticsBillStatusEnum.Name.PSZ;
            updateCurLogisticsBill.LB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            updateCurLogisticsBill.LB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            updateCurLogisticsBill.LB_UpdatedBy = LoginInfoDAX.UserName;
            updateCurLogisticsBill.LB_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            #region 新增[物流单异动日志]
            //新增状态为 当前物流单状态 的[物流单异动日志]
            newLogisticsBillTrans.LBT_ID = Guid.NewGuid().ToString();
            newLogisticsBillTrans.LBT_Org_ID = updateCurLogisticsBill.LB_Org_ID;
            newLogisticsBillTrans.LBT_Org_Name = updateCurLogisticsBill.LB_Org_Name;
            newLogisticsBillTrans.LBT_LB_ID = updateCurLogisticsBill.LB_ID ?? updateCurLogisticsBill.WHERE_LB_ID;
            newLogisticsBillTrans.LBT_LB_NO = updateCurLogisticsBill.LB_No;
            newLogisticsBillTrans.LBT_Time = BLLCom.GetCurStdDatetime();
            newLogisticsBillTrans.LBT_Status = updateCurLogisticsBill.LB_StatusName;
            newLogisticsBillTrans.LBT_IsValid = true;
            newLogisticsBillTrans.LBT_CreatedBy = LoginInfoDAX.UserName;
            newLogisticsBillTrans.LBT_CreatedTime = BLLCom.GetCurStdDatetime();
            newLogisticsBillTrans.LBT_UpdatedBy = LoginInfoDAX.UserName;
            newLogisticsBillTrans.LBT_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            if (paramHead.LB_SourceTypeName == DeliveryBillSourceTypeEnum.Name.ZZDB)
            {
                #region 更新[调拨单]

                _bll.QueryForObject<MDLPIS_TransferBill, MDLPIS_TransferBill>(new MDLPIS_TransferBill()
                {
                    WHERE_TB_No = paramHead.LB_SourceNo
                }, transferBill);
                if (!string.IsNullOrEmpty(transferBill.TB_ID))
                {
                    transferBill.TB_StatusName = TransfeStatusEnum.Name.YWC;
                    transferBill.TB_StatusCode = TransfeStatusEnum.Code.YWC;
                    transferBill.WHERE_TB_ID = transferBill.TB_ID;
                    transferBill.WHERE_TB_VersionNo = transferBill.TB_VersionNo;
                    transferBill.TB_VersionNo = +1;
                }

                #endregion
            }
            else
            {
                #region 更新[销售订单] 
                //查询物流单对应的销售订单
                _bll.QueryForObject<MDLSD_SalesOrder, MDLSD_SalesOrder>(new MDLSD_SalesOrder
                {
                    WHERE_SO_No = updateCurLogisticsBill.LB_SourceNo,
                    WHERE_SO_IsValid = true
                }, updateCurSalesOrder);
                if (!string.IsNullOrEmpty(updateCurSalesOrder.SO_ID))
                {
                    //更新[销售订单].[单据状态]为{已发货}
                    updateCurSalesOrder.SO_StatusCode = SalesOrderStatusEnum.Code.YFH;
                    updateCurSalesOrder.SO_StatusName = SalesOrderStatusEnum.Name.YFH;
                    updateCurSalesOrder.SO_UpdatedBy = LoginInfoDAX.UserName;
                    updateCurSalesOrder.SO_UpdatedTime = BLLCom.GetCurStdDatetime();
                    updateCurSalesOrder.WHERE_SO_ID = updateCurSalesOrder.SO_ID;
                    updateCurSalesOrder.WHERE_SO_VersionNo = updateCurSalesOrder.SO_VersionNo;
                }
                #endregion

                #region 获取对应的[销售订单明细]列表

                _bll.QueryForList<MDLSD_SalesOrderDetail, MDLSD_SalesOrderDetail>(new MDLSD_SalesOrderDetail
                {
                    WHERE_SOD_SO_ID = updateCurSalesOrder.SO_ID,
                    WHERE_SOD_IsValid = true
                }, updateCurSalesOrderDetailList);
                #endregion
            }

            #region 上传图片

            foreach (var loopPicture in paramPictureNameAndPath)
            {
                if (string.IsNullOrEmpty(loopPicture.Key)
                    || string.IsNullOrEmpty(loopPicture.Value))
                {
                    continue;
                }

                #region 将图片保存到本地以及上传文件服务器

                string fileNetUrl = string.Empty;
                bool savePictureResult = BLLCom.SaveFileByFileName(loopPicture.Value, loopPicture.Key, ref fileNetUrl);
                if (!savePictureResult)
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + MsgParam.IMAGE });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                //截取上传图片返回值中的文件名称
                int fileNameStartIndex = fileNetUrl.IndexOf("FileName=", StringComparison.Ordinal) + 1;
                int fileNameEndIndex = fileNameStartIndex + "FileName=".Length;
                int length = fileNetUrl.Length;
                //文件名称
                string tempFileName = fileNetUrl.Substring(fileNameEndIndex - 1, length - (fileNameEndIndex - 1));

                //给各个图片赋值
                if (loopPicture.Key == updateCurLogisticsBill.LB_AcceptPicPath1)
                {
                    updateCurLogisticsBill.LB_AcceptPicPath1 = tempFileName;
                }
                else if (loopPicture.Key == updateCurLogisticsBill.LB_AcceptPicPath2)
                {
                    updateCurLogisticsBill.LB_AcceptPicPath2 = tempFileName;
                }
                else if (loopPicture.Key == updateCurLogisticsBill.LB_ReceivedPicPath1)
                {
                    updateCurLogisticsBill.LB_ReceivedPicPath1 = tempFileName;
                }
                else if (loopPicture.Key == updateCurLogisticsBill.LB_ReceivedPicPath2)
                {
                    updateCurLogisticsBill.LB_ReceivedPicPath2 = tempFileName;
                }
            }
            #endregion

            #endregion

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 更新[物流单]
                bool updateLogisticsBillResult = _bll.Save(updateCurLogisticsBill);
                if (!updateLogisticsBillResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    foreach (var loopPicture in paramPictureNameAndPath)
                    {
                        if (string.IsNullOrEmpty(loopPicture.Key)
                            || string.IsNullOrEmpty(loopPicture.Value))
                        {
                            continue;
                        }
                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopPicture.Key, ref outMsg);
                    }

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.SD_LogisticsBill });
                    return false;
                }

                #endregion

                #region 更新[物流单明细]

                foreach (var loopLogisticsBillDetail in updateCurLogisticsBillDetailList)
                {
                    //更新[物流单明细].[物流状态]为{配送中}
                    loopLogisticsBillDetail.LBD_StatusCode = LogisticsBillDetailStatusEnum.Code.PSZ;
                    loopLogisticsBillDetail.LBD_StatusName = LogisticsBillDetailStatusEnum.Name.PSZ;
                    loopLogisticsBillDetail.LBD_UpdatedBy = LoginInfoDAX.UserName;
                    loopLogisticsBillDetail.LBD_UpdatedTime = BLLCom.GetCurStdDatetime();
                    if (paramHead.LB_SourceTypeName == DeliveryBillSourceTypeEnum.Name.ZZDB)
                    {
                        loopLogisticsBillDetail.LBD_SignQty = loopLogisticsBillDetail.LBD_DeliveryQty;
                    }
                    loopLogisticsBillDetail.WHERE_LBD_ID = loopLogisticsBillDetail.LBD_ID;
                    loopLogisticsBillDetail.WHERE_LBD_VersionNo = loopLogisticsBillDetail.LBD_VersionNo;

                    bool updateLogisticsBillDetailResult = _bll.Save(loopLogisticsBillDetail);
                    if (!updateLogisticsBillDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);

                        foreach (var loopPicture in paramPictureNameAndPath)
                        {
                            if (string.IsNullOrEmpty(loopPicture.Key)
                                || string.IsNullOrEmpty(loopPicture.Value))
                            {
                                continue;
                            }
                            //保存失败，删除本地以及文件服务器上的图片
                            var outMsg = string.Empty;
                            BLLCom.DeleteFileByFileName(loopPicture.Key, ref outMsg);
                        }

                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.SD_LogisticsBillDetail });
                        return false;
                    }
                }
                #endregion

                #region 新增[物流单日志]
                bool addLogisticsBillTransResult = _bll.Insert(newLogisticsBillTrans);
                if (!addLogisticsBillTransResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);

                    foreach (var loopPicture in paramPictureNameAndPath)
                    {
                        if (string.IsNullOrEmpty(loopPicture.Key)
                            || string.IsNullOrEmpty(loopPicture.Value))
                        {
                            continue;
                        }
                        //保存失败，删除本地以及文件服务器上的图片
                        var outMsg = string.Empty;
                        BLLCom.DeleteFileByFileName(loopPicture.Key, ref outMsg);
                    }

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.SD_LogisticsBillTrans });
                    return false;
                }
                #endregion

                if (paramHead.LB_SourceTypeName == DeliveryBillSourceTypeEnum.Name.ZZDB)
                {
                    #region 更新[调拨单]数据
                    if (!string.IsNullOrEmpty(transferBill.TB_ID))
                    {
                        bool updateTransferBillResult = _bll.Update(transferBill);
                        if (!updateTransferBillResult)
                        {
                            DBManager.RollBackTransaction(DBCONFIG.Coeus);

                            foreach (var loopPicture in paramPictureNameAndPath)
                            {
                                if (string.IsNullOrEmpty(loopPicture.Key)
                                    || string.IsNullOrEmpty(loopPicture.Value))
                                {
                                    continue;
                                }
                                //保存失败，删除本地以及文件服务器上的图片
                                var outMsg = string.Empty;
                                BLLCom.DeleteFileByFileName(loopPicture.Key, ref outMsg);
                            }

                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.PIS_TransferBill });
                            return false;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 更新[销售订单]数据
                    if (!string.IsNullOrEmpty(updateCurSalesOrder.SO_ID))
                    {
                        bool updateSalesOrderResult = _bll.Update(updateCurSalesOrder);
                        if (!updateSalesOrderResult)
                        {
                            DBManager.RollBackTransaction(DBCONFIG.Coeus);

                            foreach (var loopPicture in paramPictureNameAndPath)
                            {
                                if (string.IsNullOrEmpty(loopPicture.Key)
                                    || string.IsNullOrEmpty(loopPicture.Value))
                                {
                                    continue;
                                }
                                //保存失败，删除本地以及文件服务器上的图片
                                var outMsg = string.Empty;
                                BLLCom.DeleteFileByFileName(loopPicture.Key, ref outMsg);
                            }

                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.SD_SalesOrder });
                            return false;
                        }
                    }
                    #endregion

                    #region 更新[销售订单明细]数据

                    if (updateCurSalesOrderDetailList.Count > 0)
                    {
                        foreach (var loopSalesOrderDetail in updateCurSalesOrderDetailList)
                        {
                            //更新[销售订单明细].[单据状态]与单头一致
                            loopSalesOrderDetail.SOD_StatusName = updateCurSalesOrder.SO_StatusName;
                            loopSalesOrderDetail.SOD_StatusCode = updateCurSalesOrder.SO_StatusCode;
                            loopSalesOrderDetail.SOD_ApprovalStatusName = updateCurSalesOrder.SO_ApprovalStatusName;
                            loopSalesOrderDetail.SOD_ApprovalStatusCode = updateCurSalesOrder.SO_ApprovalStatusCode;
                            loopSalesOrderDetail.SOD_UpdatedBy = LoginInfoDAX.UserName;
                            loopSalesOrderDetail.SOD_UpdatedTime = BLLCom.GetCurStdDatetime();
                            loopSalesOrderDetail.WHERE_SOD_ID = loopSalesOrderDetail.SOD_ID;
                            loopSalesOrderDetail.WHERE_SOD_VersionNo = loopSalesOrderDetail.SOD_VersionNo;

                            bool updateLogisticsBillDetailResult = _bll.Update(loopSalesOrderDetail);
                            if (!updateLogisticsBillDetailResult)
                            {
                                DBManager.RollBackTransaction(DBCONFIG.Coeus);

                                foreach (var loopPicture in paramPictureNameAndPath)
                                {
                                    if (string.IsNullOrEmpty(loopPicture.Key)
                                        || string.IsNullOrEmpty(loopPicture.Value))
                                    {
                                        continue;
                                    }
                                    //保存失败，删除本地以及文件服务器上的图片
                                    var outMsg = string.Empty;
                                    BLLCom.DeleteFileByFileName(loopPicture.Key, ref outMsg);
                                }

                                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.SD_SalesOrderDetail });
                                return false;
                            }
                        }
                    }
                    #endregion
                }



                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);

                foreach (var loopPicture in paramPictureNameAndPath)
                {
                    if (string.IsNullOrEmpty(loopPicture.Key)
                        || string.IsNullOrEmpty(loopPicture.Value))
                    {
                        continue;
                    }
                    //保存失败，删除本地以及文件服务器上的图片
                    var outMsg = string.Empty;
                    BLLCom.DeleteFileByFileName(loopPicture.Key, ref outMsg);
                }

                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.APPROVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }

            #endregion

            //将最新数据回写给DetailDS
            CopyModel(updateCurLogisticsBill, paramHead);

            //更新明细版本号
            if (paramDetailList != null)
            {
                foreach (var loopUpdateDetail in paramDetailList)
                {
                    if (loopUpdateDetail.LBD_VersionNo == null)
                    {
                        //新增时版本号为1
                        loopUpdateDetail.LBD_VersionNo = 1;
                        if (paramHead.LB_SourceTypeName == DeliveryBillSourceTypeEnum.Name.ZZDB)
                        {
                            loopUpdateDetail.LBD_SignQty = loopUpdateDetail.LBD_DeliveryQty;
                        }
                    }
                    else
                    {
                        //更新时版本号加1
                        loopUpdateDetail.LBD_VersionNo = loopUpdateDetail.LBD_VersionNo + 1;
                        if (paramHead.LB_SourceTypeName == DeliveryBillSourceTypeEnum.Name.ZZDB)
                        {
                            loopUpdateDetail.LBD_SignQty = loopUpdateDetail.LBD_DeliveryQty;
                        }
                    }
                }
            }

            foreach (var loopPicture in paramPictureNameAndPath)
            {
                if (string.IsNullOrEmpty(loopPicture.Key)
                    || string.IsNullOrEmpty(loopPicture.Value))
                {
                    continue;
                }
                //保存成功，删除临时保存的图片
                if (File.Exists(loopPicture.Value))
                {
                    File.Delete(loopPicture.Value);
                }
            }
            return true;
        }

        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="paramHead">物流单</param>
        /// <param name="paramDetailList">物流单明细列表</param>
        /// <returns></returns>
        public bool UnApproveDetailDS(LogisticsBillManagerUIModel paramHead, SkyCarBindingList<LogisticsBillDetailManagerUIModel, MDLSD_LogisticsBillDetail> paramDetailList)
        {
            var funcName = "UnApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.LB_ID)
                || string.IsNullOrEmpty(paramHead.LB_No))
            {
                //没有获取到物流单，反审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.SD_LogisticsBill, SystemActionEnum.Name.UNAPPROVE });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            #region 准备数据

            #region 变量定义

            //待更新的当前[物流单]
            var updateCurLogisticsBill = paramHead.ToTBModelForSaveAndDelete<MDLSD_LogisticsBill>();
            //待更新的当前[物流单明细]列表
            List<MDLSD_LogisticsBillDetail> updateCurLogisticsBillDetailList = new List<MDLSD_LogisticsBillDetail>();
            CopyModelList(paramDetailList, updateCurLogisticsBillDetailList);

            //待新增的当前[物流单日志]
            MDLSD_LogisticsBillTrans newLogisticsBillTrans = new MDLSD_LogisticsBillTrans();

            //待更新的当前物流单对应的[销售单]
            MDLSD_SalesOrder updateSalesOrder = new MDLSD_SalesOrder();
            //待更新的当前物流单对应的[销售退货单明细]列表
            List<MDLSD_SalesOrderDetail> updateSalesOrderDetailList = new List<MDLSD_SalesOrderDetail>();
            //待更新的[调拨单]
            MDLPIS_TransferBill transferBill = new MDLPIS_TransferBill();

            #endregion

            #region 更新[物流单]

            //更新[物流单].[单据状态]为{已生成}，[审核状态]为{待审核}
            updateCurLogisticsBill.LB_StatusCode = LogisticsBillStatusEnum.Code.YSC;
            updateCurLogisticsBill.LB_StatusName = LogisticsBillStatusEnum.Name.YSC;
            updateCurLogisticsBill.LB_ApprovalStatusCode = ApprovalStatusEnum.Code.DSH;
            updateCurLogisticsBill.LB_ApprovalStatusName = ApprovalStatusEnum.Name.DSH;
            updateCurLogisticsBill.LB_UpdatedBy = LoginInfoDAX.UserName;
            updateCurLogisticsBill.LB_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            #region 新增的[物流单异动日志]
            //新增状态为当前物理状态的[物流单异动日志]
            newLogisticsBillTrans.LBT_ID = Guid.NewGuid().ToString();
            newLogisticsBillTrans.LBT_Org_ID = updateCurLogisticsBill.LB_Org_ID;
            newLogisticsBillTrans.LBT_Org_Name = updateCurLogisticsBill.LB_Org_Name;
            newLogisticsBillTrans.LBT_LB_ID = updateCurLogisticsBill.LB_ID ?? updateCurLogisticsBill.WHERE_LB_ID;
            newLogisticsBillTrans.LBT_LB_NO = updateCurLogisticsBill.LB_No;
            newLogisticsBillTrans.LBT_Time = BLLCom.GetCurStdDatetime();
            newLogisticsBillTrans.LBT_Status = updateCurLogisticsBill.LB_StatusName;
            newLogisticsBillTrans.LBT_IsValid = true;
            newLogisticsBillTrans.LBT_CreatedBy = LoginInfoDAX.UserName;
            newLogisticsBillTrans.LBT_CreatedTime = BLLCom.GetCurStdDatetime();
            newLogisticsBillTrans.LBT_UpdatedBy = LoginInfoDAX.UserName;
            newLogisticsBillTrans.LBT_UpdatedTime = BLLCom.GetCurStdDatetime();
            #endregion

            if (paramHead.LB_SourceTypeName == DeliveryBillSourceTypeEnum.Name.ZZDB)
            {
                #region 更新[调拨单]

                _bll.QueryForObject<MDLPIS_TransferBill, MDLPIS_TransferBill>(new MDLPIS_TransferBill()
                {
                    WHERE_TB_No = paramHead.LB_SourceNo
                }, transferBill);
                if (!string.IsNullOrEmpty(transferBill.TB_ID))
                {
                    transferBill.TB_StatusName = TransfeStatusEnum.Name.DFH;
                    transferBill.TB_StatusCode = TransfeStatusEnum.Code.DFH;
                    transferBill.WHERE_TB_ID = transferBill.TB_ID;
                    transferBill.WHERE_TB_VersionNo = transferBill.TB_VersionNo;
                    transferBill.TB_VersionNo = +1;
                }

                #endregion
            }
            else
            {
                #region 更新[销售订单]
                //查询物流单对应的销售订单
                _bll.QueryForObject<MDLSD_SalesOrder, MDLSD_SalesOrder>(new MDLSD_SalesOrder
                {
                    WHERE_SO_No = updateCurLogisticsBill.LB_SourceNo,
                    WHERE_SO_IsValid = true
                }, updateSalesOrder);
                //更新[销售订单].[单据状态]为{待发货}
                updateSalesOrder.SO_StatusCode = SalesOrderStatusEnum.Code.DFH;
                updateSalesOrder.SO_StatusName = SalesOrderStatusEnum.Name.DFH;
                updateSalesOrder.SO_UpdatedBy = LoginInfoDAX.UserName;
                updateSalesOrder.SO_UpdatedTime = BLLCom.GetCurStdDatetime();
                updateSalesOrder.WHERE_SO_ID = updateSalesOrder.SO_ID;
                updateSalesOrder.WHERE_SO_VersionNo = updateSalesOrder.SO_VersionNo;
                #endregion

                #region 获取对应的[销售订单明细]列表

                _bll.QueryForList<MDLSD_SalesOrderDetail, MDLSD_SalesOrderDetail>(new MDLSD_SalesOrderDetail
                {
                    WHERE_SOD_SO_ID = updateSalesOrder.SO_ID,
                    WHERE_SOD_IsValid = true
                }, updateSalesOrderDetailList);
                #endregion
            }

            #endregion

            #region 带事务的保存
            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存数据

                #region 更新[物流单]
                bool saveLogisticsBill = _bll.Save(updateCurLogisticsBill);
                if (!saveLogisticsBill)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.SD_LogisticsBill });
                    return false;
                }
                #endregion

                #region 更新[物流单明细]

                foreach (var loopLogisticsBillDetail in updateCurLogisticsBillDetailList)
                {
                    //更新[物流单明细].[物流状态]为{已生成}
                    loopLogisticsBillDetail.LBD_StatusCode = LogisticsBillDetailStatusEnum.Code.YSC;
                    loopLogisticsBillDetail.LBD_StatusName = LogisticsBillDetailStatusEnum.Name.YSC;
                    loopLogisticsBillDetail.LBD_UpdatedBy = LoginInfoDAX.UserName;
                    loopLogisticsBillDetail.LBD_UpdatedTime = BLLCom.GetCurStdDatetime();
                    loopLogisticsBillDetail.WHERE_LBD_ID = loopLogisticsBillDetail.LBD_ID;
                    loopLogisticsBillDetail.WHERE_LBD_VersionNo = loopLogisticsBillDetail.LBD_VersionNo;

                    bool updateLogisticsBillDetailResult = _bll.Update(loopLogisticsBillDetail);
                    if (!updateLogisticsBillDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.SD_LogisticsBillDetail });
                        return false;
                    }
                }
                #endregion

                #region 新增的[物流单日志]
                bool addLogisticsBillTransResult = _bll.Insert(newLogisticsBillTrans);
                if (!addLogisticsBillTransResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.SD_LogisticsBillTrans });
                    return false;
                }
                #endregion

                if (paramHead.LB_SourceTypeName == DeliveryBillSourceTypeEnum.Name.ZZDB)
                {
                    #region 更新[调拨单]数据
                    if (!string.IsNullOrEmpty(transferBill.TB_ID))
                    {
                        bool updateTransferBillResult = _bll.Update(transferBill);
                        if (!updateTransferBillResult)
                        {
                            DBManager.RollBackTransaction(DBCONFIG.Coeus);
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.PIS_TransferBill });
                            return false;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 更新[销售订单]
                    if (!string.IsNullOrEmpty(updateSalesOrder.SO_ID))
                    {
                        bool updateSalesOrderResult = _bll.Update(updateSalesOrder);
                        if (!updateSalesOrderResult)
                        {
                            DBManager.RollBackTransaction(DBCONFIG.Coeus);
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.SD_SalesOrder });
                            return false;
                        }
                    }
                    #endregion

                    #region 更新[销售订单明细]

                    if (updateSalesOrderDetailList.Count > 0)
                    {
                        foreach (var loopSalesOrderDetail in updateSalesOrderDetailList)
                        {
                            //更新[销售订单明细].[单据状态]与单头一致
                            loopSalesOrderDetail.SOD_StatusName = updateSalesOrder.SO_StatusName;
                            loopSalesOrderDetail.SOD_StatusCode = updateSalesOrder.SO_StatusCode;
                            loopSalesOrderDetail.SOD_ApprovalStatusName = updateSalesOrder.SO_ApprovalStatusName;
                            loopSalesOrderDetail.SOD_ApprovalStatusCode = updateSalesOrder.SO_ApprovalStatusCode;
                            loopSalesOrderDetail.SOD_UpdatedBy = LoginInfoDAX.UserName;
                            loopSalesOrderDetail.SOD_UpdatedTime = BLLCom.GetCurStdDatetime();
                            loopSalesOrderDetail.WHERE_SOD_ID = loopSalesOrderDetail.SOD_ID;
                            loopSalesOrderDetail.WHERE_SOD_VersionNo = loopSalesOrderDetail.SOD_VersionNo;

                            bool updateLogisticsBillDetailResult = _bll.Update(loopSalesOrderDetail);
                            if (!updateLogisticsBillDetailResult)
                            {
                                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.SD_SalesOrderDetail });
                                return false;
                            }
                        }
                    }
                    #endregion

                }

                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.UNAPPROVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }
            #endregion

            //将最新数据回写给DetailDS
            CopyModel(updateCurLogisticsBill, paramHead);

            //更新明细版本号
            if (paramDetailList != null)
            {
                foreach (var loopUpdateDetail in paramDetailList)
                {
                    if (loopUpdateDetail.LBD_VersionNo == null)
                    {
                        //新增时版本号为1
                        loopUpdateDetail.LBD_VersionNo = 1;
                    }
                    else
                    {
                        //更新时版本号加1
                        loopUpdateDetail.LBD_VersionNo = loopUpdateDetail.LBD_VersionNo + 1;
                    }
                }
            }

            return true;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramHead">UIModel</param>
        /// <param name="paramDetailList">明细列表</param>
        /// <returns></returns>
        private bool ServerCheck(LogisticsBillManagerUIModel paramHead, SkyCarBindingList<LogisticsBillDetailManagerUIModel, MDLSD_LogisticsBillDetail> paramDetailList)
        {
            return true;
        }

        #endregion
    }
}
