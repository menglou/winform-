using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 汽修商客户Model
    /// </summary>
    public class MDLPIS_AutoFactoryCustomer
    {
        #region 公共属性
        /// <summary>
        /// 汽修商客户ID
        /// </summary>
        public String AFC_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String AFC_Org_ID { get; set; }
        /// <summary>
        /// 是否平台商户
        /// </summary>
        public Boolean? AFC_IsPlatform { get; set; }
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
        public String AFC_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? AFC_UpdatedTime { get; set; }
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
        public Int64? AFC_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String AFC_TransID { get; set; }
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
        /// 汽修商客户ID
        /// </summary>
        public String WHERE_AFC_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_AFC_Org_ID { get; set; }
        /// <summary>
        /// 是否平台商户
        /// </summary>
        public Boolean? WHERE_AFC_IsPlatform { get; set; }
        /// <summary>
        /// 汽修商编码
        /// </summary>
        public String WHERE_AFC_Code { get; set; }
        /// <summary>
        /// 汽修商名称
        /// </summary>
        public String WHERE_AFC_Name { get; set; }
        /// <summary>
        /// 汽修商联系人
        /// </summary>
        public String WHERE_AFC_Contacter { get; set; }
        /// <summary>
        /// 汽修商联系方式
        /// </summary>
        public String WHERE_AFC_PhoneNo { get; set; }
        /// <summary>
        /// 汽修商地址
        /// </summary>
        public String WHERE_AFC_Address { get; set; }
        /// <summary>
        /// 汽修组织编码
        /// </summary>
        public String WHERE_AFC_AROrg_Code { get; set; }
        /// <summary>
        /// 汽修组织名称
        /// </summary>
        public String WHERE_AFC_AROrg_Name { get; set; }
        /// <summary>
        /// 汽修组织联系人
        /// </summary>
        public String WHERE_AFC_AROrg_Contacter { get; set; }
        /// <summary>
        /// 汽修组织联系方式
        /// </summary>
        public String WHERE_AFC_AROrg_Phone { get; set; }
        /// <summary>
        /// 汽修组织地址
        /// </summary>
        public String WHERE_AFC_AROrg_Address { get; set; }
        /// <summary>
        /// 信用额度
        /// </summary>
        public Decimal? WHERE_AFC_CreditAmount { get; set; }
        /// <summary>
        /// 默认支付类型编码
        /// </summary>
        public String WHERE_AFC_PaymentTypeCode { get; set; }
        /// <summary>
        /// 默认支付类型名称
        /// </summary>
        public String WHERE_AFC_PaymentTypeName { get; set; }
        /// <summary>
        /// 默认开票类型编码
        /// </summary>
        public String WHERE_AFC_BillingTypeCode { get; set; }
        /// <summary>
        /// 默认开票类型名称
        /// </summary>
        public String WHERE_AFC_BillingTypeName { get; set; }
        /// <summary>
        /// 默认物流人员类型编码
        /// </summary>
        public String WHERE_AFC_DeliveryTypeCode { get; set; }
        /// <summary>
        /// 默认物流人员类型名称
        /// </summary>
        public String WHERE_AFC_DeliveryTypeName { get; set; }
        /// <summary>
        /// 默认物流人员ID
        /// </summary>
        public String WHERE_AFC_DeliveryByID { get; set; }
        /// <summary>
        /// 默认物流人员名称
        /// </summary>
        public String WHERE_AFC_DeliveryByName { get; set; }
        /// <summary>
        /// 默认物流人员手机号
        /// </summary>
        public String WHERE_AFC_DeliveryByPhoneNo { get; set; }
        /// <summary>
        /// 终止销售
        /// </summary>
        public Boolean? WHERE_AFC_IsEndSales { get; set; }
        /// <summary>
        /// 配件价格类别
        /// </summary>
        public String WHERE_AFC_AutoPartsPriceType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_AFC_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_AFC_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_AFC_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_AFC_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_AFC_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_AFC_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_AFC_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_AFC_TransID { get; set; }
        #endregion

    }
}
