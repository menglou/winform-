using System;
using System.Collections.Generic;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.DAL;

namespace SkyCar.Coeus.BLL.Common
{
    /// <summary>
    /// 业务单确认收款弹出窗BLL
    /// </summary>
    public class BusinessCollectionConfirmWindowBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.COM);
        #endregion

        #region 构造方法

        /// <summary>
        /// 业务单确认收款弹出窗BLL
        /// </summary>
        public BusinessCollectionConfirmWindowBLL() : base(Trans.FM)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="paramReceiptBill"></param>
        /// <param name="paramReceiveableCollectionConfirmList"></param>
        /// <returns></returns>
        public bool SalesCashierConfirmData(MDLFM_ReceiptBill paramReceiptBill, List<BusinessCollectionConfirmUIModel> paramReceiveableCollectionConfirmList)
        {
            var funcName = "SalesCashierConfirmData";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 定义变量

            //待保存的[收款单]
            MDLFM_ReceiptBill insertReceiptBill = new MDLFM_ReceiptBill();
            //待保存的[收款单明细]列表
            List<MDLFM_ReceiptBillDetail> insertReceiptBillDetailList = new List<MDLFM_ReceiptBillDetail>();
            //待更新的应收单列表
            List<MDLFM_AccountReceivableBill> updateAccountReceivableBillList = new List<MDLFM_AccountReceivableBill>();
            //待更新的负向应付单列表
            List<MDLFM_AccountPayableBill> updateAccountPayableBillList = new List<MDLFM_AccountPayableBill>();
            //待更新的[电子钱包]
            MDLEWM_Wallet updatewallet = new MDLEWM_Wallet();
            //待新增的[电子钱包异动]
            MDLEWM_WalletTrans insertwalletTrans = new MDLEWM_WalletTrans();

            #endregion

            #region 准备数据

            #region 待保存的[收款单]

            insertReceiptBill = new MDLFM_ReceiptBill()
            {
                RB_ID = Guid.NewGuid().ToString(),
                RB_Rec_Org_ID = paramReceiveableCollectionConfirmList[0].BusinessOrgID,
                RB_Rec_Org_Name = paramReceiveableCollectionConfirmList[0].BusinessOrgName,
                RB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.RB),
                RB_PayObjectTypeCode = paramReceiveableCollectionConfirmList[0].PayObjectTypeCode,
                RB_PayObjectTypeName = paramReceiveableCollectionConfirmList[0].PayObjectTypeName,

                RB_PayObjectID = paramReceiveableCollectionConfirmList[0].PayObjectID,
                RB_PayObjectName = paramReceiveableCollectionConfirmList[0].PayObjectName,
                RB_ReceiveTypeName = paramReceiptBill.RB_ReceiveTypeName,
                RB_ReceiveTypeCode = paramReceiptBill.RB_ReceiveTypeCode,
                RB_ReceiveAccount = paramReceiptBill.RB_ReceiveAccount,
                RB_CertificateNo = paramReceiptBill.RB_CertificateNo,
                RB_ReceiveTotalAmount = paramReceiptBill.RB_ReceiveTotalAmount,
                RB_BusinessStatusName = ReceiptBillStatusEnum.Name.YWC,
                RB_BusinessStatusCode = ReceiptBillStatusEnum.Code.YWC,
                RB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH,
                RB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH,
                RB_Date = BLLCom.GetCurStdDatetime(),
                RB_IsValid = true,
                RB_Remark = paramReceiptBill.RB_Remark,
                RB_CreatedBy = LoginInfoDAX.UserName,
                RB_CreatedTime = BLLCom.GetCurStdDatetime(),
                RB_UpdatedBy = LoginInfoDAX.UserName,
                RB_UpdatedTime = BLLCom.GetCurStdDatetime(),
            };

            #endregion

            #region 待保存的[收款单明细]列表，待更新[应收单]或[应付单]列表

            foreach (var loopItem in paramReceiveableCollectionConfirmList)
            {
                if (loopItem.ThisReceiveAmount == 0 && loopItem.UnReceiveTotalAmount != 0)
                {
                    continue;
                }

                #region 待保存的[收款单明细]

                MDLFM_ReceiptBillDetail insertReceiptBillDetail = new MDLFM_ReceiptBillDetail
                {
                    RBD_ID = System.Guid.NewGuid().ToString(),
                    RBD_RB_ID = insertReceiptBill.RB_ID,
                    RBD_RB_No = insertReceiptBill.RB_No,
                    RBD_SrcBillNo = loopItem.ARB_SrcBillNo,
                    //来源类型
                    RBD_ReceiveAmount = loopItem.ThisReceiveAmount,
                    RBD_IsValid = true,
                    RBD_CreatedBy = LoginInfoDAX.UserName,
                    RBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                    RBD_UpdatedBy = LoginInfoDAX.UserName,
                    RBD_UpdatedTime = BLLCom.GetCurStdDatetime()
                };
                if (loopItem.ARB_SourceTypeName == AccountReceivableBillSourceTypeEnum.Name.SGCJ)
                {
                    insertReceiptBillDetail.RBD_SourceTypeName = ReceiptBillDetailSourceTypeEnum.Name.SGSK;
                    insertReceiptBillDetail.RBD_SourceTypeCode = ReceiptBillDetailSourceTypeEnum.Code.SGSK;
                }
                else if (loopItem.ARB_SourceTypeName == AccountReceivableBillSourceTypeEnum.Name.XSYS)
                {
                    insertReceiptBillDetail.RBD_SourceTypeName = ReceiptBillDetailSourceTypeEnum.Name.XSSK;
                    insertReceiptBillDetail.RBD_SourceTypeCode = ReceiptBillDetailSourceTypeEnum.Code.XSSK;
                }
                else if (loopItem.ARB_SourceTypeName == AccountReceivableBillSourceTypeEnum.Name.QTYS)
                {
                    insertReceiptBillDetail.RBD_SourceTypeName = ReceiptBillDetailSourceTypeEnum.Name.QTSK;
                    insertReceiptBillDetail.RBD_SourceTypeCode = ReceiptBillDetailSourceTypeEnum.Code.QTSK;
                }
                else if (loopItem.ARB_SourceTypeName == AccountPayableBillSourceTypeEnum.Name.SHYF)
                {
                    insertReceiptBillDetail.RBD_SourceTypeName = ReceiptBillDetailSourceTypeEnum.Name.RKFK;
                    insertReceiptBillDetail.RBD_SourceTypeCode = ReceiptBillDetailSourceTypeEnum.Code.RKFK;
                }
                else if (loopItem.ARB_SourceTypeName == AccountPayableBillSourceTypeEnum.Name.CKYF)
                {
                    insertReceiptBillDetail.RBD_SourceTypeName = ReceiptBillDetailSourceTypeEnum.Name.THSK;
                    insertReceiptBillDetail.RBD_SourceTypeCode = ReceiptBillDetailSourceTypeEnum.Code.THSK;
                }
                insertReceiptBillDetailList.Add(insertReceiptBillDetail);
                #endregion

                #region 待更新[应付单]或[应收单]列表

                if (loopItem.IsBusinessSourceAccountPayableBill)
                {
                    #region 待更新的[应付单]列表

                    MDLFM_AccountPayableBill updateAccountPayableBill = new MDLFM_AccountPayableBill
                    {
                        APB_ID = loopItem.ARB_ID,
                        APB_No = loopItem.ARB_No,
                        APB_BillDirectCode = loopItem.ARB_BillDirectCode,
                        APB_BillDirectName = loopItem.ARB_BillDirectName,
                        APB_SourceTypeCode = loopItem.ARB_SourceTypeCode,
                        APB_SourceTypeName = loopItem.ARB_SourceTypeName,
                        APB_SourceBillNo = loopItem.ARB_SrcBillNo,
                        APB_Org_ID = loopItem.ARB_Org_ID,
                        APB_Org_Name = loopItem.ARB_Org_Name,
                        APB_AccountPayableAmount = loopItem.ARB_AccountReceivableAmount,
                        APB_ApprovalStatusCode = loopItem.ARB_ApprovalStatusCode,
                        APB_ApprovalStatusName = loopItem.ARB_ApprovalStatusName,
                        APB_CreatedBy = loopItem.ARB_CreatedBy,
                        APB_CreatedTime = loopItem.ARB_CreatedTime,
                        APB_UpdatedBy = LoginInfoDAX.UserName,
                        APB_UpdatedTime = BLLCom.GetCurStdDatetime(),
                        APB_VersionNo = loopItem.ARB_VersionNo,
                        WHERE_APB_ID = loopItem.ARB_ID,
                        WHERE_APB_VersionNo = loopItem.ARB_VersionNo
                    };
                    if (updateAccountPayableBill.APB_BillDirectName == BillDirectionEnum.Name.PLUS)
                    {
                        //已付金额
                        updateAccountPayableBill.APB_PaidAmount = (loopItem.ARB_ReceivedAmount ?? 0) + Math.Abs(loopItem.ThisReceiveAmount ?? 0);
                    }
                    else
                    {
                        //已付金额
                        updateAccountPayableBill.APB_PaidAmount = (loopItem.ARB_ReceivedAmount ?? 0) - Math.Abs(loopItem.ThisReceiveAmount ?? 0);
                    }   
                    //未付金额
                    updateAccountPayableBill.APB_UnpaidAmount = (loopItem.ARB_AccountReceivableAmount ?? 0) - (updateAccountPayableBill.APB_PaidAmount ?? 0);
                    if (Math.Abs(updateAccountPayableBill.APB_PaidAmount ?? 0) >= Math.Abs(loopItem.ARB_AccountReceivableAmount ?? 0))
                    {
                        //单据状态
                        updateAccountPayableBill.APB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.YWC;
                        updateAccountPayableBill.APB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.YWC;
                        updateAccountPayableBill.APB_UnpaidAmount = 0;
                    }
                    updateAccountPayableBillList.Add(updateAccountPayableBill);

                    #endregion
                }
                else
                {
                    #region 待更新[应收单]列表

                    //应收单
                    MDLFM_AccountReceivableBill updateReceivableBill = new MDLFM_AccountReceivableBill
                    {
                        ARB_ID = loopItem.ARB_ID,
                        ARB_No = loopItem.ARB_No,
                        ARB_BillDirectCode = loopItem.ARB_BillDirectCode,
                        ARB_BillDirectName = loopItem.ARB_BillDirectName,
                        ARB_SourceTypeCode = loopItem.ARB_SourceTypeCode,
                        ARB_SourceTypeName = loopItem.ARB_SourceTypeName,
                        ARB_SrcBillNo = loopItem.ARB_SrcBillNo,
                        ARB_Org_ID = loopItem.ARB_Org_ID,
                        ARB_Org_Name = loopItem.ARB_Org_Name,
                        ARB_AccountReceivableAmount = loopItem.ARB_AccountReceivableAmount,
                        ARB_ApprovalStatusCode = loopItem.ARB_ApprovalStatusCode,
                        ARB_ApprovalStatusName = loopItem.ARB_ApprovalStatusName,
                        ARB_CreatedBy = loopItem.ARB_CreatedBy,
                        ARB_CreatedTime = loopItem.ARB_CreatedTime,
                        ARB_UpdatedBy = LoginInfoDAX.UserName,
                        ARB_UpdatedTime = BLLCom.GetCurStdDatetime(),
                        ARB_VersionNo = loopItem.ARB_VersionNo,
                        WHERE_ARB_ID = loopItem.ARB_ID,
                        WHERE_ARB_VersionNo = loopItem.ARB_VersionNo
                    };
                    if (updateReceivableBill.ARB_BillDirectName == BillDirectionEnum.Name.PLUS)
                    {
                        //已收金额
                        updateReceivableBill.ARB_ReceivedAmount = (loopItem.ARB_ReceivedAmount ?? 0) + Math.Abs(loopItem.ThisReceiveAmount ?? 0);
                    }
                    else
                    {
                        //已收金额
                        updateReceivableBill.ARB_ReceivedAmount = (loopItem.ARB_ReceivedAmount ?? 0) - Math.Abs(loopItem.ThisReceiveAmount ?? 0);
                    }
                    //未收金额
                    updateReceivableBill.ARB_UnReceiveAmount = (loopItem.ARB_AccountReceivableAmount ?? 0) - (updateReceivableBill.ARB_ReceivedAmount ?? 0);
                    if ((updateReceivableBill.ARB_ReceivedAmount ?? 0) >= (loopItem.ARB_AccountReceivableAmount ?? 0))
                    {
                        //单据状态
                        updateReceivableBill.ARB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.YWC;
                        updateReceivableBill.ARB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.YWC;
                        updateReceivableBill.ARB_UnReceiveAmount = 0;
                    }
                    updateAccountReceivableBillList.Add(updateReceivableBill);

                    #endregion
                }
                #endregion
            }

            #endregion

            #region 更新的[电子钱包]、新增的[电子钱包异动]

            if (paramReceiptBill.RB_ReceiveTypeName == TradeTypeEnum.Name.WALLET && (!string.IsNullOrEmpty(paramReceiveableCollectionConfirmList[0].Wal_ID)))
            {
                #region 待更新的[电子钱包]

                updatewallet.WHERE_Wal_ID = paramReceiveableCollectionConfirmList[0].Wal_ID;
                updatewallet.WHERE_Wal_VersionNo = paramReceiveableCollectionConfirmList[0].Wal_VersionNo;
                updatewallet.Wal_AvailableBalance = paramReceiveableCollectionConfirmList[0].Wal_AvailableBalance -
                                                    paramReceiptBill.RB_ReceiveTotalAmount;
                updatewallet.Wal_VersionNo = paramReceiveableCollectionConfirmList[0].Wal_VersionNo + 1;

                #endregion

                #region 待新增的[电子钱包异动]

                insertwalletTrans.WalT_ID = System.Guid.NewGuid().ToString();
                insertwalletTrans.WalT_Org_ID = LoginInfoDAX.OrgID;
                insertwalletTrans.WalT_Org_Name = LoginInfoDAX.OrgShortName;
                insertwalletTrans.WalT_Wal_ID = paramReceiveableCollectionConfirmList[0].Wal_ID;
                insertwalletTrans.WalT_Wal_No = paramReceiveableCollectionConfirmList[0].Wal_No;
                insertwalletTrans.WalT_Time = BLLCom.GetCurStdDatetime();
                insertwalletTrans.WalT_TypeCode = WalTransTypeEnum.Code.XF;
                insertwalletTrans.WalT_TypeName = WalTransTypeEnum.Name.XF;

                insertwalletTrans.WalT_Amount = -paramReceiptBill.RB_ReceiveTotalAmount; ;
                insertwalletTrans.WalT_BillNo = insertReceiptBill.RB_No;
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

                #region 保存[收款单]

                bool insertReceiptBillResult = _bll.Insert(insertReceiptBill);
                if (!insertReceiptBillResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.ADD + SystemTableEnums.Name.FM_ReceiptBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存[收款单明细]

                if (insertReceiptBillDetailList.Count > 0)
                {
                    bool insertReceiptBillDetailResult = _bll.InsertByList<MDLFM_ReceiptBillDetail, MDLFM_ReceiptBillDetail>(insertReceiptBillDetailList);
                    if (!insertReceiptBillDetailResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.ADD + SystemTableEnums.Name.FM_PayBillDetail });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
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
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.FM_AccountReceivableBill });
                            LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                            return false;
                        }
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
                            ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.FM_AccountPayableBill });
                            LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                            return false;
                        }
                    }
                }

                #endregion

                #region 更新的[电子钱包]、新增的[电子钱包异动]

                if (paramReceiptBill.RB_ReceiveTypeName == TradeTypeEnum.Name.WALLET && (!string.IsNullOrEmpty(paramReceiveableCollectionConfirmList[0].Wal_ID)))
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
