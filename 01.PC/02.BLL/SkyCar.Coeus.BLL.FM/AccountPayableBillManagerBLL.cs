using System;
using System.Collections.Generic;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.FM;

namespace SkyCar.Coeus.BLL.FM
{
    /// <summary>
    /// 应付管理BLL
    /// </summary>
    public class AccountPayableBillManagerBLL : BLLBase
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
        /// 应付管理BLL
        /// </summary>
        public AccountPayableBillManagerBLL() : base(Trans.FM)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramHead">单头UIModel</param>
        /// <returns></returns>
        public bool SaveDetailDS(AccountPayableBillManagerUIModel paramHead)
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
            var argsHead = paramHead.ToTBModelForSaveAndDelete<MDLFM_AccountPayableBill>();
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(paramHead.APB_ID))
            {
                argsHead.APB_ID = Guid.NewGuid().ToString();
                //单号
                argsHead.APB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.APB);
                argsHead.APB_CreatedBy = LoginInfoDAX.UserName;
                argsHead.APB_CreatedTime = BLLCom.GetCurStdDatetime();
            }
            argsHead.APB_UpdatedBy = LoginInfoDAX.UserName;
            argsHead.APB_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion

            #endregion

            #region 事务操作

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存单头

                //执行保存
                if (!_bll.Save(argsHead, argsHead.APB_ID))
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.FM_AccountPayableBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "保存失败，失败原因：" + ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ex.Message, "", null);
                return false;
            }

            #endregion

            //将最新数据回写给DetailDS
            CopyModel(argsHead, paramHead);

            return true;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="paramHead">UIModel</param>
        /// <returns></returns>
        public bool ApproveDetailDS(AccountPayableBillManagerUIModel paramHead)
        {
            var funcName = "ApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.APB_ID)
                || string.IsNullOrEmpty(paramHead.APB_No))
            {
                //没有获取到应付单，审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.PIS_StockOutBill, SystemActionEnum.Name.APPROVE });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            #endregion

            #region 准备数据

            #region 更新[应付单]

            //更新应付单[业务状态]为[已完成]，[审核状态]为[已审核]
            paramHead.APB_BusinessStatusName = AccountPayableBillStatusEnum.Name.ZXZ;
            paramHead.APB_BusinessStatusCode = AccountPayableBillStatusEnum.Code.ZXZ;
            paramHead.APB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            paramHead.APB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            paramHead.APB_UpdatedBy = LoginInfoDAX.UserName;
            paramHead.APB_UpdatedTime = BLLCom.GetCurStdDatetime();

            var updateHead = paramHead.ToTBModelForSaveAndDelete<MDLFM_AccountPayableBill>();
            #endregion

            #endregion

            #region 带事务的新增和保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存[应付单]
                bool updateAccountPayableBill = _bll.Save(updateHead);
                if (!updateAccountPayableBill)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.FM_AccountPayableBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
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
            CopyModel(updateHead, paramHead);

            return true;
        }

        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="paramHead">UIModel</param>
        /// <returns></returns>
        public bool UnApproveDetailDS(AccountPayableBillManagerUIModel paramHead)
        {
            var funcName = "UnApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.APB_ID)
                || string.IsNullOrEmpty(paramHead.APB_No))
            {
                //没有获取到应付单，反审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.FM_AccountPayableBill, SystemActionEnum.Name.UNAPPROVE });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }
            #endregion

            #region 准备数据

            #region 更新[应付单]

            //更新应付单[业务状态]为[已生成]，[审核状态]为[待审核]
            paramHead.APB_BusinessStatusName = AccountPayableBillStatusEnum.Name.YSC;
            paramHead.APB_BusinessStatusCode = AccountPayableBillStatusEnum.Code.YSC;
            paramHead.APB_ApprovalStatusName = ApprovalStatusEnum.Name.DSH;
            paramHead.APB_ApprovalStatusCode = ApprovalStatusEnum.Code.DSH;
            paramHead.APB_UpdatedBy = LoginInfoDAX.UserName;
            paramHead.APB_UpdatedTime = BLLCom.GetCurStdDatetime();
            //将UIModel转为TBModel
            var updateHead = paramHead.ToTBModelForSaveAndDelete<MDLFM_AccountPayableBill>();

            #endregion

            #endregion

            #region 带事务的新增和保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存[应付单]

                bool updateAccountPayableBill = _bll.Save(updateHead);
                if (!updateAccountPayableBill)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.FM_AccountPayableBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
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
            CopyModel(updateHead, paramHead);

            return true;
        }

        /// <summary>
        /// 对账
        /// </summary>
        /// <param name="paramHead">UIModel</param>
        /// <returns></returns>
        public bool ReconciliationDetailDS(List<AccountPayableBillManagerUIModel> paramHead)
        {
            var funcName = "ReconciliationDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 保存应付单

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                foreach (var loopAccountPayableBill in paramHead)
                {
                    //将UIModel转为TBModel
                    var updateAccountPayableBill = loopAccountPayableBill.ToTBModelForSaveAndDelete<MDLFM_AccountPayableBill>();

                    //更新应付单[业务状态]为[已对账]
                    updateAccountPayableBill.APB_BusinessStatusName = AccountPayableBillStatusEnum.Name.YDZ;
                    updateAccountPayableBill.APB_BusinessStatusCode = AccountPayableBillStatusEnum.Code.YDZ;
                    updateAccountPayableBill.APB_UpdatedBy = LoginInfoDAX.UserName;
                    updateAccountPayableBill.APB_UpdatedTime = BLLCom.GetCurStdDatetime();

                    bool updateAccountPayableBillResult = _bll.Save(updateAccountPayableBill);
                    if (!updateAccountPayableBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.FM_AccountPayableBill });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }

                    _bll.CopyModel(updateAccountPayableBill, loopAccountPayableBill);
                    //loopAccountPayableBill.APB_ID = updateAccountPayableBill.WHERE_APB_ID;
                }
                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.RECONCILIATION, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
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
        private bool ServerCheck(AccountPayableBillManagerUIModel paramModel)
        {
            //TODO 服务端检查（无服务端检查逻辑，请删除本行注释）
            return true;
        }

        #endregion
    }
}
