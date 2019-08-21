using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 采购订单管理UIModel
    /// </summary>
    public class PurchaseOrderManagerUIModel : BaseUIModel
    {
        #region 采购订单属性
        /// <summary>
        /// 订单号
        /// </summary>
        public String PO_No { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public String PO_SUPP_Name { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String PO_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String PO_SourceNo { get; set; }
        /// <summary>
        /// 订单总额
        /// </summary>
        public Decimal? PO_TotalAmount { get; set; }
        /// <summary>
        /// 物流费
        /// </summary>
        public Decimal? PO_LogisticFee { get; set; }
        /// <summary>
        /// 单据状态
        /// </summary>
        public String PO_StatusName { get; set; }
        /// <summary>
        /// 审核状态
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
        /// 采购订单ID
        /// </summary>
        public String PO_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String PO_Org_ID { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String PO_SUPP_ID { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String PO_SourceTypeCode { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String PO_StatusCode { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String PO_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? PO_VersionNo { get; set; }
        #endregion

        #region 其他属性
        /// <summary>
        /// 选择
        /// </summary>
        public Boolean IsChecked { get; set; }
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
        /// <summary>
        /// 组织名称
        /// </summary>
        public string Org_ShortName { get; set; }
        #endregion

    }
}
