using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.QCModel
{
    /// <summary>
    /// 组织与客户资金往来统计QCModel
    /// </summary>
    public class OrgAndCustomerAmountTransReportQCModel : BaseQCModel
    {
        /// <summary>
        /// 组织ID List
        /// </summary>
        public String OrgIdList { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public String CustomerID { get; set; }
        /// <summary>
        /// 销售时间开始
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 销售时间结束
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 业务类别
        /// </summary>
        public String BusinessType { get; set; }
    }
}
