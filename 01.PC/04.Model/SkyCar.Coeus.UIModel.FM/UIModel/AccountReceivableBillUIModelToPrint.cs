using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.FM
{
    /// <summary>
    /// 应收管理UIModel
    /// </summary>
    public class AccountReceivableBillUIModelToPrint : BaseUIModel
    {
        /// <summary>
        /// 选择
        /// </summary>
        public bool IsChecked { get; set; }
        /// <summary>
        /// 应收单号
        /// </summary>
        public String ARB_No { get; set; }
        /// <summary>
        /// 单据方向
        /// </summary>
        public String ARB_BillDirectName { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String ARB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String ARB_SrcBillNo { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String ARB_Org_Name { get; set; }
        /// <summary>
        /// 赔偿人员类型
        /// </summary>
        public String ARB_IndemnitorTypeName { get; set; }
        /// <summary>
        /// 赔偿人员
        /// </summary>
        public String ARB_IndemnitorName { get; set; }
        /// <summary>
        /// 赔偿人员手机号
        /// </summary>
        public String ARB_IndemnitorPhone { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? ARB_AccountReceivableAmount { get; set; }
        /// <summary>
        /// 已收金额
        /// </summary>
        public Decimal? ARB_ReceivedAmount { get; set; }
        /// <summary>
        /// 未收金额
        /// </summary>
        public Decimal? ARB_UnReceiveAmount { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        public String ARB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String ARB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String ARB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? ARB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String ARB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? ARB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String ARB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ARB_UpdatedTime { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String ARB_BusinessStatusCode { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String ARB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 赔偿人员ID
        /// </summary>
        public String ARB_IndemnitorID { get; set; }
        /// <summary>
        /// 应收单ID
        /// </summary>
        public String ARB_ID { get; set; }
        /// <summary>
        /// 单据方向编码
        /// </summary>
        public String ARB_BillDirectCode { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String ARB_SourceTypeCode { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String ARB_Org_ID { get; set; }
        /// <summary>
        /// 赔偿人员类型编码
        /// </summary>
        public String ARB_IndemnitorTypeCode { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? ARB_VersionNo { get; set; }
        /// <summary>
        /// 事务编号
        /// </summary>
        public String ARB_TransID { get; set; }
        
        /// <summary>
        /// 销售订单总金额
        /// </summary>
        public Decimal? SO_TotalAmount { get; set; }
    }
}
