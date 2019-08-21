using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SD
{
    /// <summary>
    /// 主动销售管理QCModel
    /// </summary>
    public class ProactiveSalesManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String WHERE_SO_SourceTypeCode { get; set; }
        /// <summary>
        /// 客户类型编码
        /// </summary>
        public String WHERE_SO_CustomerTypeCode { get; set; }
        /// <summary>
        /// 客户类型名称
        /// </summary>
        public String WHERE_SO_CustomerTypeName { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_SO_SourceTypeName { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public String WHERE_SO_CustomerName { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String WHERE_SO_StatusCode { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_SO_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_SO_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_SO_ApprovalStatusName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SO_IsValid { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public String WHERE_SO_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_SO_Org_ID { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String WHERE_SO_SourceNo { get; set; }

        /// <summary>
        /// 是否含税
        /// </summary>
        public Boolean? WHERE_SO_IsPriceIncludeTax { get; set; }
        /// <summary>
        /// 创建时间-开始（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeStart { get; set; }
        /// <summary>
        /// 创建时间-终了（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeEnd { get; set; }

        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String WHERE_AutoFactoryName { get; set; }
    }
}
