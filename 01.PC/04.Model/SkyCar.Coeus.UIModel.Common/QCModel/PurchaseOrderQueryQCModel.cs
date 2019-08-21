using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 采购订单查询QCModel
    /// </summary>
    public class PurchaseOrderQueryQCModel : BaseQCModel
    {
        #region 公共属性-条件用
        /// <summary>
        /// 订单号
        /// </summary>
        public String WHERE_PO_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_PO_Org_ID { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String WHERE_PO_SUPP_Name { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_PO_StatusName { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_PO_SourceTypeName { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_PO_ApprovalStatusName { get; set; }
        #endregion
    }
}
