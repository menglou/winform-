using System;
using System.Collections.Generic;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.FM.APModel;

namespace SkyCar.Coeus.BLL.FM
{
    /// <summary>
    /// 应付单确认付款弹出窗BLL
    /// </summary>
    public class PayablePayConfirmWindowBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.FM);
        #endregion

        #region 构造方法

        /// <summary>
        /// 应付单确认付款弹出窗BLL
        /// </summary>
        public PayablePayConfirmWindowBLL() : base(Trans.FM)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="paramPayable"></param>
        /// <param name="paramPayablePayConfirmList"></param>
        /// <returns></returns>
        public bool SavePayablePayConfirmData(MDLFM_PayBill paramPayable, List<PayablePayConfirmUIModel> paramPayablePayConfirmList)
        {
            var funcName = "SavePayableCashierConfirmData";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 准备数据

            #region 变量定义

            //待保存的[付款单]
            MDLFM_PayBill insertPayBill = new MDLFM_PayBill();
            //待保存的[付款单明细]列表
            List<MDLFM_PayBillDetail> insertPayBillDetailList = new List<MDLFM_PayBillDetail>();
            //待更新的[应付单]列表
            List<MDLFM_AccountPayableBill> updateAccountPayableBillList = new List<MDLFM_AccountPayableBill>();
            //待更新的[应收单]列表
            List<MDLFM_AccountReceivableBill> updateAccountReceivableBillList = new List<MDLFM_AccountReceivableBill>();
            //待更新的[电子钱包]
            MDLEWM_Wallet updatewallet = new MDLEWM_Wallet();
            //待新增的[电子钱包异动]
            MDLEWM_WalletTrans insertwalletTrans = new MDLEWM_WalletTrans();

            #endregion

            #region 待保存的[付款单]

            insertPayBill.PB_ID = Guid.NewGuid().ToString();
            insertPayBill.PB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.PB);
            insertPayBill.PB_Pay_Org_ID = paramPayablePayConfirmList[0].BusinessOrgID;
            insertPayBill.PB_Pay_Org_Name = paramPayablePayConfirmList[0].BusinessOrgName;
            insertPayBill.PB_Date = BLLCom.GetCurStdDatetime();

            insertPayBill.PB_RecObjectTypeCode = paramPayablePayConfirmList[0].ReceiveObjectTypeCode;
            insertPayBill.PB_RecObjectTypeName = paramPayablePayConfirmList[0].ReceiveObjectTypeName;
            insertPayBill.PB_RecObjectID = paramPayablePayConfirmList[0].ReceiveObjectID;
            insertPayBill.PB_RecObjectName = paramPayablePayConfirmList[0].ReceiveObjectName;

            insertPayBill.PB_PayableTotalAmount = paramPayable.PB_PayableTotalAmount;
            insertPayBill.PB_RealPayableTotalAmount = paramPayable.PB_RealPayableTotalAmount;
            insertPayBill.PB_PayAccount = paramPayable.PB_PayAccount;
            insertPayBill.PB_RecAccount = paramPayable.PB_RecAccount;
            insertPayBill.PB_PayTypeName = paramPayable.PB_PayTypeName;
            insertPayBill.PB_PayTypeCode = paramPayable.PB_PayTypeCode;
            insertPayBill.PB_CertificateNo = paramPayable.PB_CertificateNo;

            insertPayBill.PB_BusinessStatusName = ReceiptBillStatusEnum.Name.YWC;
            insertPayBill.PB_BusinessStatusCode = ReceiptBillStatusEnum.Code.YWC;
            insertPayBill.PB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            insertPayBill.PB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            insertPayBill.PB_Remark = paramPayable.PB_Remark;
            insertPayBill.PB_IsValid = true;
            insertPayBill.PB_CreatedBy = LoginInfoDAX.UserName;
            insertPayBill.PB_CreatedTime = BLLCom.GetCurStdDatetime();
            insertPayBill.PB_UpdatedBy = LoginInfoDAX.UserName;
            insertPayBill.PB_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #region 待保存的[付款单明细]列表，待更新[应收单]或[应付单]列表

            foreach (var loopItem in paramPayablePayConfirmList)
            {
                if (loopItem.ThisPayAmount == 0 && loopItem.UnPayTotalAmount != 0)
                {
                    continue;
                }

                #region 待保存的[付款单明细]

                MDLFM_PayBillDetail insertPayBillDetail = new MDLFM_PayBillDetail()
                {
                    PBD_ID = Guid.NewGuid().ToString(),
                    PBD_PB_ID = insertPayBill.PB_ID,
                    PBD_PB_No = insertPayBill.PB_No,
                    PBD_PayAmount = loopItem.ThisPayAmount,
                    PBD_CreatedBy = LoginInfoDAX.UserName,
                    PBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                    PBD_UpdatedBy = LoginInfoDAX.UserName,
                    PBD_UpdatedTime = BLLCom.GetCurStdDatetime(),
                };
                insertPayBillDetail.PBD_SrcBillNo = loopItem.APB_SourceBillNo == "" ? loopItem.APB_No : loopItem.APB_SourceBillNo;
                if (loopItem.APB_SourceTypeName == AccountPayableBillSourceTypeEnum.Name.SHYF)
                {
                    insertPayBillDetail.PBD_SourceTypeName = PayBillDetailSourceTypeEnum.Name.BZCG;
                    insertPayBillDetail.PBD_SourceTypeCode = PayBillDetailSourceTypeEnum.Code.BZCG;
                }
                else if (loopItem.APB_SourceTypeName == AccountPayableBillSourceTypeEnum.Name.CKYF)
                {
                    insertPayBillDetail.PBD_SourceTypeName = PayBillDetailSourceTypeEnum.Name.THSK;
                    insertPayBillDetail.PBD_SourceTypeCode = PayBillDetailSourceTypeEnum.Code.THSK;
                }
                else if (loopItem.APB_SourceTypeName == AccountPayableBillSourceTypeEnum.Name.SGCJ)
                {
                    insertPayBillDetail.PBD_SourceTypeName = PayBillDetailSourceTypeEnum.Name.SGFK;
                    insertPayBillDetail.PBD_SourceTypeCode = PayBillDetailSourceTypeEnum.Code.SGFK;
                }
                else if (loopItem.APB_SourceTypeName == AccountReceivableBillSourceTypeEnum.Name.XSYS)
                {
                    insertPayBillDetail.PBD_SourceTypeName = ReceiptBillDetailSourceTypeEnum.Name.XSSK;
                    insertPayBillDetail.PBD_SourceTypeCode = ReceiptBillDetailSourceTypeEnum.Code.XSSK;
                }
                else if (loopItem.APB_SourceTypeName == AccountReceivableBillSourceTypeEnum.Name.QTYS)
                {
                    insertPayBillDetail.PBD_SourceTypeName = ReceiptBillDetailSourceTypeEnum.Name.QTSK;
                    insertPayBillDetail.PBD_SourceTypeCode = ReceiptBillDetailSourceTypeEnum.Code.QTSK;
                }
                insertPayBillDetailList.Add(insertPayBillDetail);

                #endregion

                #region 待更新的[应付单]或[应收单]列表

                if (loopItem.IsBusinessSourceAccountPayableBill)
                {
                    #region 待更新的[应付单]列表

                    MDLFM_AccountPayableBill updateAccountPayableBill = new MDLFM_AccountPayableBill
                    {
                        APB_ID = loopItem.APB_ID,
                        APB_No = loopItem.APB_No,
                        APB_BillDirectCode = loopItem.APB_BillDirectCode,
                        APB_BillDirectName = loopItem.APB_BillDirectName,
                        APB_SourceTypeCode = loopItem.APB_SourceTypeCode,
                        APB_SourceTypeName = loopItem.APB_SourceTypeName,
                        APB_SourceBillNo = loopItem.APB_SourceBillNo,
                        APB_Org_ID = loopItem.APB_Org_ID,
                        APB_Org_Name = loopItem.APB_Org_Name,
                        APB_AccountPayableAmount = loopItem.APB_AccountPayableAmount,
                        APB_ApprovalStatusCode = loopItem.APB_ApprovalStatusCode,
                        APB_ApprovalStatusName = loopItem.APB_ApprovalStatusName,
                        APB_CreatedBy = loopItem.APB_CreatedBy,
                        APB_CreatedTime = loopItem.APB_CreatedTime,
                        APB_UpdatedBy = LoginInfoDAX.UserName,
                        APB_UpdatedTime = BLLCom.GetCurStdDatetime(),
                        APB_VersionNo = loopItem.APB_VersionNo,
                        WHERE_APB_ID = loopItem.APB_ID,
                        WHERE_APB_VersionNo = loopItem.APB_VersionNo
                    };
                    if (updateAccountPayableBill.APB_BillDirectName == BillDirectionEnum.Name.PLUS)
                    {
                        //已付金额
                        updateAccountPayableBill.APB_PaidAmount = (loopItem.APB_PaidAmount ?? 0) + Math.Abs(loopItem.ThisPayAmount ?? 0);
                    }
                    else
                    {
                        //已付金额
                        updateAccountPayableBill.APB_PaidAmount = (loopItem.APB_PaidAmount ?? 0) - Math.Abs(loopItem.ThisPayAmount ?? 0);
                    }
                    //未付金额
                    updateAccountPayableBill.APB_UnpaidAmount = (loopItem.APB_AccountPayableAmount ?? 0) - (updateAccountPayableBill.APB_PaidAmount ?? 0);

                    if (Math.Abs(updateAccountPayableBill.APB_PaidAmount ?? 0) >= Math.Abs(loopItem.APB_AccountPayableAmount ?? 0))
                    {
                        updateAccountPayableBill.APB_UnpaidAmount = 0;
                        //单据状态
                        updateAccountPayableBill.APB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.YWC;
                        updateAccountPayableBill.APB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.YWC;
                    }
                    updateAccountPayableBillList.Add(updateAccountPayableBill);

                    #endregion
                }
                else
                {
                    #region 待更新的[应收单]列表

                    MDLFM_AccountReceivableBill updateAccountReceivableBill = new MDLFM_AccountReceivableBill
                    {
                        ARB_ID = loopItem.APB_ID,
                        ARB_No = loopItem.APB_No,
                        ARB_BillDirectCode = loopItem.APB_BillDirectCode,
                        ARB_BillDirectName = loopItem.APB_BillDirectName,
                        ARB_SourceTypeCode = loopItem.APB_SourceTypeCode,
                        ARB_SourceTypeName = loopItem.APB_SourceTypeName,
                        ARB_SrcBillNo = loopItem.APB_SourceBillNo,
                        ARB_Org_ID = loopItem.APB_Org_ID,
                        ARB_Org_Name = loopItem.APB_Org_Name,
                        ARB_AccountReceivableAmount = loopItem.APB_AccountPayableAmount,
                        ARB_ApprovalStatusCode = loopItem.APB_ApprovalStatusCode,
                        ARB_ApprovalStatusName = loopItem.APB_ApprovalStatusName,
                        ARB_CreatedBy = loopItem.APB_CreatedBy,
                        ARB_CreatedTime = loopItem.APB_CreatedTime,
                        ARB_UpdatedBy = LoginInfoDAX.UserName,
                        ARB_UpdatedTime = BLLCom.GetCurStdDatetime(),
                        ARB_VersionNo = loopItem.APB_VersionNo,
                        WHERE_ARB_ID = loopItem.APB_ID,
                        WHERE_ARB_VersionNo = loopItem.APB_VersionNo
                    };
                    if (updateAccountReceivableBill.ARB_BillDirectName == BillDirectionEnum.Name.PLUS)
                    {
                        //已收金额
                        updateAccountReceivableBill.ARB_ReceivedAmount = (loopItem.APB_PaidAmount ?? 0) + Math.Abs(loopItem.ThisPayAmount ?? 0);
                    }
                    else
                    {
                        //已收金额
                        updateAccountReceivableBill.ARB_ReceivedAmount = (loopItem.APB_PaidAmount ?? 0) - Math.Abs(loopItem.ThisPayAmount ?? 0);
                    }
                    //未收金额
                    updateAccountReceivableBill.ARB_UnReceiveAmount = (loopItem.APB_AccountPayableAmount ?? 0) - (updateAccountReceivableBill.ARB_ReceivedAmount ?? 0);

                    if (Math.Abs(updateAccountReceivableBill.ARB_ReceivedAmount ?? 0) >= Math.Abs(loopItem.APB_AccountPayableAmount ?? 0))
                    {
                        updateAccountReceivableBill.ARB_UnReceiveAmount = 0;
                        //单据状态
                        updateAccountReceivableBill.ARB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.YWC;
                        updateAccountReceivableBill.ARB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.YWC;
                    }
                    updateAccountReceivableBillList.Add(updateAccountReceivableBill);

                    #endregion
                }

                #endregion
            }

            #endregion

            #region 更新的[电子钱包]、新增的[电子钱包异动]

            if (paramPayable.PB_PayTypeName == TradeTypeEnum.Name.WALLET && (!string.IsNullOrEmpty(paramPayablePayConfirmList[0].Wal_ID)))
            {
                #region 待更新的[电子钱包]

                updatewallet.WHERE_Wal_ID = paramPayablePayConfirmList[0].Wal_ID;
                updatewallet.WHERE_Wal_VersionNo = paramPayablePayConfirmList[0].Wal_VersionNo;
                updatewallet.Wal_AvailableBalance = paramPayablePayConfirmList[0].Wal_AvailableBalance +
                                                    paramPayable.PB_RealPayableTotalAmount;
                updatewallet.Wal_VersionNo = paramPayablePayConfirmList[0].Wal_VersionNo + 1;

                #endregion

                #region 待新增的[电子钱包异动]

                insertwalletTrans.WalT_ID = System.Guid.NewGuid().ToString();
                insertwalletTrans.WalT_Org_ID = LoginInfoDAX.OrgID;
                insertwalletTrans.WalT_Org_Name = LoginInfoDAX.OrgShortName;
                insertwalletTrans.WalT_Wal_ID = paramPayablePayConfirmList[0].Wal_ID;
                insertwalletTrans.WalT_Wal_No = paramPayablePayConfirmList[0].Wal_No;
                insertwalletTrans.WalT_Time = BLLCom.GetCurStdDatetime();
                insertwalletTrans.WalT_TypeCode = WalTransTypeEnum.Code.XF;
                insertwalletTrans.WalT_TypeName = WalTransTypeEnum.Name.XF;

                insertwalletTrans.WalT_Amount = -paramPayable.PB_RealPayableTotalAmount; ;
                insertwalletTrans.WalT_BillNo = insertPayBill.PB_No;
                insertwalletTrans.WalT_IsValid = true;
                insertwalletTrans.WalT_CreatedBy = LoginInfoDAX.UserName;
                insertwalletTrans.WalT_CreatedTime = BLLCom.GetCurStdDatetime();
                insertwalletTrans.WalT_UpdatedBy = LoginInfoDAX.UserName;
                insertwalletTrans.WalT_UpdatedTime = BLLCom.GetCurStdDatetime();

                #endregion
            }

            #endregion

            #endregion

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存[付款单明细]

                bool insertPayBillResult = _bll.Insert(insertPayBill);
                if (!insertPayBillResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.FM_PayBillDetail });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存[付款单明细]列表

                if (insertPayBillDetailList.Count > 0)
                {
                    bool insertPayBillDetailResult = _bll.InsertByList<MDLFM_PayBillDetail, MDLFM_PayBillDetail>(insertPayBillDetailList);
                    if (!insertPayBillDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.FM_PayBillDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }
                }
                #endregion

                #region 更新[应付单]列表

                if (updateAccountPayableBillList.Count > 0)
                {
                    foreach (var loopAccountPayableBill in updateAccountPayableBillList)
                    {
                        bool updateAccountReceivableBill = _bll.Update(loopAccountPayableBill);
                        if (!updateAccountReceivableBill)
                        {
                            DBManager.RollBackTransaction(DBCONFIG.Coeus);
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.FM_AccountPayableBill });
                            LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                            return false;
                        }
                    }
                }

                #endregion

                #region 更新[应收单]列表

                if (updateAccountReceivableBillList.Count > 0)
                {
                    foreach (var loopAccountReceivableBill in updateAccountReceivableBillList)
                    {
                        bool updateAccountReceivableBill = _bll.Update(loopAccountReceivableBill);
                        if (!updateAccountReceivableBill)
                        {
                            DBManager.RollBackTransaction(DBCONFIG.Coeus);
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.FM_AccountReceivableBill });
                            LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                            return false;
                        }
                    }
                }

                #endregion

                #region 更新的[电子钱包]、新增的[电子钱包异动]

                if (paramPayable.PB_PayTypeName == TradeTypeEnum.Name.WALLET && (!string.IsNullOrEmpty(paramPayablePayConfirmList[0].Wal_ID)))
                {
                    #region 更新的[电子钱包]

                    bool updatewalletResult = _bll.Update(updatewallet);
                    if (!updatewalletResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.EWM_Wallet });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }

                    #endregion

                    #region 待新增的[电子钱包异动]

                    bool insertwalletTranslResult = _bll.Insert(insertwalletTrans);
                    if (!insertwalletTranslResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.ADD + SystemTableEnums.Name.EWM_WalletTrans });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
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
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ex.Message, "", null);
                return false;
            }

            #endregion

            return true;
        }
        #endregion
    }
}
