using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS
{
    /// <summary>
    /// 配件档案QCModel
    /// </summary>
    public class AutoPartsArchiveManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 条形码
        /// </summary>
        public String WHERE_APA_Barcode { get; set; }
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_APA_Name { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String WHERE_APA_Brand { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        public String WHERE_APA_Specification { get; set; }
        /// <summary>
        /// 配件级别
        /// </summary>
        public String WHERE_APA_Level { get; set; }
        /// <summary>
        /// 配件编码（原厂编码或第三方编码）
        /// </summary>
        public String WHERE_AutoPartsCode { get; set; }
        /// <summary>
        /// 车型代码
        /// </summary>
        public String WHERE_APA_VehicleModelCode { get; set; }
        /// <summary>
        /// 互换码
        /// </summary>
        public String WHERE_APA_ExchangeCode { get; set; }
        /// <summary>
        /// 默认供应商
        /// </summary>
        public String WHERE_SUPP_Name { get; set; }
    }
}
