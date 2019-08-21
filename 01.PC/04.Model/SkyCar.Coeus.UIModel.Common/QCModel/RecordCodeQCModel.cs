using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 生成Venus单据编号QCModel
    /// </summary>
    public class RecordCodeQCModel
    {
        /// <summary>
        /// 组织编码
        /// </summary>
        public string OrgCode { get; set; }

        /// <summary>
        /// 表名缩写（单据类型）
        /// </summary>
        public string TableAbridge { get; set; }

        /// <summary>
        /// 业务时间
        /// </summary>
        public DateTime? BusinessTime { get; set; }
    }
}
