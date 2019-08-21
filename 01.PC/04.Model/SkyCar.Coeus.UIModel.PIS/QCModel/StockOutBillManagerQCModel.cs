using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 出库管理QCModel
    /// </summary>
    public class StockOutBillManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_SOB_Org_ID { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public String WHERE_SOB_No { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_SOB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String WHERE_SOB_SourceNo { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String WHERE_SOB_SUPP_Name { get; set; }
        /// <summary>
        /// 单据状态
        /// </summary>
        public String WHERE_SOB_StatusName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String WHERE_SOB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SOB_IsValid { get; set; }
        /// <summary>
        /// 出库单ID
        /// </summary>
        public String WHERE_SOBD_SOB_ID { get; set; }
        /// <summary>
        /// 出库单号
        /// </summary>
        public String WHERE_SOBD_SOB_No { get; set; }
    }
}
