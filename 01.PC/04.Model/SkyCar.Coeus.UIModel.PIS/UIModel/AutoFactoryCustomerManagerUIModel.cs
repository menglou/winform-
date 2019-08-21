using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 汽修商管理UIModel
    /// </summary>
    public class AutoFactoryCustomerManagerUIModel : BaseUIModel
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public String AFC_Org_ID { get; set; }
        /// <summary>
        /// 汽修商编码
        /// </summary>
        public String AFC_Code { get; set; }
        /// <summary>
        /// 汽修商名称
        /// </summary>
        public String AFC_Name { get; set; }
        /// <summary>
        /// 汽修商联系人
        /// </summary>
        public String AFC_Contacter { get; set; }
        /// <summary>
        /// 汽修商联系方式
        /// </summary>
        public String AFC_PhoneNo { get; set; }
        /// <summary>
        /// 汽修商地址
        /// </summary>
        public String AFC_Address { get; set; }
        /// <summary>
        /// 汽修组织编码
        /// </summary>
        public String AFC_AROrg_Code { get; set; }
        /// <summary>
        /// 汽修组织名称
        /// </summary>
        public String AFC_AROrg_Name { get; set; }
        /// <summary>
        /// 汽修组织联系人
        /// </summary>
        public String AFC_AROrg_Contacter { get; set; }
        /// <summary>
        /// 汽修组织联系方式
        /// </summary>
        public String AFC_AROrg_Phone { get; set; }
        /// <summary>
        /// 汽修组织地址
        /// </summary>
        public String AFC_AROrg_Address { get; set; }
        /// <summary>
        /// 是否平台商户
        /// </summary>
        public Boolean? AFC_IsPlatform { get; set; }
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
        /// <summary>
        /// 备注
        /// </summary>
        public String AFC_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? AFC_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String AFC_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? AFC_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String AFC_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? AFC_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? AFC_VersionNo { get; set; }

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
        /// 汽修商客户ID
        /// </summary>
        public String AFC_ID { get; set; }
    }
}
