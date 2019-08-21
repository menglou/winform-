using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 付款单查询UIModel
    /// </summary>
    public class PayBillQueryUIModel : BaseUIModel
    {
        /// <summary>
        /// 行标识
        /// </summary>
        public string RowID { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; set; }

        #region 公共属性
        /// <summary>
        /// 付款ID
        /// </summary>
        public String PB_ID { get; set; }
        /// <summary>
        /// 付款单号
        /// </summary>
        public String PB_No { get; set; }
        /// <summary>
        /// 来源类型编码
        /// </summary>
        public String PB_SourceTypeCode { get; set; }
        ///// <summary>
        ///// 来源类型名称
        ///// </summary>
        //public String PB_SourceTypeName { get; set; }
        ///// <summary>
        ///// 来源单据号
        ///// </summary>
        //public String PB_SrcBillNo { get; set; }
        /// <summary>
        /// 付款组织ID
        /// </summary>
        public String PB_Pay_Org_ID { get; set; }
        /// <summary>
        /// 付款组织名称
        /// </summary>
        public String PB_Pay_Org_Name { get; set; }
        /// <summary>
        /// 付款日期
        /// </summary>
        public DateTime? PB_Date { get; set; }
        /// <summary>
        /// 收款对象类型编码
        /// </summary>
        public String PB_RecObjectTypeCode { get; set; }
        /// <summary>
        /// 收款对象类型名称
        /// </summary>
        public String PB_RecObjectTypeName { get; set; }
        /// <summary>
        /// 收款对象ID
        /// </summary>
        public String PB_RecObjectID { get; set; }
        /// <summary>
        /// 收款对象
        /// </summary>
        public String PB_RecObjectName { get; set; }
        /// <summary>
        /// 应付合计金额
        /// </summary>
        public Decimal? PB_PayableTotalAmount { get; set; }
        /// <summary>
        /// 实付合计金额
        /// </summary>
        public Decimal? PB_RealPayableTotalAmount { get; set; }
        /// <summary>
        /// 付款方账号
        /// </summary>
        public String PB_PayAccount { get; set; }
        /// <summary>
        /// 收款方账号
        /// </summary>
        public String PB_RecAccount { get; set; }
        /// <summary>
        /// 付款方式编码
        /// </summary>
        public String PB_PayTypeCode { get; set; }
        /// <summary>
        /// 付款方式名称
        /// </summary>
        public String PB_PayTypeName { get; set; }
        /// <summary>
        /// 付款凭证编号
        /// </summary>
        public String PB_CertificateNo { get; set; }
        /// <summary>
        /// 付款凭证图片
        /// </summary>
        public String PB_CertificatePic { get; set; }
        /// <summary>
        /// 业务状态编码
        /// </summary>
        public String PB_BusinessStatusCode { get; set; }
        /// <summary>
        /// 业务状态名称
        /// </summary>
        public String PB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态编码
        /// </summary>
        public String PB_ApprovalStatusCode { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String PB_ApprovalStatusName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String PB_Remark { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? PB_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String PB_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? PB_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String PB_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? PB_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? PB_VersionNo { get; set; }
        #endregion
    }
}
