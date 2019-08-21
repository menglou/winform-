using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 普通客户Model
    /// </summary>
    public class MDLPIS_GeneralCustomer
    {
        #region 公共属性
        /// <summary>
        /// 普通客户ID
        /// </summary>
        public String GC_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String GC_Org_ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public String GC_Name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public String GC_PhoneNo { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public String GC_Address { get; set; }
        /// <summary>
        /// 信用额度
        /// </summary>
        public Decimal? GC_CreditAmount { get; set; }
        /// <summary>
        /// 默认支付类型编码
        /// </summary>
        public String GC_PaymentTypeCode { get; set; }
        /// <summary>
        /// 默认支付类型名称
        /// </summary>
        public String GC_PaymentTypeName { get; set; }
        /// <summary>
        /// 默认开票类型编码
        /// </summary>
        public String GC_BillingTypeCode { get; set; }
        /// <summary>
        /// 默认开票类型名称
        /// </summary>
        public String GC_BillingTypeName { get; set; }
        /// <summary>
        /// 默认物流人员类型编码
        /// </summary>
        public String GC_DeliveryTypeCode { get; set; }
        /// <summary>
        /// 默认物流人员类型名称
        /// </summary>
        public String GC_DeliveryTypeName { get; set; }
        /// <summary>
        /// 默认物流人员ID
        /// </summary>
        public String GC_DeliveryByID { get; set; }
        /// <summary>
        /// 默认物流人员名称
        /// </summary>
        public String GC_DeliveryByName { get; set; }
        /// <summary>
        /// 默认物流人员手机号
        /// </summary>
        public String GC_DeliveryByPhoneNo { get; set; }
        /// <summary>
        /// 终止销售
        /// </summary>
        public Boolean? GC_IsEndSales { get; set; }
        /// <summary>
        /// 配件价格类别
        /// </summary>
        public String GC_AutoPartsPriceType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String GC_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? GC_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String GC_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? GC_CreatedTime { get; set; }
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
        public String GC_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? GC_UpdatedTime { get; set; }
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
        public Int64? GC_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String GC_TransID { get; set; }
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
        /// 普通客户ID
        /// </summary>
        public String WHERE_GC_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_GC_Org_ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public String WHERE_GC_Name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public String WHERE_GC_PhoneNo { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public String WHERE_GC_Address { get; set; }
        /// <summary>
        /// 信用额度
        /// </summary>
        public Decimal? WHERE_GC_CreditAmount { get; set; }
        /// <summary>
        /// 默认支付类型编码
        /// </summary>
        public String WHERE_GC_PaymentTypeCode { get; set; }
        /// <summary>
        /// 默认支付类型名称
        /// </summary>
        public String WHERE_GC_PaymentTypeName { get; set; }
        /// <summary>
        /// 默认开票类型编码
        /// </summary>
        public String WHERE_GC_BillingTypeCode { get; set; }
        /// <summary>
        /// 默认开票类型名称
        /// </summary>
        public String WHERE_GC_BillingTypeName { get; set; }
        /// <summary>
        /// 默认物流人员类型编码
        /// </summary>
        public String WHERE_GC_DeliveryTypeCode { get; set; }
        /// <summary>
        /// 默认物流人员类型名称
        /// </summary>
        public String WHERE_GC_DeliveryTypeName { get; set; }
        /// <summary>
        /// 默认物流人员ID
        /// </summary>
        public String WHERE_GC_DeliveryByID { get; set; }
        /// <summary>
        /// 默认物流人员名称
        /// </summary>
        public String WHERE_GC_DeliveryByName { get; set; }
        /// <summary>
        /// 默认物流人员手机号
        /// </summary>
        public String WHERE_GC_DeliveryByPhoneNo { get; set; }
        /// <summary>
        /// 终止销售
        /// </summary>
        public Boolean? WHERE_GC_IsEndSales { get; set; }
        /// <summary>
        /// 配件价格类别
        /// </summary>
        public String WHERE_GC_AutoPartsPriceType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_GC_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_GC_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_GC_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_GC_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_GC_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_GC_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_GC_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_GC_TransID { get; set; }
        #endregion

    }
}
