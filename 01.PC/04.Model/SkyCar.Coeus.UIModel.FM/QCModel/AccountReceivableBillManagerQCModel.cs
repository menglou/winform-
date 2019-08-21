using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.FM
{
    /// <summary>
    /// 应收管理QCModel
    /// </summary>
    public class AccountReceivableBillManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 应收单号
        /// </summary>
        public String WHERE_ARB_No { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String WHERE_ARB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String WHERE_ARB_SrcBillNo { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        public String WHERE_ARB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String WHERE_ARB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_ARB_IsValid { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_ARB_Org_ID { get; set; }
        /// <summary>
        /// 应收单ID
        /// </summary>
        public String WHERE_ARBD_ARB_ID { get; set; }
        /// <summary>
        /// 收款对象类型
        /// </summary>
        public String WHERE_ARB_PayObjectTypeName { get; set; }
        /// <summary>
        /// 收款对象名称
        /// </summary>
        public String WHERE_ARB_PayObjectName { get; set; }
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
        
        /// <summary>
        /// 汽修商户编码
        /// </summary>
        public String WHERE_AutoFactoryCode { get; set; }
    }
}
