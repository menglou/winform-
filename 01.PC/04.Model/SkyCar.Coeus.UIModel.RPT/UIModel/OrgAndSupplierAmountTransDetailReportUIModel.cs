using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.UIModel
{
    /// <summary>
    /// 组织与供应商资金往来统计明细UIModel
    /// </summary>
    public class OrgAndSupplierAmountTransDetailReportUIModel : BaseNotificationUIModel
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
        /// 客户ID
        /// </summary>
        public String SupplierID { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public String SupplierName { get; set; }
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
        /// 完整条码
        /// </summary>
        public String FullBarCode { get; set; }
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
        /// 入库单价
        /// </summary>
        public Decimal? PurchaseUnitPrice { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public Decimal? AutoPartsAmount { get; set; }
        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime? CreatedTime { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WH_ID { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WH_Name { get; set; }
        #endregion
    }
}
