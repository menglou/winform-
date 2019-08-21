using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 采购补货建议查询UIModel
    /// </summary>
    public class PurchaseForecastOrderQueryUIModel : BaseUIModel
    {
        /// <summary>
        /// 单号
        /// </summary>
        public String PFO_No { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String PFO_SUPP_Name { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String PFO_SourceTypeName { get; set; }
        /// <summary>
        /// 订单总额
        /// </summary>
        public Decimal? PFO_TotalAmount { get; set; }
        /// <summary>
        /// 单据状态
        /// </summary>
        public String PFO_StatusName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? PFO_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String PFO_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? PFO_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String PFO_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? PFO_UpdatedTime { get; set; }
        /// <summary>
        /// 采购预测订单ID
        /// </summary>
        public String PFO_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String PFO_Org_ID { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String PFO_SUPP_ID { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String PFO_SourceTypeCode { get; set; }
        /// <summary>
        /// 单据状态编码
        /// </summary>
        public String PFO_StatusCode { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? PFO_VersionNo { get; set; }
        /// <summary>
        /// 选择
        /// </summary>
        public Boolean IsChecked { get; set; }
    }
}
