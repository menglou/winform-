using System;
using System.Collections.Generic;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Const;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.RIA.UIModel;

namespace SkyCar.Coeus.BLL.RIA
{
    /// <summary>
    /// 钱包充值BLL
    /// </summary>
    public class WalletDepositMoneyBLL : BLLBase
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
        /// 钱包充值BLL
        /// </summary>
        public WalletDepositMoneyBLL() : base(Trans.RIA)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <param name="paramDepositDetailList">充值明细列表</param>
        /// <returns></returns>
        public bool SaveDetailDS(WalletDepositMoneyUIModel paramModel, List<WalletDepositMoneyUIModel> paramDepositDetailList)
        {
            var funcName = "SaveDetailDS";
            LogHelper.WriteBussLogStart(BussID, LoginInfoDAX.UserName, funcName, "", "", null);

            //服务端检查
            if (!ServerCheck(paramModel))
            {
                LogHelper.WriteBussLogEndNG(BussID, LoginInfoDAX.UserName, funcName, ResultMsg, "", null);
                return false;
            }

            #region 准备数据

            //将UIModel转为TBModel
            var argsWallet = CopyModel<MDLEWM_Wallet>(paramModel);
            //待保存的钱包异动日志
            MDLEWM_WalletTrans newWalletTrans = new MDLEWM_WalletTrans();

            #region 更新[电子钱包]
            //钱包可用余额
            argsWallet.Wal_AvailableBalance = argsWallet.Wal_AvailableBalance;
            argsWallet.Wal_UpdatedBy = LoginInfoDAX.UserName;
            argsWallet.Wal_UpdatedTime = BLLCom.GetCurStdDatetime();
            argsWallet.WHERE_Wal_ID = argsWallet.Wal_ID;
            argsWallet.WHERE_Wal_VersionNo = argsWallet.Wal_VersionNo;
            #endregion

            if (paramModel.ThisDepositAmount > 0)
            {
                #region 获取充值方式

                //充值方式名称
                string tempPaymentModeName = string.Empty;
                //充值方式编码
                string tempPaymentModeCode = string.Empty;
                foreach (var loopAmountTransDetail in paramDepositDetailList)
                {
                    if (loopAmountTransDetail.PayAmount == null
                        || loopAmountTransDetail.PayAmount == 0)
                    {
                        continue;
                    }
                    tempPaymentModeName += (!string.IsNullOrEmpty(tempPaymentModeName) ? SysConst.Semicolon_DBC : string.Empty) + loopAmountTransDetail.PaymentModeName;
                    tempPaymentModeCode += (!string.IsNullOrEmpty(tempPaymentModeCode) ? SysConst.Semicolon_DBC : string.Empty) + loopAmountTransDetail.PaymentModeCode;
                }
                #endregion

                //生成钱包异动日志
                newWalletTrans = BLLCom.CreateWalletTrans(new MDLEWM_WalletTrans()
                {
                    WalT_Org_ID = LoginInfoDAX.OrgID,
                    WalT_Org_Name = LoginInfoDAX.OrgShortName,
                    WalT_Wal_ID = argsWallet.Wal_ID,
                    WalT_Wal_No = argsWallet.Wal_No,
                    //异动类型为{充值}
                    WalT_TypeName = WalTransTypeEnum.Name.CZ,
                    WalT_TypeCode = WalTransTypeEnum.Code.CZ,
                    //充值方式
                    WalT_RechargeTypeName = tempPaymentModeName,
                    WalT_RechargeTypeCode = tempPaymentModeCode,
                    //异动金额
                    WalT_Amount = paramModel.ThisDepositAmount,
                    WalT_ChannelName = LoginTerminalEnum.Name.PC,
                    WalT_ChannelCode = LoginTerminalEnum.Code.PC,
                    WalT_Remark = argsWallet.Wal_Remark,
                });

            }
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
        private bool ServerCheck(WalletDepositMoneyUIModel paramModel)
        {
            return true;
        }
        #endregion
    }
}
