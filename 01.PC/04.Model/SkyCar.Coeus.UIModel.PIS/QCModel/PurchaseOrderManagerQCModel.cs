using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 采购订单管理QCModel
    /// </summary>
    public class PurchaseOrderManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public String WHERE_PO_No { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public String WHERE_PO_SUPP_Name { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String WHERE_PO_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String WHERE_PO_SourceNo { get; set; }
        /// <summary>
        /// 单据状态
        /// </summary>
        public String WHERE_PO_StatusName { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_PO_ApprovalStatusName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_PO_IsValid { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_PO_Org_ID { get; set; }
        /// <summary>
        /// 采购ID
        /// </summary>
        public String WHERE_POD_PO_ID { get; set; }
    }
}
