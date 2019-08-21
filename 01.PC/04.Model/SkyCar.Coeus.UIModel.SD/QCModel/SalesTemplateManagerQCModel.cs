using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SD
{
    /// <summary>
    /// 主动销售模板管理QCModel
    /// </summary>
    public class SalesTemplateManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 销售模板名称
        /// </summary>
        public String WHERE_SasT_Name { get; set; }
        /// <summary>
        /// 汽修商客户ID
        /// </summary>
        public String WHERE_SasT_CustomerID { get; set; }
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String WHERE_SasT_AutoFactoryName { get; set; }
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String WHERE_SasT_CustomerName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_SasT_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_SasT_ApprovalStatusName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SasT_IsValid { get; set; }
        /// <summary>
        /// 销售套餐所属组织ID
        /// </summary>
        public String WHERE_SasT_Org_ID { get; set; }

        /// <summary>
        /// 来源组织
        /// </summary>
        public String WHERE_DP_Org_ID_From { get; set; }
        /// <summary>
        /// 目标组织
        /// </summary>
        public String WHERE_DP_Org_ID_To { get; set; }
        /// <summary>
        /// 下发数据ID
        /// </summary>
        public String WHERE_DP_SendDataID { get; set; }
        /// <summary>
        /// 汽修商组织ID
        /// </summary>
        public String WHERE_AROrgID { get; set; }
    }
}
