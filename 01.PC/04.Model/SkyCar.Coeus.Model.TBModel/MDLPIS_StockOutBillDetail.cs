using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 出库单明细Model
    /// </summary>
    public class MDLPIS_StockOutBillDetail
    {
        #region 公共属性
        /// <summary>
        /// 出库单明细ID
        /// </summary>
        public String SOBD_ID { get; set; }
        /// <summary>
        /// 出库单ID
        /// </summary>
        public String SOBD_SOB_ID { get; set; }
        /// <summary>
        /// 出库单号
        /// </summary>
        public String SOBD_SOB_No { get; set; }
        /// <summary>
        /// 来源单明细ID
        /// </summary>
        public String SOBD_SourceDetailID { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String SOBD_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String SOBD_BatchNo { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String SOBD_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String SOBD_OEMNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String SOBD_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String SOBD_Specification { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String SOBD_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String SOBD_WHB_ID { get; set; }
        /// <summary>
        /// 进货单价
        /// </summary>
        public Decimal? SOBD_UnitCostPrice { get; set; }
        /// <summary>
        /// 出库数量
        /// </summary>
        public Decimal? SOBD_Qty { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String SOBD_UOM { get; set; }
        /// <summary>
        /// 销售单价
        /// </summary>
        public Decimal? SOBD_UnitSalePrice { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public Decimal? SOBD_Amount { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SOBD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SOBD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SOBD_CreatedTime { get; set; }
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
        public String SOBD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SOBD_UpdatedTime { get; set; }
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
        public Int64? SOBD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String SOBD_TransID { get; set; }
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
        /// 出库单明细ID
        /// </summary>
        public String WHERE_SOBD_ID { get; set; }
        /// <summary>
        /// 出库单ID
        /// </summary>
        public String WHERE_SOBD_SOB_ID { get; set; }
        /// <summary>
        /// 出库单号
        /// </summary>
        public String WHERE_SOBD_SOB_No { get; set; }
        /// <summary>
        /// 来源单明细ID
        /// </summary>
        public String WHERE_SOBD_SourceDetailID { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_SOBD_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String WHERE_SOBD_BatchNo { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String WHERE_SOBD_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String WHERE_SOBD_OEMNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_SOBD_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String WHERE_SOBD_Specification { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WHERE_SOBD_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String WHERE_SOBD_WHB_ID { get; set; }
        /// <summary>
        /// 进货单价
        /// </summary>
        public Decimal? WHERE_SOBD_UnitCostPrice { get; set; }
        /// <summary>
        /// 出库数量
        /// </summary>
        public Decimal? WHERE_SOBD_Qty { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String WHERE_SOBD_UOM { get; set; }
        /// <summary>
        /// 销售单价
        /// </summary>
        public Decimal? WHERE_SOBD_UnitSalePrice { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public Decimal? WHERE_SOBD_Amount { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SOBD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_SOBD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_SOBD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_SOBD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_SOBD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_SOBD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_SOBD_TransID { get; set; }
        #endregion

    }
}
