using System;
using System.Reflection;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.RIA.UIModel;

namespace SkyCar.Coeus.BLL.RIA
{
    /// <summary>
    /// 钱包查询及操作BLL
    /// </summary>
    public class WalletQueryAndOperateBLL : BLLBase
    {
        #region 全局变量
        /// <summary>
        /// BLLBase
        /// </summary>
        BLLBase _bll = new BLLBase(Trans.RIA);
        #endregion

        #region 公共属性

        #endregion

        #region 构造方法

        /// <summary>
        /// 钱包查询及操作BLL
        /// </summary>
        public WalletQueryAndOperateBLL() : base(Trans.RIA)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        public bool SaveDetailDS(WalletQueryAndOperateUIModel paramModel)
        {
            var funcName = "ApproveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            //服务端检查
            if (!ServerCheck(paramModel))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            #region 保存数据

            //将UIModel转为TBModel
            var argsWallet = CopyModel<MDLEWM_Wallet>(paramModel);
            argsWallet.Wal_UpdatedBy = LoginInfoDAX.UserName;
            argsWallet.Wal_UpdatedTime = BLLCom.GetCurStdDatetime();
            argsWallet.WHERE_Wal_ID = argsWallet.Wal_ID;
            argsWallet.WHERE_Wal_VersionNo = argsWallet.Wal_VersionNo;

            try
            {
                bool updateWalletResult = _bll.Update(SQLID.RIA_WalletQueryAndOperate_SQL02, argsWallet);
                if (!updateWalletResult)
                {
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + MsgParam.WALLET });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, MethodBase.GetCurrentMethod().ToString(),
                    ex.Message, "", null);
                return false;
            }
            //将最新数据回写给DetailDS
            if (argsWallet.Wal_VersionNo != null)
            {
                argsWallet.Wal_VersionNo += 1;
            }
            CopyModel(argsWallet, paramModel);
            #endregion

            return true;
        }

        /// <summary>
        /// 钱包销户
        /// </summary>
        /// <param name="paramModel">待销户的钱包</param>
        /// <returns></returns>
        public bool CloseAccountDetailDS(WalletQueryAndOperateUIModel paramModel)
        {
            var funcName = "CloseAccountDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            #region 准备数据

            //将UIModel转为TBModel
            var argsWallet = CopyModel<MDLEWM_Wallet>(paramModel);
            //待保存的钱包异动日志
            MDLEWM_WalletTrans newWalletTrans = new MDLEWM_WalletTrans();

            #region 更新[电子钱包]

            argsWallet.Wal_AvailableBalance = 0;
            argsWallet.Wal_FreezingBalance = 0;
            argsWallet.Wal_IneffectiveTime = BLLCom.GetCurStdDatetime();
            argsWallet.Wal_StatusName = WalletStatusEnum.Name.YXH;
            argsWallet.Wal_StatusCode = WalletStatusEnum.Code.YXH;
            argsWallet.Wal_IsValid = false; ;
            argsWallet.Wal_UpdatedBy = LoginInfoDAX.UserName;
            argsWallet.Wal_UpdatedTime = BLLCom.GetCurStdDatetime();
            argsWallet.WHERE_Wal_ID = argsWallet.Wal_ID;
            argsWallet.WHERE_Wal_VersionNo = argsWallet.Wal_VersionNo;
            #endregion

            #region 生成钱包异动日志
            newWalletTrans = BLLCom.CreateWalletTrans(new MDLEWM_WalletTrans
            {
                WalT_Org_ID = LoginInfoDAX.OrgID,
                WalT_Org_Name = LoginInfoDAX.OrgShortName,
                WalT_Wal_ID = argsWallet.Wal_ID,
                WalT_Wal_No = argsWallet.Wal_No,
                //异动类型为{销户}
                WalT_TypeName = WalTransTypeEnum.Name.XH,
                WalT_TypeCode = WalTransTypeEnum.Code.XH,
                WalT_ChannelName = LoginTerminalEnum.Name.PC,
                WalT_ChannelCode = LoginTerminalEnum.Code.PC,
            });
            #endregion

            #endregion

            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存[电子钱包]

                bool saveWalletResult = _bll.Save(argsWallet);
                if (!saveWalletResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.RECHARGE + SystemTableEnums.Name.EWM_Wallet });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }

                #endregion

                #region 保存[电子钱包异动]

                bool insertWalletTransResult = _bll.Insert(newWalletTrans);
                if (!insertWalletTransResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.NEW + SystemTableEnums.Name.EWM_WalletTrans });
                    LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                    return false;
                }
                #endregion

                DBManager.CommitTransaction(DBCONFIG.Coeus);
            }
            catch (Exception ex)
            {
                DBManager.RollBackTransaction(DBCONFIG.Coeus);
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.RECHARGE, ex.Message });
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ex.Message, "", null);
                return false;
            }

            #endregion

            //将最新数据回写给DetailDS
            CopyModel(argsWallet, paramModel);

            return true;
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 服务端检查
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        private bool ServerCheck(WalletQueryAndOperateUIModel paramModel)
        {
            if (paramModel == null
                || string.IsNullOrEmpty(paramModel.Wal_ID)
                || string.IsNullOrEmpty(paramModel.Wal_No))
            {
                //没有获取到钱包，保存失败
                ResultMsg = MsgHelp.GetMsg(MsgCode.W_0024, new object[] { MsgParam.WALLET, SystemActionEnum.Name.SAVE });
                return false;
            }
            return true;
        }

        #endregion
    }
}
