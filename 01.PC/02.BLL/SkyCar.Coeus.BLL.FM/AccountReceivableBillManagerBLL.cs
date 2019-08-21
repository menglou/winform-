using System;
using System.Collections.Generic;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common;
using SkyCar.Coeus.UIModel.FM;

namespace SkyCar.Coeus.BLL.FM
{
    /// <summary>
    /// 应收管理BLL
    /// </summary>
    public class AccountReceivableBillManagerBLL : BLLBase
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
        /// 应收管理BLL
        /// </summary>
        public AccountReceivableBillManagerBLL() : base(Trans.FM)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramHead">单头UIModel</param>
        /// <returns></returns>
        public bool SaveDetailDS(AccountReceivableBillManagerUIModel paramHead)
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
            var argsHead = paramHead.ToTBModelForSaveAndDelete<MDLFM_AccountReceivableBill>();
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(paramHead.ARB_ID))
            {
                argsHead.ARB_ID = Guid.NewGuid().ToString();
                //单号
                argsHead.ARB_No = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.ARB);
                argsHead.ARB_CreatedBy = LoginInfoDAX.UserName;
                argsHead.ARB_CreatedTime = BLLCom.GetCurStdDatetime();
            }
            argsHead.ARB_UpdatedBy = LoginInfoDAX.UserName;
            argsHead.ARB_UpdatedTime = BLLCom.GetCurStdDatetime();

            #endregion
            
            #endregion

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存单头

                //执行保存
                if (!_bll.Save(argsHead, argsHead.ARB_ID))
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.FM_AccountReceivableBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
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
            
            return true;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="paramHead">UIModel</param>
        /// <returns></returns>
        public bool ApproveDetailDS(AccountReceivableBillManagerUIModel paramHead)
        {
            var funcName = "ApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.ARB_ID)
                || string.IsNullOrEmpty(paramHead.ARB_No))
            {
                //没有获取到应收单，审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.FM_AccountReceivableBill, SystemActionEnum.Name.APPROVE });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            #endregion

            #region 准备数据

            #region 更新[应收单]

            //更新应收单[业务状态]为[执行中]，[审核状态]为[已审核]
            paramHead.ARB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.ZXZ;
            paramHead.ARB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.ZXZ;
            paramHead.ARB_ApprovalStatusName = ApprovalStatusEnum.Name.YSH;
            paramHead.ARB_ApprovalStatusCode = ApprovalStatusEnum.Code.YSH;
            paramHead.ARB_UpdatedBy = LoginInfoDAX.UserName;
            paramHead.ARB_UpdatedTime = BLLCom.GetCurStdDatetime();
            //将UIModel转为TBModel
            var updateHead = paramHead.ToTBModelForSaveAndDelete<MDLFM_AccountReceivableBill>();

            #endregion
            
            #endregion

            #region 带事务的新增和保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存[应收单]

                bool updateAccountReceivableBill = _bll.Save(updateHead);
                if (!updateAccountReceivableBill)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.FM_AccountReceivableBill });
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
        public bool UnApproveDetailDS(AccountReceivableBillManagerUIModel paramHead)
        {
            var funcName = "UnApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 验证

            if (paramHead == null
                || string.IsNullOrEmpty(paramHead.ARB_ID)
                || string.IsNullOrEmpty(paramHead.ARB_No))
            {
                //没有获取到应收单，反审核失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { SystemTableEnums.Name.FM_AccountReceivableBill, SystemActionEnum.Name.UNAPPROVE });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }
            #endregion

            #region 准备数据

            #region 更新[应收单]

            //更新应收单[业务状态]为[已生成]，[审核状态]为[待审核]
            paramHead.ARB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.YSC;
            paramHead.ARB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.YSC;
            paramHead.ARB_ApprovalStatusName = ApprovalStatusEnum.Name.DSH;
            paramHead.ARB_ApprovalStatusCode = ApprovalStatusEnum.Code.DSH;
            paramHead.ARB_UpdatedBy = LoginInfoDAX.UserName;
            paramHead.ARB_UpdatedTime = BLLCom.GetCurStdDatetime();
            //将UIModel转为TBModel
            var updateHead = paramHead.ToTBModelForSaveAndDelete<MDLFM_AccountReceivableBill>();

            #endregion

            #endregion

            #region 带事务的新增和保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存[应收单]

                bool updateAccountReceivableBill = _bll.Save(updateHead);
                if (!updateAccountReceivableBill)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.FM_AccountReceivableBill });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion
                
                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.UNAPPROVE, ex.Message });
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
        public bool ReconciliationDetailDS(List<AccountReceivableBillManagerUIModel> paramHead)
        {
            var funcName = "ReconciliationDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);
            
            #region 保存应收单

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                foreach (var loopAccountReceivableBill in paramHead)
                {
                    //将UIModel转为TBModel
                    var updateAccountReceivableBill = loopAccountReceivableBill.ToTBModelForSaveAndDelete<MDLFM_AccountReceivableBill>();

                    //更新应收单[业务状态]为[已对账]
                    updateAccountReceivableBill.ARB_BusinessStatusName = AccountReceivableBillStatusEnum.Name.YDZ;
                    updateAccountReceivableBill.ARB_BusinessStatusCode = AccountReceivableBillStatusEnum.Code.YDZ;
                    updateAccountReceivableBill.ARB_UpdatedBy = LoginInfoDAX.UserName;
                    updateAccountReceivableBill.ARB_UpdatedTime = BLLCom.GetCurStdDatetime();

                    bool updateAccountReceivableBillResult = _bll.Save(updateAccountReceivableBill);
                    if (!updateAccountReceivableBillResult)
                    {
                        DBManager.RollBackTransaction(DBCONFIG.Coeus);
                        ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { MsgParam.UPDATE + SystemTableEnums.Name.FM_AccountReceivableBill });
                        LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                        return false;
                    }

                    _bll.CopyModel(updateAccountReceivableBill, loopAccountReceivableBill);
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
        private bool ServerCheck(AccountReceivableBillManagerUIModel paramModel)
        {
            return true;
        }

        #endregion
    }
}
