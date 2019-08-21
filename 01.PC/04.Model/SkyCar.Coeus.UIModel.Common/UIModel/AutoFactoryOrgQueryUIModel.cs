using System;

namespace SkyCar.Coeus.UIModel.Common.UIModel
{
    /// <summary>
    /// 汽修商户组织查询UIModel
    /// </summary>
    public class AutoFactoryOrgQueryUIModel : BaseUIModel
    {
        #region 汽修商客户

        /// <summary>
        /// 汽修商客户ID
        /// </summary>
        public String AFC_ID { get; set; }
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String AFC_Code { get; set; }
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String AFC_Name { get; set; }
        /// <summary>
        /// 汽修商户组织编码
        /// </summary>
        public String AFC_AROrg_Code { get; set; }
        /// <summary>
        /// 汽修商户组织名称
        /// </summary>
        public String AFC_AROrg_Name { get; set; }
        /// <summary>
        /// 汽修商户组织联系人
        /// </summary>
        public String AFC_AROrg_Contacter { get; set; }
        /// <summary>
        /// 汽修商户组织联系方式
        /// </summary>
        public String AFC_AROrg_Phone { get; set; }
        /// <summary>
        /// 汽修商户组织地址
        /// </summary>
        public String AFC_AROrg_Address { get; set; }
        /// <summary>
        /// 信用额度
        /// </summary>
        public Decimal? AFC_CreditAmount { get; set; }
        /// <summary>
        /// 默认支付类型编码
        /// </summary>
        public String AFC_PaymentTypeCode { get; set; }
        /// <summary>
        /// 默认支付类型名称
        /// </summary>
        public String AFC_PaymentTypeName { get; set; }
        /// <summary>
        /// 默认开票类型编码
        /// </summary>
        public String AFC_BillingTypeCode { get; set; }
        /// <summary>
        /// 默认开票类型名称
        /// </summary>
        public String AFC_BillingTypeName { get; set; }
        /// <summary>
        /// 默认物流人员类型编码
        /// </summary>
        public String AFC_DeliveryTypeCode { get; set; }
        /// <summary>
        /// 默认物流人员类型名称
        /// </summary>
        public String AFC_DeliveryTypeName { get; set; }
        /// <summary>
        /// 默认物流人员ID
        /// </summary>
        public String AFC_DeliveryByID { get; set; }
        /// <summary>
        /// 默认物流人员名称
        /// </summary>
        public String AFC_DeliveryByName { get; set; }
        /// <summary>
        /// 默认物流人员手机号
        /// </summary>
        public String AFC_DeliveryByPhoneNo { get; set; }
        /// <summary>
        /// 终止销售
        /// </summary>
        public Boolean? AFC_IsEndSales { get; set; }
        /// <summary>
        /// 配件价格类别
        /// </summary>
        public String AFC_AutoPartsPriceType { get; set; }
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

        #region 其他属性

        /// <summary>
        /// 行标识
        /// </summary>
        public string RowID { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// 汽修商户组织ID
        /// </summary>
        public String AutoFactoryOrgID { get; set; }

        #endregion
    }
}
