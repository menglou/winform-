using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.FM
{
    /// <summary>
    /// 收款单管理QCModel
    /// </summary>
    public class ReceiptBillManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 收款单号
        /// </summary>
        public String WHERE_RB_No { get; set; }
        /// <summary>
        /// 付款对象类型
        /// </summary>
        public String WHERE_RB_PayObjectTypeName { get; set; }
        /// <summary>
        /// 付款对象
        /// </summary>
        public String WHERE_RB_PayObjectName { get; set; }
        /// <summary>
        /// 收款通道
        /// </summary>
        public String WHERE_RB_ReceiveTypeName { get; set; }
        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime? WHERE_RB_Date { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        public String WHERE_RB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String WHERE_RB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_RB_IsValid { get; set; }
        /// <summary>
        /// 收款组织
        /// </summary>
        public String WHERE_RB_Rec_Org_ID { get; set; }
        /// <summary>
        /// 收款组织名称
        /// </summary>
        public String WHERE_RB_Rec_Org_Name { get; set; }
        /// <summary>
        /// 收款日期-开始（查询条件用）
        /// </summary>
        public DateTime? _DateStart { get; set; }
        /// <summary>
        /// 收款日期-终了（查询条件用）
        /// </summary>
        public DateTime? _DateEnd { get; set; }
        /// <summary>
        /// 收款明细来源单号
        /// </summary>
        public String WHERE_RBD_SrcBillNo { get; set; }
    }
}
