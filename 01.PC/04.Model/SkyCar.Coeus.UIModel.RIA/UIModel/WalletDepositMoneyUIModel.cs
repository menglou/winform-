using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RIA.UIModel
{
    /// <summary>
    /// 钱包充值UIModel
    /// </summary>
    public class WalletDepositMoneyUIModel : BaseUIModel
    {
        /// <summary>
        /// 钱包ID
        /// </summary>
        public String Wal_ID { get; set; }
        /// <summary>
        /// 钱包账号
        /// </summary>
        public String Wal_No { get; set; }
        /// <summary>
        /// 开户人ID
        /// </summary>
        public String Wal_CustomerID { get; set; }
        /// <summary>
        /// 开户人姓名
        /// </summary>
        public String Wal_CustomerName { get; set; }
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String Wal_AutoFactoryCode { get; set; }
        /// <summary>
        /// 汽修商户组织编码
        /// </summary>
        public String Wal_AutoFactoryOrgCode { get; set; }
        /// <summary>
        /// 可用余额
        /// </summary>
        public Decimal? Wal_AvailableBalance { get; set; }
        /// <summary>
        /// 充值基数
        /// </summary>
        public Decimal? Wal_DepositBaseAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String Wal_Remark { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? Wal_VersionNo { get; set; }

        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String AutoFactoryName { get; set; }
        /// <summary>
        /// 本次充值金额
        /// </summary>
        public Decimal? ThisDepositAmount { get; set; }
        /// <summary>
        /// 支付方式名称
        /// </summary>
        public String PaymentModeName { get; set; }
        /// <summary>
        /// 支付方式编码
        /// </summary>
        public String PaymentModeCode { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public Decimal? PayAmount { get; set; }
    }
}
