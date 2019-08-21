using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 入库查询QCModel
    /// </summary>
    public class StockOutBillQueryQCModel : BaseQCModel
    {
        #region 公共属性-条件用
        /// <summary>
        /// 入库单号
        /// </summary>
        public String WHERE_SOB_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_SOB_Org_ID { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_SOB_SourceTypeName { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_SOB_ApprovalStatusName { get; set; }
        #endregion
    }
}
