using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SD
{
    /// <summary>
    /// 物流单管理中收款单UIModel
    /// </summary>
    public class ReceiptBillUIModel : BaseNotificationUIModel
    {
        /// <summary>
        /// 选择
        /// </summary>
        public Boolean IsChecked { get; set; }
        /// <summary>
        /// 收款ID
        /// </summary>
        public String RB_ID { get; set; }
        /// <summary>
        /// 收款单号
        /// </summary>
        public String RB_No { get; set; }
        /// <summary>
        /// 收款组织ID
        /// </summary>
        public String RB_Rec_Org_ID { get; set; }
        /// <summary>
        /// 收款组织名称
        /// </summary>
        public String RB_Rec_Org_Name { get; set; }
        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime? RB_Date { get; set; }
        /// <summary>
        /// 付款对象类型编码
        /// </summary>
        public String RB_PayObjectTypeCode { get; set; }
        /// <summary>
        /// 付款对象类型名称
        /// </summary>
        public String RB_PayObjectTypeName { get; set; }
        /// <summary>
        /// 付款对象ID
        /// </summary>
        public String RB_PayObjectID { get; set; }
        /// <summary>
        /// 付款对象
        /// </summary>
        public String RB_PayObjectName { get; set; }
        /// <summary>
        /// 收款通道编码
        /// </summary>
        public String RB_ReceiveTypeCode { get; set; }
        /// <summary>
        /// 收款通道名称
        /// </summary>
        public String RB_ReceiveTypeName { get; set; }
        /// <summary>
        /// 收款账号
        /// </summary>
        public String RB_ReceiveAccount { get; set; }
        /// <summary>
        /// 收款凭证编号
        /// </summary>
        public String RB_CertificateNo { get; set; }
        /// <summary>
        /// 收款款凭证图片
        /// </summary>
        public String RB_CertificatePic { get; set; }
        /// <summary>
        /// 合计金额
        /// </summary>
        public Decimal? RB_ReceiveTotalAmount { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String RB_BusinessStatusCode { get; set; }
        /// <summary>
        /// 业务状态名称
        /// </summary>
        public String RB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String RB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String RB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String RB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? RB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String RB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? RB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String RB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? RB_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? RB_VersionNo { get; set; }

        /// <summary>
        /// 临时收款单ID
        /// </summary>
        public String Tmp_RB_ID { get; set; }

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
    }
}
