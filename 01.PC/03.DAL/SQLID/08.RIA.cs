namespace SkyCar.Coeus.DAL
{
    /// <summary>
    /// 预收管理
    /// </summary>
    public partial class SQLID
    {
        #region 钱包开户

        /// <summary>
        /// 生成钱包账号
        /// </summary>
        public static string RIA_WalletCreateAccount_SQL01 = "WalletCreateAccount_SQL01";
        #endregion

        #region 钱包查询与操作

        /// <summary>
        /// 查询钱包列表
        /// </summary>
        public static string RIA_WalletQueryAndOperate_SQL01 = "WalletQueryAndOperate_SQL01";
        /// <summary>
        /// 更新钱包数据
        /// </summary>
        public static string RIA_WalletQueryAndOperate_SQL02 = "WalletQueryAndOperate_SQL02";
        #endregion

        #region 钱包金额流水查询

        /// <summary>
        /// 查询钱包金额流水列表
        /// </summary>
        public static string RIA_WalletTransLogQuery_SQL01 = "WalletTransLogQuery_SQL01";
        #endregion

    }
}
