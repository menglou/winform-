using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 付款单查询QCModel
    /// </summary>
    public class PayBillQueryQCModel : BaseQCModel
    {
        #region 公共属性-条件用
        /// <summary>
        /// 付款单号
        /// </summary>
        public String WHERE_PB_No { get; set; }
        ///// <summary>
        ///// 来源类型名称
        ///// </summary>
        //public String WHERE_PB_SourceTypeName { get; set; }
        ///// <summary>
        ///// 来源单据号
        ///// </summary>
        //public String WHERE_PB_SrcBillNo { get; set; }
        /// <summary>
        /// 付款组织ID
        /// </summary>
        public String WHERE_PB_Pay_Org_ID { get; set; }
        /// <summary>
        /// 付款组织名称
        /// </summary>
        public String WHERE_PB_Pay_Org_Name { get; set; }
        /// <summary>
        /// 付款日期
        /// </summary>
        public DateTime? WHERE_PB_Date { get; set; }
        /// <summary>
        /// 收款对象类型名称
        /// </summary>
        public String WHERE_PB_RecObjectTypeName { get; set; }
        /// <summary>
        /// 收款对象
        /// </summary>
        public String WHERE_PB_RecObjectName { get; set; }
        /// <summary>
        /// 付款方账号
        /// </summary>
        public String WHERE_PB_PayAccount { get; set; }
        /// <summary>
        /// 收款方账号
        /// </summary>
        public String WHERE_PB_RecAccount { get; set; }
        /// <summary>
        /// 付款方式名称
        /// </summary>
        public String WHERE_PB_PayTypeName { get; set; }
        /// <summary>
        /// 付款凭证编号
        /// </summary>
        public String WHERE_PB_CertificateNo { get; set; }
        /// <summary>
        /// 业务状态名称
        /// </summary>
        public String WHERE_PB_BusinessStatusName { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public String WHERE_PB_ApprovalStatusName { get; set; }
        #endregion
    }
}
