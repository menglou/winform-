using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.UIModel
{
    /// <summary>
    /// 销售业绩统计汇总UIModel
    /// </summary>
    public class SalesPerformanceTotalReportUIModel : BaseNotificationUIModel
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
        /// 业务员ID
        /// </summary>
        public String SalesByID { get; set; }
        /// <summary>
        /// 业务员名称
        /// </summary>
        public String SalesByName { get; set; }

        /// <summary>
        /// 销售数量合计
        /// </summary>
        public Decimal? TotalSalesQty { get; set; }
        /// <summary>
        /// 销售金额合计
        /// </summary>
        public Decimal? TotalSalesAmount { get; set; }
        /// <summary>
        /// 销售成本合计
        /// </summary>
        public Decimal? TotalCostAmount { get; set; }
        /// <summary>
        /// 毛利润合计
        /// </summary>
        public Decimal? TotalGrossProfitAmount { get; set; }
        /// <summary>
        /// 退货数量合计
        /// </summary>
        public Decimal? TotalSalesReturnQty { get; set; }
        /// <summary>
        /// 退货金额合计
        /// </summary>
        public Decimal? TotalSalesReturnAmount { get; set; }

        #region 汇总
        /// <summary>
        /// 销售数量
        /// </summary>
        public Decimal? SalesQty { get; set; }
        /// <summary>
        /// 销售金额
        /// </summary>
        public Decimal? SalesAmount { get; set; }
        /// <summary>
        /// 销售成本
        /// </summary>
        public Decimal? CostAmount { get; set; }
        /// <summary>
        /// 毛利润
        /// </summary>
        public Decimal? GrossProfitAmount { get; set; }
        /// <summary>
        /// 退货数量
        /// </summary>
        public Decimal? SalesReturnQty { get; set; }
        /// <summary>
        /// 退货金额
        /// </summary>
        public Decimal? SalesReturnAmount { get; set; }

        /// <summary>
        /// 退货数量比例
        /// </summary>
        public Decimal? SalesReturnQtyPercent { get; set; }
        /// <summary>
        /// 退货金额比例
        /// </summary>
        public Decimal? SalesReturnAmountPercent { get; set; }
        #endregion
    }
}
