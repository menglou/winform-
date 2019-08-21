using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.UIModel
{
    /// <summary>
    /// 配件出库统计明细UIModel
    /// </summary>
    public class AutoPartsStockOutDetailReportUIModel : BaseNotificationUIModel
    {
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
        /// 客户名称
        /// </summary>
        public String CustomerName { get; set; }
        /// <summary>
        /// 数量合计
        /// </summary>
        public Decimal? TotalStockOutQty { get; set; }
        /// <summary>
        /// 金额合计
        /// </summary>
        public Decimal? TotalStockOutAmount { get; set; }

        #region 明细
        /// <summary>
        /// 出库单号
        /// </summary>
        public String SOB_No { get; set; }
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
        /// 出库数量
        /// </summary>
        public Decimal? SOBD_Qty { get; set; }
        /// <summary>
        /// 销售单价
        /// </summary>
        public Decimal? SOBD_UnitCostPrice { get; set; }
        /// <summary>
        /// 出库金额
        /// </summary>
        public Decimal? SOBD_Amount { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WH_Name { get; set; }
        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime? SOBD_CreatedTime { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public String SOBD_CreatedBy { get; set; }
        #endregion
    }
}
