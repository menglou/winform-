using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCar.Coeus.TBModel
{
    /// <summary>
    /// 系统作业Model
    /// </summary>
    public class MDLCSM_BatchJob
    {
        #region 公共属性
        /// <summary>
        /// ID
        /// </summary>
        public String BJ_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String BJ_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String BJ_Org_Name { get; set; }
        /// <summary>
        /// 作业编码
        /// </summary>
        public String BJ_Code { get; set; }
        /// <summary>
        /// 作业名称
        /// </summary>
        public String BJ_Name { get; set; }
        /// <summary>
        /// 作业分组
        /// </summary>
        public String BJ_GroupName { get; set; }
        /// <summary>
        /// 作业方式（消息推送，调度执行）
        /// </summary>
        public String BJ_Pattern { get; set; }
        /// <summary>
        /// 消息类别（PC端，APP端，微信）
        /// </summary>
        public String BJ_PushMode { get; set; }
        /// <summary>
        /// 业务类别（验车，交强险到期，商业险到期，换驾驶证，生日祝福）
        /// </summary>
        public String BJ_BusinessType { get; set; }
        /// <summary>
        /// 类全名
        /// </summary>
        public String BJ_FullClassName { get; set; }
        /// <summary>
        /// 执行类型
        /// </summary>
        public String BJ_ExecutionType { get; set; }
        /// <summary>
        /// 执行一次的时间
        /// </summary>
        public DateTime? BJ_ExecuteTime { get; set; }
        /// <summary>
        /// 执行一次的时间-开始（查询条件用）
        /// </summary>
        public DateTime? _ExecuteTimeStart { get; set; }
        /// <summary>
        /// 执行一次的时间-终了（查询条件用）
        /// </summary>
        public DateTime? _ExecuteTimeEnd { get; set; }
        /// <summary>
        /// 执行间隔
        /// </summary>
        public String BJ_ExecutionInterval { get; set; }
        /// <summary>
        /// 执行间隔值
        /// </summary>
        public Int32? BJ_ExecutionIntervalValue { get; set; }
        /// <summary>
        /// 日执行类型
        /// </summary>
        public String BJ_DayExecutionType { get; set; }
        /// <summary>
        /// 日一次执行时间
        /// </summary>
        public String BJ_DayExecutionTime { get; set; }
        /// <summary>
        /// 日执行间隔
        /// </summary>
        public String BJ_DayExecutionFrequency { get; set; }
        /// <summary>
        /// 日执行间隔值
        /// </summary>
        public Int32? BJ_DayExecutionIntervalValue { get; set; }
        /// <summary>
        /// 日执行间隔的开始时间
        /// </summary>
        public String BJ_DayExecutionStartTime { get; set; }
        /// <summary>
        /// 日执行间隔的结束时间
        /// </summary>
        public String BJ_DayExecutionEndTime { get; set; }
        /// <summary>
        /// 计划生效时间
        /// </summary>
        public DateTime? BJ_PlanStartDate { get; set; }
        /// <summary>
        /// 计划生效时间-开始（查询条件用）
        /// </summary>
        public DateTime? _PlanStartDateStart { get; set; }
        /// <summary>
        /// 计划生效时间-终了（查询条件用）
        /// </summary>
        public DateTime? _PlanStartDateEnd { get; set; }
        /// <summary>
        /// 计划失效时间
        /// </summary>
        public DateTime? BJ_PlanEndDate { get; set; }
        /// <summary>
        /// 计划失效时间-开始（查询条件用）
        /// </summary>
        public DateTime? _PlanEndDateStart { get; set; }
        /// <summary>
        /// 计划失效时间-终了（查询条件用）
        /// </summary>
        public DateTime? _PlanEndDateEnd { get; set; }
        /// <summary>
        /// Cron表达式
        /// </summary>
        public String BJ_CronExpression { get; set; }
        /// <summary>
        /// 计划说明
        /// </summary>
        public String BJ_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? BJ_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String BJ_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? BJ_CreatedTime { get; set; }
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
        public String BJ_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? BJ_UpdatedTime { get; set; }
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
        public Int64? BJ_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String BJ_TransID { get; set; }
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
        public String WHERE_BJ_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_BJ_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_BJ_Org_Name { get; set; }
        /// <summary>
        /// 作业编码
        /// </summary>
        public String WHERE_BJ_Code { get; set; }
        /// <summary>
        /// 作业名称
        /// </summary>
        public String WHERE_BJ_Name { get; set; }
        /// <summary>
        /// 作业分组
        /// </summary>
        public String WHERE_BJ_GroupName { get; set; }
        /// <summary>
        /// 作业方式（消息推送，调度执行）
        /// </summary>
        public String WHERE_BJ_Pattern { get; set; }
        /// <summary>
        /// 消息类别（PC端，APP端，微信）
        /// </summary>
        public String WHERE_BJ_PushMode { get; set; }
        /// <summary>
        /// 业务类别（验车，交强险到期，商业险到期，换驾驶证，生日祝福）
        /// </summary>
        public String WHERE_BJ_BusinessType { get; set; }
        /// <summary>
        /// 类全名
        /// </summary>
        public String WHERE_BJ_FullClassName { get; set; }
        /// <summary>
        /// 执行类型
        /// </summary>
        public String WHERE_BJ_ExecutionType { get; set; }
        /// <summary>
        /// 执行一次的时间
        /// </summary>
        public DateTime? WHERE_BJ_ExecuteTime { get; set; }
        /// <summary>
        /// 执行间隔
        /// </summary>
        public String WHERE_BJ_ExecutionInterval { get; set; }
        /// <summary>
        /// 执行间隔值
        /// </summary>
        public Int32? WHERE_BJ_ExecutionIntervalValue { get; set; }
        /// <summary>
        /// 日执行类型
        /// </summary>
        public String WHERE_BJ_DayExecutionType { get; set; }
        /// <summary>
        /// 日一次执行时间
        /// </summary>
        public String WHERE_BJ_DayExecutionTime { get; set; }
        /// <summary>
        /// 日执行间隔
        /// </summary>
        public String WHERE_BJ_DayExecutionFrequency { get; set; }
        /// <summary>
        /// 日执行间隔值
        /// </summary>
        public Int32? WHERE_BJ_DayExecutionIntervalValue { get; set; }
        /// <summary>
        /// 日执行间隔的开始时间
        /// </summary>
        public String WHERE_BJ_DayExecutionStartTime { get; set; }
        /// <summary>
        /// 日执行间隔的结束时间
        /// </summary>
        public String WHERE_BJ_DayExecutionEndTime { get; set; }
        /// <summary>
        /// 计划生效时间
        /// </summary>
        public DateTime? WHERE_BJ_PlanStartDate { get; set; }
        /// <summary>
        /// 计划失效时间
        /// </summary>
        public DateTime? WHERE_BJ_PlanEndDate { get; set; }
        /// <summary>
        /// Cron表达式
        /// </summary>
        public String WHERE_BJ_CronExpression { get; set; }
        /// <summary>
        /// 计划说明
        /// </summary>
        public String WHERE_BJ_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_BJ_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String WHERE_BJ_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? WHERE_BJ_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String WHERE_BJ_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? WHERE_BJ_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? WHERE_BJ_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String WHERE_BJ_TransID { get; set; }
        #endregion

    }
}
