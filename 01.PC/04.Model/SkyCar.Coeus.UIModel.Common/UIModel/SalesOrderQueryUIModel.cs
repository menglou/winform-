using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 销售订单查询UIModel
    /// </summary>
    public class SalesOrderQueryUIModel : BaseUIModel
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
        /// 销售订单ID
        /// </summary>
        public String SO_ID { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public String SO_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String SO_Org_ID { get; set; }
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
        /// 客户ID
        /// </summary>
        public String SO_CustomerID { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public String SO_CustomerName { get; set; }
        /// <summary>
        /// 是否价格含税
        /// </summary>
        public Boolean? SO_IsPriceIncludeTax { get; set; }
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
        /// 业务员ID
        /// </summary>
        public String SO_SalesByID { get; set; }
        /// <summary>
        /// 业务员名称
        /// </summary>
        public String SO_SalesByName { get; set; }
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
        #endregion
    }
}
