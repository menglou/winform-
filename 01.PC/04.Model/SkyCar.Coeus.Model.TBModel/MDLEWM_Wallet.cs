using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 电子钱包Model
    /// </summary>
    public class MDLEWM_Wallet
    {
        #region 公共属性
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
        /// 生效时间-开始（查询条件用）
        /// </summary>
        public DateTime? _EffectiveTimeStart { get; set; }
        /// <summary>
        /// 生效时间-终了（查询条件用）
        /// </summary>
        public DateTime? _EffectiveTimeEnd { get; set; }
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime? Wal_IneffectiveTime { get; set; }
        /// <summary>
        /// 失效时间-开始（查询条件用）
        /// </summary>
        public DateTime? _IneffectiveTimeStart { get; set; }
        /// <summary>
        /// 失效时间-终了（查询条件用）
        /// </summary>
        public DateTime? _IneffectiveTimeEnd { get; set; }
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
        /// 创建时间-开始（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeStart { get; set; }
        /// <summary>
        /// 创建时间-终了（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeEnd { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String Wal_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Wal_UpdatedTime { get; set; }
        /// <summary>
        /// 修改时间-开始（查询条件用）
        /// </summary>
        public DateTime? _UpdatedTimeStart { get; set; }
        /// <summary>
        /// 修改时间-终了（查询条件用）
        /// </summary>
        public DateTime? _UpdatedTimeEnd { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? Wal_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String Wal_TransID { get; set; }
        /// <summary>
        /// 总记录条数（翻页用）
        /// </summary>
        public Int32? RecordCount { get; set; }
        /// <summary>
        /// 当前页面索引/要跳转的页码（翻页用）
        /// </summary>
        public Int32? PageIndex { get; set; }
        /// <summary>
        /// 页面大小（翻页用）
        /// </summary>
        public Int32? PageSize { get; set; }
        #endregion

        #region 公共属性-条件用
        /// <summary>
        /// 钱包ID
        /// </summary>
        public String WHERE_Wal_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_Wal_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_Wal_Org_Name { get; set; }
        /// <summary>
        /// 钱包账号
        /// </summary>
        public String WHERE_Wal_No { get; set; }
        /// <summary>
        /// 钱包来源类型编码
        /// </summary>
        public String WHERE_Wal_SourceTypeCode { get; set; }
        /// <summary>
        /// 钱包来源类型名称
        /// </summary>
        public String WHERE_Wal_SourceTypeName { get; set; }
        /// <summary>
        /// 来源账号
        /// </summary>
        public String WHERE_Wal_SourceNo { get; set; }
        /// <summary>
        /// 钱包所有人类别编码
        /// </summary>
        public String WHERE_Wal_OwnerTypeCode { get; set; }
        /// <summary>
        /// 钱包所有人类别名称
        /// </summary>
        public String WHERE_Wal_OwnerTypeName { get; set; }
        /// <summary>
        /// 开户人ID
        /// </summary>
        public String WHERE_Wal_CustomerID { get; set; }
        /// <summary>
        /// 开户人姓名
        /// </summary>
        public String WHERE_Wal_CustomerName { get; set; }
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String WHERE_Wal_AutoFactoryCode { get; set; }
        /// <summary>
        /// 汽修商户组织编码
        /// </summary>
        public String WHERE_Wal_AutoFactoryOrgCode { get; set; }
        /// <summary>
        /// 交易密码
        /// </summary>
        public String WHERE_Wal_TradingPassword { get; set; }
        /// <summary>
        /// 可用余额
        /// </summary>
        public Decimal? WHERE_Wal_AvailableBalance { get; set; }
        /// <summary>
        /// 冻结余额
        /// </summary>
        public Decimal? WHERE_Wal_FreezingBalance { get; set; }
        /// <summary>
        /// 充值基数
        /// </summary>
        public Decimal? WHERE_Wal_DepositBaseAmount { get; set; }
        /// <summary>
        /// 推荐员工ID
        /// </summary>
        public String WHERE_Wal_RecommendEmployeeID { get; set; }
        /// <summary>
        /// 推荐员工
        /// </summary>
        public String WHERE_Wal_RecommendEmployee { get; set; }
        /// <summary>
        /// 开户组织ID
        /// </summary>
        public String WHERE_Wal_CreatedByOrgID { get; set; }
        /// <summary>
        /// 开户组织名称
        /// </summary>
        public String WHERE_Wal_CreatedByOrgName { get; set; }
        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime? WHERE_Wal_EffectiveTime { get; set; }
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime? WHERE_Wal_IneffectiveTime { get; set; }
        /// <summary>
        /// 钱包状态编码
        /// </summary>
        public String WHERE_Wal_StatusCode { get; set; }
        /// <summary>
        /// 钱包状态名称
        /// </summary>
        public String WHERE_Wal_StatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_Wal_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_Wal_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_Wal_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_Wal_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_Wal_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_Wal_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_Wal_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_Wal_TransID { get; set; }
        #endregion

    }
}
