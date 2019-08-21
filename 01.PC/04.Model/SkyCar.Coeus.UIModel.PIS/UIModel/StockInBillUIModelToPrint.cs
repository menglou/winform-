using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 入库管理UIModel
    /// </summary>
    public class StockInBillUIModelToPrint : BaseUIModel
    {
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String SUPP_Name { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public String SIB_No { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SIB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SIB_CreatedTime { get; set; }
        /// <summary>
        /// 总入库数量
        /// </summary>
        public decimal TotalStockInQty { get; set; }
        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal? TotalAmount { get; set; }
    }
}
