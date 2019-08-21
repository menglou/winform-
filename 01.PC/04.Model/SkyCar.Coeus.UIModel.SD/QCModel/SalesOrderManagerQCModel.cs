using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SD
{
    /// <summary>
    /// 销售订单管理QCModel
    /// </summary>
    public class SalesOrderManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 客户类型名称
        /// </summary>
        public String WHERE_SO_CustomerTypeName { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_SO_SourceTypeName { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public String WHERE_SO_CustomerName { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_SO_StatusName { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_SO_ApprovalStatusName { get; set; }
        /// <summary>
        /// 创建时间-开始（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeStart { get; set; }
        /// <summary>
        /// 创建时间-终了（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeEnd { get; set; }
        /// <summary>
        /// 单据编号
        /// </summary>
        public String WHERE_SO_No { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_SO_Org_ID { get; set; }
        /// <summary>
        /// 销售订单ID
        /// </summary>
        public String WHERE_SOD_SO_ID { get; set; }
    }
}
