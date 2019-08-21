using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.UIModel
{
    /// <summary>
    /// 销售业绩统计明细UIModel
    /// </summary>
    public class SalesPerformanceDetailReportUIModel : BaseNotificationUIModel
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
        /// 数量合计
        /// </summary>
        public Decimal? TotalSalesQty { get; set; }
        /// <summary>
        /// 销售金额合计
        /// </summary>
        public Decimal? TotalSalesAmount { get; set; }
        /// <summary>
        /// 成本金额合计
        /// </summary>
        public Decimal? TotalCostAmount { get; set; }
        /// <summary>
        /// 毛利润合计
        /// </summary>
        public Decimal? TotalGrossProfitAmount { get; set; }

        #region 明细

        /// <summary>
        /// 单据编号
        /// </summary>
        public String SO_No { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String SO_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String SO_SourceNo { get; set; }
        /// <summary>
        /// 客户类型名称
        /// </summary>
        public String SO_CustomerTypeName { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public String SO_CustomerName { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public String SO_CustomerID { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SO_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SO_CreatedTime { get; set; }

        /// <summary>
        /// 单据销售数量：签收+拒收（非退货单）/退货数量（退货单）
        /// </summary>
        public Decimal? SalesTotalQty { get; set; }
        /// <summary>
        /// 单据销售金额：签收+拒收（非退货单）/退货数量（退货单）
        /// </summary>
        public Decimal? SalesTotalAmount { get; set; }
        /// <summary>
        /// 单据成本金额：非退货单
        /// </summary>
        public Decimal? CostTotalAmount { get; set; }
        /// <summary>
        /// 单据毛利润：非退货单
        /// </summary>
        public Decimal? GrossProfitTotalAmount { get; set; }
        #endregion
    }
}
