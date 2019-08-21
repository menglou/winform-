using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.QCModel
{
    /// <summary>
    /// 销售业绩统计QCModel
    /// </summary>
    public class SalesPerformanceReportQCModel : BaseQCModel
    {
        /// <summary>
        /// 组织ID List
        /// </summary>
        public String OrgIdList { get; set; }
        /// <summary>
        /// 业务员ID
        /// </summary>
        public String SalesByID { get; set; }
        /// <summary>
        /// 销售时间开始
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 销售时间结束
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
