using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.PIS
{
    /// <summary>
    /// 调拨管理QCModel
    /// </summary>
    public class TransferBillManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 单号
        /// </summary>
        public String WHERE_TB_No { get; set; }
        /// <summary>
        /// 单据类型名称
        /// </summary>
        public String WHERE_TB_TypeName { get; set; }
        /// <summary>
        /// 调拨类型名称
        /// </summary>
        public String WHERE_TB_TransferTypeName { get; set; }
        /// <summary>
        /// 调出组织名称
        /// </summary>
        public String WHERE_TB_TransferOutOrgName { get; set; }
        /// <summary>
        /// 调入组织名称
        /// </summary>
        public String WHERE_TB_TransferInOrgName { get; set; }
        /// <summary>
        /// 单据状态名称
        /// </summary>
        public String WHERE_TB_StatusName { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_TB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_TB_IsValid { get; set; }
        /// <summary>
        /// 调拨单ID
        /// </summary>
        public String WHERE_TBD_TB_ID { get; set; }
        /// <summary>
        /// 调拨单号
        /// </summary>
        public String WHERE_TBD_TB_No { get; set; }
    }
}
