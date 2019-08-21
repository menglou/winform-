using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.QCModel
{
    /// <summary>
    /// 组织与组织资金往来统计QCModel
    /// </summary>
    public class OrgAndOrgAmountTransReportQCModel : BaseQCModel
    {
        /// <summary>
        /// 源组织ID List
        /// </summary>
        public String SourOrgIdList { get; set; }
        /// <summary>
        /// 源组织ID
        /// </summary>
        public String SourOrgId { get; set; }
        /// <summary>
        /// 目的组织ID List
        /// </summary>
        public String DestOrgIdList { get; set; }
        /// <summary>
        /// 目的组织ID
        /// </summary>
        public String DestOrgId { get; set; }
        /// <summary>
        /// 时间开始
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 时间结束
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
