using System;

namespace SkyCar.Coeus.UIModel.SD.UIModel
{
    /// <summary>
    /// 销售预测订单明细UIModel
    /// </summary>
    public class SalesForecastOrderDetailUIModel
    {
        #region 公共属性
        /// <summary>
        /// 销售预测订单明细ID
        /// </summary>
        public String SFOD_ID { get; set; }
        /// <summary>
        /// 销售预测订单ID
        /// </summary>
        public String SFOD_ST_ID { get; set; }
        /// <summary>
        /// 价格是否含税
        /// </summary>
        public Boolean? SFOD_PriceIsIncludeTax { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public Decimal? SFOD_TaxRate { get; set; }
        /// <summary>
        /// 税额
        /// </summary>
        public Decimal? SFOD_TotalTax { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? SFOD_Qty { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public Decimal? SFOD_UnitPrice { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public Decimal? SFOD_TotalAmount { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String SFOD_Barcode { get; set; }
        /// <summary>
        /// 配件批次号（汽修厂用）
        /// </summary>
        public String SFOD_BatchNoNew { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String SFOD_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String SFOD_Specification { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String SFOD_UOM { get; set; }
        /// <summary>
        /// 汽修商组织ID
        /// </summary>
        public String SFOD_AutoFactoryOrgID { get; set; }
        /// <summary>
        /// 汽修商组织名称
        /// </summary>
        public String SFOD_AutoFactoryOrgName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String SFOD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SFOD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SFOD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SFOD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String SFOD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SFOD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SFOD_VersionNo { get; set; }
        #endregion
    }
}
