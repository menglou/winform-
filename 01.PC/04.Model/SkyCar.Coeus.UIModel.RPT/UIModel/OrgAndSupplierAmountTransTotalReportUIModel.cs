using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.UIModel
{
    /// <summary>
    /// 组织与供应商资金往来统计汇总UIModel
    /// </summary>
    public class OrgAndSupplierAmountTransTotalReportUIModel : BaseNotificationUIModel
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public String OrgID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String OrgShortName { get; set; }
        /// <summary>
        /// 业务类别
        /// </summary>
        public String BusinessType { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String SupplierID { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String SupplierName { get; set; }

        #region 汇总
        /// <summary>
        /// 应付金额
        /// </summary>
        public Decimal? APB_AccountPayableAmount { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
        public Decimal? APB_PaidAmount { get; set; }
        /// <summary>
        /// 欠款金额
        /// </summary>
        public Decimal? APB_UnpaidAmount { get; set; }

        /// <summary>
        /// 应付金额合计
        /// </summary>
        public Decimal? TotalAccountPayableAmount { get; set; }
        
        /// <summary>
        /// 已付金额合计
        /// </summary>
        public Decimal? TotalPaidAmount { get; set; }

        /// <summary>
        /// 欠款金额合计
        /// </summary>
        public Decimal? TotalUnpaidAmount { get; set; }
        #endregion

    }
}
