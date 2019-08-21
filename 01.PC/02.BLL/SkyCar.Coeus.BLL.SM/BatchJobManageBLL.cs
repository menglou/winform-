using System;
using System.Collections.Generic;
using System.Reflection;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.SM;

namespace SkyCar.Coeus.BLL.SM
{
    /// <summary>
    /// 作业管理BLL
    /// </summary>
    public class BatchJobManageBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.SM);
        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 作业管理BLL
        /// </summary>
        public BatchJobManageBLL() : base(Trans.SM)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        public bool SaveDetailDS(BatchJobManageUIModel paramModel)
        {
            var funcName = "SaveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            //服务端检查
            if (!ServerCheck(paramModel))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            #region 带事务的保存

            //将UIModel转为TBModel
            var argsBatchJob = paramModel.ToTBModelForSaveAndDelete<MDLCSM_BatchJob>();
            //判断主键是否被赋值
            if (string.IsNullOrEmpty(paramModel.BJ_ID))
            {
                argsBatchJob.BJ_ID = Guid.NewGuid().ToString();
                //作业编码
                argsBatchJob.BJ_Code = BLLCom.GetCoeusDocumentNo(DocumentTypeEnums.Code.BJ);
                argsBatchJob.BJ_CreatedBy = LoginInfoDAX.UserName;
                argsBatchJob.BJ_CreatedTime = BLLCom.GetCurStdDatetime();
            }
            argsBatchJob.BJ_UpdatedBy = LoginInfoDAX.UserName;
            argsBatchJob.BJ_UpdatedTime = BLLCom.GetCurStdDatetime();

            //执行保存
            if (!_bll.Save(argsBatchJob))
            {
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.CSM_BatchJob });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            //将最新数据回写给DetailDS
            CopyModel(argsBatchJob, paramModel);

            #endregion

            return true;
        }

        /// <summary>
        /// 删除系统作业
        /// </summary>
        /// <param name="paramBatchJobList">待删除的系统作业List</param>
        /// <returns></returns>
        public bool DeleteBatchJob(List<MDLCSM_BatchJob> paramBatchJobList)
        {
            var funcName = "SaveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                var deleteBatchJobResult = _bll.DeleteByList<MDLCSM_BatchJob, MDLCSM_BatchJob>(paramBatchJobList);
                if (!deleteBatchJobResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.DELETE, SystemTableEnums.Name.CSM_BatchJob });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.DELETE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ex.Message, "", null);
                return false;
            }
            return true;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(BatchJobManageUIModel paramModel)
        {
            //检查作业内容是否已存在
            int batchJobCount = _bll.QueryForObject<int>(SQLID.SM_BatchJobManage_SQL01, new MDLCSM_BatchJob
            {
                WHERE_BJ_ID = paramModel.BJ_ID,
                WHERE_BJ_BusinessType = paramModel.BJ_BusinessType,
                WHERE_BJ_Pattern = paramModel.BJ_Pattern
            });
            if (batchJobCount > 0)
            {
                //业务类型：paramModel.BJ_BusinessType \n 作业方式：paramModel.BJ_Pattern 的系统作业已存在，不能重复添加
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0006, new object[] { MsgParam.BUSINESS_TYPE + SysConst.COLON_DBC + paramModel.BJ_BusinessType + "\n" + MsgParam.EXECUTE_PATTERN + SysConst.COLON_DBC + paramModel.BJ_Pattern + "\n" + MsgParam.OF + SystemTableEnums.Name.CSM_BatchJob });
                return false;
            }
            return true;
        }

        #endregion
    }
}
