using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 销售预测订单明细Model
    /// </summary>
    public class MDLSD_SalesForecastOrderDetail
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
        /// 创建时间-开始（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeStart { get; set; }
        /// <summary>
        /// 创建时间-终了（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeEnd { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String SFOD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SFOD_UpdatedTime { get; set; }
        /// <summary>
        /// 修改时间-开始（查询条件用）
        /// </summary>
        public DateTime? _UpdatedTimeStart { get; set; }
        /// <summary>
        /// 修改时间-终了（查询条件用）
        /// </summary>
        public DateTime? _UpdatedTimeEnd { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? SFOD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String SFOD_TransID { get; set; }
        /// <summary>
        /// 总记录条数（翻页用）
        /// </summary>
        public Int32? RecordCount { get; set; }
        /// <summary>
        /// 当前页面索引/要跳转的页码（翻页用）
        /// </summary>
        public Int32? PageIndex { get; set; }
        /// <summary>
        /// 页面大小（翻页用）
        /// </summary>
        public Int32? PageSize { get; set; }
        #endregion

        #region 公共属性-条件用
        /// <summary>
        /// 销售预测订单明细ID
        /// </summary>
        public String WHERE_SFOD_ID { get; set; }
        /// <summary>
        /// 销售预测订单ID
        /// </summary>
        public String WHERE_SFOD_ST_ID { get; set; }
        /// <summary>
        /// 价格是否含税
        /// </summary>
        public Boolean? WHERE_SFOD_PriceIsIncludeTax { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public Decimal? WHERE_SFOD_TaxRate { get; set; }
        /// <summary>
        /// 税额
        /// </summary>
        public Decimal? WHERE_SFOD_TotalTax { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? WHERE_SFOD_Qty { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public Decimal? WHERE_SFOD_UnitPrice { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public Decimal? WHERE_SFOD_TotalAmount { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_SFOD_Barcode { get; set; }
        /// <summary>
        /// 配件批次号（汽修厂用）
        /// </summary>
        public String WHERE_SFOD_BatchNoNew { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_SFOD_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String WHERE_SFOD_Specification { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String WHERE_SFOD_UOM { get; set; }
        /// <summary>
        /// 汽修商组织ID
        /// </summary>
        public String WHERE_SFOD_AutoFactoryOrgID { get; set; }
        /// <summary>
        /// 汽修商组织名称
        /// </summary>
        public String WHERE_SFOD_AutoFactoryOrgName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_SFOD_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SFOD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_SFOD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_SFOD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_SFOD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_SFOD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_SFOD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_SFOD_TransID { get; set; }
        #endregion

    }
}
