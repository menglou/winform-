using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 销售订单Model
    /// </summary>
    public class MDLSD_SalesOrder
    {
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
        /// 配件价格类别
        /// </summary>
        public String SO_AutoPartsPriceType { get; set; }
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
        public String SO_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SO_UpdatedTime { get; set; }
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
        public Int64? SO_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String SO_TransID { get; set; }
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
        /// 销售订单ID
        /// </summary>
        public String WHERE_SO_ID { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public String WHERE_SO_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_SO_Org_ID { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String WHERE_SO_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_SO_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String WHERE_SO_SourceNo { get; set; }
        /// <summary>
        /// 客户类型编码
        /// </summary>
        public String WHERE_SO_CustomerTypeCode { get; set; }
        /// <summary>
        /// 客户类型名称
        /// </summary>
        public String WHERE_SO_CustomerTypeName { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public String WHERE_SO_CustomerID { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public String WHERE_SO_CustomerName { get; set; }
        /// <summary>
        /// 是否价格含税
        /// </summary>
        public Boolean? WHERE_SO_IsPriceIncludeTax { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public Decimal? WHERE_SO_TaxRate { get; set; }
        /// <summary>
        /// 税额
        /// </summary>
        public Decimal? WHERE_SO_TotalTax { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public Decimal? WHERE_SO_TotalAmount { get; set; }
        /// <summary>
        /// 未税总金额
        /// </summary>
        public Decimal? WHERE_SO_TotalNetAmount { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String WHERE_SO_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_SO_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_SO_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_SO_ApprovalStatusName { get; set; }
        /// <summary>
        /// 配件价格类别
        /// </summary>
        public String WHERE_SO_AutoPartsPriceType { get; set; }
        /// <summary>
        /// 业务员ID
        /// </summary>
        public String WHERE_SO_SalesByID { get; set; }
        /// <summary>
        /// 业务员名称
        /// </summary>
        public String WHERE_SO_SalesByName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_SO_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SO_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_SO_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_SO_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_SO_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_SO_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_SO_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_SO_TransID { get; set; }
        #endregion

    }
}
