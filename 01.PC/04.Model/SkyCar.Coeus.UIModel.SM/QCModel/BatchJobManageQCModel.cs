using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SM
{
    /// <summary>
    /// 作业管理QCModel
    /// </summary>
    public class BatchJobManageQCModel : BaseQCModel
    {
        /// <summary>
        /// 作业编码
        /// </summary>
        public String WHERE_BJ_Code { get; set; }
        /// <summary>
        /// 作业名称
        /// </summary>
        public String WHERE_BJ_Name { get; set; }
        /// <summary>
        /// 作业方式
        /// </summary>
        public String WHERE_BJ_Pattern { get; set; }
        /// <summary>
        /// 消息类别
        /// </summary>
        public String WHERE_BJ_PushMode { get; set; }
        /// <summary>
        /// 业务类别
        /// </summary>
        public String WHERE_BJ_BusinessType { get; set; }
        /// <summary>
        /// 执行类型
        /// </summary>
        public String WHERE_BJ_ExecutionType { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime? WHERE_BJ_ExecuteTime { get; set; }
    }
}
