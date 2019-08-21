using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 盘点任务明细Model
    /// </summary>
    public class MDLPIS_StocktakingTaskDetail
    {
        #region 公共属性
        /// <summary>
        /// 盘点任务明细ID
        /// </summary>
        public String STD_ID { get; set; }
        /// <summary>
        /// 盘点任务ID
        /// </summary>
        public String STD_TB_ID { get; set; }
        /// <summary>
        /// 盘点单号
        /// </summary>
        public String STD_TB_No { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String STD_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String STD_WHB_ID { get; set; }
        /// <summary>
        /// 应有量
        /// </summary>
        public Decimal? STD_DueQty { get; set; }
        /// <summary>
        /// 实际量
        /// </summary>
        public Decimal? STD_ActualQty { get; set; }
        /// <summary>
        /// 差异数量
        /// </summary>
        public Decimal? STD_AdjustQty { get; set; }
        /// <summary>
        /// 允差数量
        /// </summary>
        public Decimal? STD_ApprDiffQty { get; set; }
        /// <summary>
        /// 数量允差比
        /// </summary>
        public Decimal? STD_ApprDiffQtyRate { get; set; }
        /// <summary>
        /// 调整数量
        /// </summary>
        public Decimal? STD_SnapshotQty { get; set; }
        /// <summary>
        /// 应有金额
        /// </summary>
        public Decimal? STD_DueAmount { get; set; }
        /// <summary>
        /// 实际金额
        /// </summary>
        public Decimal? STD_ActualAmount { get; set; }
        /// <summary>
        /// 金额损失率
        /// </summary>
        public Decimal? STD_AmountLossRatio { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String STD_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String STD_BatchNo { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String STD_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String STD_OEMNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String STD_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String STD_Specification { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String STD_UOM { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? STD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String STD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? STD_CreatedTime { get; set; }
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
        public String STD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? STD_UpdatedTime { get; set; }
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
        public Int64? STD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String STD_TransID { get; set; }
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
        /// 盘点任务明细ID
        /// </summary>
        public String WHERE_STD_ID { get; set; }
        /// <summary>
        /// 盘点任务ID
        /// </summary>
        public String WHERE_STD_TB_ID { get; set; }
        /// <summary>
        /// 盘点单号
        /// </summary>
        public String WHERE_STD_TB_No { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WHERE_STD_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String WHERE_STD_WHB_ID { get; set; }
        /// <summary>
        /// 应有量
        /// </summary>
        public Decimal? WHERE_STD_DueQty { get; set; }
        /// <summary>
        /// 实际量
        /// </summary>
        public Decimal? WHERE_STD_ActualQty { get; set; }
        /// <summary>
        /// 差异数量
        /// </summary>
        public Decimal? WHERE_STD_AdjustQty { get; set; }
        /// <summary>
        /// 允差数量
        /// </summary>
        public Decimal? WHERE_STD_ApprDiffQty { get; set; }
        /// <summary>
        /// 数量允差比
        /// </summary>
        public Decimal? WHERE_STD_ApprDiffQtyRate { get; set; }
        /// <summary>
        /// 调整数量
        /// </summary>
        public Decimal? WHERE_STD_SnapshotQty { get; set; }
        /// <summary>
        /// 应有金额
        /// </summary>
        public Decimal? WHERE_STD_DueAmount { get; set; }
        /// <summary>
        /// 实际金额
        /// </summary>
        public Decimal? WHERE_STD_ActualAmount { get; set; }
        /// <summary>
        /// 金额损失率
        /// </summary>
        public Decimal? WHERE_STD_AmountLossRatio { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_STD_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String WHERE_STD_BatchNo { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String WHERE_STD_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String WHERE_STD_OEMNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_STD_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String WHERE_STD_Specification { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String WHERE_STD_UOM { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_STD_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_STD_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_STD_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_STD_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_STD_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_STD_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_STD_TransID { get; set; }
        #endregion

    }
}
