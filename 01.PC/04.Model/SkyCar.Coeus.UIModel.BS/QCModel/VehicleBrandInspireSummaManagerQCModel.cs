using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.BS
{
    /// <summary>
    /// 品牌车系QCModel
    /// </summary>
    public class VehicleBrandInspireSummaManagerQCModel : BaseQCModel
    {
        /// <summary>
        /// 品牌
        /// </summary>
        public String WHERE_VBIS_Brand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String WHERE_VBIS_Inspire { get; set; }
        /// <summary>
        /// 车型描述
        /// </summary>
        public String WHERE_VBIS_ModelDesc { get; set; }
        /// <summary>
        /// 车辆类型
        /// </summary>
        public String WHERE_VBIS_Model { get; set; }
    }
}
