using System;
using System.Collections.Generic;
using System.Linq;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.PIS;

namespace SkyCar.Coeus.BLL.PIS
{
    /// <summary>
    /// 业务单确认付款弹出窗BLL
    /// </summary>
    public class PurchaseOrderToPayConfirmWindowBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.PIS);
        #endregion

        #region 构造方法

        /// <summary>
        /// 付款单管理BLL
        /// </summary>
        public PurchaseOrderToPayConfirmWindowBLL() : base(Trans.PIS)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="paramPayBill"></param>
        /// <param name="paramBusinessPayConfirmList"></param>
        /// <param name="paramAccountPayableBillList"></param>
        /// <returns></returns>
        public bool SavePurchaseOrderToPayConfirmData(MDLFM_PayBill paramPayBill, List<PurchaseOrderToPayConfirmWindowModel> paramBusinessPayConfirmList, List<MDLFM_AccountPayableBill> paramAccountPayableBillList)
        {
            var funcName = "SaveBusinessPayConfirmData";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 准备数据

            //待新增的[付款单]
            MDLFM_PayBill insertPayBill = new MDLFM_PayBill();
            //待新增的[付款单明细]列表
            List<MDLFM_PayBillDetail> insertPayBillDetailList = new List<MDLFM_PayBillDetail>();
            //待更新的[应付单]列表
            List<MDLFM_AccountPayableBill> updateAccountPayableBillList = new List<MDLFM_AccountPayableBill>();

            #region 新增付款单

            insertPayBill.PB_ID = Guid.NewGuid().ToString();
            insertPayBill.PB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.PB);
            insertPayBill.PB_Pay_Org_ID = paramBusinessPayConfirmList[0].BusinessOrgID;
            insertPayBill.PB_Pay_Org_Name = paramBusinessPayConfirmList[0].BusinessOrgName;
            insertPayBill.PB_Date = BLLCom.GetCurStdDatetime();

            insertPayBill.PB_RecObjectTypeCode = paramBusinessPayConfirmList[0].ReceiveObjectTypeCode;
            insertPayBill.PB_RecObjectTypeName = paramBusinessPayConfirmList[0].ReceiveObjectTypeName;
            insertPayBill.PB_RecObjectID = paramBusinessPayConfirmList[0].ReceiveObjectID;
            insertPayBill.PB_RecObjectName = paramBusinessPayConfirmList[0].ReceiveObjectName;

            insertPayBill.PB_PayableTotalAmount = paramPayBill.PB_PayableTotalAmount;
            insertPayBill.PB_RealPayableTotalAmount = paramPayBill.PB_RealPayableTotalAmount;
            insertPayBill.PB_PayAccount = paramPayBill.PB_PayAccount;
            insertPayBill.PB_RecAccount = paramPayBill.PB_RecAccount;
            insertPayBill.PB_PayTypeName = paramPayBill.PB_PayTypeName;
            insertPayBill.PB_PayTypeCode = paramPayBill.PB_PayTypeCode;
            insertPayBill.PB_CertificateNo = paramPayBill.PB_CertificateNo;

            insertPayBill.PB_BusinessStatusName = ReceiptBillStatusEnum.Name.YWC;
            insertPayBill.PB_BusinessStatusCode = ReceiptBillStatusEnum.Code.YWC;
            insertPayBill.PB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            insertPayBill.PB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            insertPayBill.PB_Remark = paramPayBill.PB_Remark;
            insertPayBill.PB_IsValid = true;
            insertPayBill.PB_CreatedBy = LoginInfoDAX.UserName;
            insertPayBill.PB_CreatedTime = BLLCom.GetCurStdDatetime();
            insertPayBill.PB_UpdatedBy = LoginInfoDAX.UserName;
            insertPayBill.PB_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            foreach (var loopItem in paramBusinessPayConfirmList)
            {
                if (loopItem.ThisPayAmount == 0 && loopItem.UnPayTotalAmount != 0)
                {
                    continue;
                }

                #region 待新增的[付款单明细]

                MDLFM_PayBillDetail insertPayBillDetail = new MDLFM_PayBillDetail()
                {
                    PBD_ID = Guid.NewGuid().ToString(),
                    PBD_PB_ID = insertPayBill.PB_ID,
                    PBD_PB_No = insertPayBill.PB_No,
                    PBD_SrcBillNo = loopItem.BusinessNo,
                    PBD_PayAmount = loopItem.ThisPayAmount,
                    PBD_CreatedBy = LoginInfoDAX.UserName,
                    PBD_CreatedTime = BLLCom.GetCurStdDatetime(),
                    PBD_UpdatedBy = LoginInfoDAX.UserName,
                    PBD_UpdatedTime = BLLCom.GetCurStdDatetime(),
                };
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

                #region 待更新的[应付单]列表

                //筛选出当前业务单下所有的应付单
                var tempAccountPayableBillList =
                    paramAccountPayableBillList.Where(x => x.APB_SourceBillNo == loopItem.APB_SourceBillNo).ToList();
                //当前业务单对应的未付总金额
                decimal unPaidTotalAmount = 0;
                //当前业务单对应的本次付款金额
                decimal thisPayAmount = loopItem.ThisPayAmount ?? 0;
                for (int i = 0; i < tempAccountPayableBillList.Count; i++)
                {
                    //当本次付款金额小于或等于0是调出循环
                    if (loopItem.ThisPayAmount <= 0)
                    {
                        break;
                    }

                    unPaidTotalAmount += (tempAccountPayableBillList[i].APB_UnpaidAmount ?? 0);

                    //本次付款多付出的钱
                    decimal overPaidAmount = 0;
                    if (thisPayAmount > unPaidTotalAmount)
                    {
                        overPaidAmount = thisPayAmount - unPaidTotalAmount;
                    }

                    if (loopItem.ThisPayAmount <= tempAccountPayableBillList[i].APB_UnpaidAmount && tempAccountPayableBillList[i].APB_UnpaidAmount > 0)
                    {
                        #region 更新的应付单

                        MDLFM_AccountPayableBill updatePayableBill = new MDLFM_AccountPayableBill()
                        {
                            APB_ID = tempAccountPayableBillList[i].APB_ID,
                            APB_No = tempAccountPayableBillList[i].APB_No,
                            APB_BillDirectCode = tempAccountPayableBillList[i].APB_BillDirectCode,
                            APB_BillDirectName = tempAccountPayableBillList[i].APB_BillDirectName,
                            APB_SourceTypeCode = tempAccountPayableBillList[i].APB_SourceTypeCode,
                            APB_SourceTypeName = tempAccountPayableBillList[i].APB_SourceTypeName,
                            APB_SourceBillNo = tempAccountPayableBillList[i].APB_SourceBillNo,
                            APB_Org_ID = tempAccountPayableBillList[i].APB_Org_ID,
                            APB_Org_Name = tempAccountPayableBillList[i].APB_Org_Name,
                            APB_AccountPayableAmount = tempAccountPayableBillList[i].APB_AccountPayableAmount,
                            APB_ApprovalStatusCode = tempAccountPayableBillList[i].APB_ApprovalStatusCode,
                            APB_ApprovalStatusName = tempAccountPayableBillList[i].APB_ApprovalStatusName,
                            APB_CreatedBy = tempAccountPayableBillList[i].APB_CreatedBy,
                            APB_CreatedTime = tempAccountPayableBillList[i].APB_CreatedTime,
                            APB_UpdatedBy = LoginInfoDAX.UserName,
                            APB_UpdatedTime = BLLCom.GetCurStdDatetime(),
                            APB_VersionNo = tempAccountPayableBillList[i].APB_VersionNo,
                            WHERE_APB_ID = tempAccountPayableBillList[i].APB_ID,
                            WHERE_APB_VersionNo = tempAccountPayableBillList[i].APB_VersionNo
                        };
                        //已付金额
                        updatePayableBill.APB_PaidAmount = (tempAccountPayableBillList[i].APB_PaidAmount ?? 0) +
                                                           (loopItem.ThisPayAmount ?? 0);
                        //本次付款金额
                        loopItem.ThisPayAmount = loopItem.ThisPayAmount - ((updatePayableBill.APB_AccountPayableAmount ?? 0) - (updatePayableBill.APB_UnpaidAmount ?? 0));
                        //未付金额
                        updatePayableBill.APB_UnpaidAmount = (tempAccountPayableBillList[i].APB_AccountPayableAmount ?? 0) -
                                                             (updatePayableBill.APB_PaidAmount ?? 0);

                        if ((updatePayableBill.APB_PaidAmount ?? 0) >= (tempAccountPayableBillList[i].APB_AccountPayableAmount ?? 0))
                        {
                            //单据状态
                            updatePayableBill.APB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.YWC;
                            updatePayableBill.APB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.YWC;
                        }
                        updateAccountPayableBillList.Add(updatePayableBill);

                        #endregion
                    }
                    else if (loopItem.ThisPayAmount > tempAccountPayableBillList[i].APB_UnpaidAmount && tempAccountPayableBillList[i].APB_UnpaidAmount > 0)
                    {
                        //本次付款金额>当前应付单未付金额的场合，当前应付单的已付金额=应付金额
                        #region 更新的应付单

                        MDLFM_AccountPayableBill updatePayableBill = new MDLFM_AccountPayableBill()
                        {
                            APB_ID = tempAccountPayableBillList[i].APB_ID,
                            APB_No = tempAccountPayableBillList[i].APB_No,
                            APB_BillDirectCode = tempAccountPayableBillList[i].APB_BillDirectCode,
                            APB_BillDirectName = tempAccountPayableBillList[i].APB_BillDirectName,
                            APB_SourceTypeCode = tempAccountPayableBillList[i].APB_BillDirectName,
                            APB_SourceTypeName = tempAccountPayableBillList[i].APB_SourceTypeName,
                            APB_SourceBillNo = tempAccountPayableBillList[i].APB_SourceBillNo,
                            APB_Org_ID = tempAccountPayableBillList[i].APB_Org_ID,
                            APB_Org_Name = tempAccountPayableBillList[i].APB_Org_Name,
                            APB_AccountPayableAmount = tempAccountPayableBillList[i].APB_AccountPayableAmount,
                            APB_ApprovalStatusCode = tempAccountPayableBillList[i].APB_ApprovalStatusCode,
                            APB_ApprovalStatusName = tempAccountPayableBillList[i].APB_ApprovalStatusName,
                            APB_CreatedBy = tempAccountPayableBillList[i].APB_CreatedBy,
                            APB_CreatedTime = tempAccountPayableBillList[i].APB_CreatedTime,
                            APB_UpdatedBy = LoginInfoDAX.UserName,
                            APB_UpdatedTime = BLLCom.GetCurStdDatetime(),
                            APB_VersionNo = tempAccountPayableBillList[i].APB_VersionNo,
                            WHERE_APB_ID = tempAccountPayableBillList[i].APB_ID,
                            WHERE_APB_VersionNo = tempAccountPayableBillList[i].APB_VersionNo
                        };
                        loopItem.ThisPayAmount = loopItem.ThisPayAmount - paramAccountPayableBillList[i].APB_UnpaidAmount;
                        //已付金额
                        updatePayableBill.APB_PaidAmount = updatePayableBill.APB_AccountPayableAmount;
                        //未付金额
                        updatePayableBill.APB_UnpaidAmount = 0;

                        //单据状态
                        updatePayableBill.APB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.YWC;
                        updatePayableBill.APB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.YWC;
                        updateAccountPayableBillList.Add(updatePayableBill);

                        #endregion
                    }
                    if (overPaidAmount > 0 && i == tempAccountPayableBillList.Count - 1)
                    {
                        bool isRepeat = false;
                        foreach (var loopUpdateAccountPayableBill in updateAccountPayableBillList)
                        {
                            if (loopUpdateAccountPayableBill.APB_ID == tempAccountPayableBillList[i].APB_ID)
                            {
                                loopUpdateAccountPayableBill.APB_PaidAmount = loopUpdateAccountPayableBill.APB_PaidAmount + overPaidAmount;
                                loopUpdateAccountPayableBill.APB_UnpaidAmount = 0;
                                isRepeat = true;
                                break;
                            }
                        }
                        if (!isRepeat)
                        {
                            MDLFM_AccountPayableBill updatePayableBill = new MDLFM_AccountPayableBill()
                            {
                                APB_ID = tempAccountPayableBillList[i].APB_ID,
                                APB_No = tempAccountPayableBillList[i].APB_No,
                                APB_BillDirectCode = tempAccountPayableBillList[i].APB_BillDirectCode,
                                APB_BillDirectName = tempAccountPayableBillList[i].APB_BillDirectName,
                                APB_SourceTypeCode = tempAccountPayableBillList[i].APB_BillDirectName,
                                APB_SourceTypeName = tempAccountPayableBillList[i].APB_SourceTypeName,
                                APB_SourceBillNo = tempAccountPayableBillList[i].APB_SourceBillNo,
                                APB_Org_ID = tempAccountPayableBillList[i].APB_Org_ID,
                                APB_Org_Name = tempAccountPayableBillList[i].APB_Org_Name,
                                APB_AccountPayableAmount = tempAccountPayableBillList[i].APB_AccountPayableAmount,
                                APB_ApprovalStatusCode = tempAccountPayableBillList[i].APB_ApprovalStatusCode,
                                APB_ApprovalStatusName = tempAccountPayableBillList[i].APB_ApprovalStatusName,
                                APB_CreatedBy = tempAccountPayableBillList[i].APB_CreatedBy,
                                APB_CreatedTime = tempAccountPayableBillList[i].APB_CreatedTime,
                                APB_UpdatedBy = LoginInfoDAX.UserName,
                                APB_UpdatedTime = BLLCom.GetCurStdDatetime(),
                                APB_VersionNo = tempAccountPayableBillList[i].APB_VersionNo,
                                WHERE_APB_ID = tempAccountPayableBillList[i].APB_ID,
                                WHERE_APB_VersionNo = tempAccountPayableBillList[i].APB_VersionNo
                            };
                            updatePayableBill.APB_PaidAmount = tempAccountPayableBillList[i].APB_PaidAmount + overPaidAmount;
                            updatePayableBill.APB_UnpaidAmount = tempAccountPayableBillList[i].APB_UnpaidAmount - overPaidAmount;
                            updateAccountPayableBillList.Add(updatePayableBill);
                        }
                    }
                }

                #endregion

            }

            #endregion

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 新增的[付款单]

                bool insertPayBillResult = _bll.Insert(insertPayBill);
                if (!insertPayBillResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.FM_PayBill });
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

                #region 更新应付单

                if (updateAccountPayableBillList.Count > 0)
                {
                    foreach (var loopAccountReceivableBill in updateAccountPayableBillList)
                    {
                        bool updateAccountReceivableBill = _bll.Update(loopAccountReceivableBill);
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
