using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.UIModel
{
    /// <summary>
    /// 组织与客户资金往来统计明细UIModel
    /// </summary>
    public class OrgAndCustomerAmountTransDetailReportUIModel : BaseNotificationUIModel
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
        /// <summary>
        /// 数量合计
        /// </summary>
        public Decimal? TotalAutoPartsQty { get; set; }
        /// <summary>
        /// 金额合计
        /// </summary>
        public Decimal? TotalAutoPartsAmount { get; set; }

        #region 明细
        /// <summary>
        /// 业务单号
        /// </summary>
        public String BusinessNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String APA_Name { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        public String APA_Barcode { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String APA_ThirdNo { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String APA_Specification { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? AutoPartsQty { get; set; }
        /// <summary>
        /// 销售单价
        /// </summary>
        public Decimal? SalesUnitPrice { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public Decimal? AutoPartsAmount { get; set; }
        /// <summary>
        /// 销售时间
        /// </summary>
        public DateTime? SO_CreatedTime { get; set; }
        /// <summary>
        /// 业务员ID
        /// </summary>
        public String SO_SalesByID { get; set; }
        /// <summary>
        /// 业务员名称
        /// </summary>
        public String SO_SalesByName { get; set; }
        #endregion
    }
}
