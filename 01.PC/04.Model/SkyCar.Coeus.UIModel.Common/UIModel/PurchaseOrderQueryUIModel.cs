using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 采购订单查询UIModel
    /// </summary>
    public class PurchaseOrderQueryUIModel : BaseUIModel
    {
        /// <summary>
        /// 行标识
        /// </summary>
        public string RowID { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// 应付金额
        /// </summary>
        public decimal? APB_AccountPayableAmount { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
        public decimal? APB_PaidAmount { get; set; }
        /// <summary>
        /// 未付金额
        /// </summary>
        public decimal? APB_UnpaidAmount { get; set; }

        #region 公共属性
        /// <summary>
        /// 采购订单ID
        /// </summary>
        public String PO_ID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public String PO_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String PO_Org_ID { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String PO_SUPP_ID { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String PO_SUPP_Name { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String PO_SourceTypeCode { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String PO_SourceTypeName { get; set; }
        /// <summary>
        /// 订单总额
        /// </summary>
        public Decimal? PO_TotalAmount { get; set; }
        /// <summary>
        /// 物流费
        /// </summary>
        public Decimal? PO_LogisticFee { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String PO_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String PO_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String PO_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String PO_ApprovalStatusName { get; set; }
        /// <summary>
        /// 到货时间
        /// </summary>
        public DateTime? PO_ReceivedTime { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? PO_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String PO_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? PO_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String PO_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? PO_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? PO_VersionNo { get; set; }
        #endregion
    }
}
