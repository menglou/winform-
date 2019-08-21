using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 领料查询QCModel
    /// </summary>
    public class PickPartsQueryQCModel : BaseQCModel
    {
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_INV_Name { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String WHERE_INV_Org_ID { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String WHERE_INV_SUPP_ID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WHERE_INV_WH_ID { get; set; }
        /// <summary>
        /// 仓位ID
        /// </summary>
        public String WHERE_INV_WHB_ID { get; set; }
        /// <summary>
        /// 创建时间-开始（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeStart { get; set; }
        /// <summary>
        /// 创建时间-终了（查询条件用）
        /// </summary>
        public DateTime? _CreatedTimeEnd { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public String WHERE_SUPP_Name { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public String WHERE_WH_Name { get; set; }
        /// <summary>
        /// 仓位名称
        /// </summary>
        public String WHERE_WHB_Name { get; set; }

        /// <summary>
        /// 配件条码（条形码+批次号）
        /// </summary>
        public String WHERE_AutoPartsBarcode { get; set; }
        /// <summary>
        /// 配件编码（原厂编码或第三方编码）
        /// </summary>
        public String WHERE_AutoPartsCode { get; set; }
        /// <summary>
        /// 其他描述（专有属性或适用范围中的任一项）
        /// </summary>
        public String WHERE_OtherDesc { get; set; }
        /// <summary>
        /// 车型代码
        /// </summary>
        public String WHERE_APA_VehicleModelCode { get; set; }
        /// <summary>
        /// 互换码
        /// </summary>
        public String WHERE_APA_ExchangeCode { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String WHERE_APA_Brand { get; set; }
        /// <summary>
        /// 车辆品牌
        /// </summary>
        public String WHERE_APA_VehicleBrand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String WHERE_APA_VehicleInspire { get; set; }
    }
}
