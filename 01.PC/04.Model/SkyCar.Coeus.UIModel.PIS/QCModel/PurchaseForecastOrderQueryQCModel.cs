using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 采购补货建议查询QCModel
    /// </summary>
    public class PurchaseForecastOrderQueryQCModel : BaseQCModel
    {
        /// <summary>
        /// 单号
        /// </summary>
        public String WHERE_PFO_No { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String WHERE_PFO_SUPP_Name { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_PFO_SourceTypeName { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_PFO_StatusName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_PFO_IsValid { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_PFO_Org_ID { get; set; }
        /// <summary>
        /// 采购预测订单ID
        /// </summary>
        public String WHERE_PFOD_PFO_ID { get; set; }
    }
}
