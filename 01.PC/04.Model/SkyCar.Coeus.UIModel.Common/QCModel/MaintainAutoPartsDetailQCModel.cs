using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.Common
{
    /// <summary>
    /// 维护配件明细QCModel
    /// </summary>
    public class MaintainAutoPartsDetailQCModel : BaseQCModel
    {
        /// <summary>
        /// 配件名称
        /// </summary>
        public String WHERE_APA_Name { get; set; }
        /// <summary>
        /// 原厂编码
        /// </summary>
        public String WHERE_APA_OEMNo { get; set; }
        /// <summary>
        /// 第三方编码
        /// </summary>
        public String WHERE_APA_ThirdNo { get; set; }
        /// <summary>
        /// 配件品牌
        /// </summary>
        public String WHERE_APA_Brand { get; set; }


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
        /// 供应商名称
        /// </summary>
        public String WHERE_Org_ID { get; set; }

    }
}
