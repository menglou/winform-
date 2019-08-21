using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 销售订单查询QCModel
    /// </summary>
    public class SalesOrderQueryQCModel : BaseQCModel
    {
        #region 公共属性-条件用
        /// <summary>
        /// 单据编号
        /// </summary>
        public String WHERE_SO_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_SO_Org_ID { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_SO_SourceTypeName { get; set; }
        /// <summary>
        /// 客户类型名称
        /// </summary>
        public String WHERE_SO_CustomerTypeName { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public String WHERE_SO_CustomerName { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_SO_StatusName { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_SO_ApprovalStatusName { get; set; }
        #endregion
    }
}
