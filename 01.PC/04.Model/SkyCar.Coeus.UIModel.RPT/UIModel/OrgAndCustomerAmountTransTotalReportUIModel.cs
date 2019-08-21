using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.UIModel
{
    /// <summary>
    /// 组织与客户资金往来统计汇总UIModel
    /// </summary>
    public class OrgAndCustomerAmountTransTotalReportUIModel : BaseNotificationUIModel
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
        /// 客户类别
        /// </summary>
        public String CustomerTypeName { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public String CustomerID { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public String CustomerName { get; set; }

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
        /// 欠款金额
        /// </summary>
        public Decimal? ARB_UnReceiveAmount { get; set; }

        /// <summary>
        /// 应收金额合计
        /// </summary>
        public Decimal? TotalAccountReceivableAmount { get; set; }
        
        /// <summary>
        /// 已收金额合计
        /// </summary>
        public Decimal? TotalReceivedAmount { get; set; }

        /// <summary>
        /// 欠款金额合计
        /// </summary>
        public Decimal? TotalUnReceiveAmount { get; set; }
        #endregion

    }
}
