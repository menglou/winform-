using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.QCModel
{
    /// <summary>
    /// 配件入库统计QCModel
    /// </summary>
    public class AutoPartsStockInReportQCModel : BaseQCModel
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
        /// 库存异动类型
        /// </summary>
        public String InventoryTransType { get; set; }
        /// <summary>
        /// 入库时间开始
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 入库时间结束
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
