using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.FM
{
    /// <summary>
    /// 应付管理QCModel
    /// </summary>
    public class AccountPayableBillManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 应付单号
        /// </summary>
        public String WHERE_APB_No { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String WHERE_APB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String WHERE_APB_SourceBillNo { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        public String WHERE_APB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String WHERE_APB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_APB_IsValid { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_APB_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_APB_Org_Name { get; set; }
        /// <summary>
        /// 收款对象类型
        /// </summary>
        public String WHERE_APB_ReceiveObjectTypeName { get; set; }
        /// <summary>
        /// 收款对象名称
        /// </summary>
        public String WHERE_APB_ReceiveObjectName { get; set; }
        /// <summary>
        /// 创建时间-开始（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeStart { get; set; }
        /// <summary>
        /// 创建时间-终了（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeEnd { get; set; }
        /// <summary>
        /// 修改时间-开始（查询条件用）
        /// </summary>
        public DateTime? _UpdatedTimeStart { get; set; }
        /// <summary>
        /// 修改时间-终了（查询条件用）
        /// </summary>
        public DateTime? _UpdatedTimeEnd { get; set; }

    }
}
