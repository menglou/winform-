using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.QCModel
{
    /// <summary>
    /// 经营情况统计QCModel
    /// </summary>
    public class ManagementSituationReportQCModel : BaseQCModel
    {
        /// <summary>
        /// 组织ID List
        /// </summary>
        public String OrgIdList { get; set; }
        /// <summary>
        /// 统计时间开始
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 统计时间结束
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 统计方式
        /// </summary>
        public String StatisticsMode { get; set; }
        /// <summary>
        /// 是否包含退货
        /// </summary>
        public bool IsContainReturn { get; set; }
    }
}
