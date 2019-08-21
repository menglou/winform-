using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.UIModel
{
    /// <summary>
    /// 库存统计汇总UIModel
    /// </summary>
    public class InventoryTotalReportUIModel : BaseNotificationUIModel
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
        /// 库存数量
        /// </summary>
        public Decimal? InventoryQty { get; set; }
        /// <summary>
        /// 库存金额
        /// </summary>
        public Decimal? InventoryAmount { get; set; }

        /// <summary>
        /// 库存数量占比
        /// </summary>
        public Decimal? InventoryQtyPercent { get; set; }
        /// <summary>
        /// 库存金额占比
        /// </summary>
        public Decimal? InventoryAmountPercent { get; set; }

        /// <summary>
        /// 库存数量合计
        /// </summary>
        public Decimal? TotalInventoryQty { get; set; }
        /// <summary>
        /// 库存金额合计
        /// </summary>
        public Decimal? TotalInventoryAmount { get; set; }
        
    }
}
