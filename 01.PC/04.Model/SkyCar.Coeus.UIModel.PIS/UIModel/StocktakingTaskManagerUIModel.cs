using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 盘点管理UIModel
    /// </summary>
    public class StocktakingTaskManagerUIModel : BaseUIModel
    {
        /// <summary>
        /// 选择
        /// </summary>
        public bool IsChecked { get; set; }
        /// <summary>
        /// 盘点单号
        /// </summary>
        public String ST_No { get; set; }
        /// <summary>
        /// 盘点次数
        /// </summary>
        public Int32? ST_CheckAmount { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? ST_StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? ST_EndTime { get; set; }
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
        /// 盘点结果
        /// </summary>
        public String ST_CheckResultName { get; set; }
        /// <summary>
        /// 盘点单状态
        /// </summary>
        public String ST_StatusName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String ST_ApprovalStatusName { get; set; }
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
        /// 修改人
        /// </summary>
        public String ST_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ST_UpdatedTime { get; set; }
        /// <summary>
        /// 盘点任务ID
        /// </summary>
        public String ST_ID { get; set; }
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
        /// 盘点结果编码
        /// </summary>
        public String ST_CheckResultCode { get; set; }
        /// <summary>
        /// 盘点单状态编码
        /// </summary>
        public String ST_StatusCode { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String ST_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? ST_VersionNo { get; set; }
        /// <summary>
        /// 组织简称
        /// </summary>
        public String Org_ShortName { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WH_Name { get; set; }
        /// <summary>
        /// 仓位名称
        /// </summary>
        public String WHB_Name { get; set; }

    }
}
