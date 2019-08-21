using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RIA.UIModel
{
    /// <summary>
    /// 钱包提现UIModel
    /// </summary>
    public class WalletWithdrawalUIModel : BaseUIModel
    {
        /// <summary>
        /// 钱包ID
        /// </summary>
        public String Wal_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String Wal_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String Wal_Org_Name { get; set; }
        /// <summary>
        /// 钱包账号
        /// </summary>
        public String Wal_No { get; set; }
        /// <summary>
        /// 开户人姓名
        /// </summary>
        public String Wal_CustomerName { get; set; }
        /// <summary>
        /// 汽修商户组织编码
        /// </summary>
        public String Wal_AutoFactoryOrgCode { get; set; }
        /// <summary>
        /// 可用余额
        /// </summary>
        public Decimal? Wal_AvailableBalance { get; set; }
        /// <summary>
        /// 交易密码
        /// </summary>
        public String Wal_TradingPassword { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String Wal_Remark { get; set; }
        /// <summary>
        /// 本次提现金额
        /// </summary>
        public Decimal? ThisWithdrawalAmount { get; set; }
    }
}
