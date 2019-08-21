using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 入库单明细Model
    /// </summary>
    public class MDLPIS_StockInDetail
    {
        #region 公共属性
        /// <summary>
        /// 入库单明细ID
        /// </summary>
        public String SID_ID { get; set; }
        /// <summary>
        /// 入库单ID
        /// </summary>
        public String SID_SIB_ID { get; set; }
        /// <summary>
        /// 入库单号
        /// </summary>
        public String SID_SIB_No { get; set; }
        /// <summary>
        /// 来源单明细ID
        /// </summary>
        public String SID_SourceDetailID { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String SID_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String SID_BatchNo { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String SID_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String SID_OEMNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String SID_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String SID_Specification { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String SID_SUPP_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String SID_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String SID_WHB_ID { get; set; }
        /// <summary>
        /// 入库数量
        /// </summary>
        public Decimal? SID_Qty { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String SID_UOM { get; set; }
        /// <summary>
        /// 入库单价
        /// </summary>
        public Decimal? SID_UnitCostPrice { get; set; }
        /// <summary>
        /// 入库金额
        /// </summary>
        public Decimal? SID_Amount { get; set; }
        /// <summary>
        /// 已结算标志
        /// </summary>
        public Boolean? SID_IsSettled { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? SID_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String SID_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? SID_CreatedTime { get; set; }
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
        public String SID_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? SID_UpdatedTime { get; set; }
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
        public Int64? SID_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String SID_TransID { get; set; }
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
        /// 入库单明细ID
        /// </summary>
        public String WHERE_SID_ID { get; set; }
        /// <summary>
        /// 入库单ID
        /// </summary>
        public String WHERE_SID_SIB_ID { get; set; }
        /// <summary>
        /// 入库单号
        /// </summary>
        public String WHERE_SID_SIB_No { get; set; }
        /// <summary>
        /// 来源单明细ID
        /// </summary>
        public String WHERE_SID_SourceDetailID { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_SID_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String WHERE_SID_BatchNo { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String WHERE_SID_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String WHERE_SID_OEMNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_SID_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String WHERE_SID_Specification { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String WHERE_SID_SUPP_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WHERE_SID_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String WHERE_SID_WHB_ID { get; set; }
        /// <summary>
        /// 入库数量
        /// </summary>
        public Decimal? WHERE_SID_Qty { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String WHERE_SID_UOM { get; set; }
        /// <summary>
        /// 入库单价
        /// </summary>
        public Decimal? WHERE_SID_UnitCostPrice { get; set; }
        /// <summary>
        /// 入库金额
        /// </summary>
        public Decimal? WHERE_SID_Amount { get; set; }
        /// <summary>
        /// 已结算标志
        /// </summary>
        public Boolean? WHERE_SID_IsSettled { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SID_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_SID_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_SID_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_SID_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_SID_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_SID_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_SID_TransID { get; set; }
        #endregion

    }
}
