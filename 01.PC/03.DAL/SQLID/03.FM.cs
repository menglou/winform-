namespace SkyCar.Coeus.DAL
{
    /// <summary>
    /// 财务管理
    /// </summary>
    public partial class SQLID
    {
        #region 应收管理

        /// <summary>
        /// 查询应收单列表
        /// </summary>
        public static string FM_AccountReceivableBillManager_SQL_01 = "AccountReceivableBillManager_SQL_01";

        /// <summary>
        /// 查询收付单列表
        /// </summary>
        public static string FM_AccountReceivableBillManager_SQL_02 = "AccountReceivableBillManager_SQL_02";

        #endregion

        #region 应付管理

        /// <summary>
        /// 查询应收单列表
        /// </summary>
        public static string FM_AccountPayableBillManager_SQL_01 = "AccountPayableBillManager_SQL_01";
        /// <summary>
        /// 查询应收单明细列表
        /// </summary>
        public static string FM_AccountPayableBillManager_SQL_02 = "AccountPayableBillManager_SQL_02";
        
        #endregion

        #region 收款单管理

        /// <summary>
        /// 查询收款单列表
        /// </summary>
        public static string FM_ReceiptBillManager_SQL01 = "ReceiptBillManager_SQL01";
        /// <summary>
        /// 删除图片时更新收款单
        /// </summary>
        public static string FM_ReceiptBillManager_SQL02 = "ReceiptBillManager_SQL02";

        #endregion

        #region 付款单管理
        
        /// <summary>
        /// 删除图片时更新付款单
        /// </summary>
        public static string FM_PayBillManager_SQL01 = "PayBillManager_SQL01";
        /// <summary>
        /// 查询付款单列表
        /// </summary>
        public static string FM_PayBillManager_SQL02 = "PayBillManager_SQL02";

        #endregion
    }
}
