using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 应付查询QCModel
    /// </summary>
    public class AccountPayableBillQueryQCModel : BaseQCModel
    {
        /// <summary>
        /// 应付单号
        /// </summary>
        public String WHERE_APB_No { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String WHERE_APB_SourceTypeName { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        public String WHERE_APB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String WHERE_APB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_APB_IsValid { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_APB_Org_ID { get; set; }
    }
}
