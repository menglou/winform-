using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.UIModel
{
    /// <summary>
    /// 经营情况统计汇总UIModel
    /// </summary>
    public class ManagementSituationTotalReportUIModel : BaseNotificationUIModel
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
        /// 统计类别
        /// </summary>
        public String StatisticsType { get; set; }
        /// <summary>
        /// 统计金额
        /// </summary>
        public Decimal? StatisticsAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }
    }
}
