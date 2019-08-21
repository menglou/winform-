using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.UIModel
{
    /// <summary>
    /// 组织与组织资金往来统计汇总UIModel
    /// </summary>
    public class OrgAndOrgAmountTransTotalReportUIModel : BaseNotificationUIModel
    {
        /// <summary>
        /// 源组织ID
        /// </summary>
        public String SourOrgID { get; set; }
        /// <summary>
        /// 源组织名称
        /// </summary>
        public String SourOrgName { get; set; }
        /// <summary>
        /// 目的组织ID
        /// </summary>
        public String DestOrgID { get; set; }
        /// <summary>
        /// 目的组织名称
        /// </summary>
        public String DestOrgName { get; set; }

        #region 汇总
        /// <summary>
        /// 应收金额
        /// </summary>
        public Decimal? ARB_AccountReceivableAmount { get; set; }
        /// <summary>
        /// 已收金额
        /// </summary>
        public Decimal? ARB_ReceivedAmount { get; set; }

        /// <summary>
        /// 应付金额
        /// </summary>
        public Decimal? APB_AccountPayableAmount { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
        public Decimal? APB_PaidAmount { get; set; }

        /// <summary>
        /// 应收金额合计
        /// </summary>
        public Decimal? TotalAccountReceivableAmount { get; set; }

        /// <summary>
        /// 已收金额合计
        /// </summary>
        public Decimal? TotalReceivedAmount { get; set; }

        /// <summary>
        /// 应付金额合计
        /// </summary>
        public Decimal? TotalAccountPayableAmount { get; set; }
        
        /// <summary>
        /// 已付金额合计
        /// </summary>
        public Decimal? TotalPaidAmount { get; set; }
        #endregion

    }
}
