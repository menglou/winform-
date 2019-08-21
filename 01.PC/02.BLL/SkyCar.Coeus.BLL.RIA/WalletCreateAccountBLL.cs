using System;
using System.Collections.Generic;
using SkyCar.Coeus.BLL.Common;
using SkyCar.Coeus.Common.Enums;
using SkyCar.Coeus.Common.Log;
using SkyCar.Coeus.Common.Message;
using SkyCar.Coeus.DAL;
using SkyCar.Coeus.TBModel;
using SkyCar.Coeus.UIModel.Common.APModel;
using SkyCar.Coeus.UIModel.RIA;
using SkyCar.Coeus.UIModel.RIA.UIModel;

namespace SkyCar.Coeus.BLL.RIA
{
    /// <summary>
    /// 钱包开户BLL
    /// </summary>
    public class WalletCreateAccountBLL : BLLBase
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
        /// 钱包开户BLL
        /// </summary>
        public WalletCreateAccountBLL() : base(Trans.RIA)
        {

        }

        #endregion

        #region  公共方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="paramModel">UIModel</param>
        /// <returns></returns>
        public bool SaveDetailDS(WalletCreateAccountUIModel paramModel)
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

            //判断主键是否被赋值
            if (string.IsNullOrEmpty(argsWallet.Wal_ID))
            {
                #region 新增
                //生成新ID
                argsWallet.Wal_ID = Guid.NewGuid().ToString();
                //生成钱包账号（汽配商户编码+7位顺序号）
                argsWallet.Wal_No = GenerateWalletNo();
                argsWallet.Wal_CreatedBy = LoginInfoDAX.UserName;
                argsWallet.Wal_CreatedTime = BLLCom.GetCurStdDatetime();
                #endregion
            }
            else
            {
                #region 更新
                //主键被赋值，则需要更新，更新需要设定更新条件
                argsWallet.WHERE_Wal_ID = argsWallet.Wal_ID;
                argsWallet.WHERE_Wal_VersionNo = argsWallet.Wal_VersionNo;
                argsWallet.Wal_VersionNo++;
                #endregion
            }
            argsWallet.Wal_UpdatedBy = LoginInfoDAX.UserName;
            argsWallet.Wal_UpdatedTime = BLLCom.GetCurStdDatetime();

            //生成钱包异动日志
            newWalletTrans = BLLCom.CreateWalletTrans(new MDLEWM_WalletTrans()
            {
                WalT_Org_ID = LoginInfoDAX.OrgID,
                WalT_Org_Name = LoginInfoDAX.OrgShortName,
                WalT_Wal_ID = argsWallet.Wal_ID,
                WalT_Wal_No = argsWallet.Wal_No,
                WalT_TypeName = WalTransTypeEnum.Name.KH,
                WalT_TypeCode = WalTransTypeEnum.Code.KH,
                WalT_ChannelName = LoginTerminalEnum.Name.PC,
                WalT_ChannelCode = LoginTerminalEnum.Code.PC,
                WalT_Remark = argsWallet.Wal_Remark,
            });
            #endregion
            #region 带事务的保存

            try
            {
                DBManager.BeginTransaction(DBCONFIG.Coeus);

                #region 保存[电子钱包]

                //执行保存
                bool saveWalletResult = _bll.Save(argsWallet, argsWallet.Wal_ID);
                if (!saveWalletResult)
                {
                    DBManager.RollBackTransaction(DBCONFIG.Coeus);
                    ResultMsg = MsgHelp.GetMsg(MsgCode.E_0010, new object[] { SystemActionEnum.Name.SAVE + SystemTableEnums.Name.EWM_Wallet });
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
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0018, new object[] { SystemActionEnum.Name.SAVE, ex.Message });
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
        private bool ServerCheck(WalletCreateAccountUIModel paramModel)
        {
            //验证该客户是否已开户
            List<WalletInfoUIModel> resultWalletList = BLLCom.GetWalletListByOwnerInfo(paramModel.Wal_OwnerTypeName, paramModel.Wal_CustomerID, paramModel.Wal_AutoFactoryCode, paramModel.Wal_AutoFactoryOrgCode);
            if (resultWalletList.Count > 0)
            {
                //该客户已开户
                ResultMsg = MsgHelp.GetMsg(MsgCode.E_0000, new object[] { "客户：" + (string.IsNullOrEmpty(paramModel.AutoFactoryOrgName) ? paramModel.Wal_CustomerName : paramModel.AutoFactoryOrgName) + "已开户！" });
                return false;
            }
            return true;
        }

        /// <summary>
        /// 生成钱包账号
        /// </summary>
        /// <returns></returns>
        private string GenerateWalletNo()
        {
            string resultWalletNo = string.Empty;

            //汽配商户编码LoginInfoDAX.MCTCode
            string mctCode = LoginInfoDAX.MCTCode;
            resultWalletNo = (string)_bll.QueryForObject(SQLID.RIA_WalletCreateAccount_SQL01, mctCode);

            return resultWalletNo;
        }
        #endregion
    }
}
