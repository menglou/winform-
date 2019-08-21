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
using SkyCar.Coeus.DAL;
using System.Reflection;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.BLL.FM
{
    /// <summary>
    /// 付款单管理BLL
    /// </summary>
    public class PayBillManagerBLL : BLLBase
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
        /// 付款单管理BLL
        /// </summary>
        public PayBillManagerBLL() : base(Trans.FM)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramModel"></param>
        /// <param name="paramDetailList"></param>
        /// <param name="paramPictureNameAndPath"></param>
        /// <returns></returns>
        public bool SaveDetailDS(PayBillManagerUIModel paramHead, SkyCarBindingList<PayBillManagerDetailUIModel, MDLFM_PayBillDetail> paramDetailList,
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
            var argsHead = paramHead.ToTBModelForSaveAndDelete<MDLFM_PayBill>();
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(paramHead.PB_ID))
            {
                argsHead.PB_ID = Guid.NewGuid().ToString();
                //单号
                argsHead.PB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.PB);
                argsHead.PB_CreatedBy = LoginInfoDAX.UserName;
                argsHead.PB_CreatedTime = BLLCom.GetCurStdDatetime();
            }
            argsHead.PB_UpdatedBy = LoginInfoDAX.UserName;
            argsHead.PB_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region 明细

            //添加的明细
            if (paramDetailList != null && paramDetailList.InsertList != null &&
                paramDetailList.InsertList.Count > 0)
            {
                foreach (var loopDetailItem in paramDetailList)
                {
                    loopDetailItem.PBD_PB_ID = argsHead.PB_ID ?? argsHead.WHERE_PB_ID;
                    loopDetailItem.PBD_PB_No = argsHead.PB_No;
                    loopDetailItem.PBD_CreatedBy = LoginInfoDAX.UserName;
                    loopDetailItem.PBD_CreatedTime = BLLCom.GetCurStdDatetime();
                    loopDetailItem.PBD_UpdatedBy = LoginInfoDAX.UserName;
                    loopDetailItem.PBD_UpdatedTime = BLLCom.GetCurStdDatetime();
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
                if (loopPicture.Key == argsHead.PB_CertificatePic)
                {
                    argsHead.PB_CertificatePic = tempFileName;
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
                if (!_bll.Save(argsHead, argsHead.PB_ID))
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

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.FM_PayBill });
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

                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.FM_PayBillDetail });
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
                        loopInsertDetail.PBD_VersionNo = 1;
                    }
                }

                foreach (var loopUpdateDetail in paramDetailList.UpdateList)
                {
                    //更新时版本号加1
                    loopUpdateDetail.PBD_VersionNo = loopUpdateDetail.PBD_VersionNo + 1;
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
        /// <param name="paramHead"></param>
        /// <param name="paramDetailList"></param>
        /// <returns></returns>
        public bool ApprovePayBill(PayBillManagerUIModel paramHead, SkyCarBindingList<PayBillManagerDetailUIModel, MDLFM_PayBillDetail> paramDetailList)
        {
            var funcName = "ApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            if (paramHead == null)
            {
                //没有获取到付款单，审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.FM_PayBill, SystemActionEnum.Name.APPROVE });
                return false;
            }
            var payBillManagerDetailList =
                paramDetailList.Where(x => x.PBD_SourceTypeName == PayBillDetailSourceTypeEnum.Name.SGFK);
            if (payBillManagerDetailList.Count() != paramDetailList.Count()|| payBillManagerDetailList.Count()==0)
            {
                //收款单不为手工付款，不能审核
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0017, new object[] { SystemTableEnums.Name.FM_PayBill + MsgParam.NO + MsgParam.BE + PayBillDetailSourceTypeEnum.Name.SGFK, SystemActionEnum.Name.APPROVE });
                return false;
            }

            #region 定义变量

            //待更新[付款单]
            MDLFM_PayBill updatePayBill = paramHead.ToTBModelForSaveAndDelete<MDLFM_PayBill>();
            //待更新[付款单明细]
            List<MDLFM_PayBillDetail> updatePayBillDetaillList = new List<MDLFM_PayBillDetail>();
            //待新增对应的[应付单]
            List<MDLFM_AccountPayableBill> addAccountPayableBillList = new List<MDLFM_AccountPayableBill>();
            //待新增对应的[应付单明细]
            List<MDLFM_AccountPayableBillDetail> addAccountPayableBillDetailList = new List<MDLFM_AccountPayableBillDetail>();
            //待更新的[钱包]
            MDLEWM_Wallet updateWallet = new MDLEWM_Wallet();
            //待新增的[钱包异动]
            MDLEWM_WalletTrans addWalletTrans = new MDLEWM_WalletTrans();

            #endregion

            #region 准备数据

            if (paramDetailList[0].PBD_SourceTypeName == PayBillDetailSourceTypeEnum.Name.SGFK)
            {
                #region 新增[应付单]和[应付单明细]

                foreach (var loopDetail in paramDetailList)
                {
                    #region 新增[应付单]

                    MDLFM_AccountPayableBill accountPayableBill = new MDLFM_AccountPayableBill
                    {
                        APB_ID = System.Guid.NewGuid().ToString(),
                        APB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.APB),
                        APB_BillDirectCode = BillDirectionEnum.Code.PLUS,
                        APB_BillDirectName = BillDirectionEnum.Name.PLUS,
                        APB_SourceTypeCode = AccountPayableBillSourceTypeEnum.Code.SGCJ,
                        APB_SourceTypeName = AccountPayableBillSourceTypeEnum.Name.SGCJ,
                        APB_ReceiveObjectTypeCode = paramHead.PB_RecObjectTypeCode,
                        APB_ReceiveObjectTypeName = paramHead.PB_RecObjectTypeName,
                        APB_ReceiveObjectID = paramHead.PB_RecObjectID,
                        APB_ReceiveObjectName = paramHead.PB_RecObjectName,
                        APB_Org_ID = LoginInfoDAX.OrgID,
                        APB_Org_Name = LoginInfoDAX.OrgShortName,
                        APB_AccountPayableAmount = loopDetail.PBD_PayAmount,
                        APB_PaidAmount = loopDetail.PBD_PayAmount,
                        APB_UnpaidAmount = 0,
                        APB_BusinessStatusName = AccountPayableBillStatusEnum.Name.YWC,
                        APB_BusinessStatusCode = AccountPayableBillStatusEnum.Code.YWC,
                        APB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH,
                        APB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH,
                        APB_IsValid = true,
                        APB_CreatedBy = LoginInfoDAX.UserName,
                        APB_CreatedTime = BLLCom.GetCurStdDatetime(),
                        APB_UpdatedBy = LoginInfoDAX.UserName,
                        APB_UpdatedTime = BLLCom.GetCurStdDatetime(),
                    };
                    addAccountPayableBillList.Add(accountPayableBill);
                    //回写付款单的来源的单号
                    loopDetail.PBD_SrcBillNo = accountPayableBill.APB_No;
                    #endregion

                    #region 新增[应付单明细]

                    MDLFM_AccountPayableBillDetail accountPayableBillDetail = new MDLFM_AccountPayableBillDetail
                    {
                        APBD_ID = Guid.NewGuid().ToString(),
                        APBD_APB_ID = accountPayableBill.APB_ID,
                        APBD_IsMinusDetail = false,
                        APBD_Org_ID = accountPayableBill.APB_Org_ID,
                        APBD_Org_Name = accountPayableBill.APB_Org_Name,
                        APBD_AccountPayableAmount = loopDetail.PBD_PayAmount,
                        APBD_PaidAmount = loopDetail.PBD_PayAmount,
                        APBD_UnpaidAmount = 0,
                        APBD_BusinessStatusName = accountPayableBill.APB_BusinessStatusName,
                        APBD_BusinessStatusCode = accountPayableBill.APB_BusinessStatusCode,
                        APBD_ApprovalStatusName = accountPayableBill.APB_ApprovalStatusName,
                        APBD_ApprovalStatusCode = accountPayableBill.APB_ApprovalStatusCode,
                        APBD_IsValid = true,
                        APBD_CreatedBy = LoginInfoDAX.UserName,
                        APBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                        APBD_UpdatedBy = LoginInfoDAX.UserName,
                        APBD_UpdatedTime = BLLCom.GetCurStdDatetime(),
                    };
                    addAccountPayableBillDetailList.Add(accountPayableBillDetail);
                    #endregion
                }
                #endregion
            }

            #region 更新[付款单]数据

            updatePayBill.PB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            updatePayBill.PB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            updatePayBill.PB_BusinessStatusName = PayBillStatusEnum.Name.YWC;
            updatePayBill.PB_BusinessStatusCode = PayBillStatusEnum.Code.YWC;
            updatePayBill.PB_UpdatedBy = LoginInfoDAX.UserName;
            updatePayBill.PB_UpdatedTime = BLLCom.GetCurStdDatetime();
            
            #endregion

            #region 更新[付款单明细]数据

            _bll.CopyModelList<PayBillManagerDetailUIModel, MDLFM_PayBillDetail>(paramDetailList, updatePayBillDetaillList);
            foreach (var loopPayBillDetaill in updatePayBillDetaillList)
            {
                loopPayBillDetaill.WHERE_PBD_ID = loopPayBillDetaill.PBD_ID;
                loopPayBillDetaill.WHERE_PBD_VersionNo = loopPayBillDetaill.PBD_VersionNo;
                loopPayBillDetaill.PBD_VersionNo++;
            }

            #endregion

            #region 更新[钱包余额]和新增[钱包异动]

            if (paramHead.PB_PayTypeName == TradeTypeEnum.Name.WALLET)
            {
                #region 更新[钱包]

                updateWallet.WHERE_Wal_ID = paramHead.Wal_ID;
                updateWallet.WHERE_Wal_VersionNo = paramHead.Wal_VersionNo;
                updateWallet.Wal_AvailableBalance = paramHead.Wal_AvailableBalance + paramHead.PB_RealPayableTotalAmount;
                paramHead.Wal_AvailableBalance = updateWallet.Wal_AvailableBalance ?? 0;

                #endregion

                #region 新增[钱包异动]

                addWalletTrans.WalT_ID = System.Guid.NewGuid().ToString();
                addWalletTrans.WalT_Org_ID = LoginInfoDAX.OrgID;
                addWalletTrans.WalT_Org_Name = LoginInfoDAX.OrgShortName;
                addWalletTrans.WalT_Wal_ID = paramHead.Wal_ID;
                addWalletTrans.WalT_Wal_No = paramHead.Wal_No;
                addWalletTrans.WalT_Time = BLLCom.GetCurStdDatetime();
                addWalletTrans.WalT_TypeCode = WalTransTypeEnum.Code.TK;
                addWalletTrans.WalT_TypeName = WalTransTypeEnum.Name.TK;
                addWalletTrans.WalT_Amount = paramHead.PB_RealPayableTotalAmount;
                addWalletTrans.WalT_BillNo = paramHead.PB_No;
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

                #region 更新[付款单]

                bool updatePayBillResult = _bll.Save(updatePayBill);
                if (!updatePayBillResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.APPROVE + SystemTableEnums.Name.FM_PayBill });
                    return false;
                }

                #endregion

                #region 更新[付款单明细]

                foreach (var loopPayBillDetaill in updatePayBillDetaillList)
                {
                    bool updateDetailResult = _bll.Update(loopPayBillDetaill);
                    if (!updateDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.APPROVE + SystemTableEnums.Name.FM_PayBillDetail });
                        return false;
                    }
                }

                #endregion

                #region 新增[应付单]

                bool addAccountPayableBillResult = _bll.InsertByList<MDLFM_AccountPayableBill, MDLFM_AccountPayableBill>(addAccountPayableBillList);
                if (!addAccountPayableBillResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.ADD + SystemTableEnums.Name.FM_AccountPayableBill });
                    return false;
                }

                #endregion

                #region 新增[应付单明细]

                bool addAccountPayableBillDetailResult = _bll.InsertByList<MDLFM_AccountPayableBillDetail, MDLFM_AccountPayableBillDetail>(addAccountPayableBillDetailList);
                if (!addAccountPayableBillDetailResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.ADD + SystemTableEnums.Name.FM_AccountPayableBillDetail });
                    return false;
                }

                #endregion

                #region  更新[钱包余额]和新增[钱包异动]

                if (paramHead.PB_PayTypeName == TradeTypeEnum.Name.WALLET)
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
            CopyModel(updatePayBill, paramHead);
            return true;

        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel"></param>
        /// <param name="paramDetailList"></param>
        /// <returns></returns>
        private bool ServerCheck(PayBillManagerUIModel paramModel, SkyCarBindingList<PayBillManagerDetailUIModel, MDLFM_PayBillDetail> paramDetailList)
        {
            return true;
        }

        #endregion
    }
}
