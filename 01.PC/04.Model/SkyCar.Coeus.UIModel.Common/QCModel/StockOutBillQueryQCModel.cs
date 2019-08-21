using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 出库查询QCModel
    /// </summary>
    public class StockInBillQueryQCModel : BaseQCModel
    {
        #region 公共属性-条件用
        /// <summary>
        /// 出库单号
        /// </summary>
        public String WHERE_SIB_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_SIB_Org_ID { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_SIB_SourceTypeName { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_SIB_ApprovalStatusName { get; set; }
        #endregion
    }
}
