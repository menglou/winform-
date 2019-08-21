using System;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 库存查询QCModel
    /// </summary>
    public class InventoryToShareQueryQCModel : BaseQCModel
    {
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String WHERE_INV_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String WHERE_INV_OEMNo { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_INV_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String WHERE_INV_BatchNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_INV_Name { get; set; }
        /// <summary>
        /// 仓库id
        /// </summary>
        public String WHERE_INV_WH_Name { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String WHERE_INV_Org_ID { get; set; }

        /// <summary>
        /// 车型代码
        /// </summary>
        public String WHERE_APA_VehicleModelCode { get; set; }
        /// <summary>
        /// 互换码
        /// </summary>
        public String WHERE_APA_ExchangeCode { get; set; }
    }
}
