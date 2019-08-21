using System;

namespace SkyCar.Coeus.UIModel.Common.UIModel
{
    /// <summary>
    /// 客户查询UIModel
    /// </summary>
    public class CustomerQueryUIModel : BaseUIModel
    {
        /// <summary>
        /// 行标识
        /// </summary>
        public string RowID { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; set; }

        #region 公共属性

        /// <summary>
        /// 客户ID
        /// </summary>
        public String CustomerID { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>
        public String CustomerType { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public String CustomerName { get; set; }
        /// <summary>
        /// 汽修商编码
        /// </summary>
        public String AutoFactoryCode { get; set; }
        /// <summary>
        /// 汽修商编码
        /// </summary>
        public String AutoFactoryName { get; set; }
        /// <summary>
        /// 汽修商组织ID
        /// </summary>
        public String AutoFactoryOrgID { get; set; }
        /// <summary>
        /// 汽修商组织编码
        /// </summary>
        public String AutoFactoryOrgCode { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public String CustomerPhoneNo { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public String CustomerAddress { get; set; }
        /// <summary>
        /// 信用额度
        /// </summary>
        public Decimal? CreditAmount { get; set; }
        /// <summary>
        /// 欠款金额
        /// </summary>
        public Decimal? DebtAmount { get; set; }
        /// <summary>
        /// 默认支付类型编码
        /// </summary>
        public String PaymentTypeCode { get; set; }
        /// <summary>
        /// 默认支付类型名称
        /// </summary>
        public String PaymentTypeName { get; set; }
        /// <summary>
        /// 默认开票类型编码
        /// </summary>
        public String BillingTypeCode { get; set; }
        /// <summary>
        /// 默认开票类型名称
        /// </summary>
        public String BillingTypeName { get; set; }
        /// <summary>
        /// 默认物流人员类型编码
        /// </summary>
        public String DeliveryTypeCode { get; set; }
        /// <summary>
        /// 默认物流人员类型名称
        /// </summary>
        public String DeliveryTypeName { get; set; }
        /// <summary>
        /// 默认物流人员ID
        /// </summary>
        public String DeliveryByID { get; set; }
        /// <summary>
        /// 默认物流人员名称
        /// </summary>
        public String DeliveryByName { get; set; }
        /// <summary>
        /// 默认物流人员手机号
        /// </summary>
        public String DeliveryByPhone { get; set; }
        /// <summary>
        /// 终止销售
        /// </summary>
        public Boolean? IsEndSales { get; set; }
        #endregion

        #region 电子钱包
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
        /// 可用余额
        /// </summary>
        public Decimal? Wal_AvailableBalance { get; set; }
        #endregion

        /// <summary>
        /// 汽修商户组织信息（平台内汽修商专用）
        /// </summary>
        public String AutoFactoryOrgInfo { get; set; }
        /// <summary>
        /// 配件价格类别
        /// </summary>
        public String AutoPartsPriceType { get; set; }
    }
}
