using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.IS.QCModel
{
    /// <summary>
    /// 库存共享管理QCModel
    /// </summary>
    public class AutoPartsShareInventoryManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String WHERE_SI_WH_Name { get; set; }
       
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String WHERE_SI_ThirdNo { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String WHERE_SI_OEMNo { get; set; }
        /// <summary>
        /// 配件条码
        /// </summary>
        public String WHERE_SI_Barcode { get; set; }
        /// <summary>
        /// 配件批次号
        /// </summary>
        public String WHERE_SI_BatchNo { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_SI_Name { get; set; }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public String WHERE_SI_Org_ID { get; set; }
        /// <summary>
        /// 有效
        /// </summary>
        public Boolean? WHERE_SI_IsValid { get; set; }
    }
}
