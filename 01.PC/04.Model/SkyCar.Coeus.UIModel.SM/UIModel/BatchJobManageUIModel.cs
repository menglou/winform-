using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SM
{
    /// <summary>
    /// 作业管理UIModel
    /// </summary>
    public class BatchJobManageUIModel : BaseUIModel
    {
        private bool _isChecked = false;
        /// <summary>
        /// 选择
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    PropertyValueChanged = true;
                }
                _isChecked = value;
            }
        }
        /// <summary>
        /// 作业编码
        /// </summary>
        public String BJ_Code { get; set; }
        /// <summary>
        /// 作业名称
        /// </summary>
        public String BJ_Name { get; set; }
        /// <summary>
        /// 作业方式
        /// </summary>
        public String BJ_Pattern { get; set; }
        /// <summary>
        /// 消息类别
        /// </summary>
        public String BJ_PushMode { get; set; }
        /// <summary>
        /// 业务类别
        /// </summary>
        public String BJ_BusinessType { get; set; }
        /// <summary>
        /// 执行类型
        /// </summary>
        public String BJ_ExecutionType { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime? BJ_ExecuteTime { get; set; }
        /// <summary>
        /// 计划生效时间
        /// </summary>
        public DateTime? BJ_PlanStartDate { get; set; }
        /// <summary>
        /// 计划失效时间
        /// </summary>
        public DateTime? BJ_PlanEndDate { get; set; }
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
        /// 修改人
        /// </summary>
        public String BJ_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? BJ_UpdatedTime { get; set; }
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
        /// 作业分组
        /// </summary>
        public String BJ_GroupName { get; set; }
        /// <summary>
        /// 类全名
        /// </summary>
        public String BJ_FullClassName { get; set; }
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
        /// Cron表达式
        /// </summary>
        public String BJ_CronExpression { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? BJ_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String BJ_TransID { get; set; }
    }
}
