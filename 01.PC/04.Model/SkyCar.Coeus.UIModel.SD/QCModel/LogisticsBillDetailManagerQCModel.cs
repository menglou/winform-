using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SD
{
    /// <summary>
    /// 物流单管理QCModel
    /// </summary>
    public class LogisticsBillDetailManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 物流单号
        /// </summary>
        public String WHERE_LB_No { get; set; }
        /// <summary>
        /// 物流单来源单号
        /// </summary>
        public String WHERE_LB_SourceNo { get; set; }
        /// <summary>
        /// 收件人姓名
        /// </summary>
        public String WHERE_LB_Receiver { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_LB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String WHERE_LB_SourceTypeName { get; set; }
        /// <summary>
        /// 物流费付款状态
        /// </summary>
        public String WHERE_LB_PayStautsName { get; set; }
        /// <summary>
        /// 单据状态
        /// </summary>
        public String WHERE_LB_StatusName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_LB_IsValid { get; set; }
    }
}
