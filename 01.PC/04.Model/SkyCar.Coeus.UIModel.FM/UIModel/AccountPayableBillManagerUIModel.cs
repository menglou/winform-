using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.FM
{
    /// <summary>
    /// 应付管理UIModel
    /// </summary>
    public class AccountPayableBillManagerUIModel : BaseUIModel
    {
        #region 应付单

        /// <summary>
        /// 选择
        /// </summary>
        public bool IsChecked { get; set; }
        /// <summary>
        /// 应付单号
        /// </summary>
        public String APB_No { get; set; }
        /// <summary>
        /// 单据方向
        /// </summary>
        public String APB_BillDirectName { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String APB_SourceTypeName { get; set; }
        /// <summary>
        /// 来源单号
        /// </summary>
        public String APB_SourceBillNo { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String APB_Org_Name { get; set; }
        /// <summary>
        /// 收款对象类型编码
        /// </summary>
        public String APB_ReceiveObjectTypeCode { get; set; }
        /// <summary>
        /// 收款对象类型名称
        /// </summary>
        public String APB_ReceiveObjectTypeName { get; set; }
        /// <summary>
        /// 收款对象ID
        /// </summary>
        public String APB_ReceiveObjectID { get; set; }
        /// <summary>
        /// 收款对象名称
        /// </summary>
        public String APB_ReceiveObjectName { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public Decimal? APB_AccountPayableAmount { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
        public Decimal? APB_PaidAmount { get; set; }
        /// <summary>
        /// 未付金额
        /// </summary>
        public Decimal? APB_UnpaidAmount { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        public String APB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public String APB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 对账时间
        /// </summary>
        public DateTime? APB_ReconciliationTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String APB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String APB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APB_UpdatedTime { get; set; }
        /// <summary>
        /// 应付单ID
        /// </summary>
        public String APB_ID { get; set; }
        /// <summary>
        /// 单据方向编码
        /// </summary>
        public String APB_BillDirectCode { get; set; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public String APB_SourceTypeCode { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String APB_Org_ID { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String APB_BusinessStatusCode { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String APB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? APB_VersionNo { get; set; }

        #endregion

        #region 其他属性

        /// <summary>
        /// 临时ID
        /// </summary>
        public String Tmp_SID_ID { get; set; }
        #endregion
    }
}
