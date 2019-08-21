using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 系统作业日志Model
    /// </summary>
    public class MDLCSM_BatchJobLog
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String BJL_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String BJL_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String BJL_Org_Name { get; set; }
        /// <summary>
        /// 作业编码
        /// </summary>
        public String BJL_Code { get; set; }
        /// <summary>
        /// 作业名称
        /// </summary>
        public String BJL_Name { get; set; }
        /// <summary>
        /// 作业方式（消息推送，调度执行）
        /// </summary>
        public String BJL_Pattern { get; set; }
        /// <summary>
        /// 推送方式（PC端，APP端，微信）
        /// </summary>
        public String BJL_PushMode { get; set; }
        /// <summary>
        /// 执行开始时间
        /// </summary>
        public DateTime? BJL_ExectueStartDate { get; set; }
        /// <summary>
        /// 执行开始时间-开始（查询条件用）
        /// </summary>
        public DateTime? _ExectueStartDateStart { get; set; }
        /// <summary>
        /// 执行开始时间-终了（查询条件用）
        /// </summary>
        public DateTime? _ExectueStartDateEnd { get; set; }
        /// <summary>
        /// 执行结束时间
        /// </summary>
        public DateTime? BJL_ExectueEndDate { get; set; }
        /// <summary>
        /// 执行结束时间-开始（查询条件用）
        /// </summary>
        public DateTime? _ExectueEndDateStart { get; set; }
        /// <summary>
        /// 执行结束时间-终了（查询条件用）
        /// </summary>
        public DateTime? _ExectueEndDateEnd { get; set; }
        /// <summary>
        /// 启动方式（自动，手动）
        /// </summary>
        public String BJL_StartMode { get; set; }
        /// <summary>
        /// 日志明细
        /// </summary>
        public String BJL_Details { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? BJL_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String BJL_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? BJL_CreatedTime { get; set; }
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
        public String BJL_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? BJL_UpdatedTime { get; set; }
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
        public Int64? BJL_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String BJL_TransID { get; set; }
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
        /// ID
        /// </summary>
        public String WHERE_BJL_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_BJL_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_BJL_Org_Name { get; set; }
        /// <summary>
        /// 作业编码
        /// </summary>
        public String WHERE_BJL_Code { get; set; }
        /// <summary>
        /// 作业名称
        /// </summary>
        public String WHERE_BJL_Name { get; set; }
        /// <summary>
        /// 作业方式（消息推送，调度执行）
        /// </summary>
        public String WHERE_BJL_Pattern { get; set; }
        /// <summary>
        /// 推送方式（PC端，APP端，微信）
        /// </summary>
        public String WHERE_BJL_PushMode { get; set; }
        /// <summary>
        /// 执行开始时间
        /// </summary>
        public DateTime? WHERE_BJL_ExectueStartDate { get; set; }
        /// <summary>
        /// 执行结束时间
        /// </summary>
        public DateTime? WHERE_BJL_ExectueEndDate { get; set; }
        /// <summary>
        /// 启动方式（自动，手动）
        /// </summary>
        public String WHERE_BJL_StartMode { get; set; }
        /// <summary>
        /// 日志明细
        /// </summary>
        public String WHERE_BJL_Details { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_BJL_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_BJL_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_BJL_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_BJL_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_BJL_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_BJL_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_BJL_TransID { get; set; }
        #endregion

    }
}
