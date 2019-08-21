using System;
using SkyCar.Coeus.UIModel.Common;

namespace SkyCar.Coeus.UIModel.IS.UIModel
{
    /// <summary>
    /// 汽修组织车辆品牌车系查询UIModel
    /// </summary>
    public class AFOrgVehicleBrandInspireQueryUIModel : BaseUIModel
    {
        /// <summary>
        /// 商户编码
        /// </summary>
        public String MCT_Code { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public String MCT_Name { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public String VC_Org_ID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public String VC_Org_Name { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public String VC_Brand { get; set; }
        /// <summary>
        /// 车系
        /// </summary>
        public String VC_Inspire { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int VC_Count { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalVehicleCount { get; set; }

        public Boolean IsChecked { get; set; }
    }
}
