using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.UIModel
{
    /// <summary>
    /// 配件出库统计汇总UIModel
    /// </summary>
    public class AutoPartsStockOutTotalReportUIModel : BaseNotificationUIModel
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
        /// 库存异动类型
        /// </summary>
        public String InventoryTransType { get; set; }
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
        /// 应收金额合计
        /// </summary>
        public Decimal? TotalAccountReceivableAmount { get; set; }
        
        /// <summary>
        /// 已付金额合计
        /// </summary>
        public Decimal? TotalReceivedAmount { get; set; }
        #endregion
        
    }
}
