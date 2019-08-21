using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 普通客户管理UIModel
    /// </summary>
    public class GeneralCustomerManagerUIModel : BaseUIModel
    {
        private bool _isChecked = false;
        /// <summary>
        /// 选择
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    PropertyValueChanged = true;
                }
                _isChecked = value;
            }
        }
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
        /// 修改人
        /// </summary>
        public String GC_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? GC_UpdatedTime { get; set; }
        /// <summary>
        /// 普通客户ID
        /// </summary>
        public String GC_ID { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? GC_VersionNo { get; set; }
    }
}
