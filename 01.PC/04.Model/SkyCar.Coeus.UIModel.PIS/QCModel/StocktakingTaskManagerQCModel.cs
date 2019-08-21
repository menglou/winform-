using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 盘点管理QCModel
    /// </summary>
    public class StocktakingTaskManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_ST_Org_ID { get; set; }
        /// <summary>
        /// 盘点单号
        /// </summary>
        public String WHERE_ST_No { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? WHERE_ST_StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? WHERE_ST_EndTime { get; set; }
        /// <summary>
        /// 盘点结果名称
        /// </summary>
        public String WHERE_ST_CheckResultName { get; set; }
        /// <summary>
        /// 盘点单状态名称
        /// </summary>
        public String WHERE_ST_StatusName { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_ST_ApprovalStatusName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_ST_IsValid { get; set; }
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
        public String WHERE_ST_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String WHERE_ST_WHB_ID { get; set; }
        /// <summary>
        /// 开始时间-开始（查询条件用）
        /// </summary>
        public DateTime? _StartTimeStart { get; set; }
        /// <summary>
        /// 开始时间-终了（查询条件用）
        /// </summary>
        public DateTime? _StartTimeEnd { get; set; }
        /// <summary>
        /// 结束时间-开始（查询条件用）
        /// </summary>
        public DateTime? _EndTimeStart { get; set; }
        /// <summary>
        /// 结束时间-终了（查询条件用）
        /// </summary>
        public DateTime? _EndTimeEnd { get; set; }

        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_STD_Name { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_STD_Barcode { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String STD_CreatedBy { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String STD_UpdatedBy { get; set; }
    }
}
