using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.UIModel
{
    /// <summary>
    /// 经营情况统计明细UIModel
    /// </summary>
    public class ManagementSituationDetailReportUIModel : BaseNotificationUIModel
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public String OrgID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String OrgShortName { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public String CustomerID { get; set; }

        /// <summary>
        /// 统计类别
        /// </summary>
        public String StatisticsType { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public Decimal? SalesQty { get; set; }
        /// <summary>
        /// 销售金额
        /// </summary>
        public Decimal? SalesAmount { get; set; }
        /// <summary>
        /// 成本金额
        /// </summary>
        public Decimal? CostAmount { get; set; }
        /// <summary>
        /// 毛利润
        /// </summary>
        public Decimal? GrossProfitAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 配件名称
        /// </summary>
        public String APA_Name { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        public String APA_Barcode { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String APA_Specification { get; set; }
        /// <summary>
        /// 平均毛利润
        /// </summary>
        public Decimal? AvgGrossProfitAmount { get; set; }
    }
}
