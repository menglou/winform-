using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.IS.UIModel
{
    /// <summary>
    /// 汽修商库存UIModel
    /// </summary>
    public class ARInventoryUIModel : BaseUIModel
    {
        //汽修商户组织（只在“分组织”的场合显示），配件条码，第三方编码，配件名称，配件品牌，规格型号，当前数量，入库数量，出库数量，单位
        /// <summary>
        /// 汽修商户组织ID
        /// </summary>
        public String INV_Org_ID { get; set; }
        /// <summary>
        /// 汽修商户组织ID
        /// </summary>
        public String INV_Org_ShortName{ get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String INV_Barcode { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String INV_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String INV_OEMNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String APA_Name { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String APA_Specification { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public String APA_UOM { get; set; }
        /// <summary>
        /// 当前数量
        /// </summary>
        public Decimal? INV_Quantity { get; set; }
        /// <summary>
        /// 入库数量
        /// </summary>
        public Decimal? INV_StockInQuantity { get; set; }
        /// <summary>
        /// 出库数量
        /// </summary>
        public Decimal? INV_StockOutQuantity { get; set; }
        /// <summary>
        /// 总记录条数（翻页用）
        /// </summary>
        public Int32? RecordCount { get; set; }

        /// <summary>
        /// 自增列
        /// </summary>
        public Int32? RowIndex { get; set; }

        public Boolean IsChecked { get; set; }
    }
}
