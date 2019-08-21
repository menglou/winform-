using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RIA.UIModel
{
    /// <summary>
    /// 钱包开户UIModel
    /// </summary>
    public class WalletCreateAccountUIModel : BaseUIModel
    {
        /// <summary>
        /// 选择
        /// </summary>
        public bool IsChecked { get; set; }
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
        /// 钱包来源类型编码
        /// </summary>
        public String Wal_SourceTypeCode { get; set; }
        /// <summary>
        /// 钱包来源类型名称
        /// </summary>
        public String Wal_SourceTypeName { get; set; }
        /// <summary>
        /// 来源账号
        /// </summary>
        public String Wal_SourceNo { get; set; }
        /// <summary>
        /// 钱包所有人类别编码
        /// </summary>
        public String Wal_OwnerTypeCode { get; set; }
        /// <summary>
        /// 钱包所有人类别名称
        /// </summary>
        public String Wal_OwnerTypeName { get; set; }
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
        /// 交易密码
        /// </summary>
        public String Wal_TradingPassword { get; set; }
        /// <summary>
        /// 可用余额
        /// </summary>
        public Decimal? Wal_AvailableBalance { get; set; }
        /// <summary>
        /// 冻结余额
        /// </summary>
        public Decimal? Wal_FreezingBalance { get; set; }
        /// <summary>
        /// 充值基数
        /// </summary>
        public Decimal? Wal_DepositBaseAmount { get; set; }
        /// <summary>
        /// 推荐员工ID
        /// </summary>
        public String Wal_RecommendEmployeeID { get; set; }
        /// <summary>
        /// 推荐员工
        /// </summary>
        public String Wal_RecommendEmployee { get; set; }
        /// <summary>
        /// 开户组织ID
        /// </summary>
        public String Wal_CreatedByOrgID { get; set; }
        /// <summary>
        /// 开户组织名称
        /// </summary>
        public String Wal_CreatedByOrgName { get; set; }
        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime? Wal_EffectiveTime { get; set; }
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime? Wal_IneffectiveTime { get; set; }
        /// <summary>
        /// 钱包状态编码
        /// </summary>
        public String Wal_StatusCode { get; set; }
        /// <summary>
        /// 钱包状态名称
        /// </summary>
        public String Wal_StatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String Wal_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? Wal_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String Wal_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Wal_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String Wal_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Wal_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? Wal_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String Wal_TransID { get; set; }
        
        /// <summary>
        /// 汽修商户组织名称
        /// </summary>
        public String AutoFactoryOrgName { get; set; }

        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String AutoFactoryName { get; set; }
    }
}
