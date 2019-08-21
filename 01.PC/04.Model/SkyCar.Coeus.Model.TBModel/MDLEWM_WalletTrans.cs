using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 电子钱包异动Model
    /// </summary>
    public class MDLEWM_WalletTrans
    {
        #region 公共属性
        /// <summary>
        /// 钱包异动ID
        /// </summary>
        public String WalT_ID { get; set; }
        /// <summary>
        /// 受理组织ID
        /// </summary>
        public String WalT_Org_ID { get; set; }
        /// <summary>
        /// 受理组织名称
        /// </summary>
        public String WalT_Org_Name { get; set; }
        /// <summary>
        /// 钱包ID
        /// </summary>
        public String WalT_Wal_ID { get; set; }
        /// <summary>
        /// 钱包账号
        /// </summary>
        public String WalT_Wal_No { get; set; }
        /// <summary>
        /// 异动时间
        /// </summary>
        public DateTime? WalT_Time { get; set; }
        /// <summary>
        /// 异动时间-开始（查询条件用）
        /// </summary>
        public DateTime? _TimeStart { get; set; }
        /// <summary>
        /// 异动时间-终了（查询条件用）
        /// </summary>
        public DateTime? _TimeEnd { get; set; }
        /// <summary>
        /// 异动类型编码
        /// </summary>
        public String WalT_TypeCode { get; set; }
        /// <summary>
        /// 异动类型名称
        /// </summary>
        public String WalT_TypeName { get; set; }
        /// <summary>
        /// 充值方式编码
        /// </summary>
        public String WalT_RechargeTypeCode { get; set; }
        /// <summary>
        /// 充值方式名称
        /// </summary>
        public String WalT_RechargeTypeName { get; set; }
        /// <summary>
        /// 通道编码
        /// </summary>
        public String WalT_ChannelCode { get; set; }
        /// <summary>
        /// 通道名称
        /// </summary>
        public String WalT_ChannelName { get; set; }
        /// <summary>
        /// 异动金额
        /// </summary>
        public Decimal? WalT_Amount { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public String WalT_BillNo { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WalT_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WalT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WalT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WalT_CreatedTime { get; set; }
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
        public String WalT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WalT_UpdatedTime { get; set; }
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
        public Int64? WalT_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WalT_TransID { get; set; }
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
        /// 钱包异动ID
        /// </summary>
        public String WHERE_WalT_ID { get; set; }
        /// <summary>
        /// 受理组织ID
        /// </summary>
        public String WHERE_WalT_Org_ID { get; set; }
        /// <summary>
        /// 受理组织名称
        /// </summary>
        public String WHERE_WalT_Org_Name { get; set; }
        /// <summary>
        /// 钱包ID
        /// </summary>
        public String WHERE_WalT_Wal_ID { get; set; }
        /// <summary>
        /// 钱包账号
        /// </summary>
        public String WHERE_WalT_Wal_No { get; set; }
        /// <summary>
        /// 异动时间
        /// </summary>
        public DateTime? WHERE_WalT_Time { get; set; }
        /// <summary>
        /// 异动类型编码
        /// </summary>
        public String WHERE_WalT_TypeCode { get; set; }
        /// <summary>
        /// 异动类型名称
        /// </summary>
        public String WHERE_WalT_TypeName { get; set; }
        /// <summary>
        /// 充值方式编码
        /// </summary>
        public String WHERE_WalT_RechargeTypeCode { get; set; }
        /// <summary>
        /// 充值方式名称
        /// </summary>
        public String WHERE_WalT_RechargeTypeName { get; set; }
        /// <summary>
        /// 通道编码
        /// </summary>
        public String WHERE_WalT_ChannelCode { get; set; }
        /// <summary>
        /// 通道名称
        /// </summary>
        public String WHERE_WalT_ChannelName { get; set; }
        /// <summary>
        /// 异动金额
        /// </summary>
        public Decimal? WHERE_WalT_Amount { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public String WHERE_WalT_BillNo { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_WalT_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_WalT_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_WalT_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_WalT_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_WalT_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_WalT_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_WalT_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_WalT_TransID { get; set; }
        #endregion

    }
}
