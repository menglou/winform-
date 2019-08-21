using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.QCModel
{
    /// <summary>
    /// 库存统计QCModel
    /// </summary>
    public class InventoryReportQCModel : BaseQCModel
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public String OrgID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WarehouseID { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WarehouseName { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String AutoPartsName { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String AutoPartsBrand { get; set; }
        /// <summary>
        /// 入库时间开始
        /// </summary>
        public DateTime? StockInTimeStart { get; set; }
        /// <summary>
        /// 入库时间结束
        /// </summary>
        public DateTime? StockInTimeEnd { get; set; }
    }
}
