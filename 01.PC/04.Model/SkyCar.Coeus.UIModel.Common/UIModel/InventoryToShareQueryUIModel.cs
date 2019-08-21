using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 库存查询UIModel
    /// </summary>
    public class InventoryToShareQueryUIModel : BaseUIModel
    {
        #region 库存
        /// <summary>
        /// ID
        /// </summary>
        public String INV_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String INV_Org_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String INV_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String INV_WHB_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String INV_Org_FullName { get; set; }
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
        /// 配件批次号
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
        /// 供应商ID
        /// </summary>
        public String INV_SUPP_ID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? INV_Qty { get; set; }
        /// <summary>
        /// 采购单价
        /// </summary>
        public Decimal? INV_PurchaseUnitPrice { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? INV_IsValid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String INV_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? INV_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String INV_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? INV_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? INV_VersionNo { get; set; }

        #endregion

        #region 配件档案
        /// <summary>
        /// 配件名称
        /// </summary>
        public String APA_Name { get; set; }
        /// <summary>
        /// 车型代码
        /// </summary>
        public String APA_VehicleModelCode { get; set; }
        /// <summary>
        /// 互换码
        /// </summary>
        public String APA_ExchangeCode { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand { get; set; }

        #endregion

        /// <summary>
        /// 行ID
        /// </summary>
        public String RowID { get; set; }
        /// <summary>
        /// 选中
        /// </summary>
        public Boolean IsChecked { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public String WH_Name { get; set; }
        /// <summary>
        /// 仓位名称
        /// </summary>
        public String WHB_Name { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String SUPP_Name { get; set; }

    }
}
