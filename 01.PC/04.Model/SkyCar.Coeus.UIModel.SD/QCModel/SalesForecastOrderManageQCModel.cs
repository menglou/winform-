using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SD
{
    /// <summary>
    /// 销售补货建议查询QCModel
    /// </summary>
    public class SalesForecastOrderManageQCModel : BaseQCModel
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_SFO_Org_ID { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public String WHERE_SFO_No { get; set; }
        /// <summary>
        /// 汽修商户名称
        /// </summary>
        public String WHERE_SFO_AutoFactoryName { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public String WHERE_SFO_CustomerName { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_SFO_SourceTypeName { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_SFO_StatusName { get; set; }
        /// <summary>
        /// 创建时间-开始（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeStart { get; set; }
        /// <summary>
        /// 创建时间-终了（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeEnd { get; set; }
    }
}
