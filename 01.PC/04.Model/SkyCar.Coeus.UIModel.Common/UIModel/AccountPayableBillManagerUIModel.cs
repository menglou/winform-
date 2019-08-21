using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 应付查询UIModel
    /// </summary>
    public class AccountPayableBillQueryUIModel : BaseUIModel
    {
        /// <summary>
        /// 应付单号
        /// </summary>
        public String APB_No { get; set; }
        /// <summary>
        /// 单据方向
        /// </summary>
        public String APB_BillDirectName { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String APB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String APB_SourceBillNo { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String APB_Org_Name { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public Decimal? APB_AccountPayableAmount { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
        public Decimal? APB_PaidAmount { get; set; }
        /// <summary>
        /// 未付金额
        /// </summary>
        public Decimal? APB_UnpaidAmount { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        public String APB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String APB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String APB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String APB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APB_UpdatedTime { get; set; }
        /// <summary>
        /// 应付单ID
        /// </summary>
        public String APB_ID { get; set; }
        /// <summary>
        /// 单据方向编码
        /// </summary>
        public String APB_BillDirectCode { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String APB_SourceTypeCode { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String APB_Org_ID { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String APB_BusinessStatusCode { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String APB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? APB_VersionNo { get; set; }
        /// <summary>
        /// 行标识
        /// </summary>
        public string RowID { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; set; }
    }
}
