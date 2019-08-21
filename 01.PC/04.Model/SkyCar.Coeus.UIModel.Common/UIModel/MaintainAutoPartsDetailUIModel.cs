using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 维护配件明细UIModel
    /// </summary>
    public class MaintainAutoPartsDetailUIModel : BaseUIModel
    {
        #region 配件档案

        /// <summary>
        /// 配件名称
        /// </summary>
        public String APA_Name { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String APA_OEMNo { get; set; }
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
        /// 计量单位
        /// </summary>
        public String APA_UOM { get; set; }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String APA_Level { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public String APA_VehicleBrand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String APA_VehicleInspire { get; set; }
        /// <summary>
        /// 排量
        /// </summary>
        public String APA_VehicleCapacity { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public String APA_VehicleYearModel { get; set; }
        /// <summary>
        /// 变速类型
        /// </summary>
        public String APA_VehicleGearboxTypeName { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        public String APA_Barcode { get; set; }

        /// <summary>
        /// 变速类型编码
        /// </summary>
        public String APA_VehicleGearboxTypeCode { get; set; }
        /// <summary>
        /// 车型代码
        /// </summary>
        public String APA_VehicleModelCode { get; set; }
        /// <summary>
        /// 互换码
        /// </summary>
        public String APA_ExchangeCode { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? APA_IsValid { get; set; }
        /// <summary>
        /// 配置档案ID
        /// </summary>
        public String APA_ID { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String APA_Org_ID { get; set; }
        /// <summary>
        /// 默认供应商ID
        /// </summary>
        public String APA_SUPP_ID { get; set; }
        /// <summary>
        /// 默认仓库ID
        /// </summary>
        public String APA_WH_ID { get; set; }
        /// <summary>
        /// 默认仓位ID
        /// </summary>
        public String APA_WHB_ID { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public String APA_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? APA_CreatedTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public String APA_UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? APA_UpdatedTime { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public Int64? APA_VersionNo { get; set; }
        #endregion

        #region 供应商

        /// <summary>
        /// 供应商ID
        /// </summary>
        public String SUPP_ID { get; set; }
        /// <summary>
        /// 供应商编码
        /// </summary>
        public String SUPP_Code { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public String SUPP_Name { get; set; }
        /// <summary>
        /// 供应商简称
        /// </summary>
        public String SUPP_ShortName { get; set; }
        #endregion

        #region 仓库

        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WH_ID { get; set; }
        /// <summary>
        /// 仓库编号
        /// </summary>
        public String WH_No { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WH_Name { get; set; }
        #endregion

        #region 仓位

        /// <summary>
        /// 仓位ID
        /// </summary>
        public String WHB_ID { get; set; }
        /// <summary>
        /// 仓位名称
        /// </summary>
        public String WHB_Name { get; set; }
        #endregion

        #region 配件名称

        /// <summary>
        /// 固定计量单位
        /// </summary>
        public Boolean? APN_FixUOM { get; set; }
        #endregion

        #region 其他

        /// <summary>
        /// 明细ID
        /// </summary>
        public String Detail_ID { get; set; }
        /// <summary>
        /// 上次入库时间
        /// </summary>
        public DateTime? LastStockInTime { get; set; }
        /// <summary>
        /// 采购单价
        /// </summary>
        public Decimal? PurchaseUnitPrice { get; set; }
        /// <summary>
        /// 采购数量
        /// </summary>
        public Decimal? PurchaseQuantity { get; set; }
        /// <summary>
        /// 入库金额
        /// </summary>
        public Decimal? StockInAmount { get; set; }

        /// <summary>
        /// 条码关键信息是否可编辑(签收后的预定明细，不可编辑条码)
        /// </summary>
        public Boolean? BarcodeKeyInfoEditable = true;
        #endregion
    }
}
