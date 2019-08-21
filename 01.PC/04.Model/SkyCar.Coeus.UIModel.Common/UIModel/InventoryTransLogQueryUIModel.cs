using System;

namespace SkyCar.Coeus.UIModel.Common.UIModel
{
    /// <summary>
    /// 库存异动日志查询UIModel
    /// </summary>
    public class InventoryTransLogQueryUIModel : BaseUIModel
    {
        #region 库存异动日志
        /// <summary>
        /// 库存异动日志ID
        /// </summary>
        public String ITL_ID { get; set; }
        /// <summary>
        /// 异动类型
        /// </summary>
        public String ITL_TransType { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String ITL_Org_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String ITL_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String ITL_WHB_ID { get; set; }
        /// <summary>
        /// 业务单号
        /// </summary>
        public String ITL_BusinessNo { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String ITL_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String ITL_BatchNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String ITL_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String ITL_Specification { get; set; }
        /// <summary>
        /// 单位成本
        /// </summary>
        public Decimal? ITL_UnitCostPrice { get; set; }
        /// <summary>
        /// 单位销价
        /// </summary>
        public Decimal? ITL_UnitSalePrice { get; set; }
        /// <summary>
        /// 异动数量
        /// </summary>
        public Decimal? ITL_Qty { get; set; }
        /// <summary>
        /// 异动后库存数量
        /// </summary>
        public Decimal? ITL_AfterTransQty { get; set; }
        /// <summary>
        /// 出发地
        /// </summary>
        public String ITL_Source { get; set; }
        /// <summary>
        /// 目的地
        /// </summary>
        public String ITL_Destination { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? ITL_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String ITL_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ITL_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String ITL_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ITL_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? ITL_VersionNo { get; set; }
        #endregion

        #region 其他属性

        #endregion
    }
}
