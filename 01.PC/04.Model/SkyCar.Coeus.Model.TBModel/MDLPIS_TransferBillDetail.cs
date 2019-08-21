using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 调拨单明细Model
    /// </summary>
    public class MDLPIS_TransferBillDetail
    {
        #region 公共属性
        /// <summary>
        /// 调拨单明细ID
        /// </summary>
        public String TBD_ID { get; set; }
        /// <summary>
        /// 调拨单ID
        /// </summary>
        public String TBD_TB_ID { get; set; }
        /// <summary>
        /// 调拨单号
        /// </summary>
        public String TBD_TB_No { get; set; }
        /// <summary>
        /// 调出仓库ID
        /// </summary>
        public String TBD_TransOutWhId { get; set; }
        /// <summary>
        /// 调出仓位ID
        /// </summary>
        public String TBD_TransOutBinId { get; set; }
        /// <summary>
        /// 调入仓库ID
        /// </summary>
        public String TBD_TransInWhId { get; set; }
        /// <summary>
        /// 调入仓位ID
        /// </summary>
        public String TBD_TransInBinId { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String TBD_Barcode { get; set; }
        /// <summary>
        /// 调出配件批次号
        /// </summary>
        public String TBD_TransOutBatchNo { get; set; }
        /// <summary>
        /// 调入配件批次号
        /// </summary>
        public String TBD_TransInBatchNo { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String TBD_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String TBD_OEMNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String TBD_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String TBD_Specification { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String TBD_SUPP_ID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? TBD_Qty { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String TBD_UOM { get; set; }
        /// <summary>
        /// 源库存单价
        /// </summary>
        public Decimal? TBD_SourUnitPrice { get; set; }
        /// <summary>
        /// 入库单价
        /// </summary>
        public Decimal? TBD_DestUnitPrice { get; set; }
        /// <summary>
        /// 已结算标志
        /// </summary>
        public String TBD_IsSettled { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? TBD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String TBD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? TBD_CreatedTime { get; set; }
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
        public String TBD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? TBD_UpdatedTime { get; set; }
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
        public Int64? TBD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String TBD_TransID { get; set; }
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
        /// 调拨单明细ID
        /// </summary>
        public String WHERE_TBD_ID { get; set; }
        /// <summary>
        /// 调拨单ID
        /// </summary>
        public String WHERE_TBD_TB_ID { get; set; }
        /// <summary>
        /// 调拨单号
        /// </summary>
        public String WHERE_TBD_TB_No { get; set; }
        /// <summary>
        /// 调出仓库ID
        /// </summary>
        public String WHERE_TBD_TransOutWhId { get; set; }
        /// <summary>
        /// 调出仓位ID
        /// </summary>
        public String WHERE_TBD_TransOutBinId { get; set; }
        /// <summary>
        /// 调入仓库ID
        /// </summary>
        public String WHERE_TBD_TransInWhId { get; set; }
        /// <summary>
        /// 调入仓位ID
        /// </summary>
        public String WHERE_TBD_TransInBinId { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_TBD_Barcode { get; set; }
        /// <summary>
        /// 调出配件批次号
        /// </summary>
        public String WHERE_TBD_TransOutBatchNo { get; set; }
        /// <summary>
        /// 调入配件批次号
        /// </summary>
        public String WHERE_TBD_TransInBatchNo { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String WHERE_TBD_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String WHERE_TBD_OEMNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_TBD_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String WHERE_TBD_Specification { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String WHERE_TBD_SUPP_ID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? WHERE_TBD_Qty { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String WHERE_TBD_UOM { get; set; }
        /// <summary>
        /// 源库存单价
        /// </summary>
        public Decimal? WHERE_TBD_SourUnitPrice { get; set; }
        /// <summary>
        /// 入库单价
        /// </summary>
        public Decimal? WHERE_TBD_DestUnitPrice { get; set; }
        /// <summary>
        /// 已结算标志
        /// </summary>
        public String WHERE_TBD_IsSettled { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_TBD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_TBD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_TBD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_TBD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_TBD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_TBD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_TBD_TransID { get; set; }
        #endregion

    }
}
