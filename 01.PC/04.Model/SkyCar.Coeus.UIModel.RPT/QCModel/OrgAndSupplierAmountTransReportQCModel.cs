using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.QCModel
{
    /// <summary>
    /// 组织与供应商资金往来统计QCModel
    /// </summary>
    public class OrgAndSupplierAmountTransReportQCModel : BaseQCModel
    {
        /// <summary>
        /// 组织ID List
        /// </summary>
        public String OrgIdList { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String SupplierID { get; set; }
        /// <summary>
        /// 出/入库时间开始
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 出/入库时间结束
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 业务类别
        /// </summary>
        public String BusinessType { get; set; }
    }
}
