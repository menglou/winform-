using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 入库管理QCModel
    /// </summary>
    public class StockInBillManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_SIB_Org_ID { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public String WHERE_SIB_No { get; set; }
        /// <summary>
        /// 来源类型名称
        /// </summary>
        public String WHERE_SIB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String WHERE_SIB_SourceNo { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_SIB_StatusName { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_SIB_ApprovalStatusName { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public String WHERE_SIB_SUPP_ID { get; set; }

        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SIB_IsValid { get; set; }

        /// <summary>
        /// 入库单ID
        /// </summary>
        public String WHERE_SID_SIB_ID { get; set; }
        /// <summary>
        /// 入库单号
        /// </summary>
        public String WHERE_SID_SIB_No { get; set; }
        /// <summary>
        /// 创建时间-开始（查询条件用）
        /// </summary>
        public DateTime? _DateStart { get; set; }
        /// <summary>
        /// 创建时间-终了（查询条件用）
        /// </summary>
        public DateTime? _DateEnd { get; set; }
    }
}
