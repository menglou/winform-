using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.SD.APModel
{
    /// <summary>
    /// 销售出库明细UIModel
    /// </summary>
    public class SalesStockOutDetailUIModel : BaseNotificationUIModel
    {
        #region 库存

        /// <summary>
        /// ID
        /// </summary>
        public String INV_ID { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String INV_SUPP_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String INV_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String INV_WHB_ID { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String INV_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String INV_OEMNo { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String INV_Barcode { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public String INV_BatchNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String INV_Name { get; set; }
        /// <summary>
        /// 配件规格型号
        /// </summary>
        public String INV_Specification { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        public Decimal? INV_Qty { get; set; }
        /// <summary>
        /// 采购单价
        /// </summary>
        public Decimal? INV_PurchaseUnitPrice { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? INV_VersionNo { get; set; }

        #endregion

        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WH_Name { get; set; }
        /// <summary>
        /// 仓位名称
        /// </summary>
        public String WHB_Name { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String SUPP_Name { get; set; }

        private Decimal? _stockOutQty;
        /// <summary>
        /// 出库数量
        /// </summary>
        public Decimal? StockOutQty
        {
            get { return _stockOutQty; }
            set
            {
                _stockOutQty = value;
                RaisePropertyChanged(() => StockOutQty);
            }
        }
        /// <summary>
        /// 销售单价
        /// </summary>
        public Decimal? SOBD_UnitSalePrice { get; set; }
    }
}
