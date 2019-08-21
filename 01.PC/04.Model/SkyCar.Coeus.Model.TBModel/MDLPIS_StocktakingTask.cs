using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 盘点任务Model
    /// </summary>
    public class MDLPIS_StocktakingTask
    {
        #region 公共属性
        /// <summary>
        /// 盘点任务ID
        /// </summary>
        public String ST_ID { get; set; }
        /// <summary>
        /// 盘点单号
        /// </summary>
        public String ST_No { get; set; }
        /// <summary>
        /// 盘点次数
        /// </summary>
        public Int32? ST_CheckAmount { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String ST_Org_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String ST_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String ST_WHB_ID { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? ST_StartTime { get; set; }
        /// <summary>
        /// 开始时间-开始（查询条件用）
        /// </summary>
        public DateTime? _StartTimeStart { get; set; }
        /// <summary>
        /// 开始时间-终了（查询条件用）
        /// </summary>
        public DateTime? _StartTimeEnd { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? ST_EndTime { get; set; }
        /// <summary>
        /// 结束时间-开始（查询条件用）
        /// </summary>
        public DateTime? _EndTimeStart { get; set; }
        /// <summary>
        /// 结束时间-终了（查询条件用）
        /// </summary>
        public DateTime? _EndTimeEnd { get; set; }
        /// <summary>
        /// 显示成本
        /// </summary>
        public Boolean? ST_IsShowCost { get; set; }
        /// <summary>
        /// 应有库存量
        /// </summary>
        public Decimal? ST_DueQty { get; set; }
        /// <summary>
        /// 实际库存量
        /// </summary>
        public Decimal? ST_ActualQty { get; set; }
        /// <summary>
        /// 数量损失率
        /// </summary>
        public Decimal? ST_QtyLossRatio { get; set; }
        /// <summary>
        /// 应有库存金额
        /// </summary>
        public Decimal? ST_DueAmount { get; set; }
        /// <summary>
        /// 实际库存金额
        /// </summary>
        public Decimal? ST_ActualAmount { get; set; }
        /// <summary>
        /// 金额损失率
        /// </summary>
        public Decimal? ST_AmountLossRatio { get; set; }
        /// <summary>
        /// 盘点结果编码
        /// </summary>
        public String ST_CheckResultCode { get; set; }
        /// <summary>
        /// 盘点结果名称
        /// </summary>
        public String ST_CheckResultName { get; set; }
        /// <summary>
        /// 盘点单状态编码
        /// </summary>
        public String ST_StatusCode { get; set; }
        /// <summary>
        /// 盘点单状态名称
        /// </summary>
        public String ST_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String ST_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String ST_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String ST_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? ST_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String ST_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ST_CreatedTime { get; set; }
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
        public String ST_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ST_UpdatedTime { get; set; }
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
        public Int64? ST_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String ST_TransID { get; set; }
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
        /// 盘点任务ID
        /// </summary>
        public String WHERE_ST_ID { get; set; }
        /// <summary>
        /// 盘点单号
        /// </summary>
        public String WHERE_ST_No { get; set; }
        /// <summary>
        /// 盘点次数
        /// </summary>
        public Int32? WHERE_ST_CheckAmount { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_ST_Org_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WHERE_ST_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String WHERE_ST_WHB_ID { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? WHERE_ST_StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? WHERE_ST_EndTime { get; set; }
        /// <summary>
        /// 显示成本
        /// </summary>
        public Boolean? WHERE_ST_IsShowCost { get; set; }
        /// <summary>
        /// 应有库存量
        /// </summary>
        public Decimal? WHERE_ST_DueQty { get; set; }
        /// <summary>
        /// 实际库存量
        /// </summary>
        public Decimal? WHERE_ST_ActualQty { get; set; }
        /// <summary>
        /// 数量损失率
        /// </summary>
        public Decimal? WHERE_ST_QtyLossRatio { get; set; }
        /// <summary>
        /// 应有库存金额
        /// </summary>
        public Decimal? WHERE_ST_DueAmount { get; set; }
        /// <summary>
        /// 实际库存金额
        /// </summary>
        public Decimal? WHERE_ST_ActualAmount { get; set; }
        /// <summary>
        /// 金额损失率
        /// </summary>
        public Decimal? WHERE_ST_AmountLossRatio { get; set; }
        /// <summary>
        /// 盘点结果编码
        /// </summary>
        public String WHERE_ST_CheckResultCode { get; set; }
        /// <summary>
        /// 盘点结果名称
        /// </summary>
        public String WHERE_ST_CheckResultName { get; set; }
        /// <summary>
        /// 盘点单状态编码
        /// </summary>
        public String WHERE_ST_StatusCode { get; set; }
        /// <summary>
        /// 盘点单状态名称
        /// </summary>
        public String WHERE_ST_StatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String WHERE_ST_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_ST_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String WHERE_ST_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_ST_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_ST_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_ST_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_ST_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_ST_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_ST_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_ST_TransID { get; set; }
        #endregion

    }
}
