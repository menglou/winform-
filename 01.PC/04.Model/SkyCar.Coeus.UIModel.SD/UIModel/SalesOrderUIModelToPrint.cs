using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SD
{
    /// <summary>
    /// 销售订单管理UIModel
    /// </summary>
    public class SalesOrderUIModelToPrint : BaseUIModel
    {
        /// <summary>
        /// 单据编号
        /// </summary>
        public String SO_No { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String SO_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String SO_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String SO_SourceNo { get; set; }
        /// <summary>
        /// 客户类型编码
        /// </summary>
        public String SO_CustomerTypeCode { get; set; }
        /// <summary>
        /// 客户类型名称
        /// </summary>
        public String SO_CustomerTypeName { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public String SO_CustomerName { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public String SO_CustomerID { get; set; }
        /// <summary>
        /// 是否价格含税
        /// </summary>
        public String SO_IsPriceIncludeTax { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public Decimal? SO_TaxRate { get; set; }
        /// <summary>
        /// 税额
        /// </summary>
        public Decimal? SO_TotalTax { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public Decimal? SO_TotalAmount { get; set; }
        /// <summary>
        /// 未税总金额
        /// </summary>
        public Decimal? SO_TotalNetAmount { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String SO_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String SO_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String SO_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String SO_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String SO_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SO_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SO_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SO_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String SO_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SO_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SO_VersionNo { get; set; }
        /// <summary>
        /// 销售订单ID
        /// </summary>
        public String SO_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String SO_Org_ID { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public String CustomerPhoneNo { get; set; }
        /// <summary>
        /// 汽修商组织名称
        /// </summary>
        public String AROrgName { get; set; }
        /// <summary>
        /// 汽修商组织联系方式
        /// </summary>
        public String AROrgPhone { get; set; }
        /// <summary>
        /// 汽修商组织地址
        /// </summary>
        public String AROrgAddress { get; set; }
       
        /// <summary>
        /// 总销售数量
        /// </summary>
        public decimal TotalSalesQty { get; set; }

    }
}
