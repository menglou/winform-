using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.FM;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.DAL;
using System.Reflection;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.BLL.FM
{
    /// <summary>
    /// 收款单管理BLL
    /// </summary>
    public class ReceiptBillManagerBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.FM);
        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 收款单管理BLL
        /// </summary>
        public ReceiptBillManagerBLL() : base(Trans.FM)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramHead"></param>
        /// <param name="paramDetailList"></param>
        /// <param name="paramPictureNameAndPath"></param>
        /// <returns></returns>
        public bool SaveDetailDS(ReceiptBillManagerUIModel paramHead, SkyCarBindingList<ReceiptBillManagerDetailUIModel, MDLFM_ReceiptBillDetail> paramDetailList,
            Dictionary<string, string> paramPictureNameAndPath)
        {
            var funcName = "SaveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);
            //服务端检查
            if (!ServerCheck(paramHead, paramDetailList))
            {
                return false;
            }

            #region 准备数据

            #region 单头
            //将UIModel转为TBModel
            var argsHead = paramHead.ToTBModelForSaveAndDelete<MDLFM_ReceiptBill>();
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(paramHead.RB_ID))
            {
                argsHead.RB_ID = Guid.NewGuid().ToString();
                //单号
                argsHead.RB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.RB);
                argsHead.RB_CreatedBy = LoginInfoDAX.UserName;
                argsHead.RB_CreatedTime = BLLCom.GetCurStdDatetime();
            }
            argsHead.RB_UpdatedBy = LoginInfoDAX.UserName;
            argsHead.RB_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region 明细

            //添加的明细
            if (paramDetailList != null && paramDetailList.InsertList != null &&
                paramDetailList.InsertList.Count > 0)
            {
                foreach (var loopDetailItem in paramDetailList)
                {
                    loopDetailItem.RBD_RB_ID = argsHead.RB_ID ?? argsHead.WHERE_RB_ID;
                    loopDetailItem.RBD_RB_No = argsHead.RB_No;
                    loopDetailItem.RBD_CreatedBy = LoginInfoDAX.UserName;
                    loopDetailItem.RBD_CreatedTime = BLLCom.GetCurStdDatetime();
                    loopDetailItem.RBD_UpdatedBy = LoginInfoDAX.UserName;
                    loopDetailItem.RBD_UpdatedTime = BLLCom.GetCurStdDatetime();
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
                if (loopPicture.Key == argsHead.RB_CertificatePic)
                {
                    argsHead.RB_CertificatePic = tempFileName;
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
                if (!_bll.Save(argsHead, argsHead.RB_ID))
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

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.FM_ReceiptBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存明细

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

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.FM_ReceiptBillDetail });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
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
                        loopInsertDetail.RBD_VersionNo = 1;
                    }
                }

                foreach (var loopUpdateDetail in paramDetailList.UpdateList)
                {
                    //更新时版本号加1
                    loopUpdateDetail.RBD_VersionNo = loopUpdateDetail.RBD_VersionNo + 1;
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
        /// <param name="paramHead">UIModel</param>
        /// <param name="paramDetailList">明细UIModel</param>
        /// <returns></returns>
        public bool ApproveDetailDS(ReceiptBillManagerUIModel paramHead, SkyCarBindingList<ReceiptBillManagerDetailUIModel, MDLFM_ReceiptBillDetail> paramDetailList)
        {
            var funcName = "ApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.RB_ID)
                || string.IsNullOrEmpty(paramHead.RB_No))
            {
                //没有获取到收款单，审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.FM_ReceiptBill, SystemActionEnum.Name.APPROVE });
                return false;
            }

            var receiptBillManagerDetailList =
                paramDetailList.Where(x => x.RBD_SourceTypeName == ReceiptBillDetailSourceTypeEnum.Name.SGSK);
            if (receiptBillManagerDetailList.Count() != paramDetailList.Count() || receiptBillManagerDetailList.Count() == 0)
            {
                //收款单不为手工收款，不能审核
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.FM_ReceiptBill + MsgParam.NO + MsgParam.BE + PayBillDetailSourceTypeEnum.Name.SGFK, SystemActionEnum.Name.APPROVE });
                return false;
            }

            #region 定义变量

            //待更新的[收款单]
            MDLFM_ReceiptBill updateReceiptBill = paramHead.ToTBModelForSaveAndDelete<MDLFM_ReceiptBill>();
            //待更新[收款单明细]
            List<MDLFM_ReceiptBillDetail> updateReceiptBillDetailList = new List<MDLFM_ReceiptBillDetail>();
            //待新增对应的[应收单]
            List<MDLFM_AccountReceivableBill> addAccountReceivableBillList = new List<MDLFM_AccountReceivableBill>();
            //待新增对应的[应收单明细]
            List<MDLFM_AccountReceivableBillDetail> addAccountReceivableBillDetailList = new List<MDLFM_AccountReceivableBillDetail>();
            //待更新的[钱包]
            MDLEWM_Wallet updateWallet = new MDLEWM_Wallet();
            //待新增的[钱包异动]
            MDLEWM_WalletTrans addWalletTrans = new MDLEWM_WalletTrans();
            #endregion

            #region 准备数据

            if (paramDetailList[0].RBD_SourceTypeName == ReceiptBillDetailSourceTypeEnum.Name.SGSK)
            {
                #region 新增[应收单]和[应收单明细]

                foreach (var loopDetail in paramDetailList)
                {
                    #region 新增[应收单]

                    MDLFM_AccountReceivableBill accountReceivableBill = new MDLFM_AccountReceivableBill
                    {
                        ARB_ID = System.Guid.NewGuid().ToString(),
                        ARB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.ARB),
                        ARB_BillDirectCode = BillDirectionEnum.Code.PLUS,
                        ARB_BillDirectName = BillDirectionEnum.Name.PLUS,
                        ARB_SourceTypeCode = AccountReceivableBillSourceTypeEnum.Code.SGCJ,
                        ARB_SourceTypeName = AccountReceivableBillSourceTypeEnum.Name.SGCJ,
                        ARB_Org_ID = LoginInfoDAX.OrgID,
                        ARB_Org_Name = LoginInfoDAX.OrgShortName,
                        ARB_PayObjectTypeCode = paramHead.RB_PayObjectTypeCode,
                        ARB_PayObjectTypeName = paramHead.RB_PayObjectTypeName,
                        ARB_PayObjectID = paramHead.RB_PayObjectID,
                        ARB_PayObjectName = paramHead.RB_PayObjectName,
                        ARB_AccountReceivableAmount = loopDetail.RBD_ReceiveAmount,
                        ARB_ReceivedAmount = loopDetail.RBD_ReceiveAmount,
                        ARB_UnReceiveAmount = 0,
                        ARB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.YWC,
                        ARB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.YWC,
                        ARB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH,
                        ARB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH,
                        ARB_IsValid = true,
                        ARB_CreatedBy = LoginInfoDAX.UserName,
                        ARB_CreatedTime = BLLCom.GetCurStdDatetime(),
                        ARB_UpdatedBy = LoginInfoDAX.UserName,
                        ARB_UpdatedTime = BLLCom.GetCurStdDatetime(),
                    };
                    addAccountReceivableBillList.Add(accountReceivableBill);
                    //回写付款单的来源的单号
                    loopDetail.RBD_SrcBillNo = accountReceivableBill.ARB_No;
                    #endregion

                    #region 新增[应收单明细]

                    MDLFM_AccountReceivableBillDetail accountPayableBillDetail = new MDLFM_AccountReceivableBillDetail
                    {
                        ARBD_ID = Guid.NewGuid().ToString(),
                        ARBD_ARB_ID = accountReceivableBill.ARB_ID,
                        ARBD_IsMinusDetail = false,
                        ARBD_Org_ID = accountReceivableBill.ARB_Org_ID,
                        ARBD_Org_Name = accountReceivableBill.ARB_Org_Name,
                        ARBD_AccountReceivableAmount = loopDetail.RBD_ReceiveAmount,
                        ARBD_ReceivedAmount = loopDetail.RBD_ReceiveAmount,
                        ARBD_UnReceiveAmount = 0,
                        ARBD_BusinessStatusName = accountReceivableBill.ARB_BusinessStatusName,
                        ARBD_BusinessStatusCode = accountReceivableBill.ARB_BusinessStatusCode,
                        ARBD_ApprovalStatusName = accountReceivableBill.ARB_ApprovalStatusName,
                        ARBD_ApprovalStatusCode = accountReceivableBill.ARB_ApprovalStatusCode,
                        ARBD_IsValid = true,
                        ARBD_CreatedBy = LoginInfoDAX.UserName,
                        ARBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                        ARBD_UpdatedBy = LoginInfoDAX.UserName,
                        ARBD_UpdatedTime = BLLCom.GetCurStdDatetime(),
                    };
                    addAccountReceivableBillDetailList.Add(accountPayableBillDetail);
                    #endregion
                }
                #endregion
            }

            #region 收款单

            //更新收款单[业务状态]为{已完成}，[审核状态]为{已审核}
            updateReceiptBill.RB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            updateReceiptBill.RB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            updateReceiptBill.RB_BusinessStatusName = ReceiptBillStatusEnum.Name.YWC;
            updateReceiptBill.RB_BusinessStatusCode = ReceiptBillStatusEnum.Code.YWC;
            updateReceiptBill.RB_UpdatedBy = LoginInfoDAX.UserName;
            updateReceiptBill.RB_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region 更新[付款单明细]数据

            _bll.CopyModelList<ReceiptBillManagerDetailUIModel, MDLFM_ReceiptBillDetail>(paramDetailList, updateReceiptBillDetailList);
            foreach (var loopPayBillDetaill in updateReceiptBillDetailList)
            {
                loopPayBillDetaill.WHERE_RBD_ID = loopPayBillDetaill.RBD_ID;
                loopPayBillDetaill.WHERE_RBD_VersionNo = loopPayBillDetaill.RBD_VersionNo;
                loopPayBillDetaill.RBD_VersionNo++;
            }

            #endregion

            #region 更新[钱包余额]和新增[钱包异动]

            if (paramHead.RB_ReceiveTypeName == TradeTypeEnum.Name.WALLET)
            {
                #region 更新[钱包]

                updateWallet.WHERE_Wal_ID = paramHead.Wal_ID;
                updateWallet.WHERE_Wal_VersionNo = paramHead.Wal_VersionNo;
                updateWallet.Wal_AvailableBalance = paramHead.Wal_AvailableBalance - paramHead.RB_ReceiveTotalAmount;
                paramHead.Wal_AvailableBalance = updateWallet.Wal_AvailableBalance ?? 0;

                #endregion

                #region 新增[钱包异动]

                addWalletTrans.WalT_ID = System.Guid.NewGuid().ToString();
                addWalletTrans.WalT_Org_ID = LoginInfoDAX.OrgID;
                addWalletTrans.WalT_Org_Name = LoginInfoDAX.OrgShortName;
                addWalletTrans.WalT_Wal_ID = paramHead.Wal_ID;
                addWalletTrans.WalT_Wal_No = paramHead.Wal_No;
                addWalletTrans.WalT_Time = BLLCom.GetCurStdDatetime();
                addWalletTrans.WalT_TypeCode = WalTransTypeEnum.Code.XF;
                addWalletTrans.WalT_TypeName = WalTransTypeEnum.Name.XF;
                addWalletTrans.WalT_Amount = -paramHead.RB_ReceiveTotalAmount;
                addWalletTrans.WalT_BillNo = paramHead.RB_No;
                addWalletTrans.WalT_IsValid = true;
                addWalletTrans.WalT_CreatedBy = LoginInfoDAX.UserName;
                addWalletTrans.WalT_CreatedTime = BLLCom.GetCurStdDatetime();
                addWalletTrans.WalT_UpdatedBy = LoginInfoDAX.UserName;
                addWalletTrans.WalT_UpdatedTime = BLLCom.GetCurStdDatetime();

                #endregion
            }

            #endregion

            #endregion

            #region 带事务的保存
            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 更新[收款单]

                bool updateReceiptBillResult = _bll.Save(updateReceiptBill);
                if (!updateReceiptBillResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.FM_ReceiptBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                #region 更新[收款单明细]

                foreach (var loopReceiptBillDetail in updateReceiptBillDetailList)
                {
                    bool updateDetailResult = _bll.Update(loopReceiptBillDetail);
                    if (!updateDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.APPROVE + SystemTableEnums.Name.FM_ReceiptBillDetail });
                        return false;
                    }
                }

                #endregion

                #region 新增[应收单]

                bool addAccountReceivableBillResult = _bll.InsertByList<MDLFM_AccountReceivableBill, MDLFM_AccountReceivableBill>(addAccountReceivableBillList);
                if (!addAccountReceivableBillResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.ADD + SystemTableEnums.Name.FM_AccountReceivableBill });
                    return false;
                }

                #endregion

                #region 新增[应付单明细]

                bool addAccountReceivableBillDetailResult = _bll.InsertByList<MDLFM_AccountReceivableBillDetail, MDLFM_AccountReceivableBillDetail>(addAccountReceivableBillDetailList);
                if (!addAccountReceivableBillDetailResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.ADD + SystemTableEnums.Name.FM_AccountReceivableBillDetail });
                    return false;
                }

                #endregion

                #region  更新[钱包余额]和新增[钱包异动]

                if (paramHead.RB_ReceiveTypeName == TradeTypeEnum.Name.WALLET)
                {
                    #region 更新[钱包余额]

                    bool updateWalletResult = _bll.Save(updateWallet);
                    if (!updateWalletResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.EWM_Wallet });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }

                    #endregion

                    #region 新增[钱包异动]

                    bool addWalletTransResult = _bll.Insert<MDLEWM_WalletTrans>(addWalletTrans);
                    if (!addWalletTransResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.ADD + SystemTableEnums.Name.EWM_WalletTrans });
                        return false;
                    }

                    #endregion
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
            CopyModel(updateReceiptBill, paramHead);
            return true;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramHead"></param>
        /// <param name="paramDetailModel"></param>
        /// <returns></returns>
        private bool ServerCheck(ReceiptBillManagerUIModel paramHead, SkyCarBindingList<ReceiptBillManagerDetailUIModel, MDLFM_ReceiptBillDetail> paramDetailModel)
        {
            return true;
        }

        #endregion
    }
}
