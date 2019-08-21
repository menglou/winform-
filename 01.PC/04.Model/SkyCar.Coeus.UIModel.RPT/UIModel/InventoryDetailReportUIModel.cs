using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.RPT.UIModel
{
    /// <summary>
    /// 库存统计明细UIModel
    /// </summary>
    public class InventoryDetailReportUIModel : BaseNotificationUIModel
    {
        #region 库存

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
        /// 创建人
        /// </summary>
        public String INV_CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? INV_CreatedTime { get; set; }
        #endregion

        #region 配件档案
        
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String APA_Brand { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public String APA_UOM { get; set; }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String APA_Level { get; set; }
        /// <summary>
        /// 汽车品牌
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
        /// 变速类型编码
        /// </summary>
        public String APA_VehicleGearboxTypeCode { get; set; }
        /// <summary>
        /// 变速类型名称
        /// </summary>
        public String APA_VehicleGearboxTypeName { get; set; }
        /// <summary>
        /// 车型代码
        /// </summary>
        public String APA_VehicleModelCode { get; set; }
        /// <summary>
        /// 互换码
        /// </summary>
        public String APA_ExchangeCode { get; set; }
        #endregion

        #region 其他属性

        /// <summary>
        /// 组织简称
        /// </summary>
        public String Org_ShortName { get; set; }
        /// <summary>
        /// 仓库名称
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

        /// <summary>
        /// 库存金额
        /// </summary>
        public Decimal? InventoryAmount { get; set; }

        /// <summary>
        /// 数量合计
        /// </summary>
        public Decimal? TotalInventoryQty { get; set; }
        /// <summary>
        /// 金额合计
        /// </summary>
        public Decimal? TotalInventoryAmount { get; set; }

        #endregion
    }
}
