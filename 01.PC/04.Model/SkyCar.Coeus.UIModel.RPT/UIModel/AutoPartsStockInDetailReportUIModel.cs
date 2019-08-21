using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.UIModel
{
    /// <summary>
    /// 配件入库统计明细UIModel
    /// </summary>
    public class AutoPartsStockInDetailReportUIModel : BaseNotificationUIModel
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
        public Decimal? TotalStockInQty { get; set; }
        /// <summary>
        /// 金额合计
        /// </summary>
        public Decimal? TotalStockInAmount { get; set; }

        #region 明细
        /// <summary>
        /// 入库单号
        /// </summary>
        public String SIB_No { get; set; }
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
        /// 入库数量
        /// </summary>
        public Decimal? SID_Qty { get; set; }
        /// <summary>
        /// 采购单价
        /// </summary>
        public Decimal? SID_UnitCostPrice { get; set; }
        /// <summary>
        /// 入库金额
        /// </summary>
        public Decimal? SID_Amount { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WH_Name { get; set; }
        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime? SID_CreatedTime { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public String SID_CreatedBy { get; set; }
        #endregion
    }
}
